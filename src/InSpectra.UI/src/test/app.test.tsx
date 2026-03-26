import { fireEvent, render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { InSpectraApp } from "../InSpectraApp";
import { testDocument, testXmlDoc } from "./fixtures";

async function renderImportInput() {
  render(<InSpectraApp />);
  return await screen.findByLabelText("OpenCLI files");
}

describe("InSpectraUI app", () => {
  beforeEach(() => {
    document.body.innerHTML =
      '<div id="inspectra-root"></div><script id="inspectra-bootstrap" type="application/json">__INSPECTRA_BOOTSTRAP__</script>';
    window.history.replaceState({}, "", "https://example.test/viewer/index.html#/");
  });

  it("imports JSON only through the manual picker", async () => {
    const user = userEvent.setup();
    const input = await renderImportInput();
    await user.upload(
      input,
      new File([JSON.stringify(testDocument)], "opencli.json", { type: "application/json" }),
    );

    expect((await screen.findAllByText("demo")).length).toBeGreaterThan(0);
    expect((await screen.findAllByText("alpha")).length).toBeGreaterThan(0);
  });

  it("imports JSON and XML together", async () => {
    const user = userEvent.setup();
    const input = await renderImportInput();
    await user.upload(input, [
      new File([JSON.stringify(testDocument)], "opencli.json", { type: "application/json" }),
      new File([testXmlDoc], "xmldoc.xml", { type: "application/xml" }),
    ]);

    expect((await screen.findAllByText("Filled from XML.")).length).toBeGreaterThan(0);
  });

  it("applies inline bootstrap visibility options", async () => {
    document.getElementById("inspectra-bootstrap")!.textContent = JSON.stringify({
      mode: "inline",
      openCli: testDocument,
      options: { includeHidden: true, includeMetadata: true },
    });

    render(<InSpectraApp />);

    expect((await screen.findAllByText("alpha")).length).toBeGreaterThan(0);
    expect((await screen.findAllByText("secret")).length).toBeGreaterThan(0);
    expect(await screen.findByText("Assembly")).toBeInTheDocument();
  });

  it("shows a picker error when opencli.json is missing", async () => {
    const user = userEvent.setup();
    const input = await renderImportInput();
    await user.upload(input, new File([testXmlDoc], "xmldoc.xml", { type: "application/xml" }));

    expect(await screen.findByRole("alert")).toHaveTextContent("opencli.json is required.");
  });

  it("shows a picker error when more than two files are uploaded", async () => {
    const user = userEvent.setup();
    const input = await renderImportInput();
    await user.upload(input, [
      new File([JSON.stringify(testDocument)], "opencli.json", { type: "application/json" }),
      new File([testXmlDoc], "xmldoc.xml", { type: "application/xml" }),
      new File(["{}"], "extra.json", { type: "application/json" }),
    ]);

    expect(await screen.findByRole("alert")).toHaveTextContent(
      "Import accepts one or two files: opencli.json and optional xmldoc.xml.",
    );
  });

  it("shows a dropzone error for unsupported files", async () => {
    render(<InSpectraApp />);

    fireEvent.drop(await screen.findByRole("button", { name: "Import OpenCLI snapshot" }), {
      dataTransfer: {
        files: [new File(["oops"], "notes.txt", { type: "text/plain" })],
      },
    });

    await waitFor(() => {
      expect(screen.getByRole("alert")).toHaveTextContent('Unsupported file "notes.txt".');
    });
  });
});
