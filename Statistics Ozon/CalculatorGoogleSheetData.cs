using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Statistics_Ozon.JsonDocuments;

namespace Statistics_Ozon
{
    public static class  CalculatorGoogleSheetData
    {
        private delegate int CountOrders(JsonDocumentOrdersFBO orderFBO, long sku);

        public static int CountSumProduct(long sku, List<JsonDocumentOrdersFBO> ordersFBO, string value)
        {
            CountOrders countOrders;
            switch (value)
            {
                case "price":
                    countOrders = CountOrdersPrice;
                    break;
                case "unit":
                    countOrders = CountOrdersUnit;
                    break;
                default:
                    throw new Exception("CountSumProduct - неизвестный value");
            }
            int numberOfProduct = ordersFBO.Where(x => x.Status.Trim() != "cancelled")
                .Select(x => countOrders(x, sku))
                .Sum();
            return numberOfProduct;
        }

        public static void CalculateData(GoogleSheetData googleSheetData)
        {
            CalculateAveragePrice(googleSheetData);
            CalculateGeneralProductsData(googleSheetData);
            CalculateExpenses(googleSheetData);
            CalculateCostPrice(googleSheetData);
            CalculateStatistic(googleSheetData);
        }

        private static void CalculateAveragePrice(GoogleSheetData googleSheetData)
        {
            if(googleSheetData.NumberOfAP != 0)
            {
                googleSheetData.AveragePriceAP = googleSheetData.SumPriceAP / googleSheetData.NumberOfAP;
            }
            if (googleSheetData.NumberOfDD != 0)
            {
                googleSheetData.AveragePriceDD = googleSheetData.SumPriceDD / googleSheetData.NumberOfDD;
            }
            if (googleSheetData.NumberOfTDM != 0)
            {
                googleSheetData.AveragePriceTDM = googleSheetData.SumPriceTDM / googleSheetData.NumberOfTDM;
            }
            if (googleSheetData.NumberOfTDM001a != 0)
            {
                googleSheetData.AveragePriceTDM001a = googleSheetData.SumPriceTDM001a / googleSheetData.NumberOfTDM001a;
            }
        }

        private static void CalculateGeneralProductsData(GoogleSheetData googleSheetData)
        {
            googleSheetData.SumPriceProducts = googleSheetData.SumPriceAP + googleSheetData.SumPriceDD + googleSheetData.SumPriceTDM + googleSheetData.SumPriceTDM001a;
            googleSheetData.NumberOfProducts = googleSheetData.NumberOfAP + googleSheetData.NumberOfDD + googleSheetData.NumberOfTDM + googleSheetData.NumberOfTDM001a;
            if(googleSheetData.NumberOfProducts != 0)
            {
                googleSheetData.AveragePriceProducts = googleSheetData.SumPriceProducts / googleSheetData.NumberOfProducts;
            }
        }

        private static int CountOrdersUnit(JsonDocumentOrdersFBO orderFBO, long sku)
        {
            int quntity = orderFBO.Products
                        .Where(x => x.SKU == sku)
                        .Select(x => (int)x.Quantity)
                        .Sum();
            return quntity;
        }

        private static int CountOrdersPrice(JsonDocumentOrdersFBO orderFBO, long sku)
        {
            int quntity = orderFBO.Products
                        .Where(x => x.SKU == sku)
                        .Select(x => Convert.ToInt32(double.Parse(x.Price, CultureInfo.InvariantCulture.NumberFormat) * x.Quantity))
                        .Sum();
            return quntity;
        }

        private static void CalculateExpenses(GoogleSheetData googleSheetData)
        {
            googleSheetData.ExpensesGoogle = AdvertisingExpenses.AdvertisingExpensesGoogle
                .Where(x => x.Date.Date == googleSheetData.DateTime.Date)?
                .FirstOrDefault().Sum ?? 0;
            googleSheetData.ExpensesYandex = AdvertisingExpenses.AdvertisingExpensesYandex.
                Where(x => x.Date.Date == googleSheetData.DateTime.Date)?
                .FirstOrDefault().Sum ?? 0;
            googleSheetData.ExpensesPlatform = AdvertisingExpenses.GetOzonMedia(googleSheetData.DateTime);
            googleSheetData.ExpensesFB = 0;
            googleSheetData.ExpensesBuyouts = 0;
            googleSheetData.ExpensesSum = googleSheetData.ExpensesGoogle + googleSheetData.ExpensesYandex + googleSheetData.ExpensesPlatform;
        }

        private static void CalculateStatistic(GoogleSheetData googleSheetData)
        {
            googleSheetData.CommissionMP = Convert.ToInt32(googleSheetData.SumPriceProducts * 0.05f);
            int valueLogistic = googleSheetData.AveragePriceProducts * 0.044f > 20 ? Convert.ToInt32(googleSheetData.AveragePriceProducts * 0.044f) : 20;
            googleSheetData.InternalLogistics = Convert.ToInt32(((30 + 5 * 5) * googleSheetData.NumberOfProducts + valueLogistic * googleSheetData.NumberOfProducts) + 8 * 5 * googleSheetData.NumberOfProducts);
            googleSheetData.ExternallLogistics = googleSheetData.NumberOfProducts * 48;
            googleSheetData.PriceProfit = googleSheetData.SumPriceProducts - googleSheetData.ExpensesSum - googleSheetData.CommissionMP - googleSheetData.InternalLogistics - googleSheetData.CostPrice;
            googleSheetData.PriceProfitWithExternallLogistics = googleSheetData.PriceProfit - googleSheetData.ExternallLogistics;
        }

        private static void CalculateCostPrice(GoogleSheetData googleSheetData)
        {
            var costPrice = JsonHandler.JsonDocumentCostPrices.Where(x => DateTime.Parse(x[0].ToString()) == googleSheetData.DateTime).FirstOrDefault();
            if(costPrice == null)
            {
                int defoultCostPrice = FileManager.CostPriceNow;
                List<object> costPriceForThisDay = new List<object> { googleSheetData.DateTime, defoultCostPrice };
                JsonHandler.UpdateCostPrices(costPriceForThisDay);
                googleSheetData.CostPrice = defoultCostPrice  * googleSheetData.NumberOfProducts;
            }
            else
            {
                googleSheetData.CostPrice = Convert.ToInt32(costPrice[1]) * googleSheetData.NumberOfProducts;
            }
        }
    }
}
