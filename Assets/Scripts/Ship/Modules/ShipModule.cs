using UnityEngine;

namespace SpaceRail.Ship
{
    public abstract class ShipModule : MonoBehaviour
    {
        [Header("Module Configuration")]
        public ModuleConfig Config;
        public ModuleType Type;
        public string ModuleName;
        public string ModuleId;

        [Header("Current State")]
        public ModuleStatus Status = ModuleStatus.Operational;
        public float CurrentHealth = 100f;
        public float CurrentHeat = 0f;
        public float CurrentPowerDraw = 0f;
        public float CurrentPowerGeneration = 0f;

        [Header("Hardpoint Connection")]
        public HardpointSlot ConnectedSlot;
        public bool IsConnected = false;

        protected float maxHeat;
        protected float heatDissipationRate;
        protected float powerCapacity;

        protected virtual void Start()
        {
            InitializeModule();
        }

        protected virtual void InitializeModule()
        {
            if (Config != null)
            {
                CurrentHealth = Config.MaxHealth;
                maxHeat = Config.MaxHeatCapacity;
                heatDissipationRate = Config.HeatDissipationRate;
                powerCapacity = Config.PowerCapacity;
            }
        }

        public virtual bool ConnectToHardpoint(HardpointSlot slot)
        {
            if (slot == null || !slot.CanAcceptModule(this))
            {
                Debug.LogError($"Cannot connect module {ModuleName} to hardpoint {slot?.name}");
                return false;
            }

            ConnectedSlot = slot;
            IsConnected = true;
            
            // Register with the ship's power/heat system
            if (ConnectedSlot.Ship != null)
            {
                ConnectedSlot.Ship.RegisterModule(this);
            }

            return true;
        }

        public virtual void Disconnect()
        {
            if (ConnectedSlot != null && ConnectedSlot.Ship != null)
            {
                ConnectedSlot.Ship.UnregisterModule(this);
            }
            
            ConnectedSlot = null;
            IsConnected = false;
        }

        public virtual void ProcessDamage(float damage, DamageType damageType)
        {
            if (Status == ModuleStatus.Destroyed)
                return;

            // Apply damage with type-specific modifiers
            float effectiveDamage = CalculateDamageResistance(damage, damageType);
            CurrentHealth -= effectiveDamage;

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Status = ModuleStatus.Destroyed;
                OnModuleDestroyed();
            }
            else if (CurrentHealth <= Config.MaxHealth * 0.25f)
            {
                Status = ModuleStatus.Disabled;
                OnModuleDisabled();
            }
            else if (CurrentHealth <= Config.MaxHealth * 0.5f)
            {
                Status = ModuleStatus.Damaged;
                OnModuleDamaged();
            }

            // Damage generates additional heat
            CurrentHeat += effectiveDamage * 10f; // Heat from damage
        }

        protected virtual float CalculateDamageResistance(float damage, DamageType damageType)
        {
            // Apply damage type specific resistance
            switch (damageType)
            {
                case DamageType.Kinetic:
                    return damage * (1f - Config.KineticResistance);
                case DamageType.Energy:
                    return damage * (1f - Config.EnergyResistance);
                case DamageType.Explosive:
                    return damage * (1f - Config.ExplosiveResistance);
                case DamageType.Warp:
                    return damage * (1f - Config.WarpResistance);
                default:
                    return damage;
            }
        }

        protected virtual void OnModuleDamaged()
        {
            Debug.Log($"Module {ModuleName} is damaged!");
            // Visual effects, performance degradation, etc.
        }

        protected virtual void OnModuleDisabled()
        {
            Debug.Log($"Module {ModuleName} is disabled!");
            // Module stops functioning
        }

        protected virtual void OnModuleDestroyed()
        {
            Debug.Log($"Module {ModuleName} is destroyed!");
            // Module is permanently offline
            Disconnect();
        }

        public virtual void UpdateModule(float deltaTime)
        {
            // Update heat dissipation
            CurrentHeat -= heatDissipationRate * deltaTime;
            CurrentHeat = Mathf.Max(0, CurrentHeat);

            // Check for overheating
            if (CurrentHeat > maxHeat * 0.8f)
            {
                Status = ModuleStatus.Damaged; // Performance degradation
            }
            else if (CurrentHeat > maxHeat)
            {
                Status = ModuleStatus.Disabled; // Module shuts down
            }
        }

        public virtual float GetPowerConsumption()
        {
            return CurrentPowerDraw;
        }

        public virtual float GetPowerGeneration()
        {
            return CurrentPowerGeneration;
        }

        public virtual float GetHeatGeneration()
        {
            return Mathf.Max(0, CurrentHeat - heatDissipationRate * Time.deltaTime);
        }

        public virtual ModuleStats GetStats()
        {
            return new ModuleStats
            {
                Health = CurrentHealth,
                MaxHealth = Config.MaxHealth,
                Heat = CurrentHeat,
                MaxHeat = maxHeat,
                PowerDraw = CurrentPowerDraw,
                PowerGeneration = CurrentPowerGeneration
            };
        }

        // Subsystems that can be targeted
        public virtual bool IsSubsystemVulnerable(ModuleSubsystem subsystem)
        {
            return Config.VulnerableSubsystems.Contains(subsystem);
        }

        public virtual void DamageSubsystem(ModuleSubsystem subsystem, float damage)
        {
            if (!IsSubsystemVulnerable(subsystem))
                return;

            switch (subsystem)
            {
                case ModuleSubsystem.PowerCore:
                    CurrentPowerGeneration *= (1f - damage * 0.01f);
                    break;
                case ModuleSubsystem.HeatSink:
                    heatDissipationRate *= (1f - damage * 0.01f);
                    break;
                case ModuleSubsystem.ControlSystem:
                    // Affects accuracy, response time, etc.
                    break;
            }
        }
    }

    [System.Serializable]
    public class ModuleConfig
    {
        public string Name;
        public ModuleType Type;
        public float MaxHealth = 100f;
        public float MaxHeatCapacity = 100f;
        public float HeatDissipationRate = 10f;
        public float PowerCapacity = 100f;
        public float KineticResistance = 0.1f;
        public float EnergyResistance = 0.1f;
        public float ExplosiveResistance = 0.1f;
        public float WarpResistance = 0.1f;
        public ModuleSubsystem[] VulnerableSubsystems = { ModuleSubsystem.PowerCore, ModuleSubsystem.HeatSink, ModuleSubsystem.ControlSystem };
    }

    public enum ModuleType
    {
        Reactor,
        Engine,
        Weapon,
        Shield,
        Sensor,
        Cargo,
        Crew,
        LifeSupport,
        FTL,
        Support
    }

    public enum ModuleSubsystem
    {
        PowerCore,
        HeatSink,
        ControlSystem,
        StructuralIntegrity,
        TargetingSystem
    }

    [System.Serializable]
    public struct ModuleStats
    {
        public float Health;
        public float MaxHealth;
        public float Heat;
        public float MaxHeat;
        public float PowerDraw;
        public float PowerGeneration;
    }
}