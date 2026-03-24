using Mono.Cecil;
using InSpectra.Probe.Models;

namespace InSpectra.Probe;

internal sealed class SettingsModelFactory
{
    public void Apply(TypeDefinition? settingsType, MutableCommandNode command)
    {
        if (settingsType is null)
        {
            return;
        }

        command.SettingsTypeName = settingsType.FullName;

        foreach (var property in EnumerateProperties(settingsType))
        {
            var optionAttribute = property.CustomAttributes.FirstOrDefault(attribute => attribute.AttributeType.FullName == "Spectre.Console.Cli.CommandOptionAttribute");
            if (optionAttribute is not null)
            {
                var option = ParseOption(property, optionAttribute);
                if (option is not null)
                {
                    command.Options.Add(option);
                }
            }

            var argumentAttribute = property.CustomAttributes.FirstOrDefault(attribute => attribute.AttributeType.FullName == "Spectre.Console.Cli.CommandArgumentAttribute");
            if (argumentAttribute is not null)
            {
                var argument = ParseArgument(property, argumentAttribute);
                if (argument is not null)
                {
                    command.Arguments.Add(argument);
                }
            }
        }
    }

    private static IEnumerable<PropertyDefinition> EnumerateProperties(TypeDefinition settingsType)
    {
        var chain = new Stack<TypeDefinition>();
        for (var current = settingsType; current is not null; current = current.BaseType?.Resolve())
        {
            chain.Push(current);
            if (current.FullName == "Spectre.Console.Cli.CommandSettings")
            {
                break;
            }
        }

        while (chain.Count > 0)
        {
            foreach (var property in chain.Pop().Properties)
            {
                yield return property;
            }
        }
    }

    private static OpenCliOption? ParseOption(PropertyDefinition property, CustomAttribute attribute)
    {
        var template = attribute.ConstructorArguments.FirstOrDefault().Value as string;
        if (string.IsNullOrWhiteSpace(template))
        {
            return null;
        }

        var spec = OptionTemplate.Parse(template);
        var option = new OpenCliOption
        {
            Name = spec.Name,
            Required = false,
            Aliases = spec.Aliases,
            Description = ReadDescription(property),
            Recursive = false,
            Hidden = ReadBoolProperty(attribute, "IsHidden"),
            Metadata = BuildMetadata(property.PropertyType)
        };

        if (!string.IsNullOrWhiteSpace(spec.ValueName))
        {
            option.Arguments.Add(new OpenCliArgument
            {
                Name = spec.ValueName,
                Required = spec.ValueRequired,
                Arity = new OpenCliArity { Minimum = spec.ValueRequired ? 1 : 0, Maximum = 1 },
                AcceptedValues = [],
                Description = ReadDescription(property),
                Hidden = option.Hidden,
                Metadata = BuildMetadata(property.PropertyType)
            });
        }

        return option;
    }

    private static OpenCliArgument? ParseArgument(PropertyDefinition property, CustomAttribute attribute)
    {
        if (attribute.ConstructorArguments.Count < 2)
        {
            return null;
        }

        var template = attribute.ConstructorArguments[1].Value as string;
        if (string.IsNullOrWhiteSpace(template))
        {
            return null;
        }

        var spec = PositionalTemplate.Parse(template);
        return new OpenCliArgument
        {
            Name = spec.Name,
            Required = spec.Required,
            Arity = new OpenCliArity { Minimum = spec.Required ? 1 : 0, Maximum = 1 },
            AcceptedValues = [],
            Description = ReadDescription(property),
            Hidden = false,
            Metadata = BuildMetadata(property.PropertyType)
        };
    }

    private static List<OpenCliMetadata> BuildMetadata(TypeReference propertyType)
    {
        return [new OpenCliMetadata { Name = "ClrType", Value = propertyType.FullName }];
    }

    private static bool ReadBoolProperty(CustomAttribute attribute, string name)
    {
        return attribute.Properties.FirstOrDefault(property => property.Name == name).Argument.Value as bool? == true;
    }

    private static string? ReadDescription(ICustomAttributeProvider provider)
    {
        return provider.CustomAttributes
            .FirstOrDefault(attribute => attribute.AttributeType.FullName == "System.ComponentModel.DescriptionAttribute")
            ?.ConstructorArguments
            .FirstOrDefault()
            .Value as string;
    }
}
