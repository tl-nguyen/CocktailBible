using System;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

using CocktailBible.Utils;
using CocktailBible.ViewModels;

namespace CocktailBible.Pages
{
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RecipePage));
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!StatusManager.CheckInternetConnection())
            {
                MessageDialog dialog = new MessageDialog("No Internet Connection!");
                await dialog.ShowAsync();
            }

            (this.DataContext as RecipesViewModel).Recipes = App.dbRecipes;
        }

        private void Item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(RecipePage), (sender as Grid).DataContext);
        }
    }
}
