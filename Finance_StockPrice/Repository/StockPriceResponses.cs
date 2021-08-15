using Dapper;
using Finance_StockPrice.Utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using TwelveDataSharp;
using TwelveDataSharp.Interfaces;
using TwelveDataSharp.Library.ResponseModels;

namespace Finance_StockPrice.Repository
{
    public class StockPriceResponses : IStockPriceResponses
    {
        public ITwelveDataClient GetPriceResponse(string symbol)
        {
            HttpClient _client = new();
            ITwelveDataClient _twelveDataClient = new TwelveDataClient(AppConsole.Keys[0], _client);
            return _twelveDataClient;
        }

        public List<string> GetSymbols()
        {
            using (var connection = new SqlConnection(AppConsole.ConnectionString))
            {
                var query = "SELECT Symbol FROM StockList";
                return connection.Query<string>(query).ToList();
            };
        }

        public bool? ValidationHistoryValue(TimeSeriesValues timeSeriesValues, string symbol)
        {
            var id = GetIdSymbol(symbol);
            bool? res = false;
            var validation = GetHistory(id, timeSeriesValues.Datetime);
            if (!validation)
            {
                res = PostHistoryPrice(id, timeSeriesValues);
            }
            return res;
        }

        public long GetIdSymbol(string symbol)
        {
            using (var connection = new SqlConnection(AppConsole.ConnectionString))
            {
                var query = "SELECT Id FROM StockList WITH(NOLOCK) WHERE Symbol=@symbol";
                return connection.QueryFirstOrDefault<long>(query, new { symbol });
            };
        }

        private static bool GetHistory(long id, DateTime dateRegister)
        {
            using (var connection = new SqlConnection(AppConsole.ConnectionString))
            {
                var query = "SELECT Id FROM HistoryPrice WITH(NOLOCK) WHERE IdStockList=@id AND DateRegister=@dateRegister";
                return connection.QueryFirstOrDefault<long>(query, new { id, dateRegister }) > 0;
            };
        }

        private static bool? PostHistoryPrice(long IdStockList, TimeSeriesValues timeSeriesValues)
        {
            try
            {
                using (var connection = new SqlConnection(AppConsole.ConnectionString))
                {
                    var query = @"INSERT INTO HistoryPrice(IdStockList,DateRegister,Opening,HighestValue,LowerValue,CloseValue,Volume)
                              VALUES(@IdStockList,@Datetime,@Open,@High,@Low,@Close,@Volume)";
                    return connection.Execute(query, new
                    {
                        IdStockList,
                        timeSeriesValues.Datetime,
                        timeSeriesValues.Open,
                        timeSeriesValues.High,
                        timeSeriesValues.Low,
                        timeSeriesValues.Close,
                        timeSeriesValues.Volume
                    }) > 0;
                };
            }
            catch (Exception e)
            {
                new LogStockPriceRepository().PostLog(new Model.LogStockPrice
                {
                    IdStockList = IdStockList,
                    DateRegister = timeSeriesValues.Datetime,
                    DataValidation = DateTime.Now,
                    StatusRegister = 3,
                    LogDescrition = e.Message
                });
                Log.Logger.Error($"{IdStockList} -" + e.Message);
                return null;
            }
        }
    }
}
