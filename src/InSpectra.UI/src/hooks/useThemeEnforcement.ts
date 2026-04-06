import { useEffect } from "react";
import { FeatureFlags, ViewerOptions } from "../boot/contracts";

/** Applies preset theme settings from ViewerOptions and enforces feature-flag constraints. */
export function useThemeEnforcement(featureFlags: FeatureFlags, viewerOptions?: ViewerOptions) {
  // Apply preset theme/colorTheme/customAccent when viewerOptions arrives from bootstrap
  const presetTheme = viewerOptions?.theme;
  const presetColor = viewerOptions?.colorTheme;
  const customAccent = viewerOptions?.customAccent;
  const customAccentDark = viewerOptions?.customAccentDark;

  useEffect(() => {
    applyPresetTheme(presetTheme, presetColor, customAccent, customAccentDark);
  }, [presetTheme, presetColor, customAccent, customAccentDark]);

  // Enforce feature-flag constraints (forces light-only or dark-only)
  useEffect(() => {
    if (!featureFlags.darkTheme) {
      window.document.documentElement.dataset.theme = "light";
    } else if (!featureFlags.lightTheme) {
      window.document.documentElement.dataset.theme = "dark";
    }
    syncCustomAccent();
  }, [featureFlags.darkTheme, featureFlags.lightTheme]);
}

function applyPresetTheme(
  theme?: string,
  colorTheme?: string,
  customAccent?: string,
  customAccentDark?: string,
) {
  const root = document.documentElement;

  // Preset light/dark mode
  if (theme === "light" || theme === "dark") {
    root.dataset.theme = theme;
    localStorage.setItem("inspectra-theme", theme);
  }

  // Custom accent — overrides colorTheme
  if (customAccent) {
    root.dataset.colorTheme = "custom";
    root.dataset.customAccentLight = customAccent;
    root.dataset.customAccentDark = customAccentDark || customAccent;
    syncCustomAccent();
    return;
  }

  // Preset named color theme
  if (colorTheme) {
    if (colorTheme === "cyan") {
      delete root.dataset.colorTheme;
      localStorage.removeItem("inspectra-color-theme");
    } else {
      root.dataset.colorTheme = colorTheme;
      localStorage.setItem("inspectra-color-theme", colorTheme);
    }
  }
}

/**
 * Sets the inline --accent/--accent-hover CSS properties to match the
 * current light/dark mode when a custom accent is active.
 * Safe to call any time — no-ops if no custom accent is set.
 */
export function syncCustomAccent() {
  const root = document.documentElement;
  const light = root.dataset.customAccentLight;
  const dark = root.dataset.customAccentDark;
  if (!light) return;

  const isDark = root.dataset.theme === "dark";
  const color = isDark ? (dark || light) : light;
  root.style.setProperty("--accent", color);
  root.style.setProperty("--accent-hover", color);
}
