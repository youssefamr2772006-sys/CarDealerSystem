# Car Dealer Management System — Design System

**Version 1.0 · Internal use · Safa Soft**

This document is the single source of truth for the visual and interaction design of the internal **Car Dealer Management System** (inventory, customers, suppliers, employees, sales, branches).

It is a direct extension of the **Safa Soft CRM Design System**: the same brand palette, the same Cairo type scale, the same 8px spatial grid, and the same component anatomy. Nothing in this document contradicts the CRM. What is added here are the patterns the CRM does not cover — vehicle inventory, VIN and specification data, stock status, and deal-oriented sales screens.

**Language and direction:** the interface is **English only, LTR only**. No RTL mirroring, no bilingual labels.

---

## 1. Design principles

The product is used all day by showroom staff who type specifications, look up stock, and close deals under time pressure. Every decision below serves that.

| Principle | What it means in practice |
| :--- | :--- |
| **Data first, chrome second** | The screen belongs to the table, the specification sheet, and the price. Decoration never competes with data. |
| **One accent, used sparingly** | Gold marks the single primary action on a screen. If two things are gold, neither reads as primary. |
| **Colour carries meaning, not mood** | Green, amber, and red only ever mean success, attention, and destruction/error. Never use a semantic colour for branding. |
| **Predictable placement** | Page title top-left, primary action top-right, filters above the table, row actions at the row end. The same on every screen. |
| **Density with air** | Tables are compact so more stock fits on screen; the space around them stays generous (32px page padding, 24px between sections). |
| **Quiet motion** | 150–200ms transitions on hover, focus, and disclosure. Nothing bounces, nothing slides across the page. |

---

## 2. Colour palette

### 2.1 Brand core

Inherited from the CRM without modification. These two colours identify the product.

| Token | Hex | Role |
| :--- | :--- | :--- |
| `--navy` | `#0B2545` | Primary brand colour. Sidebar, dark headers, table headers, page titles, secondary buttons. |
| `--navy-hover` | `#163761` | Hover state for navy surfaces and buttons. |
| `--navy-800` | `#002B49` | Deepest navy. Sidebar footer, active navigation background. |
| `--gold` | `#E2C89B` | Secondary accent. Primary call-to-action, active tab underline, selected state, key metric highlight. |
| `--gold-hover` | `#D4B785` | Hover state for gold surfaces. |
| `--gold-soft` | `#F6EEE0` | Gold at low intensity. Selected table row, highlighted specification field. |

### 2.2 Neutrals

| Token | Hex | Role |
| :--- | :--- | :--- |
| `--bg-app` | `#F8FAFC` | Application background behind all content. |
| `--bg-surface` | `#FFFFFF` | Cards, inputs, table rows, modals. |
| `--bg-subtle` | `#F1F5F9` | Table header on light tables, icon badges, disabled fields, zebra rows. |
| `--text-primary` | `#1A202C` | Headings, table values, vehicle names, prices. |
| `--text-secondary` | `#475569` | Supporting text, secondary specification values. |
| `--text-muted` | `#64748B` | Labels, helper text, timestamps, placeholder-adjacent copy. |
| `--text-placeholder` | `#94A3B8` | Input placeholders only. |
| `--text-on-dark` | `#FFFFFF` | Text on navy surfaces. |
| `--border` | `#E2E8F0` | Default border and divider. |
| `--border-strong` | `#CBD5E1` | Input borders on hover, table outer border. |

### 2.3 Semantic colours

| Token | Hex | Meaning | Tint (background) |
| :--- | :--- | :--- | :--- |
| `--success` | `#10B981` | Saved, confirmed, available, paid | `--success-tint` `#ECFDF5` |
| `--success-strong` | `#00875A` | Success text on tint (contrast-safe) | — |
| `--warning` | `#F59E0B` | Needs attention, reserved, pending, low stock | `--warning-tint` `#FFFBEB` |
| `--danger` | `#EF4444` | Delete, validation error, overdue | `--danger-tint` `#FEF2F2` |
| `--info` | `#0EA5E9` | Neutral information, in transit, in preparation | `--info-tint` `#F0F9FF` |

> **Rule:** a semantic colour at full strength is only used for icons, borders, dots, and solid buttons. For text on a tinted background, use the `-strong` variant or `--text-primary` — never full-strength `--warning` as body text.

### 2.4 Inventory status colours

Vehicle stock status is the most scanned value in the system, so it has a fixed, learnable colour map. Status is always rendered as a **badge with a leading dot** (see §6.6) — never as coloured text alone, and never as a coloured row.

| Status | Dot / border | Background | Label text |
| :--- | :--- | :--- | :--- |
| **Available** | `#10B981` | `#ECFDF5` | `#065F46` |
| **Reserved** | `#F59E0B` | `#FFFBEB` | `#92400E` |
| **In preparation** | `#0EA5E9` | `#F0F9FF` | `#075985` |
| **Sold** | `#0B2545` | `#EEF2F7` | `#0B2545` |
| **Archived** | `#94A3B8` | `#F1F5F9` | `#475569` |

---

## 3. Typography

**Typeface:** `Cairo` (Google Fonts), weights 400 / 500 / 600 / 700. Loaded once, self-hosted or via Google Fonts, with a system fallback stack.

```css
--font-family: 'Cairo', -apple-system, 'Segoe UI', Roboto, sans-serif;
--font-mono: 'JetBrains Mono', 'SFMono-Regular', Consolas, monospace;
```

**Monospace is used for one purpose only:** identifiers that are read character by character — VIN, chassis number, engine number, plate number, invoice number. Everything else is Cairo.

### 3.1 Weights

| Weight | Use |
| :--- | :--- |
| **700 Bold** | Page titles, dashboard metric values, modal titles. |
| **600 SemiBold** | Section headers, card titles, table column headers, active tab, button labels. |
| **500 Medium** | Input labels, sidebar items, badges, secondary buttons, pagination. |
| **400 Regular** | Body text, table cell values, helper text, descriptions. |

### 3.2 Scale

| Style | Size | Line height | Weight | Use |
| :--- | :--- | :--- | :--- | :--- |
| Display | `32px` | `1.2` | 700 | Dashboard metric values (total stock, monthly sales, revenue). |
| H1 — Page title | `24px` | `1.3` | 700 | "Vehicle Inventory", "Add New Vehicle". |
| H2 — Section header | `20px` | `1.4` | 600 | "Specifications", "Pricing", "Ownership history". |
| H3 — Card header | `16px` | `1.4` | 600 | Card and panel titles. |
| Body | `14px` | `1.5` | 400 | Default text and all table values. |
| Body strong | `14px` | `1.5` | 600 | Vehicle name in a table row, totals, emphasised values. |
| Label | `13px` | `1.4` | 500 | Input labels, table column headers, badges. |
| Helper | `12px` | `1.4` | 400 | Field hints, "Last updated" text, empty-state copy. |
| Micro | `11px` | `1.3` | 500 | Uppercase eyebrows and column group labels, letter-spacing `0.04em`. |

### 3.3 Numeric and identifier formatting

Consistency here prevents costly misreading.

| Data | Format | Example |
| :--- | :--- | :--- |
| Price | Thousands separator, currency code after the amount, no decimals unless present | `1,450,000 EGP` |
| Mileage | Thousands separator + unit | `48,500 km` |
| Year | Four digits, no separator | `2023` |
| VIN / chassis | Monospace, uppercase, `letter-spacing: 0.02em` | `WDD2130421A123456` |
| Engine capacity | One decimal + unit | `2.0 L` |
| Date | `DD MMM YYYY` — never numeric-only, to avoid ambiguity | `14 Mar 2026` |
| Date + time | `DD MMM YYYY · HH:mm` (24h) | `14 Mar 2026 · 16:40` |

All numeric columns in tables are **right-aligned**; text columns are left-aligned; status and action columns are centred.

---

## 4. Spacing and layout

### 4.1 Grid base

An **8px base grid**. Permitted spacing values only:

```
4 · 8 · 12 · 16 · 20 · 24 · 32 · 40 · 48 · 64
```

`4px` is reserved for tight relationships only (icon-to-label, dot-to-badge-text).

| Token | Value |
| :--- | :--- |
| `--space-1` | `4px` |
| `--space-2` | `8px` |
| `--space-3` | `12px` |
| `--space-4` | `16px` |
| `--space-5` | `20px` |
| `--space-6` | `24px` |
| `--space-8` | `32px` |
| `--space-10` | `40px` |
| `--space-12` | `48px` |

### 4.2 Application shell

```
┌──────────────────────────────────────────────────────────────────────┐
│  TOP BAR — 72px, white, bottom border                                │
│  [logo]        [ global search ]        [branch ▾] [bell] [avatar ▾]  │
├────────────┬─────────────────────────────────────────────────────────┤
│ SIDEBAR    │  PAGE HEADER — title, breadcrumb, primary action        │
│ 260px      │  ───────────────────────────────────────────────────    │
│ navy       │  FILTER BAR — search, selects, reset                    │
│            │                                                         │
│ Dashboard  │  ┌───────────────────────────────────────────────────┐  │
│ Inventory  │  │  CONTENT — table / form / detail panels           │  │
│ Sales      │  │                                                   │  │
│ Customers  │  └───────────────────────────────────────────────────┘  │
│ Suppliers  │                                                         │
│ Employees  │  Page padding: 32px · Section gap: 32px · Card gap: 24px │
│ Branches   │                                                         │
│ Reports    │                                                         │
└────────────┴─────────────────────────────────────────────────────────┘
```

**Top bar**
- Height `72px`, background `--bg-surface`, `border-bottom: 1px solid --border`.
- Contents left to right: logo (max height `32px`), global search (`max-width: 420px`, placeholder `Search vehicles, customers, invoices`), branch selector, notifications, user menu.
- Position `sticky`, `top: 0`, `z-index: 100`.

**Sidebar**
- Width `260px` expanded, `80px` collapsed (icons only, label on hover tooltip).
- Background `--navy`, item text `rgba(255,255,255,0.78)`, padding `24px 16px`.
- Item height `44px`, radius `10px`, icon `20px`, gap `12px`, font `14px / 500`.
- **Hover:** background `rgba(255,255,255,0.08)`, text `#FFFFFF`.
- **Active:** background `rgba(226,200,155,0.14)`, text `#FFFFFF`, `3px` gold bar on the left edge, icon in `--gold`.
- Group labels: Micro style, `rgba(255,255,255,0.45)`, `24px` top margin.
- Full height, `position: fixed`, own scroll, `z-index: 90`.

**Main content**
- Background `--bg-app`, padding `32px`, `max-width: 1440px` for form and detail pages; full width for tables.

**Page header**
- Breadcrumb (Helper style, `--text-muted`) → page title (H1) on the left; primary action button on the right, vertically centred with the title. `24px` bottom margin.

### 4.3 Responsive breakpoints

| Breakpoint | Behaviour |
| :--- | :--- |
| `≥ 1440px` | Sidebar expanded. Dashboard 4 columns. Forms 3 columns. |
| `1200–1439px` | Sidebar expanded. Dashboard 4 columns. Forms 2 columns. |
| `992–1199px` | Sidebar collapsed to `80px`. Dashboard 2 columns. Forms 2 columns. |
| `768–991px` | Sidebar becomes an overlay drawer. Dashboard 2 columns. Forms 1 column. Tables scroll horizontally with the first column pinned. |
| `< 768px` | Drawer sidebar. Everything single column. Tables become stacked row cards (§6.5). Page padding `16px`. |

---

## 5. Radii, borders, shadows

```css
--radius-sm:   6px;   /* badges, small tags, checkboxes */
--radius-md:  10px;   /* buttons, inputs, selects */
--radius-lg:  16px;   /* cards, panels, table container */
--radius-xl:  24px;   /* modals, wide banners */
--radius-full: 9999px;/* pills, avatars, status chips */

--border-width: 1px;
--border-width-focus: 1.5px;

--shadow-sm: 0 1px 3px rgba(0,0,0,0.05);                /* resting cards */
--shadow-md: 0 4px 12px rgba(0,0,0,0.06);               /* dropdowns, popovers */
--shadow-lg: 0 10px 25px rgba(11,37,69,0.08);           /* modals, drawers */
--shadow-focus-navy: 0 0 0 3px rgba(11,37,69,0.10);
--shadow-focus-gold: 0 0 0 3px rgba(226,200,155,0.40);
--shadow-focus-danger: 0 0 0 3px rgba(239,68,68,0.12);
```

Cards use a border **or** a shadow, never both at full strength: `1px solid --border` plus `--shadow-sm` is the standard resting card.

---

## 6. Components

### 6.1 Buttons

Height `44px` standard, `36px` small (table rows, filter bars), `52px` large (single-action forms and modals). Radius `--radius-md`. Font `14px / 600`. Padding `12px 24px` (`8px 16px` small). Icon `18px`, gap `8px`. Transition `all 150ms ease`.

| Variant | Background | Text | Border | Hover | Use |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **Primary** | `--gold` | `--navy` | none | `--gold-hover` + `--shadow-md` | The one main action per screen: Save vehicle, Create invoice, Add customer. |
| **Secondary** | `--navy` | `#FFFFFF` | none | `--navy-hover` | Supporting action: Print, Export, Assign. |
| **Outline** | transparent | `--navy` | `1px solid --border-strong` | background `--bg-subtle` | Tertiary action: Back, Duplicate, Filters. |
| **Ghost** | transparent | `--text-secondary` | none | background `--bg-subtle` | Row actions, icon-only actions, Cancel in toolbars. |
| **Success** | `--success` | `#FFFFFF` | none | `#059669` | Confirmed state transitions: Mark as sold, Confirm payment. |
| **Danger** | `--danger` | `#FFFFFF` | none | `#DC2626` | Destructive confirmation inside a modal only. |
| **Danger ghost** | transparent | `--danger` | none | background `--danger-tint` | Delete trigger in a table row or detail header. |

**States (all variants)**
- **Focus:** `outline: 2px solid --navy; outline-offset: 2px` — always visible for keyboard users.
- **Disabled:** `opacity: 0.5; cursor: not-allowed`, no hover change.
- **Loading:** label replaced with `16px` spinner + original label dimmed; button stays the same width to prevent layout shift.

**Rules**
- One Primary button per screen region. A form footer has exactly one gold button.
- Destructive actions are never the primary button on a page. Delete always goes through a confirmation modal, and only the modal's confirm button is red.
- Icon-only buttons are `40 × 40px`, radius `--radius-md`, and always carry a `title` and `aria-label`.

### 6.2 Inputs and form controls

| Property | Value |
| :--- | :--- |
| Height | `44px` (`52px` large, `36px` compact filter) |
| Background | `--bg-surface` |
| Border | `1px solid --border` |
| Radius | `--radius-md` |
| Padding | `10px 16px` |
| Font | `14px / 400`, colour `--text-primary` |
| Placeholder | `13px`, `--text-placeholder` |
| Label | `13px / 500`, `--text-primary`, `margin-bottom: 6px` |
| Helper text | `12px / 400`, `--text-muted`, `margin-top: 6px` |

**States**

| State | Style |
| :--- | :--- |
| Hover | `border-color: --border-strong` |
| Focus | `border: 1.5px solid --navy` + `--shadow-focus-navy` |
| Filled | no change — value colour alone signals content |
| Error | `border: 1.5px solid --danger` + `--shadow-focus-danger`; message below in `11px` `--danger` with a `14px` alert icon |
| Success | `border: 1.5px solid --success`; use only for async checks such as VIN uniqueness |
| Disabled | background `--bg-subtle`, text `--text-muted`, `cursor: not-allowed` |
| Read-only | background `--bg-subtle`, border `--border`, text `--text-primary` |

**Required fields** carry a `*` in `--danger` after the label. Optional fields are never marked.

**Specific controls**
- **Select:** same box as a text input, `18px` chevron in `--text-muted` at the end. Menu: `--bg-surface`, `--radius-md`, `--shadow-md`, item height `40px`, hover `--bg-subtle`, selected `--gold-soft` with a `16px` navy check.
- **Searchable select** (brand, model, supplier, customer): search field pinned at the top of the menu, results virtualised past 50 items, "No matches" empty row in Helper style.
- **Number input** (price, mileage, year): right-aligned value, live thousands separators, unit shown as a static suffix inside the field on `--bg-subtle`.
- **VIN input:** monospace, `uppercase` transform, `maxlength 17`, live character counter `13/17` in Helper style at the field end.
- **Date input:** native picker, `DD MMM YYYY` display format, `18px` calendar icon at the end.
- **Textarea:** min height `96px`, `vertical` resize only, same border and radius as inputs.
- **Checkbox / radio:** `18px`, radius `4px` (checkbox) or full (radio), unchecked `1px solid --border-strong`, checked `--navy` background with a white glyph.
- **Toggle:** `40 × 22px` track, `--border-strong` off, `--success` on, white `18px` knob, `150ms` transition. Used for reversible boolean settings such as "Publish to showroom display".
- **File / image upload:** dashed `1.5px --border-strong` drop zone, radius `--radius-lg`, `--bg-subtle` background, `32px` upload icon in `--text-muted`, primary line Body strong, secondary line Helper (`JPG or PNG · up to 5 MB each · up to 12 images`). Thumbnails `96 × 72px`, radius `--radius-sm`, drag to reorder, first image marked with a gold `Cover` pill.

### 6.3 Form layout

- Group fields into cards by meaning: **Identification** (brand, model, year, VIN, plate), **Specifications** (body type, transmission, fuel, engine, colour, mileage), **Commercial** (purchase price, list price, supplier, branch, status), **Media** (photos), **Notes**.
- Card: `--bg-surface`, radius `--radius-lg`, `1px solid --border`, padding `24px`, header H3 with `20px` bottom margin.
- Grid: `repeat(3, 1fr)` at `≥1440px`, `repeat(2, 1fr)` at `1200–1439px`, single column below `768px`. Column gap `20px`, row gap `20px`.
- A field may span 2 columns (address, notes) or the full row (photos, long text).
- **Form footer:** sticky at the bottom of the viewport, `--bg-surface`, `border-top: 1px solid --border`, padding `16px 32px`, `--shadow-md` upward. Buttons right-aligned, `12px` gap, order: Outline "Cancel" → Primary "Save vehicle".
- Validation runs on blur per field and again on submit. On submit failure, focus and scroll to the first invalid field and show a `--danger-tint` summary banner at the top of the form.

### 6.4 Data tables

The primary interface of the system. One table pattern, used everywhere.

**Container:** `--bg-surface`, radius `--radius-lg`, `1px solid --border`, `--shadow-sm`, `overflow: hidden`.

**Header row**
- Background `--navy`, text `#FFFFFF`, Label style, `uppercase`, `letter-spacing: 0.04em`.
- Height `48px`, cell padding `0 16px`.
- Sortable columns show a `14px` chevron at `40%` opacity, full opacity and gold when active.
- Sticky on vertical scroll (`position: sticky; top: 72px`).

**Body rows**
- Height `56px`, background `--bg-surface`, `border-bottom: 1px solid --border`, cell padding `12px 16px`, Body style.
- **Hover:** background `--bg-subtle`.
- **Selected:** background `--gold-soft`, `3px` gold bar on the leading edge.
- Last row has no bottom border.
- No zebra striping by default; enable `--bg-subtle` on even rows only for tables wider than 9 columns.

**Column conventions for the inventory table**

| # | Column | Alignment | Content |
| :--- | :--- | :--- | :--- |
| 1 | Checkbox | centre | Bulk selection |
| 2 | Vehicle | left | `48 × 36px` thumbnail (radius `--radius-sm`) + Body strong `Brand Model` + Helper `Trim · Year` |
| 3 | VIN | left | Monospace, last 8 characters shown, full value on hover |
| 4 | Mileage | right | `48,500 km` |
| 5 | Colour | left | `12px` colour swatch (radius full, `1px solid --border`) + name |
| 6 | Branch | left | Branch name |
| 7 | List price | right | Body strong |
| 8 | Status | centre | Status badge (§6.6) |
| 9 | Actions | centre | Ghost icon buttons: View, Edit, Delete |

**Row actions:** up to three icon buttons at `36 × 36px`, `4px` gap, `18px` icons — View (`--text-secondary`), Edit (`--navy`), Delete (`--danger`). Anything beyond three collapses into a `⋯` overflow menu.

**Toolbar above the table:** `--bg-surface`, radius `--radius-lg`, padding `16px`, `16px` bottom margin. Left: search input (`320px`, compact height) plus up to four filter selects. Right: "Reset" ghost button, "Export" outline button. Active filters appear below as removable gold-outlined pills.

**Bulk action bar:** when rows are selected, the toolbar is replaced by a `--navy` bar, radius `--radius-lg`, white text: `4 vehicles selected` on the left, actions on the right (Change branch, Change status, Export, Delete).

**Pagination:** below the table, `24px` top margin. Left: `Showing 1–25 of 312 vehicles` in Helper style. Right: page-size select (`25 / 50 / 100`) and page buttons at `36 × 36px`, radius `--radius-md`; active page `--navy` with white text.

**Loading:** skeleton rows — `--bg-subtle` bars at `60%` cell width, `1.4s` shimmer, matching the real row height. Never a full-page spinner over an existing table.

**Empty states:** centred in the table container, `48px` vertical padding — `40px` icon in `--text-placeholder`, H3 title, Helper description, one Primary action.
- No data yet: `No vehicles in inventory` · `Add your first vehicle to start tracking stock.` · **Add vehicle**
- No results: `No vehicles match these filters` · `Try widening the price or year range.` · **Reset filters**

### 6.5 Cards

| Card | Anatomy |
| :--- | :--- |
| **Metric card** (dashboard) | `--bg-surface`, radius `--radius-lg`, `1px solid --border`, `--shadow-sm`, padding `24px`. `48px` icon badge (radius `--radius-md`, semantic tint background, semantic icon) top-left; Label caption in `--text-muted` `uppercase`; Display value in `--text-primary`; trend line in Helper style with a `14px` arrow — `--success` up, `--danger` down, `--text-muted` flat. Hover: `--shadow-md`, `translateY(-2px)`, `200ms`. |
| **Panel card** | Section container for forms and detail views. Header: H3 + optional right-aligned ghost action, `1px solid --border` divider, `20px` gap to content. Padding `24px`. |
| **Vehicle card** (grid view / showroom display) | `16:10` cover image, radius `--radius-lg` top corners, status badge overlaid top-right; body padding `16px`: H3 `Brand Model`, Helper `Year · Mileage · Transmission`, price in Body strong `16px` `--navy`, footer row with branch name and a Ghost "View details". Hover: `--shadow-md`, `translateY(-2px)`. |
| **Stacked row card** (mobile table replacement) | `--bg-surface`, radius `--radius-lg`, `1px solid --border`, padding `16px`. Top row: thumbnail + name + status badge. Then label/value pairs stacked with `8px` gaps, label `--text-muted` left, value `--text-primary` right. Footer: full-width Outline "View details". |

### 6.6 Badges and pills

- **Status badge:** height `24px`, radius `--radius-full`, padding `0 10px`, Label style, `6px` leading dot, colours from §2.4. Text is always a full word or phrase — never a single letter or bare colour.
- **Count badge:** `20px` minimum width, radius `--radius-full`, `--navy` background, white `11px / 600` text. Used on sidebar items and tabs.
- **Filter pill:** height `28px`, radius `--radius-full`, `1px solid --gold`, background `--gold-soft`, text `--navy` `12px / 500`, `14px` remove icon.
- **Tag:** height `24px`, radius `--radius-sm`, background `--bg-subtle`, text `--text-secondary`. Used for non-semantic attributes such as body type or fuel.

### 6.7 Tabs

Used on vehicle and customer detail pages (Overview · Specifications · Photos · Documents · History).

- Row height `48px`, `border-bottom: 1px solid --border`, `24px` gap between tabs.
- Item: optional `18px` icon + label `14px`, `500` inactive in `--text-muted`, `600` active in `--navy`.
- Active tab: `2px solid --gold` bottom border, drawn over the container border.
- Hover on inactive: text `--text-primary`.
- Optional count badge after the label.
- Below `768px` the row scrolls horizontally with hidden scrollbars.

### 6.8 Modals and drawers

**Modal**
- Width `480px` (confirm), `640px` (form), `840px` (wide). Radius `--radius-xl`, `--bg-surface`, `--shadow-lg`.
- Overlay `rgba(11,37,69,0.45)` with `2px` backdrop blur.
- Header: padding `24px`, H2 title, `20px` ghost close button top-right, `1px solid --border` divider.
- Body: padding `24px`, `max-height: 60vh`, own scroll.
- Footer: padding `20px 24px`, `border-top: 1px solid --border`, buttons right-aligned with `12px` gap.
- Entry: `opacity 0→1` and `scale 0.98→1` over `180ms ease-out`.
- **Destructive confirm:** `40px` icon badge in `--danger-tint` with a `--danger` alert icon, title naming the object (`Delete this vehicle?`), body stating the consequence and irreversibility, footer `Cancel` (Outline) + `Delete vehicle` (Danger).

**Drawer**
- Right-anchored, width `480px` (`720px` wide variant), full height, radius `--radius-xl` on the leading corners, `--shadow-lg`, slides in over `220ms ease-out`. Same header/body/footer anatomy as a modal. Used for quick edits and record previews that should not lose the table's scroll position.

### 6.9 Feedback

- **Toast:** bottom-right, width `360px`, radius `--radius-md`, `--bg-surface`, `--shadow-lg`, `4px` semantic leading border, `20px` semantic icon, Body strong title + Helper description, ghost close. Auto-dismiss after `4s` (success/info) or manual only (error). Stack upward, `12px` gap, maximum three.
- **Inline banner:** radius `--radius-md`, semantic tint background, `1px` semantic border, `20px` icon, padding `16px`. Sits at the top of a form or card to explain a page-level condition.
- **Progress bar:** `6px` height, radius `--radius-full`, track `--bg-subtle`, fill `--gold` (or semantic where meaningful).
- **Spinner:** `2px` stroke, `--navy` on light surfaces, `--gold` on navy, `16 / 20 / 32px` sizes.
- **Tooltip:** `--navy` background, white `12px` text, radius `--radius-sm`, padding `6px 10px`, `8px` offset, `300ms` delay.

---

## 7. Iconography

- **Library:** `Lucide Icons`. One library only — no mixing.
- **Style:** outline, stroke `1.75px` (`2px` at `24px` and above), `round` caps and joins.
- **Sizes:** `16px` inline with text · `18px` buttons and inputs · `20px` sidebar and toolbars · `24px` card headers · `32–40px` empty states.
- **Icon badge:** circle or `--radius-md` square, `40px` or `48px`, background `--bg-subtle` or a semantic tint, icon at `50%` of the badge size.
- Icons inherit text colour unless they carry semantic meaning.

**Domain mapping**

| Concept | Icon |
| :--- | :--- |
| Dashboard | `layout-dashboard` |
| Inventory / vehicles | `car` |
| Sales / invoices | `receipt-text` |
| Revenue | `trending-up` |
| Customers | `users` |
| Suppliers | `truck` |
| Employees | `id-card` |
| Branches | `building-2` |
| Reports | `bar-chart-3` |
| VIN / identification | `hash` |
| Mileage | `gauge` |
| Transmission | `settings-2` |
| Fuel | `fuel` |
| Colour | `palette` |
| Photos | `image` |
| Documents | `file-text` |
| History / audit | `history` |
| Test drive | `key-round` |
| Add | `plus` |
| Edit | `pencil` |
| Delete | `trash-2` |
| View | `eye` |
| Export | `download` |
| Print | `printer` |
| Filter | `sliders-horizontal` |
| Search | `search` |

---

## 8. Screen patterns

### 8.1 Dashboard

1. Page header: `Dashboard` + branch selector.
2. Metric row — 4 cards: **Total stock** (navy tint, `car`), **Available now** (success tint, `check-circle`), **Sold this month** (gold tint, `receipt-text`), **Revenue this month** (info tint, `trending-up`).
3. Two-column band, `24px` gap: sales trend chart (2/3 width) and stock by status donut (1/3 width). Chart palette: navy → gold → info → success → warning, in that order. Axes and gridlines `--border`, labels Helper style.
4. Two-column band: "Recent sales" table (5 rows, compact) and "Vehicles in preparation" list.
5. Every panel header carries a ghost "View all" link on the right.

### 8.2 Inventory list

Page header (`Vehicle Inventory`, count in Helper style under the title, Primary **Add vehicle**) → view switch (Table / Grid, segmented Outline control) → toolbar with search and filters (Brand, Model, Status, Branch, Year range, Price range) → table or card grid → pagination.

### 8.3 Vehicle detail

- Header band: `--bg-surface` panel, radius `--radius-lg`, padding `24px` — cover image `240 × 180px` (radius `--radius-md`) on the left; on the right: H1 `Brand Model Year`, VIN in monospace Helper style, status badge, list price in Display style, and an action row (Primary **Create invoice**, Secondary **Edit**, Outline **Print sheet**, Danger ghost **Delete**).
- Tabs below: Overview · Specifications · Photos · Documents · History.
- **Specifications** render as a two-column definition grid: label `13px / 500 --text-muted` left, value `14px / 400 --text-primary` right, `1px solid --border` divider between rows, row height `44px`.
- **History** is a vertical timeline: `8px` navy dot on a `1px --border` rail, `24px` left offset, event title Body strong, actor and timestamp in Helper style.

### 8.4 Sales / invoice

- Two-column layout: left (2/3) the line-item builder — vehicle picker, price, discount, fees, taxes; right (1/3) a sticky summary card in `--bg-subtle`, radius `--radius-lg`, showing subtotal, discount, tax, and a total in Display style with a `--gold` top border above it.
- Payment status uses the semantic set: Paid (success), Partially paid (warning), Unpaid (danger).
- Footer: Outline **Save draft** + Primary **Issue invoice**.

### 8.5 People (customers, suppliers, employees)

Same list-and-detail pattern as inventory. Avatars are `40px` circles; when no photo exists, use initials on `--navy` with white `14px / 600` text. Detail pages use the tab set Overview · Purchases · Documents · Notes.

---

## 9. Accessibility

- Body text and UI labels meet **WCAG AA** contrast (4.5:1); large text and non-text elements meet 3:1.
- Gold `#E2C89B` is a **light** colour: only navy or `--text-primary` may sit on it. Never white text on gold.
- Focus is always visible: `2px` navy outline with `2px` offset, or the input focus ring. Focus styles are never removed.
- Colour is never the only carrier of meaning — status uses a dot plus a word, errors use an icon plus a message, chart series carry direct labels.
- All interactive targets are at least `36 × 36px`; `44px` on touch layouts.
- Tables use proper `<th scope>`, modals trap focus and close on `Esc`, toasts announce via `aria-live="polite"` (`assertive` for errors).
- `prefers-reduced-motion: reduce` disables transforms and shimmer, keeping opacity changes only.

---

## 10. Voice and microcopy

- **Sentence case everywhere** except Micro eyebrows and table headers (uppercase) — no Title Case buttons.
- Buttons name the outcome: `Save vehicle`, `Issue invoice`, `Add customer` — not `Submit`, `OK`, `Proceed`.
- An action keeps the same name through the flow: the `Save vehicle` button produces the toast `Vehicle saved`.
- Errors state what happened and how to fix it, without apologising: `This VIN already exists on stock #1043. Enter a different VIN.`
- Empty screens are invitations, not dead ends: a title, one sentence of direction, one action.
- Use showroom vocabulary, not database vocabulary: *vehicle*, *stock*, *branch*, *supplier*, *invoice* — never *record*, *entity*, *row*, *entry*.
- Never use "please" in interface copy, and never use exclamation marks.

---

## 11. CSS token reference

Drop this into the global stylesheet. All component CSS derives from these values — no raw hex codes anywhere else in the codebase.

```css
:root {
  /* ── Brand ─────────────────────────────────────── */
  --navy:            #0B2545;
  --navy-hover:      #163761;
  --navy-800:        #002B49;
  --gold:            #E2C89B;
  --gold-hover:      #D4B785;
  --gold-soft:       #F6EEE0;

  /* ── Neutrals ──────────────────────────────────── */
  --bg-app:          #F8FAFC;
  --bg-surface:      #FFFFFF;
  --bg-subtle:       #F1F5F9;
  --text-primary:    #1A202C;
  --text-secondary:  #475569;
  --text-muted:      #64748B;
  --text-placeholder:#94A3B8;
  --text-on-dark:    #FFFFFF;
  --border:          #E2E8F0;
  --border-strong:   #CBD5E1;

  /* ── Semantic ──────────────────────────────────── */
  --success:         #10B981;
  --success-strong:  #00875A;
  --success-tint:    #ECFDF5;
  --warning:         #F59E0B;
  --warning-tint:    #FFFBEB;
  --danger:          #EF4444;
  --danger-tint:     #FEF2F2;
  --info:            #0EA5E9;
  --info-tint:       #F0F9FF;

  /* ── Inventory status ──────────────────────────── */
  --status-available-dot:  #10B981;
  --status-available-bg:   #ECFDF5;
  --status-available-text: #065F46;
  --status-reserved-dot:   #F59E0B;
  --status-reserved-bg:    #FFFBEB;
  --status-reserved-text:  #92400E;
  --status-prep-dot:       #0EA5E9;
  --status-prep-bg:        #F0F9FF;
  --status-prep-text:      #075985;
  --status-sold-dot:       #0B2545;
  --status-sold-bg:        #EEF2F7;
  --status-sold-text:      #0B2545;
  --status-archived-dot:   #94A3B8;
  --status-archived-bg:    #F1F5F9;
  --status-archived-text:  #475569;

  /* ── Typography ────────────────────────────────── */
  --font-family: 'Cairo', -apple-system, 'Segoe UI', Roboto, sans-serif;
  --font-mono:   'JetBrains Mono', 'SFMono-Regular', Consolas, monospace;
  --fs-display: 32px;  --lh-display: 1.2;
  --fs-h1:      24px;  --lh-h1:      1.3;
  --fs-h2:      20px;  --lh-h2:      1.4;
  --fs-h3:      16px;  --lh-h3:      1.4;
  --fs-body:    14px;  --lh-body:    1.5;
  --fs-label:   13px;  --lh-label:   1.4;
  --fs-helper:  12px;  --lh-helper:  1.4;
  --fs-micro:   11px;  --lh-micro:   1.3;
  --fw-regular: 400;
  --fw-medium:  500;
  --fw-semibold:600;
  --fw-bold:    700;

  /* ── Spacing (8px base) ────────────────────────── */
  --space-1: 4px;  --space-2: 8px;  --space-3: 12px;
  --space-4: 16px; --space-5: 20px; --space-6: 24px;
  --space-8: 32px; --space-10: 40px; --space-12: 48px;

  /* ── Radii ─────────────────────────────────────── */
  --radius-sm:   6px;
  --radius-md:  10px;
  --radius-lg:  16px;
  --radius-xl:  24px;
  --radius-full: 9999px;

  /* ── Shadows ───────────────────────────────────── */
  --shadow-sm: 0 1px 3px rgba(0,0,0,0.05);
  --shadow-md: 0 4px 12px rgba(0,0,0,0.06);
  --shadow-lg: 0 10px 25px rgba(11,37,69,0.08);
  --shadow-focus-navy:   0 0 0 3px rgba(11,37,69,0.10);
  --shadow-focus-gold:   0 0 0 3px rgba(226,200,155,0.40);
  --shadow-focus-danger: 0 0 0 3px rgba(239,68,68,0.12);

  /* ── Layout ────────────────────────────────────── */
  --topbar-height:      72px;
  --sidebar-width:     260px;
  --sidebar-collapsed:  80px;
  --page-padding:       32px;
  --content-max-width:1440px;
  --control-height:     44px;
  --control-height-sm:  36px;
  --control-height-lg:  52px;
  --table-header-height:48px;
  --table-row-height:   56px;

  /* ── Motion ────────────────────────────────────── */
  --ease:       cubic-bezier(0.4, 0, 0.2, 1);
  --duration-fast: 150ms;
  --duration-base: 200ms;
  --duration-slow: 300ms;
}
```

### Reference component CSS

```css
/* Base */
body {
  font-family: var(--font-family);
  font-size: var(--fs-body);
  line-height: var(--lh-body);
  color: var(--text-primary);
  background: var(--bg-app);
}

/* Buttons */
.btn {
  display: inline-flex; align-items: center; justify-content: center;
  gap: var(--space-2);
  height: var(--control-height);
  padding: 0 var(--space-6);
  border: none; border-radius: var(--radius-md);
  font: var(--fw-semibold) var(--fs-body)/1 var(--font-family);
  cursor: pointer;
  transition: all var(--duration-fast) var(--ease);
}
.btn:focus-visible { outline: 2px solid var(--navy); outline-offset: 2px; }
.btn:disabled { opacity: .5; cursor: not-allowed; }

.btn--primary   { background: var(--gold); color: var(--navy); }
.btn--primary:hover:not(:disabled) { background: var(--gold-hover); box-shadow: var(--shadow-md); }
.btn--secondary { background: var(--navy); color: var(--text-on-dark); }
.btn--secondary:hover:not(:disabled) { background: var(--navy-hover); }
.btn--outline   { background: transparent; color: var(--navy); border: 1px solid var(--border-strong); }
.btn--outline:hover:not(:disabled) { background: var(--bg-subtle); }
.btn--ghost     { background: transparent; color: var(--text-secondary); }
.btn--ghost:hover:not(:disabled) { background: var(--bg-subtle); color: var(--text-primary); }
.btn--danger    { background: var(--danger); color: #fff; }
.btn--danger:hover:not(:disabled) { background: #DC2626; }
.btn--sm { height: var(--control-height-sm); padding: 0 var(--space-4); font-size: var(--fs-label); }

/* Inputs */
.field { display: flex; flex-direction: column; }
.field__label {
  font-size: var(--fs-label); font-weight: var(--fw-medium);
  color: var(--text-primary); margin-bottom: 6px;
}
.field__label .req { color: var(--danger); margin-inline-start: 2px; }
.input {
  height: var(--control-height);
  padding: 10px var(--space-4);
  background: var(--bg-surface);
  border: 1px solid var(--border);
  border-radius: var(--radius-md);
  font: var(--fw-regular) var(--fs-body)/1.5 var(--font-family);
  color: var(--text-primary);
  transition: border-color var(--duration-fast) var(--ease),
              box-shadow var(--duration-fast) var(--ease);
}
.input::placeholder { color: var(--text-placeholder); font-size: var(--fs-label); }
.input:hover:not(:disabled) { border-color: var(--border-strong); }
.input:focus { outline: none; border: 1.5px solid var(--navy); box-shadow: var(--shadow-focus-navy); }
.input--error { border: 1.5px solid var(--danger); box-shadow: var(--shadow-focus-danger); }
.input--mono  { font-family: var(--font-mono); text-transform: uppercase; letter-spacing: .02em; }
.input:disabled, .input[readonly] { background: var(--bg-subtle); color: var(--text-muted); }
.field__error {
  margin-top: 6px; font-size: var(--fs-micro); color: var(--danger);
  display: flex; align-items: center; gap: var(--space-1);
}

/* Card */
.card {
  background: var(--bg-surface);
  border: 1px solid var(--border);
  border-radius: var(--radius-lg);
  box-shadow: var(--shadow-sm);
  padding: var(--space-6);
}

/* Table */
.table-wrap {
  background: var(--bg-surface);
  border: 1px solid var(--border);
  border-radius: var(--radius-lg);
  box-shadow: var(--shadow-sm);
  overflow: hidden;
}
.table { width: 100%; border-collapse: collapse; }
.table thead th {
  height: var(--table-header-height);
  padding: 0 var(--space-4);
  background: var(--navy);
  color: var(--text-on-dark);
  font-size: var(--fs-label); font-weight: var(--fw-semibold);
  text-transform: uppercase; letter-spacing: .04em;
  text-align: left; white-space: nowrap;
  position: sticky; top: var(--topbar-height); z-index: 2;
}
.table tbody td {
  height: var(--table-row-height);
  padding: var(--space-3) var(--space-4);
  border-bottom: 1px solid var(--border);
  vertical-align: middle;
}
.table tbody tr:last-child td { border-bottom: none; }
.table tbody tr:hover { background: var(--bg-subtle); }
.table tbody tr[aria-selected="true"] {
  background: var(--gold-soft);
  box-shadow: inset 3px 0 0 var(--gold);
}
.table .num  { text-align: right; font-variant-numeric: tabular-nums; }
.table .mono { font-family: var(--font-mono); letter-spacing: .02em; }

/* Status badge */
.badge {
  display: inline-flex; align-items: center; gap: 6px;
  height: 24px; padding: 0 10px;
  border-radius: var(--radius-full);
  font-size: var(--fs-label); font-weight: var(--fw-medium);
  white-space: nowrap;
}
.badge::before { content: ""; width: 6px; height: 6px; border-radius: 50%; background: currentColor; }
.badge--available { background: var(--status-available-bg); color: var(--status-available-text); }
.badge--reserved  { background: var(--status-reserved-bg);  color: var(--status-reserved-text); }
.badge--prep      { background: var(--status-prep-bg);      color: var(--status-prep-text); }
.badge--sold      { background: var(--status-sold-bg);      color: var(--status-sold-text); }
.badge--archived  { background: var(--status-archived-bg);  color: var(--status-archived-text); }

/* Sidebar item */
.nav-item {
  display: flex; align-items: center; gap: var(--space-3);
  height: 44px; padding: 0 var(--space-3);
  border-radius: var(--radius-md);
  color: rgba(255,255,255,.78);
  font-size: var(--fs-body); font-weight: var(--fw-medium);
  text-decoration: none;
  transition: background var(--duration-fast) var(--ease);
}
.nav-item:hover { background: rgba(255,255,255,.08); color: #fff; }
.nav-item--active {
  background: rgba(226,200,155,.14); color: #fff;
  box-shadow: inset 3px 0 0 var(--gold);
}
.nav-item--active svg { color: var(--gold); }

@media (prefers-reduced-motion: reduce) {
  * { animation: none !important; transition-duration: .01ms !important; }
}
```

---

## 12. Rules and anti-patterns

**Always**
- Use tokens. A raw hex value in a component file is a bug.
- One gold Primary button per screen region.
- Status as badge + word. Prices right-aligned with a thousands separator. VIN in monospace.
- Confirm every destructive action in a modal that names the object.
- Keep the page header, filter bar, table, and pagination in the same order on every list screen.

**Never**
- White text on gold, or gold text on white.
- More than one icon library, or more than one typeface for UI text.
- Spacing values outside the 8px scale.
- Coloured table rows to indicate status.
- Removed focus outlines.
- Semantic colours used decoratively, or brand colours used to signal state.
- Full-page spinners where a skeleton would preserve context.

---

*Maintained by Safa Soft · Derived from the Safa Soft CRM Design System · Any change to a token must be applied in both systems.*
