using UnityEngine;

namespace SpaceRail
{
    public interface ICombatPlugin
    {
        /// <summary>
        /// Calculates damage between attacker and target
        /// </summary>
        CombatResult CalculateDamage(DamageData damage, ShipModule target);

        /// <summary>
        /// Processes weapon firing logic
        /// </summary>
        FireResult ProcessWeaponFire(WeaponModule weapon, Transform target);

        /// <summary>
        /// Handles shield deflection/absorption
        /// </summary>
        ShieldResult ProcessShieldHit(ShieldModule shield, DamageData incomingDamage);
    }

    [System.Serializable]
    public struct DamageData
    {
        public float Amount;
        public DamageType Type;
        public Vector3 Direction;
        public float Penetration;
    }

    public enum DamageType
    {
        Kinetic,
        Energy,
        Explosive,
        Warp
    }

    [System.Serializable]
    public struct CombatResult
    {
        public float DamageApplied;
        public float DamageBlocked;
        public bool CriticalHit;
        public ModuleStatus TargetStatus;
    }

    public enum ModuleStatus
    {
        Operational,
        Damaged,
        Disabled,
        Destroyed
    }

    [System.Serializable]
    public struct FireResult
    {
        public bool Success;
        public float Accuracy;
        public float TimeToTarget;
    }

    [System.Serializable]
    public struct ShieldResult
    {
        public float BlockedDamage;
        public float RemainingCapacity;
        public bool Penetrated;
    }
}