using System.Net;
using System.Text;
using System.Text.Json;
using OpenCli.Renderer.Models;
using OpenCli.Renderer.Runtime;

namespace OpenCli.Renderer.Services;

public sealed class HtmlRenderer : IDocumentRenderer
{
    public DocumentFormat Format => DocumentFormat.Html;

    public string RenderSingle(NormalizedCliDocument document, bool includeMetadata)
    {
        var content = new StringBuilder();
        AppendRootHero(document, content);
        AppendRootDetails(document, content, includeMetadata);

        if (document.Commands.Count > 0)
        {
            content.AppendLine("<section class=\"panel section\" id=\"commands\"><div class=\"section-head\"><span class=\"eyebrow\">Command Tree</span><h2>Commands</h2></div>");
            AppendSinglePageCommandSections(document.Commands, content, includeMetadata);
            content.AppendLine("</section>");
        }

        return RenderShell(document, "Overview", BuildSingleSidebar(document), content.ToString());
    }

    public IReadOnlyList<RelativeRenderedFile> RenderTree(NormalizedCliDocument document, bool includeMetadata)
    {
        var files = new List<RelativeRenderedFile>
        {
            new("index.html", RenderRootPage(document, includeMetadata)),
        };

        foreach (var command in document.Commands)
        {
            AppendCommandPages(document, command, includeMetadata, files);
        }

        return files;
    }

    private string RenderRootPage(NormalizedCliDocument document, bool includeMetadata)
    {
        var content = new StringBuilder();
        AppendRootHero(document, content);
        AppendRootDetails(document, content, includeMetadata);

        if (document.Commands.Count > 0)
        {
            content.AppendLine("<section class=\"panel section\"><div class=\"section-head\"><span class=\"eyebrow\">Browse</span><h2>Top-level commands</h2></div><div class=\"card-grid\">");
            foreach (var command in document.Commands)
            {
                content.AppendLine($"<a class=\"command-card\" href=\"{Encode(GetCommandRelativePath(command))}\"><strong>{Encode(command.Command.Name)}</strong><p>{EncodeOrFallback(command.Command.Description, "No description provided.")}</p></a>");
            }

            content.AppendLine("</div></section>");
        }

        return RenderShell(document, "Overview", BuildTreeSidebar(document, "index.html", null), content.ToString());
    }

    private void AppendCommandPages(
        NormalizedCliDocument document,
        NormalizedCommand command,
        bool includeMetadata,
        ICollection<RelativeRenderedFile> files)
    {
        var relativePath = GetCommandRelativePath(command);
        files.Add(new RelativeRenderedFile(relativePath, RenderCommandPage(document, command, relativePath, includeMetadata)));

        foreach (var child in command.Commands)
        {
            AppendCommandPages(document, child, includeMetadata, files);
        }
    }

    private string RenderCommandPage(
        NormalizedCliDocument document,
        NormalizedCommand command,
        string currentPagePath,
        bool includeMetadata)
    {
        var content = new StringBuilder();
        content.AppendLine("<section class=\"panel breadcrumb\">");
        content.AppendLine($"<a href=\"{Encode(CreateRelativeLink(currentPagePath, "index.html"))}\">{Encode(document.Source.Info.Title)}</a>");
        foreach (var segment in command.Path.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            content.AppendLine($"<span class=\"crumb-sep\">›</span><span>{Encode(segment)}</span>");
        }

        content.AppendLine("</section>");
        AppendCommandBody(
            command,
            content,
            includeMetadata,
            child => CreateRelativeLink(currentPagePath, GetCommandRelativePath(child)),
            includeWrapper: true);

        return RenderShell(document, command.Path, BuildTreeSidebar(document, currentPagePath, command.Path), content.ToString());
    }

    private static void AppendRootHero(NormalizedCliDocument document, StringBuilder builder)
    {
        builder.AppendLine("<section class=\"panel hero\" id=\"overview\">");
        builder.AppendLine("<span class=\"eyebrow\">OpenCLI Renderer</span>");
        builder.AppendLine($"<h1>{Encode(document.Source.Info.Title)}</h1>");
        builder.AppendLine("<div class=\"badge-row\">");
        builder.AppendLine($"<span class=\"badge badge-primary\">v{Encode(document.Source.Info.Version)}</span>");
        builder.AppendLine($"<span class=\"badge\">OpenCLI {Encode(document.Source.OpenCliVersion)}</span>");
        if (document.Source.Interactive)
        {
            builder.AppendLine("<span class=\"badge badge-success\">Interactive</span>");
        }

        builder.AppendLine("</div>");
        if (!string.IsNullOrWhiteSpace(document.Source.Info.Summary))
        {
            builder.AppendLine($"<p class=\"lede\">{Encode(document.Source.Info.Summary)}</p>");
        }

        builder.AppendLine(RenderParagraphBlock(document.Source.Info.Description));
        builder.AppendLine("</section>");
    }

    private static void AppendRootDetails(NormalizedCliDocument document, StringBuilder builder, bool includeMetadata)
    {
        AppendOverviewCards(document.Source, builder);

        if (document.RootArguments.Count > 0)
        {
            builder.AppendLine("<section class=\"panel section\" id=\"root-arguments\"><div class=\"section-head\"><span class=\"eyebrow\">Input</span><h2>Root arguments</h2></div>");
            AppendArgumentTable(document.RootArguments, builder);
            builder.AppendLine("</section>");
        }

        if (document.RootOptions.Count > 0)
        {
            builder.AppendLine("<section class=\"panel section\" id=\"root-options\"><div class=\"section-head\"><span class=\"eyebrow\">Flags</span><h2>Root options</h2></div>");
            AppendOptionCards(document.RootOptions.Select(option => new ResolvedOption { Option = option, IsInherited = false }), builder);
            builder.AppendLine("</section>");
        }

        if (document.Source.Examples.Count > 0)
        {
            builder.AppendLine("<section class=\"panel section\" id=\"root-examples\"><div class=\"section-head\"><span class=\"eyebrow\">Usage</span><h2>Examples</h2></div>");
            AppendExamples(document.Source.Examples, builder);
            builder.AppendLine("</section>");
        }

        if (document.Source.ExitCodes.Count > 0)
        {
            builder.AppendLine("<section class=\"panel section\" id=\"root-exit-codes\"><div class=\"section-head\"><span class=\"eyebrow\">Runtime</span><h2>Exit codes</h2></div>");
            AppendExitCodeTable(document.Source.ExitCodes, builder);
            builder.AppendLine("</section>");
        }

        if (includeMetadata && document.Source.Metadata.Count > 0)
        {
            builder.AppendLine("<section class=\"panel section\" id=\"root-metadata\"><div class=\"section-head\"><span class=\"eyebrow\">Appendix</span><h2>Metadata</h2></div>");
            AppendMetadataPanel(document.Source.Metadata, builder);
            builder.AppendLine("</section>");
        }
    }

    private static void AppendOverviewCards(OpenCliDocument document, StringBuilder builder)
    {
        var cards = new List<string>();
        if (document.Conventions is not null)
        {
            cards.Add($"<article class=\"panel info-card\"><h3>Conventions</h3><dl>{CreateDefinition("Group short options", document.Conventions.GroupOptions?.ToString() ?? "unspecified")}{CreateDefinition("Option separator", document.Conventions.OptionSeparator ?? "unspecified")}</dl></article>");
        }

        if (document.Info.Contact is not null)
        {
            cards.Add($"<article class=\"panel info-card\"><h3>Contact</h3><dl>{CreateDefinition("Name", document.Info.Contact.Name)}{CreateDefinition("Email", document.Info.Contact.Email)}{CreateLinkDefinition("URL", document.Info.Contact.Url)}</dl></article>");
        }

        if (document.Info.License is not null)
        {
            cards.Add($"<article class=\"panel info-card\"><h3>License</h3><dl>{CreateDefinition("Name", document.Info.License.Name)}{CreateDefinition("Identifier", document.Info.License.Identifier)}{CreateLinkDefinition("URL", document.Info.License.Url)}</dl></article>");
        }

        if (cards.Count == 0)
        {
            return;
        }

        builder.AppendLine("<section class=\"section\"><div class=\"section-head\"><span class=\"eyebrow\">Overview</span><h2>Reference context</h2></div><div class=\"info-grid\">");
        foreach (var card in cards)
        {
            builder.AppendLine(card);
        }

        builder.AppendLine("</div></section>");
    }

    private static void AppendSinglePageCommandSections(IEnumerable<NormalizedCommand> commands, StringBuilder builder, bool includeMetadata)
    {
        foreach (var command in commands)
        {
            AppendCommandBody(command, builder, includeMetadata, child => $"#command-{CreateAnchorId(child.Path)}", includeWrapper: true);
            AppendSinglePageCommandSections(command.Commands, builder, includeMetadata);
        }
    }

    private static void AppendCommandBody(
        NormalizedCommand command,
        StringBuilder builder,
        bool includeMetadata,
        Func<NormalizedCommand, string> childLinkFactory,
        bool includeWrapper)
    {
        if (includeWrapper)
        {
            builder.AppendLine($"<section class=\"panel command-detail\" id=\"command-{CreateAnchorId(command.Path)}\">");
        }

        builder.AppendLine($"<span class=\"command-path\">{Encode(command.Path)}</span>");
        builder.AppendLine($"<h2>{Encode(command.Command.Name)}</h2>");
        builder.AppendLine(RenderParagraphBlock(command.Command.Description, "No description provided."));

        var attributes = BuildCommandAttributes(command.Command);
        if (attributes.Count > 0)
        {
            builder.AppendLine("<div class=\"badge-row\">");
            foreach (var attribute in attributes)
            {
                builder.AppendLine($"<span class=\"badge\">{Encode(attribute)}</span>");
            }

            builder.AppendLine("</div>");
        }

        if (command.Commands.Count > 0)
        {
            builder.AppendLine("<div class=\"detail-block\"><h3>Subcommands</h3><div class=\"card-grid\">");
            foreach (var child in command.Commands)
            {
                builder.AppendLine($"<a class=\"command-card\" href=\"{Encode(childLinkFactory(child))}\"><strong>{Encode(child.Command.Name)}</strong><p>{EncodeOrFallback(child.Command.Description, "No description provided.")}</p></a>");
            }

            builder.AppendLine("</div></div>");
        }

        if (command.Arguments.Count > 0)
        {
            builder.AppendLine("<div class=\"detail-block\"><h3>Arguments</h3>");
            AppendArgumentTable(command.Arguments, builder);
            builder.AppendLine("</div>");
        }

        if (command.DeclaredOptions.Count > 0 || command.InheritedOptions.Count > 0)
        {
            builder.AppendLine("<div class=\"detail-block\"><h3>Options</h3>");
            AppendOptionCards(
                command.DeclaredOptions.Select(option => new ResolvedOption { Option = option, IsInherited = false }).Concat(command.InheritedOptions),
                builder);
            builder.AppendLine("</div>");
        }

        if (command.Command.Examples.Count > 0)
        {
            builder.AppendLine("<div class=\"detail-block\"><h3>Examples</h3>");
            AppendExamples(command.Command.Examples, builder);
            builder.AppendLine("</div>");
        }

        if (command.Command.ExitCodes.Count > 0)
        {
            builder.AppendLine("<div class=\"detail-block\"><h3>Exit codes</h3>");
            AppendExitCodeTable(command.Command.ExitCodes, builder);
            builder.AppendLine("</div>");
        }

        if (includeMetadata)
        {
            AppendCommandMetadata(command, builder);
        }

        if (includeWrapper)
        {
            builder.AppendLine("</section>");
        }
    }

    private static void AppendArgumentTable(IEnumerable<OpenCliArgument> arguments, StringBuilder builder)
    {
        builder.AppendLine("<div class=\"table-wrap\"><table><thead><tr><th>Name</th><th>Required</th><th>Arity</th><th>Accepted values</th><th>Group</th><th>Description</th></tr></thead><tbody>");
        foreach (var argument in arguments)
        {
            builder.AppendLine($"<tr><td><code>{Encode(argument.Hidden ? $"{argument.Name} (hidden)" : argument.Name)}</code></td><td>{Encode(argument.Required ? "Yes" : "No")}</td><td>{Encode(FormatArity(argument))}</td><td>{Encode(argument.AcceptedValues.Count == 0 ? "—" : string.Join(", ", argument.AcceptedValues))}</td><td>{Encode(argument.Group ?? "—")}</td><td>{Encode(argument.Description ?? "—")}</td></tr>");
        }

        builder.AppendLine("</tbody></table></div>");
    }

    private static void AppendOptionCards(IEnumerable<ResolvedOption> options, StringBuilder builder)
    {
        builder.AppendLine("<div class=\"option-list\">");
        foreach (var resolved in options)
        {
            var option = resolved.Option;
            builder.AppendLine("<article class=\"panel option-card\">");
            builder.AppendLine($"<div class=\"option-head\"><div><strong><code>{Encode(option.Hidden ? $"{option.Name} (hidden)" : option.Name)}</code></strong></div><div class=\"option-aliases\">{string.Join(" ", option.Aliases.Select(alias => $"<code>{Encode(alias)}</code>"))}</div></div>");
            builder.AppendLine("<div class=\"badge-row\">");
            builder.AppendLine($"<span class=\"badge badge-primary\">{Encode(FormatOptionValue(option))}</span>");
            if (option.Required)
            {
                builder.AppendLine("<span class=\"badge badge-danger\">Required</span>");
            }

            if (option.Recursive)
            {
                builder.AppendLine("<span class=\"badge badge-success\">Recursive</span>");
            }

            builder.AppendLine($"<span class=\"badge\">{Encode(resolved.IsInherited ? $"Inherited from {resolved.InheritedFromPath}" : "Declared")}</span>");
            if (!string.IsNullOrWhiteSpace(option.Group))
            {
                builder.AppendLine($"<span class=\"badge\">Group {Encode(option.Group)}</span>");
            }

            foreach (var argument in option.Arguments)
            {
                builder.AppendLine($"<span class=\"badge\">{Encode(argument.Name)} · {Encode(FormatArity(argument))}</span>");
                var clrType = TryGetClrType(argument.Metadata);
                if (clrType is not null)
                {
                    builder.AppendLine($"<span class=\"badge badge-warning\">{Encode(clrType)}</span>");
                }
            }

            builder.AppendLine("</div>");
            builder.AppendLine($"<p>{EncodeOrFallback(option.Description, "No description provided.")}</p>");
            builder.AppendLine("</article>");
        }

        builder.AppendLine("</div>");
    }

    private static void AppendExamples(IEnumerable<string> examples, StringBuilder builder)
    {
        builder.AppendLine("<div class=\"example-list\">");
        foreach (var example in examples)
        {
            builder.AppendLine($"<pre><code>$ {Encode(example)}</code></pre>");
        }

        builder.AppendLine("</div>");
    }

    private static void AppendExitCodeTable(IEnumerable<OpenCliExitCode> exitCodes, StringBuilder builder)
    {
        builder.AppendLine("<div class=\"table-wrap\"><table><thead><tr><th>Code</th><th>Description</th></tr></thead><tbody>");
        foreach (var exitCode in exitCodes)
        {
            builder.AppendLine($"<tr><td><code>{exitCode.Code}</code></td><td>{Encode(exitCode.Description ?? "—")}</td></tr>");
        }

        builder.AppendLine("</tbody></table></div>");
    }

    private static void AppendCommandMetadata(NormalizedCommand command, StringBuilder builder)
    {
        var hasMetadata = command.Command.Metadata.Count > 0 ||
                          command.Arguments.Any(argument => argument.Metadata.Count > 0) ||
                          command.DeclaredOptions.Any(option => option.Metadata.Count > 0) ||
                          command.InheritedOptions.Any(option => option.Option.Metadata.Count > 0);

        if (!hasMetadata)
        {
            return;
        }

        builder.AppendLine("<div class=\"detail-block\"><h3>Metadata appendix</h3>");
        if (command.Command.Metadata.Count > 0)
        {
            builder.AppendLine("<section class=\"panel metadata-panel\"><h4>Command</h4>");
            AppendMetadataPanel(command.Command.Metadata, builder);
            builder.AppendLine("</section>");
        }

        foreach (var argument in command.Arguments.Where(argument => argument.Metadata.Count > 0))
        {
            builder.AppendLine($"<section class=\"panel metadata-panel\"><h4>Argument <code>{Encode(argument.Name)}</code></h4>");
            AppendMetadataPanel(argument.Metadata, builder);
            builder.AppendLine("</section>");
        }

        foreach (var option in command.DeclaredOptions.Where(option => option.Metadata.Count > 0))
        {
            builder.AppendLine($"<section class=\"panel metadata-panel\"><h4>Option <code>{Encode(option.Name)}</code></h4>");
            AppendMetadataPanel(option.Metadata, builder);
            builder.AppendLine("</section>");
        }

        foreach (var option in command.InheritedOptions.Where(option => option.Option.Metadata.Count > 0))
        {
            builder.AppendLine($"<section class=\"panel metadata-panel\"><h4>Inherited <code>{Encode(option.Option.Name)}</code> from <code>{Encode(option.InheritedFromPath)}</code></h4>");
            AppendMetadataPanel(option.Option.Metadata, builder);
            builder.AppendLine("</section>");
        }

        builder.AppendLine("</div>");
    }

    private static void AppendMetadataPanel(IEnumerable<OpenCliMetadata> metadata, StringBuilder builder)
    {
        builder.AppendLine("<dl class=\"metadata-list\">");
        foreach (var item in metadata)
        {
            builder.AppendLine($"<div><dt>{Encode(item.Name)}</dt><dd>{FormatMetadataValue(item)}</dd></div>");
        }

        builder.AppendLine("</dl>");
    }

    private string BuildSingleSidebar(NormalizedCliDocument document)
    {
        var builder = new StringBuilder();
        builder.AppendLine("<nav class=\"sidebar-nav\"><a class=\"nav-link active\" href=\"#overview\">Overview</a>");
        if (document.RootArguments.Count > 0) builder.AppendLine("<a class=\"nav-link\" href=\"#root-arguments\">Root arguments</a>");
        if (document.RootOptions.Count > 0) builder.AppendLine("<a class=\"nav-link\" href=\"#root-options\">Root options</a>");
        if (document.Source.Examples.Count > 0) builder.AppendLine("<a class=\"nav-link\" href=\"#root-examples\">Examples</a>");
        if (document.Source.ExitCodes.Count > 0) builder.AppendLine("<a class=\"nav-link\" href=\"#root-exit-codes\">Exit codes</a>");
        if (document.Source.Metadata.Count > 0) builder.AppendLine("<a class=\"nav-link\" href=\"#root-metadata\">Metadata</a>");
        if (document.Commands.Count > 0)
        {
            builder.AppendLine("<div class=\"nav-label\">Commands</div><ul class=\"nav-tree\">");
            foreach (var command in document.Commands)
            {
                AppendSidebarNode(builder, command, $"#command-{CreateAnchorId(command.Path)}", null, currentPagePath: string.Empty, isSinglePage: true);
            }

            builder.AppendLine("</ul>");
        }

        builder.AppendLine("</nav>");
        return builder.ToString();
    }

    private string BuildTreeSidebar(NormalizedCliDocument document, string currentPagePath, string? currentPath)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"<nav class=\"sidebar-nav\"><a class=\"nav-link{(currentPath is null ? " active" : string.Empty)}\" href=\"{Encode(CreateRelativeLink(currentPagePath, "index.html"))}\">Overview</a>");
        if (document.Commands.Count > 0)
        {
            builder.AppendLine("<div class=\"nav-label\">Commands</div><ul class=\"nav-tree\">");
            foreach (var command in document.Commands)
            {
                AppendSidebarNode(builder, command, CreateRelativeLink(currentPagePath, GetCommandRelativePath(command)), currentPath, currentPagePath, isSinglePage: false);
            }

            builder.AppendLine("</ul>");
        }

        builder.AppendLine("</nav>");
        return builder.ToString();
    }

    private static void AppendSidebarNode(StringBuilder builder, NormalizedCommand command, string href, string? currentPath, string currentPagePath, bool isSinglePage)
    {
        builder.AppendLine($"<li class=\"nav-item\" data-nav-item data-label=\"{Encode(command.Path.ToLowerInvariant())}\"><a class=\"nav-link{(command.Path == currentPath ? " active" : string.Empty)}\" href=\"{Encode(href)}\">{Encode(command.Command.Name)}</a>");
        if (command.Commands.Count > 0)
        {
            builder.AppendLine("<ul class=\"nav-tree\">");
            foreach (var child in command.Commands)
            {
                var childHref = isSinglePage
                    ? $"#command-{CreateAnchorId(child.Path)}"
                    : CreateRelativeLink(currentPagePath, GetCommandRelativePath(child));
                AppendSidebarNode(builder, child, childHref, currentPath, currentPagePath, isSinglePage);
            }

            builder.AppendLine("</ul>");
        }

        builder.AppendLine("</li>");
    }

    private static string RenderShell(NormalizedCliDocument document, string pageTitle, string sidebar, string content)
    {
        return $$"""
            <!DOCTYPE html>
            <html lang="en">
            <head>
              <meta charset="utf-8" />
              <meta name="viewport" content="width=device-width, initial-scale=1" />
              <title>{{Encode(document.Source.Info.Title)}} · {{Encode(pageTitle)}}</title>
              <style>{{GetStyles()}}</style>
            </head>
            <body>
              <div class="shell">
                <header class="topbar">
                  <div class="brand"><span class="brand-mark">⌘</span><div><strong>{{Encode(document.Source.Info.Title)}}</strong><span>v{{Encode(document.Source.Info.Version)}} · OpenCLI {{Encode(document.Source.OpenCliVersion)}}</span></div></div>
                  <div class="topbar-context">{{Encode(pageTitle)}}</div>
                </header>
                <div class="layout">
                  <aside class="sidebar">
                    <label class="search"><span>Filter commands</span><input type="search" placeholder="Search command tree" data-nav-search /></label>
                    {{sidebar}}
                  </aside>
                  <main class="content"><div class="content-inner">{{content}}</div></main>
                </div>
              </div>
              <script>{{GetScript()}}</script>
            </body>
            </html>
            """;
    }

    private static string GetStyles()
    {
        return """
            :root{--bg:#f8fafc;--panel:#ffffff;--border:#e2e8f0;--text:#0f172a;--muted:#475569;--soft:#64748b;--blue:#2563eb;--blue-soft:#dbeafe;--green-soft:#dcfce7;--green:#166534;--red-soft:#fee2e2;--red:#b91c1c;--amber-soft:#fef3c7;--amber:#b45309;--shadow:0 18px 40px rgba(15,23,42,.08);font-family:Inter,"Segoe UI",system-ui,sans-serif}*{box-sizing:border-box}html{scroll-behavior:smooth}body{margin:0;background:linear-gradient(180deg,#f8fafc 0%,#eff6ff 100%);color:var(--text)}a{text-decoration:none;color:inherit}code,pre,input{font-family:"Cascadia Code",Consolas,monospace}.topbar{position:sticky;top:0;z-index:10;height:4rem;display:flex;justify-content:space-between;align-items:center;padding:0 1.25rem;border-bottom:1px solid var(--border);background:rgba(248,250,252,.92);backdrop-filter:blur(10px)}.brand{display:flex;align-items:center;gap:.8rem}.brand-mark{display:inline-flex;align-items:center;justify-content:center;width:2rem;height:2rem;border-radius:.7rem;background:var(--blue);color:#fff;font-weight:700;box-shadow:0 10px 20px rgba(37,99,235,.22)}.brand div{display:flex;flex-direction:column}.brand span,.topbar-context,.search span,.nav-label,dt,th{font-size:.76rem;color:var(--soft)}.topbar-context,.breadcrumb{padding:.45rem .8rem;border:1px solid var(--border);border-radius:999px;background:rgba(255,255,255,.78)}.layout{display:flex;min-height:calc(100vh - 4rem)}.sidebar{width:19rem;flex:0 0 19rem;padding:1rem;border-right:1px solid var(--border);background:rgba(248,250,252,.9);position:sticky;top:4rem;max-height:calc(100vh - 4rem);overflow:auto}.search{display:flex;flex-direction:column;gap:.45rem}.search input{width:100%;padding:.75rem .9rem;border:1px solid var(--border);border-radius:.8rem;background:#fff;outline:none}.search input:focus{border-color:rgba(37,99,235,.4);box-shadow:0 0 0 4px rgba(37,99,235,.12)}.content{flex:1;padding:2rem}.content-inner{max-width:72rem;margin:0 auto;display:flex;flex-direction:column;gap:1.35rem}.panel{background:rgba(255,255,255,.92);border:1px solid rgba(226,232,240,.9);border-radius:1.3rem;box-shadow:var(--shadow)}.hero,.section,.command-detail,.info-card,.metadata-panel{padding:1.4rem}.hero{background:radial-gradient(circle at top left,rgba(59,130,246,.12),transparent 42%),rgba(255,255,255,.94)}.hero h1,.command-detail h2,.section h2{margin:.2rem 0 0;font-size:clamp(1.8rem,3vw,2.7rem)}.eyebrow,.nav-label,dt,th{text-transform:uppercase;letter-spacing:.08em;font-weight:700}.eyebrow{color:var(--blue)}.lede,p,dd{color:var(--muted);line-height:1.7}.badge-row{display:flex;flex-wrap:wrap;gap:.55rem;margin-top:.9rem}.badge{display:inline-flex;padding:.35rem .65rem;border-radius:999px;border:1px solid #dbe2ea;background:#f1f5f9;color:var(--muted);font-size:.78rem;font-weight:600}.badge-primary{background:var(--blue-soft);border-color:#bfdbfe;color:#1d4ed8}.badge-success{background:var(--green-soft);border-color:#bbf7d0;color:var(--green)}.badge-danger{background:var(--red-soft);border-color:#fecaca;color:var(--red)}.badge-warning{background:var(--amber-soft);border-color:#fde68a;color:var(--amber)}.info-grid,.card-grid{display:grid;grid-template-columns:repeat(auto-fit,minmax(16rem,1fr));gap:1rem}.info-card h3,.detail-block h3,.metadata-panel h4{margin:0 0 .9rem}.info-card dl,.metadata-list{display:grid;gap:.8rem;margin:0}.info-card dd,.metadata-list dd{margin:.2rem 0 0}.section-head{margin-bottom:1rem}.table-wrap{overflow:auto;border:1px solid var(--border);border-radius:1rem;background:#fff}table{width:100%;border-collapse:collapse;min-width:42rem}th,td{padding:.9rem 1rem;border-bottom:1px solid var(--border);text-align:left;vertical-align:top}th{background:#f8fafc}tbody tr:last-child td{border-bottom:none}.option-list,.example-list{display:grid;gap:.9rem}.option-card p,.command-card p{margin:0}.option-head{display:flex;justify-content:space-between;gap:1rem;align-items:center;margin-bottom:.8rem}.option-aliases{display:flex;flex-wrap:wrap;gap:.45rem;color:var(--soft)}.command-card{display:flex;flex-direction:column;gap:.55rem;padding:1rem;border:1px solid var(--border);border-radius:1rem;background:linear-gradient(180deg,rgba(248,250,252,.92),rgba(255,255,255,.96));transition:transform .12s ease,border-color .12s ease,box-shadow .12s ease}.command-card:hover{transform:translateY(-1px);border-color:rgba(37,99,235,.35);box-shadow:0 16px 30px rgba(37,99,235,.08)}.command-card strong,.command-path{font-family:"Cascadia Code",Consolas,monospace;color:var(--blue)}.command-path{display:inline-flex;margin-bottom:.8rem;padding:.45rem .7rem;border-radius:.8rem;background:#0f172a;color:#e2e8f0}.detail-block+.detail-block{margin-top:1.25rem}pre{margin:0;padding:1rem 1.1rem;border-radius:1rem;background:#0f172a;color:#86efac;overflow:auto}.metadata-panel{margin-top:.8rem}.sidebar-nav{display:flex;flex-direction:column;gap:.25rem}.nav-tree{list-style:none;margin:0;padding:0 0 0 .75rem}.nav-link{display:block;padding:.55rem .75rem;border-radius:.8rem;color:var(--muted);font-size:.88rem}.nav-link:hover{background:rgba(37,99,235,.08);color:var(--text)}.nav-link.active{background:rgba(37,99,235,.12);color:#1d4ed8;font-weight:700}.empty{font-style:italic;color:var(--soft)}.breadcrumb{display:inline-flex;align-items:center;gap:.5rem}.crumb-sep{color:#94a3b8}@media (max-width:960px){.layout{flex-direction:column}.sidebar{position:static;width:auto;max-height:none;border-right:none;border-bottom:1px solid var(--border)}.content{padding:1.2rem}table{min-width:34rem}}
            """;
    }

    private static string GetScript()
    {
        return """
            (()=>{const search=document.querySelector('[data-nav-search]');if(!search)return;const roots=()=>Array.from(document.querySelectorAll('.nav-tree>.nav-item'));const visit=(item,query)=>{const label=(item.dataset.label||'').toLowerCase();const children=Array.from(item.querySelectorAll(':scope > .nav-tree > .nav-item'));const childMatch=children.map(child=>visit(child,query)).some(Boolean);const selfMatch=query===''||label.includes(query);item.hidden=!(selfMatch||childMatch);return selfMatch||childMatch;};search.addEventListener('input',()=>{const query=search.value.trim().toLowerCase();roots().forEach(item=>visit(item,query));});})();
            """;
    }

    private static string Encode(string? value) => WebUtility.HtmlEncode(value ?? string.Empty);

    private static string EncodeOrFallback(string? value, string fallback) => Encode(string.IsNullOrWhiteSpace(value) ? fallback : value);

    private static string RenderParagraphBlock(string? value, string fallback = "")
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.IsNullOrWhiteSpace(fallback) ? string.Empty : $"<p class=\"empty\">{Encode(fallback)}</p>";
        }

        return string.Join(string.Empty, value.Split(["\r\n\r\n", "\n\n"], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(paragraph => $"<p>{Encode(paragraph).Replace("\r", string.Empty, StringComparison.Ordinal).Replace("\n", "<br />", StringComparison.Ordinal)}</p>"));
    }

    private static List<string> BuildCommandAttributes(OpenCliCommand command)
    {
        var attributes = new List<string>();
        if (command.Aliases.Count > 0) attributes.Add($"Aliases: {string.Join(", ", command.Aliases)}");
        if (command.Interactive) attributes.Add("Interactive");
        if (command.Hidden) attributes.Add("Hidden");
        return attributes;
    }

    private static string CreateDefinition(string label, string? value) => string.IsNullOrWhiteSpace(value) ? string.Empty : $"<div><dt>{Encode(label)}</dt><dd>{Encode(value)}</dd></div>";

    private static string CreateLinkDefinition(string label, string? value) => string.IsNullOrWhiteSpace(value) ? string.Empty : $"<div><dt>{Encode(label)}</dt><dd><a href=\"{Encode(value)}\">{Encode(value)}</a></dd></div>";

    private static string FormatArity(OpenCliArgument argument)
    {
        var minimum = argument.Arity?.Minimum ?? (argument.Required ? 1 : 0);
        var maximum = argument.Arity?.Maximum;
        return maximum switch
        {
            null => $"{minimum}..n",
            _ when minimum == maximum => minimum.ToString(),
            _ => $"{minimum}..{maximum}",
        };
    }

    private static string FormatOptionValue(OpenCliOption option)
    {
        if (option.Arguments.Count == 0)
        {
            return "flag";
        }

        return string.Join(' ', option.Arguments.Select(argument => argument.Arity?.Maximum is null or > 1 ? $"<{argument.Name}...>" : $"<{argument.Name}>"));
    }

    private static string? TryGetClrType(IEnumerable<OpenCliMetadata> metadata)
    {
        var value = metadata.FirstOrDefault(item => string.Equals(item.Name, "ClrType", StringComparison.OrdinalIgnoreCase))?.Value;
        return value is not null && value.GetValueKind() == JsonValueKind.String ? value.GetValue<string>() : null;
    }

    private static string FormatMetadataValue(OpenCliMetadata metadata)
    {
        if (metadata.Value is null) return "<code>null</code>";
        return metadata.Value.GetValueKind() switch
        {
            JsonValueKind.String => $"<code>{Encode(metadata.Value.GetValue<string>())}</code>",
            JsonValueKind.Number or JsonValueKind.True or JsonValueKind.False => $"<code>{Encode(metadata.Value.ToJsonString())}</code>",
            _ => $"<pre><code>{Encode(metadata.Value.ToJsonString(new JsonSerializerOptions { WriteIndented = true }))}</code></pre>",
        };
    }

    private static string CreateAnchorId(string value)
    {
        var builder = new StringBuilder();
        foreach (var character in value.ToLowerInvariant())
        {
            if (char.IsLetterOrDigit(character)) builder.Append(character);
            else if (character is ' ' or '-' or '_') builder.Append('-');
        }

        return builder.ToString().Trim('-');
    }

    private static string GetCommandRelativePath(NormalizedCommand command)
    {
        var parts = command.Path.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(SanitizePathSegment).ToArray();
        if (command.Commands.Count > 0) return Path.Combine(parts).Replace('\\', '/') + "/index.html";
        var parent = parts.Length > 1 ? Path.Combine(parts[..^1]).Replace('\\', '/') : string.Empty;
        var fileName = parts[^1] + ".html";
        return string.IsNullOrEmpty(parent) ? fileName : $"{parent}/{fileName}";
    }

    private static string SanitizePathSegment(string value)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var sanitized = new string(value.Select(character => invalid.Contains(character) ? '-' : character).ToArray());
        return string.IsNullOrWhiteSpace(sanitized) ? "command" : sanitized;
    }

    private static string CreateRelativeLink(string currentPagePath, string targetPath)
    {
        var currentDirectory = Path.GetDirectoryName(currentPagePath)?.Replace('\\', '/');
        var baseDirectory = string.IsNullOrWhiteSpace(currentDirectory) ? "." : currentDirectory;
        return Path.GetRelativePath(baseDirectory, targetPath).Replace('\\', '/');
    }
}
