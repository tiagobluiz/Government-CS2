# Democracy V1 Data Contracts

## Naming Conventions

This document defines conceptual contracts, not final source-code syntax.

The purpose is to give implementers stable mental models for:

- runtime state
- save state
- ruleset boundaries
- demand effect output
- election output
- UI view-model output

The exact language syntax may differ during implementation, but the ownership and semantics should remain stable.

Recommended naming principles:

- `State` for owned mutable runtime state
- `Snapshot` for read-only calculated state at a moment in time
- `Result` for resolved output from a computation or player action
- `ViewModel` for UI-facing data
- `SaveData` for persisted state

Recommended ownership principles:

- runtime state belongs to owned `State` objects
- persisted state belongs to versioned `SaveData`
- tunable balance/configuration data belongs to validated JSON configuration models loaded separately from save data
- transient ruleset-only runtime details belong to typed ruleset runtime contracts and should not be serialized accidentally

## Phase 0 Implementation Note

The current codebase already includes the first concrete forms of the contracts described here.

Implemented concrete types now include:

- `GovernmentModelState`
- `GovernmentRuntimeState`
- `GovernmentSaveDataV1`
- `GovernmentInitializationContext`
- `GovernmentBootstrapResult`
- `GovernmentDebugSnapshot`
- `IGovernmentRulesetRuntimeState`
- `DemocracyRuntimeState`
- `GovernmentPanelViewModel`
- `GovernmentDemandEffects`
- `GovernmentActionCostResult`

## Runtime State Objects

## Configuration Objects

Democracy V1 should separate runtime state from configuration data.

Configuration data is not the same thing as save data:

- configuration defines how the ruleset behaves by default
- save data defines what state the current city is in

Recommended initial file layout:

- `config/government/core.json`
- `config/government/democracy.json`

Recommended conceptual configuration models:

- `GovernmentCoreConfig`
- `DemocracyConfig`
- `BlocConfig`
- `PartyConfig`
- `ElectionConfig`
- `PoliticalCapitalConfig`
- `DemandEffectConfig`
- `ProgressionConfig`
- `OverridePenaltyConfig`

Configuration contract rules:

- configuration should be loaded and validated at startup or first use
- configuration should fail loudly in development when required fields are missing or invalid
- configuration should own weights, thresholds, caps, affinities, and tunable mappings
- configuration should not try to encode arbitrary procedural logic

### `GovernmentModelState`

Top-level owned state for the current city government.

Responsibilities:

- identifies the active government ruleset
- stores current core political values
- points to government-specific runtime state
- stores persistent political timers and flags

Important ownership rule:

- this object should not become a dumping ground for tunable constants that belong in configuration

Suggested fields:

- `ActiveRulesetId`
- `SchemaVersion`
- `CurrentUnlockLayer`
- `Approval`
- `Legitimacy`
- `PoliticalCapital`
- `CorruptionPressure`
- `ElectionCycle`
- `OverrideState`
- `BlocSnapshots`
- `PartySnapshots`
- `DistrictPoliticalSeeds`
- `RulesetRuntimeState`

Current implementation notes:

- `ActiveRulesetId` is the runtime source-of-truth ruleset identifier
- `SchemaVersion` is persisted and restored through `GovernmentSaveDataV1`
- `RulesetRuntimeState` is now typed as `IGovernmentRulesetRuntimeState`
- `RulesetRuntimeState` is explicitly transient and is deliberately not carried by `GovernmentSaveDataV1`

### `GovernmentRuntimeState`

Shared runtime shell for cross-government computed state.

Responsibilities:

- stores current computed outputs shared across rulesets
- provides a consistent place for debug and UI reads
- owns the canonical current demand-effect output at runtime

Important ownership rule:

- this object may cache references or snapshots of active configuration for debug visibility, but it should not own the source of truth for tunable configuration values

Suggested fields:

- `CurrentRiskLevel`
- `LastSupportUpdateTimestamp`
- `CurrentDemandEffects`
- `CurrentActionFrictionSummary`
- `CurrentGovernmentStatusFlags`

Current implementation notes:

- `CurrentDemandEffects` is passed through the shared modifier pipeline before becoming the canonical runtime output
- `CurrentGovernmentStatusFlags` is the current lightweight debug/status seam for cross-cutting runtime markers
- player-facing warning text must not be mixed into `CurrentGovernmentStatusFlags`

### `DemocracyRuntimeState`

Democracy-specific runtime data that does not belong in the shared shell.

Responsibilities:

- democracy support evaluation
- party standings
- election-specific pressure state
- consultation state

Suggested fields:

- `ElectionPressureScore`
- `RecentPolicyDirectionSummary`
- `ConsultationState`
- `RecentBlocShiftSummary`
- `RecentPartyShiftSummary`
- `RecentOverridePenaltyState`

### `IGovernmentRulesetRuntimeState`

Typed base contract for ruleset-specific transient runtime state.

Responsibilities:

- give rulesets a typed place to store non-shared runtime details
- avoid using `object` for ruleset runtime state
- make ephemeral-vs-persistent ownership explicit

Important rule:

- this state is recalculated or re-seeded at runtime and must not be serialized into `GovernmentSaveDataV1`

## Save Objects

### `GovernmentSaveDataV1`

Minimal versioned persisted data for the first government implementation.

This object persists city-specific political state. It does not persist the ruleset's normal tuning tables.

Required persisted fields:

- `SchemaVersion`
- `RulesetId`
- `UnlockLayer`
- `Approval`
- `Legitimacy`
- `PoliticalCapital`
- `CorruptionPressure`
- `ElectionCycleState`
- `OverrideState`
- `BlocScores`
- `PartyStandings`
- `DistrictSeedAggregates`

Fields that should generally remain derived rather than persisted:

- temporary explanation strings
- last-opened UI tab
- transient notification state
- recomputable debug-only summaries

Fields that should generally remain configuration-owned rather than persisted:

- election coefficients
- bloc weight tables
- party affinity tables
- demand caps
- political-capital costs
- unlock thresholds

Explicit exclusion:

- `RulesetRuntimeState` is not part of `GovernmentSaveDataV1`
- debug snapshots are not part of `GovernmentSaveDataV1`

### `ElectionCycleState`

Persisted election schedule and current term state.

Suggested fields:

- `CurrentTermIndex`
- `CurrentTermStartGameTime`
- `CurrentTermEndGameTime`
- `DefaultTermLengthYears`
- `LastElectionResult`

Current Phase 0 convention:

- `CurrentTermStartGameTime` and `CurrentTermEndGameTime` currently use the same abstract in-game-year unit as `DefaultTermLengthYears`

## Ruleset Interface Contract

### `IGovernmentRuleset`

The core interface that every government type implements.

Responsibilities:

- initialize ruleset state
- reseed ruleset state for existing saves
- update runtime political state
- produce government-specific demand effects
- evaluate action friction
- expose ruleset-specific UI summaries

Suggested methods:

- `InitializeNewCity(...)`
- `SeedExistingCity(...)`
- `TickUpdate(...)`
- `EvaluateDemandEffects(...)`
- `EvaluateActionCost(...)`
- `ResolveMajorGovernmentEvent(...)`
- `BuildPanelViewModel(...)`

Suggested configuration-oriented methods or collaborators:

- `LoadConfig(...)`
- `ValidateConfig(...)`
- or a separate ruleset configuration provider injected into the ruleset

Important rule:

The interface should not assume elections always exist. Democracy uses elections, but future governments may not.

Important configurability rule:

The ruleset should consume typed configuration models rather than reading ad hoc constants spread across multiple systems.

## Demand Modifier Contract

### `GovernmentDemandEffects`

The shared output contract for government impact on demand.

Responsibilities:

- carry bounded direct effects for the four demand bars
- explain their origin at a high level

Suggested fields:

- `ResidentialModifier`
- `CommercialModifier`
- `IndustrialModifier`
- `OfficeModifier`
- `ConfidenceChannelContribution`
- `PolicyDirectionChannelContribution`
- `ReasonCodes`

Contract rules:

- modifiers must be bounded
- modifiers compose with existing demand logic
- modifiers do not replace or rewrite the vanilla demand engine
- the canonical runtime owner is `GovernmentRuntimeState.CurrentDemandEffects`
- this object should generally remain derived rather than separately persisted in `GovernmentModelState`

Configuration note:

- caps, coefficients, and channel weights that produce these modifiers should be owned by configuration objects, not by the result object itself

## Election Result Contract

### `ElectionResolutionResult`

The resolved output of a democracy election.

Responsibilities:

- declare incumbent win/loss
- summarize why the outcome happened
- provide enough data for UI and debug views

Suggested fields:

- `IncumbentWon`
- `ApprovalContribution`
- `LegitimacyContribution`
- `BlocContributionSummary`
- `PartyContributionSummary`
- `FinalScore`
- `RiskWarnings`
- `OutcomeReasonSummary`

## Bloc Snapshot Contract

### `PoliticalBlocSnapshot`

The current readable state of a single bloc.

Responsibilities:

- expose current support value
- expose trend direction
- expose major causes
- expose district-sensitivity where relevant

Suggested fields:

- `BlocId`
- `DisplayName`
- `CurrentSupport`
- `Trend`
- `TopPositiveDrivers`
- `TopNegativeDrivers`
- `PrimaryDemandChannels`
- `DistrictSensitivityMode`

Configuration note:

- bloc thresholds, sensitivities, and issue weights should live in configuration

## Party Snapshot Contract

### `PartyStandingSnapshot`

The current readable state of a single party.

Responsibilities:

- expose current strength
- explain what bloc and issue signals are lifting or hurting it

Suggested fields:

- `PartyId`
- `DisplayName`
- `CurrentStanding`
- `Trend`
- `BlocAffinitySummary`
- `IssueAlignmentSummary`
- `TopPositiveDrivers`
- `TopNegativeDrivers`

Configuration note:

- party affinity matrices and issue sensitivity tables should live in configuration

## Government Panel View Model Contract

### `GovernmentPanelViewModel`

The UI-facing structure for the main government panel.

Responsibilities:

- present all player-relevant political state in one normalized structure

Suggested fields:

- `GovernmentTypeName`
- `Approval`
- `Legitimacy`
- `ElectionCountdown`
- `CurrentRiskLevel`
- `PoliticalCapital`
- `CorruptionPressure`
- `BlocCards`
- `PartyCards`
- `DemandEffectSummary`
- `OverrideStateSummary`
- `UnlockLayerSummary`
- `ActionWarnings`

Current Phase 0 note:

- this contract is already used by the `GovernmentPanelShell` as the first government panel seam
- `GovernmentPanelShell` may overlay post-pipeline runtime values such as risk and demand summary onto the panel view model
- `ActionWarnings` is reserved for readable player-facing copy; internal phase/debug markers belong in runtime diagnostics instead

## Migration Contract

### `GovernmentMigrationStep`

A conceptual migration unit for moving save data from one schema version to another.

Responsibilities:

- declare source and target schema
- transform persisted government state
- preserve semantics intentionally

Suggested fields or members:

- `FromVersion`
- `ToVersion`
- `ApplyMigration(...)`
- `MigrationNotes`

Migration contract rules:

- migration must be explicit
- migration should be deterministic
- migration should avoid silent resets unless absolutely required

Configuration compatibility note:

- migrations should assume configuration may evolve separately from saved runtime state, so saved fields should remain semantic rather than duplicating fragile tuning tables

Current implementation note:

- `GovernmentSaveMigrationRunner` is the current concrete migration seam
- unsupported or malformed save payloads should fail explicitly rather than silently degrading into schema `0`

## Government Action Cost Contract

### `GovernmentActionCostResult`

The output of evaluating a politically meaningful player action.

Responsibilities:

- describe political-capital cost
- summarize likely support reaction
- summarize likely party reaction
- expose warnings without necessarily blocking the action

Suggested fields:

- `PoliticalCapitalCost`
- `ApprovalRisk`
- `LegitimacyRisk`
- `BlocReactionSummary`
- `PartyReactionSummary`
- `WarningLevel`
- `IsHardBlocked`

Important default:

`IsHardBlocked` should usually be false in democracy v1. The main pattern is cost plus warning, not broad denial of action.

Configuration note:

- cost curves, warning thresholds, and soft-block heuristics should be configuration-owned wherever practical

## Example Save JSON

This example illustrates the intended shape of `GovernmentSaveDataV1`.

```json
{
  "SchemaVersion": 1,
  "RulesetId": "democracy",
  "UnlockLayer": 1,
  "Approval": 50.0,
  "Legitimacy": 60.0,
  "PoliticalCapital": 50.0,
  "CorruptionPressure": 5.0,
  "ElectionCycleState": {
    "CurrentTermIndex": 0,
    "CurrentTermStartGameTime": 0,
    "CurrentTermEndGameTime": 4,
    "DefaultTermLengthYears": 4,
    "LastElectionResult": {
      "IncumbentWon": false,
      "ApprovalContribution": 0.0,
      "LegitimacyContribution": 0.0,
      "BlocContributionSummary": {},
      "PartyContributionSummary": {},
      "FinalScore": 0.0,
      "RiskWarnings": [],
      "OutcomeReasonSummary": ""
    }
  },
  "OverrideState": {
    "IsElectionOverrideActive": false,
    "PenaltiesDisabled": false,
    "Reason": ""
  },
  "BlocScores": [],
  "PartyStandings": [],
  "DistrictSeedAggregates": {}
}
```
