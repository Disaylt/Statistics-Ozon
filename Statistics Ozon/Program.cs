using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Statistics_Ozon.JsonDocuments;

namespace Statistics_Ozon
{
    class Program
    {
        private static string _sheetName => "Шапка домики Бот";
        static void Main(string[] args)
        {
            DateTime startApplication = DateTime.Now;
            while(true)
            {
                if (DateTime.Now >= startApplication)
                {
                    UpdateOzonSaleData(45);
                    Console.WriteLine($"Update - {DateTime.Now}");
                    startApplication = startApplication.Date.AddDays(1).AddHours(8);
                }
                Thread.Sleep(60 * 1000);
            }
        }

        private static void UpdateOzonSaleData(int numberOfDays)
        {
            List<GoogleSheetData> listGoogleSheetData = new List<GoogleSheetData>();
            DateTime beginningWithDate = DateTime.Now.AddDays(numberOfDays * -1);
            AdvertisingExpenses.LoadAdvertisingExpenses(beginningWithDate, "Гугл");
            AdvertisingExpenses.LoadAdvertisingExpenses(beginningWithDate, "Яндекс");

            var ozonSaleData = GetOzonSaleData(beginningWithDate);
            foreach(DateTime dateTime in ozonSaleData.Keys)
            {
                GoogleSheetData googleSheetData = new GoogleSheetData(ozonSaleData[dateTime], dateTime);
                listGoogleSheetData.Add(googleSheetData);
            }
            UpdateGoogleSheet(listGoogleSheetData);
        }

        private static Dictionary<DateTime, List<JsonDocumentOrdersFBO>> GetOzonSaleData(DateTime beginningWithDate)
        {
            var ozonSaleData = new Dictionary<DateTime, List<JsonDocumentOrdersFBO>>();
            DateTime lastDate = DateTime.Now;
            DateTime ordersDate = beginningWithDate;
            while(ordersDate <= lastDate)
            {
                string responseStringOrderFBO = OzonApiClient.GetOrdersFBO(ordersDate);
                var ordersFBO = JsonHandler.ConvertToListJsonDocumentOrdersFBO(responseStringOrderFBO);
                ozonSaleData.Add(ordersDate.Date, ordersFBO);
                ordersDate = ordersDate.AddDays(1);
            }
            return ozonSaleData;
        }

        private static void UpdateGoogleSheet(List<GoogleSheetData> listGoogleSheetData)
        {
            IList<IList<object>> googleSheet = GoogleSheetApi.GetGoogleTable(_sheetName, "statistic");
            if(googleSheet == null || googleSheet.Count == 0)
            {
                GoogleSheetApi.CreateNewSheet(_sheetName);
                foreach(GoogleSheetData rowData in listGoogleSheetData)
                {
                    List<object> googleRow = GoogleSheetApi.ConvertToGoogleSheetFormat(rowData);
                    GoogleSheetApi.InsertValue(_sheetName, googleRow);
                }
            }
            else
            {
                foreach (GoogleSheetData rowData in listGoogleSheetData)
                {
                    int numberOfRowsDate = googleSheet.Where(x => DateTime.Parse(x[0].ToString()).Date == rowData.DateTime.Date).Count();
                    if (numberOfRowsDate == 0)
                    {
                        List<object> googleRow = GoogleSheetApi.ConvertToGoogleSheetFormat(rowData);
                        GoogleSheetApi.InsertValue(_sheetName, googleRow);
                    }
                    else
                    {
                        int numberOfRows = googleSheet.Count;
                        for (int i = 0; i < numberOfRows; i++)
                        {
                            if (DateTime.Parse(googleSheet[i][0].ToString()).Date == rowData.DateTime.Date)
                            {
                                int row = i + 2;
                                List<object> googleRow = GoogleSheetApi.ConvertToGoogleSheetFormat(rowData);
                                GoogleSheetApi.UpdateValue(_sheetName, row, googleRow);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
