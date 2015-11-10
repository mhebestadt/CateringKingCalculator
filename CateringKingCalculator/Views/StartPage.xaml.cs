using hebestadt.CateringKingCalculator;
using hebestadt.CateringKingCalculator.ViewModels;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage.Streams;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;
using CateringKingCalculator.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CateringKingCalculator.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartPage : Page
    {
        MealsViewModel _mealsViewModel = null;
        ObservableCollection<MealViewModel> _meals = null;
        List<string> _unknownIngredients = new List<string>();
        List<string> _noMatchIngredients = new List<string>();
        ObservableCollection<MealItemViewModel> _mealItems = new ObservableCollection<MealItemViewModel>();

        public StartPage()
        {
            this.InitializeComponent();

            _mealsViewModel = new MealsViewModel();
            _meals = _mealsViewModel.GetMeals();
            MealsStartView.ItemsSource = _meals;
        }

        private void MealsStartView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(MealItems), e.ClickedItem);
        }

        private void MealsAdd_AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewMealCustomerEventDataInput), new MealViewModel());
        }

        private async void ImportIngredientsData(string fileContent)
        {
            string line = "";
            using (System.IO.StringReader stringReader = new System.IO.StringReader(fileContent))
            {
                while ((line = await stringReader.ReadLineAsync()) != null)
                {
                    IngredientViewModel ingredient = new IngredientViewModel();

                    string[] ingredientAndManufacturer = line.Split(';');
                    ingredient.Name = ingredientAndManufacturer[0].TrimEnd();
                    //ingredient.Manufacturer = ingredientAndManufacturer[1].TrimEnd();
                    ingredient.Category = "Metzgerei";
                    ingredient.UnitOfMeasure = 1;
                    ingredient.SaveIngredientCheckExisting(ingredient);
                }
            }
        }

        private string CleanMealItemName(string mealItemRaw)
        {
            string result = mealItemRaw;

            Regex regex = new Regex(@" [A-Za-z\d]{1,2},");
            Match match = regex.Match(mealItemRaw);
            int index = match.Index;
            if (index > 0) { result = result.Substring(0, index); }
            return result;
        }

        //private ObservableCollection<IngredientViewModel> InterpretIngredientDefinition(string ingredientDefinition)
        List<string> InterpretIngredientDefinition(string ingredientDefinition)
        {
            //ObservableCollection<IngredientViewModel> result = new ObservableCollection<IngredientViewModel>();
            List<string> result = new List<string>();

            /*IngredientsViewModel ingredientsModel = new IngredientsViewModel();
            ObservableCollection<IngredientViewModel> allIngredients = ingredientsModel.GetAllIngredients();
            
            foreach (var ingredient in allIngredients)
            {
                int index = ingredientDefinition.IndexOf(ingredient.Name);
                if (index != -1)
                {
                    result.Add(ingredient);
                }
            }*/

            Regex regex = new Regex(@"[,\d]{1,3}kg |[,\d]{1,4} Ltr| [\d]{1,4}g|[,\d]{1,4}ml|\d{1,3}([.,]\d{1,3})? Stck");
            List<int> startIndices = new List<int>();
            Match match = regex.Match(ingredientDefinition);
            startIndices.Add(match.Index);

            while (match.Success)
            {
                match = match.NextMatch();
                if (match.Index > 0) { startIndices.Add(match.Index); }
            }

            int loopCount = 1;
            foreach (var indx in startIndices)
            {
                if (loopCount <= startIndices.Count - 1)
                    result.Add(ingredientDefinition.Substring(indx, startIndices[loopCount] - indx).Trim());
                else if (loopCount == startIndices.Count)
                    result.Add(ingredientDefinition.Substring(indx, ingredientDefinition.Length - indx).Trim());
                loopCount++;
            }

            return result;
        }

        private IngredientViewModel GetIngredientDetails(string rawDetails, out string amount, out string unitOfMeasure)
        {
            IngredientViewModel result = null;
            amount = String.Empty;
            unitOfMeasure = String.Empty;

            Regex regex = new Regex(@"[,\d]{1,6}(kg|g|x| Ltr|ml| Stck)", RegexOptions.IgnoreCase);
            Match match = regex.Match(rawDetails);

            if (match.Success)
            {
                result = new IngredientViewModel();
                string ingredientName = rawDetails.Substring(match.Value.Length, rawDetails.Length - match.Value.Length).Trim(); ;
                if (match.Value.Contains("x"))
                {
                    Regex regexValue = new Regex(@"[\(\d]{1,4}(ml)[\)]");
                    Match matchValue = regexValue.Match(ingredientName);
                    ingredientName = ingredientName.Substring(0, matchValue.Index).Trim();
                }

                result = result.IngredientExists(ingredientName);
                if (result != null)
                {
                    Regex regexAmount = new Regex(@"[\d,]{1,4}", RegexOptions.IgnoreCase);
                    Match matchAmount = regexAmount.Match(match.Value);
                    amount = matchAmount.Value.ToString().Trim();
                    unitOfMeasure = match.Value.Substring(amount.Length, match.Value.Length - amount.Length).Trim();
                }
                else
                {
                    _unknownIngredients.Add(rawDetails + "|" + ingredientName);
                }
            }
            else
            {
                _noMatchIngredients.Add(rawDetails);
            }

            return result;
        }

        private void GetGrossWeightAndDivisionFactor(string mealItemInfo, out string grossWeight, 
            out string unitOfMeasure, out string divisionFactor)
        {
            grossWeight = "";
            divisionFactor = "";
            unitOfMeasure = "";
            Regex regexDiv = new Regex(@"[\d,]{1,4}", RegexOptions.IgnoreCase);
            Match matchDiv = regexDiv.Match(mealItemInfo);

            if (matchDiv.Success)
            {
                divisionFactor = matchDiv.Value.Trim();
                Regex regexGrossWeight = new Regex(@"[\d,]{1,4}", RegexOptions.RightToLeft);
                Match matchGrossWeight = regexGrossWeight.Match(mealItemInfo);
                grossWeight = matchGrossWeight.Value.Trim();
                unitOfMeasure = mealItemInfo.Substring(matchGrossWeight.Index + matchGrossWeight.Value.Length,
                    mealItemInfo.Length - (matchGrossWeight.Index + matchGrossWeight.Value.Length));
            }
        }

        private async void ImportMealItemsData(string fileContent, string mealItemCategory)
        {
            string line = "";
            string[] mealItemsRaw = fileContent.Split('|');
            char[] trimChar = {'\n','\r'};
            bool mealItemExists = false;

            foreach(var mealItemRaw in mealItemsRaw)
            {
                if (mealItemRaw.ToString().Length == 0) { continue; }
                string mealItemInfo = mealItemRaw.TrimStart(trimChar);
                using (System.IO.StringReader stringReader = new System.IO.StringReader(mealItemInfo))
                {
                    int lineNo = 0;
                    string mealItemName = "";
                    float grossWeight = 0;
                    float divisionFactor = 0;
                    string grossWeightUnitOfMeasure = "";
                    List<string> rawIngredients = new List<string>();

                    while ((line = await stringReader.ReadLineAsync()) != null)
                    {
                        // At 0 this should be the name
                        if (lineNo == 0)
                        {
                            mealItemName = CleanMealItemName(line);
                            MealItemViewModel mealItemView = new MealItemViewModel();
                            // In case the meal item exists we move on to the next
                            if (mealItemView.MealItemExists(mealItemName) == true) { mealItemExists = true; break; }
                            lineNo++;
                            continue;
                        }

                        if (line.Contains("Personen"))
                        {
                            string _grossWeight;
                            string _divisionFactor;
                            GetGrossWeightAndDivisionFactor(line, out _grossWeight, 
                                out grossWeightUnitOfMeasure, out _divisionFactor);
                            _divisionFactor = _divisionFactor.Replace(',', '.');
                            _grossWeight = _grossWeight.Replace(',', '.');
                            divisionFactor = float.Parse(_divisionFactor, CultureInfo.InvariantCulture.NumberFormat);
                            grossWeight = float.Parse(_grossWeight, CultureInfo.InvariantCulture.NumberFormat) / divisionFactor;
                            
                            lineNo++;
                            continue;
                        }

                        rawIngredients = InterpretIngredientDefinition(line);
                        lineNo++;
                    }

                    // Let's see if there was an inner break
                    if (mealItemExists) { mealItemExists = false; continue; }

                    // if go this far it means a new meal item
                    MealItemViewModel mealItem = new MealItemViewModel();
                    IngredientViewModel ingredient = null;
                    foreach (var rawIngredient in rawIngredients)
                    {
                        string amount;
                        string unitOfMeasure;
                        ingredient = GetIngredientDetails(rawIngredient, out amount, out unitOfMeasure);

                        if (ingredient != null)
                        {
                            UnitOfMeasureViewModel unitOfMeasureModel = new UnitOfMeasureViewModel();
                            unitOfMeasureModel = unitOfMeasureModel.GetUnitOfMeasure(ingredient.UnitOfMeasure);
                            amount = amount.Replace(',', '.');
                            float floatAmount = float.Parse(amount, CultureInfo.InvariantCulture.NumberFormat);

                            // Recalculating kilogramms and liters
                            if ((unitOfMeasure.Equals("kg", StringComparison.CurrentCultureIgnoreCase)) ||
                                    (unitOfMeasure.Equals("Ltr", StringComparison.CurrentCultureIgnoreCase)))
                                floatAmount = floatAmount * 1000;
                            // Devive by the number of people all amounts were calculated for
                            floatAmount = floatAmount / divisionFactor;
                            mealItem.IngredientIDsWithTotalAmount.Add(ingredient.Id, floatAmount);
                        }
                    }

                    mealItem.Name = mealItemName;
                    if (grossWeightUnitOfMeasure.Equals("Ltr", StringComparison.CurrentCultureIgnoreCase) ||
                        grossWeightUnitOfMeasure.Equals("kg", StringComparison.CurrentCultureIgnoreCase))
                        grossWeight = grossWeight * 1000;

                    if (grossWeightUnitOfMeasure.Equals("kg", StringComparison.CurrentCultureIgnoreCase) ||
                        grossWeightUnitOfMeasure.Equals("g", StringComparison.CurrentCultureIgnoreCase))
                        mealItem.TotalAmountUnitOfMeasure = 1;

                    if (grossWeightUnitOfMeasure.Equals("Ltr", StringComparison.CurrentCultureIgnoreCase) ||
                        grossWeightUnitOfMeasure.Equals("ml", StringComparison.CurrentCultureIgnoreCase))
                        mealItem.TotalAmountUnitOfMeasure = 2;

                    mealItem.TotalAmount = grossWeight;
                    FoodCategoryViewModel mealItemCategoryModel = new FoodCategoryViewModel();
                    mealItemCategoryModel = mealItemCategoryModel.GetFoodCategoryByName(mealItemCategory);
                    if (mealItemCategoryModel != null) { mealItem.CategoryId = mealItemCategoryModel.Id; }
                    _mealItems.Add(mealItem);
                     
                }
            }
        }

        private async void Import_AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a text file.
            Windows.Storage.Pickers.FileOpenPicker openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".csv");
            openPicker.FileTypeFilter.Add(".txt");
            Windows.Storage.StorageFile file = await openPicker.PickSingleFileAsync();
            
            if (file != null)
            {
                Windows.Storage.Streams.IRandomAccessStream randomAccessStream = await file.OpenReadAsync();
                
                byte[] fileAsBytes = null;
                fileAsBytes = new byte[randomAccessStream.Size];
                
                DataReader reader = new DataReader(randomAccessStream);
                await reader.LoadAsync((uint)randomAccessStream.Size);
                reader.ReadBytes(fileAsBytes);

                byte[] converted = Encoding.Convert(Encoding.GetEncoding("iso-8859-1"), Encoding.UTF8, fileAsBytes);
                string fileContent = Encoding.UTF8.GetString(converted, 0, converted.Length);

                ImportMealItemsData(fileContent, "Vom Geflügel");
            }
        }
    }
}
