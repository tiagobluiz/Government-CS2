# Common Forms of Government

## Scope

This note focuses on four common or widely recognizable forms of government that are useful for gameplay design:

- parliamentary democracy
- presidential republic
- constitutional monarchy
- authoritarian dictatorship / one-party state

The goal is not legal perfection. The goal is to extract patterns we can turn into systems.

## Comparison snapshot

| Form | Who leads government | How leaders are chosen | How decisions get made | Main source of legitimacy | Main way order is maintained |
| --- | --- | --- | --- | --- | --- |
| Parliamentary democracy | Prime minister and cabinet | Parliament chooses or sustains government after elections | Cabinet governs while accountable to parliament | Elections plus parliamentary confidence | Consent, law, party discipline |
| Presidential republic | President | Usually direct or electoral popular vote | President executes laws, legislature passes them | National election and constitution | Consent, law, institutional checks |
| Constitutional monarchy | Prime minister governs; monarch is head of state | Voters elect parliament, monarch is hereditary | Government acts through parliament and cabinet | Elections plus tradition/constitution | Consent, law, symbolism, continuity |
| Authoritarian dictatorship / one-party state | Dictator, junta, or ruling party elite | Seizure, controlled elections, party appointment, or succession | Centralized command, decree, party hierarchy | Force, ideology, nationalism, performance claims | Police, military, surveillance, patronage |

## 1. Parliamentary democracy

### How it works

- Voters elect a legislature.
- The government is formed by the party or coalition that can command confidence in parliament.
- The prime minister is usually the head of government.
- The cabinet is typically drawn from the legislature or majority coalition.

### How leaders get elected or selected

- Citizens usually do not directly elect the prime minister as a separate executive office.
- Citizens elect representatives.
- The parliamentary majority or coalition determines who can govern.

### How the government keeps governing

- It must keep the confidence of parliament.
- Party discipline, coalition agreements, and cabinet solidarity are the main operating tools.
- If confidence is lost, the government can fall, be replaced, or trigger elections.

### Strengths

- High responsiveness to election outcomes
- Good fit for coalition-building and compromise
- Easier to remove an ineffective government without waiting for a fixed executive term

### Weaknesses

- Coalitions can be fragile
- Decision-making can slow down when multiple parties must agree
- Short-term political bargaining can block long-term infrastructure planning

### Gameplay translation

- High legitimacy when parliament reflects the city
- Lower action speed when the ruling coalition is fragmented
- Lower coup risk than dictatorship
- Higher cabinet reshuffle / coalition collapse risk than a strong presidency

## 2. Presidential republic

### How it works

- The president is both head of state and head of government in many presidential systems.
- The executive and legislature are institutionally separate.
- The president administers government, but legislation still depends on the legislature.

### How leaders get elected or selected

- The president is usually elected independently of the legislature, either directly or through an electoral system such as an electoral college.
- Legislators are elected separately.

### How the government keeps governing

- The president relies on constitutional authority, executive departments, budgets, appointments, and negotiation with the legislature.
- Government can continue even when the legislature is hostile, but major reforms become harder.

### Strengths

- Clear executive accountability
- Faster unilateral executive action in some domains
- More visible leadership during crises

### Weaknesses

- Deadlock is common when legislature and executive are politically opposed
- Strong personalization of politics
- Crisis powers can accumulate around one office

### Gameplay translation

- Faster emergency response than parliamentary coalition rule
- Medium-to-high policy friction when legislature is divided
- High visibility of the leader means big popularity swings matter more

## 3. Constitutional monarchy

### How it works

- The monarch remains head of state, usually by heredity.
- Day-to-day governing is performed by an elected prime minister and cabinet.
- The monarch is politically neutral in modern constitutional monarchies and performs ceremonial and constitutional functions.

### How leaders get elected or selected

- The monarch is not elected.
- The government is still usually formed through parliamentary elections.
- The monarch formally appoints the prime minister according to constitutional convention.

### How the government keeps governing

- Practical governing is maintained through parliament, law, and cabinet government.
- The monarchy adds symbolic continuity and non-partisan national identity.

### Strengths

- Strong continuity and ceremonial legitimacy
- Can reduce conflict over the head-of-state role
- Useful national symbol during crises and transitions

### Weaknesses

- Inherits a non-democratic element at the top of the state
- Can look outdated or unequal
- Real power can still be contested if constitutional norms are weak

### Gameplay translation

- Stable legitimacy bonus from tradition and continuity
- Similar practical governing logic to parliamentary democracy
- Lower symbolic volatility during government changes

## 4. Authoritarian dictatorship / one-party state

### How it works

- Power is concentrated in one leader, a military junta, or a narrow ruling elite.
- Elections may be absent, heavily manipulated, or non-competitive.
- Constitutional limits are weak or irrelevant in practice.

### How leaders get elected or selected

- Leaders may seize power by force, inherit party control, win sham elections, or be chosen within the ruling apparatus.
- Public participation, if present, usually does not meaningfully determine executive power.

### How the government keeps governing

- Through coercion, surveillance, censorship, propaganda, patronage, and control of security institutions.
- Some authoritarian governments also maintain support through economic performance or nationalist legitimacy.

### Strengths

- Very fast decision-making
- Very low legislative friction
- Can mobilize resources quickly in the short term

### Weaknesses

- Requires costly coercive institutions
- High corruption risk
- Poor feedback loops because dissent is suppressed
- Succession crises, coups, and unrest can be severe

### Gameplay translation

- Fastest policy implementation
- Highest ongoing security and repression upkeep
- Rising hidden instability when public demands are ignored too long

## Consent vs force

This is the most useful cross-cutting design lens for the mod.

### Governing by asking citizens

Typical tools:
- elections
- public consultation
- referenda
- legislative debate
- coalition negotiation

Benefits:
- higher legitimacy
- lower repression cost
- better information from society
- lower revolt risk when citizens feel represented

Costs:
- slower decisions
- watered-down reforms
- risk of indecision during urgent crises

### Governing by force

Typical tools:
- police repression
- military presence
- censorship
- decrees
- emergency rule

Benefits:
- very fast decisions
- easier to impose unpopular projects or austerity
- fewer formal veto points

Costs:
- requires larger armed or coercive institutions
- greater corruption and abuse risk
- weaker feedback from citizens
- legitimacy decays faster unless fear, ideology, or prosperity compensates

## Suggested gameplay variables

Note: these variables are exploratory and are not part of Democracy V1 unless they are explicitly promoted into [`docs/specs/*`](../specs). For current approved vocabulary and scope, treat the files under [`docs/specs`](../specs) as authoritative.

Across all four systems, the same city could track:

- legitimacy
- coercive capacity
- administrative capacity
- public trust
- elite loyalty
- reform speed
- consultation delay
- corruption pressure
- unrest risk

That would let us model a democracy and a dictatorship with the same base systems but very different parameter values.

## Sources

- UK Parliament on parliament and government: [parliament.uk/about/how/role/relations-with-other-institutions/parliament-government](https://www.parliament.uk/about/how/role/relations-with-other-institutions/parliament-government/)
- UK Parliament explainer on how parliamentary democracy works: [parliament.uk/globalassets/about-parliament/how-parliament-works/accessible-resources/how-the-uk-parliament-works-e-text-accessible.pdf](https://www.parliament.uk/globalassets/about-parliament/how-parliament-works/accessible-resources/how-the-uk-parliament-works-e-text-accessible.pdf)
- White House overview of U.S. government: [whitehouse.gov/government](https://www.whitehouse.gov/government/)
- White House overview of the executive branch: [whitehouse.gov/government/executive-branch](https://www.whitehouse.gov/government/executive-branch/)
- White House overview of the legislative branch: [whitehouse.gov/government/legislative-branch](https://www.whitehouse.gov/government/legislative-branch/)
- Royal Family on the role of the monarchy: [royal.uk/role-monarchy](https://www.royal.uk/role-monarchy)
- Royal Family on the sovereign and the prime minister: [royal.uk/the-sovereign-and-the-prime-minister](https://www.royal.uk/the-sovereign-and-the-prime-minister)
- Britannica on dictatorship: [britannica.com/topic/dictatorship](https://www.britannica.com/topic/dictatorship)
- Britannica on authoritarianism: [britannica.com/topic/authoritarianism](https://www.britannica.com/topic/authoritarianism)

## Caveats

- Real governments are hybrids, not pure categories.
- Many countries combine parliamentary, presidential, federal, monarchic, and authoritarian elements in ways that blur simple labels.
- For the mod, the important part is not taxonomy purity. It is whether the system creates distinct constraints, legitimacy patterns, and crisis behaviors.
