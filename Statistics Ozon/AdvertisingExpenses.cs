using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Statistics_Ozon.ProgramDocumets;
using Statistics_Ozon.JsonDocuments;
using System.Globalization;

namespace Statistics_Ozon
{
    public static class AdvertisingExpenses
    {
        public static List<AdvertisingExpensesData> AdvertisingExpensesGoogle { get; set; }
        public static List<AdvertisingExpensesData> AdvertisingExpensesYandex { get; set; }

        public static void LoadAdvertisingExpenses(DateTime beginningWithDate, string site)
        {
            List<AdvertisingExpensesData> advertisingExpenses = new List<AdvertisingExpensesData>();
            DateTime dateTimeNow = DateTime.Now;
            while(beginningWithDate.Month <= dateTimeNow.Month)
            {
                string sheetName = $"{site} {beginningWithDate.Date:MMMM}";
                IList<IList<object>> googleSheet = GoogleSheetApi.GetGoogleTable(sheetName, "expenses");
                foreach(IList<object> dataRow in googleSheet)
                {
                    try
                    {
                        DateTime dateTime = DateTime.Parse(dataRow[0].ToString());
                        int sum = Convert.ToInt32(double.Parse(dataRow[2].ToString()));
                        advertisingExpenses.Add(new AdvertisingExpensesData { Sum = sum, Date = dateTime });

                    }
                    catch(Exception)
                    {
                        continue;
                    }
                }
                beginningWithDate = beginningWithDate.AddMonths(1);
            }
            switch(site)
            {
                case "Гугл":
                    AdvertisingExpensesGoogle = advertisingExpenses;
                    break;
                case "Яндекс":
                    AdvertisingExpensesYandex = advertisingExpenses;
                    break;
                default:
                    throw new Exception($"{nameof(LoadAdvertisingExpenses)} - неизвестный сайт.");
            }
        }

        public static int GetOzonMedia(DateTime dateTime)
        {
            string[] keysMedia = { "company", "products", "promo" };
            string keyProduct = "домики";
            int sumExpensesMedia = 0;
            foreach(string key in keysMedia)
            {
                string responseMedia = OzonApiClient.GetMedia(dateTime, key);
                var champing = JsonHandler.ConverToJsonDocumentChamping(responseMedia);
                int sum = champing.Where(x => x.Title.ToLower().Contains(keyProduct))
                    .Select(x => Convert.ToInt32(double.Parse(x.Metrics.Expense, CultureInfo.InvariantCulture.NumberFormat)))
                    .Sum();
                sumExpensesMedia += sum;
            }
            return sumExpensesMedia;
        }
    }
}
