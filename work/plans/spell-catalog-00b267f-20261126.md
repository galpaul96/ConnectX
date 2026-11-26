# Spell catalog: database, service, and read UI

Status: Approved — implemented and validated
Content revision: 4 (includes the user-requested modal redesign)
Last updated: 2026-11-26 (Europe/Bucharest)
Workspace: /home/paul/Documents/Projects/Github/ConnectX
Evidence baseline: HEAD 00b267f plus the existing working tree.
Approval: User accepted the recommendation and clarified explicit primitive/JSON column storage for every component role and field. Initial implementation request remains active; approved revision 2 includes that precise condition.

Current checkpoint: T1–T5 complete. Solution build and Release publish passed; seven tests passed on isolated PostgreSQL and seven on EF InMemory; all 470 records round-tripped and rendered. Browser pagination, search, detailed/sparse modal, Escape and focus restoration verified.

## 1. Goal and success criteria

Use ingest/spell-catalog to design a lossless database domain model, implement basic repository-backed spell CRUD, and replace the Skills page preview with a paginated database-backed spell catalog. Cards show the name only; selecting one loads full spell details into a modal with stable component positions and hidden absent/empty sections. The user must choose which discovered component types are excluded from that modal.

The initial request mentions ingest, but its detailed service instruction explicitly defers creation/ingestion until later. This increment prepares the schema and CRUD contracts and validates against local data; it must not automatically import data or add a creation UI. An empty database must show a real empty state.

## 2. Scope and constraints

- Entities belong in ConnectX.Domain.Entities.DND.Catalogs.Spells.
- Move/refactor SpellConfiguration and ComponentConfiguration into ConnectX.Ef.EntityConfigurations.DND.Catalogs.Spells.
- Add SpellService under ConnectX.Services.DND.Catalogs, using ConnectX.Ef.IRepository and the established DI/service approach.
- Read queries are the priority. Basic create/update/delete methods exist without public write endpoints or ingestion execution.
- Refactor ConnectX.Web.Components.Pages.DND.Catalogs.Skills. Preserve the current /dnd/catalogs/skills route and InteractiveServer rendering.
- Excluded modal components remain in storage and detail results. Exclusion is presentation-only.
- Preserve unrelated user changes, including moved domain files, deleted EF migrations/snapshot, and modified service/AppHost project files. Do not restore the deleted spell seed migrations automatically.
- No deployment, production database mutation, external source fetch, authentication redesign, or changes to non-spell catalogs.

## 3. Confirmed repository evidence

- ingest/spell-catalog has 470 spell JSON files plus _summary.json, which is historical crawl metadata and is not a spell. Each record has id, slug, name, storageFile, and ordered components.
- All 470 source IDs and slugs are unique. A standard-library structural check found no duplicate component types per spell, missing envelope fields, or storageFile/filename mismatches.
- Each component has type, label, value, html. Values are objects, arrays, or strings. Source README allows arbitrary future JSON roles and legacy key records; new records use type.
- Observed maxima: spell name 41 characters, component type 18, label 27, HTML 4,036, serialized JSON value 6,328. Existing Value/Html limits of 255 are insufficient. Empty HTML/values are valid for absent content.
- src/ConnectX.Domain/Entities/DND/Catalogs/Spells currently contains Spell/SpellComponent but still declares the former ConnectX.Domain.Entities.Catalogs.Spells namespace. DTOs under the corresponding DND path have the same stale namespace issue.
- Spell currently has Name and Components. SpellComponent has Label, Key, Value, Html, SpellId, and Spell. Both inherit Audit (GUID identity, creation/update timestamps, soft deletion).
- src/ConnectX.Ef/EntityConfigurations/EntityTypeBuilderExtensions.cs configures GUID generation, audit defaults, soft-delete filtering, and an audit index. Existing component configuration has unique (SpellId, Key) and cascade deletion.
- EfContext discovers configurations by assembly and currently declares Spells as a field. Its using must follow the new namespace; use a conventional DbSet property.
- IRepository.GetAllAsync<T>() returns IQueryable<T>, allowing database-side Where/OrderBy/Select/Skip/Take and EF async execution. GetByIdAsync supports includes, AddAsync saves, UpdateAsync saves tracked changes, DeleteAsync soft-deletes.
- CatalogsService currently exposes GetAllSpellsAsync via AutoMapper projection, but ServiceMapperProfile is empty. Service registration registers the concrete CatalogsService. There are no other consumers discovered outside its own interface/registration.
- Skills/Index.razor uses SpellCatalogPreviewData, filters full in-memory records, and pages in memory. SpellCatalogEntry is a flattened preview-specific model. Existing modal has image, metadata, notes, description, lists, and source sections; it focuses the dialog and closes with Escape/backdrop.
- No existing .NET test project was found. .NET SDK 10.0.404 is installed; dotnet-ef is unavailable. No application build or database runtime verification has yet been performed.
- EF repository startup runs migrations. src/ConnectX.Ef/Configuration.cs currently references migration assembly Blazor.Api.Ef, which must be corrected for this repository if used by this flow. Existing migration files and snapshot are already deleted in the user's working tree; do not assume a fresh database or rewrite migration history without evidence.
- No applicable AGENTS.md was found in the repository or inspected ancestor paths.
- ingest/spell-catalog/README.md describes scripts, a schema, and a sanitizer from the source data's previous application. Those references are documentation, not proof those facilities exist here. No sanitizer was found in the current app.

## 4. Requirements and acceptance criteria

- R1 Lossless storage: persist source identity, slug, storage filename, spell name, ordered component roles/labels, arbitrary structured JSON values, and complete HTML. Round trips preserve JSON types, null/false/zero semantics, and array order. T1/T5.
- R2 Read service: database-side deterministic name ordering and pagination; only name plus separate selection identity is projected for catalog listings; detail-by-GUID includes all nondeleted spell components in source order. Cancellation must reach async query execution. T2/T5.
- R3 Basic CRUD: repository-backed create/update/soft-delete, validation of required identities/roles/JSON, explicit missing-record behavior, and no duplicate component roles. Do not accidentally delete an existing component set when a partial object is supplied. T2/T5.
- R4 Catalog: name-only cards, server-side paging, loading/empty/error/retry states, page-size controls, stable identity, and a separate detail load on selection. Avoid old filters that require fields the new summary query does not return. Name search can remain if performed in the repository query. T3/T5.
- R5 Modal: fixed semantic ordering for included types; hide absent/empty content without losing false/zero values; display full detail according to D1 [DECIDED]; preserve rich description content or provide complete encoded text, without trusting stored HTML. T4/T5.
- R6 Safe integration: correct namespaces/DI/EF configuration discovery, build the solution, and preserve unrelated edits and existing route behavior. T1–T5.

## 5. Proposed architecture and data contracts

Keep the existing Spell + SpellComponent relationship rather than introducing separate entity tables for every optional mechanic. Component JSON values retain the source's rich structure and extension roles. A class/table per component type would introduce unnecessary schema migrations and risk losing unparsed source wording.

Proposed Spell fields: inherited Audit identity, SourceId (source string ID, not the application GUID), Slug, Name, StorageFile, initialized component collection. Use source-ID/slug uniqueness to prepare for later idempotent import; preserve Unicode names. Set practical string limits above observed lengths, and text storage for large fields.

Proposed SpellComponent fields: inherited Audit identity, SpellId/Spell relationship, Type (source semantic role), Label, Position (source array order), Value (arbitrary JSON), Html (unbounded text). Use unique (SpellId, Type) consistent with verified input and deterministic Position ordering. Keep a string type rather than an enum that rejects extensions. The UI binds known role names explicitly to their dedicated renderers. Value is JsonElement explicitly mapped to PostgreSQL jsonb. Actual Npgsql/PostgreSQL tests verified JSON string/scalar, array, object and null round trips. Nested contents are never EF entities.

Retain the audit query filter. Reads must exclude soft-deleted spells and components. Updates must load tracked aggregates and reconcile components explicitly, since IRepository.UpdateAsync only saves changes and does not attach detached graphs. Replacing component sets requires removing or reactivating old roles deliberately so the unique index remains valid; do not insert a second soft-deleted duplicate role. Prefer updating matching rows and explicit aggregate reconciliation over changing shared repository semantics.

Service methods (names proposed): GetPageAsync(pageNumber, pageSize, search, cancellationToken), GetByIdAsync(Guid, cancellationToken), CreateAsync, UpdateAsync, DeleteAsync. Page inputs must be bounded, offsets safe, and final-page behavior defined. Read queries use AsNoTracking, stable OrderBy(Name).ThenBy(Id), CountAsync, and Select before materialization. Pass cancellation to EF calls. Existing repository write methods do not accept cancellation; avoid claiming cancellation support for those writes unless the interface is deliberately extended.

The summary DTO must contain only Name as requested. Carry the stable GUID separately in the page-entry envelope, e.g. SpellPageEntry(Id, SpellNameDto), within a page result that includes total count/page/page size. This keeps card content and summary data minimal while enabling reliable detail-by-ID retrieval. Repository projection reads only GUID and name, never the component graph. Detail DTO contains identity/source metadata and ordered component DTOs retaining type/label/value/html/position.

Replace or retire old spell-only CatalogsService/ICatalogsService paths after confirming references; do not leave two inconsistent query implementations. Register SpellService and, if introduced, ISpellService using the same scoped conventions. Keep EF infrastructure behind IRepository; do not inject DbContext into the UI.

## 6. Component inventory and modal proposal

D1 [DECIDED] controls exclusions. Counts below are records containing the role, including empty values; not the number of visible sections.

| Role | Label | Present / 470 | Proposed modal area |
| --- | --- | ---: | --- |
| level | Level | 470 | Metadata |
| castingTime | Casting time | 470 | Metadata |
| rangeArea | Range / area | 470 | Metadata |
| components | Casting requirements (V/S/M) | 470 | Metadata + material details |
| duration | Duration | 470 | Metadata |
| school | School | 470 | Metadata |
| attackSave | Attack / save | 470 | Metadata |
| damageEffect | Damage / effect | 470 | Metadata |
| description | Description, including scaling, tables, stat blocks | 470 | Main content |
| castingTimeBlurb | Casting time note | 470 (10 populated) | After metadata |
| spellTags | Spell tags | 470 (389 populated) | Lists |
| availableFor | Available classes/subclasses | 470 (448 populated) | Lists |
| source | Bibliographic source | 470 | Footer |
| url | Source URL | 470 | Footer |
| imageUrl | Image | 470 (397 populated) | Artwork |
| bookMetadata | Handbook casting details | 391 | Additional source details |
| sourceFiles | Local source file paths | 423 | Provenance |
| descriptionVariant | Alternate source descriptions | 8 | Separate alternate text section |

Recommendation: exclude sourceFiles (internal provenance) and bookMetadata (mostly duplicates the primary metadata); keep descriptionVariant available in a separate section because it can contain materially different wording. User accepted both exclusions; retain descriptionVariant.

Proposed display rules: fixed role registry/layout order, each section tests semantic content rather than mere component existence. All records contain some empty role placeholders, e.g. damageEffect.text may be empty with an empty damageTypes array, or spellTags may contain an empty list with <ul></ul> HTML. Such sections are hidden. Structured false values such as material.consumed=false and concentration=false remain meaningful within an otherwise populated component. Per the subsequent user clarification, there is no generic JSON/HTML fallback: dedicated components explicitly render each known role. Unknown roles and fields stay in storage/detail DTOs until dedicated presentation is added.

Rendering uses encoded structured values only. Description text blocks retain all ordered content (including table rows and stat blocks as text); source HTML is preserved in storage/detail results rather than injected. All 16 included types have dedicated Razor components. Images and source links allow HTTP/HTTPS URLs only. Metadata has named CSS grid slots so missing neighboring types do not move a field to another slot.

## 7. End-to-end flows

F1 Browse (R2/R4): route initializes -> scoped SpellService -> IRepository spell query with name search if present -> count/project/page in database -> show names -> page or size change reruns query. No component payload is needed. Empty page after concurrent removal clamps/reloads sensibly; empty catalog explains no records are available, with no preview fallback.

F2 Details (R2/R5): select GUID -> show modal loading shell -> detail query projects the spell and complete ordered component list -> render included populated roles in fixed slots. Not-found shows an explicit state; query failure offers retry/close. Close/cancel or selecting another spell must prevent a slower old request from reopening/replacing the modal. Preserve Escape, close button, focus management, and backdrop behavior; add focus containment/restoration where needed.

F3 Writes (R3): service validates full write contract -> create or load tracked spell aggregate -> map/reconcile component data -> repository save -> return identity/detail. Missing update/delete has explicit semantics. No UI exposes this path and no importer calls it in this increment.

## 8. Decision register and assumptions

- D1 [DECIDED] Modal exclusions: choose any of the 18 roles above, or none. This was explicitly requested by the user. Recommendation and alternatives are in section 6. Unblocks final T4 layout and R5 verification. Storage and detail retrieval retain every role regardless of the choice. Resolved by the user: exclude sourceFiles and bookMetadata from the modal only.
- D2 [DELEGATED TO IMPLEMENTATION] Local schema/service/UI mechanics: user permits replacement/refactoring and names target namespaces. Implementer can choose DTO names, CSS, bounded page sizes, JSON CLR mapping, and interface arrangement within the contracts above. Escalate only if evidence requires data loss, destructive migration history changes, broader scope, or external behavior contradicting the request.
- D3 [DECIDED] Ingestion timing: use data to design/validate now; ingestion execution and creation UX are later, per the user's explicit detailed instruction. Initial broad ingest wording is interpreted under that more specific instruction.
- A1 Proposed simplification: retain name search; remove preview-only rich filters for this read increment, because a name-only projection cannot support those filters client-side. Any later rich filters should be implemented in database queries.
- A2 Migration feasibility remains a fact to verify, not a request to erase/recreate databases. Inspect current migration state before generating a migration. No production connectivity is needed for planning.

## 9. Ordered implementation packages

### T1 Domain and persistence (R1/R6)

Update the existing moved entity files and DTO namespaces. Move/refactor both EF configuration files into EntityConfigurations/DND/Catalogs/Spells, update context imports/property, establish jsonb/text storage, source identity, component ordering, uniqueness, and audit relationships. Correct only relevant stale migration-assembly configuration. Preserve unrelated edits. Inspect deleted migration state before choosing a forward migration approach; do not restore seeds or run automatic import. Verify EF model/configuration discovery and structured value round trip, including Unicode, long description and null values. Owner: primary implementing agent.

### T2 Service and CRUD (R2/R3/R6; depends T1)

Add SpellService and optional interface under Services/DND/Catalogs; add summary/detail/page/write DTOs under the corresponding Domain/Services/DND/Catalogs/Spells namespace. Implement repository query projections and aggregate writes as in section 5. Wire scoped DI; resolve obsolete spell-only service references and mapper configuration intentionally. Verify pagination ordering/counts, minimal list projection, cancellation, missing/deleted reads, and update/delete behavior. Keep writes internal to the service boundary without introducing endpoints. Owner: primary implementing agent; shared contract work is sequential with T1.

### T3 Database-backed list (R4/R6; depends T2)

Refactor Skills/Index.razor, its scoped CSS, imports, and SpellCard. Remove preview dependency/unsupported filters; show name-only cards, search if retained, and page-size/previous/next controls. Trigger async service reads with loading/error/retry/empty states; avoid overlapping stale requests and duplicate unbounded full-catalog fetches. Preserve page route and public access conventions. Remove preview-only model/data only after checking remaining references. Verify page transitions and selection IDs. Owner: primary implementing agent.

### T4 Component modal (R5/R6; depends T2/T3 and D1)

Refactor SpellDetailModal and existing subcomponents to consume database detail DTOs and role-based fixed slots. Implement every included role, including nested casting requirements/materials, source variants, handbook metadata/provenance if selected, and long/tabular description content. Add encoded rendering and semantic absence checks. Handle asynchronous detail loading/errors/not-found/close races and keyboard/focus behavior. No exclusion changes storage. Verify representative fireball, a homebrew record, a source-variant record, and sparse role values. Owner: primary implementing agent; D1 is resolved.

### T5 Focused integration validation (R1–R6; depends T1–T4)

Run dotnet build src/ConnectX.slnx and focused automated tests or a local validation harness suited to the available dependencies. Use the EF model and a database provider appropriate to each claim: InMemory can exercise repository service semantics, but cannot prove PostgreSQL jsonb/SQL behavior. Verify SQL projection/query translation using Npgsql where feasible and clearly state any real database limitation. Load fixture records into an isolated test context only; do not ingest into the user's app database. Exercise data round trips across all 470 JSON records without treating _summary as a spell. Test meaningful cases: arbitrary JSON values, long HTML, complete description variants, empty optional roles, name-only list projections, detail completeness, page boundaries, soft deletion, missing IDs, graph updates, and stale modal requests. Finish with a diff/status check preserving baseline user changes. Report commands and limitations truthfully. Owner/integrator: primary implementing agent.

## 10. Delegation

No agents dispatched. Work is tightly coupled through schema/DTO contracts and can proceed sequentially. After T2 fixes shared contracts, service validation and frontend implementation would be bounded parallel candidates only if later explicitly authorized; no delegation is needed now.

## 11. Validation performed and pending

Performed: standard-library parse and structural inspection of all 470 records; unique source IDs/slugs, matching storage filenames, complete component envelopes, no duplicate per-spell role types; measured lengths, shapes, role counts/population. Result: zero structural issues in the inspected invariants. This is not full JSON-schema validation or an application round-trip test. Checked dotnet SDK availability and lack of dotnet-ef. The implementation checks below supersede the initial lack of build/runtime evidence.

Completed: T1–T5, Npgsql model/migration/SQL mapping, browser interaction and screenshot verification. No supplied scripts from the ingest README were claimed or used; this repository now has its own .NET test project.

## 12. Delivery, risks, and recovery

Deliver source changes and focused validation evidence locally. No deploy/commit/import is implied. Migration artifacts must reflect the approved schema without silently restoring or rewriting the user's removed history. If current databases need a different baseline strategy, document the actual migration limitation and preserve existing data. Preview content is removed from the runtime path; a database with no records will be empty until the later ingest.

Material risks: data truncation under old fields; JSON shape flattening; rendering untrusted HTML; losing alternate source wording; repository updates on detached graphs; soft-deleted components colliding with unique roles; migration startup referencing the wrong assembly; overlapping UI requests; confusing source string IDs with application GUIDs. Mitigations are assigned to T1–T5 above.

## 13. Handoff

Revision 2 was approved by the user accepting the recommendation and requiring component primitive/JSON storage. The subsequent clarification requires dedicated UI components for every included type. All T1–T5 work is complete within that scope; no user-owned blockers remain. No application database was ingested. Existing unrelated project-version edits and old migration deletions remain; a new initial catalog migration and replacement snapshot describe the new schema.

### Approved JSON storage clarification

Every component field is either a primitive mapped column or an explicitly mapped JSON column. Value uses a JSON DOM value mapped to jsonb, including JSON string/scalar values, arrays and arbitrarily nested objects; EF must never discover owned types or tables from its contents. Type/Label/Html are strings, Position is an integer, and the only relationship is SpellId to the parent Spell. No subtype tables or tables for nested casting/material/description fields. Validate the relational model has exactly Spells and SpellComponents tables and round-trip all observed shapes.

### UI clarification (user)
Each included role must have a dedicated Razor component with an explicit field layout. No generic JSON/HTML renderer or unknown-role fallback in the modal. Retain unknown roles in storage/detail DTOs for future dedicated renderers. Render structured text through Razor encoding to avoid source-markup artifacts.

## 14. Completed implementation and verification evidence

- Entity/DTO namespaces now include DND. Spell preserves SourceId, Slug, Name, StorageFile; SpellComponent has primitive fields plus JsonElement Value, explicit jsonb mapping and text Html. Only Spells/SpellComponents are modeled; no nested tables or owned types.
- Added Services/DND/Catalogs/ISpellService.cs and SpellService.cs. Removed obsolete spell-only CatalogsService/ICatalogsService. CRUD uses IRepository; summary SQL selects only GUID and Name, with deterministic server paging/search; the name DTO contains only Name. Full detail retains every role, including modal-excluded provenance.
- Full update reconciles/soft-deletes/reintroduces roles without duplicate unique keys. Calls serialize access to the scoped EF repository. Invalid inputs, cancellation, missing/deleted records and page bounds have focused coverage.
- Replaced preview UI/model/data with service-backed name-only cards and separate asynchronous detail loading. Added explicit components for all 16 included roles and named metadata slots. No raw HTML or JSON dump renderer exists. Native dialog + wwwroot/js/spellDialog.js supplies focus containment, dismissal and restoration. Loading, empty, retry and missing-detail states are explicit.
- Generated Migrations/20261126144902_SpellCatalog.cs, its designer and EfContextModelSnapshot.cs using cached EF tooling. This is an initial migration for the separate catalog database. Legacy databases with old spell tables require a deliberate upgrade/baseline strategy rather than applying this initial migration over those tables; tests and README state that constraint. No old seed migration was restored.
- Web references Services, registers the repository and spell services; AppHost provisions/references api-db separately from identity. Dockerfile copies all six project files in the Web dependency graph before restore. Compose and .env.example provide CATALOG_DB/api-db. This wiring is required because Web previously referenced only ServiceDefaults.
- Added tests/ConnectX.Spells.Tests to src/ConnectX.slnx, plus an exact workflow README. Uses cached xUnit v3 packages and test-only InternalsVisibleTo access to the EF context/repository.
- `dotnet build src/ConnectX.slnx --no-restore -m:1 -v quiet`: passed, 0 errors; 26 existing warnings in account/identity pages. New test analyzer warnings were resolved by passing the xUnit cancellation token.
- `dotnet publish src/ConnectX.Web/ConnectX.Web.csproj --no-restore -m:1 --configuration Release --output /tmp/connectx-spell-publish -v quiet`: passed. Verified published EF/Services assemblies and spellDialog.js.
- `dotnet test tests/ConnectX.Spells.Tests/ConnectX.Spells.Tests.csproj --no-build --no-restore -m:1 -v minimal` with CONNECTX_SPELL_TEST_POSTGRES pointing to an isolated local PostgreSQL 18.3 container: 7 passed, 0 failed, 0 skipped (6 seconds final run). Each database uses a unique connectx_spell_test_<guid> name and is removed by the test. Tests exercise all 470 JSON records, migrations, SQL projections, JSON kinds, long values, CRUD reconciliation and UI rendering.
- Same test command without PostgreSQL configuration: 7 passed, 0 failed, 0 skipped (3 seconds), verifying the documented InMemory fallback. This run is not used as evidence of SQL behavior; the PostgreSQL run provides that evidence.
- `POSTGRES_PASSWORD=validation-only docker compose config --quiet`: passed. Static dependency-graph check confirms every project reference is copied in Docker restore. A Docker image build was not performed.
- Browser validation used a temporary local Web instance with disposable catalog/identity databases. Only that disposable catalog received fixture data. Verified 470 real records, page 2 showing 25–48, sparse homebrew modal hiding empty damage/tags/image, populated Revivify showing material cost/consumption, search results for Fireball/Revivify, Escape dismissal and restored focus. Screenshot saved to /tmp/connectx-spell-modal.jpg. A later browser fill immediately after close hit a transient automation focus mismatch; the observed final close/focus state was correct. No unverified extra browser outcome is claimed.
- `git diff --check`: passed. Temporary validation services are stopped/removed at completion; fixture data is not an application ingest and no production/existing app database was modified.

## 15. User-requested modal redesign follow-up

The user explicitly requested a skill-guided redesign/improvement of the spell modal. Applied Product Design context/review guidance and fixing-accessibility, using fresh browser captures of the current code as evidence. The scope is presentation and modal interaction; database/service contracts and exclusions remain intact.

Implemented a compact header with decorative school image, title and dedicated level/school renderers; a six-field casting sidebar with fixed semantic positions; a wider rules column; persistent source footer; responsive single-column layout; conditional removal of empty groups; larger close target; keyboard-scrollable information region; background scroll lock/restoration; and explicit forward/backward Tab wrapping in addition to the native dialog. Existing dedicated components continue to render every included role. No generic JSON/source-HTML renderer was introduced.

Build passed. Seven tests passed, including all 470 spell renderings and updated assertions for omitted empty metadata/sidebar/footer groups. Browser checks covered populated Revivify and sparse/long-title Owlheart Aria at desktop, 390px and 320px widths; no horizontal overflow; Tab/Shift+Tab wrapping; Escape; original trigger-focus restoration; and restored page scroll. Key text contrast pairs measured above 7:1. No full WCAG or screen-reader compliance claim is made. Review notes and before/after screenshots are in /tmp/connectx-modal-redesign/. The final preview is left running at http://127.0.0.1:5079/dnd/catalogs/skills against disposable fixture databases for user inspection. No production/app catalog ingest occurred.

## 16. Authorized single-spell upload increment

The user has now explicitly authorized creation/ingestion of one spell from an uploaded file; this supersedes the earlier deferral for that bounded flow. Bulk ingestion remains outside scope.

Added a DungeonMaster-only Create spell button and compact accessible native upload modal. Anonymous users and ordinary authenticated members cannot see it. Opening and submitting call SpellUploadService, which checks authentication/role claims and current identity-database membership through a fresh UserManager scope. Membership is rechecked after file parsing as well. The modal uses a labeled single InputFile, size/type checks, explicit validation/status/errors, disabled repeat submission, cancel/close behavior and the existing keyboard/focus helper. File contents are held in bounded memory and never written to app storage. Browser automation confirmed anonymous hidden state, test DungeonMaster login, visible button and modal opening. Its file-chooser automation stalled before file selection, so end-to-end browser file submission is not claimed.

SpellUploadReader maps a single JSON object using the ingest envelope into SpellWriteDto, preserving cloned JSON values, HTML and order. It enforces a 1 MB actual-stream limit, depth 32, required fields, safe metadata paths and a nonempty description. Shared SpellWriteValidation provides required/length/component count/content size/unique role/known-shape checks. SpellService.AddAsync validates independently, checks duplicate source IDs/slugs including retired records, catches PostgreSQL unique races, and uses IRepository.AddAsync. Existing CreateAsync forwards for compatibility. Repository failure handling detaches newly tracked insert graphs to allow retries after rejected inserts. On success the catalog refreshes and searches for the newly created name, with an announced confirmation.

Validation: build passed (existing account-page warnings). Twelve tests passed on InMemory, including 470 reader-to-AddAsync round trips, malformed files, oversized declared/actual content, duplicate values, role visibility for anonymous/member/DM, current database membership and role removal. PostgreSQL validation uses isolated uniquely named test databases. Screenshot: /tmp/connectx-spell-upload-modal.jpg. No existing application data or role assignments were changed; the browser test account is confined to the disposable preview identity database.

## 17. Rich spell card follow-up

The user explicitly requested image, title, level and school on catalog cards, superseding the earlier name-only presentation. Replaced the name DTO with SpellSummaryDto (Name, ImageUrl, Level, School). The paginated query projects only the three required component type/value pairs with GUID/name; it excludes component HTML and the other roles. Full details still load on selection.

Cards use a compact 48px thumbnail, wrapping serif title, school subtitle and level badge. The existing responsive grid and modal interaction remain. Missing/invalid image URLs are omitted and load errors remove the image rather than leave a broken asset. The browser preview against the disposable 470-spell catalog was inspected and saved to /tmp/connectx-spell-cards.jpg. Build and all 14 tests passed against PostgreSQL (10 seconds), including card field mapping, SQL payload checks, full template round trips, batch upload and authorization regressions. git diff --check passed.
