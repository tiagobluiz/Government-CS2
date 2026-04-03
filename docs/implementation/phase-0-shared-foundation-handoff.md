# Phase 0 Handoff: Shared Foundation

## Purpose

This document is the active implementation handoff for `Phase 0: Shared foundation` as defined in the roadmap.

Its purpose is to ensure that any future contributor or agent can resume the work without relying on chat history and without drifting from the roadmap phase order.

## Roadmap Alignment Rule

This handoff intentionally follows the roadmap exactly.

Important rule:

- this repository must complete the roadmap's `Phase 0: Shared foundation` before it begins implementing `Phase 1: Democracy V1` gameplay features

Phase 0 may be executed in smaller slices, but those slices must remain sub-slices of Phase 0, not a relabeled early Phase 1.

## What Phase 0 Means in This Repository

According to the roadmap, Phase 0 includes these deliverables:

- government state owner/service
- save/load skeleton
- schema migration skeleton
- modifier pipeline for demand/confidence effects
- ruleset abstraction
- government panel shell
- debug logging and internal visibility
- existing-save initialization path

Phase 0 is complete only when those deliverables exist in a credible foundational form.

## What Has Already Been Implemented

The repository now contains a complete Phase 0 foundation baseline in credible foundational form.

Implemented:

- shared government state and contracts
- ruleset abstraction
- JSON-backed configuration foundation
- default democracy ruleset shell
- thin mod bootstrap integration
- persistence capture/restore skeleton
- explicit migration runner seam
- shared modifier pipeline seam
- government panel shell seam
- structured debug snapshot/internal visibility seam
- existing-save initialization path
- automated tests for the Phase 0 foundation

Key files already present:

### Shared core

- `src/GovernmentCS2.Core/GovernmentCS2.Core.csproj`
- `src/GovernmentCS2.Core/Contracts/GovernmentIdentifiers.cs`
- `src/GovernmentCS2.Core/Contracts/GovernmentStateModels.cs`
- `src/GovernmentCS2.Core/Configuration/GovernmentConfigurationModels.cs`
- `src/GovernmentCS2.Core/Configuration/GovernmentConfigLoader.cs`
- `src/GovernmentCS2.Core/Contracts/GovernmentLifecycleModels.cs`
- `src/GovernmentCS2.Core/Persistence/IGovernmentSaveMigrationStep.cs`
- `src/GovernmentCS2.Core/Persistence/GovernmentSaveMigrationRunner.cs`
- `src/GovernmentCS2.Core/Persistence/GovernmentPersistenceService.cs`
- `src/GovernmentCS2.Core/Presentation/GovernmentPanelShell.cs`
- `src/GovernmentCS2.Core/Runtime/GovernmentModifierPipeline.cs`
- `src/GovernmentCS2.Core/Runtime/GovernmentDebugInspector.cs`
- `src/GovernmentCS2.Core/Rulesets/IGovernmentRuleset.cs`
- `src/GovernmentCS2.Core/Rulesets/DemocracyGovernmentRuleset.cs`
- `src/GovernmentCS2.Core/Runtime/GovernmentModule.cs`

### Runtime configuration

- `config/government/core.json`
- `config/government/democracy.json`

### Mod bootstrap

- `GovernmentModHost.cs`
- `GovernmentCS2.csproj`
- `Mod.cs`

### Automated tests

- `tests/GovernmentCS2.Core.Tests/GovernmentCS2.Core.Tests.csproj`
- `tests/GovernmentCS2.Core.Tests/TestConfigFactory.cs`
- `tests/GovernmentCS2.Core.Tests/GovernmentConfigLoaderTests.cs`
- `tests/GovernmentCS2.Core.Tests/DemocracyGovernmentRulesetTests.cs`
- `tests/GovernmentCS2.Core.Tests/GovernmentModifierPipelineTests.cs`

## What Phase 0 Now Covers

The following roadmap Phase 0 deliverables now exist in code:

### Save/load skeleton

- `GovernmentPersistenceService` can capture, serialize, and restore government state
- `GovernmentModule.Initialize(...)` can bootstrap from serialized state or from fresh initialization context
- bootstrap remains game-safe by falling back to fresh initialization when no serialized government state is supplied

### Schema migration skeleton

- `GovernmentSaveMigrationRunner` and `IGovernmentSaveMigrationStep` now provide the explicit migration seam
- the current schema path works without migration
- unsupported older schemas fail explicitly instead of silently drifting

### Modifier pipeline for demand/confidence effects

- `GovernmentModifierPipeline` now owns shared clamping/composition behavior for government demand effects
- the ruleset still emits placeholder effects by design, but the shared pipeline seam now exists

### Government panel shell

- `GovernmentPanelShell` now provides the first dedicated panel host seam on top of the panel view model contract
- this is intentionally a shell, not a finished player-facing UI

### Debug logging and internal visibility

- `GovernmentDebugInspector` now captures structured internal state for inspection
- mod bootstrap logs now include configuration and panel-shell summaries
- debug is still development-oriented, which is acceptable for Phase 0

### Existing-save initialization path

- `GovernmentModule.Initialize(...)` now distinguishes between:
  - new-city initialization
  - existing-city seeding
  - restore from serialized government state
- the current path is intentionally foundational and does not yet read live CS2 household/economy data, which belongs to later work

## Phase 0 Execution Order

The intended Phase 0 execution order has now been satisfied in code.

The next roadmap boundary is `Phase 1: Democracy V1`, but that phase should not begin until the current Phase 0 work is reviewed and accepted.

## Explicit Non-Goals Until Phase 0 Is Done

Do not advance into these items yet:

- real bloc scoring
- real party movement
- election resolution
- approval versus legitimacy gameplay behavior
- political capital gameplay loop
- consultation gameplay
- corruption gameplay
- override-state gameplay

Those belong to later roadmap work.

## Verification Commands

Current baseline verification commands:

1. `dotnet test tests/GovernmentCS2.Core.Tests/GovernmentCS2.Core.Tests.csproj`
2. `dotnet build GovernmentCS2.csproj`

These verify the implemented Phase 0 foundation baseline.

## Exit Criteria for Phase 0

Phase 0 exit criteria are now satisfied by the current codebase:

- shared government ownership is real and stable
- persistence seams are real
- migration seam is real
- modifier pipeline seam is real
- UI shell seam exists
- debug visibility is usable
- existing-save initialization path exists

The project is now ready to begin `Phase 1: Democracy V1`, but that next phase should start as a distinct follow-on step rather than being mixed into the tail end of Phase 0.
