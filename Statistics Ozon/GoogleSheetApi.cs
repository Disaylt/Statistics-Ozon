using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Google.Apis.Sheets.v4;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using Google.Apis.Sheets.v4.Data;
using System.IO;
using Statistics_Ozon.JsonDocuments;

namespace Statistics_Ozon
{
    public static class GoogleSheetApi
    {
        private static SheetsService _sheetsService;
        private static readonly string[] _scopes = { SheetsService.Scope.Spreadsheets };

        public static SheetsService SheetsService { 
            get 
            {
                if (_sheetsService == null)
                {
                    _sheetsService = GetSheetsService();
                }
                return _sheetsService;
            }
        }
        public delegate void NewSheet(string nameSheet);

        private static SheetsService GetSheetsService()
        {
            SheetsService sheetsService;
            UserCredential userCredential;
            using (var stream = new FileStream($"{FileManager.PathToWorkDocuments}client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    _scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }
            sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = userCredential,
                ApplicationName = JsonHandler.JsonDocumentTokens.GoogleeApplicationName
            });
            return sheetsService;
        }

        public static IList<IList<object>> GetGoogleTable(string sheetName, string table)
        {
            string range = $"{sheetName}!A2:AD";
            SpreadsheetsResource.ValuesResource.GetRequest request;
            switch (table)
            {
                case "expenses":
                    request = SheetsService.Spreadsheets.Values.Get(JsonHandler.JsonDocumentTokens.GoogleTableIdOfExpenses, range);
                    break;
                case "statistic":
                    request = SheetsService.Spreadsheets.Values.Get(JsonHandler.JsonDocumentTokens.GoogltTableIdOfStatistics, range);
                    break;
                default:
                    throw new Exception($"{nameof(GetGoogleTable)} - Неизвестная таблица");
            }
            ValueRange response;
            try
            {
                response = request.Execute();
            }
            catch(Exception)
            {
                return null;
            }
            IList<IList<object>> googleTable = response.Values;
            return googleTable;
        }

        public static void UpdateValue(string sheetName, int row, List<object> value)
        {
            var range = $"{sheetName}!A{row}";
            var valueRange = new ValueRange();
            valueRange.Values = new List<IList<object>> { value };
            var updateRequest = _sheetsService.Spreadsheets.Values.Update(valueRange, JsonHandler.JsonDocumentTokens.GoogltTableIdOfStatistics, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            updateRequest.Execute();
        }

        public static void InsertValue(string sheetName, List<object> value)
        {
            var range = $"{sheetName}!A1:AD";
            var valueRange = new ValueRange();

            valueRange.Values = new List<IList<object>> { value };
            var appendRequest = _sheetsService.Spreadsheets.Values.Append(valueRange, JsonHandler.JsonDocumentTokens.GoogltTableIdOfStatistics, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            appendRequest.Execute();
        }

        public static void CreateNewSheet(string sheetName)
        {
            NewSheet newSheet;
            newSheet = AddSheet;
            if(sheetName == "Шапка домики бот")
            {
                newSheet += AddTitlesHomes;
            }
            newSheet(sheetName);
        }

        private static void AddTitlesHomes(string sheetName)
        {
            var range = $"{sheetName}!A1";
            var valueRange = new ValueRange();
            var objectList = new List<object>() { "Дата", "ТДМ", "ТДМ сред цена", "ТДМ(001а)", "ТДМ(001а) сред цена", "ДД", "ДД сред цена", "АП", "АП сред цена", "Всего проданых штук", "Сумма проданных товаров", "Средняя цена", "Расходы на маркетинг яндекс", "Расходы на маркетинг гугл",
            "Расходы на маркетинг платформа", "Расходы на маркетинг ФБ", "Расходы на маркетинг блогеры/накрутка", "Расходы на маркетинг всего", "Комиссия МП", "Логистика внутренняя", "Логистика внешняя", "Себестоимость", "Прибыль тотал", "Прибыль с учетом внешней логистики", "ТДМ сумма", "ТДМ(001а) сумма", "ДД сумма", "АП сумма"};
            valueRange.Values = new List<IList<object>> { objectList };
            var updateRequest = _sheetsService.Spreadsheets.Values.Update(valueRange, JsonHandler.JsonDocumentTokens.GoogltTableIdOfStatistics, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            updateRequest.Execute();
        }
        private static void AddSheet(string nameSheet)
        {
            var spreadsheet = new AddSheetRequest();
            spreadsheet.Properties = new SheetProperties();
            spreadsheet.Properties.Title = nameSheet;
            BatchUpdateSpreadsheetRequest batchUpdateSpreadsheetRequest = new BatchUpdateSpreadsheetRequest();
            batchUpdateSpreadsheetRequest.Requests = new List<Request>();
            batchUpdateSpreadsheetRequest.Requests.Add(new Request
            {
                AddSheet = spreadsheet
            });
            var batchUpdateRequest =
            _sheetsService.Spreadsheets.BatchUpdate(batchUpdateSpreadsheetRequest, JsonHandler.JsonDocumentTokens.GoogltTableIdOfStatistics);
            batchUpdateRequest.Execute();
        }

        public static List<object> ConvertToGoogleSheetFormat(GoogleSheetData googleSheetData)
        {
            List<object> googleRow = new List<object>
            {
                googleSheetData.DateTime.ToString("dd.MM.yyyy"),
                googleSheetData.NumberOfTDM.ToString(),
                googleSheetData.AveragePriceTDM.ToString(),
                googleSheetData.NumberOfTDM001a.ToString(),
                googleSheetData.AveragePriceTDM001a.ToString(),
                googleSheetData.NumberOfDD.ToString(),
                googleSheetData.AveragePriceDD.ToString(),
                googleSheetData.NumberOfAP.ToString(),
                googleSheetData.AveragePriceAP.ToString(),
                googleSheetData.NumberOfProducts.ToString(),
                googleSheetData.SumPriceProducts.ToString(),
                googleSheetData.AveragePriceProducts.ToString(),
                googleSheetData.ExpensesYandex.ToString(),
                googleSheetData.ExpensesGoogle.ToString(),
                googleSheetData.ExpensesPlatform.ToString(),
                googleSheetData.ExpensesFB.ToString(),
                googleSheetData.ExpensesBuyouts.ToString(),
                googleSheetData.ExpensesSum.ToString(),
                googleSheetData.CommissionMP.ToString(),
                googleSheetData.InternalLogistics.ToString(),
                googleSheetData.ExternallLogistics.ToString(),
                googleSheetData.CostPrice.ToString(),
                googleSheetData.PriceProfit.ToString(),
                googleSheetData.PriceProfitWithExternallLogistics.ToString(),
                googleSheetData.SumPriceTDM.ToString(),
                googleSheetData.SumPriceTDM001a.ToString(),
                googleSheetData.SumPriceDD.ToString(),
                googleSheetData.SumPriceAP.ToString(),
                DateTime.Now.ToString()
            };
            return googleRow;
        }
    }
}