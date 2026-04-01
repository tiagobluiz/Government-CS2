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

## Runtime State Objects

### `GovernmentModelState`

Top-level owned state for the current city government.

Responsibilities:

- identifies the active government ruleset
- stores current core political values
- points to government-specific runtime state
- stores persistent political timers and flags

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

### `GovernmentRuntimeState`

Shared runtime shell for cross-government computed state.

Responsibilities:

- stores current computed outputs shared across rulesets
- provides a consistent place for debug and UI reads
- owns the canonical current demand-effect output at runtime

Suggested fields:

- `CurrentRiskLevel`
- `LastSupportUpdateTimestamp`
- `CurrentDemandEffects`
- `CurrentActionFrictionSummary`
- `CurrentGovernmentStatusFlags`

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

## Save Objects

### `GovernmentSaveDataV1`

Minimal versioned persisted data for the first government implementation.

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

### `ElectionCycleState`

Persisted election schedule and current term state.

Suggested fields:

- `CurrentTermIndex`
- `CurrentTermStartGameTime`
- `CurrentTermEndGameTime`
- `DefaultTermLengthYears`
- `LastElectionResult`

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

Important rule:

The interface should not assume elections always exist. Democracy uses elections, but future governments may not.

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
