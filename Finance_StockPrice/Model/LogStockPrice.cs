using System;

namespace Finance_StockPrice.Model
{
    public class LogStockPrice
    {
        public long Id { get; set; }
        public long IdStockList { get; set; }
        public DateTime? DateRegister { get; set; }
        public DateTime DataValidation { get; set; }
        public long StatusRegister { get; set; }
        public string LogDescrition { get; set; }
    }

    public class StatusRegisters_LogStockPrice
    {
        public long Id { get; set; }
        public string InformationStatus { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
