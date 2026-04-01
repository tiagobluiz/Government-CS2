# Democracy V1 Specification

## Executive Summary

This document is the primary implementation specification for the first government type in the mod: `Democracy V1`.

The goal of Democracy V1 is to add a municipal democratic government layer to Cities: Skylines II without turning the game into a separate political simulator. The player remains a city-builder, but now has to stay in power through elections, maintain public legitimacy, and absorb political consequences when using taxes, budgets, policies, and major planning decisions.

Democracy V1 is intentionally:

- municipal, not national
- generic democracy first, not a parliamentary/presidential split
- mid-depth, not a full political simulation
- extensible toward later governments
- strongly integrated with existing CS2 systems

This specification is written to be usable by an engineering agent with no prior conversation context.

## V1 Player Experience

The player should experience democracy as:

- a visible but readable political layer
- a recurring reelection pressure loop
- a source of meaningful tradeoffs around taxes, services, and growth
- a system that rewards healthy city management but still tempts short-term popular choices
- a native-feeling extension of the existing CS2 experience

The intended player loop is:

1. Govern the city.
2. Citizens and voter blocs react to outcomes and policy choices.
3. Parties convert social pressure into political pressure.
4. Elections occur on a fixed cadence.
5. The player tries to remain in power while keeping the city functional and growing.

The player should not feel like they are playing a detached visual novel, council simulator, or parliamentary procedure simulator.

## In-Scope vs Out-of-Scope

### In scope

- one generic democracy ruleset
- elections on a fixed cadence
- approval and legitimacy as separate concepts
- six voter blocs
- three broad fixed parties
- lightweight political capital
- bounded direct influence on all four demand bars
- strong integration with taxes, service budgets, and selected city policies
- limited consultation
- lightweight corruption
- a narrow election-loss override state
- support for existing saves
- explicit schema migrations
- a dedicated government panel plus lightweight HUD signals
- hidden debug tooling

### Out of scope

- parliamentary vs presidential split
- a formal chamber or council simulation
- emergency powers
- broad democratic backsliding systems
- deep media mechanics
- procedural party generation
- district-first election resolution
- a full dictatorship implementation
- highly configurable rule matrices

## Core Concepts and Terms

### Approval

`Approval` is the short-to-mid-term measure of how satisfied the electorate is with the current government.

Approval should move based on:

- household living conditions
- taxes
- jobs and unemployment
- housing pressure
- service quality
- service budget changes
- politically salient policy choices
- recent controversial major decisions

Approval is volatile enough to matter before the next election.

### Legitimacy

`Legitimacy` is the perceived right of the government to rule.

Legitimacy should move more slowly than approval, but major events can change it sharply.

Examples:

- a government may have mediocre approval after a necessary tax increase, but still retain strong legitimacy if elections are respected and the city broadly works
- a government that overrides an election may keep control of the city, but should suffer severe legitimacy damage

Legitimacy influences:

- demand confidence effects
- recovery speed from political damage
- severity of election loss consequences
- perceived democratic credibility

### Political capital

`Political capital` is a lightweight action-friction resource.

It is not a full legislature simulation. It exists to limit how many controversial or politically meaningful actions the player can push through in a short period.

Political capital should apply mainly to:

- tax changes
- major service budget changes
- selected city policies
- major spending moves
- major zoning actions

Ordinary city interaction must remain responsive.

### Voter bloc

A `voter bloc` is a readable political grouping that represents a family of city interests rather than a literal legal voting class.

Blocs are used to:

- make approval legible
- explain why different groups react differently
- feed party standing and election results
- support future governments later

### Party

A `party` in Democracy V1 is a broad election actor, not a full AI institution.

Parties are used to:

- aggregate bloc sentiment into political positioning
- make elections more readable
- add identity to democratic competition

### District seed

The `district seed` model is a lightweight district-level political layer stored from v1.

Its purpose is:

- minor localized political effects
- more informative UI
- future extensibility

It is intentionally not the main source of truth in v1. Citywide support remains primary.

## Democracy Ruleset Overview

Democracy V1 is a municipal government ruleset with fixed elections and bounded governing friction.

Default behavior decisions:

- default term length: `4 in-game years`
- election presentation: `light interrupt`
- support model: `citywide primary + district seed`
- recalculation cadence: `periodic tick`
- save strategy: `minimal versioned state`
- migration style: `explicit schema migrations`
- debug visibility: `hidden debug panel + logging`
- unlock structure: `3 layers`
- party count: `3`
- bloc count: `6`
- direct demand influence: `all 4 demand bars`
- modifier strength: `small bounded direct modifiers`
- mod interoperability posture: `defensive by default`
- existing save support: `yes`

Democracy V1 should feel safer and more legitimate than later authoritarian systems, but slower and more politically constrained.

## Political Support Model

The support model is a blended citywide system informed by existing CS2 simulation signals.

Primary support inputs:

- household conditions
- taxes
- jobs and unemployment
- housing pressure and availability
- service quality
- service budgets
- selected city policies
- bounded district-local variation

### Source of truth

The primary source of truth for political support in v1 is citywide support.

Reasons:

- it is more stable
- it is cheaper to calculate
- it is easier to explain
- it is easier to balance

### District seed purpose

District seed data exists in v1 to support:

- minor localized weighting
- district-level insight in the UI
- future district expansion without a save-model reset

District logic is intentionally minor in v1 because:

- district-first politics would add large UI and balancing complexity
- the first government system needs to stay city-builder-friendly
- future governments may want district usage shaped differently

### Extensibility requirement

The support model must be implemented in a way that future governments can reuse the same broad support infrastructure but interpret it differently.

Example:

- democracy may use support for elections and approval
- dictatorship may use a related model for loyalty, fear, unrest, and elite stability

## Bloc Definitions

The initial bloc set is:

- Households
- Workers
- Business
- Students
- Seniors
- Order/Environment

### Households

Represents:

- ordinary resident households
- general family and home-life concerns
- broad city livability sentiment

Primary inputs:

- residential taxes
- housing availability
- affordability pressure
- service quality affecting daily life
- neighborhood quality and general city satisfaction

Secondary inputs:

- public transport quality
- healthcare access
- education access
- city cleanliness and safety

Likely friendly policies:

- pro-housing expansion
- stronger public services
- affordability-friendly tax policy
- improvements to daily quality of life

Likely hostile policies:

- sharp residential tax increases
- service cuts that affect daily living
- chronic housing shortages
- visible decline in livability

Most affected demand channels:

- residential most strongly
- commercial secondarily through consumer confidence

District sensitivity in v1:

- yes, mildly district-sensitive

### Workers

Represents:

- employed residents
- job seekers
- wage and working-condition concerns
- labor-side reaction to city growth choices

Primary inputs:

- employment level
- job availability
- commute friction where readable
- industrial/commercial health
- service access that affects work-life stability

Secondary inputs:

- education access for job mobility
- transit reliability
- tax pressure on working households

Likely friendly policies:

- job creation
- stable services
- transit that supports work access
- balanced taxation that does not overly squeeze working households

Likely hostile policies:

- job loss
- severe unemployment
- service degradation that harms working life
- perceived favoritism toward business without resident benefit

Most affected demand channels:

- industrial
- commercial
- residential indirectly

District sensitivity in v1:

- yes, mildly district-sensitive

### Business

Represents:

- employers
- investors
- office and commercial confidence
- growth-oriented private-sector interests

Primary inputs:

- commercial taxes
- industrial taxes
- office taxes
- growth posture
- service reliability important to business operations

Secondary inputs:

- education pipeline
- infrastructure reliability
- logistics confidence
- policy signals around development

Likely friendly policies:

- pro-growth policy
- stable operating environment
- moderate tax burden
- infrastructure and workforce investment

Likely hostile policies:

- abrupt tax hikes on firms
- anti-growth policy moves
- visible instability
- unreliable services that harm operations

Most affected demand channels:

- office
- commercial
- industrial

District sensitivity in v1:

- lightly district-sensitive

### Students

Represents:

- current students
- education-oriented young residents
- future mobility and opportunity concerns

Primary inputs:

- education funding
- school and university access
- affordability and housing access for younger residents

Secondary inputs:

- transit availability
- quality-of-life services
- job pipeline for educated residents

Likely friendly policies:

- education investment
- transit investment
- opportunity-oriented development
- affordable housing support

Likely hostile policies:

- education cuts
- stagnant opportunity
- policies perceived as anti-future or anti-mobility

Most affected demand channels:

- residential
- office indirectly

District sensitivity in v1:

- lightly district-sensitive

### Seniors

Represents:

- older residents
- service reliability concerns
- stability and quality-of-life priorities

Primary inputs:

- healthcare quality
- public safety
- transit usability
- tax stability
- neighborhood stability

Secondary inputs:

- housing suitability
- environmental quality
- service continuity

Likely friendly policies:

- healthcare investment
- safe and stable neighborhoods
- predictable taxes
- reliable transit and services

Likely hostile policies:

- healthcare cuts
- rapid destabilizing change
- severe service degradation
- visible disorder

Most affected demand channels:

- residential primarily

District sensitivity in v1:

- lightly district-sensitive

### Order/Environment

Represents:

- residents who prioritize safety, cleanliness, order, and environmental quality
- this bloc intentionally combines two real-world preferences into one readable gameplay bloc for v1

Primary inputs:

- policing quality
- visible disorder and instability
- pollution and environmental strain
- policy tone around control versus sustainability

Secondary inputs:

- transit and traffic management
- neighborhood quality
- land-use externalities

Likely friendly policies:

- order-oriented service investment
- environmental protection
- clean-city measures
- predictable, stable governance

Likely hostile policies:

- visible disorder
- heavy pollution
- neglect of safety or environmental damage
- chaotic planning

Most affected demand channels:

- residential
- commercial indirectly through city attractiveness

District sensitivity in v1:

- yes, this bloc is one of the better candidates for district-local variation

## Party Definitions

Democracy V1 uses three fixed broad parties:

- Growth
- Civic
- Order

These parties are not deep AI governments. They are lightweight standing trackers that make elections and political pressure easier to read.

### Growth

Core issue cluster:

- development
- business confidence
- investment
- expansion
- jobs and productivity

Bloc over-index:

- Business strongly
- Workers moderately when growth also creates jobs

Gains support from:

- strong office/commercial outlook
- visible job creation
- pro-development policies
- stable business environment

Loses support from:

- anti-growth policy choices
- hostile tax posture toward firms
- stagnation
- operational instability

Implicit election message:

- "keep the city expanding and opportunity-rich"

Strongest gameplay movers:

- tax policy on business-facing sectors
- growth-oriented planning decisions
- infrastructure that improves business confidence

### Civic

Core issue cluster:

- public services
- livability
- fairness
- housing
- education
- general civic health

Bloc over-index:

- Households strongly
- Students strongly
- Seniors moderately

Gains support from:

- strong services
- affordability-sensitive decisions
- housing supply support
- visible resident welfare improvements

Loses support from:

- resident-facing austerity
- housing neglect
- education cuts
- visible decline in everyday quality of life

Implicit election message:

- "govern for the city as a place to live"

Strongest gameplay movers:

- residential tax choices
- education and healthcare budgets
- housing-related policies
- welfare/service policy posture

### Order

Core issue cluster:

- stability
- safety
- predictability
- discipline
- clean governance tone

Bloc over-index:

- Order/Environment strongly
- Seniors moderately
- Households moderately when instability rises

Gains support from:

- safe and orderly city conditions
- reliable policing and civic control
- stable governance tone

Loses support from:

- visible instability
- severe disorder
- unmanaged deterioration
- policies read as chaotic or reckless

Implicit election message:

- "keep the city stable, safe, and governable"

Strongest gameplay movers:

- police and order-related budgets
- environmental/order policy choices
- visible instability or governance failure

## Approval, Legitimacy, and Political Capital

### Approval

Approval is the quickest-moving top-line popularity metric.

It should:

- update from a periodic weighted evaluation
- respond meaningfully to taxes, budgets, services, jobs, and housing
- be visible in the HUD and government panel

### Legitimacy

Legitimacy is slower and more structural than approval.

It should:

- be affected by democratic credibility
- drop sharply from explicit anti-democratic acts such as election override
- support better confidence and recovery when healthy

Approval and legitimacy must not be collapsed into one number.

### Political capital

Political capital is a limited action-friction resource.

Purpose:

- avoid infinite politically costly moves in a short period
- make major decisions feel governed, not free
- keep ordinary play fast by targeting only politically important actions

Political capital should be spent by:

- tax changes
- major service budget changes
- selected city policies
- major spending commitments
- major zoning actions

Political capital should broadly regenerate from:

- time passing
- acceptable approval
- acceptable legitimacy
- stable governance periods

Political capital should broadly regenerate more slowly when:

- approval is weak
- legitimacy is weak
- the city is politically unstable
- the player recently pushed several controversial actions

Hard blocks should be avoided in favor of:

- cost previews
- reaction previews
- warnings about political consequences

Reason:

- CS2 must remain playable as a city-builder
- political friction should shape decisions, not constantly freeze them

## Election Cycle and Resolution

### Cadence

- default term length is 4 in-game years
- elections occur on a fixed cadence
- the cadence is part of the saved government state

### Evaluation timing

Election results should be evaluated using the government state accumulated across the current term, with strongest weighting on recent conditions and the final lead-in to the vote.

This supports the intended reelection pressure:

- short-term popular choices can matter
- long-term city collapse still matters
- players can feel electoral timing pressure

### Inputs to election resolution

Election resolution must include:

- overall approval
- bloc support
- party standings
- legitimacy pressure

Recommended interpretation:

- approval measures current popularity
- bloc support measures distribution of support across city interests
- party standings create readable electoral identity
- legitimacy modifies whether the government is seen as a proper democratic incumbent

### Player-visible election flow

The election should be a `light interrupt`.

Player flow:

1. An election alert appears.
2. The government panel highlights the current electoral state.
3. A compact results surface shows:
   - incumbent outcome
   - party standings
   - bloc movement summary
   - any legitimacy warning
4. The player returns quickly to normal city play.

### Win/loss definition

The player wins the election if the incumbent democratic government remains in office according to the election resolution.

The player loses if the incumbent fails reelection.

Default behavior after loss:

- the save does not hard-end
- the player may remain in power by overriding the result

## Major Action Friction Rules

Democracy V1 should apply friction to the following surfaces:

- taxes
- major service budget changes
- selected city policies
- major spending decisions
- major zoning moves

### Major zoning moves

Ordinary zoning should remain responsive and direct.

Only major zoning moves should trigger political consequences, such as:

- large citywide rezoning posture changes
- particularly visible contentious land-use decisions
- large interventions that clearly signal government priorities

For Democracy V1, a `major zoning move` should be understood as a zoning decision that is politically legible to citizens because it changes the city's direction, not just because the player painted a few cells.

This should be interpreted using three tests:

- `scale`: the move affects a large visible area, such as several city blocks, a corridor, or a meaningful part of a district
- `type shift`: the move changes the social or economic character of an area in a visible way
- `salience`: the move clearly affects housing, jobs, affordability, pollution, traffic, or district identity

Recommended implementation rule for v1:

- treat a zoning change as major if it clearly meets at least two of those three tests
- aggregate repeated small zoning changes over a short window so the player cannot bypass political consequences by painting the same district in tiny pieces

### Concrete in-game examples of major zoning moves

The following should count as major zoning moves in v1:

- painting a whole new neighborhood or district primarily as `Low Density Residential`, `Medium Density Housing`, or `Medium Density Row Housing`
- pushing a deliberate densification plan by converting a broad area from `Low Density Residential` into `Medium Density Housing`, `Mixed Housing`, or `High Density Housing`
- creating a large `Low Rent Housing` program in one part of the city, because it is a politically visible affordability decision
- turning several downtown blocks from `Commercial` or `Mixed Housing` into `Office`, especially if this visibly shifts the area toward jobs instead of homes
- rezoning a broad central corridor into `Mixed Housing` to create a denser mixed-use urban center
- creating or greatly expanding a large `Industrial` belt near existing residential districts, because the move is highly visible and has clear pollution, traffic, and jobs implications
- rezoning a large area away from housing into `Office` or `Industrial`, because the move changes who the city is being built for

The following should generally not count as major zoning moves in v1:

- filling a few missing cells of `Low Density Residential` in an existing suburb
- adding a small strip of `Commercial` along a neighborhood road
- tweaking a few `Office` lots while finishing an existing district
- doing routine cleanup zoning while following an already-established area pattern

### Important boundary for v1

This definition is mainly about `growable zoning` decisions made with the zoning tools.

`Specialized industry areas` are not literally normal zoning in CS2, but large specialized industry placement should later be treated with similar political logic because it is just as visible and politically meaningful as a major industrial zoning wave.

### Reaction model

Each politically meaningful action should expose:

- political capital cost
- likely bloc reaction
- likely party reaction
- any likely approval or legitimacy impact

The player should be warned. The player should rarely be fully blocked.

## Demand Integration Rules

Politics affects all four demand bars:

- Residential
- Commercial
- Industrial
- Office

### Modifier philosophy

Demand effects must be:

- bounded
- compositional
- additive onto the existing demand model

They must not:

- replace core CS2 demand logic
- dominate city fundamentals

City fundamentals must remain the primary driver.

### Two demand channels

Democracy affects demand through two channels.

#### Confidence and legitimacy channel

This captures whether the city feels governable, predictable, and socially credible.

Examples:

- healthy legitimacy can improve city confidence
- low legitimacy can weaken confidence and slow recovery
- stable democratic operation can modestly help demand

#### Policy-direction channel

This captures the player's visible governing posture.

Examples:

- pro-housing posture can nudge residential demand
- pro-business posture can nudge office or commercial demand
- anti-growth or unstable taxation can reduce business-side demand

### Intended demand influence by bar

#### Residential

Main democratic influences:

- trust in government
- housing availability and affordability
- resident-facing services
- household confidence

#### Commercial

Main democratic influences:

- consumer stability
- city activity confidence
- household willingness to spend
- general civic health

#### Industrial

Main democratic influences:

- labor confidence
- logistics confidence
- tax posture
- growth posture

#### Office

Main democratic influences:

- business confidence
- education and skilled workforce posture
- service reliability
- growth stability

## Budget and Policy Integration Rules

Democracy V1 must strongly reuse existing CS2 surfaces:

- taxes
- all major service budgets
- selected city policies

This is mandatory because the political layer should feel native to CS2.

### Taxes

Why politically relevant:

- taxes are among the most visible policy levers the player already understands
- different groups feel different tax changes differently

Expected reaction style:

- immediate reaction signal
- then slower secondary effects through city conditions

Tax pain must be asymmetric by taxed target.

### All major service budgets

Why politically relevant:

- services are core to municipal governance
- they directly shape daily lived city conditions
- they provide clear voter-facing outcomes

Expected reaction style:

- mixed immediate and delayed effects

Immediate:

- citizens react politically to visible budget decisions

Delayed:

- real city outcomes later reinforce or weaken the initial reaction

Initial politically salient service families include, at minimum:

- education
- healthcare
- police
- transit

But Democracy V1 should treat all major service budgets as eligible pressure points.

### Selected city policies

The first policy themes are:

- housing
- welfare/services
- business/growth
- environment/order

Why politically relevant:

- they map directly onto blocs and parties
- they are more expressive than raw numbers alone
- they help define government identity

Expected reaction style:

- usually mixed immediate and delayed

## District Seed Model

District data in v1 exists for:

- minor gameplay effects
- UI insight
- future extensibility

Districts should not drive the whole political simulation.

The district seed model should broadly store lightweight aggregated local sentiment signals, not deep district election history.

Recommended uses in v1:

- localized bloc flavor
- localized explanation of why some neighborhoods are politically hotter than others
- modest weighting or small event hooks later within democracy v1

Not recommended in v1:

- district seat allocation
- district-level electoral maps as the main result engine

## Democracy Progression Layers

Democracy V1 uses three explicit unlock layers.

Unlocks should be driven by a mix of:

- city milestones
- election progression

The intent is to keep early gameplay understandable while allowing later political depth.

### Layer 1

Includes:

- approval
- election timer
- top-line blocs

### Layer 2

Includes:

- parties
- political capital
- major decision friction

### Layer 3

Includes:

- consultation
- corruption
- district effects

### Messaging requirement

Unlocks must be communicated explicitly.

The player should be told:

- what unlocked
- why it unlocked
- what new decisions now matter

## Corruption and Consultation

### Corruption

Corruption is in scope for Democracy V1, but only in lightweight form.

Purpose:

- create a cost to unstable, self-serving, or credibility-damaging governance
- provide a reusable pressure type for later governments

Corruption should not yet become:

- a full patronage simulator
- a large event tree
- a deep elite-management subsystem

Corruption should be able to:

- increase slowly from harmful governance patterns
- slightly worsen trust and recovery behavior
- act as a penalty amplifier in later democratic layers

### Consultation

Consultation is in scope for Democracy V1, but in limited form.

Purpose:

- express the tradeoff between democratic responsiveness and decision speed
- create a legitimacy-positive alternative to pure top-down action

Consultation should:

- apply only to selected major moves
- cost time or momentum rather than feeling like a required procedure on every action
- provide legitimacy upside when used well

Consultation should not yet become:

- a referendum simulator on every issue
- a replacement for the core approval/election loop

### Explicit exclusions

The following are out of scope for Democracy V1:

- emergency powers
- broad democratic backsliding systems
- full authoritarian branching

The only anti-democratic exception in scope is the narrow election-override state described in this document.

## Override State After Election Loss

This section is intentionally explicit because it is easy to misunderstand.

### Core rule

Losing reelection does not hard-end the save by default.

The player can remain in power by overriding the election result.

### Scope boundary

This override state is:

- a narrow exception path
- not a full dictatorship ruleset
- not silent democratic backsliding

### UI and messaging requirement

The override must be presented very explicitly.

The player should never mistake this for normal democratic continuation.

Messaging should make clear that:

- the election was lost
- the democratic outcome was overridden
- political and economic penalties are now in effect

### Default penalties

The default override penalties are:

- legitimacy damage
- economic confidence penalty
- slower political recovery

### Sandbox exception

A sandbox-style setting can disable override penalties.

Even when penalties are disabled, the UI should still be clear about what the setting changed.

## Save Data and Migration

### Persistence philosophy

Democracy V1 must use minimal versioned persistence.

Persist the core state the government system truly owns. Derive or reseed everything else when practical.

### Minimum persisted fields

The persisted model must at minimum include:

- current ruleset id
- current unlock layer
- election schedule
- override-state flags
- legitimacy
- approval
- bloc scores
- party standings
- political capital
- corruption pressure
- district seed aggregates
- schema version

### Data that should remain derived where practical

The following should generally not be persisted unless implementation requires it:

- temporary explanation strings
- transient notification state
- recalculated presentation summaries
- UI-only grouping state
- debug-only display aggregates

### Existing save initialization

Existing saves must be supported.

If a save has no government data:

- initialize Democracy V1 as the current ruleset
- seed approval and legitimacy from current city conditions
- seed blocs and parties from current city signals
- initialize election timing with a sensible starting schedule
- set default unlock layer according to city maturity if desired, or start safely at the lowest compatible layer

### Migration rules

Save evolution must use explicit schema migrations.

Each new schema version should:

- identify the prior version(s) it upgrades
- define exact field additions or transformations
- avoid silent data loss where possible

## UI/UX Requirements

### Main presentation

Democracy V1 should live primarily in a dedicated government panel.

That panel should contain:

- current government type
- approval
- legitimacy
- election date or countdown
- top-line risk indicator
- bloc state
- party standings
- political capital
- active modifiers or major political effects

### Always-visible political signals

The HUD should keep a small persistent political summary visible:

- approval
- election timing
- immediate political risk

### Demand explanation

Government-related demand explanations should appear in:

- the government panel
- relevant tooltips where possible

The player should be told clearly when government confidence or a policy direction is helping or hurting a demand bar.

## Balancing Rules

Democracy V1 should feel:

- more legitimate than later authoritarian systems
- more politically constrained than a pure sandbox
- rewarding when the city is healthy
- punishing when the player chases short-term popularity at the expense of real city health

It should not feel:

- constantly blocked
- overwhelmingly numerical
- like politics dominates every other system

## Debug and Telemetry

Democracy V1 must include hidden debug visibility.

Required debug capabilities:

- support breakdown visibility
- bloc weighting visibility
- party standing visibility
- demand modifier breakdown visibility
- election result breakdown visibility
- override penalty visibility

Debug delivery:

- hidden debug panel
- structured logging where appropriate

## Acceptance Criteria

Democracy V1 is acceptable when all the following are true.

### Initialization and persistence

- New saves initialize with valid democratic government state.
- Existing saves initialize with seeded democratic state safely.
- Save/load preserves all core government fields.
- Schema migration upgrades old political saves forward correctly.

### Elections and support

- Election cadence follows the configured 4-year default.
- Support updates on the intended periodic cadence.
- Election resolution uses approval, bloc support, party standings, and legitimacy.
- Election results are presented as a light interrupt.

### Gameplay integration

- Tax changes create asymmetric political reactions.
- Major service budget changes affect political state.
- Selected policy themes affect relevant blocs and parties.
- Major zoning actions have political consequences while ordinary zoning remains responsive.
- All four demand bars receive bounded democratic influence.

### Progression and special states

- The three democracy layers unlock in a clear staged fashion.
- Unlock messaging is explicit.
- Election override state is explicit and not mistaken for normal democracy.
- Default override penalties affect legitimacy, confidence, and recovery.
- Sandbox no-penalty mode works without breaking the rest of the system.

### UX and clarity

- Government panel communicates the main political state clearly.
- HUD surfaces approval, election timing, and risk clearly.
- Demand explanations reveal government-related causes clearly.

### Compatibility and maintainability

- The implementation avoids invasive rewrites of the vanilla demand model.
- The design degrades reasonably around mod interoperability issues.
- Hidden debug instrumentation is available for balancing.

## Future Extension Hooks

The democracy implementation must leave clean extension points for future rulesets.

At minimum, future governments should be able to replace or reinterpret:

- how power is retained
- how legitimacy is calculated
- whether elections exist
- how action friction works
- how corruption behaves
- how demand confidence is produced

Democracy V1 should therefore be implemented as the first ruleset on top of a shared government core, not as the whole government architecture.
