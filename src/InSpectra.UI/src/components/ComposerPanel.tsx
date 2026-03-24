import { Check, Copy, Terminal } from "lucide-react";
import { useEffect, useRef, useState } from "react";
import { formatOptionValue, NormalizedCommand } from "../data/normalize";

interface ComposerPanelProps {
  command: NormalizedCommand | undefined;
  cliTitle: string;
  width: number;
  onResize: (width: number) => void;
}

export function ComposerPanel({ command, cliTitle, width, onResize }: ComposerPanelProps) {
  const [flagValues, setFlagValues] = useState<Record<string, boolean>>({});
  const [textValues, setTextValues] = useState<Record<string, string>>({});
  const [copied, setCopied] = useState(false);
  const resizeRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    setFlagValues({});
    setTextValues({});
  }, [command?.path]);

  function buildPreview(): string {
    const parts = [cliTitle, ...(command?.path.split(" ") ?? [])];

    for (const [name, enabled] of Object.entries(flagValues)) {
      if (enabled) parts.push(name);
    }

    for (const [name, value] of Object.entries(textValues)) {
      if (value.trim()) {
        parts.push(name);
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

    function onMove(ev: MouseEvent) {
      const newWidth = Math.min(576, Math.max(224, document.documentElement.clientWidth - ev.clientX));
      onResize(newWidth);
    }

    function onUp() {
      document.removeEventListener("mousemove", onMove);
      document.removeEventListener("mouseup", onUp);
      document.body.style.userSelect = "";
      if (handle) handle.classList.remove("dragging");
      localStorage.setItem("inspectra-composer-width", String(width));
    }

    document.addEventListener("mousemove", onMove);
    document.addEventListener("mouseup", onUp);
  }

  const allOptions = [
    ...(command?.declaredOptions ?? []),
    ...(command?.inheritedOptions.map((r) => r.option) ?? []),
  ];

  return (
    <aside className="composer" style={{ width }}>
      <div className="composer-resize" ref={resizeRef} onMouseDown={handleResizeStart} />
      <div className="composer-header">
        <Terminal width={16} height={16} />
        Composer
      </div>
      <div className="composer-body">
        {!command ? (
          <div className="composer-empty">Select a command to start composing.</div>
        ) : allOptions.length === 0 ? (
          <div className="composer-empty">This command has no configurable options.</div>
        ) : (
          <>
            <div className="composer-section-title">
              Options for <code>{command.path}</code>
            </div>
            {allOptions.map((option) => {
              const kind = formatOptionValue(option);
              const optName = option.aliases?.[0] ?? option.name;

              if (kind === "flag") {
                return (
                  <div className="composer-field" key={optName}>
                    <label className="composer-flag">
                      <input
                        type="checkbox"
                        checked={flagValues[optName] ?? false}
                        onChange={(e) =>
                          setFlagValues((prev) => ({ ...prev, [optName]: e.target.checked }))
                        }
                      />
                      <div>
                        <span className="composer-opt-name">{optName}</span>
                        {option.description && (
                          <span className="composer-opt-desc">{option.description}</span>
                        )}
                      </div>
                    </label>
                  </div>
                );
              }

              return (
                <div className="composer-field" key={optName}>
                  <label className="composer-opt-name">{optName}</label>
                  <input
                    type="text"
                    placeholder={kind}
                    value={textValues[optName] ?? ""}
                    onChange={(e) =>
                      setTextValues((prev) => ({ ...prev, [optName]: e.target.value }))
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
