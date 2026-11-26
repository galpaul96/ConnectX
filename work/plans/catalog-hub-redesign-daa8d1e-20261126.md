# Catalog hub redesign

Status: Approved
Content revision: 4
Last updated: 2026-11-26, Europe/Bucharest
Repository: `/home/paul/Documents/Projects/Github/ConnectX`, branch `main`, baseline `daa8d1e`; working tree clean before planning.
Approval: Revision 3 authorized by the user’s follow-up instruction to make DND self-contained, including integrating Directory functionality into DND pages. This instruction changes the ownership boundary of revision 2 while retaining its Catalog redesign. Local implementation is authorized; no deployment or database changes.

Current checkpoint: T1–T3 complete. DND owns all feature components/data/styles under Pages/DND; the generic Directory layer is removed. Revision 4 records implementation and verification of the approved revision 3 scope; approval is unchanged.

## Goal and scope

Redesign the Web Catalog hub while preserving a grid of cards linking to the individual application sections. Make the page easier to understand, modular, and responsive. Preserve the existing dark plum/copper design language, native links, all eight destination routes, and Blazor Interactive Server architecture.

Scope: redesign `/dnd/catalogs` and remove `Components/Directory` by integrating each hub’s markup/data/filtering/styles directly into its DND page. Move the repeated placeholder component and its isolated CSS under `Pages/DND/Components`, with a DND-scoped import. Update all ten placeholder callers. Replace the global NavMenu’s data-driven two-link loop with its two explicit native NavLinks and remove the global Directory import. Catalog, DND, and Codex routes remain unchanged. Keep existing Skills & Spells components in place. Backend/data migration, authentication, global navigation redesign, new packages, publication, and deployment are out of scope.

## Evidence baseline before implementation

- `src/ConnectX.Web/Components/Pages/DND/Catalogs/Index.razor` renders `DirectoryHub` with Catalogs entries and Interactive Server mode.
- `Components/Directory/DirectoryHub.razor` owns header, search, filtering, grid, empty state, and count. Matching is case-insensitive against title and description. There is no explicit reset action. The whole grid is a polite live region, while the result count is outside it.
- `Components/Directory/DirectoryCard.razor` renders one whole-card anchor with a decorative numeric index, title, description, and northeast arrow. Shared CSS sets small description text and a three-part horizontal composition.
- `Components/Directory/DirectoryEntries.cs` provides eight static Catalogs entries: Classes, Subclasses, Species, Backgrounds, Skills & Spells, Feats, Stat Blocks, Items. `DirectoryEntry` has Title, Description, Href, Index. The data also drives DND/Codex hubs and sidebar labels.
- Seven Catalogs destinations render `DirectoryIndexLanding`, a migration scaffold. Skills & Spells is a functional sample-data preview. Existing card copy exposes migration language without consistently explaining destination availability.
- `DirectoryHub.razor.css` uses auto-fill with a 20rem minimum and forces one column below 680px. Shared shell uses a 306px sidebar above 900px and a mobile drawer below.
- `wwwroot/app.css` defines reusable colors and Bootstrap overrides. `Components/App.razor` imports Bootstrap 5.3.8 from its CDN; there is no discovered icon library.
- .NET SDK 10.0.115 is installed. Program startup requires Redis configuration and a PostgreSQL identity connection, and automatically migrates the identity database. AppHost provisions both with Docker.
- No AGENTS.md or automated test project was found in the inspected workspace. Elevated read-only Docker inspection succeeded and found existing service containers, but no running Web listener was found by elevated `ss -ltnp`. The in-app browser inventory had no preview tabs. Baseline `dotnet build src/ConnectX.Web/ConnectX.Web.csproj --no-restore` passed with 0 errors and 26 existing account-component warnings (nullable references and form parameter initializers). No app startup was attempted because `Program.Main` automatically applies identity database migrations, which is outside read-only planning. Browser visual audit remains unavailable until an app preview can run.

## Proposed design

Retain one flat, scannable grid and the existing theme. Give the header a concise purpose statement, a breadcrumb back to DND, and an explicit section-search label. Use a toolbar above the grid for search, a compact result count, and conditional reset. Give cards more breathing room, stronger titles, concise user-facing descriptions, a consistent internal-navigation cue, and plain-language preview/coming-soon status. Use a vertical card composition: quiet section number and status at the top, title and description in the middle, and an understated “Explore section” cue at the bottom. Keep the whole card as the single anchor; the cue is not a nested button. Keep every destination linked, including scaffold destinations.

Avoid extra tabs or grouping controls for eight entries. Each hub owns its small local entry list, markup, search state, and isolated CSS; Catalog-specific presentation needs no generic opt-in abstraction. A single DND-local landing component avoids duplicating ten placeholder layouts. Responsive layout fits available content width. Reuse global app color tokens as primitives; all DND-specific styling stays under Pages/DND.

### Concrete presentation specification

- Header: breadcrumb “DND / Catalogs”, existing “Reference library” eyebrow, “Catalogs” H1, and one short sentence: “Explore character options, spells, creatures, and equipment for your next adventure.” Preserve restrained serif headings and system-font supporting text.
- Toolbar: visible “Find a section” label, full-width input on mobile, placeholder “Search classes, spells, items…”, result count alongside on wider screens, and conditional “Clear search” button. Explain that these are section links rather than record-level search results.
- Grid: use content-container breakpoints at 34rem and 52rem for one, two, and three columns, capped at three. This replaces the earlier auto-fill proposal with a simpler explicit cap. Aim for three columns when the actual content area can fit them, two at intermediate content widths, one on phones; preserve readable text rather than forcing a viewport-only column count. Use approximately 1rem gaps and 1.25–1.5rem card padding. Wrap all text and use minimum heights only, never clipping.
- Cards: retain the existing section numbers as quiet navigation markers. Use roughly 1.25rem titles and at least .9rem supporting copy. Set a clear surface/border contrast with the existing theme tokens and an obvious focus outline. Keep all card bottoms/cues aligned within a row using flexible layout. Do not add artwork or an icon dependency.
- Exact card descriptions: Classes — “Explore class features, roles, and progression.” Subclasses — “Find specializations and paths for each class.” Species — “Discover playable peoples and their traits.” Backgrounds — “Choose origins, proficiencies, and starting features.” Skills & Spells — “Browse spell details, casting requirements, and effects.” Feats — “Explore character benefits and their prerequisites.” Stat Blocks — “Find creatures, NPCs, and their game statistics.” Items — “Explore equipment, tools, treasure, and magic items.”
- Status: Skills & Spells is “Sample preview”; the other seven are “Coming soon”. Add a brief legend explaining that coming-soon sections currently contain an overview. Keep those cards clickable because all eight routes already exist and the user requested section links. No record counts or “ready” claims.
- Empty state: “No sections found”, show the searched term safely through Razor text rendering, suggest another section name, and provide “Clear search”. Maintain a predictable focus target when removing a conditional reset control; explicitly restore focus to the input if needed.
- Search result announcement: a short `role="status"` summary such as “2 of 8 sections”, with only the summary in a live region. Preserve semantic heading hierarchy and never announce the full card DOM on each keystroke.

Presentation follows revision 2 with the user-directed DND-local architecture in D1. Exact CSS values and local names remain implementation discretion.

## Requirements and observable acceptance

- R1: Preserve all eight current routes and whole-card native anchors, including normal browser link behavior. T1/T2.
- R2: A visitor can identify each section's purpose and whether it contains a preview or a placeholder before navigating. Status must be textual, not color alone; no invented record counts or availability. T1/T2.
- R3: Search is explicitly described as searching sections, matches titles/descriptions case-insensitively after trimming whitespace, reports a concise result count, and provides reset/recovery when no matches remain. T2.
- R4: Grid reflows cleanly at 320px, 390px, 768px, 1024px, and 1440px viewport widths, including the existing sidebar transition. No horizontal page overflow, clipped labels, or fixed card heights that truncate content. T2/T3.
- R5: Use semantic heading order, visible input labels and keyboard focus, native controls, adequate text contrast, and reduced-motion behavior. Announce result summary updates rather than the full card grid. T2/T3.
- R6: Each hub owns its data and behavior inside Pages/DND. No production reference to Components.Directory or DirectoryHub/Card/Entry/Entries/IndexLanding remains. No DND-specific CSS is added to global app.css; existing global theme tokens are reused. No new backend calls/storage/packages. T1/T3.
- R7: Preserve all DND/Codex routes, search behavior, and landing-page navigation; keep repeated placeholder markup in one DND-local component. T1/T3. Enable InteractiveServer on the DND root so its existing search actually works.

## Contracts and flows

The authoritative Catalog route list becomes the private entry list in `Pages/DND/Catalogs/Index.razor`. A private record may model Title/Description/Href/Index/IsPreview; do not export a generic model. DND and Codex own their respective short entry lists. Global NavMenu retains only explicit DND/Catalogs/Codex links, without importing DND models. Do not infer availability from URL or index. Search uses existing synchronous local data, adds whitespace trimming, and continues to search Title/Description only. Clearing search restores all eight cards in their current order and keeps a predictable keyboard focus target.

F1: Open Catalogs → read title/purpose → scan all eight cards and status → activate a card → existing section route. Destination routes and spell functionality remain. Placeholder callers switch to the DND-local landing component, with clear coming-soon copy instead of migration terminology.

F2: Enter search → update matching cards and polite result summary → if empty, show a clear explanation and reset button → reset returns all cards without losing keyboard access. A broad query can match multiple sections; filtering never changes hrefs.

## Decisions and assumptions

- D1 [DECIDED]: Make DND feature code and design self-contained. Authority: user follow-up, “have the design and everything about the DND pages be as self contained as possible,” including removing Components/Directory and integrating functionality directly into DND pages. This precisely replaces the shared-directory architecture while retaining the Catalog design; revision 3 is the resulting authorized scope.
- D2 [DELEGATED TO IMPLEMENTATION]: Choose local private entry types, CSS values, and one DND-only placeholder component name. Keep hub state/data/markup page-local; do not recreate a generic directory framework under a new folder. Escalate only for changes to global shell behavior or destination functionality.
- A1: “Web's Catalog hub” refers to `/dnd/catalogs`, supported by the repository's only matching hub.
- A2: Existing theme and route structure are constraints inferred from “keeping the same approach”; proposed refinements are reversible.
- A3: Prefer text/typography and existing code assets; do not add a new icon dependency solely for eight cards.
- Verification limit: baseline visual audit was unavailable because no normal Web app was running. Implementation was later checked in a loopback-only isolated UI host using the actual built Razor components, Routes, MainLayout, global theme CSS, and compiled isolated CSS. It uses anonymous authentication and no app database or Redis. These checks establish UI behavior, not backend integration or full accessibility compliance.

## Implementation work packages

### T1 — Remove the generic directory layer

Authorized by D1. Serves R1/R6/R7. Integrate DND and Codex entry lists, card markup, search state, count/empty behavior, and isolated CSS in `Pages/DND/Index.razor[.css]` and `Pages/DND/Codex/Index.razor[.css]`. Keep native link routes and titles; add the missing InteractiveServer directive to DND. Move `DirectoryIndexLanding.razor[.css]` into proposed `Pages/DND/Components/DndSectionLanding.razor[.css]`, update ten callers and use a DND-scoped `_Imports.razor`. Keep this small repeated-layout component, since ten copies would complicate maintenance. Use user-facing coming-soon copy. Replace NavMenu’s two-entry data loop with explicit Catalogs and Codex NavLinks. Remove global Directory import. Delete all Directory files once Catalog integration in T2 is complete; verify no stale names/imports. No database/API/schema change.

### T2 — Catalog page-local redesign

Depends on T1’s ownership boundary; serves R1–R6. Implement the concrete presentation in `Pages/DND/Catalogs/Index.razor` and its new isolated CSS. Keep private entry metadata in that page, with the eight exact current hrefs and descriptions above. Render header/breadcrumb, visible search label, result summary, reset, empty state, vertical cards, and accurate status text directly in the page. Trim query whitespace and compare title/description case-insensitively. Use one native anchor per card and semantic h2 card headings. Keep result announcements in a compact status region. Use ElementReference.FocusAsync after reset so conditional controls can disappear without losing focus. Flexible CSS tracks, wrapping, visible focus, reduced motion, and minimum touch sizes apply. Do not change app.css or add packages. DND/Codex may use simpler versions of the card presentation in their own stylesheets; do not introduce a generic card abstraction solely to deduplicate these small pages.

### T3 — Focused build and browser verification

Depends on T2. Serves all requirements. Build `dotnet build src/ConnectX.Web/ConnectX.Web.csproj --no-restore` (restore only if needed and available). Use the actual app where dependencies are available. Check initial grid, matching/whitespace/mixed-case queries, no-results/reset, keyboard focus, route destinations, responsive widths, and DND/Codex regression surfaces and all ten placeholder routes. Verify no stale Directory symbols remain. If normal Web startup needs database migrations, use a temporary isolated preview host under /tmp referencing the actual compiled Razor components, MainLayout, Routes, and styles with anonymous auth; report that this verifies UI behavior, not production service integration. Capture desktop/mobile screenshots. Do not add tests that merely mirror static markup. Record concrete limitations if app dependencies prevent browser checks. Build and source inspection cannot establish full accessibility compliance.

## Validation, delivery, and recovery

Planning-time: source review completed and baseline build passed with the 26 existing warnings described above; browser review could not be completed without a running Web preview. Implementation-time: T3 supplies build and interaction evidence. Walk F1 and F2 through all relevant requirements before handoff. Final review must distinguish observed rendering from source inference.

Delivery is a local uncommitted change with no publication, database operations, or deployment. Recovery is reverting only the scoped tracked edits. Keep this plan as temporary working material; do not automatically commit or remove it. Stale imports/routes after removing Directory and inaccurate readiness labels are the main risks. Search the whole src tree for old symbols, compile all Razor callers, and verify links and source-backed statuses.

## Delegation

Sequential work under the primary agent: metadata, presentation, and verification are tightly coupled and small enough that delegation adds little value. No agents dispatched.

## Approval and handoff

Revision 3 is approved for local implementation by the user’s directive recorded in D1. The precise ownership change is fully propagated above. No user-owned decisions remain pending. T1 → T2 → T3 is the dependency order; the primary agent owns integration. F1 verifies native navigation and preserved routes; F2 verifies filtering and reset/focus recovery. Local implementation details are bounded by D2. Browser verification must identify whether it used the normal app or an isolated UI host. Final delivery must state build and visual-verification limits. Do not deploy, publish, or migrate a database as part of this work.

## Completed implementation and verification

- Removed all eight files under `Components/Directory`. Catalogs, DND, and Codex each own their markup, short entry list, filtering, state, and isolated CSS in their corresponding `Index.razor[.css]` files.
- Repeated placeholder markup/styles now live only in `Pages/DND/Components/DndSectionLanding.razor[.css]`. Ten callers use the DND-scoped `_Imports.razor`. The global import was removed; NavMenu has explicit Catalogs/Codex links.
- Catalog keeps the eight native links and adds accurate availability text, clearer descriptions, visible search labeling, a compact result status, empty-state recovery, reset focus, responsive container queries, and visible keyboard focus. Catalog card class names avoid the unrelated global `.catalog-card` selector.
- DND now uses InteractiveServer so its page-local search works. DND/Codex use auto-fit card grids so their smaller lists use available space. Existing spell UI is unchanged.
- Source-integrity check compared all DND `@page` declarations against HEAD: all 14 routes preserved. No old Directory symbols/imports remain anywhere in production Razor/C# source.
- Web build passed after the code refactor with the same 26 pre-existing account warnings and no errors. Final incremental `dotnet build src/ConnectX.Web/ConnectX.Web.csproj --no-restore -v quiet` passed with 0 warnings and 0 errors. `git diff --check` passed.
- Browser verified Catalog initial eight cards, trimmed mixed-case query `  sPeLlS  ` yielding one result, zero-result recovery, restoration of all eight cards, reset focus on `catalog-search`, and Tab focus to the Classes anchor with a solid focus outline.
- Browser measured no horizontal overflow at 320, 390, 768, 1024, 1440, and 1920px. Columns were 1, 1, 2, 2, 3, and 3 respectively. Phone and desktop screenshots were inspected.
- Browser followed Catalog → Classes → Catalog, Catalog → Skills & Spells, DND → Codex, and Codex → Quests → Codex. Verified DND zero-results/reset (focus restored), Codex trimmed mixed-case search (`  qUeStS  ` yielding one result), and existing spell-preview rendering. Other placeholder routes were compiled and checked for preserved declarations, not individually exercised in the browser.
- Temporary isolated host: `/tmp/connectx-dnd-preview`, listening on loopback port 5287 for review. Its static-asset setup required a compiled assembly reference instead of a Web project reference to avoid duplicate framework assets. No preview harness files were added to the repository. Its package-audit fetch could not reach NuGet; the production Web build itself needed no restore.
- Accepted screenshots: `/tmp/connectx-dnd-screenshots/catalog-desktop.jpg` and `/tmp/connectx-dnd-screenshots/catalog-mobile.jpg`. Browser viewport override reset after responsive checks. Preview tab left available.
- No deployment, publication, database migration, new package dependency, or unrelated product change performed. Changes remain uncommitted.
