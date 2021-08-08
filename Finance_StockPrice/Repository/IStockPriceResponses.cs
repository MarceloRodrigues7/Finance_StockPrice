using Finance_StockPrice.Model;
using System.Collections.Generic;

namespace Finance_StockPrice.Repository
{
    public interface IStockPriceResponses
    {
        public object GetPriceResponse(string symbol);
        public List<string> GetSymbols();
        public StockPrice GetStockPrice(string symbol);
        public bool Put_StockPrice(StockPrice stockPrice);
    }
}
