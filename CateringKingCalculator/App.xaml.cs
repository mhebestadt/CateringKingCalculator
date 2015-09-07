using CateringKingCalculator.Converters;
using CateringKingCalculator.Views;
using hebestadt.CateringKingCalculator;
using hebestadt.CateringKingCalculator.Converters;
using hebestadt.CateringKingCalculator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace hebestadtaCateringKingCalculator
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public static string DBPath = string.Empty;
        public string[] foodCategories = { "Antipasti",
"Asiatisch",
"Canapés  Brötchen Snacks",
"Dessert",
"Dips und Butterteller",
"Gemüsebeilagen",
"Griechisch",
"Grillbuffet Extra",
"Italienisch",
"Kalte  Fleischplatten",
"Mexikanisch",
"Mitternachtssnacks",
"Party Snacks",
"Pfannengemse",
"Salate",
"Saucen",
"Schnitzelparade",
"Spanisch",
"Suppen",
"Suppengemüse",
"Sättigungsbeilagen",
"Vip Grillbuffet",
"Vom Geflgel",
"Vom Lamm und Fisch",
"Vom Rind",
"Vom Schwein",
"Vom Wild",
"Vorspeisen"
};

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        private void addFoodCategories()
        {
            using (var db = new SQLite.SQLiteConnection(DBPath))
            {
                foreach (var foodCategory in foodCategories)
                {

                    var newFoodCategory = new FoodCategory()
                    {
                        Name = foodCategory.ToString()
                    };
                    db.Insert(newFoodCategory);
                }
            }   
        }

        private void ResetData()
        {
            GenericListToByteArray converter = new GenericListToByteArray();
            GenericDictionaryToByteArray dictionaryConverter = new GenericDictionaryToByteArray();

            using (var db = new SQLite.SQLiteConnection(DBPath))
            {
                // Empty the Customer and Project tables 
                //db.DeleteAll<Meal>();
                db.DeleteAll<MealItem>();


                var newMealItem = new MealItem()
                {
                    Name = "Schweinerückenbraten in Rahmsauce",
                    Category = "Pork",
                    IngredientIDsWithTotalAmount = (byte[])dictionaryConverter.Convert(new Dictionary<int, int>() { {1,15}, {2,20}, {3,30}, {4, 40} }, null, null, "")
                };
                db.Insert(newMealItem);

                newMealItem = null;
                newMealItem = new MealItem()
                {
                    Name = "Rinderrouladen in herzhafter Sauce",
                    Category = "Beef",
                    IngredientIDsWithTotalAmount = (byte[])dictionaryConverter.Convert(new Dictionary<int, int>() { { 1, 15 }, { 2, 20 }, { 3, 30 }, { 4, 40 } }, null, null, "")
                };
                db.Insert(newMealItem);

                newMealItem = null;
                newMealItem = new MealItem()
                {
                    Name = "kleine Röstkartoffeln(mit geschmorten Speck und Zwiebeln)",
                    Category = "SideDish",
                    IngredientIDsWithTotalAmount = (byte[])dictionaryConverter.Convert(new Dictionary<int, int>() { { 1, 15 }, { 2, 20 }, { 3, 30 }, { 4, 40 } }, null, null, "")
                };
                db.Insert(newMealItem);

                newMealItem = null;
                newMealItem = new MealItem()
                {
                    Name = "Rahmkohlrabi",
                    Category = "Vegetables",
                    IngredientIDsWithTotalAmount = (byte[])dictionaryConverter.Convert(new Dictionary<int, int>() { { 1, 15 }, { 2, 20 }, { 3, 30 }, { 4, 40 } }, null, null, "")
                };
                db.Insert(newMealItem);

                newMealItem = null;
                newMealItem = new MealItem()
                {
                    Name = "Blumenkohl (mit Sauce Hollandaise)",
                    Category = "Vegetables",
                    IngredientIDsWithTotalAmount = (byte[])dictionaryConverter.Convert(new Dictionary<int, int>() { { 1, 15 }, { 2, 20 }, { 3, 30 }, { 4, 40 } }, null, null, "")
                };
                db.Insert(newMealItem);

                newMealItem = null;
                newMealItem = new MealItem()
                {
                    Name = "Rote Grütze mit Vanillesauce",
                    Category = "Desserts",
                    IngredientIDsWithTotalAmount = (byte[])dictionaryConverter.Convert(new Dictionary<int, int>() { { 1, 15 }, { 2, 20 }, { 3, 30 }, { 4, 40 } }, null, null, "")
                };
                db.Insert(newMealItem);

                newMealItem = null;
                newMealItem = new MealItem()
                {
                    Name = "Herrencreme",
                    Category = "Desserts",
                    IngredientIDsWithTotalAmount = (byte[])dictionaryConverter.Convert(new Dictionary<int, int>() { { 1, 15 }, { 2, 20 }, { 3, 30 }, { 4, 40 } }, null, null, "")
                };
                db.Insert(newMealItem);

                // Add seed customers and projects
                /*
                var newMeal = new Meal()
                {
                    Name = "Jutta Althues",
                    DeliveryDate = DateTime.Now,
                    NumberOfGuests = 120,
                    MealItemIDs = (byte[])converter.Convert(new List<int>() {1,2,3,4,5,6,7,8}, null, null, ""),
                    Contact = "Contact1"
                };
                db.Insert(newMeal);

                newMeal = null;
                newMeal = new Meal()
                {
                    Name = "Heinz Benter",
                    DeliveryDate = DateTime.Now,
                    NumberOfGuests = 83,
                    MealItemIDs = (byte[])converter.Convert(new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 }, null, null, ""),
                    Contact = "Contact2"
                };
                db.Insert(newMeal);

                newMeal = null;
                newMeal = new Meal()
                {
                    Name = "MGV",
                    DeliveryDate = DateTime.Now,
                    NumberOfGuests = 83,
                    MealItemIDs = (byte[])converter.Convert(new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 }, null, null, ""),
                    Contact = "Contact3"
                };
                db.Insert(newMeal);

                newMeal = null;
                newMeal = new Meal()
                {
                    Name = "Möllers",
                    DeliveryDate = DateTime.Now,
                    NumberOfGuests = 5,
                    MealItemIDs = (byte[])converter.Convert(new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 }, null, null, "")
                };
                db.Insert(newMeal);

                newMeal = null;
                newMeal = new Meal()
                {
                    Name = "Wiedenbrück Schützenfest 2015",
                    DeliveryDate = DateTime.Now,
                    NumberOfGuests = 2434,
                    MealItemIDs = (byte[])converter.Convert(new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 }, null, null, "")
                };
                db.Insert(newMeal);*/
            }
        }


        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Get a reference to the SQLite database
                DBPath = Path.Combine(
                    Windows.Storage.ApplicationData.Current.LocalFolder.Path, "meals.db");
                // Initialize the database if necessary
                using (var db = new SQLite.SQLiteConnection(DBPath))
                {
                    // Create the tables if they don't exist
                    db.CreateTable<Meal>();
                    db.CreateTable<MealItem>();
                    db.CreateTable<Ingredient>();
                    db.CreateTable<FoodCategory>();
                    db.CreateTable<UnitOfMeasure>();
                    db.CreateTable<Contact>();
                }

                //this.ResetData();
                //addFoodCategories();

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(StartPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
