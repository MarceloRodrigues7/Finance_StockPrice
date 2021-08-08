using Dapper;
using Finance_StockPrice.Model;
using Finance_StockPrice.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance_StockPrice.Repository
{
    public class HistoryFinances : IHistoryFinances
    {
        public bool PostHistory(HistoryFinance historyFinance)
        {
            using (var connection = new SqlConnection(AppConsole.ConnectionString))
            {
                var query = $"INSERT INTO History_Finance(IdCompany,Market_Cap,Price,Change_Percent,Day,Hour)" +
                            $"VALUES(@IdCompany,@Market_Cap,@Price,@Change_Percent,@Day,@Hour)";
                return connection.Execute(query,
                    new
                    {
                        historyFinance.IdCompany,
                        historyFinance.Market_Cap,
                        historyFinance.Price,
                        historyFinance.Change_Percent,
                        historyFinance.Day,
                        historyFinance.Hour
                    }) > 0;
            };
        }

        public bool ValidLastHistory(long IdCompany, DateTime CurrentDay, TimeSpan CurrentHour)
        {
            var lastDay = GetLastDay(IdCompany);
            if (!lastDay.HasValue || CurrentDay > lastDay.Value)
            {
                return true;
            }
            else if (CurrentDay == lastDay.Value)
            {
                var lastHour = GetLastHour(IdCompany, lastDay);
                if (CurrentHour > lastHour)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        private static DateTime? GetLastDay(long IdCompany)
        {
            using (var connection = new SqlConnection(AppConsole.ConnectionString))
            {
                var query = "SELECT TOP(1) MAX(Day) FROM History_Finance WITH(NOLOCK) WHERE IdCompany=@IdCompany GROUP BY Day ORDER BY Day DESC";
                return connection.QueryFirstOrDefault<DateTime>(query, new { IdCompany });
            };
        }

        private static TimeSpan? GetLastHour(long IdCompany, DateTime? LastDay)
        {
            using (var connection = new SqlConnection(AppConsole.ConnectionString))
            {
                var query = "SELECT TOP(1) MAX(Hour) FROM History_Finance WITH(NOLOCK) WHERE IdCompany=@IdCompany AND Day=@LastDay GROUP BY Hour ORDER BY Hour DESC";
                return connection.QueryFirstOrDefault<TimeSpan>(query, new { IdCompany, LastDay });
            };
        }
    }
}
