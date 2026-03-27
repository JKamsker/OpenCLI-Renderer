import type { OpenCliArgument, OpenCliMetadata, OpenCliOption } from "./openCli";

export type InputKind = "text" | "number" | "boolean" | "select";

export interface InputDescriptor {
  kind: InputKind;
  selectOptions?: string[];
  nullable: boolean;
  isArray: boolean;
  typeLabel: string | null;
}

export function getClrType(metadata: OpenCliMetadata[]): string | undefined {
  const match = metadata.find((m) => m.name === "ClrType");
  return typeof match?.value === "string" ? match.value : undefined;
}

function unwrapNullable(clrType: string): { inner: string; isNullable: boolean } {
  const match = clrType.match(/^System\.Nullable<(.+)>$/);
  return match ? { inner: match[1], isNullable: true } : { inner: clrType, isNullable: false };
}

const INTEGER_TYPES = new Set([
  "System.Int32", "System.Int64", "System.Int16",
  "System.UInt32", "System.UInt64", "System.UInt16",
  "System.Byte",
]);

const FLOAT_TYPES = new Set([
  "System.Double", "System.Single", "System.Decimal",
]);

function typeLabel(inner: string, isArray: boolean): string | null {
  const base = isArray ? inner.replace(/\[\]$/, "") : inner;
  let label: string | null = null;
  if (INTEGER_TYPES.has(base)) label = "int";
  else if (FLOAT_TYPES.has(base)) label = "number";
  else if (base === "System.Boolean") label = "bool";
  if (label && isArray) label += "[]";
  return label;
}

export function resolveInputDescriptor(argument: OpenCliArgument): InputDescriptor {
  if (argument.acceptedValues.length > 0) {
    return {
      kind: "select",
      selectOptions: argument.acceptedValues,
      nullable: !argument.required,
      isArray: false,
      typeLabel: null,
    };
  }

  const clrType = getClrType(argument.metadata);
  if (!clrType) {
    return { kind: "text", nullable: !argument.required, isArray: false, typeLabel: null };
  }

  const { inner, isNullable } = unwrapNullable(clrType);
  const isArray = inner.endsWith("[]");
  const baseType = isArray ? inner.slice(0, -2) : inner;

  if (baseType === "System.Boolean") {
    return {
      kind: "text",
      nullable: isNullable || !argument.required,
      isArray: false,
      typeLabel: "bool",
    };
  }

  if (INTEGER_TYPES.has(baseType) || FLOAT_TYPES.has(baseType)) {
    return {
      kind: "number",
      nullable: isNullable || !argument.required,
      isArray,
      typeLabel: typeLabel(inner, isArray),
    };
  }

  return {
    kind: "text",
    nullable: !argument.required || isNullable,
    isArray,
    typeLabel: typeLabel(inner, isArray),
  };
}

export function validateValue(
  value: string,
  argument: OpenCliArgument,
  required: boolean,
): string | null {
  const trimmed = value.trim();

  if (required && !trimmed) {
    return "Required";
  }

  if (!trimmed) return null;

  if (argument.acceptedValues.length > 0) {
    if (!argument.acceptedValues.some((av) => av.toLowerCase() === trimmed.toLowerCase())) {
      return `Must be one of: ${argument.acceptedValues.join(", ")}`;
    }
    return null;
  }

  const clrType = getClrType(argument.metadata);
  if (!clrType) return null;

  const { inner } = unwrapNullable(clrType);
  const baseType = inner.endsWith("[]") ? inner.slice(0, -2) : inner;

  if (INTEGER_TYPES.has(baseType)) {
    if (!/^-?\d+$/.test(trimmed)) {
      return "Must be an integer";
    }
    const num = Number(trimmed);
    const range = getIntegerRange(baseType);
    if (range && (num < range.min || num > range.max)) {
      return `Must be between ${range.min} and ${range.max}`;
    }
  } else if (FLOAT_TYPES.has(baseType)) {
    if (isNaN(Number(trimmed))) {
      return "Must be a number";
    }
  } else if (baseType === "System.Boolean") {
    if (!["true", "false"].includes(trimmed.toLowerCase())) {
      return "Must be true or false";
    }
  }

  return null;
}

function getIntegerRange(type: string): { min: number; max: number } | null {
  switch (type) {
    case "System.Byte": return { min: 0, max: 255 };
    case "System.Int16": return { min: -32768, max: 32767 };
    case "System.UInt16": return { min: 0, max: 65535 };
    case "System.Int32": return { min: -2147483648, max: 2147483647 };
    case "System.UInt32": return { min: 0, max: 4294967295 };
    default: return null;
  }
}

export function getOptionInputDescriptor(option: OpenCliOption): InputDescriptor | null {
  if (option.arguments.length === 0) return null;
  return resolveInputDescriptor(option.arguments[0]);
}

export function isBooleanOption(option: OpenCliOption): boolean {
  if (option.arguments.length === 0) return false;
  const clrType = getClrType(option.arguments[0].metadata);
  if (!clrType) return false;
  const { inner } = unwrapNullable(clrType);
  return inner === "System.Boolean";
}

export function validateOptionValue(value: string, option: OpenCliOption): string | null {
  if (option.arguments.length === 0) return null;
  return validateValue(value, option.arguments[0], option.required);
}
