using Dapper;
using Finance_StockPrice.Model;
using Finance_StockPrice.Utils;
using Refit;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace Finance_StockPrice.Repository
{
    public class StockPriceResponses : IStockPriceResponses
    {
        public object GetPriceResponse(string symbol)
        {
            var tentativas = 0;
            var concluido = false;
            var numKey = 0;
            object res = string.Empty;

            while (tentativas < 3 && concluido == false)
            {
                try
                {
                    var request = RestService.For<IStockPriceApi>("http://api.hgbrasil.com/finance");
                    res = request.GetPrice(AppConsole.Keys[numKey], symbol).Result;
                    concluido = true;
                }
                catch (Exception e)
                {
                    res = e.Message;
                }
                finally
                {
                    tentativas++;
                    numKey++;
                }
            }
            return res;
        }

        public List<string> GetSymbols()
        {
            using (var connection = new SqlConnection(AppConsole.ConnectionString))
            {
                var query = "SELECT Symbol FROM Finance_StockPrice";
                return connection.Query<string>(query).ToList();
            };
        }

        public StockPrice GetStockPrice(string symbol)
        {
            using (var connection = new SqlConnection(AppConsole.ConnectionString))
            {
                var query = "SELECT * FROM Finance_StockPrice WHERE Symbol=@symbol";
                return connection.QueryFirstOrDefault<StockPrice>(query, new { symbol });
            };
        }

        public bool Put_StockPrice(StockPrice stockPrice)
        {
            using (var connection = new SqlConnection(AppConsole.ConnectionString))
            {
                var query = "UPDATE Finance_StockPrice SET Market_Cap=@Market_Cap,Price=@Price,Change_Percent=@Change_Percent,Updated_At=@Updated_At WHERE Symbol=@Symbol";
                var Updated_At = DateTime.Parse(stockPrice.Updated_At.Substring(1, 19));
                return connection.Execute(query, new { stockPrice.Market_Cap, stockPrice.Price, stockPrice.Change_Percent, Updated_At, stockPrice.Symbol }) > 0;
            };
        }
    }
}
