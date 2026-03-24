import { OpenCliDocument } from "../data/openCli";

export interface ViewerOptions {
  includeHidden: boolean;
  includeMetadata: boolean;
}

export type InSpectraBootstrap =
  | {
      mode: "inline";
      openCli: OpenCliDocument;
      xmlDoc?: string;
      options: ViewerOptions;
    }
  | {
      mode: "links";
      openCliUrl?: string;
      xmlDocUrl?: string;
      directoryUrl?: string;
      options?: Partial<ViewerOptions>;
    };

export function defaultViewerOptions(): ViewerOptions {
  return {
    includeHidden: false,
    includeMetadata: false,
  };
}
