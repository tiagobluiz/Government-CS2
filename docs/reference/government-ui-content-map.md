# Government UI Content Map

## UI Principles

The government UI should make the political layer readable without making it feel like a separate game bolted onto CS2.

Guiding principles:

- political detail belongs primarily in one dedicated panel
- the most important political signals should remain visible at a glance
- existing CS2 surfaces should be reused where it feels natural
- explanations should be explicit

## Government Panel Information Architecture

The government panel is the main home for political detail.

Shared-foundation note:

- the current codebase already includes `GovernmentPanelViewModel` and `GovernmentPanelShell` as the first implementation seam for this panel
- later UI work should extend those contracts instead of inventing a disconnected surface
- the panel shell may overlay post-pipeline runtime values so the panel matches the live runtime/debug state

Recommended top-level sections:

- government summary
- election status
- approval and legitimacy
- voter blocs
- party standings
- political capital and corruption
- demand impact summary
- recent major political effects

### Government summary

Content:

- current government type
- current democracy layer
- current overall political risk
- active special state such as election override

### Election status

Content:

- next election countdown
- incumbent reelection outlook
- summary of recent political trend

### Approval and legitimacy

Content:

- current approval
- current legitimacy
- short explanation of the difference
- recent drivers up/down

### Voter blocs

Content:

- six bloc cards
- current support
- trend
- top positive and negative drivers

### Party standings

Content:

- three party cards
- current standing
- trend
- issue identity

### Political capital and corruption

Content:

- current political capital
- current corruption pressure
- short explanation of what each affects

### Demand impact summary

Content:

- current government influence on residential demand
- current government influence on commercial demand
- current government influence on industrial demand
- current government influence on office demand
- explanation split between confidence and policy-direction channels

## Always-Visible Political Signals

The HUD should keep a minimal political footprint visible at all times.

Required always-visible items:

- approval
- election timing
- immediate political risk

Purpose:

- keep the player aware of democratic pressure without needing the panel open

## Demand Explanation Surfaces

Government-related demand explanations should appear in two places:

- government panel
- existing demand-related tooltips where feasible

Explanation requirements:

- clearly state whether the effect comes from confidence/legitimacy or policy direction
- use plain cause-and-effect language
- player-facing warnings must remain readable copy; internal diagnostics should stay out of `ActionWarnings`

## Election Presentation Flow

Elections are a `light interrupt`.

The intended UI flow:

1. Election alert/notification appears.
2. Player can open the election summary quickly.
3. A compact election result surface shows:
   - incumbent outcome
   - current party standings
   - key bloc changes
   - any legitimacy warning
4. Player returns quickly to normal city play.

The election UI should not:

- become a long decision tree
- replace the normal game loop
- hide the result in a tiny notification

## Override-State Messaging

If the player loses an election but remains in power, the UI must say so directly.

Required messaging content:

- the election was lost
- the result was overridden
- democracy has been compromised
- penalties are now active unless sandbox setting disables them

Recommended presentation locations:

- election result surface
- government summary section
- risk indicator styling

## Unlock Messaging

The democracy system uses three layers, and each unlock should be explicit.

Unlock message should explain:

- what new political feature was unlocked
- why it unlocked
- what the player should now pay attention to

## Debug Visibility Model

Debug information should not clutter normal player UI.

Recommended debug approach:

- hidden debug panel
- developer-only or hidden toggle access
- detailed breakdowns for:
  - approval
  - legitimacy
  - bloc support
  - party standing
  - demand modifiers
  - election resolution
  - override penalties

Purpose:

- balancing
- troubleshooting
- future extension to additional government types
