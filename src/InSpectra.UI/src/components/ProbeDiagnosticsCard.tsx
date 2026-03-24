import { Search } from "lucide-react";
import { ProbeDiagnostics, describeDocumentSource } from "../data/toolProbe";

interface ProbeDiagnosticsCardProps {
  diagnostics: ProbeDiagnostics;
}

export function ProbeDiagnosticsCard({ diagnostics }: ProbeDiagnosticsCardProps) {
  const summary = diagnostics.summary;

  return (
    <section className="section-card panel">
      <div className="section-heading">
        <Search aria-hidden="true" />
        <h2>Last Probe Attempt</h2>
      </div>

      {summary ? (
        <>
          <div className="chip-row">
            <span className="info-chip">{summary.id}</span>
            <span className="info-chip subtle">{summary.version}</span>
            <span className="info-chip subtle">{describeDocumentSource(summary.documentSource)}</span>
            <span className="info-chip subtle">{diagnostics.confidence}</span>
            {summary.targetFramework ? <span className="info-chip subtle">{summary.targetFramework}</span> : null}
          </div>

          <div className="detail-grid source-summary-grid">
            <div className="detail-card">
              <strong>Command</strong>
              <p>{summary.commandName || "Unknown"}</p>
            </div>
            <div className="detail-card">
              <strong>Entry point</strong>
              <p>{summary.entryPoint || "Unknown"}</p>
            </div>
            <div className="detail-card">
              <strong>Spectre.Console.Cli</strong>
              <p>{summary.isSpectreCli ? "Detected" : "Not detected"}</p>
            </div>
            <div className="detail-card">
              <strong>Status</strong>
              <p>{diagnostics.status}</p>
            </div>
          </div>
        </>
      ) : null}

      {diagnostics.error ? (
        <p className="inline-alert" role="alert">
          {diagnostics.error}
        </p>
      ) : null}

      {diagnostics.warnings.length > 0 ? (
        <div className="probe-warning-list" role="status">
          {diagnostics.warnings.map((warning) => (
            <p key={warning}>{warning}</p>
          ))}
        </div>
      ) : null}
    </section>
  );
}
