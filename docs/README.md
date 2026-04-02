# Government Docs Index

This folder contains the working documentation package for the government system planned for this mod.

The package is intentionally written so that a future engineer or agent can start with no prior chat context.

## Recommended Reading Order

1. `roadmap/government-system-roadmap.md`
2. `specs/democracy-v1-spec.md`
3. `specs/democracy-v1-implementation-breakdown.md`
4. `specs/democracy-v1-test-plan.md`
5. `reference/democracy-v1-data-contracts.md`
6. `reference/democracy-v1-balancing-reference.md`
7. `reference/government-ui-content-map.md`

## What Each Folder Is For

### `investigation`

Background research and inspiration documents.

Use these to understand:

- Democracy 4 mechanics worth borrowing
- real-world government patterns
- where the design ideas came from

These are reference inputs, not the implementation source of truth.

### `roadmap`

Long-term project direction.

Use this to understand:

- why the architecture is shared-core plus rulesets
- which government types come after democracy
- what is intentionally deferred

### `specs`

Implementation-facing product specifications.

Use these as the main source of truth for:

- what Democracy V1 must do
- what is in and out of scope
- how to break the work into delivery slices
- how Democracy V1 should be validated

### `reference`

Supporting implementation guidance.

Use these to understand:

- state and interface contracts
- balancing guardrails
- UI information architecture

## Source of Truth Rules

- `specs/democracy-v1-spec.md` is the primary product and behavior source of truth for Democracy V1.
- `specs/democracy-v1-implementation-breakdown.md` is the primary execution-order source of truth.
- `specs/democracy-v1-test-plan.md` is the primary validation and QA source of truth.
- `reference/*` documents support implementation and should stay aligned with the spec.
- `investigation/*` files are informative but do not override the spec.
