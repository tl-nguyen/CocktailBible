using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using CocktailBible.Common;
using CocktailBible.Models;
using CocktailBible.ViewModels;

namespace CocktailBible.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RecipePage : Page
    {
        private NavigationHelper navigationHelper;
   
        public RecipePage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ///**********************************
            ///Added - start
            ///**********************************

            //This checks to see if there is a parameter.
            // if there is, it is currently assuming that the
            // parameter is of the correct format and data structure
            // you might want to put a little more in the way
            // of checks and balances in this if the app gets
            // more complicated.
            // 
            // The parameter is typed to the BBQRecipe model and then added as the 
            // Recipe in the page's RecipeDetailViewModel.
            if (e.Parameter == null)
            {
                (this.DataContext as RecipeViewModel).Recipe = new Recipe();
                PageTitle.Text = "New BBQRecipe";
                BBQImage.Visibility = Visibility.Collapsed;
                CameraImage.Visibility = Visibility.Visible;

            }
            else
            {
                CameraImage.Visibility = Visibility.Collapsed;
                (this.DataContext as RecipeViewModel).Recipe = e.Parameter as Recipe;
            }
            ///**********************************
            //Added - end
            ///**********************************

             navigationHelper.OnNavigatedTo(e);
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        private void CameraImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            picker.PickSingleFileAndContinue();  

        }

        public void ContinueFileOpenPicker(FileOpenPickerContinuationEventArgs fileOpenPickerContinuationEventArgs)
        {
            if (fileOpenPickerContinuationEventArgs.Files != null)
            {
                StorageFile file = fileOpenPickerContinuationEventArgs.Files[0];
                //TODO: get the image
                //(this.DataContext as RecipeViewModel).Recipe.ImageSource = file.Path;         
                BBQImage.Visibility = Visibility.Visible;
                CameraImage.Visibility = Visibility.Collapsed;
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            bool result = await (this.DataContext as RecipeViewModel).SaveData();

            if (result)
            {
                this.Frame.Navigate(typeof(MainPage));
            }

        }

        private void SnapPicture_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private static async Task<DeviceInformation> GetCameraDeviceInfoAsync(Windows.Devices.Enumeration.Panel desiredPanel)
        {

            DeviceInformation device = (await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture))
                .FirstOrDefault(d => d.EnclosureLocation != null && d.EnclosureLocation.Panel == desiredPanel);

            if (device == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No suitable devices found for the camera of type {0}.", desiredPanel));
            }
            return device;
        }
    }
}
