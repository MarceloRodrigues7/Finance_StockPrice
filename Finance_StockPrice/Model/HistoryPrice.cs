using System;

namespace Finance_StockPrice.Model
{
    public class HistoryPrice
    {
        public long Id { get; set; }
        public long IdStockList { get; set; }
        public DateTime DateRegister { get; set; }
        public float Opening { get; set; }
        public float HighestValue { get; set; }
        public float LowerValue { get; set; }
        public float CloseValue { get; set; }
        public int Volume { get; set; }
    }
}
