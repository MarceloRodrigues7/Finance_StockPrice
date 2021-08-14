using Finance_StockPrice.Repository;
using Finance_StockPrice.Utils;
using Serilog;
using System;
using System.Globalization;
using System.IO;
using System.Threading;

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
                Log.Logger.Information("Finishing Process.");
                Thread.Sleep(new TimeSpan(6, 0, 0));
            }
            Log.Logger.Information("Finishing Application");
            Log.CloseAndFlush();
        }

        private static void Startup()
        {
            try
            {
                IStockPriceResponses stockPriceReponse = new StockPriceResponses();
                var symbols = stockPriceReponse.GetSymbols();
                foreach (var symbol in symbols)
                {
                    var data = stockPriceReponse.GetPriceResponse(symbol);
                    var quote = data.GetTimeSeriesAsync("AAPL", "1h");
                    foreach (var item in quote.Result.Values)
                    {
                        var res = stockPriceReponse.ValidationHistoryValue(item, symbol);
                        Log.Logger.Information($"{symbol} - {item.Datetime} - Status:{res}");
                    }
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error(e.Message);
            }
            finally
            {
                RunProcess = false;
            }
        }
    }
}
