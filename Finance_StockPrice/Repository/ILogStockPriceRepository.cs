using Finance_StockPrice.Model;

namespace Finance_StockPrice.Repository
{
    public interface ILogStockPriceRepository
    {
        public bool PostLog(LogStockPrice logStockPrice);
        public bool PostLog(string symbol, LogStockPrice logStockPrice);
    }
}
