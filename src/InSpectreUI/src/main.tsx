import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { InSpectreApp } from "./InSpectreApp";
import "./styles.css";

const element = document.getElementById("root");
if (!element) {
  throw new Error("Missing #root element.");
}

createRoot(element).render(
  <StrictMode>
    <InSpectreApp />
  </StrictMode>,
);
