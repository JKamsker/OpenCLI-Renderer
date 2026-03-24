import { OpenCliDocument } from "../data/openCli";

export const testDocument: OpenCliDocument = {
  opencli: "0.1-draft",
  info: {
    title: "demo",
    version: "1.2.3",
    summary: "A compact demo CLI.",
    description: "Inspect commands, options, and metadata.",
  },
  arguments: [
    {
      name: "TARGET",
      required: true,
      acceptedValues: [],
      description: "Target environment.",
      hidden: false,
      metadata: [{ name: "ClrType", value: "System.String" }],
    },
  ],
  options: [
    {
      name: "--profile",
      required: false,
      aliases: [],
      arguments: [
        {
          name: "NAME",
          required: true,
          acceptedValues: [],
          description: "Profile name.",
          hidden: false,
          metadata: [{ name: "ClrType", value: "System.String" }],
        },
      ],
      description: "Profile override.",
      recursive: true,
      hidden: false,
      metadata: [{ name: "Settings", value: "Demo.Profile" }],
    },
  ],
  commands: [
    {
      name: "alpha",
      aliases: ["a"],
      options: [
        {
          name: "--trace",
          required: false,
          aliases: [],
          arguments: [],
          description: "Trace nested execution.",
          recursive: true,
          hidden: false,
          metadata: [],
        },
      ],
      arguments: [
        {
          name: "INPUT",
          required: true,
          acceptedValues: [],
          description: "Input file.",
          hidden: false,
          metadata: [{ name: "ClrType", value: "System.String" }],
        },
      ],
      commands: [
        {
          name: "leaf",
          aliases: [],
          options: [],
          arguments: [],
          commands: [],
          exitCodes: [{ code: 0, description: "OK" }],
          description: "Leaf command.",
          hidden: false,
          examples: ["demo alpha leaf --trace"],
          interactive: false,
          metadata: [{ name: "ClrType", value: "Demo.LeafCommand" }],
        },
        {
          name: "shadow",
          aliases: [],
          options: [],
          arguments: [],
          commands: [],
          exitCodes: [],
          description: "Hidden child command.",
          hidden: true,
          examples: [],
          interactive: false,
          metadata: [],
        },
      ],
      exitCodes: [{ code: 0, description: "OK" }],
      description: undefined,
      hidden: false,
      examples: ["demo alpha INPUT"],
      interactive: true,
      metadata: [{ name: "ClrType", value: "Demo.AlphaCommand" }],
    },
    {
      name: "secret",
      aliases: [],
      options: [],
      arguments: [],
      commands: [],
      exitCodes: [],
      description: "Hidden top command.",
      hidden: true,
      examples: [],
      interactive: false,
      metadata: [],
    },
  ],
  exitCodes: [],
  examples: ["demo alpha sample.txt"],
  interactive: false,
  metadata: [{ name: "Assembly", value: "Demo.Cli" }],
};

export const testXmlDoc = `<?xml version="1.0" encoding="utf-8"?>
<Model>
  <Command Name="alpha">
    <Description>Filled from XML.</Description>
    <Parameters>
      <Option Long="trace">
        <Description>Trace nested execution.</Description>
      </Option>
      <Argument Name="INPUT">
        <Description>Input file from XML.</Description>
      </Argument>
    </Parameters>
    <Command Name="leaf">
      <Description>Leaf command from XML.</Description>
    </Command>
  </Command>
</Model>`;
