---
name: Brew & Dream
colors:
  surface: '#fff8f2'
  surface-dim: '#e2d9cb'
  surface-bright: '#fff8f2'
  surface-container-lowest: '#ffffff'
  surface-container-low: '#fcf2e4'
  surface-container: '#f7edde'
  surface-container-high: '#f1e7d9'
  surface-container-highest: '#ebe1d3'
  on-surface: '#1f1b13'
  on-surface-variant: '#51443d'
  inverse-surface: '#353027'
  inverse-on-surface: '#f9efe1'
  outline: '#83746c'
  outline-variant: '#d5c3b9'
  surface-tint: '#805437'
  primary: '#502c12'
  on-primary: '#ffffff'
  primary-container: '#6b4226'
  on-primary-container: '#e9b08c'
  inverse-primary: '#f4ba96'
  secondary: '#39693b'
  on-secondary: '#ffffff'
  secondary-container: '#b8edb3'
  on-secondary-container: '#3e6d3e'
  tertiary: '#5a2600'
  on-tertiary: '#ffffff'
  tertiary-container: '#7b3904'
  on-tertiary-container: '#ffa873'
  error: '#ba1a1a'
  on-error: '#ffffff'
  error-container: '#ffdad6'
  on-error-container: '#93000a'
  primary-fixed: '#ffdbc7'
  primary-fixed-dim: '#f4ba96'
  on-primary-fixed: '#311300'
  on-primary-fixed-variant: '#653d22'
  secondary-fixed: '#baf0b6'
  secondary-fixed-dim: '#9fd49b'
  on-secondary-fixed: '#002105'
  on-secondary-fixed-variant: '#215025'
  tertiary-fixed: '#ffdbc9'
  tertiary-fixed-dim: '#ffb68c'
  on-tertiary-fixed: '#321200'
  on-tertiary-fixed-variant: '#753400'
  background: '#fff8f2'
  on-background: '#1f1b13'
  surface-variant: '#ebe1d3'
  espresso-dark: '#2C1810'
  milk-foam: '#FAF6F0'
  amber-honey: '#E5A93C'
  danger-red: '#C84B31'
typography:
  display-hero:
    fontFamily: Quicksand
    fontSize: 48px
    fontWeight: '700'
    lineHeight: 56px
    letterSpacing: -0.02em
  headline-lg:
    fontFamily: Quicksand
    fontSize: 32px
    fontWeight: '700'
    lineHeight: 40px
    letterSpacing: -0.01em
  headline-md:
    fontFamily: Quicksand
    fontSize: 24px
    fontWeight: '600'
    lineHeight: 32px
  headline-sm:
    fontFamily: Quicksand
    fontSize: 20px
    fontWeight: '600'
    lineHeight: 28px
  title-md:
    fontFamily: Be Vietnam Pro
    fontSize: 16px
    fontWeight: '600'
    lineHeight: 24px
  title-sm:
    fontFamily: Be Vietnam Pro
    fontSize: 14px
    fontWeight: '600'
    lineHeight: 20px
  body-lg:
    fontFamily: Be Vietnam Pro
    fontSize: 16px
    fontWeight: '400'
    lineHeight: 24px
  body-md:
    fontFamily: Be Vietnam Pro
    fontSize: 14px
    fontWeight: '400'
    lineHeight: 20px
  body-sm:
    fontFamily: Be Vietnam Pro
    fontSize: 12px
    fontWeight: '400'
    lineHeight: 16px
  label-lg:
    fontFamily: Be Vietnam Pro
    fontSize: 14px
    fontWeight: '500'
    lineHeight: 20px
    letterSpacing: 0.01em
  label-md:
    fontFamily: Be Vietnam Pro
    fontSize: 12px
    fontWeight: '500'
    lineHeight: 16px
    letterSpacing: 0.02em
  label-sm:
    fontFamily: Be Vietnam Pro
    fontSize: 10px
    fontWeight: '600'
    lineHeight: 14px
    letterSpacing: 0.04em
rounded:
  sm: 0.25rem
  DEFAULT: 0.5rem
  md: 0.75rem
  lg: 1rem
  xl: 1.5rem
  full: 9999px
spacing:
  gutter: 1rem
  gutter-lg: 1.5rem
  margin: 1.5rem
  margin-edge: 2rem
  space-xs: 0.25rem
  space-sm: 0.5rem
  space-md: 1rem
  space-lg: 1.5rem
  space-xl: 2rem
---

## Brand & Style

The design system establishes a tactile, warm craft aesthetic tailored for a cozy Vietnamese coffee shop and business simulation. It balances nostalgic artisanal Vietnamese street culture (wooden counters, craft paper receipts, warm lanterns, lush sidewalk greenery) with the functional precision required for tycoon gameplay overlays, resource management tables, and high-frequency point-of-sale interactions.

### Emotional Target
The interface evokes comfort, mindful entrepreneurship, and culinary warmth. Interactions should feel organic, tactile, and forgiving—inviting players into slow-paced yet engaging management loops without visual fatigue or clinical spreadsheet anxiety.

### Design Movement: Tactile Craft Minimalist
Drawing from skeuomorphic warmth and clean modern layout design, the visual language relies on:
- Warm paperboard surfaces and rich espresso framing.
- Tactile wooden button blocks with subtle beveled undersides rather than harsh digital gloss.
- Crisp vector line illustrations paired with natural, grounding colors.
- Expressive Vietnamese typography with diacritics integrated cleanly into tight game HUD constraints.

## Colors

The palette derives strictly from raw beverage elements, street-side materials, and natural vegetation. It operates in a persistent warm daylight mode to retain the cozy ambiance across all game states.

### Core Roles
- **Primary (`#6B4226` · Roasted Coffee Brown):** Structural headers, primary panel borders, heavy text glyphs, and foundational frames.
- **Secondary (`#5B8C5A` · Leaf Green):** Positive indicators, safe patience levels on customer order timers, growth metrics, and high satisfaction ratings.
- **Tertiary (`#D9824B` · Terracotta Accent):** High-urgency game decisions, primary bottom action buttons (`Thanh toán`, `Bắt đầu kinh doanh`), active selection rings, and low-inventory alert banners.
- **Neutral (`#F5EBDD` · Cream Base):** Base game screen canvas, background card backings, and root window surfaces.

### Supplementary Named Roles
- **`espresso-dark` (`#2C1810`):** Ultra-high-contrast body text, dialogue text boxes, and bottom panel anchor bars.
- **`milk-foam` (`#FAF6F0`):** Elevated inner card containers, text input surfaces, and receipt order chits.
- **`amber-honey` (`#E5A93C`):** Currency badges (VND), star rating fills, and golden milestone rewards.
- **`danger-red` (`#C84B31`):** Urgent warnings, patience expiration warnings, and negative profit KPI numbers.

### Dynamic Timer State Rules
Customer patience bars transition from `leaf-green` (`#5B8C5A`) at 100%–50% to `amber-honey` (`#E5A93C`) at 50%–20%, finally shifting to `danger-red` (`#C84B31`) below 20%.

## Typography

Typography prioritizes legibility for Vietnamese tone marks while delivering an approachable, cozy silhouette. 

### Font Roles
- **Headlines & Big Titles (`Quicksand`):** Rounded geometric sans-serif that softens bold statements, game titles, screen names, and modal headlines. Provides welcoming curves to high-impact UI elements.
- **Body & Data Panels (`Be Vietnam Pro`):** Engineered specifically for native Vietnamese diacritics. Prevents vertical clipping on tones (hỏi, ngã, nặng) inside dense inventory lists, order receipts, and dialogue bubbles.
- **Game Labels & HUD Badges (`Be Vietnam Pro`):** High-weight labels ensuring compact stat counters, currency tags, and action buttons remain readable on high-DPI desktop viewports.

### Vietnamese Typesetting Principles
- Maintain at least `1.3x` to `1.4x` line heights across all body containers to accommodate accented character combinations (e.g., `ệ`, `ở`, `ẩ`).
- Avoid uppercase formatting on text longer than two words to ensure stacked diacritics maintain clear visual distinction during gameplay.

## Layout & Spacing

The layout is built for a desktop-first `16:9` fixed aspect landscape presentation (target native: `1920x1080`), anchoring HUD layers cleanly over a live-rendered or illustrated background.

### Grid & Composition Strategy
- **Master Setup Screens (3-Column / Master-Detail):** Split into a `60% / 40%` ratio where choice matrices (e.g., startup options, inventory tables) occupy the left and real-time visual previews occupy the right.
- **POS Screen Grid (3-Tier Layout):**
  - **Left Sidebar (280px - 320px fixed):** Scrollable vertical customer ticket feed.
  - **Center Canvas (Flexible / Fluid):** 3-to-4 column drink and recipe grid with tactile touch/click buttons.
  - **Right Sidebar (340px fixed):** Order summary, receipt breakdown, payment method toggle, and persistent checkout CTA.
- **Top Pinned Header (56px - 64px):** Pinned persistent statistics (Cash balance, current day, shop reputation, weather icon) isolated by `margin-edge`.
- **Bottom Actions Bar (72px):** Pinned system navigations (`Quay lại`, `Tiếp theo`, `Bắt đầu kinh doanh`) flush with bottom screen boundaries.

## Elevation & Depth

Visual hierarchy uses physical material cues—soft paper ticket layers, smooth wooden backing planks, and tactile stamp-like extrusions.

### Elevation Levels

- **Level 0 (World Canvas):** The rendered cafe interior or ambient street backdrop.
- **Level 1 (Structural Wood Panels):** Primary container panels representing counters or menu boards. Uses `#6B4226` or `#2C1810` subtle border framing with inner paper fill (`#F5EBDD`), cast over the background with a soft drop shadow: `0 8px 24px rgba(44, 24, 16, 0.18)`.
- **Level 2 (Paper Cards & Tickets):** Receipt chits, upgrade cards, and review modals rendered in `#FAF6F0` (Milk Foam) with light fiber-like borders (`#6B4226` at 15% opacity). Elevated with: `0 3px 8px rgba(44, 24, 16, 0.08)`.
- **Level 3 (Interactive Controls & Buttons):** Beveled wooden and terracotta buttons. Uses a dual physical shadow: an outer soft ambient blur (`0 4px 10px rgba(107, 66, 38, 0.2)`) and a bottom edge inset/offset of `3px` solid `#8F4A21` (or `#432613`) creating a tactile press-down depth.
- **Level 4 (Floating Alerts & In-Game Modals):** Customer dialogue overlays, surprise events (`Sự kiện bất ngờ`), and the phone app review screen. Backed by a warm translucent vignette overlay (`rgba(44, 24, 16, 0.6)`) and cast with `0 16px 40px rgba(44, 24, 16, 0.3)`.

## Shapes

The shape system relies on approachable, soft organic contours, avoiding sharp institutional edges while maintaining enough crisp structure for simulation data tables.

### Corner Treatment Rules
- **Base Components (`0.5rem` / `8px`):** Used on inventory table rows, input steppers, item icons, and small HUD stat chips.
- **Cards & Sub-panels (`1rem` / `16px`):** Applied to beverage recipe cards, customer review slips, starting capital cards, and receipt containers.
- **Window Frames & Modals (`1.5rem` / `24px`):** Applied to whole-screen floating game dialogs, setup panels, and the in-game smartphone wrapper.
- **Full Capsule (Pill):** Dedicated strictly to customer feedback tags (`Nhanh`, `Sạch`, `Ngon`, `Ồn`), rating filters, and operational status indicators.

## Components

### Buttons
- **Primary Game CTA (`Thanh toán`, `Bắt đầu kinh doanh`):** Background `#D9824B` (Terracotta Accent), text `#FAF6F0`, border: 2px solid `#6B4226`, bottom shadow offset 3px `#8F4A21`. On active press, the offset shifts to 0px with a 2px downward transform.
- **Secondary Action (`Nâng cấp quán`, `Nhập thêm`):** Background `#5B8C5A` (Leaf Green), text `#FAF6F0`, border: 2px solid `#375636`, bottom shadow offset 3px `#375636`.
- **Ghost / Tertiary Action (`Quay lại`, `Cài đặt`):** Background `#FAF6F0`, text `#6B4226`, border: 1.5px solid `#6B4226`.

### Customer Order Tickets (POS)
- Rendered as vertical craft chits with a simulated jagged tear line at the top.
- Surface: `#FAF6F0` (Milk Foam) with a subtle vertical divider.
- Content Stack: Left-aligned customer mini-portrait (48x48px, rounded 12px), bold customer name (`title-sm`), itemized drink rows with sub-ingredients/notes in `body-sm`.
- Embedded patience timer progress bar docked along the ticket bottom margin.

### Patience & Progress Bars
- Height: 6px to 8px with a fully pill-shaped track.
- Track Background: `#EADBC8`.
- Fill Gradient: Solid active state transitions based on threshold (`#5B8C5A` $\rightarrow$ `#E5A93C` $\rightarrow$ `#C84B31`).

### Inventory Stepper & Row Controls
- Table rows alternate subtly between transparent and `rgba(245, 235, 221, 0.4)`.
- Stepper buttons (`+` / `-`): Rounded square 32x32px, surface `#FAF6F0`, border 1.5px solid `#6B4226`, typography `title-sm` centered.

### Sentiment & Attribute Chips
- Capsule-shaped (`height: 24px`), padding `0 10px`.
- Positive Tag (`Sạch`, `Ngon`, `Nhanh`): Background `rgba(91, 140, 90, 0.15)`, text `#3D613C`, border 1px solid `#5B8C5A`.
- Negative Tag (`Ồn`, `Chậm`, `Đắt`): Background `rgba(200, 75, 49, 0.12)`, text `#C84B31`, border 1px solid `#C84B31`.

### In-Game Phone Frame ("Quán Quen Review")
- A smartphone bezel with smooth 28px rounded corners, warm dark espresso body, and cream top status bar.
- Tab bar segmented control at top with pill-shaped active slider in `#6B4226` with `#FAF6F0` text.