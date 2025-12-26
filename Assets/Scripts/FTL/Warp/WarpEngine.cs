using UnityEngine;

namespace SpaceRail.FTL
{
    public class WarpEngine : MonoBehaviour
    {
        [Header("Warp Configuration")]
        public WarpEngineConfig Config;
        
        [Header("Current State")]
        public bool IsWarping = false;
        public float CurrentWarpFactor = 0f;
        public float CurrentPowerDraw = 0f;
        public float CurrentHeat = 0f;
        
        [Header("Cross-Section Efficiency")]
        public float CurrentCrossSection = 1.0f; // Smaller is more efficient
        public float OptimalCrossSection = 0.1f;  // Optimal for "train" design
        
        private float maxHeat;
        private float heatDissipationRate;
        private float powerGenerationCapacity;
        private float warpFieldStrength;

        private void Start()
        {
            InitializeWarpEngine();
        }

        private void InitializeWarpEngine()
        {
            if (Config != null)
            {
                maxHeat = Config.MaxHeatCapacity;
                heatDissipationRate = Config.HeatDissipationRate;
                powerGenerationCapacity = Config.PowerGenerationCapacity;
                warpFieldStrength = Config.BaseFieldStrength;
                CurrentCrossSection = CalculateOptimalCrossSection();
            }
        }

        public bool EngageWarp(Vector3 destination)
        {
            if (!CanEngageWarp())
                return false;

            // Calculate warp path considering interdiction fields
            WarpPath path = CalculateWarpPath(destination);
            
            if (path.IsValid)
            {
                IsWarping = true;
                CurrentWarpFactor = CalculateWarpFactor(path);
                StartCoroutine(ExecuteWarpTransit(path));
                return true;
            }

            return false;
        }

        public void DisengageWarp()
        {
            IsWarping = false;
            CurrentWarpFactor = 0f;
        }

        private bool CanEngageWarp()
        {
            // Check if ship is within mass interdiction field
            if (IsInInterdictionField())
                return false;

            // Check power and heat requirements
            if (CurrentHeat > maxHeat * 0.8f)
                return false;

            return true;
        }

        private WarpPath CalculateWarpPath(Vector3 destination)
        {
            // In a real implementation, this would check for interdiction fields along the path
            WarpPath path = new WarpPath();
            path.Start = transform.position;
            path.End = destination;
            path.Distance = Vector3.Distance(transform.position, destination);
            path.IsValid = !IsPathBlockedByInterdiction(transform.position, destination);
            
            return path;
        }

        private bool IsPathBlockedByInterdiction(Vector3 start, Vector3 end)
        {
            // Check for mass interdiction along the path
            // This would normally involve physics queries or checking against known interdiction fields
            return false;
        }

        private float CalculateWarpFactor(WarpPath path)
        {
            // Warp factor is affected by cross-section efficiency
            float efficiencyFactor = CalculateCrossSectionEfficiency();
            float distanceFactor = Mathf.Clamp(path.Distance / 10000f, 0.1f, 10f); // Normalize for distance
            
            return Config.BaseWarpSpeed * efficiencyFactor / distanceFactor;
        }

        private float CalculateCrossSectionEfficiency()
        {
            // Smaller cross-sections are more efficient (train design)
            // Exponential efficiency curve favoring small cross-sections
            return Mathf.Pow(0.1f / Mathf.Max(CurrentCrossSection, 0.01f), 0.5f);
        }

        private float CalculateOptimalCrossSection()
        {
            // For civilian/transport ships, optimal is long and thin ("train" design)
            // For military ships, optimal is bulkier for warp shield capability
            if (IsMilitaryConfiguration())
            {
                return Config.MilitaryCrossSectionOptimal;
            }
            else
            {
                return Config.CivilianCrossSectionOptimal;
            }
        }

        private bool IsMilitaryConfiguration()
        {
            // Determine if this ship is configured for military use
            // This might check for military modules or other factors
            return GetComponent<ShipHull>()?.IsMilitary == true;
        }

        private System.Collections.IEnumerator ExecuteWarpTransit(WarpPath path)
        {
            float totalDistance = path.Distance;
            float traveled = 0f;
            Vector3 direction = (path.End - path.Start).normalized;

            while (traveled < totalDistance && IsWarping)
            {
                float deltaTime = Time.deltaTime;
                
                // Update position based on warp factor
                float distanceThisFrame = CurrentWarpFactor * deltaTime;
                transform.position += direction * distanceThisFrame;
                traveled += distanceThisFrame;

                // Update power draw and heat generation
                UpdatePowerAndHeat(deltaTime);

                // Check for interdiction during transit
                if (IsInInterdictionField())
                {
                    Debug.Log("Warp interdicted during transit!");
                    DropOutOfWarp();
                    yield break;
                }

                yield return null;
            }

            if (IsWarping)
            {
                // Successfully reached destination
                transform.position = path.End;
                DisengageWarp();
            }
        }

        private void UpdatePowerAndHeat(float deltaTime)
        {
            // Warp engines generate heat and draw power
            CurrentPowerDraw = Config.PowerDrawAtMaxWarp * (CurrentWarpFactor / Config.BaseWarpSpeed);
            CurrentHeat += Config.HeatGenerationRate * deltaTime;
            
            // Dissipate heat
            CurrentHeat -= heatDissipationRate * deltaTime;
            CurrentHeat = Mathf.Max(0, CurrentHeat);
        }

        private bool IsInInterdictionField()
        {
            // Check if the ship is currently in a mass interdiction field
            // This could be from other ships, stations, or debris
            Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, Config.InterdictionDetectionRange);
            
            foreach (Collider col in nearbyObjects)
            {
                if (col.CompareTag("InterdictionField"))
                {
                    return true;
                }
            }
            
            return false;
        }

        private void DropOutOfWarp()
        {
            IsWarping = false;
            CurrentWarpFactor = 0f;
            // Additional logic for emergency warp drop
        }

        private void Update()
        {
            if (IsWarping)
            {
                // Visual effects for warp transit
                UpdateWarpEffects();
            }
            
            // Update heat dissipation
            CurrentHeat -= heatDissipationRate * Time.deltaTime;
            CurrentHeat = Mathf.Max(0, CurrentHeat);
        }

        private void UpdateWarpEffects()
        {
            // Update visual and audio effects during warp
        }

        // Active Defense - Warp Shield functionality
        public bool ActivateWarpShield()
        {
            if (CurrentPowerDraw > powerGenerationCapacity * 0.9f)
            {
                Debug.LogWarning("Insufficient power for warp shield activation");
                return false;
            }

            // Increase warp field strength to deflect incoming projectiles
            warpFieldStrength = Config.ShieldFieldStrength;
            CurrentPowerDraw += Config.ShieldPowerDraw;
            CurrentHeat += Config.ShieldHeatGeneration;

            return true;
        }

        public void DeactivateWarpShield()
        {
            warpFieldStrength = Config.BaseFieldStrength;
            CurrentPowerDraw -= Config.ShieldPowerDraw;
        }

        // Warp Radar functionality
        public DensityScanResult ScanDensity(Vector3 targetPosition, float scanPower)
        {
            DensityScanResult result = new DensityScanResult();
            
            // Calculate scan range based on power output
            float scanRange = Config.BaseScanRange * Mathf.Sqrt(scanPower / Config.MaxScanPower);
            
            // Perform density scan at target position
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position, 
                scanRange * 0.1f, // narrow beam approximation
                (targetPosition - transform.position).normalized, 
                scanRange
            );

            foreach (RaycastHit hit in hits)
            {
                // Calculate mass signature based on object properties
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    result.MassSignature += rb.mass;
                    result.DensityVariations.Add(new DensityVariation 
                    { 
                        Position = hit.point, 
                        Mass = rb.mass 
                    });
                }
            }

            return result;
        }
    }

    [System.Serializable]
    public class WarpEngineConfig
    {
        public float BaseWarpSpeed = 1000f;
        public float MaxHeatCapacity = 1000f;
        public float HeatDissipationRate = 100f;
        public float HeatGenerationRate = 50f;
        public float PowerGenerationCapacity = 1000f;
        public float PowerDrawAtMaxWarp = 500f;
        public float BaseFieldStrength = 1.0f;
        public float ShieldFieldStrength = 5.0f;
        public float ShieldPowerDraw = 800f;
        public float ShieldHeatGeneration = 200f;
        public float CivilianCrossSectionOptimal = 0.1f;
        public float MilitaryCrossSectionOptimal = 0.5f;
        public float InterdictionDetectionRange = 1000f;
        public float BaseScanRange = 10000f;
        public float MaxScanPower = 1000f;
    }

    public struct WarpPath
    {
        public Vector3 Start;
        public Vector3 End;
        public float Distance;
        public bool IsValid;
    }

    public struct DensityScanResult
    {
        public float MassSignature;
        public System.Collections.Generic.List<DensityVariation> DensityVariations;
        
        public DensityScanResult(float massSig)
        {
            MassSignature = massSig;
            DensityVariations = new System.Collections.Generic.List<DensityVariation>();
        }
    }

    public struct DensityVariation
    {
        public Vector3 Position;
        public float Mass;
    }
}