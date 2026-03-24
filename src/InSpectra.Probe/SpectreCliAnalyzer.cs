using System.Text.Json;
using InSpectra.Probe.Models;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace InSpectra.Probe;

internal sealed class SpectreCliAnalyzer
{
    private readonly SettingsModelFactory settingsFactory = new();

    public PackageProbeResult Analyze(ProbePackageContext context, PackageProbeResult result)
    {
        using var stream = new MemoryStream(context.EntryAssemblyBytes!, writable: false);
        var assembly = AssemblyDefinition.ReadAssembly(stream);
        result.Package!.IsSpectreCli = assembly.MainModule.AssemblyReferences.Any(reference =>
            reference.Name == "Spectre.Console.Cli");

        if (!result.Package.IsSpectreCli)
        {
            result.Error = "The entry assembly does not reference Spectre.Console.Cli.";
            return result;
        }

        var root = new MutableCommandNode { Name = context.CommandName ?? context.Id };
        var entryPoint = assembly.EntryPoint;
        if (entryPoint is null)
        {
            result.Error = "The tool entry point could not be resolved.";
            return result;
        }

        var reachableMethods = GetReachableMethods(entryPoint).ToList();
        ApplyDefaultRootCommand(reachableMethods, root);
        var configureMethods = reachableMethods
            .SelectMany(FindConfigureMethods)
            .Distinct(MethodReferenceComparer.Instance)
            .ToList();
        if (configureMethods.Count == 0 && root.Options.Count == 0 && root.Arguments.Count == 0)
        {
            result.Error = "The Spectre command graph could not be recovered from the entry assembly.";
            return result;
        }

        foreach (var method in configureMethods)
        {
            AnalyzeConfigurator(method, root);
        }

        var document = BuildDocument(context, root);
        result.Document = document;
        result.Status = "supported";
        result.Confidence = result.Warnings.Count == 0 ? "high" : "partial";
        if (document.Commands.Count == 0 && document.Options.Count == 0 && document.Arguments.Count == 0)
        {
            result.Status = "unsupported";
            result.Confidence = "unsupported";
            result.Error = "No Spectre commands or settings could be recovered from the entry assembly.";
            result.Document = null;
        }

        return result;
    }

    private void AnalyzeConfigurator(MethodDefinition method, MutableCommandNode parent)
    {
        MutableCommandNode? current = null;
        foreach (var instruction in method.Body.Instructions)
        {
            if (instruction.Operand is not MethodReference target)
            {
                continue;
            }

            switch (target.Name)
            {
                case "SetDescription":
                {
                    parent.Description ??= FindLastString(method, instruction);
                    break;
                }
                case "HideBranch":
                {
                    parent.Hidden = true;
                    break;
                }
                case "SetDefaultCommand":
                {
                    var commandType = (target as GenericInstanceMethod)?.GenericArguments.FirstOrDefault()?.Resolve();
                    ApplyCommandType(commandType, parent);
                    break;
                }
                case "AddCommand":
                {
                    var commandType = (target as GenericInstanceMethod)?.GenericArguments.FirstOrDefault()?.Resolve();
                    var node = new MutableCommandNode { Name = FindLastString(method, instruction) ?? "command" };
                    ApplyCommandType(commandType, node);
                    parent.Commands.Add(node);
                    current = node;
                    break;
                }
                case "AddBranch":
                {
                    var settingsType = (target as GenericInstanceMethod)?.GenericArguments.FirstOrDefault()?.Resolve();
                    var node = new MutableCommandNode { Name = FindLastString(method, instruction) ?? "branch" };
                    settingsFactory.Apply(settingsType, node);
                    var nested = FindDelegateMethod(method, instruction);
                    if (nested is not null)
                    {
                        AnalyzeConfigurator(nested, node);
                    }

                    parent.Commands.Add(node);
                    current = node;
                    break;
                }
                case "WithDescription":
                {
                    if (current is not null)
                    {
                        current.Description = FindLastString(method, instruction);
                    }

                    break;
                }
                case "WithAlias":
                {
                    var alias = FindLastString(method, instruction);
                    if (current is not null && !string.IsNullOrWhiteSpace(alias))
                    {
                        current.Aliases.Add(alias);
                    }

                    break;
                }
                case "IsHidden":
                {
                    if (current is not null)
                    {
                        current.Hidden = true;
                    }

                    break;
                }
            }
        }
    }

    private void ApplyCommandType(TypeDefinition? commandType, MutableCommandNode node)
    {
        if (commandType is null)
        {
            return;
        }

        node.CommandTypeName = commandType.FullName;
        node.Description ??= commandType.CustomAttributes
            .FirstOrDefault(attribute => attribute.AttributeType.FullName == "System.ComponentModel.DescriptionAttribute")
            ?.ConstructorArguments
            .FirstOrDefault()
            .Value as string;
        settingsFactory.Apply(ResolveSettingsType(commandType), node);
    }

    private void ApplyDefaultRootCommand(IEnumerable<MethodDefinition> methods, MutableCommandNode root)
    {
        foreach (var method in methods)
        {
            foreach (var instruction in method.Body.Instructions)
            {
                if (instruction.OpCode != OpCodes.Newobj || instruction.Operand is not MethodReference constructor)
                {
                    continue;
                }

                if (constructor.DeclaringType is GenericInstanceType genericType &&
                    genericType.Name.StartsWith("CommandApp`", StringComparison.Ordinal))
                {
                    ApplyCommandType(genericType.GenericArguments.FirstOrDefault()?.Resolve(), root);
                }
            }
        }
    }

    private static IEnumerable<MethodDefinition> FindConfigureMethods(MethodDefinition method)
    {
        var methods = new List<MethodDefinition>();
        foreach (var instruction in method.Body.Instructions)
        {
            if (instruction.Operand is MethodReference target &&
                target.Name == "Configure" &&
                FindDelegateMethod(method, instruction) is { } nested)
            {
                methods.Add(nested);
            }
        }

        return methods;
    }

    private static IEnumerable<MethodDefinition> GetReachableMethods(MethodDefinition root)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var queue = new Queue<MethodDefinition>();
        Enqueue(root);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            yield return current;

            var asyncMoveNext = ResolveAsyncStateMachine(current);
            if (asyncMoveNext is not null)
            {
                Enqueue(asyncMoveNext);
            }

            foreach (var instruction in current.Body.Instructions)
            {
                if (instruction.Operand is MethodReference reference &&
                    reference.Resolve() is { HasBody: true } resolved &&
                    resolved.Module == current.Module)
                {
                    Enqueue(resolved);
                }
            }
        }

        void Enqueue(MethodDefinition method)
        {
            if (seen.Add(method.FullName))
            {
                queue.Enqueue(method);
            }
        }
    }

    private static MethodDefinition? ResolveAsyncStateMachine(MethodDefinition method)
    {
        var attribute = method.CustomAttributes.FirstOrDefault(candidate =>
            candidate.AttributeType.FullName == "System.Runtime.CompilerServices.AsyncStateMachineAttribute");
        var stateMachine = attribute?.ConstructorArguments.FirstOrDefault().Value as TypeReference;
        return stateMachine?.Resolve()?.Methods.FirstOrDefault(candidate => candidate.Name == "MoveNext");
    }

    private static MethodDefinition? FindDelegateMethod(MethodDefinition method, Instruction anchor)
    {
        for (var current = anchor.Previous; current is not null; current = current.Previous)
        {
            if ((current.OpCode == OpCodes.Ldftn || current.OpCode == OpCodes.Ldvirtftn) &&
                current.Operand is MethodReference reference)
            {
                return reference.Resolve();
            }

            if (current.OpCode == OpCodes.Call || current.OpCode == OpCodes.Callvirt)
            {
                break;
            }
        }

        return null;
    }

    private static string? FindLastString(MethodDefinition method, Instruction anchor)
    {
        for (var current = anchor.Previous; current is not null; current = current.Previous)
        {
            if (current.OpCode == OpCodes.Ldstr)
            {
                return current.Operand as string;
            }

            if (current.OpCode == OpCodes.Call || current.OpCode == OpCodes.Callvirt)
            {
                break;
            }
        }

        return null;
    }

    private static TypeDefinition? ResolveSettingsType(TypeDefinition commandType)
    {
        for (TypeReference? current = commandType; current is not null; current = current.Resolve()?.BaseType)
        {
            if (current is GenericInstanceType generic &&
                (generic.ElementType.FullName == "Spectre.Console.Cli.Command`1" ||
                 generic.ElementType.FullName == "Spectre.Console.Cli.AsyncCommand`1"))
            {
                return generic.GenericArguments.FirstOrDefault()?.Resolve();
            }
        }

        return null;
    }

    private static OpenCliDocument BuildDocument(ProbePackageContext context, MutableCommandNode root)
    {
        return new OpenCliDocument
        {
            Info = new OpenCliInfo
            {
                Title = context.CommandName ?? context.Id,
                Version = context.Version,
                Summary = context.Description,
                Description = context.Description
            },
            Arguments = [.. root.Arguments],
            Options = [.. root.Options],
            Commands = root.Commands.Select(command => command.ToOpenCli()).ToList(),
            ExitCodes = [],
            Examples = [.. root.Examples],
            Interactive = false,
            Metadata =
            [
                new OpenCliMetadata { Name = "PackageId", Value = context.Id },
                new OpenCliMetadata { Name = "PackageVersion", Value = context.Version },
                new OpenCliMetadata { Name = "ProbeMode", Value = "static-spectre" },
                new OpenCliMetadata { Name = "EntryPoint", Value = context.EntryPoint ?? string.Empty }
            ]
        };
    }

    private sealed class MethodReferenceComparer : IEqualityComparer<MethodDefinition>
    {
        public static MethodReferenceComparer Instance { get; } = new();

        public bool Equals(MethodDefinition? x, MethodDefinition? y)
        {
            return StringComparer.Ordinal.Equals(x?.FullName, y?.FullName);
        }

        public int GetHashCode(MethodDefinition obj)
        {
            return StringComparer.Ordinal.GetHashCode(obj.FullName);
        }
    }
}
