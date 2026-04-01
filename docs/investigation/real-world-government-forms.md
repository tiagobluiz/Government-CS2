# Real-World Government Forms Investigation

## Scope

This note focuses on four common or influential government forms that are useful reference points for our mod design:

- parliamentary representative democracy
- presidential republic
- constitutional monarchy
- authoritarian dictatorship / one-party state

These are not the only forms of government, but they are enough to give us a strong design foundation.

## 1. Parliamentary representative democracy

### What it is

In a parliamentary system, the party or coalition with the greatest representation in parliament forms the government, and its leader becomes prime minister.

### How leaders are selected

- Citizens elect representatives to parliament.
- The leader of the majority party, or a coalition leader, becomes prime minister.
- Cabinet members are usually drawn from parliament.

### How decisions get made

- The executive and legislature are closely linked.
- Government policy is usually easier to pass when the government has a working majority.
- Coalition bargaining is common in multi-party systems.

### How the government keeps governing

- It must retain the **confidence** of parliament.
- If it loses that confidence, the prime minister can be removed and a new government or election may follow.

### Strengths

- High responsiveness to election outcomes.
- Usually better at passing legislation when a majority exists.
- Coalition systems can force negotiation and compromise.

### Weaknesses

- Coalitions can be unstable.
- Governments can collapse if support fragments.
- Long negotiations can slow urgent action.

### Gameplay-relevant levers

- council majority or coalition majority
- confidence meter
- coalition demands
- compromise bonuses and instability penalties

### Design note

This is a strong model for a city council simulation because it naturally turns politics into an ongoing negotiation instead of a yes/no popularity check.

## 2. Presidential republic

### What it is

A presidential system separates the executive from the legislature. The president is both head of state and head of government.

### How leaders are selected

- Citizens elect the president directly or indirectly.
- In the United States, the president is chosen through the Electoral College process rather than direct national popular vote.
- The cabinet is appointed by the president, not drawn from a legislative majority.

### How decisions get made

- Executive and legislature have separate mandates.
- The president has constitutionally defined powers, but legislation still depends on the legislature.
- Checks and balances can slow action but also limit concentrated power.

### How the government keeps governing

- The president usually serves a fixed term.
- Losing legislative support does not automatically remove the president.
- Governing can continue through negotiation, veto power, executive action, and legislative compromise.

### Strengths

- Stable executive tenure.
- Clear accountability for executive leadership.
- Better insulation from sudden coalition collapse.

### Weaknesses

- Deadlock between executive and legislature can stall policy.
- Separate elections can produce conflicting mandates.
- Crisis action may tempt executive overreach.

### Gameplay-relevant levers

- executive orders vs normal legislation
- vetoes
- legislature approval thresholds
- fixed-term stability with gridlock risk

### Design note

This is useful if we want a strong-mayor system where the player remains in office for a term but still has to negotiate with council or institutions to actually implement changes.

## 3. Constitutional monarchy

### What it is

In a constitutional monarchy, the monarch is head of state, but elected institutions govern. In the UK model, the monarch is politically impartial and legislation is made by elected parliament.

### How leaders are selected

- The monarch inherits the head-of-state role.
- The prime minister is appointed by the monarch under constitutional convention and must be able to command the confidence of the elected lower house.

### How decisions get made

- Day-to-day policy is set by elected officials.
- The monarch performs constitutional and ceremonial functions, not normal partisan governing.
- This structure separates symbolic continuity from operational political rule.

### How the government keeps governing

- The elected government still needs parliamentary confidence.
- The monarchy helps provide continuity, legitimacy, and national identity, but does not usually govern directly.

### Strengths

- Institutional continuity and symbolism.
- Reduced personalization of day-to-day executive power in the head of state.
- Can stabilize transitions by separating ceremonial legitimacy from partisan conflict.

### Weaknesses

- Hereditary office can feel undemocratic.
- The ceremonial structure can obscure where accountability really sits.
- In edge cases, reserve powers and constitutional conventions can be ambiguous.

### Gameplay-relevant levers

- legitimacy and continuity bonus
- ceremonial approval or public tradition score
- low direct executive power for the monarch
- parliamentary confidence still required for real policy

### Design note

For our mod, this is probably less useful as a literal city form, but it is useful as a model for separating **symbolic legitimacy** from **operational governing power**.

## 4. Authoritarian dictatorship / one-party state

### What it is

A dictatorship concentrates power in one person or a small group without effective constitutional limits. A one-party state is a system where one political party controls the government in law or in practice.

### How leaders are selected

- Leadership may come through force, fraud, party promotion, military control, or tightly managed elections.
- In a one-party state, real power often sits with party leadership rather than the nominal head of state.

### How decisions get made

- Decision-making is faster because opposition veto points are weak or absent.
- Orders can be implemented through hierarchy, security institutions, censorship, and party discipline.

### How the government keeps governing

- Through force, coercion, surveillance, propaganda, censorship, elite bargains, or monopoly party control.
- Some authoritarian systems also seek performance legitimacy by delivering order, jobs, or growth.

### Strengths

- Fast decision-making.
- High short-term coordination capacity.
- Few formal obstacles to sweeping policy change.

### Weaknesses

- Requires stronger coercive capacity to suppress dissent.
- Information quality can degrade because subordinates fear telling the truth.
- Legitimacy is brittle when performance drops.
- Abuse risk is much higher because checks are weak.

### Force vs asking citizens

As a design principle:

- **Asking citizens** is slower, but it builds legitimacy, consent, and better long-term stability.
- **Forcing decisions** is faster, but it increases resentment, resistance, enforcement cost, and dependence on police or military capacity.

That tradeoff is excellent game material. A player should feel that authoritarian efficiency is real, but expensive and dangerous to sustain.

### Gameplay-relevant levers

- coercion capacity
- censorship / information control
- unrest suppression
- elite loyalty
- propaganda
- legitimacy decay when repression rises

### Design note

If we include this path in the mod, it should not just be "easy mode." It should trade faster execution for higher unrest risk, weaker information quality, and heavier enforcement costs.

## Cross-form comparison for mod design

### Elections and leadership turnover

- Parliamentary democracy: indirect executive selection through parliament; removal via lost confidence.
- Presidential republic: separate executive mandate; fixed term.
- Constitutional monarchy: hereditary head of state, but elected government.
- Authoritarian / one-party: no fully open transfer mechanism, or transfer is controlled by regime insiders.

### How power is maintained

- Parliamentary democracy: majority support and coalition management.
- Presidential republic: constitutional tenure plus bargaining with legislature.
- Constitutional monarchy: elected government plus symbolic continuity from the crown.
- Authoritarian / one-party: coercion, party control, propaganda, and managed elite loyalty.

### Speed vs legitimacy

- Parliamentary majority government: medium to high speed, medium to high legitimacy.
- Coalition parliament: slower speed, broader buy-in.
- Presidential republic: medium speed, but deadlock risk.
- Authoritarian system: high speed, but fragile legitimacy and higher repression cost.

## Recommendation for our mod

For a city-government simulation, the most reusable ideas are:

- **parliamentary / coalition logic** for council politics
- **presidential logic** for a strong-mayor variant
- **authoritarian logic** for emergency or anti-democratic paths
- **constitutional-monarchy logic** mainly as a reference for symbolic legitimacy vs executive authority

These are post-V1 design inputs only: `parliamentary / coalition logic`, `presidential logic`, and `constitutional-monarchy logic` should be explored only after the Democracy V1 foundations in [`docs/specs`](../specs) have been validated.

## Sources

- [Britannica: Parliamentary system](https://www.britannica.com/topic/parliamentary-system)
- [The Royal Family: The Sovereign and the Prime Minister](https://www.royal.uk/the-sovereign-and-the-prime-minister)
- [The Royal Family: The role of the Monarchy](https://www.royal.uk/the-role-of-the-monarchy)
- [USAGov: Electoral College](https://www.usa.gov/electoral-college)
- [Britannica: Presidential systems and separation of powers](https://www.britannica.com/topic/constitutional-law/Executives-and-legislatures)
- [Britannica: Dictatorship](https://www.britannica.com/topic/dictatorship)
- [Britannica: One-party state](https://www.britannica.com/topic/one-party-state)

## Source-backed facts vs inference

Source-backed:

- parliamentary systems depend on majority or coalition support
- prime ministers can fall when they lose parliamentary confidence
- the UK monarch is politically impartial and legislation resides with elected parliament
- the UK prime minister must command Commons confidence
- U.S. presidents are selected through the Electoral College
- presidential systems separate executive and legislature
- dictatorships concentrate power without effective constitutional limits
- one-party states centralize power in a single ruling party

Inference:

- the exact gameplay levers proposed here
- the city-scale translation of national systems into mayor/council mechanics
- the explicit speed-versus-legitimacy framework as a balancing model
