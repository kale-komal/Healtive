const CODE_STOP_WORDS = new Set([
    "THE",
    "A",
    "AN",
    "BRANCH",
    "BRANCHES",
    "HOSPITAL",
]);

export const generateCodeFromName = (name, maxLength = 40) => {
    const value = String(name ?? "");
    const tokens = value
        .toUpperCase()
        .split(/[^A-Z0-9]+/)
        .filter(Boolean);

    if (tokens.length === 0) return "";

    let words = tokens.filter((token) => !CODE_STOP_WORDS.has(token));
    if (words.length === 0) words = tokens;

    let code = words.join("-");
    if (code.length > maxLength) code = code.slice(0, maxLength);

    return code.replace(/-+$/, "");
};

export const uniqueCode = (base, existingCodes) => {
    if (!base) return "";

    const used = new Set(
        (existingCodes || [])
            .map((code) => String(code ?? "").toUpperCase().trim())
            .filter(Boolean)
    );

    const normalized = base.toUpperCase();
    if (!used.has(normalized)) return normalized;

    let suffix = 2;
    while (used.has(`${normalized}-${suffix}`)) suffix += 1;

    return `${normalized}-${suffix}`;
};