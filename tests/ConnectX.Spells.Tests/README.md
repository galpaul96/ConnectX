# Spell catalog validation

Run the default service and rendering checks with:

```sh
dotnet test tests/ConnectX.Spells.Tests/ConnectX.Spells.Tests.csproj
```

Set `CONNECTX_SPELL_TEST_POSTGRES` to a connection string for a disposable local
PostgreSQL instance to run the same service checks against PostgreSQL and the
actual migration. The account needs permission to create databases. Every test
creates and removes its own `connectx_spell_test_<guid>` database; the database
named in the supplied connection string is never migrated or deleted.

Without that variable, service checks use EF InMemory. The relational-model test
still checks Npgsql's model, but SQL projection and PostgreSQL JSON round trips
require the PostgreSQL run.

The tests read all 470 records in `ingest/spell-catalog`, excluding `_summary.json`.
They cover migration/model shape (only Spells and SpellComponents), every JSON
value shape, source identity, ordering, long HTML, compact card SQL projection,
pagination/search, cancellation, missing/deleted records, removed/reintroduced
component roles, and dedicated UI rendering/encoding/optional sections.

`SpellWriteDto` is a complete replacement contract. Its component list is required;
positions are assigned from list order. Unspecified existing roles are retired;
reintroduced roles reuse their rows. JSON values are cloned so callers can dispose
their source JsonDocument after the write.

The Web app uses a separate `api-db` connection for spells, wired through AppHost.
The new initial catalog migration targets that catalog database. An installation
with the former spell tables/migration history needs a deliberate upgrade/baseline
strategy; do not apply the new initial migration over the old tables.

The catalog now includes a multi-file JSON upload/Create spell modal for signed-in
DungeonMasters. SpellUploadService rechecks current database role membership before
reading and before insertion, maps the file through SpellUploadReader, and calls
ISpellService.AddAsync. Bulk import is not provided. Tests use disposable databases.
Stored HTML remains available in detail DTOs, but UI components display encoded
structured values through explicit role-specific layouts. Unknown roles remain
in storage and detail DTOs until they receive a dedicated UI component.


Each spell upload accepts `.json` files up to 1 MB, with the same `id`, `slug`,
`name`, `storageFile`, and `components` envelope as `ingest/spell-catalog`.
A nonempty description is required. Web checks enforce file limits, valid JSON,
safe metadata filenames, and a single spell object. Shared DTO validation checks
required fields, component count/size, unique roles and expected known value shapes.
Unknown extension roles retain arbitrary JSON values. SpellService repeats DTO
validation, rejects existing source IDs/slugs (including retired records), and
relies on database uniqueness for concurrent inserts. A failed repository insert
detaches the rejected graph so a subsequent valid attempt can succeed.

Upload tests cover all 470 templates through reader -> DTO -> AddAsync -> detail,
malformed/wrong/oversized data, invalid value shapes, duplicates, anonymous/member
button visibility, current DungeonMaster membership, and role removal mid-session.

The upload modal accepts up to 500 files and processes them sequentially through the
existing single-spell AddAsync flow. Individual errors do not roll back successful
files. Created files are never resubmitted within the same selection; select failed
files again to retry. Loss of authorization stops the remaining queue. The catalog
refreshes after completion and the modal retains per-file results until closed.

Spell card summaries now include name, image URL, level label and school. The paginated
query selects only the level/school/imageUrl component values, without HTML or other
spell content. Full descriptions remain part of the separate detail query. Card
images accept HTTP/HTTPS URLs and are hidden when absent or when loading fails.
