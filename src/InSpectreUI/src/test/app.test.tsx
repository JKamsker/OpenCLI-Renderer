import { fireEvent, render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { InSpectreApp } from "../InSpectreApp";
import { testDocument, testXmlDoc } from "./fixtures";

describe("InSpectreUI app", () => {
  beforeEach(() => {
    document.body.innerHTML =
      '<div id="inspectre-root"></div><script id="inspectre-bootstrap" type="application/json">__INSPECTRE_BOOTSTRAP__</script>';
    window.history.replaceState({}, "", "https://example.test/viewer/index.html#/");
  });

  it("imports JSON only through the manual picker", async () => {
    const user = userEvent.setup();
    render(<InSpectreApp />);

    const input = screen.getByLabelText("OpenCLI files");
    await user.upload(
      input,
      new File([JSON.stringify(testDocument)], "opencli.json", { type: "application/json" }),
    );

    expect((await screen.findAllByText("demo")).length).toBeGreaterThan(0);
    expect((await screen.findAllByText("alpha")).length).toBeGreaterThan(0);
  });

  it("imports JSON and XML together", async () => {
    const user = userEvent.setup();
    render(<InSpectreApp />);

    const input = screen.getByLabelText("OpenCLI files");
    await user.upload(input, [
      new File([JSON.stringify(testDocument)], "opencli.json", { type: "application/json" }),
      new File([testXmlDoc], "xmldoc.xml", { type: "application/xml" }),
    ]);

    expect((await screen.findAllByText("Filled from XML.")).length).toBeGreaterThan(0);
  });

  it("toggles hidden items and metadata when bootstrapped inline", async () => {
    document.getElementById("inspectre-bootstrap")!.textContent = JSON.stringify({
      mode: "inline",
      openCli: testDocument,
      options: { includeHidden: false, includeMetadata: false },
    });

    const user = userEvent.setup();
    render(<InSpectreApp />);

    expect((await screen.findAllByText("alpha")).length).toBeGreaterThan(0);
    expect(screen.queryByText("secret")).not.toBeInTheDocument();
    expect(screen.queryByText("Assembly")).not.toBeInTheDocument();

    await user.click(screen.getByRole("button", { name: /show hidden/i }));
    expect((await screen.findAllByText("secret")).length).toBeGreaterThan(0);

    await user.click(screen.getByRole("button", { name: /show metadata/i }));
    expect(await screen.findByText("Assembly")).toBeInTheDocument();
  });

  it("shows a picker error when opencli.json is missing", async () => {
    const user = userEvent.setup();
    render(<InSpectreApp />);

    const input = screen.getByLabelText("OpenCLI files");
    await user.upload(input, new File([testXmlDoc], "xmldoc.xml", { type: "application/xml" }));

    expect(await screen.findByRole("alert")).toHaveTextContent("opencli.json is required.");
  });

  it("shows a picker error when more than two files are uploaded", async () => {
    const user = userEvent.setup();
    render(<InSpectreApp />);

    const input = screen.getByLabelText("OpenCLI files");
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
    render(<InSpectreApp />);

    fireEvent.drop(screen.getByRole("button", { name: "Import OpenCLI snapshot" }), {
      dataTransfer: {
        files: [new File(["oops"], "notes.txt", { type: "text/plain" })],
      },
    });

    await waitFor(() => {
      expect(screen.getByRole("alert")).toHaveTextContent('Unsupported file "notes.txt".');
    });
  });
});
