import { Search } from "lucide-react";
import { ProbePackageSummary, describeDocumentSource } from "../data/toolProbe";

interface SourceSummaryCardProps {
  summary: ProbePackageSummary;
}

export function SourceSummaryCard({ summary }: SourceSummaryCardProps) {
  return (
    <section className="section-card panel">
      <div className="section-heading">
        <Search aria-hidden="true" />
        <h2>NuGet Tool Probe</h2>
      </div>

      <div className="chip-row">
        <span className="info-chip">{summary.id}</span>
        <span className="info-chip subtle">{summary.version}</span>
        <span className="info-chip subtle">{describeDocumentSource(summary.documentSource)}</span>
        <span className="info-chip subtle">{summary.confidence}</span>
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
          <strong>Document source</strong>
          <p>{describeDocumentSource(summary.documentSource)}</p>
        </div>
        <div className="detail-card">
          <strong>Execution</strong>
          <p>No tool code executed. The package was downloaded and inspected in the browser.</p>
        </div>
      </div>

      <p className="source-summary-note">
        {summary.hasPackagedOpenCli
          ? "This package bundled an OpenCLI snapshot, so the viewer rendered the packaged document directly."
          : summary.isSpectreCli
            ? "This package did not bundle OpenCLI, so the viewer recovered a best-effort document from static Spectre.Console.Cli metadata."
            : "This package did not match the browser probe contract."}
      </p>
    </section>
  );
}
