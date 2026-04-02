# Democracy V1 Test Plan

## Purpose

This document defines how Democracy V1 should be tested once implementation starts.

It exists because:

- the main spec defines behavior and acceptance criteria
- the implementation breakdown defines delivery slices
- a separate test plan is still needed to explain how we validate the feature in practice

This file is intended for engineers, reviewers, and future agents with no prior chat context.

## Scope

This test plan covers:

- Democracy V1 functional validation
- save/load and migration behavior
- UI clarity and explanation quality
- balancing-safe behavior
- compatibility-safe behavior
- regression checks after changes

This plan does not cover:

- dictatorship or later government types
- full game-wide CS2 regression coverage unrelated to the government system
- automated testing infrastructure that does not yet exist in the repo

## Test Philosophy

Democracy V1 should be tested in layers:

1. `State correctness`
- the right political values exist and move correctly

2. `Simulation integration`
- politics correctly reacts to taxes, budgets, zoning, policies, and demand

3. `Player clarity`
- the player can understand what changed and why

4. `Persistence safety`
- saves load correctly and migration does not lose political meaning

5. `Compatibility safety`
- the feature does not rely on invasive rewrites and degrades reasonably

The system should not be considered validated just because values change. It must also be:

- understandable
- bounded
- resistant to obvious exploits

## Validation Responsibility Split

This section defines what an implementation agent can realistically validate inside the repository versus what requires manual in-game verification.

### Agent-responsible validation

An agent working from the repository is responsible for validating anything that can be checked through code, build outputs, logs, or deterministic test harnesses.

That includes:

- project restore/build success
- pure logic tests
- serialization and migration tests
- deterministic scoring and calculation tests
- smoke checks implemented as scripts or automated test cases
- static verification that code and docs remain aligned

An agent should not claim full gameplay validation when only code-level checks were performed.

### Manual tester responsibility

A human tester is responsible for any validation that depends on live CS2 gameplay behavior, in-game UI feel, pacing, player comprehension, or interaction with a real running city.

That includes:

- whether the feature feels understandable in live play
- whether election flow feels too intrusive or too invisible
- whether zoning thresholds feel politically legible
- whether demand explanations are actually clear to a player
- whether the feature feels balanced in real city sessions
- whether the feature remains fun rather than merely correct

### Shared responsibility

Some areas need both:

- agents validate the underlying logic
- human testers validate that the same logic feels correct in the game

Important examples:

- election resolution
- demand modifiers
- political-capital friction
- override penalties
- unlock pacing

## Test Environments

### Environment A: Fresh city

Use a newly created save with the mod enabled from the beginning.

Purpose:

- validate initialization
- validate early unlock behavior
- validate early political pacing

### Environment B: Mature existing save

Use an existing city save with no prior government data.

Purpose:

- validate seeding of approval, legitimacy, blocs, parties, and election timing
- validate compatibility with existing save progression

### Environment C: Stress city

Use a city with:

- multiple active districts
- unstable demand
- budget pressure
- traffic or pollution problems

Purpose:

- validate bloc differentiation
- validate demand effects under pressure
- validate override penalties and recovery behavior

### Environment D: Sandbox-style validation

Use the same city conditions as another test case, but with sandbox/no-penalty override settings enabled.

Purpose:

- validate special-setting behavior
- confirm sandbox mode does not corrupt the main logic

## What Can Be Tested By an Agent Today

Without additional game automation tooling, an agent can realistically test the following once the relevant code exists:

- build and restore success
- runtime state initialization in pure code
- save-data serialization and deserialization
- schema migration correctness
- bloc scoring logic
- party standing logic
- approval and legitimacy divergence rules
- political capital cost calculation
- bounded demand modifier calculation
- election resolution scoring
- settings and override penalty branching logic

These should become automated tests wherever practical.

## What Requires Manual In-Game Testing

The following must be treated as manual validation until the project gains dedicated automation or scenario-driving tools:

- whether the feature feels native inside CS2
- whether the government panel is readable in actual gameplay
- whether election interrupts feel appropriately light
- whether approval, legitimacy, and risk are understandable without debug tools
- whether the major-zoning threshold feels fair and legible while playing
- whether demand shifts feel believable in a live city
- whether sandbox mode feels correct to a player
- whether progression unlock timing feels natural across real city sessions

These checks must be documented as manual playtest expectations rather than automated guarantees.

## Test Stages

### Stage 1: Core state and persistence

Validate:

- government state initializes
- democracy ruleset becomes active
- default values are sensible
- save/load round-trips preserve political state

Required checks:

- `Approval` exists and is readable
- `Legitimacy` exists and is distinct from approval
- `PoliticalCapital` exists
- `ElectionCycleState` exists
- bloc and party state exists
- schema version exists

Pass criteria:

- a new save and an existing save both produce usable political state
- no missing or obviously nonsensical values appear in the UI or debug output

Validation owner:

- agent for state creation, serialization, and migration behavior
- manual tester for final in-game readability of seeded values

### Stage 2: Periodic political updates

Validate:

- support recalculates on periodic cadence
- bloc values change when city conditions change
- party standings react to bloc movement
- legitimacy and approval can diverge

Required checks:

- raise taxes and observe approval change
- restore taxes and observe recovery
- degrade a major service budget and observe bloc changes
- improve a major service budget and observe bloc changes
- validate that legitimacy remains steadier than approval during ordinary governance

Pass criteria:

- changes are visible, directional, and explainable
- approval is more reactive than legitimacy

Validation owner:

- agent for deterministic scoring and update behavior
- manual tester for whether the magnitude and pacing feel understandable in live play

### Stage 3: Election loop

Validate:

- the 4-year election cadence runs correctly
- election outcomes use approval, bloc support, party standings, and legitimacy
- election presentation remains a light interrupt

Required checks:

- simulate or advance time until an election occurs
- validate incumbent win case
- validate incumbent loss case
- validate that recent term behavior influences results

Pass criteria:

- elections do not feel arbitrary
- election UI is visible but not overly disruptive

Validation owner:

- agent for election timing and resolution logic
- manual tester for election pacing, interruption level, and readability in-game

### Stage 4: Override-state behavior

Validate:

- losing an election does not hard-end the save by default
- override state is explicit
- penalties apply correctly
- sandbox mode can disable the penalties

Required checks:

- lose an election and remain in power
- confirm legitimacy drops
- confirm economic confidence penalty appears
- confirm political recovery slows
- repeat with sandbox/no-penalty mode enabled

Pass criteria:

- override state is clearly different from normal democracy
- sandbox setting only removes the intended penalties

Validation owner:

- agent for branching logic and penalty application
- manual tester for whether the override state is obvious and narratively understandable

### Stage 5: Demand integration

Validate:

- all four demand bars can receive political effects
- effects remain bounded
- city fundamentals still dominate demand

Required checks:

- validate a stable, legitimate government modestly improves confidence-sensitive demand
- validate low legitimacy weakens confidence-sensitive demand
- validate pro-growth posture benefits office/commercial or industrial bars in the expected direction
- validate pro-housing posture benefits residential demand in the expected direction

Pass criteria:

- demand changes are noticeable in explanations
- government modifiers do not swamp base CS2 city logic

Validation owner:

- agent for modifier bounds and composition correctness
- manual tester for whether demand reactions feel believable in a running city

### Stage 6: Taxes, budgets, policies, and zoning

Validate:

- taxes create asymmetric political reactions
- major service budget changes create immediate and delayed political consequences
- selected policy themes move the expected blocs and parties
- major zoning moves trigger political reaction
- ordinary zoning remains responsive

Required checks:

- raise residential taxes and verify stronger Households/Civic pain
- raise office/commercial/industrial taxes and verify stronger Business/Growth pain
- cut education, healthcare, police, and transit budgets separately and compare reactions
- apply selected policies in:
  - housing
  - welfare/services
  - business/growth
  - environment/order
- perform at least one major rezoning example from the spec
- perform several minor zoning actions and confirm they do not trigger the same political weight

Pass criteria:

- reactions align with the documented bloc and party definitions
- the major-zoning threshold feels legible rather than arbitrary

Validation owner:

- agent for rule execution and expected reaction mapping
- manual tester for political legibility of actual zoning, budget, and policy decisions

### Stage 7: Progression layers

Validate:

- the three democracy layers unlock in the intended order
- unlock messaging is explicit
- feature gates activate correctly

Required checks:

- validate Layer 1 provides approval, election timer, and top-line blocs
- validate Layer 2 adds parties, political capital, and major decision friction
- validate Layer 3 adds consultation, corruption, and district effects

Pass criteria:

- the player understands what unlocked and why
- later features do not appear before their unlock layer

Validation owner:

- agent for unlock ordering and feature gating
- manual tester for pacing and player comprehension

### Stage 8: UI clarity

Validate:

- government panel contains the right information
- always-visible HUD signals are useful
- demand explanations are understandable
- warnings before major actions are informative

Required checks:

- verify the panel shows approval, legitimacy, election timing, risk, blocs, parties, and political capital
- verify the HUD surfaces approval, election timing, and risk
- inspect at least one demand explanation per bar
- inspect at least one warning for a major politically meaningful action

Pass criteria:

- a tester can explain what changed and why without using debug tools

Validation owner:

- manual tester primarily
- agent support is limited to checking presence of the expected UI data and explanation strings

### Stage 9: Save/load and migration

Validate:

- political state survives normal save/load
- migration works when schema changes occur
- existing saves seed safely

Required checks:

- save and reload during normal democracy
- save and reload during override state
- save and reload after higher unlock layers are active
- validate at least one migration path once schema v2 exists

Pass criteria:

- no core political fields are lost
- no save loads into a broken or contradictory political state

Validation owner:

- agent primarily
- manual tester secondarily for in-game continuity checks after load

### Stage 10: Compatibility and regression

Validate:

- the mod does not depend on full demand-model replacement
- changes remain bounded after future edits
- docs and implementation stay aligned

Required checks:

- inspect whether a code change altered documented behavior
- verify relevant docs were updated when behavior changed
- re-run a small smoke set after each meaningful system change

Minimum smoke set:

- new save initialization
- existing save seeding
- tax reaction
- one election cycle
- one major zoning move
- one override-state case
- save/load round-trip

Pass criteria:

- no major regression in the smoke set
- no silent spec drift

Validation owner:

- agent for smoke checks, doc alignment, and deterministic regression cases
- manual tester for live-game interoperability symptoms that cannot be observed from repo-only checks

## Manual Test Scenarios

### Scenario 1: Stable civic city

Setup:

- healthy services
- low unemployment
- moderate taxes
- adequate housing

Expected result:

- decent approval
- healthy legitimacy
- modest demand-confidence benefit
- strong reelection chances

### Scenario 2: Growth-first city

Setup:

- pro-business policy posture
- moderate resident strain
- good office/commercial outlook

Expected result:

- Growth party improves
- Business bloc improves
- some resident blocs may weaken
- office/commercial demand gets a bounded lift

### Scenario 3: Austerity city

Setup:

- service budget cuts
- aggressive taxes
- unstable housing or jobs

Expected result:

- approval falls
- affected blocs react clearly
- legitimacy erodes more slowly unless anti-democratic action occurs
- reelection becomes harder

### Scenario 4: Election override

Setup:

- lose reelection
- remain in power

Expected result:

- explicit override messaging
- legitimacy damage
- confidence penalty
- slower recovery

### Scenario 5: Major rezoning backlash

Setup:

- perform a politically visible rezoning shift defined as major in the spec

Expected result:

- political warning before action
- meaningful bloc/party reaction after action
- routine small zoning actions still remain comparatively light

## Automation Guidance

This repo does not yet have a complete automated test harness for game behavior.

Until that exists:

- use debug output as the first automation-friendly validation surface
- prefer deterministic calculations where possible for election scoring and political reactions
- isolate pure logic so future tests can validate:
  - bloc scoring
  - party scoring
  - political capital cost calculation
  - demand modifier composition
  - election resolution
  - migration steps

Current practical rule:

- if a check depends only on code and deterministic inputs, it belongs in agent-runnable validation
- if a check depends on live CS2 behavior or player interpretation, it belongs in manual in-game validation

The long-term automated goal should be:

- unit tests for pure scoring and migration logic
- scenario tests for core government state transitions
- smoke checks for serialization and version upgrades

## Release Gate for Democracy V1

Democracy V1 should not be considered ready until all of the following are true:

- new saves work
- existing saves seed correctly
- election loop works end to end
- override state works end to end
- all four demand bars show bounded political influence
- tax and budget reactions match bloc/party expectations
- major zoning moves trigger political consequences as documented
- UI explanations are readable
- save/load is stable
- hidden debug tools can explain outcomes

## Reporting Requirement

Any implementation summary for Democracy V1 testing should explicitly separate:

- `Agent-tested`
- `Manual in-game testing still required`

An agent must not present repo-only validation as if the full feature has been playtested in Cities: Skylines II.

## Relationship to Other Docs

Use this document together with:

- [`democracy-v1-spec.md`](./democracy-v1-spec.md) for required behavior
- [`democracy-v1-implementation-breakdown.md`](./democracy-v1-implementation-breakdown.md) for build order
- [`democracy-v1-data-contracts.md`](../reference/democracy-v1-data-contracts.md) for state ownership
- [`democracy-v1-balancing-reference.md`](../reference/democracy-v1-balancing-reference.md) for tuning boundaries

If this test plan and the spec ever disagree, the spec defines product behavior and this test plan should be updated to match it.
