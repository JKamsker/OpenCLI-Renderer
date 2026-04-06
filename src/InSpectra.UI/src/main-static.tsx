import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { StaticViewerApp } from "./StaticViewerApp";
import "./styles-static.css";

const element = document.getElementById("root");
if (!element) {
  throw new Error("Missing #root element.");
}

createRoot(element).render(
  <StrictMode>
    <StaticViewerApp />
  </StrictMode>,
);
