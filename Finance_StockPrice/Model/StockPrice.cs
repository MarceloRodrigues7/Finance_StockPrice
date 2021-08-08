using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance_StockPrice.Model
{
    public class StockPrice
    {
        public long Id { get; set; }
        public string Company_Name { get; set; }
        public string Symbol { get; set; }
        public decimal Market_Cap { get; set; }
        public decimal Price { get; set; }
        public float Change_Percent { get; set; }
        public string Updated_At { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as StockPrice;

            if (Market_Cap != item.Market_Cap)
            {
                return false;
            }
            else if (Price != item.Price)
            {
                return false;
            }
            else if (DateTime.Parse(Updated_At) != DateTime.Parse(item.Updated_At.Substring(1, 19)))
            {
                return false;
            }
            else if (Change_Percent.ToString() != item.Change_Percent.ToString())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Market_Cap, Price, Change_Percent, Updated_At);
        }

        public static StockPrice FormatObject(string obj, CultureInfo culture, string symbol)
        {
            try
            {
                var objs = obj.Replace("{", "").Replace("}", "").Split(",");

                return new StockPrice
                {
                    Company_Name = objs[4][(objs[4].IndexOf(":") + 1)..].ToString().Replace(@"\", ""),
                    Symbol = symbol,
                    Market_Cap = decimal.Parse(objs[13][(objs[13].IndexOf(":") + 1)..], culture),
                    Price = decimal.Parse(objs[14][(objs[14].IndexOf(":") + 1)..], culture),
                    Change_Percent = float.Parse(objs[15][(objs[15].IndexOf(":") + 1)..], culture),
                    Updated_At = objs[16][(objs[16].IndexOf(":") + 1)..].ToString()
                };
            }
            catch (Exception)
            {
                Log.Error("non-standard symbol");
                return new StockPrice();
            }

        }
    }
}
