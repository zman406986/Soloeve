using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SpaceRail.Data
{
    public class LoadingManager : MonoBehaviour
    {
        [Header("Data Paths")]
        public string DataPath = "Data/";
        public string ShipsPath = "Data/Ships/";
        public string WeaponsPath = "Data/Weapons/";
        public string FactionsPath = "Data/Factions/";
        public string SystemsPath = "Data/Systems/";
        public string CommoditiesPath = "Data/Commodities/";
        public string ModulesPath = "Data/Modules/";

        private Dictionary<string, object> loadedDataCache = new Dictionary<string, object>();
        private bool isInitialized = false;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (isInitialized) return;

            // Create data directories if they don't exist
            EnsureDataDirectories();

            // Initialize the data cache
            loadedDataCache = new Dictionary<string, object>();

            isInitialized = true;
            Debug.Log("LoadingManager initialized successfully");
        }

        private void EnsureDataDirectories()
        {
            // Make sure all required data directories exist
            Directory.CreateDirectory(Path.Combine(Application.streamingAssetsPath, DataPath));
            Directory.CreateDirectory(Path.Combine(Application.streamingAssetsPath, ShipsPath));
            Directory.CreateDirectory(Path.Combine(Application.streamingAssetsPath, WeaponsPath));
            Directory.CreateDirectory(Path.Combine(Application.streamingAssetsPath, FactionsPath));
            Directory.CreateDirectory(Path.Combine(Application.streamingAssetsPath, SystemsPath));
            Directory.CreateDirectory(Path.Combine(Application.streamingAssetsPath, CommoditiesPath));
            Directory.CreateDirectory(Path.Combine(Application.streamingAssetsPath, ModulesPath));
        }

        #region JSON Loading Methods
        /// <summary>
        /// Load data from a JSON file
        /// </summary>
        public T LoadFromJson<T>(string fileName) where T : class
        {
            string fullPath = Path.Combine(Application.streamingAssetsPath, fileName);
            
            if (!File.Exists(fullPath))
            {
                Debug.LogError($"JSON file not found: {fullPath}");
                return null;
            }

            try
            {
                string json = File.ReadAllText(fullPath);
                T data = JsonUtility.FromJson<T>(json);
                
                // Cache the loaded data
                string cacheKey = typeof(T).Name + "_" + fileName;
                loadedDataCache[cacheKey] = data;
                
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading JSON file {fullPath}: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Load data from a JSON file with caching
        /// </summary>
        public T LoadFromJsonCached<T>(string fileName) where T : class
        {
            string cacheKey = typeof(T).Name + "_" + fileName;
            
            if (loadedDataCache.ContainsKey(cacheKey))
            {
                return loadedDataCache[cacheKey] as T;
            }

            T data = LoadFromJson<T>(fileName);
            if (data != null)
            {
                loadedDataCache[cacheKey] = data;
            }

            return data;
        }

        /// <summary>
        /// Load multiple JSON files from a directory
        /// </summary>
        public List<T> LoadMultipleFromJson<T>(string directoryPath) where T : class
        {
            List<T> results = new List<T>();
            
            string fullPath = Path.Combine(Application.streamingAssetsPath, directoryPath);
            
            if (!Directory.Exists(fullPath))
            {
                Debug.LogError($"Directory not found: {fullPath}");
                return results;
            }

            string[] jsonFiles = Directory.GetFiles(fullPath, "*.json");
            
            foreach (string file in jsonFiles)
            {
                try
                {
                    string json = File.ReadAllText(file);
                    T data = JsonUtility.FromJson<T>(json);
                    
                    if (data != null)
                    {
                        results.Add(data);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error loading JSON file {file}: {e.Message}");
                }
            }

            return results;
        }
        #endregion

        #region CSV Loading Methods
        /// <summary>
        /// Load data from a CSV file
        /// </summary>
        public List<T> LoadFromCsv<T>(string fileName) where T : class, new()
        {
            string fullPath = Path.Combine(Application.streamingAssetsPath, fileName);
            
            if (!File.Exists(fullPath))
            {
                Debug.LogError($"CSV file not found: {fullPath}");
                return new List<T>();
            }

            try
            {
                string[] lines = File.ReadAllLines(fullPath);
                if (lines.Length < 2)
                {
                    Debug.LogWarning($"CSV file is empty or has no data: {fullPath}");
                    return new List<T>();
                }

                List<T> results = new List<T>();
                string[] headers = ParseCSVLine(lines[0]); // First line is headers

                for (int i = 1; i < lines.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(lines[i])) continue;

                    string[] values = ParseCSVLine(lines[i]);
                    T item = CreateFromCSVLine<T>(values, headers);
                    
                    if (item != null)
                    {
                        results.Add(item);
                    }
                }

                return results;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading CSV file {fullPath}: {e.Message}");
                return new List<T>();
            }
        }

        private string[] ParseCSVLine(string line)
        {
            // Simple CSV parsing - can be enhanced for more complex cases
            return line.Split(',');
        }

        private T CreateFromCSVLine<T>(string[] values, string[] headers) where T : class, new()
        {
            T item = new T();
            Type type = typeof(T);
            var properties = type.GetProperties();

            for (int i = 0; i < values.Length && i < headers.Length; i++)
            {
                string header = headers[i].Trim();
                string value = values[i].Trim();

                var property = System.Array.Find(properties, p => 
                    string.Equals(p.Name, header, StringComparison.OrdinalIgnoreCase));
                
                if (property != null && !string.IsNullOrEmpty(value))
                {
                    try
                    {
                        object convertedValue = Convert.ChangeType(value, property.PropertyType);
                        property.SetValue(item, convertedValue);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning($"Could not convert value '{value}' to {property.PropertyType.Name} for property {property.Name}: {e.Message}");
                    }
                }
            }

            return item;
        }
        #endregion

        #region AssetBundle Loading Methods
        /// <summary>
        /// Load asset from AssetBundle
        /// </summary>
        public T LoadAssetFromBundle<T>(string bundleName, string assetName) where T : UnityEngine.Object
        {
            // In a real implementation, this would load from AssetBundles
            // For now, we'll just return null as a placeholder
            Debug.LogWarning($"AssetBundle loading is not implemented yet: {bundleName}/{assetName}");
            return null;
        }
        #endregion

        #region Data Management
        /// <summary>
        /// Clear the data cache
        /// </summary>
        public void ClearCache()
        {
            loadedDataCache.Clear();
        }

        /// <summary>
        /// Reload specific data from file (bypassing cache)
        /// </summary>
        public T ReloadFromJson<T>(string fileName) where T : class
        {
            string fullPath = Path.Combine(Application.streamingAssetsPath, fileName);
            
            if (!File.Exists(fullPath))
            {
                Debug.LogError($"JSON file not found: {fullPath}");
                return null;
            }

            try
            {
                string json = File.ReadAllText(fullPath);
                T data = JsonUtility.FromJson<T>(json);
                
                // Update cache
                string cacheKey = typeof(T).Name + "_" + fileName;
                loadedDataCache[cacheKey] = data;
                
                return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error reloading JSON file {fullPath}: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Check if a data file exists
        /// </summary>
        public bool DataFileExists(string fileName)
        {
            string fullPath = Path.Combine(Application.streamingAssetsPath, fileName);
            return File.Exists(fullPath);
        }
        #endregion

        #region Specific Data Loading Methods
        /// <summary>
        /// Load ship configurations
        /// </summary>
        public List<ShipData> LoadShips()
        {
            return LoadMultipleFromJson<ShipData>(ShipsPath);
        }

        /// <summary>
        /// Load weapon configurations
        /// </summary>
        public List<WeaponData> LoadWeapons()
        {
            return LoadMultipleFromJson<WeaponData>(WeaponsPath);
        }

        /// <summary>
        /// Load faction data
        /// </summary>
        public List<FactionData> LoadFactions()
        {
            return LoadMultipleFromJson<FactionData>(FactionsPath);
        }

        /// <summary>
        /// Load star system data
        /// </summary>
        public List<SystemData> LoadSystems()
        {
            return LoadMultipleFromJson<SystemData>(SystemsPath);
        }

        /// <summary>
        /// Load commodity data
        /// </summary>
        public List<CommodityData> LoadCommodities()
        {
            return LoadMultipleFromJson<CommodityData>(CommoditiesPath);
        }

        /// <summary>
        /// Load module configurations
        /// </summary>
        public List<ModuleData> LoadModules()
        {
            return LoadMultipleFromJson<ModuleData>(ModulesPath);
        }
        #endregion
    }

    #region Data Classes
    [Serializable]
    public class ShipData
    {
        public string Id;
        public string Name;
        public string HullType; // Civilian, Military, etc.
        public float BaseHullIntegrity;
        public float CrossSection;
        public int HardpointCount;
        public ModuleType[] AllowedHardpointTypes;
        public float BaseCargoCapacity;
        public float BaseCrewCapacity;
        public float MaxAcceleration;
        public float BasePowerGeneration;
        public float BaseHeatDissipation;
    }

    [Serializable]
    public class WeaponData
    {
        public string Id;
        public string Name;
        public WeaponType Type;
        public DamageType DamageType;
        public float Damage;
        public float FireRate;
        public float Range;
        public float Accuracy;
        public float PowerConsumption;
        public float HeatGeneration;
        public float AmmoConsumption;
        public float ReloadTime;
        public float ProjectileSpeed;
    }

    [Serializable]
    public class FactionData
    {
        public string Id;
        public string Name;
        public string Description;
        public Color PrimaryColor;
        public Color SecondaryColor;
        public float StartingRelations;
        public string[] AlliedFactions;
        public string[] HostileFactions;
        public string[] TradePartners;
        public string HeadquartersSystem;
    }

    [Serializable]
    public class SystemData
    {
        public string Id;
        public string Name;
        public Vector3 Position;
        public string StarType;
        public int PlanetCount;
        public string[] ConnectedSystems;
        public string[] Stations;
        public string[] FactionsPresent;
        public float SecurityLevel;
        public float EconomicValue;
    }

    [Serializable]
    public class CommodityData
    {
        public string Id;
        public string Name;
        public string Description;
        public CommodityCategory Category;
        public float BasePrice;
        public float VolumePerUnit;
        public float MassPerUnit;
        public bool Perishable;
        public float PerishRate;
        public string[] SourceSystems;
        public string[] DemandSystems;
    }

    [Serializable]
    public class ModuleData
    {
        public string Id;
        public string Name;
        public ModuleType Type;
        public float Mass;
        public float Volume;
        public float PowerConsumption;
        public float PowerGeneration;
        public float HeatGeneration;
        public float HeatDissipation;
        public float Integrity;
        public float KineticResistance;
        public float EnergyResistance;
        public float ExplosiveResistance;
        public float WarpResistance;
        public ModuleSubsystem[] VulnerableSubsystems;
    }
    #endregion

    #region Enums
    public enum WeaponType
    {
        Kinetic,
        Energy,
        Missile,
        Torpedo,
        Mine,
        PointDefense
    }

    public enum CommodityCategory
    {
        RawMaterials,
        ManufacturedGoods,
        Food,
        Luxury,
        Technology,
        Contraband
    }
    #endregion
}