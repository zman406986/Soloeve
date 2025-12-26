using System;

namespace SpaceRail
{
    public interface IMarketAPI
    {
        /// <summary>
        /// Gets the current price of a commodity at a specific market node
        /// </summary>
        float GetPrice(string commodity, string marketId);

        /// <summary>
        /// Updates market supply and demand based on transactions
        /// </summary>
        void ProcessTransaction(string commodity, string marketId, float quantity, float price);

        /// <summary>
        /// Gets supply/demand information for a commodity at a market
        /// </summary>
        MarketData GetMarketData(string commodity, string marketId);
    }

    [Serializable]
    public struct MarketData
    {
        public float Supply;
        public float Demand;
        public float BasePrice;
        public float CurrentPrice;
    }
}