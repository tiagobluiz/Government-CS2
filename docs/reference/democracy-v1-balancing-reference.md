# Democracy V1 Balancing Reference

## Balancing Goals

The purpose of this reference is to protect the feature from drifting into either of two bad extremes:

- politics is so weak that it feels cosmetic
- politics is so strong that it overwhelms the base CS2 city simulation

The target feel is:

- visible
- readable
- strategically meaningful
- bounded

## What Democracy Should Feel Like

Democracy should feel like:

- a city government with reelection pressure
- a system where taxes, service budgets, and policy direction have political meaning
- a regime that gains strength from consent and credibility
- a system that sometimes tempts short-term popular decisions over long-term city health

The player should feel:

- accountable
- constrained, but not trapped
- rewarded for healthy city management
- exposed to political consequences when pushing unpopular actions

## What Democracy Should Not Feel Like

Democracy should not feel like:

- constant permission-seeking
- a full legislature simulator
- a total conversion of the economy
- a UI-heavy spreadsheet with no city-builder rhythm
- a regime where one number decides everything

## Demand Modifier Strength Rules

Government demand effects must be small and bounded.

Core rule:

- government modifies demand
- government does not replace demand

Recommended tuning posture:

- the player should notice politics in demand explanations
- the player should not be able to fix a fundamentally broken city with political bonuses alone
- the player should not lose all demand solely because of politics if the city fundamentals remain strong

Demand tuning guidance:

- Residential should be the most visibly linked to trust, services, affordability, and household confidence.
- Office should be the most visibly linked to business confidence, education posture, and growth stability.
- Commercial should react to shopper confidence and general city vitality.
- Industrial should react to labor confidence, logistics confidence, and growth posture.

## Political Capital Tuning Rules

Political capital exists to shape tempo, not to paralyze gameplay.

Tuning rules:

- ordinary city actions should remain mostly free of political capital friction
- major actions should be expensive enough to force prioritization
- capital regeneration should reward stable, legitimate governance
- capital should not crash to zero so easily that the player cannot recover

Warning pattern:

- favor previews and warnings
- use hard blocks sparingly

## Approval/Legitimacy Sensitivity Rules

Approval:

- should react fast enough for the player to see consequences before the next election
- should move from taxes, services, housing pressure, and jobs strongly enough to matter

Legitimacy:

- should move more slowly than approval during ordinary governance
- should drop sharply after anti-democratic acts such as overriding an election
- should influence recovery and confidence differently from approval

Important distinction:

- a government can be unpopular yet legitimate
- a government can remain in control yet lack legitimacy

## Election Frequency Tuning

The default term is 4 in-game years.

Why:

- 3 years would keep politics more constantly active
- 5 years would reduce pressure too much for the intended v1 feel
- 4 years is the intended middle ground

Tuning goals:

- elections should arrive often enough to matter
- elections should not dominate every session
- recent governance should matter enough that pre-election behavior is strategically meaningful

## Override Penalty Tuning

The override state is the narrow path where the player loses an election and remains in power anyway.

Default penalty focus:

- legitimacy damage
- economic confidence penalty
- slower political recovery

Tuning goals:

- the player should feel a real cost
- the player should still be able to continue playing the city
- the state should not quietly normalize into standard democracy

Avoid:

- instant unwinnable spiral
- trivial penalties the player can ignore

## Readability Rules

The player should be able to answer:

- why approval moved
- which blocs are unhappy
- which party is gaining
- why a demand bar changed politically
- what a major action is likely to cost politically

If the player cannot answer those questions, the system is under-explained.

Shared-foundation note:

- `GovernmentDemandEffects`, `GovernmentActionCostResult`, `WarningLevel`, and `GovernmentDebugSnapshot` are the current contract seams that later balancing and explanation work should continue to use

## Anti-Frustration Rules

- do not put friction on every click
- keep ordinary zoning responsive
- show predicted reaction before controversial actions
- stage complexity through unlocks
- keep elections a light interrupt

## Anti-Exploit Rules

- avoid single-action loops that permanently farm approval or political capital
- avoid one bloc or party becoming a universal answer for every city
- avoid settings defaults that let the player trivialize the core democracy loop unintentionally
- avoid unbounded stacking demand effects

## Compatibility-Safe Tuning Rules

The mod should be tuned in a way that remains reasonably safe around other gameplay mods.

That means:

- prefer additive modifiers over full simulation replacement
- prefer reading existing state over overriding large systems
- keep caps explicit
- keep debug output rich enough to detect interaction issues quickly
