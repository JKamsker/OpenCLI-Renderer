import { Check, Copy } from "lucide-react";
import { useState } from "react";

export function CopyButton({ text, className = "example-copy", title = "Copy" }: { text: string; className?: string; title?: string }) {
  const [copied, setCopied] = useState(false);

  async function copy() {
    try {
      await navigator.clipboard.writeText(text);
      setCopied(true);
      setTimeout(() => setCopied(false), 1500);
    } catch {
      /* clipboard unavailable */
    }
  }

  return (
    <button type="button" className={className} onClick={copy} title={title}>
      {copied ? <Check /> : <Copy />}
    </button>
  );
}
