import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { InSpectraApp } from "./InSpectraApp";
import "./styles.css";

const element = document.getElementById("root");
if (!element) {
  throw new Error("Missing #root element.");
}

createRoot(element).render(
  <StrictMode>
    <InSpectraApp />
  </StrictMode>,
);
