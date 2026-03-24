import { ChevronRight, CornerDownRight, Fingerprint, Shield, SquareTerminal } from "lucide-react";
import { type ReactNode } from "react";
import { buildCommandHash } from "../data/navigation";
import {
  NormalizedCommand,
  formatArity,
  formatOptionValue,
  getMetadataValue,
} from "../data/normalize";

interface CommandPanelProps {
  command: NormalizedCommand;
  includeMetadata: boolean;
  onCommandSelect: (path: string) => void;
}

export function CommandPanel({ command, includeMetadata, onCommandSelect }: CommandPanelProps) {
  const badges = [
    ...(command.command.interactive ? ["Interactive"] : []),
    ...(command.command.hidden ? ["Hidden"] : []),
    ...(command.command.aliases.length > 0 ? [`Aliases: ${command.command.aliases.join(", ")}`] : []),
  ];

  return (
    <>
      <section className="panel command-hero">
        <div className="breadcrumb-row">
          {command.path.split(" ").map((segment, index, all) => (
            <span key={`${segment}-${index}`} className="crumb">
              {index > 0 ? <ChevronRight aria-hidden="true" /> : null}
              {index === all.length - 1 ? <strong>{segment}</strong> : <span>{segment}</span>}
            </span>
          ))}
        </div>

        <div className="eyebrow">Command</div>
        <h1>{command.path}</h1>
        <p className="lede">{command.command.description ?? "No description available for this command."}</p>

        <div className="chip-row">
          {badges.map((badge) => (
            <span key={badge} className="info-chip">
              {badge}
            </span>
          ))}
          <span className="info-chip subtle">Deep link {buildCommandHash(command.path)}</span>
        </div>
      </section>

      <CommandGroup
        icon={<CornerDownRight aria-hidden="true" />}
        title="Subcommands"
        empty="No subcommands."
        items={command.commands.map((child) => ({
          key: child.path,
          title: child.command.name,
          body: child.command.description ?? "No description",
          footnote: child.commands.length > 0 ? `${child.commands.length} nested commands` : "Leaf command",
          action: () => onCommandSelect(child.path),
          actionLabel: "Open",
        }))}
      />

      <CommandGroup
        icon={<SquareTerminal aria-hidden="true" />}
        title="Arguments"
        empty="No arguments."
        items={command.arguments.map((argument) => ({
          key: argument.name,
          title: argument.name,
          body: argument.description ?? "No description",
          footnote: `${argument.required ? "Required" : "Optional"} · arity ${formatArity(argument)}${formatClrType(argument.metadata)}`,
        }))}
      />

      <CommandGroup
        icon={<Shield aria-hidden="true" />}
        title="Declared options"
        empty="No declared options."
        items={command.declaredOptions.map((option) => ({
          key: option.name,
          title: option.name,
          body: option.description ?? "No description",
          footnote: `${formatOptionValue(option)}${option.required ? " · required" : ""}${option.recursive ? " · recursive" : ""}${formatClrType(option.metadata)}`,
        }))}
      />

      <CommandGroup
        icon={<Fingerprint aria-hidden="true" />}
        title="Inherited options"
        empty="No inherited options."
        items={command.inheritedOptions.map((option) => ({
          key: option.option.name,
          title: option.option.name,
          body: option.option.description ?? "No description",
          footnote: `${formatOptionValue(option.option)} · inherited from ${option.inheritedFromPath}`,
        }))}
      />

      {command.command.examples.length > 0 ? (
        <section className="panel section-card">
          <div className="section-heading">
            <SquareTerminal aria-hidden="true" />
            <h2>Examples</h2>
          </div>
          <div className="example-stack">
            {command.command.examples.map((example) => (
              <pre key={example} className="example-block">
                <code>{example}</code>
              </pre>
            ))}
          </div>
        </section>
      ) : null}

      {command.command.exitCodes.length > 0 ? (
        <section className="panel section-card">
          <div className="section-heading">
            <Fingerprint aria-hidden="true" />
            <h2>Exit codes</h2>
          </div>
          <div className="detail-grid">
            {command.command.exitCodes.map((exitCode) => (
              <article key={exitCode.code} className="detail-card">
                <strong>{exitCode.code}</strong>
                <p>{exitCode.description ?? "No description"}</p>
              </article>
            ))}
          </div>
        </section>
      ) : null}

      {includeMetadata && command.command.metadata.length > 0 ? (
        <section className="panel section-card">
          <div className="section-heading">
            <Fingerprint aria-hidden="true" />
            <h2>Metadata</h2>
          </div>
          <div className="detail-grid">
            {command.command.metadata.map((item) => (
              <article key={item.name} className="detail-card">
                <strong>{item.name}</strong>
                <p>{typeof item.value === "string" ? item.value : JSON.stringify(item.value)}</p>
              </article>
            ))}
          </div>
        </section>
      ) : null}
    </>
  );
}

function CommandGroup({
  icon,
  title,
  empty,
  items,
}: {
  icon: ReactNode;
  title: string;
  empty: string;
  items: Array<{
    key: string;
    title: string;
    body: string;
    footnote: string;
    action?: () => void;
    actionLabel?: string;
  }>;
}) {
  return (
    <section className="panel section-card">
      <div className="section-heading">
        {icon}
        <h2>{title}</h2>
      </div>
      {items.length === 0 ? (
        <p className="muted">{empty}</p>
      ) : (
        <div className="detail-grid">
          {items.map((item) => (
            <article key={item.key} className="detail-card">
              <strong>{item.title}</strong>
              <p>{item.body}</p>
              <small>{item.footnote}</small>
              {item.action ? (
                <button type="button" className="link-button" onClick={item.action}>
                  {item.actionLabel ?? "Open"}
                </button>
              ) : null}
            </article>
          ))}
        </div>
      )}
    </section>
  );
}

function formatClrType(metadata: { name: string; value?: unknown }[]): string {
  const clrType = getMetadataValue(metadata, "ClrType");
  return clrType ? ` · ${clrType}` : "";
}
