using Finance_StockPrice.Model;
using System;

namespace Finance_StockPrice.Repository
{
    public interface IHistoryFinances
    {
        public bool PostHistory(HistoryFinance historyFinance);
        public bool ValidLastHistory(long IdCompany, DateTime dataAtual, TimeSpan horaAtual);
    }
}
