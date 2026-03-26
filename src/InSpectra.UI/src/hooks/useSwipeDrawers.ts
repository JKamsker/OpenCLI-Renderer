import { useEffect, useRef } from "react";

interface SwipeDrawersOptions {
  mobileSidebarOpen: boolean;
  composerOpen: boolean;
  setMobileSidebarOpen: (value: boolean) => void;
  setComposerOpen: (value: boolean) => void;
}

const EDGE_ZONE = 30;
const LOCK_DISTANCE = 10;
const COMMIT_RATIO = 0.35;
const VELOCITY_COMMIT = 0.4; // px/ms

type Mode = "open-nav" | "close-nav" | "open-composer" | "close-composer";

function clamp(v: number) {
  return Math.max(0, Math.min(1, v));
}

export function useSwipeDrawers(opts: SwipeDrawersOptions) {
  const ref = useRef(opts);
  ref.current = opts;

  useEffect(() => {
    let startX = 0;
    let startY = 0;
    let startTime = 0;
    let mode: Mode | null = null;
    let locked = false;
    let panel: HTMLElement | null = null;
    let panelWidth = 0;
    let overlay: HTMLElement | null = null;

    function isMobile() {
      return window.innerWidth <= 768;
    }

    function findPanel(side: "nav" | "composer"): HTMLElement | null {
      return document.querySelector(side === "nav" ? ".sidebar" : ".composer");
    }

    function createOverlay(): HTMLElement {
      const el = document.createElement("div");
      el.className = "swipe-gesture-overlay";
      const isDark = document.documentElement.dataset.theme === "dark";
      Object.assign(el.style, {
        position: "fixed",
        inset: "0",
        zIndex: "45",
        background: isDark ? "rgba(0, 0, 0, 0.5)" : "rgba(15, 23, 42, 0.35)",
        backdropFilter: "blur(4px)",
        WebkitBackdropFilter: "blur(4px)",
        opacity: "0",
        pointerEvents: "none",
        transition: "none",
      });
      document.body.appendChild(el);
      return el;
    }

    function hideReactOverlay() {
      const el = document.querySelector(".mobile-drawer-overlay") as HTMLElement;
      if (el) el.style.display = "none";
    }

    function restoreReactOverlay() {
      const el = document.querySelector(".mobile-drawer-overlay") as HTMLElement;
      if (el) el.style.display = "";
    }

    function getProgress(dx: number): number {
      switch (mode) {
        case "open-nav":
          return clamp(dx / panelWidth);
        case "close-nav":
          return clamp(1 + dx / panelWidth);
        case "open-composer":
          return clamp(-dx / panelWidth);
        case "close-composer":
          return clamp(1 - dx / panelWidth);
        default:
          return 0;
      }
    }

    function applyProgress(progress: number) {
      if (!panel || !mode) return;
      if (mode.includes("nav")) {
        panel.style.transform = `translateX(${(-1 + progress) * 100}%)`;
      } else {
        panel.style.transform = `translateX(${(1 - progress) * 100}%)`;
      }
      if (overlay) {
        overlay.style.opacity = String(progress);
        overlay.style.pointerEvents = progress > 0.01 ? "auto" : "none";
      }
    }

    function resolveMode(dx: number): Mode | null {
      const { mobileSidebarOpen, composerOpen } = ref.current;
      const right = dx > 0;
      const fromLeft = startX < EDGE_ZONE;
      const fromRight = startX > window.innerWidth - EDGE_ZONE;

      if (right) {
        if (composerOpen) return "close-composer";
        if (fromLeft && !mobileSidebarOpen) return "open-nav";
      } else {
        if (mobileSidebarOpen) return "close-nav";
        if (fromRight && !composerOpen) return "open-composer";
      }
      return null;
    }

    function initMode(m: Mode) {
      mode = m;
      const side = m.includes("nav") ? "nav" : "composer";
      panel = findPanel(side);
      if (panel) {
        panelWidth = panel.offsetWidth;
        panel.style.transition = "none";
      }
      overlay = createOverlay();
      hideReactOverlay();
    }

    function onTouchStart(e: TouchEvent) {
      if (!isMobile()) return;
      const t = e.touches[0];
      startX = t.clientX;
      startY = t.clientY;
      startTime = Date.now();
      mode = null;
      locked = false;
      panel = null;
      overlay = null;
    }

    function onTouchMove(e: TouchEvent) {
      if (mode === null && locked) return; // already rejected
      const t = e.touches[0];
      const dx = t.clientX - startX;
      const dy = t.clientY - startY;

      if (!locked) {
        if (Math.abs(dx) < LOCK_DISTANCE && Math.abs(dy) < LOCK_DISTANCE) return;
        locked = true;
        if (Math.abs(dy) > Math.abs(dx)) {
          mode = null;
          return;
        }
        const m = resolveMode(dx);
        if (!m) {
          mode = null;
          return;
        }
        initMode(m);
      }

      if (!mode || !panel) return;
      applyProgress(getProgress(dx));
    }

    function onTouchEnd(e: TouchEvent) {
      if (!mode || !panel) {
        if (overlay) {
          overlay.remove();
          overlay = null;
        }
        restoreReactOverlay();
        return;
      }

      const t = e.changedTouches[0];
      const dx = t.clientX - startX;
      const elapsed = Date.now() - startTime;
      const velocity = Math.abs(dx) / Math.max(elapsed, 1);
      const progress = getProgress(dx);

      const isOpening = mode === "open-nav" || mode === "open-composer";
      const shouldOpen = isOpening
        ? progress > COMMIT_RATIO || velocity > VELOCITY_COMMIT
        : progress > 1 - COMMIT_RATIO && velocity < VELOCITY_COMMIT;

      // Animate to final position
      panel.style.transition = "transform 0.2s cubic-bezier(0.4, 0, 0.2, 1)";
      if (overlay) {
        overlay.style.transition = "opacity 0.2s cubic-bezier(0.4, 0, 0.2, 1)";
      }
      applyProgress(shouldOpen ? 1 : 0);

      const capturedPanel = panel;
      const capturedOverlay = overlay;
      const capturedMode = mode;
      let finished = false;

      function finish() {
        if (finished) return;
        finished = true;

        // Update React state first
        if (capturedMode.includes("nav")) {
          ref.current.setMobileSidebarOpen(shouldOpen);
        } else {
          ref.current.setComposerOpen(shouldOpen);
        }

        // Wait for React to re-render and apply CSS classes, then clear inline styles
        requestAnimationFrame(() => {
          requestAnimationFrame(() => {
            capturedPanel.style.transition = "";
            capturedPanel.style.transform = "";
            if (capturedOverlay) capturedOverlay.remove();
            restoreReactOverlay();
          });
        });
      }

      capturedPanel.addEventListener("transitionend", finish, { once: true });
      setTimeout(finish, 250);

      mode = null;
      panel = null;
      overlay = null;
    }

    window.addEventListener("touchstart", onTouchStart, { passive: true });
    window.addEventListener("touchmove", onTouchMove, { passive: true });
    window.addEventListener("touchend", onTouchEnd, { passive: true });

    return () => {
      window.removeEventListener("touchstart", onTouchStart);
      window.removeEventListener("touchmove", onTouchMove);
      window.removeEventListener("touchend", onTouchEnd);
      if (overlay) overlay.remove();
    };
  }, []);
}
