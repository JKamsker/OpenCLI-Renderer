import react from "@vitejs/plugin-react";
import { defineConfig } from "vitest/config";

export default defineConfig({
  base: "./",
  plugins: [react()],
  build: {
    rollupOptions: {
      input: {
        main: "index.html",
        static: "static.html",
      },
    },
  },
  test: {
    environment: "jsdom",
    environmentOptions: {
      jsdom: {
        url: "https://example.test/viewer/index.html",
      },
    },
    setupFiles: "./vitest.setup.ts",
    css: true,
    globals: true,
  },
});
