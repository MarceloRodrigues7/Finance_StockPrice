using Finance_StockPrice.Model;
using Finance_StockPrice.Repository;
using Finance_StockPrice.Utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TwelveDataSharp.Library.ResponseModels;

namespace Finance_StockPrice
{
    class Program
    {
        private static bool RunProcess = true;
        static void Main()
        {
            Console.Title = $"Stock Price - {AppConsole.CurrentVersion}";
            CultureInfo culture = new("en-US", false);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            #region Serilog Configuration
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "Logs/.log"),
                    outputTemplate: "{Timestamp:dd/MM/yyyy HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();
            Console.WriteLine(AppConsole.InitText);
            #endregion

            Log.Logger.Information($"Version: {AppConsole.CurrentVersion}");
            Log.Logger.Information($"Connection DataBase: {AppConsole.ValidConectionDb()}");
            Log.Logger.Information("Starting Application");
            while (true)
            {
                Thread workerThread = new(new ThreadStart(Startup));
                workerThread.Start();
                while (RunProcess)
                {
                    Thread.Sleep(10000);
                }
                Log.Logger.Information("Finished Process.");
                Thread.Sleep(new TimeSpan(2, 0, 0));
            }
        }

        private static void Startup()
        {
            try
            {
                IStockPriceResponses stockPriceReponse = new StockPriceResponses();
                var symbols = stockPriceReponse.GetSymbols();
                List<Task> task = new();
                symbols.ForEach((value) =>
                {
                    task.Add(Task.Factory.StartNew(() =>
                    {
                        InitializationToProcess(stockPriceReponse, value);
                    }));
                });
                Task.WaitAll(task.ToArray());
                task.Clear();
                symbols.Clear();
            }
            catch (Exception e)
            {
                Log.Logger.Error($"ERROR - " + e.Message);
            }
            finally
            {
                RunProcess = false;
            }
        }

        private static void InitializationToProcess(IStockPriceResponses stockPriceReponse, string symbol)
        {
            ILogStockPriceRepository logStockPrice = new LogStockPriceRepository();
            var data = stockPriceReponse.GetPriceResponse(symbol);
            var quote = data.GetTimeSeriesAsync(symbol, "5min").Result;
            if (quote.ResponseStatus != Enums.TwelveDataClientResponseStatus.Ok)
            {
                logStockPrice.PostLog(symbol, new LogStockPrice
                {
                    DateRegister = DateTime.Now,
                    DataValidation = DateTime.Now,
                    StatusRegister = 4,
                    LogDescrition = quote.ResponseMessage
                });
            }
            else
            {
                foreach (var item in quote.Values)
                {
                    var res = stockPriceReponse.ValidationHistoryValue(item, symbol);
                    PostRegistration(logStockPrice, symbol, item, res);
                    Log.Logger.Information($"{symbol} - {item.Datetime} - Status:{res}");
                }
            }
        }

        private static void PostRegistration(ILogStockPriceRepository logStockPrice, string symbol, TimeSeriesValues item, bool? res)
        {
            if (res.Value)
            {
                logStockPrice.PostLog(symbol, new LogStockPrice
                {
                    DateRegister = item.Datetime,
                    DataValidation = DateTime.Now,
                    StatusRegister = 1,
                    LogDescrition = "Successfully registered"
                });
            }
            else if (!res.Value)
            {
                logStockPrice.PostLog(symbol, new LogStockPrice
                {
                    DateRegister = item.Datetime,
                    DataValidation = DateTime.Now,
                    StatusRegister = 2,
                    LogDescrition = "Already contains history"
                });
            }
        }
    }
}
