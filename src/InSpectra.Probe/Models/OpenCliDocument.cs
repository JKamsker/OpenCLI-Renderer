namespace InSpectra.Probe.Models;

public sealed class OpenCliDocument
{
    public string Opencli { get; set; } = "0.1-draft";

    public OpenCliInfo Info { get; set; } = new();

    public List<OpenCliArgument> Arguments { get; set; } = [];

    public List<OpenCliOption> Options { get; set; } = [];

    public List<OpenCliCommand> Commands { get; set; } = [];

    public List<OpenCliExitCode> ExitCodes { get; set; } = [];

    public List<string> Examples { get; set; } = [];

    public bool Interactive { get; set; }

    public List<OpenCliMetadata> Metadata { get; set; } = [];
}

public sealed class OpenCliInfo
{
    public string Title { get; set; } = string.Empty;

    public string Version { get; set; } = string.Empty;

    public string? Summary { get; set; }

    public string? Description { get; set; }
}

public sealed class OpenCliCommand
{
    public string Name { get; set; } = string.Empty;

    public List<string> Aliases { get; set; } = [];

    public List<OpenCliOption> Options { get; set; } = [];

    public List<OpenCliArgument> Arguments { get; set; } = [];

    public List<OpenCliCommand> Commands { get; set; } = [];

    public List<OpenCliExitCode> ExitCodes { get; set; } = [];

    public string? Description { get; set; }

    public bool Hidden { get; set; }

    public List<string> Examples { get; set; } = [];

    public bool Interactive { get; set; }

    public List<OpenCliMetadata> Metadata { get; set; } = [];
}

public sealed class OpenCliOption
{
    public string Name { get; set; } = string.Empty;

    public bool Required { get; set; }

    public List<string> Aliases { get; set; } = [];

    public List<OpenCliArgument> Arguments { get; set; } = [];

    public string? Description { get; set; }

    public bool Recursive { get; set; }

    public bool Hidden { get; set; }

    public List<OpenCliMetadata> Metadata { get; set; } = [];
}

public sealed class OpenCliArgument
{
    public string Name { get; set; } = string.Empty;

    public bool Required { get; set; }

    public OpenCliArity? Arity { get; set; }

    public List<string> AcceptedValues { get; set; } = [];

    public string? Description { get; set; }

    public bool Hidden { get; set; }

    public List<OpenCliMetadata> Metadata { get; set; } = [];
}

public sealed class OpenCliArity
{
    public int? Minimum { get; set; }

    public int? Maximum { get; set; }
}

public sealed class OpenCliExitCode
{
    public int Code { get; set; }

    public string? Description { get; set; }
}

public sealed class OpenCliMetadata
{
    public string Name { get; set; } = string.Empty;

    public object? Value { get; set; }
}
