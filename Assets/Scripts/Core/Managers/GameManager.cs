using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceRail
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game State")]
        public GameState CurrentState = GameState.MainMenu;
        
        [Header("Core Systems")]
        public MarketManager MarketManager;
        public CombatManager CombatManager;
        public FTLManager FTLManager;
        public ShipManager ShipManager;
        public WorldManager WorldManager;
        public EventManager EventManager;
        public ModManager ModManager;

        [Header("Configuration")]
        public GameConfig Config;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeSystems();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            StartGame();
        }

        private void InitializeSystems()
        {
            // Initialize all core systems
            MarketManager = gameObject.AddComponent<MarketManager>();
            CombatManager = gameObject.AddComponent<CombatManager>();
            FTLManager = gameObject.AddComponent<FTLManager>();
            ShipManager = gameObject.AddComponent<ShipManager>();
            WorldManager = gameObject.AddComponent<WorldManager>();
            EventManager = gameObject.AddComponent<EventManager>();
            ModManager = gameObject.AddComponent<ModManager>();
            
            // Initialize mod system first
            ModManager.Initialize();
            
            // Initialize other systems
            MarketManager.Initialize();
            WorldManager.Initialize();
        }

        public void StartGame()
        {
            CurrentState = GameState.InGame;
            // Load the main game scene
            SceneManager.LoadScene("MainGame");
        }

        public void ChangeState(GameState newState)
        {
            CurrentState = newState;
        }

        private void Update()
        {
            // Handle global game logic based on state
            switch (CurrentState)
            {
                case GameState.InGame:
                    // Update game systems
                    UpdateInGame();
                    break;
                case GameState.Paused:
                    // Handle pause logic
                    break;
                case GameState.MainMenu:
                    // Handle main menu logic
                    break;
            }
        }

        private void UpdateInGame()
        {
            // Update all systems each frame
            WorldManager?.UpdateSimulation();
            MarketManager?.UpdateEconomy();
            ShipManager?.UpdateFleet();
        }
    }

    public enum GameState
    {
        MainMenu,
        InGame,
        Paused,
        GameOver
    }
}