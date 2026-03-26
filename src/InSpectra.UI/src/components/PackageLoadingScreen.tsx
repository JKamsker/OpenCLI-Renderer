import { useEffect, useState } from "react";

interface PackageLoadingScreenProps {
  message?: string;
}

export function PackageLoadingScreen({ message }: PackageLoadingScreenProps) {
  const [phase, setPhase] = useState(0);

  useEffect(() => {
    const id = setInterval(() => setPhase((p) => (p + 1) % 4), 500);
    return () => clearInterval(id);
  }, []);

  const dots = ".".repeat(phase);

  return (
    <main className="pkg-loading-screen">
      <div className="pkg-loading-card">
        <div className="pkg-loading-terminal">
          <div className="pkg-loading-dots">
            <span />
            <span />
            <span />
          </div>
          <div className="pkg-loading-prompt">
            <span className="pkg-loading-caret">{">_"}</span>
            <span className="pkg-loading-cmd">
              fetch<span className="pkg-loading-flag"> --inspect</span>
            </span>
          </div>
          <div className="pkg-loading-bar-track">
            <div className="pkg-loading-bar-fill" />
          </div>
        </div>

        <div className="pkg-loading-info">
          <p className="pkg-loading-label">{message || "Loading"}{dots}</p>
          <p className="pkg-loading-sub">Resolving package from the discovery index</p>
        </div>
      </div>
    </main>
  );
}
