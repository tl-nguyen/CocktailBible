using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using CocktailBible.ViewModels;
using Windows.Networking.Connectivity;
using Windows.UI.Popups;
using System.Threading.Tasks;
using CocktailBible.Utils;
using CocktailBible.Models;

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

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!StatusManager.CheckInternetConnection())
            {
                MessageDialog dialog = new MessageDialog("No Internet Connection!");
                await dialog.ShowAsync();
            }

            if (!App.IsDataLoaded)
            {
                (this.DataContext as RecipesViewModel).Recipes = App.remoteDbRecipes;
            }
        }

        private async void AddItem(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RecipePage));
        }

        private void ItemTapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(RecipePage), (sender as Grid).DataContext);
        }

        private async void RemoveItem(object sender, HoldingRoutedEventArgs e)
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
}
