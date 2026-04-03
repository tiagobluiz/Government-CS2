# Democracy V1 Implementation Breakdown

## Suggested Delivery Order

This document converts the democracy specification into execution workstreams.

Implementation-wide default:

- tunable government behavior should be backed by validated JSON configuration files
- procedural rules should remain in C#
- configuration loading and validation should be treated as foundation work, not an optional later cleanup step

The recommended order is:

1. Shared government core and persistence
2. Democracy runtime state and periodic update loop
3. Election system
4. Demand and economy hooks
5. Budget and policy integration
6. UI panel and HUD signals
7. Progression and unlocks
8. Override state and settings
9. Debug and balancing tooling

This order is chosen to:

- establish stable state ownership early
- allow non-UI logic to be tested before presentation work
- keep demand and action friction grounded in a working runtime model
- make later UI and balancing work read from actual state rather than mocks

## Workstream 1: Shared Core and Persistence

### Goal

Create the reusable government foundation that all future government types can sit on.

### Prerequisites

- none beyond the current mod scaffold

### Outputs

- a government state owner/service
- a ruleset abstraction such as `IGovernmentRuleset`
- typed configuration loader(s) for shared government and democracy tuning
- runtime state container(s)
- minimal versioned save model
- migration skeleton
- internal debug/logging hooks

### Main implementation tasks

- Define government-owned runtime state and save state.
- Define a ruleset interface that lets democracy plug in as the first implementation.
- Define typed JSON configuration models for shared government and democracy-specific tunables.
- Define configuration validation rules and startup/load behavior.
- Add a save schema version field from day one.
- Add an existing-save initialization path.
- Add a bounded modifier output contract that later workstreams can consume.

### Tests

- new save initializes cleanly
- existing save seeds cleanly
- state survives save/load
- schema version is stored and read correctly
- configuration files load successfully and fail loudly when required fields are missing or invalid

### Failure risks

- democracy-specific assumptions leak into the shared core
- too much derived UI state gets persisted
- migration is left underspecified

### Implementation notes

- keep the core intentionally small
- do not put election-specific logic here
- prefer explicit state ownership to global ad hoc reads
- keep configuration separate from save data and runtime state

## Workstream 2: Democracy State and Update Loop

### Goal

Implement the democracy-specific runtime model and periodic political recalculation.

### Prerequisites

- shared core exists
- persistence model exists

### Outputs

- democracy runtime state
- periodic update cadence
- support evaluation pipeline
- bloc evaluation
- party standing evaluation
- separated approval and legitimacy behavior

### Main implementation tasks

- Build the citywide support pipeline from existing CS2 signals.
- Add district seed aggregates for minor local weighting and future use.
- Implement six bloc snapshots.
- Implement three party standings.
- Separate fast-moving approval from slower-moving legitimacy.
- Feed lightweight political capital regeneration from the runtime state.
- Source bloc weights, party affinities, thresholds, and regeneration coefficients from typed JSON-backed configuration.

### Tests

- support updates on the intended cadence
- bloc reactions change when taxes, jobs, or services change
- approval and legitimacy can diverge
- district data exists without taking over the system
- config-driven tuning changes alter results without code changes

### Failure risks

- approval and legitimacy collapse into one value in practice
- district data becomes more expensive or central than intended
- support update cadence becomes too noisy

### Implementation notes

- periodic recalculation is the default
- if immediate refreshes are added later, keep them limited to major player actions

## Workstream 3: Election System

### Goal

Implement the democratic retention-of-power loop.

### Prerequisites

- democracy runtime state exists
- approval, legitimacy, blocs, and parties are live

### Outputs

- election timer/cycle
- election resolution logic
- election result presentation hooks
- loss/override handling

### Main implementation tasks

- Track a fixed 4-year election cadence.
- Resolve elections from approval, bloc support, party standings, and legitimacy pressure.
- Create the light-interrupt election presentation flow.
- Implement incumbent win/loss determination.
- Implement the election override path.
- Keep election coefficients and caps in JSON-backed configuration while keeping the resolution algorithm in code.

### Tests

- election date advances correctly
- resolution uses all required inputs
- election result is readable to the player
- loss can transition to override state
- election weighting changes can be validated through configuration-driven test cases

### Failure risks

- election outcomes feel arbitrary
- the election result UI becomes too heavy for normal play
- override state is ambiguous or too hidden

### Implementation notes

- weight recent term conditions strongly enough to preserve reelection pressure
- do not turn the election into a large separate gameplay mode

## Workstream 4: Demand and Economy Hooks

### Goal

Connect government state to CS2 demand safely and visibly.

### Prerequisites

- government modifier pipeline exists
- democracy state exists

### Outputs

- bounded demand modifier injection
- per-bar democratic influence model
- confidence and policy-direction channels

### Main implementation tasks

- Add small bounded modifiers to all four demand bars.
- Keep modifiers compositional, not full rewrites.
- Map legitimacy and confidence into a general demand-confidence channel.
- Map political posture into a policy-direction channel.
- Make sure residential, commercial, industrial, and office each read political state in distinct ways.
- Drive caps, per-bar multipliers, and channel strengths from configuration.

### Tests

- all four demand bars can receive democratic influence
- modifiers remain bounded
- city fundamentals still dominate demand
- low legitimacy weakens confidence appropriately
- configuration caps prevent out-of-range demand effects

### Failure risks

- government modifiers overpower vanilla demand
- demand effects are too small to notice
- modifier composition becomes opaque to debug

### Implementation notes

- expose breakdowns in debug output
- keep modifier caps explicit and easy to tune

## Workstream 5: Budget and Policy Integration

### Goal

Connect democracy to the existing decisions players already make in CS2.

### Prerequisites

- runtime political state exists
- demand modifier pipeline exists

### Outputs

- tax reaction hooks
- service budget reaction hooks
- selected policy reaction hooks
- major decision friction previews

### Main implementation tasks

- Wire asymmetric political reaction into tax changes.
- Wire major service budget changes into bloc, party, approval, and political-capital consequences.
- Wire selected policy themes into the same reaction system.
- Determine which zoning actions count as major enough to matter politically.
- Keep reaction strengths and mappings data-driven through configuration tables wherever practical.

### Tests

- residential and business-side taxes create distinct reactions
- service budget changes produce immediate and delayed effects
- selected policy themes move the expected blocs and parties
- ordinary zoning stays responsive
- configuration updates can rebalance reactions without touching core logic

### Failure risks

- too many actions become politically noisy
- hooks become overly invasive for compatibility
- reaction previews do not match final consequences closely enough

### Implementation notes

- keep the first integration set focused on:
  - taxes
  - all major service budgets
  - selected policies in housing, welfare/services, business/growth, and environment/order

## Workstream 6: UI Panel and HUD Signals

### Goal

Make the political system readable without taking over the screen.

### Prerequisites

- runtime state exists
- election state exists
- demand effects and action friction exist

### Outputs

- government panel
- persistent HUD summary
- tooltips or contextual explanation surfaces
- election result surface
- override-state warning presentation

### Main implementation tasks

- Build the dedicated government panel.
- Show approval, legitimacy, election timing, risk, bloc state, party standings, and political capital.
- Add always-visible HUD signals for approval, election timing, and risk.
- Add demand explanations in the government panel and relevant tooltips.
- Add election and override presentation.

### Tests

- player can understand current political status quickly
- player can understand why demand moved politically
- override state is explicit
- election flow is a light interrupt rather than a heavy modal experience

### Failure risks

- too much data is shown at once
- the panel becomes a detached mini-game
- important political effects are not explained

### Implementation notes

- strongly reuse existing CS2 surfaces where possible
- keep the dedicated panel as the center of political detail

## Workstream 7: Progression and Unlocks

### Goal

Introduce democracy in layers so players are not overwhelmed early.

### Prerequisites

- base runtime state exists
- UI shell exists

### Outputs

- unlock layer tracking
- milestone/election driven promotion logic
- per-layer feature gating
- unlock messaging

### Main implementation tasks

- Implement three democracy layers.
- Gate systems by current unlock layer.
- Trigger unlocks from a mix of city milestones and election progression.
- Present explicit unlock explanations.
- Keep thresholds and progression gates configuration-backed where practical.

### Tests

- layer 1 exposes approval, election timer, and top-line blocs
- layer 2 adds parties, political capital, and major decision friction
- layer 3 adds consultation, corruption, and district effects
- unlock messaging is clear
- unlock thresholds can be tuned through configuration without code edits

### Failure risks

- unlock rules feel arbitrary
- later layers accidentally depend on data not initialized earlier
- players miss what changed

### Implementation notes

- design unlock logic so existing saves can be seeded safely

## Workstream 8: Override State and Settings

### Goal

Support election loss continuity without collapsing democracy into a full authoritarian system.

### Prerequisites

- election flow exists
- legitimacy and confidence effects exist

### Outputs

- override-state flow
- default override penalties
- sandbox/no-penalty setting

### Main implementation tasks

- Add explicit election-loss override handling.
- Apply legitimacy damage, economic confidence penalty, and slower political recovery by default.
- Add settings support for penalty disablement in sandbox-like play.
- Ensure the UI remains explicit about the override condition.
- Keep penalty strengths and recovery modifiers configuration-backed.

### Tests

- default override state applies all expected penalties
- sandbox setting disables penalties cleanly
- override state does not silently masquerade as normal democracy
- override-penalty edge cases are covered with matrix-style configuration tests

### Failure risks

- override penalties are too weak or too severe
- sandbox behavior breaks progression or election flow
- state labeling is unclear

### Implementation notes

- keep this narrowly scoped
- do not let this workstream expand into a full dictatorship path

## Workstream 9: Debug/Balancing Tooling

### Goal

Make the political system tunable and inspectable.

### Prerequisites

- the major systems above exist

### Outputs

- hidden debug panel
- structured logs
- calculation breakdown visibility

### Main implementation tasks

- expose support component breakdowns
- expose bloc contributions
- expose party standings and why they changed
- expose demand modifier sources and caps
- expose election resolution weighting
- expose override penalty state
- expose the active loaded configuration and resolved tuned values for debugging

### Tests

- debug panel reflects real runtime state
- logs are useful without being overwhelming
- balancing work can trace political cause and effect
- configuration validation failures are discoverable and actionable

### Failure risks

- insufficient visibility causes long balancing cycles
- too much debug state becomes expensive

### Implementation notes

- bias toward inspectability in development builds and hidden developer surfaces

## Definition of Done Per Workstream

Each workstream is done only when:

- required runtime state exists
- player-facing behavior matches the democracy specification
- save/load behavior is validated where relevant
- debug visibility is adequate for the subsystem
- obvious balancing parameters are exposed
- known failure risks are documented or handled
- configuration ownership is clear enough that future tuning does not require hunting through unrelated code

## Final Integration Criteria

The implementation breakdown should be considered complete when:

- the democracy feature can be built incrementally from these workstreams
- no workstream requires inventing missing product decisions
- the sequence is practical for a new engineering agent with zero prior context
