using System;
using System.Collections.Generic;

namespace SpaceRail.Modding
{
    /// <summary>
    /// Interface for mod interaction with the core game systems
    /// </summary>
    public interface IModAPI
    {
        /// <summary>
        /// Register a new event listener that can respond to game events
        /// </summary>
        void RegisterEventListener<T>(Action<T> listener) where T : BaseEvent;

        /// <summary>
        /// Unregister an event listener
        /// </summary>
        void UnregisterEventListener<T>(Action<T> listener) where T : BaseEvent;

        /// <summary>
        /// Override a specific game system implementation
        /// </summary>
        void OverrideSystem<T>(T implementation) where T : class;

        /// <summary>
        /// Register a new ship configuration
        /// </summary>
        void RegisterShip(ShipData shipData);

        /// <summary>
        /// Register a new weapon configuration
        /// </summary>
        void RegisterWeapon(WeaponData weaponData);

        /// <summary>
        /// Register a new faction
        /// </summary>
        void RegisterFaction(FactionData factionData);

        /// <summary>
        /// Register a new commodity
        /// </summary>
        void RegisterCommodity(CommodityData commodityData);

        /// <summary>
        /// Register a new module
        /// </summary>
        void RegisterModule(ModuleData moduleData);

        /// <summary>
        /// Register a new star system
        /// </summary>
        void RegisterSystem(SystemData systemData);

        /// <summary>
        /// Get access to the game's data loading system
        /// </summary>
        LoadingManager GetDataManager();

        /// <summary>
        /// Get access to the game's event system
        /// </summary>
        EventManager GetEventManager();

        /// <summary>
        /// Get current game state information
        /// </summary>
        GameState GetGameState();

        /// <summary>
        /// Register a new UI element or replace existing one
        /// </summary>
        void RegisterUIElement(string elementName, UnityEngine.GameObject uiPrefab);
    }

    /// <summary>
    /// Main mod manager that implements the modding API
    /// </summary>
    public class ModManager : UnityEngine.MonoBehaviour, IModAPI
    {
        private EventManager eventManager;
        private LoadingManager dataManager;
        private GameManager gameManager;

        private List<ModInfo> loadedMods = new List<ModInfo>();
        private Dictionary<string, UnityEngine.GameObject> registeredUIElements = new Dictionary<string, UnityEngine.GameObject>();

        public void Initialize()
        {
            eventManager = FindObjectOfType<EventManager>();
            dataManager = FindObjectOfType<LoadingManager>();
            gameManager = FindObjectOfType<GameManager>();
            
            LoadMods();
        }

        private void LoadMods()
        {
            // In a real implementation, this would scan for mod files and load them
            // For now, we'll just log that the system is ready
            UnityEngine.Debug.Log("ModManager: Ready to load mods");
        }

        #region IModAPI Implementation
        public void RegisterEventListener<T>(Action<T> listener) where T : BaseEvent
        {
            eventManager?.Subscribe(listener);
        }

        public void UnregisterEventListener<T>(Action<T> listener) where T : BaseEvent
        {
            eventManager?.Unsubscribe(listener);
        }

        public void OverrideSystem<T>(T implementation) where T : class
        {
            // Replace the system implementation
            // This would use reflection or a service locator pattern in a real implementation
            UnityEngine.Debug.Log($"System override requested for type: {typeof(T).Name}");
        }

        public void RegisterShip(ShipData shipData)
        {
            // Add to the game's ship registry
            UnityEngine.Debug.Log($"Registered ship: {shipData.Name}");
        }

        public void RegisterWeapon(WeaponData weaponData)
        {
            // Add to the game's weapon registry
            UnityEngine.Debug.Log($"Registered weapon: {weaponData.Name}");
        }

        public void RegisterFaction(FactionData factionData)
        {
            // Add to the game's faction registry
            UnityEngine.Debug.Log($"Registered faction: {factionData.Name}");
        }

        public void RegisterCommodity(CommodityData commodityData)
        {
            // Add to the game's commodity registry
            UnityEngine.Debug.Log($"Registered commodity: {commodityData.Name}");
        }

        public void RegisterModule(ModuleData moduleData)
        {
            // Add to the game's module registry
            UnityEngine.Debug.Log($"Registered module: {moduleData.Name}");
        }

        public void RegisterSystem(SystemData systemData)
        {
            // Add to the game's system registry
            UnityEngine.Debug.Log($"Registered system: {systemData.Name}");
        }

        public LoadingManager GetDataManager()
        {
            return dataManager;
        }

        public EventManager GetEventManager()
        {
            return eventManager;
        }

        public GameState GetGameState()
        {
            return gameManager?.CurrentState ?? GameState.MainMenu;
        }

        public void RegisterUIElement(string elementName, UnityEngine.GameObject uiPrefab)
        {
            if (!registeredUIElements.ContainsKey(elementName))
            {
                registeredUIElements[elementName] = uiPrefab;
                UnityEngine.Debug.Log($"Registered UI element: {elementName}");
            }
            else
            {
                registeredUIElements[elementName] = uiPrefab;
                UnityEngine.Debug.Log($"Replaced UI element: {elementName}");
            }
        }
        #endregion

        /// <summary>
        /// Information about a loaded mod
        /// </summary>
        public class ModInfo
        {
            public string Name { get; set; }
            public string Version { get; set; }
            public string Author { get; set; }
            public string Description { get; set; }
            public string[] Dependencies { get; set; }
            public bool Enabled { get; set; }
        }
    }
}