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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.IsDataLoaded)
            {
                (this.DataContext as RecipesViewModel).Recipes = App.dbRecipes;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RecipePage));
        }

        private void Item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(RecipePage), (sender as Grid).DataContext);
        }
    }
}
