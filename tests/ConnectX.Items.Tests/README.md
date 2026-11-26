# Items catalog validation

Run `dotnet test tests/ConnectX.Items.Tests/ConnectX.Items.Tests.csproj` from the repository root.
The suite reads all 310 JSON records in `ingest/items` and covers lossless component
round trips, compact card queries, CRUD and soft deletion, restored components,
identity conflicts, pagination/search, cancellation, upload validation, batch
results, current DungeonMaster membership, and encoded detail rendering.

By default, database operations use EF InMemory. Set `CONNECTX_ITEM_TEST_POSTGRES`
to a local PostgreSQL connection string to exercise the real migration, SQL
projection, JSONB storage, and unique constraints. The account must be able to
create databases. Each test creates and removes its own `connectx_item_test_<guid>`
database; the database in the supplied connection string is never modified.
The model test also verifies snapshot consistency and that the Items migration
adds only `Items` and `ItemComponents` alongside the existing Spells tables.

Items use their own entities, DTOs, service, upload reader/batch, Razor components,
styles, and dialog module. The `ItemWriteDto` contract replaces the entire ordered
component list. Source JSON and HTML are retained; the interface renders encoded
structured values, with semantic tables and source variants. Internal crawl
provenance stays in storage and detail DTOs.

At `/dnd/catalogs/items`, DungeonMasters can select **Create** and upload up to 500
JSON files, 1 MB each. Every file must use the `ingest/items` envelope and include
`recordKind: item`, a nonempty `itemType`, and a nonempty description component.
Files run sequentially; individual failures retain successful imports. Role
removal stops the remaining batch. Reopen the selection to retry failed files.

The application applies the additive `ItemCatalog` migration through its existing
startup migration flow. Importing catalog content remains an explicit Create action.
