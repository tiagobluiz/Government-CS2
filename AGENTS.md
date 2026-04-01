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
4. [`docs/reference/democracy-v1-data-contracts.md`](docs/reference/democracy-v1-data-contracts.md)
5. [`docs/reference/democracy-v1-balancing-reference.md`](docs/reference/democracy-v1-balancing-reference.md)
6. [`docs/reference/government-ui-content-map.md`](docs/reference/government-ui-content-map.md)
7. [`docs/roadmap/government-system-roadmap.md`](docs/roadmap/government-system-roadmap.md)

If the task is unrelated to the government system, only read the files needed for that task.

## Source of Truth Hierarchy

When documents overlap, use this precedence order:

1. [`docs/specs/democracy-v1-spec.md`](docs/specs/democracy-v1-spec.md)
2. [`docs/specs/democracy-v1-implementation-breakdown.md`](docs/specs/democracy-v1-implementation-breakdown.md)
3. [`docs/reference/*`](docs/reference)
4. [`docs/roadmap/government-system-roadmap.md`](docs/roadmap/government-system-roadmap.md)
5. [`docs/investigation/*`](docs/investigation)

Use the investigation files for background and rationale, not to override the spec.

## Drift Prevention Rules

Agents must not silently drift from the approved documentation.

That means:

- Do not invent alternative Democracy V1 behavior when the spec already defines it.
- Do not introduce a new government mechanic that contradicts the spec without updating the relevant docs in the same change.
- Do not treat ambiguous implementation freedom as permission to change product behavior.
- Do not collapse distinct concepts the docs deliberately separate, such as `approval` and `legitimacy`.
- Do not broaden scope by smuggling in later-government mechanics during Democracy V1 work.

If implementation reveals the spec is incomplete or technically unworkable:

- stop treating the missing detail as an implicit free choice
- update the relevant docs first, or in the same change, before implementing the divergent behavior
- state clearly in the final summary what changed in the documentation and why

## Required Implementation Discipline

For government-related implementation work:

- Reuse the shared-core-plus-ruleset architecture described in the roadmap and spec.
- Keep direct demand effects bounded and compositional.
- Preserve the city-global-primary plus district-seed support model for Democracy V1.
- Preserve the three-layer democracy progression unless the docs are intentionally updated.
- Preserve the fixed party and bloc definitions unless the docs are intentionally updated.
- Keep elections as a light interrupt, not a large separate mode.
- Keep Democracy V1 municipal, generic, and mid-depth.

## Required Documentation Discipline

When code changes affect government behavior, the agent must check whether one or more of these files also need updates:

- [`docs/specs/democracy-v1-spec.md`](docs/specs/democracy-v1-spec.md)
- [`docs/specs/democracy-v1-implementation-breakdown.md`](docs/specs/democracy-v1-implementation-breakdown.md)
- [`docs/reference/democracy-v1-data-contracts.md`](docs/reference/democracy-v1-data-contracts.md)
- [`docs/reference/democracy-v1-balancing-reference.md`](docs/reference/democracy-v1-balancing-reference.md)
- [`docs/reference/government-ui-content-map.md`](docs/reference/government-ui-content-map.md)

If behavior changed and docs were not updated, the agent must explain why no doc update was necessary.

## Final Response Requirements for Agents

For any government-related implementation, the final response must include:

- which spec/reference documents were followed
- whether any behavior differed from the docs
- whether any docs were updated to keep code and spec aligned

This is required so future contributors can spot drift early.
