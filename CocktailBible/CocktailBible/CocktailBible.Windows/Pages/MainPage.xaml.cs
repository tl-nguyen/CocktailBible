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
using CocktailBible.Models;

namespace CocktailBible.Pages
{
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void AddItem(object sender, RoutedEventArgs e)
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

            (this.DataContext as RecipesViewModel).Recipes = App.remoteDbRecipes;
        }

        private void ItemTapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(RecipePage), (sender as Grid).DataContext);
        }

        private async void ItemRemove(object sender, HoldingRoutedEventArgs e)
        {
            MessageDialog dialog;
            if (await (this.DataContext as RecipesViewModel).Remove((sender as Grid).DataContext as Recipe))
            {
                dialog= new MessageDialog("Successfuly deleted Recipe");
            }
            else
            {
                dialog = new MessageDialog("You dont have permission to delete this Recipe");
            }

            try
            {
                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                // TODO:
            }
        }
    }
}
