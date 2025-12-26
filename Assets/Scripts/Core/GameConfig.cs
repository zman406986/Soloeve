using UnityEngine;

namespace SpaceRail
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "SpaceRail/Game Configuration", order = 1)]
    public class GameConfig : ScriptableObject
    {
        [Header("Gameplay Settings")]
        public float MaxPlayerAcceleration = 10f; // g-force limit for manned vessels
        public float WarpEfficiencyFactor = 1.0f;
        public float BaseCargoCapacity = 100f;

        [Header("Economy Settings")]
        public float BasePriceVariation = 0.1f;
        public float SupplyDemandRatio = 1.0f;
        public float MarketUpdateFrequency = 60f; // seconds

        [Header("Combat Settings")]
        public float CombatRangeMultiplier = 100000f; // 100km base
        public float MissileSpeedMultiplier = 1000f;
        public float BeamSpeed = 299792458f; // speed of light

        [Header("FTL Settings")]
        public float WarpSpeedMultiplier = 1000f;
        public float WarpEnergyCost = 10f;
        public float WarpDetectionRange = 5000f;

        [Header("Ship Settings")]
        public float BaseHullIntegrity = 1000f;
        public float BaseShieldCapacity = 500f;
        public float BasePowerGeneration = 100f;
        public float BaseHeatDissipation = 50f;

        [Header("Simulation Settings")]
        public float GalacticTickRate = 1f; // seconds per tick
        public int MaxFleetSize = 10;
        public int MaxCrewPerShip = 50;

        [Header("Graphics Settings")]
        public float MaxRenderDistance = 100000f;
        public int MaxParticles = 10000;
        public float LodBias = 1.0f;
    }
}