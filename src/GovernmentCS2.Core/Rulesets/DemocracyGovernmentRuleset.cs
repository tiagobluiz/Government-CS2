using System.Collections.Generic;
using System.Linq;
using GovernmentCS2.Core.Configuration;
using GovernmentCS2.Core.Contracts;

namespace GovernmentCS2.Core.Rulesets
{
    public sealed class DemocracyGovernmentRuleset : IGovernmentRuleset
    {
        public string RulesetId => GovernmentRulesetIds.Democracy;

        public GovernmentModelState InitializeNewCity(GovernmentConfigurationSet configurationSet, GovernmentInitializationContext initializationContext)
        {
            return BuildInitialState(configurationSet, initializationContext, false);
        }

        public GovernmentModelState SeedExistingCity(GovernmentConfigurationSet configurationSet, GovernmentInitializationContext initializationContext)
        {
            return BuildInitialState(configurationSet, initializationContext, true);
        }

        public GovernmentRuntimeState TickUpdate(GovernmentModelState currentState, GovernmentConfigurationSet configurationSet)
        {
            var demandEffects = EvaluateDemandEffects(currentState, configurationSet);
            var friction = EvaluateActionCost("phase1-default-major-action", currentState, configurationSet);

            return new GovernmentRuntimeState
            {
                CurrentRiskLevel = currentState.Legitimacy >= 50f ? "Stable" : "Watch",
                LastSupportUpdateTimestamp = 0L,
                CurrentDemandEffects = demandEffects,
                CurrentActionFrictionSummary = friction,
                CurrentGovernmentStatusFlags = new List<string>
                {
                    "phase-0-foundation",
                    "democracy-shell-active"
                }
            };
        }

        public GovernmentDemandEffects EvaluateDemandEffects(GovernmentModelState currentState, GovernmentConfigurationSet configurationSet)
        {
            return new GovernmentDemandEffects
            {
                ResidentialModifier = 0f,
                CommercialModifier = 0f,
                IndustrialModifier = 0f,
                OfficeModifier = 0f,
                ConfidenceChannelContribution = 0f,
                PolicyDirectionChannelContribution = 0f,
                ReasonCodes = new List<string>
                {
                    "phase-0-demand-effects-placeholder"
                }
            };
        }

        public GovernmentActionCostResult EvaluateActionCost(string actionId, GovernmentModelState currentState, GovernmentConfigurationSet configurationSet)
        {
            var cost = configurationSet.Democracy.PoliticalCapital.DefaultMajorActionCost;
            var warningThreshold = configurationSet.Democracy.PoliticalCapital.MajorActionWarningThreshold;

            return new GovernmentActionCostResult
            {
                PoliticalCapitalCost = cost,
                ApprovalRisk = cost >= warningThreshold ? 5f : 0f,
                LegitimacyRisk = 0f,
                BlocReactionSummary = new List<string>
                {
                    $"Phase 0 placeholder reaction for '{actionId}'."
                },
                PartyReactionSummary = new List<string>
                {
                    "No party-specific reaction is calculated yet."
                },
                WarningLevel = cost >= warningThreshold ? WarningLevel.Moderate : WarningLevel.Low,
                IsHardBlocked = false
            };
        }

        public GovernmentPanelViewModel BuildPanelViewModel(GovernmentModelState currentState, GovernmentConfigurationSet configurationSet)
        {
            return new GovernmentPanelViewModel
            {
                GovernmentTypeName = "Democracy",
                Approval = currentState.Approval,
                Legitimacy = currentState.Legitimacy,
                ElectionCountdown = $"{currentState.ElectionCycle.DefaultTermLengthYears} in-game years per term",
                CurrentRiskLevel = currentState.Legitimacy >= 50f ? "Stable" : "Watch",
                PoliticalCapital = currentState.PoliticalCapital,
                CorruptionPressure = currentState.CorruptionPressure,
                BlocCards = currentState.BlocSnapshots.ToList(),
                PartyCards = currentState.PartySnapshots.ToList(),
                DemandEffectSummary = EvaluateDemandEffects(currentState, configurationSet),
                OverrideStateSummary = currentState.OverrideState.IsElectionOverrideActive ? "Override active" : "No override",
                UnlockLayerSummary = currentState.CurrentUnlockLayer.ToString(),
                ActionWarnings = new List<string>
                {
                    "Phase 0 UI summary only. Real political warnings come in later slices."
                }
            };
        }

        private static GovernmentModelState BuildInitialState(
            GovernmentConfigurationSet configurationSet,
            GovernmentInitializationContext initializationContext,
            bool seededFromExistingCity)
        {
            var democracy = configurationSet.Democracy;
            var context = initializationContext ?? new GovernmentInitializationContext();
            var unlockLayer = democracy.StartingUnlockLayer;

            if (seededFromExistingCity && context.CurrentMilestoneLevel >= 10)
            {
                unlockLayer = GovernmentUnlockLayer.Layer3;
            }
            else if (seededFromExistingCity && context.CurrentMilestoneLevel >= 5)
            {
                unlockLayer = GovernmentUnlockLayer.Layer2;
            }

            return new GovernmentModelState
            {
                ActiveRulesetId = GovernmentRulesetIds.Democracy,
                SchemaVersion = configurationSet.Core.SchemaVersion,
                CurrentUnlockLayer = unlockLayer,
                Approval = democracy.StartingApproval,
                Legitimacy = democracy.StartingLegitimacy,
                PoliticalCapital = democracy.StartingPoliticalCapital,
                CorruptionPressure = democracy.StartingCorruptionPressure,
                ElectionCycle = new ElectionCycleState
                {
                    // Phase 0 treats CurrentGameTime as "in-game years elapsed" so it can share units with TermLengthYears.
                    CurrentTermIndex = seededFromExistingCity ? 1 : 0,
                    CurrentTermStartGameTime = context.CurrentGameTime,
                    CurrentTermEndGameTime = context.CurrentGameTime + democracy.TermLengthYears,
                    DefaultTermLengthYears = democracy.TermLengthYears
                },
                OverrideState = new GovernmentOverrideState(),
                BlocSnapshots = democracy.Blocs.Select(CreateBlocSnapshot).ToList(),
                PartySnapshots = democracy.Parties.Select(CreatePartySnapshot).ToList(),
                DistrictPoliticalSeeds = new Dictionary<string, float>(),
                RulesetRuntimeState = new DemocracyRuntimeState
                {
                    ConsultationState = unlockLayer >= GovernmentUnlockLayer.Layer3 ? "Available" : "Locked"
                }
            };
        }

        private static PoliticalBlocSnapshot CreateBlocSnapshot(BlocConfig blocConfig)
        {
            return new PoliticalBlocSnapshot
            {
                BlocId = blocConfig.Id,
                DisplayName = blocConfig.DisplayName,
                CurrentSupport = 50f,
                Trend = PoliticalTrend.Stable,
                TopPositiveDrivers = new List<string>
                {
                    "Phase 0 baseline state"
                },
                TopNegativeDrivers = new List<string>(),
                PrimaryDemandChannels = blocConfig.PrimaryDemandChannels.ToList(),
                DistrictSensitivityMode = blocConfig.DistrictSensitive ? DistrictSensitivityMode.Light : DistrictSensitivityMode.None
            };
        }

        private static PartyStandingSnapshot CreatePartySnapshot(PartyConfig partyConfig)
        {
            return new PartyStandingSnapshot
            {
                PartyId = partyConfig.Id,
                DisplayName = partyConfig.DisplayName,
                CurrentStanding = 50f,
                Trend = PoliticalTrend.Stable,
                BlocAffinitySummary = partyConfig.AffinityBlocIds.ToList(),
                IssueAlignmentSummary = new List<string>
                {
                    "Phase 0 baseline alignment"
                },
                TopPositiveDrivers = new List<string>
                {
                    "Phase 0 baseline state"
                },
                TopNegativeDrivers = new List<string>()
            };
        }
    }
}
