import { Check, Copy, Terminal } from "lucide-react";
import { useEffect, useRef, useState } from "react";
import { formatOptionValue, NormalizedCommand } from "../data/normalize";
import type { OpenCliArgument, OpenCliOption } from "../data/openCli";

interface ComposerPanelProps {
  command: NormalizedCommand | undefined;
  cliTitle: string;
  width: number;
  onResize: (width: number) => void;
  rootArguments?: OpenCliArgument[];
  rootOptions?: OpenCliOption[];
}

export function ComposerPanel({ command, cliTitle, width, onResize, rootArguments = [], rootOptions = [] }: ComposerPanelProps) {
  const [flagValues, setFlagValues] = useState<Record<string, boolean>>({});
  const [textValues, setTextValues] = useState<Record<string, string>>({});
  const [argValues, setArgValues] = useState<Record<string, string>>({});
  const [copied, setCopied] = useState(false);
  const resizeRef = useRef<HTMLDivElement>(null);

  const allOptions = command
    ? [
        ...command.declaredOptions,
        ...command.inheritedOptions.map((r) => r.option),
      ]
    : rootOptions;

  const allArguments = command ? command.arguments : rootArguments;

  const composerKey = command?.path ?? "__root__";

  useEffect(() => {
    setFlagValues({});
    setTextValues({});
    setArgValues({});
  }, [composerKey]);

  function buildPreview(): string {
    const parts = command
      ? [cliTitle, ...command.path.split(" ")]
      : [cliTitle];

    for (const arg of allArguments) {
      const value = argValues[arg.name];
      if (value?.trim()) {
        parts.push(value.includes(" ") ? `"${value}"` : value);
      }
    }

    const nameToDisplay = new Map(
      allOptions.map((opt) => [opt.name, opt.aliases?.[0] ?? opt.name]),
    );

    for (const [key, enabled] of Object.entries(flagValues)) {
      if (enabled) parts.push(nameToDisplay.get(key) ?? key);
    }

    for (const [key, value] of Object.entries(textValues)) {
      if (value.trim()) {
        parts.push(nameToDisplay.get(key) ?? key);
        parts.push(value.includes(" ") ? `"${value}"` : value);
      }
    }

    return parts.join(" ");
  }

  async function copyCommand() {
    try {
      await navigator.clipboard.writeText(buildPreview());
      setCopied(true);
      setTimeout(() => setCopied(false), 1500);
    } catch {
      /* clipboard unavailable */
    }
  }

  function handleResizeStart(e: React.MouseEvent) {
    e.preventDefault();
    const handle = resizeRef.current;
    if (handle) handle.classList.add("dragging");
    document.body.style.userSelect = "none";
    let latestWidth = width;

    function onMove(ev: MouseEvent) {
      latestWidth = Math.min(576, Math.max(224, document.documentElement.clientWidth - ev.clientX));
      onResize(latestWidth);
    }

    function onUp() {
      document.removeEventListener("mousemove", onMove);
      document.removeEventListener("mouseup", onUp);
      document.body.style.userSelect = "";
      if (handle) handle.classList.remove("dragging");
      localStorage.setItem("inspectra-composer-width", String(latestWidth));
    }

    document.addEventListener("mousemove", onMove);
    document.addEventListener("mouseup", onUp);
  }

  return (
    <aside className="composer" style={{ width }}>
      <div className="composer-resize" ref={resizeRef} onMouseDown={handleResizeStart} />
      <div className="composer-header">
        <Terminal width={16} height={16} />
        Composer
      </div>
      <div className="composer-body">
        {!command && allOptions.length === 0 && allArguments.length === 0 ? (
          <div className="composer-empty">Select a command to start composing.</div>
        ) : allOptions.length === 0 && allArguments.length === 0 ? (
          <div className="composer-empty">This command has no configurable options.</div>
        ) : (
          <>
            <div className="composer-section-title">
              {command ? <>Options for <code>{command.path}</code></> : "Root options"}
            </div>
            {allArguments.map((argument) => {
              const key = argument.name;
              const inputId = `composer-arg-${key.replace(/[^a-zA-Z0-9-]/g, "-")}`;
              return (
                <div className="composer-field" key={key}>
                  <label className="composer-opt-name" htmlFor={inputId}>{key}</label>
                  <input
                    id={inputId}
                    type="text"
                    placeholder={argument.acceptedValues.length > 0 ? argument.acceptedValues.join(" | ") : key}
                    value={argValues[key] ?? ""}
                    onChange={(e) =>
                      setArgValues((prev) => ({ ...prev, [key]: e.target.value }))
                    }
                    className="composer-input"
                  />
                  {argument.description && (
                    <span className="composer-opt-desc">{argument.description}</span>
                  )}
                </div>
              );
            })}
            {allOptions.map((option) => {
              const kind = formatOptionValue(option);
              const key = option.name;
              const displayName = option.aliases?.[0] ?? option.name;
              const inputId = `composer-opt-${key.replace(/[^a-zA-Z0-9-]/g, "-")}`;

              if (kind === "flag") {
                return (
                  <div className="composer-field" key={key}>
                    <label className="composer-flag">
                      <input
                        type="checkbox"
                        checked={flagValues[key] ?? false}
                        onChange={(e) =>
                          setFlagValues((prev) => ({ ...prev, [key]: e.target.checked }))
                        }
                      />
                      <div>
                        <span className="composer-opt-name">{displayName}</span>
                        {option.description && (
                          <span className="composer-opt-desc">{option.description}</span>
                        )}
                      </div>
                    </label>
                  </div>
                );
              }

              return (
                <div className="composer-field" key={key}>
                  <label className="composer-opt-name" htmlFor={inputId}>{displayName}</label>
                  <input
                    id={inputId}
                    type="text"
                    placeholder={kind}
                    value={textValues[key] ?? ""}
                    onChange={(e) =>
                      setTextValues((prev) => ({ ...prev, [key]: e.target.value }))
                    }
                    className="composer-input"
                  />
                  {option.description && (
                    <span className="composer-opt-desc">{option.description}</span>
                  )}
                </div>
              );
            })}
          </>
        )}
      </div>
      <div className="composer-footer">
        <span className="composer-label">Generated Command</span>
        <div className="composer-output-wrap">
          <pre className="composer-output">{buildPreview()}</pre>
          <button type="button" className="composer-copy" onClick={copyCommand} title="Copy command">
            {copied ? <Check /> : <Copy />}
          </button>
        </div>
      </div>
    </aside>
  );
}
