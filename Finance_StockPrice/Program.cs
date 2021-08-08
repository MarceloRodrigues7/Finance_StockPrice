using Finance_StockPrice.Model;
using Finance_StockPrice.Repository;
using Finance_StockPrice.Utils;
using Serilog;
using Serilog.Events;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Finance_StockPrice
{
    class Program
    {
        static void Main()
        {
            Console.Title = $"Stock Price - {AppConsole.CurrentVersion}";
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "Logs/.log"),
                    outputTemplate: "{Timestamp:dd/MM/yyyy HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();
            Console.WriteLine(AppConsole.InitText);
            Log.Logger.Information($"Version: {AppConsole.CurrentVersion}");
            Log.Logger.Information($"Connection DataBse: {AppConsole.ValidConectionDb()}");
            Log.Logger.Information("Sating Application");
            Startup();
            Log.Logger.Information("Finishing Application");
            Log.CloseAndFlush();
        }

        private static void Startup()
        {
            CultureInfo culture = new("en-US", false);
            IStockPriceResponses stockPriceReponse = new StockPriceResponses();
            var symbols = stockPriceReponse.GetSymbols();
            foreach (var symbol in symbols)
            {
                var res = stockPriceReponse.GetPriceResponse(symbol).ToString();
                var stockPrice = StockPrice.FormatObject(res, culture, symbol);
                if (stockPrice.Company_Name == null)
                {
                    Log.Logger.Error(res);
                }
                else
                {
                    var db_StockPrice = stockPriceReponse.GetStockPrice(symbol);
                    if (!db_StockPrice.Equals(stockPrice))
                    {
                        stockPriceReponse.Put_StockPrice(stockPrice);
                        Log.Logger.Information($"{symbol} stock price successfully update!");
                        WorkerHistory(db_StockPrice.Id, stockPrice);
                        Log.Logger.Information($"{symbol} history successfully added!");
                    }
                    else
                    {
                        Log.Logger.Warning($"{symbol} without changes.");
                    }
                }
            }
        }

        private static void WorkerHistory(long IdCompany, StockPrice stockPrice)
        {
            IHistoryFinances historyFinances = new HistoryFinances();

            var currentDay = DateTime.Now.Date;
            var currentHour = new TimeSpan(DateTime.Now.Hour, 0, 0);
            var validLastHistory = historyFinances.ValidLastHistory(IdCompany, currentDay, currentHour);
            if (validLastHistory)
            {
                var history = HistoryFinance.FormatObject(IdCompany, stockPrice, currentDay, currentHour);
                historyFinances.PostHistory(history);
            }
        }
    }
}
