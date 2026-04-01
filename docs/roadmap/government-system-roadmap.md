# Government System Roadmap

## Overview

This repository is building a Cities: Skylines II mod that adds a government layer on top of the base city simulation.

The player is the head of government. The government form in power changes how the city is governed, how citizens react, how legitimacy is maintained, and how demand for residential, commercial, industrial, and office zones evolves over time.

This mod is not trying to turn Cities: Skylines II into a full grand strategy game. It is trying to add a readable, city-builder-friendly political layer that:

- feels native to CS2
- gives the player new strategic constraints
- creates meaningful differences between government types
- remains extensible enough to support multiple future regimes

The first government type to implement is `Democracy V1`.

## Long-Term Goal

The long-term goal is a family of government rulesets built on top of one reusable government core.

Every government type should share a small set of common ideas:

- a current regime type
- a notion of legitimacy or regime stability
- citizen or bloc reaction to governance
- bounded influence on city demand and confidence
- action friction or governing capacity
- corruption pressure
- save/load persistence and schema migration
- a shared UI shell for political information

What changes between government types is not the existence of governance. What changes is:

- where legitimacy comes from
- how the player stays in power
- which city actions face friction
- whether citizens can remove the government peacefully
- whether order is maintained through consent, institutions, or coercion

## Core Design Principles

### Native CS2 first

The mod must feel like an extension of Cities: Skylines II, not a disconnected management mini-game.

That means:

- reuse existing surfaces where possible
- hook into existing simulation results instead of replacing them wholesale
- keep ordinary city-building responsive
- explain political effects using city language the player already understands

### Realism is intentionally abstracted

Real governments are vastly more complex than what belongs in a city-builder.

This mod intentionally simplifies reality in order to:

- stay readable
- stay balanceable
- stay fun
- stay compatible with the existing CS2 loop

The goal is not legal or institutional realism. The goal is distinct governance behavior with believable tradeoffs.

### Shared core, government-specific rules

The architecture should not hardwire elections, parties, or democratic assumptions into the entire system.

Instead:

- the shared core owns regime state, persistence, common modifier output, UI scaffolding, and debug visibility
- each government ruleset defines how power is kept, how legitimacy behaves, and how political friction is produced

This is required because later governments such as dictatorship will still need legitimacy, stability, corruption, and demand effects, but they will not necessarily use elections.

### Bounded simulation influence

Government should matter, but it should not replace the city simulation.

The mod should:

- add bounded direct modifiers
- react to taxes, services, jobs, housing, and policies
- never fully override the fundamentals of demand and city health

Bad city management should still fail. Good city management should still matter most.

### Extensible, but not overbuilt

The first release must leave room for future governments, but should not create speculative subsystems that are not yet justified by real implementation needs.

That means:

- small reusable interfaces
- explicit save schema versioning
- a compact runtime model
- room for later district depth, richer parties, and new rulesets

## Shared Government Core

The shared core is the non-negotiable foundation for every future government implementation.

It should eventually own:

- the active government type identifier
- regime-level legitimacy or stability
- a government runtime state container
- bounded demand modifier output
- corruption pressure output
- action-friction output
- persistence and migration
- government panel shell
- always-visible political HUD signals
- debug instrumentation

It should not own government-specific policy logic such as:

- democracy election resolution
- dictatorship repression mechanics
- monarchy succession logic

Those belong in government-specific rulesets or adapters.

## Government Type Rollout

### Phase 0: Shared foundation

Build the reusable pieces that every government type will need.

Deliverables:

- government state owner/service
- save/load skeleton
- schema migration skeleton
- modifier pipeline for demand/confidence effects
- ruleset abstraction
- government panel shell
- debug logging and internal visibility
- existing-save initialization path

### Phase 1: Democracy V1

Democracy is the first fully playable government ruleset.

Design goals:

- municipal rather than national framing
- generic democracy first, not a parliamentary/presidential split
- mid-depth friction, not full political simulation
- heavy reuse of existing CS2 taxes, budgets, and policies

Main features:

- approval
- legitimacy
- voter blocs
- three broad parties
- periodic support updates
- fixed election cadence
- political capital as lightweight friction
- bounded demand influence
- explicit election loss override state
- limited consultation
- lightweight corruption
- staged unlock progression

### Phase 2: Democracy expansion

After the first democracy release is stable, deepen it before branching too wide.

Likely additions:

- stronger district differentiation
- richer bloc weighting
- more nuanced party movement
- improved political explanations
- more balancing controls
- better consultation flow

### Phase 3: Dictatorship ruleset

Dictatorship is the first non-democratic government type planned after democracy.

Expected differences:

- legitimacy shifts toward fear, control, performance, and elite loyalty
- action friction shifts from public consent to coercion upkeep and corruption cost
- order maintenance becomes more expensive and more direct
- demand reacts differently to stability, repression, and business confidence

### Phase 4: Additional government types

Only after democracy and dictatorship prove the core architecture should more nuanced systems be added.

Likely candidates:

- constitutional monarchy
- hybrid/managed democracy
- parliamentary vs presidential variants
- more complex transition states between regimes

## Dependency Order Between Phases

The intended dependency chain is:

1. shared core
2. democracy base ruleset
3. democracy refinement
4. dictatorship
5. additional government types

This order matters because:

- democracy provides the clearest first bridge to CS2 systems
- dictatorship tests the abstraction boundary
- later forms can only be implemented cleanly if the shared core is already proven

## What Is Intentionally Deferred

The following items are intentionally not first-release goals:

- full real-world constitutional accuracy
- a full parliamentary chamber simulation
- procedural party generation
- detailed media simulation
- emergency powers in democracy v1
- deep district-by-district election resolution
- a full authoritarian transition tree
- large settings matrices

These are deferred to keep the first release coherent and implementable.

## Risks and Mitigations

### Risk: politics overpowers vanilla CS2 demand

Mitigation:

- use small bounded modifiers
- keep city fundamentals dominant
- document tuning rules clearly

### Risk: democracy becomes a separate mini-game

Mitigation:

- reuse taxes, budgets, and policies
- keep election flow a light interrupt
- avoid excessive hard locks

### Risk: future government types require heavy rewrites

Mitigation:

- build a shared core plus rulesets
- keep save data versioned
- separate government-agnostic from democracy-specific logic

### Risk: the first version is too complex to balance

Mitigation:

- stage progression in three layers
- keep party state lightweight
- start with a small curated bloc set
- include hidden debug tooling from the beginning

### Risk: existing saves break or initialize badly

Mitigation:

- support existing saves intentionally
- seed political state from current city conditions
- use explicit migration steps instead of ad hoc fallback logic

## Delivery Intent

The documentation in `docs/specs` and `docs/reference` should be treated as implementation-facing companions to this roadmap.

This roadmap answers:

- what the system is for
- why the architecture is shaped this way
- which government types come next
- what is deliberately out of scope for now

The democracy specification answers:

- exactly what should be built first
- how it should behave
- how it should integrate into CS2
