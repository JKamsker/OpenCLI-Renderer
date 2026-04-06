import { Moon, Sun, Palette } from "lucide-react";
import { useCallback, useEffect, useRef, useState } from "react";
import { syncCustomAccent } from "../hooks/useThemeEnforcement";

const COLOR_THEMES = [
  { id: "cyan", label: "Cyan", swatch: "#0891b2", swatchDark: "#22d3ee" },
  { id: "indigo", label: "Indigo", swatch: "#6366f1", swatchDark: "#818cf8" },
  { id: "emerald", label: "Emerald", swatch: "#059669", swatchDark: "#34d399" },
  { id: "amber", label: "Amber", swatch: "#d97706", swatchDark: "#fbbf24" },
  { id: "rose", label: "Rose", swatch: "#e11d48", swatchDark: "#fb7185" },
  { id: "blue", label: "Blue", swatch: "#3b82f6", swatchDark: "#60a5fa" },
] as const;

type ColorThemeId = (typeof COLOR_THEMES)[number]["id"];

function readTheme(): "light" | "dark" {
  return (document.documentElement.dataset.theme as "light" | "dark") || "light";
}

function readColorTheme(): string {
  return document.documentElement.dataset.colorTheme || "cyan";
}

interface ThemeToggleProps {
  colorThemePicker?: boolean;
}

export function ThemeToggle({ colorThemePicker = true }: ThemeToggleProps) {
  const [theme, setTheme] = useState<"light" | "dark">(readTheme);
  const [colorTheme, setColorTheme] = useState<string>(readColorTheme);
  const [dropdownOpen, setDropdownOpen] = useState(false);
  const longPressTimer = useRef<ReturnType<typeof setTimeout> | null>(null);
  const didLongPress = useRef(false);
  const wrapperRef = useRef<HTMLDivElement>(null);

  const isCustom = colorTheme === "custom";
  const canPickColor = colorThemePicker && !isCustom;

  // Toggle light/dark
  function toggleMode() {
    const next = theme === "dark" ? "light" : "dark";
    document.documentElement.dataset.theme = next;
    localStorage.setItem("inspectra-theme", next);
    setTheme(next);
    syncCustomAccent();
  }

  // Apply color theme
  function applyColorTheme(id: ColorThemeId) {
    if (id === "cyan") {
      delete document.documentElement.dataset.colorTheme;
      localStorage.removeItem("inspectra-color-theme");
    } else {
      document.documentElement.dataset.colorTheme = id;
      localStorage.setItem("inspectra-color-theme", id);
    }
    // Clear any custom accent inline styles when switching to a named theme
    document.documentElement.style.removeProperty("--accent");
    document.documentElement.style.removeProperty("--accent-hover");
    setColorTheme(id);
    setDropdownOpen(false);
  }

  // Long-press handlers — only if color picker is enabled
  function onPointerDown() {
    if (!canPickColor) return;
    didLongPress.current = false;
    longPressTimer.current = setTimeout(() => {
      didLongPress.current = true;
      setDropdownOpen((o) => !o);
    }, 500);
  }

  function onPointerUp() {
    if (longPressTimer.current) {
      clearTimeout(longPressTimer.current);
      longPressTimer.current = null;
    }
    if (!didLongPress.current) {
      toggleMode();
    }
  }

  function onPointerLeave() {
    if (longPressTimer.current) {
      clearTimeout(longPressTimer.current);
      longPressTimer.current = null;
    }
  }

  // Close dropdown on click outside
  const handleClickOutside = useCallback((e: MouseEvent) => {
    if (wrapperRef.current && !wrapperRef.current.contains(e.target as Node)) {
      setDropdownOpen(false);
    }
  }, []);

  useEffect(() => {
    if (dropdownOpen) {
      document.addEventListener("pointerdown", handleClickOutside);
      return () => document.removeEventListener("pointerdown", handleClickOutside);
    }
  }, [dropdownOpen, handleClickOutside]);

  // Close on Escape
  useEffect(() => {
    if (!dropdownOpen) return;
    function onKey(e: KeyboardEvent) {
      if (e.key === "Escape") setDropdownOpen(false);
    }
    document.addEventListener("keydown", onKey);
    return () => document.removeEventListener("keydown", onKey);
  }, [dropdownOpen]);

  const isDark = theme === "dark";
  const title = canPickColor
    ? "Click to toggle theme · Long-press for color themes"
    : "Toggle theme";

  return (
    <div className="theme-toggle-wrapper" ref={wrapperRef}>
      <button
        type="button"
        className="toolbar-button"
        onPointerDown={onPointerDown}
        onPointerUp={onPointerUp}
        onPointerLeave={onPointerLeave}
        onContextMenu={(e) => e.preventDefault()}
        title={title}
      >
        {isDark ? <Sun aria-hidden="true" /> : <Moon aria-hidden="true" />}
        <span>{isDark ? "Light" : "Dark"}</span>
      </button>

      {dropdownOpen && canPickColor && (
        <div className="theme-dropdown">
          <div className="theme-dropdown-header">
            <Palette aria-hidden="true" />
            <span>Color Theme</span>
          </div>
          <div className="theme-dropdown-list">
            {COLOR_THEMES.map((ct) => (
              <button
                key={ct.id}
                type="button"
                className={`theme-dropdown-item${colorTheme === ct.id ? " active" : ""}`}
                onClick={() => applyColorTheme(ct.id)}
              >
                <span
                  className="theme-swatch"
                  style={{ background: isDark ? ct.swatchDark : ct.swatch }}
                />
                <span className="theme-dropdown-label">{ct.label}</span>
                {colorTheme === ct.id && <span className="theme-dropdown-check">&#10003;</span>}
              </button>
            ))}
          </div>
        </div>
      )}
    </div>
  );
}
