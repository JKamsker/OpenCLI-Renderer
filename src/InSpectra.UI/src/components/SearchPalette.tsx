import { Search } from "lucide-react";
import { useEffect, useRef, useState } from "react";

export interface SearchPaletteItem {
  key: string;
  title: string;
  description?: string;
}

interface SearchPaletteProps {
  items: SearchPaletteItem[];
  open: boolean;
  onClose: () => void;
  onSelect: (key: string) => void;
  placeholder?: string;
  emptyText?: string;
  ariaLabel?: string;
}

export function SearchPalette({
  items, open, onClose, onSelect,
  placeholder = "Search…",
  emptyText = "No results found.",
  ariaLabel = "Search",
}: SearchPaletteProps) {
  const [query, setQuery] = useState("");
  const [activeIndex, setActiveIndex] = useState(0);
  const inputRef = useRef<HTMLInputElement>(null);
  const resultsRef = useRef<HTMLDivElement>(null);

  const filtered = query
    ? items.filter(
        (item) =>
          item.title.toLowerCase().includes(query.toLowerCase()) ||
          (item.description ?? "").toLowerCase().includes(query.toLowerCase()),
      )
    : items;

  useEffect(() => {
    if (open) {
      setQuery("");
      setActiveIndex(0);
      requestAnimationFrame(() => inputRef.current?.focus());
    }
  }, [open]);

  useEffect(() => { setActiveIndex(0); }, [query]);

  useEffect(() => {
    const active = resultsRef.current?.querySelector(".cmd-item.active");
    active?.scrollIntoView({ block: "nearest" });
  }, [activeIndex]);

  function handleKeyDown(e: React.KeyboardEvent) {
    if (e.key === "ArrowDown") {
      e.preventDefault();
      setActiveIndex((i) => (filtered.length === 0 ? 0 : Math.min(i + 1, filtered.length - 1)));
    } else if (e.key === "ArrowUp") {
      e.preventDefault();
      setActiveIndex((i) => Math.max(i - 1, 0));
    } else if (e.key === "Enter") {
      e.preventDefault();
      if (filtered[activeIndex]) {
        onSelect(filtered[activeIndex].key);
        onClose();
      }
    } else if (e.key === "Escape") {
      e.preventDefault();
      onClose();
    }
  }

  if (!open) return null;

  return (
    <div className="cmd-palette" onKeyDown={handleKeyDown}>
      <div className="cmd-backdrop" onClick={onClose} />
      <div className="cmd-dialog" role="dialog" aria-modal="true" aria-label={ariaLabel}>
        <div className="cmd-header">
          <Search width={16} height={16} />
          <input
            ref={inputRef}
            className="cmd-input"
            type="text"
            placeholder={placeholder}
            value={query}
            onChange={(e) => setQuery(e.target.value)}
          />
          <kbd>Esc</kbd>
        </div>
        <div className="cmd-results" ref={resultsRef}>
          {filtered.length === 0 ? (
            <div className="cmd-empty">{emptyText}</div>
          ) : (
            filtered.map((item, i) => (
              <button
                key={item.key}
                type="button"
                className={`cmd-item${i === activeIndex ? " active" : ""}`}
                onMouseEnter={() => setActiveIndex(i)}
                onClick={() => { onSelect(item.key); onClose(); }}
              >
                <span className="cmd-path">{highlightMatch(item.title, query)}</span>
                {item.description && (
                  <span className="cmd-desc">{highlightMatch(item.description, query)}</span>
                )}
              </button>
            ))
          )}
        </div>
        <div className="cmd-footer">
          <span><kbd>&uarr;</kbd><kbd>&darr;</kbd> Navigate</span>
          <span><kbd>&crarr;</kbd> Open</span>
          <span><kbd>Esc</kbd> Close</span>
        </div>
      </div>
    </div>
  );
}

function highlightMatch(text: string, query: string): React.ReactNode {
  if (!query) return text;
  const idx = text.toLowerCase().indexOf(query.toLowerCase());
  if (idx === -1) return text;
  return (
    <>
      {text.slice(0, idx)}
      <mark>{text.slice(idx, idx + query.length)}</mark>
      {text.slice(idx + query.length)}
    </>
  );
}
