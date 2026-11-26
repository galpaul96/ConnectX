# Spell catalog component contract

Each of the 470 JSON records has the same identity envelope (`id`, `slug`, `name`,
`storageFile`) and an ordered `components` array. `_summary.json` is the original
crawl summary, not a spell; it is preserved as historical metadata and its totals
are not the current catalog size.

```json
{
  "type": "components",
  "label": "Components",
  "value": {
    "text": "V, S, M",
    "verbal": true,
    "somatic": true,
    "material": {
      "required": true,
      "description": "a diamond worth 300+ GP, which the spell consumes",
      "consumed": true,
      "costGp": 300,
      "costIsMinimum": true
    }
  },
  "html": "<p>V, S, M (a diamond worth 300+ GP, which the spell consumes)</p>"
}
```

- `type` is the semantic role, not a renderer or JSON data type. The role
  `components` specifically means the spell's V/S/M casting requirements.
- `label` is the UI heading. `value` holds a string/scalar, array, or nested JSON
  object. New roles accept any JSON value without changing the schema or UI.
- `html` is required for every component, including metadata. Nonempty values
  have nonempty HTML. Empty fields may use `""`. Treat HTML as untrusted and render
  through the app sanitizer.
- Known structured roles retain their existing value shapes: `level`,
  `castingTime`, `rangeArea`, `components`, `duration`, `attackSave`, `damageEffect`.
  Source wording remains authoritative when a value is ambiguous. `costGp` is
  the quoted requirement; `costIsMinimum` records explicit minimum wording.
  Null means unknown/unparsed, not false or zero.
- `description.value` contains all text blocks in reading order, including table
  rows, list items, headings, scaling rules, stat blocks and footnotes.
  `description.html` preserves their formatting. Arrays retain ordered content;
  spells do not need empty components for every conceivable mechanic.
- `bookMetadata` retains the handbook classification, class list, and complete
  casting requirements. `sourceFiles` identifies supplied local source paths
  relative to `src/`. `source` retains the previous bibliographic attribution.
- `descriptionVariant` retains source descriptions that differ from the primary
  text. Each entry contains `sourceFile`, `text`, and `html`. The primary
  description keeps previous details; conflicting versions are never combined
  into one set of rules. Some differences are artwork captions or references.
- The server accepts old `key` records for compatibility and emits `type` in
  components and catalog component definitions. Existing layout preferences
  remain valid because the role names did not change.

`../spell-catalog.schema.json` validates records and supports extension roles.
The generic UI displays unknown roles; no list of spell effects is hardcoded.

## Offline migration and verification

From `src/`:

```sh
npm run migrate:spells
npm run check:spell-migration
npm run test:spell-data
npm run test:spells
```

Migration uses Python 3 standard libraries. The data tests additionally require
Python's `jsonschema` package; server tests use Node's built-in test runner.
`--check` performs no writes. Repeating `--write` leaves spell records unchanged.
The script matches numeric source IDs first and uses exact slugs for the three
older detail-page URLs without IDs. The handbook's 391 unique spell names match
existing records; it does not create duplicates or fetch external data.

The audit is in `../spell-catalog-migration-audit.json`. It includes SHA-256 source
hashes, field-level description gaps found before migration, page exclusions,
and the 47 records without a supplied matching source. Gaps in description values
can include footnotes already present in another component; they are not all
claims that information was completely absent from the old record.

All 428 input files are accounted for: 423 spell detail pages, one handbook
spell-description chapter, two general spellcasting chapters, one homebrew index,
and one marketplace redirect. General rules are preserved separately in
`../spell-references/`, using the same component envelope. Index and marketplace
pages contain no full spell descriptions and are excluded with reasons. The 47
unmatched catalog spells retain their existing content, with structure normalized
and plain text recovered from existing HTML where possible. Their completeness
cannot be verified against this source folder.
