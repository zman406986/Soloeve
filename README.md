# SpaceRail - Space Sandbox Game

SpaceRail is a single-player space sandbox defined by systemic realism and physics-driven tactics. Unlike arcade shooters, gameplay is governed by consistent physical laws (specifically Spacetime mechanics), creating a reactive simulation where player agency is expressed through engineering, logistics, and tactical positioning.

## Project Structure

This repository contains the complete Unity project structure for SpaceRail, organized as follows:

```
Assets/
├── Scripts/                 # All C# scripts
│   ├── Core/              # Core game systems and interfaces
│   │   ├── Interfaces/    # API interfaces (IMarketAPI, ICombatPlugin, etc.)
│   │   └── Managers/      # Game managers (GameManager, etc.)
│   ├── Combat/            # Combat-related systems
│   │   ├── Weapons/       # Weapon systems
│   │   ├── Shields/       # Shield systems
│   │   └── Targeting/     # Targeting systems
│   ├── FTL/               # Faster-than-light travel systems
│   │   ├── Warp/          # Warp engine and mechanics
│   │   ├── Navigation/    # Navigation systems
│   │   └── Radar/         # Sensor systems
│   ├── Ship/              # Ship-related systems
│   │   ├── Modules/       # Ship modules (reactors, weapons, etc.)
│   │   ├── Hulls/         # Hull configurations
│   │   └── Fleets/        # Fleet management
│   ├── World/             # World simulation systems
│   │   ├── Economy/       # Economic simulation
│   │   ├── Diplomacy/     # Diplomacy systems
│   │   └── Simulation/    # General world simulation
│   ├── UI/                # User interface components
│   │   ├── Menus/         # Menu systems
│   │   ├── Hud/           # Heads-up display
│   │   └── Views/         # UI views
│   ├── Data/              # Data loading and management
│   │   ├── Loaders/       # Data loading systems
│   │   └── Providers/     # Data providers
│   ├── Utils/             # Utility scripts
│   │   ├── Math/          # Mathematical utilities
│   │   ├── Physics/       # Physics utilities
│   │   └── Serialization/ # Data serialization
│   ├── Events/            # Event system
│   │   └── Systems/       # Event management
│   └── Modding/           # Modding API and systems
│       ├── API/           # Modding interfaces
│       └── Loaders/       # Mod loading systems
├── Prefabs/               # Unity prefabs
├── Scenes/                # Unity scenes (MainMenu, MainGame, etc.)
├── Materials/             # Material assets
├── Textures/              # Texture assets
├── Models/                # 3D model assets
├── Audio/                 # Audio assets
├── Fonts/                 # Font assets
└── Plugins/               # Plugin assets
Data/                      # External data files (JSON, CSV)
├── Ships/                 # Ship configurations
├── Weapons/               # Weapon configurations
├── Factions/              # Faction data
├── Systems/               # Star system data
├── Commodities/           # Commodity data
├── Modules/               # Module configurations
└── Events/                # Event data
Config/                    # Configuration files
Logs/                      # Log files (runtime)
Temp/                      # Temporary files
```

## Key Features

### Systemic Realism
Mechanics act as immutable physical laws. Combat range, ship architecture, and trade routes are downstream consequences of specific constraints (e.g., g-force limits, warp cross-sections).

### Reactive World Simulation
The universe operates independently of the player. A persistent "Galactic Tick" drives an asynchronous economy of supply, demand, and faction politics.

### Meaningful Customization
Ship design is a strategic commitment. Every module choice impacts mass, heat, and power, directly influencing combat performance.

### The Spacetime Paradigm (Warp Mechanics)
The defining mechanic of the universe is the Warp Engine, which operates by "squeezing" space rather than propelling the ship. This physics rule dictates ship design, detection, and defense.

## Technical Architecture

The project prioritizes moddability and data-driven design:

- **API-First Design**: Core logic interacts with interfaces (MarketAPI, CombatPlugin) to allow mod overrides without source modification
- **Externalized Data**: All stats (Ships, Commodities, Factions) are loaded from external JSON/CSV files via a LoadingManager
- **Event Bus**: A ListenerManager decouples systems, allowing mods to subscribe to global events (OnFleetDestroyed, OnMarketUpdate)

## Getting Started

1. Open the project in Unity 2022.3.15f1 or later
2. The main scenes are located in `Assets/Scenes/`
3. The main entry point is the `GameManager` component
4. Data files can be found in the `Data/` directory and are loaded via the `LoadingManager`

## Design Document

For complete design details, see the `SpaceRail_Design_Document.md` file in the root directory.
