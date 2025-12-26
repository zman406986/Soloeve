using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceRail.Events
{
    public class EventManager : MonoBehaviour
    {
        private Dictionary<Type, List<Delegate>> eventListeners = new Dictionary<Type, List<Delegate>>();

        private void Start()
        {
            // Initialize the event system
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Subscribe to an event of type T
        /// </summary>
        public void Subscribe<T>(Action<T> listener) where T : BaseEvent
        {
            Type eventType = typeof(T);
            
            if (!eventListeners.ContainsKey(eventType))
            {
                eventListeners[eventType] = new List<Delegate>();
            }

            if (!eventListeners[eventType].Contains(listener))
            {
                eventListeners[eventType].Add(listener);
            }
        }

        /// <summary>
        /// Unsubscribe from an event of type T
        /// </summary>
        public void Unsubscribe<T>(Action<T> listener) where T : BaseEvent
        {
            Type eventType = typeof(T);

            if (eventListeners.ContainsKey(eventType))
            {
                eventListeners[eventType].Remove(listener);
                
                // Clean up empty lists
                if (eventListeners[eventType].Count == 0)
                {
                    eventListeners.Remove(eventType);
                }
            }
        }

        /// <summary>
        /// Publish an event to all subscribers
        /// </summary>
        public void Publish<T>(T eventObj) where T : BaseEvent
        {
            Type eventType = typeof(T);

            if (eventListeners.ContainsKey(eventType))
            {
                var listeners = new List<Delegate>(eventListeners[eventType]);
                
                foreach (Action<T> listener in listeners)
                {
                    try
                    {
                        listener?.Invoke(eventObj);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Error in event listener: {e.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Clear all event listeners (useful when changing scenes)
        /// </summary>
        public void ClearAllListeners()
        {
            eventListeners.Clear();
        }

        /// <summary>
        /// Get count of listeners for a specific event type
        /// </summary>
        public int GetListenerCount<T>() where T : BaseEvent
        {
            Type eventType = typeof(T);
            
            if (eventListeners.ContainsKey(eventType))
            {
                return eventListeners[eventType].Count;
            }
            
            return 0;
        }
    }

    /// <summary>
    /// Base class for all events in the system
    /// </summary>
    public abstract class BaseEvent
    {
        public DateTime Timestamp { get; private set; }
        public object Sender { get; set; }

        protected BaseEvent()
        {
            Timestamp = DateTime.Now;
        }
    }

    // Example event implementations based on the design document

    #region Fleet Events
    public class FleetCreatedEvent : BaseEvent
    {
        public string FleetId { get; set; }
        public string CommanderName { get; set; }
    }

    public class FleetDestroyedEvent : BaseEvent
    {
        public string FleetId { get; set; }
        public string AttackerId { get; set; }
    }

    public class ShipAddedToFleetEvent : BaseEvent
    {
        public string FleetId { get; set; }
        public string ShipId { get; set; }
    }

    public class ShipRemovedFromFleetEvent : BaseEvent
    {
        public string FleetId { get; set; }
        public string ShipId { get; set; }
        public ShipRemovalReason Reason { get; set; }
    }

    public enum ShipRemovalReason
    {
        Destroyed,
        Transferred,
        Disbanded,
        Lost
    }
    #endregion

    #region Market Events
    public class MarketUpdateEvent : BaseEvent
    {
        public string MarketId { get; set; }
        public string Commodity { get; set; }
        public float OldPrice { get; set; }
        public float NewPrice { get; set; }
        public float SupplyChange { get; set; }
        public float DemandChange { get; set; }
    }

    public class TradeTransactionEvent : BaseEvent
    {
        public string BuyerId { get; set; }
        public string SellerId { get; set; }
        public string Commodity { get; set; }
        public float Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float TotalValue { get; set; }
        public string MarketId { get; set; }
    }
    #endregion

    #region Combat Events
    public class CombatStartedEvent : BaseEvent
    {
        public string AttackerFleetId { get; set; }
        public string DefenderFleetId { get; set; }
        public Vector3 Location { get; set; }
    }

    public class CombatEndedEvent : BaseEvent
    {
        public string WinnerFleetId { get; set; }
        public string LoserFleetId { get; set; }
        public CombatResult Result { get; set; }
    }

    public class ShipHitEvent : BaseEvent
    {
        public string AttackerShipId { get; set; }
        public string TargetShipId { get; set; }
        public string WeaponId { get; set; }
        public float DamageDealt { get; set; }
        public DamageType DamageType { get; set; }
        public Vector3 HitPosition { get; set; }
    }

    public class ModuleDestroyedEvent : BaseEvent
    {
        public string ShipId { get; set; }
        public string ModuleId { get; set; }
        public ModuleType ModuleType { get; set; }
    }
    #endregion

    #region FTL Events
    public class WarpEngagedEvent : BaseEvent
    {
        public string ShipId { get; set; }
        public Vector3 StartPosition { get; set; }
        public Vector3 Destination { get; set; }
        public float WarpFactor { get; set; }
    }

    public class WarpCompletedEvent : BaseEvent
    {
        public string ShipId { get; set; }
        public Vector3 FinalPosition { get; set; }
        public float TravelTime { get; set; }
    }

    public class WarpInterdictedEvent : BaseEvent
    {
        public string ShipId { get; set; }
        public string InterdictorId { get; set; }
        public Vector3 Position { get; set; }
    }
    #endregion

    #region Diplomacy Events
    public class FactionStandingChangedEvent : BaseEvent
    {
        public string FactionId { get; set; }
        public string ActorId { get; set; }
        public float OldStanding { get; set; }
        public float NewStanding { get; set; }
    }

    public class TreatySignedEvent : BaseEvent
    {
        public string Faction1Id { get; set; }
        public string Faction2Id { get; set; }
        public TreatyType Treaty { get; set; }
    }

    public enum TreatyType
    {
        NonAggression,
        TradeAgreement,
        MilitaryAlliance,
        ResearchPact
    }
    #endregion
}