using hebestadt.CateringKingCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CateringKingCalculator.Import
{
    public class ImportMealItemsFromFile
    {
        private const int MealItemNameLineNo = 0;
        private const int MealItemGrossWeightLineNo = 1;
        private const int MealItemIngredientsLineNo = 2;

        MealItemViewModel _mealItemView = new MealItemViewModel();

        public ImportMealItemsFromFile() { }

        public string[] GetMealItemDefinitions(string fileContent)
        {
            return fileContent.Split('|');
        }

        public async Task<string> GetMealItemName(string mealItemData)
        {
            string mealItemName = String.Empty;

            string line = await GetLineContent(MealItemNameLineNo, mealItemData);
            mealItemName = ExtractMealItemName(line);

            return mealItemName;
        }

        public async Task<Tuple<float,string>> GetMealItemWeightUnitOfMeasure(string mealItemData)
        {
            Tuple<float, string> mealItemWeightUnitOfMeasure;

            string line = await GetLineContent(MealItemGrossWeightLineNo, mealItemData);
            mealItemWeightUnitOfMeasure = ExtractMealItemWeightUnitOfMeasure(line);

            return mealItemWeightUnitOfMeasure;
        }

        private Tuple<float, string> ExtractMealItemWeightUnitOfMeasure(string line)
        {
            Tuple<float, string> result;

            Match match = GetRegExResult(@"[\d,]{1,4}", RegexOptions.RightToLeft, line);
            string grossWeight = match.Value.Trim();
            string unitOfMeasure = line.Substring(match.Index + match.Value.Length,
                    line.Length - (match.Index + match.Value.Length));

            result = Tuple.Create(float.Parse(grossWeight, CultureInfo.InvariantCulture.NumberFormat), unitOfMeasure);

            return result;
        }

        private string ExtractMealItemName(string line)
        {
            string result = String.Empty;

            Match match = GetRegExResult(@" [A-Za-z\d]{1,2},", RegexOptions.IgnoreCase, line);
            if (match.Index > 0) { result = result.Substring(0, match.Index); }
            return result;
        }

        private Match GetRegExResult(string pattern, RegexOptions regExOptions, string source)
        {
            string regexResult = String.Empty;

            Regex regex = new Regex(pattern, regExOptions);
            Match match = regex.Match(source);

            return match;
        }

        public bool MealItemExists(string mealItemName)
        {
            return _mealItemView.MealItemExists(mealItemName);
        }

        private async Task<string> GetLineContent(int lineNumber, string source)
        {
            string result = String.Empty;
            System.IO.StringReader stringReader;
            string line = String.Empty;
            int lineNo = 0;

            using (stringReader = GetStringReader(source))
            {
                while ((line = await stringReader.ReadLineAsync()) != null)
                {
                    if (lineNo == lineNumber)
                    {
                        result = line;
                        break;
                    }
                }
            }

            return result;
        }

        private System.IO.StringReader GetStringReader(string stringToRead)
        {
            return new System.IO.StringReader(stringToRead);
        }

    }
}
