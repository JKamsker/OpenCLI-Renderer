import { useEffect } from "react";
import { FeatureFlags } from "../boot/contracts";

/** Enforces theme based on feature flags — forces light-only or dark-only when one is disabled. */
export function useThemeEnforcement(featureFlags: FeatureFlags) {
  useEffect(() => {
    if (!featureFlags.darkTheme) {
      window.document.documentElement.dataset.theme = "light";
    } else if (!featureFlags.lightTheme) {
      window.document.documentElement.dataset.theme = "dark";
    }
  }, [featureFlags.darkTheme, featureFlags.lightTheme]);
}
