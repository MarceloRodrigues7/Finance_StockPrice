using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance_StockPrice.Model
{
    public class HistoryFinance
    {
        public long Id { get; set; }
        public long IdCompany { get; set; }
        public decimal Market_Cap { get; set; }
        public decimal Price { get; set; }
        public float Change_Percent { get; set; }
        public DateTime Day { get; set; }
        public TimeSpan Hour { get; set; }

        public static HistoryFinance FormatObject(long idCompany, StockPrice stockPrice,DateTime currentDay,TimeSpan currentHour)
        {
            return new HistoryFinance
            {
                IdCompany = idCompany,
                Market_Cap = stockPrice.Market_Cap,
                Price = stockPrice.Price,
                Change_Percent = stockPrice.Change_Percent,
                Day = currentDay,
                Hour = currentHour
            };
        }
    }
}
