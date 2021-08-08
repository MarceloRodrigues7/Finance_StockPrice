using Finance_StockPrice.Model;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance_StockPrice.Repository
{
    public interface IStockPriceApi
    {
        [Get("/stock_price?key={keys}&symbol={symbol}")]
        Task<object> GetPrice(string keys, string symbol);
    }
}
