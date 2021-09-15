using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Statistics_Ozon.JsonDocuments;

namespace Statistics_Ozon
{
    public class GoogleSheetData
    {
        public DateTime DateTime { get; set; }
        public int NumberOfTDM { get; set; }
        public int AveragePriceTDM { get; set; }
        public int NumberOfTDM001a { get; set; }
        public int AveragePriceTDM001a { get; set; }
        public int NumberOfDD { get; set; }
        public int AveragePriceDD { get; set; }
        public int NumberOfAP { get; set; }
        public int AveragePriceAP { get; set; }
        public int NumberOfProducts { get; set; }
        public int SumPriceProducts { get; set; }
        public int AveragePriceProducts { get; set; }
        public int ExpensesYandex { get; set; }
        public int ExpensesGoogle { get; set; }
        public int ExpensesPlatform { get; set; }
        public int ExpensesFB { get; set; }
        public int ExpensesBuyouts { get; set; }
        public int ExpensesSum { get; set; }
        public int CommissionMP { get; set; }
        public int InternalLogistics { get; set; }
        public int ExternallLogistics { get; set; }
        public int CostPrice { get; set; }
        public int PriceProfit { get; set; }
        public int PriceProfitWithExternallLogistics { get; set; }
        public int SumPriceTDM { get; set; }
        public int SumPriceTDM001a { get; set; }
        public int SumPriceDD { get; set; }
        public int SumPriceAP { get; set; }

        public GoogleSheetData(List<JsonDocumentOrdersFBO> ordersFBO, DateTime dateTime)
        {
            DateTime = dateTime;
            NumberOfTDM = CalculatorGoogleSheetData.CountSumProduct(OzonId.TDM, ordersFBO, "unit");
            NumberOfAP = CalculatorGoogleSheetData.CountSumProduct(OzonId.AP, ordersFBO, "unit");
            NumberOfDD = CalculatorGoogleSheetData.CountSumProduct(OzonId.DD, ordersFBO, "unit");
            NumberOfTDM001a = CalculatorGoogleSheetData.CountSumProduct(OzonId.TDM001a, ordersFBO, "unit");
            SumPriceAP = CalculatorGoogleSheetData.CountSumProduct(OzonId.AP, ordersFBO, "price");
            SumPriceDD = CalculatorGoogleSheetData.CountSumProduct(OzonId.DD, ordersFBO, "price");
            SumPriceTDM = CalculatorGoogleSheetData.CountSumProduct(OzonId.TDM, ordersFBO, "price");
            SumPriceTDM001a = CalculatorGoogleSheetData.CountSumProduct(OzonId.TDM001a, ordersFBO, "price");
            CalculatorGoogleSheetData.CalculateData(this);
        }
    }
}
