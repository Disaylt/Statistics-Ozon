using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Statistics_Ozon
{
    public static class FileManager
    {
        private static string _tokensDocument;
        private static string _costPricesDocument;

        public static string PathToWorkDocuments => $@"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName}\WorkDocuments\";
        public static string FileNameCostPrice => "CostPrices.json";
        public static string TokensDocument
        {
            get
            {
                if(_tokensDocument == null)
                {
                    _tokensDocument = File.ReadAllText($"{PathToWorkDocuments}Tokens.json");
                }
                return _tokensDocument;
            }
        }
        public static string CostPricesDocument
        {
            get
            {
                if(_costPricesDocument == null)
                {
                    try
                    {
                        _costPricesDocument = File.ReadAllText($"{PathToWorkDocuments}{FileNameCostPrice}");
                    }
                    catch(FileNotFoundException)
                    {
                        File.WriteAllText($"{PathToWorkDocuments}{FileNameCostPrice}", string.Empty);
                    }
                }
                return _costPricesDocument;
            }
        }
        public static int CostPriceNow
        {
            get
            {
                string pathFile = $"{PathToWorkDocuments}CostPrice.txt";
                if(File.Exists(pathFile))
                {
                    int costPrice = int.Parse(File.ReadAllText(pathFile).Trim());
                    return costPrice;
                }
                else
                {
                    return 350;
                }
            }
        }

        public static void SaveCostPricesFile(string data)
        {
            File.WriteAllText($"{PathToWorkDocuments}{FileNameCostPrice}", data);
        }
    }
}
