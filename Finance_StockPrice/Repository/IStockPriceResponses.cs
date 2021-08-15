using System.Collections.Generic;
using TwelveDataSharp.Interfaces;
using TwelveDataSharp.Library.ResponseModels;

namespace Finance_StockPrice.Repository
{
    public interface IStockPriceResponses
    {
        public ITwelveDataClient GetPriceResponse(string symbol);
        public List<string> GetSymbols();
        public bool? ValidationHistoryValue(TimeSeriesValues timeSeriesValues, string symbol);
        public long GetIdSymbol(string symbol);
    }
}
