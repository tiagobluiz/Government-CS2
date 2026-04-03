# Repository Instructions

## Purpose

This repository is building a Cities: Skylines II mod that introduces a government layer.

The current implementation source of truth is the documentation package under [`docs`](docs), especially the Democracy V1 documents.

Any agent working in this repository must treat those documents as binding product and implementation guidance, not as optional background reading.

## Mandatory Read Order Before Implementation

Before proposing, planning, or implementing any government-related change, read these files in order:

1. [`docs/README.md`](docs/README.md)
2. [`docs/specs/democracy-v1-spec.md`](docs/specs/democracy-v1-spec.md)
3. [`docs/specs/democracy-v1-implementation-breakdown.md`](docs/specs/democracy-v1-implementation-breakdown.md)
4. [`docs/specs/democracy-v1-test-plan.md`](docs/specs/democracy-v1-test-plan.md)
5. [`docs/reference/democracy-v1-data-contracts.md`](docs/reference/democracy-v1-data-contracts.md)
6. [`docs/reference/democracy-v1-balancing-reference.md`](docs/reference/democracy-v1-balancing-reference.md)
7. [`docs/reference/government-ui-content-map.md`](docs/reference/government-ui-content-map.md)
8. [`docs/roadmap/government-system-roadmap.md`](docs/roadmap/government-system-roadmap.md)

If the task is unrelated to the government system, only read the files needed for that task.

## Source of Truth Hierarchy

When documents overlap, use this precedence order:

1. [`docs/specs/democracy-v1-spec.md`](docs/specs/democracy-v1-spec.md)
2. [`docs/specs/democracy-v1-implementation-breakdown.md`](docs/specs/democracy-v1-implementation-breakdown.md)
3. [`docs/specs/democracy-v1-test-plan.md`](docs/specs/democracy-v1-test-plan.md)
4. [`docs/reference/*`](docs/reference)
5. [`docs/roadmap/government-system-roadmap.md`](docs/roadmap/government-system-roadmap.md)
6. [`docs/investigation/*`](docs/investigation)

Use the investigation files for background and rationale, not to override the spec.

## Drift Prevention Rules

Agents must not silently drift from the approved documentation.

That means:

- Do not invent alternative Democracy V1 behavior when the spec already defines it.
- Do not introduce a new government mechanic that contradicts the spec without updating the relevant docs in the same change.
- Do not treat ambiguous implementation freedom as permission to change product behavior.
- Do not collapse distinct concepts the docs deliberately separate, such as `approval` and `legitimacy`.
- Do not broaden scope by smuggling in later-government mechanics during Democracy V1 work.
- Do not trade away extensibility, configurability, or testability for short-term speed unless the docs are explicitly updated to allow it.

If implementation reveals the spec is incomplete or technically unworkable:

- stop treating the missing detail as an implicit free choice
- update the relevant docs first, or in the same change, before implementing the divergent behavior
- state clearly in the final summary what changed in the documentation and why

Agents must also follow the phase ordering defined in [`docs/roadmap/government-system-roadmap.md`](docs/roadmap/government-system-roadmap.md) exactly.

That means:

- do not start implementing later-phase functionality while earlier roadmap-phase deliverables are still incomplete
- do not relabel incomplete work from one roadmap phase as a later phase for convenience
- if a phase needs to be split into smaller execution slices, those slices must remain explicitly nested under the same roadmap phase instead of inventing a different phase order

## Required Implementation Discipline

For government-related implementation work:

- Reuse the shared-core-plus-ruleset architecture described in the roadmap and spec.
- Follow the roadmap phase order exactly. Complete `Phase 0: Shared foundation` before advancing into `Phase 1: Democracy V1` feature work.
- Treat extensibility as a hard requirement. Prefer designs that can support later government types without major rewrites.
- Treat configurability as a hard requirement. Prefer parameterized or data-driven rules over hardcoded behavior when the behavior is likely to vary by government type, tuning pass, difficulty, or settings.
- Default to JSON-backed configuration for tunable government data unless there is a clear documented reason to use another format.
- Treat testability as a hard requirement. Prefer designs that isolate deterministic logic into units that can be validated outside the live game.
- Keep direct demand effects bounded and compositional.
- Preserve the city-global-primary plus district-seed support model for Democracy V1.
- Preserve the three-layer democracy progression unless the docs are intentionally updated.
- Preserve the fixed party and bloc definitions unless the docs are intentionally updated.
- Keep elections as a light interrupt, not a large separate mode.
- Keep Democracy V1 municipal, generic, and mid-depth.

### Non-negotiable engineering priorities

The following are mandatory throughout development:

- extensibility
- configurability
- high test coverage

These are not optional quality improvements. They are core development constraints.

Interpretation:

- `extensibility` means the design should leave clean seams for later government types, richer political models, and deeper balancing without forcing large rewrites
- `configurability` means important behavior should be adjustable through clearly owned constants, settings, data structures, or ruleset-specific parameters instead of being buried in rigid logic
- By default, that configurability should live in validated JSON configuration files for tunable values such as weights, thresholds, affinities, caps, unlock rules, and per-ruleset balance data. Do not turn JSON into an unbounded scripting language; keep procedural behavior in code.
- `high test coverage` means every deterministic subsystem should have strong automated coverage, including normal cases, boundary cases, regression cases, and explicit edge cases

Agents must not justify weak test coverage by saying a future manual playtest will catch it. Manual testing complements automated coverage; it does not replace it.

## Required Documentation Discipline

When code changes affect government behavior, the agent must check whether one or more of these files also need updates:

- [`docs/specs/democracy-v1-spec.md`](docs/specs/democracy-v1-spec.md)
- [`docs/specs/democracy-v1-implementation-breakdown.md`](docs/specs/democracy-v1-implementation-breakdown.md)
- [`docs/specs/democracy-v1-test-plan.md`](docs/specs/democracy-v1-test-plan.md)
- [`docs/reference/democracy-v1-data-contracts.md`](docs/reference/democracy-v1-data-contracts.md)
- [`docs/reference/democracy-v1-balancing-reference.md`](docs/reference/democracy-v1-balancing-reference.md)
- [`docs/reference/government-ui-content-map.md`](docs/reference/government-ui-content-map.md)

If behavior changed and docs were not updated, the agent must explain why no doc update was necessary.

If implementation changes affect:

- extensibility assumptions
- configurability points
- test strategy
- edge-case handling

then the relevant docs must be updated in the same change.

## Required Testing Discipline

For government-related implementation work:

- add or update automated tests for every deterministic subsystem that is introduced or changed
- cover happy paths, edge cases, boundary conditions, and obvious regression cases
- prefer table-driven or matrix-style coverage when the system has multiple combinations of inputs or policy states
- keep business logic separable enough that it can be tested without launching Cities: Skylines II

Minimum expectation:

- if a rules engine, scorer, modifier calculator, migration step, or state transition is added, it should come with direct automated coverage unless there is a documented technical reason it cannot

Insufficient coverage examples:

- only testing one representative path when multiple blocs, parties, demand bars, or settings can change the outcome
- relying only on build success
- relying only on manual in-game testing for deterministic logic

Preferred coverage targets include:

- bloc scoring
- party standing updates
- approval and legitimacy changes
- election resolution
- political-capital cost calculation
- demand modifier composition
- override-state branching
- save migration logic

## Final Response Requirements for Agents

For any government-related implementation, the final response must include:

- which spec/reference documents were followed
- whether any behavior differed from the docs
- whether any docs were updated to keep code and spec aligned

This is required so future contributors can spot drift early.
