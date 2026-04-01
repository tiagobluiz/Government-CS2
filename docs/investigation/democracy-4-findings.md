# Democracy 4 Findings

## Purpose

This note captures what Democracy 4 appears to model well, which government patterns it supports directly, and which additional government forms we can reasonably derive for a Cities: Skylines 2 government mod.

## High-confidence sourced findings

### Core player role

- Democracy 4 puts the player in the role of `President / Prime minister`.
- The game is explicitly about governing while staying popular enough to be re-elected.

Implication for us:
- The baseline loop is not "build whatever you want", but "govern under political constraints".

### Core systemic pillars

From official/store and developer sources, Democracy 4 clearly models:

- elections and re-election pressure
- popularity with different voter groups
- political capital as a hard limit on how much a government can do
- coalition governments
- emergency powers during crises
- three-party systems
- corruption
- press freedom / media effects
- modern policy tradeoffs and delayed consequences

This matters because the game is less about static government types and more about the friction of holding power.

### Political capital is a central governing constraint

- Political capital is intentionally limited.
- The developer explicitly said two Democracy 4 mechanics rely on limited political capital:
  - emergency powers, which grant more capital during bad situations
  - coalitions, where partners offer political capital in exchange for specific actions
- The developer also stated that high voter popularity can increase available political capital.

Implication for our mod:
- A government form should not only change available actions.
- It should also change governing bandwidth, bargaining costs, and crisis response speed.

### Coalition governments are first-class

- Coalition governments are listed as one of the major Democracy 4 features.
- The July 2020 developer notes mention support for coalition government and additional ways to gain political capital.
- Forum comments from the developer confirm that coalition partners can trade political capital for policy concessions.

Implication for our mod:
- "No outright majority" should be a real state, not just flavor.
- Minority rule and coalition bargaining should create instability, slower reform, and deal-driven policy compromises.

### Emergency powers are crisis accelerators

- Democracy 4 includes emergency powers that activate in critical situations.
- Emergency powers increase political capital and help governments push through otherwise difficult changes.
- The system exists to let governments act faster in crises.

Implication for our mod:
- Emergency powers are a useful bridge between democracy and authoritarianism.
- They are ideal for modeling how states centralize authority during war, disaster, economic collapse, epidemics, or civil disorder.

### Electoral systems matter

The Voting Systems DLC adds or highlights:

- first-past-the-post vs proportional representation
- campaign finance rules
- voter eligibility rules
- turnout-shaping systems like compulsory voting
- state funding and campaign restrictions

Implication for our mod:
- The same government form can behave very differently depending on election rules.
- "Form of government" and "electoral system" should probably be separate but connected systems.

## What Democracy 4 directly supports

These are directly supported or strongly evidenced by the sourced material:

### 1. Electoral democracy

Requirements to keep it:
- retain enough popularity to win elections
- keep key voter blocs from defecting or abstaining
- manage policy costs through political capital

Likely gameplay pressures:
- slow reform pace
- broad accountability
- risk of losing office after unpopular but necessary decisions

### 2. Coalition democracy

Requirements to keep it:
- maintain enough parliamentary or electoral support to form a government
- satisfy coalition partners often enough to avoid collapse
- spend political capital carefully because coalition bargaining consumes it

Likely gameplay pressures:
- slower decision-making
- more compromises
- increased policy instability

### 3. Crisis government under emergency powers

Requirements to keep it:
- a crisis severe enough to justify exceptional powers
- enough legitimacy to avoid backlash while exercising those powers

Likely gameplay pressures:
- faster action
- temporary suspension of normal political constraints
- danger of democratic norms weakening if emergency rule becomes routine

## Government forms we can infer from Democracy 4

These are not all explicit selectable government types in Democracy 4. They are design inferences based on its mechanics.

### 4. Majoritarian presidential or prime-ministerial government

Why it fits:
- the player is a singular executive
- the game centers on policy direction, re-election, and governing capacity

Requirements to keep it:
- win elections
- maintain popularity
- preserve enough cabinet loyalty / governing capacity to keep reform moving

Best translated gameplay:
- decisive leadership when popularity is strong
- legitimacy from elections
- strong downside when public opinion turns quickly

### 5. Illiberal or managed democracy

Why it fits:
- Democracy 4 models corruption, press freedom, campaign controls, voting rules, and emergency powers
- those mechanics can be combined into a democracy that still holds elections but increasingly rigs the playing field

Requirements to keep it:
- maintain enough public legitimacy to avoid open revolt
- keep media, opposition, and institutions weakened enough to prevent replacement
- use crises, propaganda, or patronage to justify concentration of power

Best translated gameplay:
- easier short-term control
- higher long-term corruption, unrest, elite capture, and legitimacy decay

### 6. Soft dictatorship or one-party state

Why it fits:
- not a default advertised Democracy 4 government form
- but it is a logical endpoint if emergency powers, reduced freedoms, corruption, and suppression mechanics are extended further

Requirements to keep it:
- coercive capacity: police, military, intelligence, patronage networks
- propaganda or ideological legitimacy
- suppression of organized opposition
- enough economic performance or fear to prevent regime collapse

Best translated gameplay:
- fastest implementation speed
- weakest accountability
- highest maintenance cost in coercion and internal security
- severe risk of corruption, coups, insurgency, or elite fragmentation

## Recommended design takeaway for our CS2 mod

### V1 scope guardrail

This section is architecture guidance beyond Democracy V1, not the implementation source of truth for V1.

For current implementation decisions, use [`docs/specs/democracy-v1-spec.md`](../specs/democracy-v1-spec.md). Items such as emergency powers, government-form splits, coalition/parliamentary variants, and broader authoritarian branching are deferred to future phases unless they are formally promoted into the specs.

Democracy 4 suggests that we should model government in layers:

1. Governing structure
- presidential
- parliamentary
- coalition
- monarchy-backed parliamentary
- authoritarian

2. Legitimacy source
- elections
- tradition
- ideology
- force
- performance

3. Governing friction
- political capital
- coalition bargaining
- bureaucratic delay
- referendum delay
- emergency override

4. Order maintenance
- citizen consent
- law and institutions
- patronage
- policing
- military coercion

That layered approach is likely more useful than a flat dropdown of government names.

## Early design rules we can borrow

- Democracies should be slower but more stable when citizens broadly consent.
- Coalition governments should be harder to steer but more representative.
- Emergency powers should increase short-term capacity but erode long-term legitimacy if overused.
- Authoritarian systems should act faster but require a larger security apparatus and suffer stronger corruption risks.
- Electoral systems should shape who counts, who votes, and how fragmented the government becomes.

## Source notes

### Primary sources used

- Democracy 4 Steam page: [store.steampowered.com/app/1410710/Democracy_4](https://store.steampowered.com/app/1410710/Democracy_4/)
- Democracy 4 Voting Systems DLC Steam page: [store.steampowered.com/app/2004030/Democracy_4__Voting_Systems](https://store.steampowered.com/app/2004030/Democracy_4__Voting_Systems/)
- "Announcing Democracy 4" developer post: [positech.co.uk/cliffsblog/2018/09/14/announcing-democracy-4](https://www.positech.co.uk/cliffsblog/2018/09/14/announcing-democracy-4/)
- Developer blog summary for July 2020: [positech.co.uk/cliffsblog/2020/07](https://www.positech.co.uk/cliffsblog/2020/07/)
- Developer forum note on emergency powers: [forums.positech.co.uk/t/emergency-powers/14754](https://forums.positech.co.uk/t/emergency-powers/14754)
- Developer forum note on political capital and emergencies: [forums.positech.co.uk/t/political-power-cap/15150](https://forums.positech.co.uk/t/political-power-cap/15150)
- Developer forum note on political capital and coalitions: [forums.positech.co.uk/t/request-let-political-capital-being-disabled/16045](https://forums.positech.co.uk/t/request-let-political-capital-being-disabled/16045)

## Confidence and caveats

- High confidence: the game's baseline loop, coalition support, emergency powers, limited political capital, election systems, and voter-management framing.
- Medium confidence: using those mechanics to define broader government archetypes for our mod.
- Low confidence unless we inspect the game data directly later: exact hidden thresholds, exact emergency triggers, and whether the live game meaningfully supports fully authoritarian end states as more than an emergent interpretation.
