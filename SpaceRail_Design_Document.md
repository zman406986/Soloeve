# Game Design Doctrine: Project SpaceRail

## 1. Core Vision & Pillars

Project SpaceRail is a single-player space sandbox defined by systemic realism and physics-driven tactics. Unlike arcade shooters, gameplay is governed by consistent physical laws (specifically Spacetime mechanics), creating a reactive simulation where player agency is expressed through engineering, logistics, and tactical positioning.

### 1.1 Design Pillars

- **Systemic Realism**: Mechanics act as immutable physical laws. Combat range, ship architecture, and trade routes are downstream consequences of specific constraints (e.g., g-force limits, warp cross-sections).
- **Reactive World Simulation**: The universe operates independently of the player. A persistent "Galactic Tick" drives an asynchronous economy of supply, demand, and faction politics.
- **Meaningful Customization**: Ship design is a strategic commitment. Every module choice impacts mass, heat, and power, directly influencing combat performance.

## 2. The Spacetime Paradigm (Warp Mechanics)

The defining mechanic of the universe is the Warp Engine, which operates by "squeezing" space rather than propelling the ship. This physics rule dictates ship design, detection, and defense.

### 2.1 Navigation & Hull Architecture

- **Cross-Section Efficiency**: Warp engines are most efficient when operating on a small spatial cross-section (a "warp tunnel").
- **"Train" Design Standard**: To maximize efficiency, civilian and transport ships are universally designed as long, thin fuselages.
- **Mass Interdiction**: Compressing space containing mass is exponentially more energy-intensive than compressing empty space.
  - **Tactical Interdiction**: Objects with mass (asteroids, debris, other ships) naturally block warp paths. Deploying random mass ("spraying" cargo/debris) into a trajectory is a standard interdiction tactic.

### 2.2 Active Defense (The Warp Shield)

- **Warp as Shielding**: High-output military engines can "squeeze" space containing incoming projectiles or energy beams, curving their trajectory around the hull.
- **Design Trade-off**: Military vessels sacrifice the efficient "train" profile for bulkier frames to house the massive reactors/engines required for this "Warp Shield" capability.
- **Non-Offensive Nature**: The warp effect moves space, not the object directly; it cannot deform hulls and is therefore not an offensive weapon.

### 2.3 Warp Radar (Active Sensing)

- **Density Detection**: Engines can "ping" distant space to detect density variations (mass signatures), functioning as a long-range radar.
- **Performance**: Scan range, resolution, and speed are linear functions of engine power output.

## 3. Combat Doctrine

Combat occurs at long ranges (100s of km) due to the physiological limit of <10g acceleration for manned vessels. It is a tactical "game of inches" focused on positioning and resource management.

### 3.1 Electronic Warfare (The Meta-Layer)

- **Warp Disruption**: Ships actively project random, low-level spatial micro-warps. This "scrambles" local space, preventing enemies from warping in projectiles and stopping nearby ships from warping out.
- **Result**: Guided missiles/torpedoes traveling through normal space are the only reliable payload delivery method; "warping" a bomb into an enemy bridge is impossible.

### 3.2 Weapon Archetypes & Counter-Play

| Weapon Type | Characteristics | Strategic Role | Counters |
|-------------|----------------|----------------|----------|
| Missiles / Torpedoes | Slow velocity, high mass, high "alpha" damage. Expensive ammunition | The Heavyweight. The decisive strike weapon. If a fleet survives until impact, it wins | Point Defense: Kinetics/Lasers. Evasion: Warping away before impact |
| Directed Energy (Beams) | Instant travel, low logistical cost. Sustained damage output | The Pirate/Merc Standard. Wins via attrition by overwhelming enemy reactor/shield output | Reflective Foils: Unfolding physical mirrors. Blocks incoming beams but also blocks user vision/fire |
| Unguided Kinetics | Ineffective at combat ranges (>100km) | Close Defense. Used strictly for point-defense against missiles/fighters | Distance: Easily dodged at range |

### 3.3 Tactical Scenarios

- **Beam vs. Missile**:
  - *Beam Strategy*: Must intercept incoming warheads (Point Defense) or destroy the launching ships before impact. If the missile ships are destroyed, their warp disruption fails, allowing the beam fleet to escape.
  - *Missile Strategy*: Use superior engine power to close distance (penetrating disruption range) and endure the beam barrage until warheads connect.

- **Beam vs. Beam**: A contest of Reactor Output vs. Heat Dissipation. Flanking is critical: attacking from multiple vectors forces the enemy to split their warp shield output, reducing its efficiency.

- **Missile vs. Missile**: A race condition. The faster fleet launches, then warps to safety before the enemy volley arrives.

## 4. World Architecture: Seamless Scale

The engine utilizes a multi-layered coordinate system to facilitate seamless transitions without loading screens.

### 4.1 Coordinate Layers

1. **Local Grid (Physics Bubble)**: A high-precision, floating-origin environment centered on the player. Handles rigid body physics, collision, and rendering.
2. **System Scale (Orrery View)**: An abstracted simulation of the star system. Objects outside the Local Grid exist as data proxies governed by orbital mechanics. Used for macro-navigation.

### 4.2 FTL Transit (State)

- **Active Gameplay**: Warp is a controllable state in the System Scale, not a fast-travel menu.
- **Interdiction**: Transit occurs through the coordinate space. Hitting an interdiction field (mass/anomaly) forces a drop into the Local Grid.
- **Scouting**: Due to FTL travel speed exceeding light speed scanning, fleets use Warp Drone Probes to scout routes and system states before jumping.

## 5. Dynamic Universe Simulation (The Galactic Tick)

The world state is updated via a persistent, asynchronous "Galactic Tick".

### 5.1 Economic Engine

- **Market Nodes**: Every colony/station acts as a market with Fixed Attributes (Resource Richness) and Dynamic Attributes (Wealth, Stability, Population).
- **Supply & Demand**: Industries consume and produce commodities. Shortages and surpluses drive dynamic pricing.
- **Emergent Logistics**: Price differentials automatically generate NPC trade convoys. High wealth attracts both better security responses and increased piracy.

### 5.2 Faction & Diplomacy

- **Dual Standing System**:
  1. Faction Standing: Formal diplomatic status (Hostile/Neutral/Allied).
  2. Personal Standing: Informal reputation with specific NPC contacts (e.g., Station Admins, Fixers). High personal standing can unlock opportunities (black market access) even if Faction Standing is hostile.
- **NPC Interaction**: Interaction is centralized through "Points of Contact" at stations (Administrators for official business, Fixers for clandestine tasks).

## 6. Ship Engineering & Fleet Management

The player manages a strategic entity (the Fleet), not just a single cockpit.

### 6.1 Modular Construction

- **Hull/Hardpoint System**: Hulls provide a base profile (Mass/Cross-Section) and hardpoint layout. Modules (Reactors, E-War, Weapons) are fitted to hardpoints.
- **Power/Heat Economy**: A closed-loop system. Power (Generation) fills Capacitor (Buffer), while actions generate Heat (Waste). High performance requires managing thermal throttling.

### 6.2 Logistics & Attrition

- **Consumables**:
  - Fuel: Required for FTL.
  - Supplies: Consumed for maintenance and repairs. Running out causes system degradation.
  - Crew: A resource that degrades with combat damage. Low crew levels reduce ship efficiency.
- **Officers**: Hireable NPCs with unique traits (e.g., +10% Beam Output) assignable to fleet ships.

### 6.3 Subsystem Targeting

Combat allows targeting specific modules on enemy ships (e.g., destroying Engines to prevent warp escape, or Reactors to collapse shields).

## 7. Technical Architecture (Unity/C#)

The project prioritizes moddability and data-driven design.

- **API-First Design**: Core logic interacts with interfaces (MarketAPI, CombatPlugin) to allow mod overrides without source modification.
- **Externalized Data**: All stats (Ships, Commodities, Factions) are loaded from external JSON/CSV files via a LoadingManager.
- **Event Bus**: A ListenerManager decouples systems, allowing mods to subscribe to global events (OnFleetDestroyed, OnMarketUpdate).