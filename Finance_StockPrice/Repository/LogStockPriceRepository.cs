using Dapper;
using Finance_StockPrice.Model;
using Finance_StockPrice.Utils;
using System.Data.SqlClient;

namespace Finance_StockPrice.Repository
{
    public class LogStockPriceRepository : ILogStockPriceRepository
    {
        public bool PostLog(LogStockPrice logStockPrice)
        {
            using (var connection = new SqlConnection(AppConsole.ConnectionString))
            {
                var query = @"INSERT INTO LogStockPrice(IdStockList,DateRegister,DataValidation,StatusRegister,LogDescrition)
                              VALUES(@IdStockList,@DateRegister,@DataValidation,@StatusRegister,@LogDescrition)";
                return connection.Execute(query, new
                {
                    logStockPrice.IdStockList,
                    logStockPrice.DateRegister,
                    logStockPrice.DataValidation,
                    logStockPrice.StatusRegister,
                    logStockPrice.LogDescrition
                }) > 0;
            };
        }

        public bool PostLog(string symbol, LogStockPrice logStockPrice)
        {
            IStockPriceResponses stockPriceResponses = new StockPriceResponses();
            logStockPrice.IdStockList = stockPriceResponses.GetIdSymbol(symbol);
            return PostLog(logStockPrice);
        }
    }
}
