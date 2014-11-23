using CocktailBible.ViewModels;
using System;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CocktailBible.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
        }

        public async Task<bool> CheckInternetConnection()
        {
            try
            {
                ConnectionProfile InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();

                if (InternetConnectionProfile == null)
                {
                    MessageDialog dialog = new MessageDialog("No Internet Connection!\n Must have Internet Connection in order to manipulate your blacklist!");
                    await dialog.ShowAsync();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageDialog dialog = new MessageDialog("Could not retrive Internet connection info!");
                dialog.ShowAsync();
                return false;
            }

        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RecipePage));
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!await this.CheckInternetConnection())
            {
                return;
            }

            (this.DataContext as RecipesViewModel).Recipes = App.dbRecipes;
        }


        /// <summary>
        /// In the XAML I changed the SelectionMode to None
        /// to ensure that there was no selected item being 
        /// held which would cause issues on back navigation
        /// by holding the selection state of the GridView.
        /// 
        /// I added a Tapped event handler (seen below) to 
        /// the ItemTemplate and that is what institutes 
        /// all the navigation.
        /// </summary>

        private void Item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            /// Frame is used to instigate navigation in both WP and WinRT
            /// we send the DataContext of the grid tapped which, in this 
            /// case, is always BBQRecipe, you might want to add more in 
            /// way of checks and balances in the future.
            Frame.Navigate(typeof(RecipePage), (sender as Grid).DataContext);
        }


    }
}
