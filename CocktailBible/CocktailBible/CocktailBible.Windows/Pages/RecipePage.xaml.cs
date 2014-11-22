using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

using CocktailBible.Common;
using CocktailBible.Models;
using CocktailBible.ViewModels;

namespace CocktailBible.Pages
{
    public sealed partial class RecipePage : Page
    {
        private NavigationHelper navigationHelper;

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }



        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

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

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        public RecipePage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
        }

        private async void SnapPicture_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                // Using Windows.Media.Capture.CameraCaptureUI API to capture a photo
                CameraCaptureUI dialog = new CameraCaptureUI();
                Size aspectRatio = new Size(16, 9);
                dialog.PhotoSettings.CroppedAspectRatio = aspectRatio;

                StorageFile file = await dialog.CaptureFileAsync(CameraCaptureUIMode.Photo);
                if (file != null)
                {

                    using (var streamCamera = await file.OpenAsync(FileAccessMode.Read))
                    {

                        BitmapImage bitmapCamera = new BitmapImage();
                        bitmapCamera.SetSource(streamCamera);

                        int width = bitmapCamera.PixelWidth;
                        int height = bitmapCamera.PixelHeight;


                        WriteableBitmap bitmapImage = new WriteableBitmap(width, height);
                        using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                        {
                            bitmapImage.SetSource(fileStream);
                        }
                        SaveImageAsJpeg(bitmapImage);
                    }
                }
                else
                {
                    new MessageDialog("No photo captured.").ShowAsync();
                }
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message).ShowAsync();
            }
        }

        private async void SaveImageAsJpeg(WriteableBitmap image)
        {
            
            // Create the File Picker control
            FileSavePicker picker = new FileSavePicker();
            picker.FileTypeChoices.Add("JPG File", new List<string>() { ".jpg" });
            StorageFile file = await picker.PickSaveFileAsync();

            if (file != null)
            {
                // If the file path and name is entered properly, and user has not tapped 'cancel'..

                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    // Encode the image into JPG format,reading for saving
                    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                    Stream pixelStream = image.PixelBuffer.AsStream();
                    byte[] pixels = new byte[pixelStream.Length];
                    await pixelStream.ReadAsync(pixels, 0, pixels.Length);
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)image.PixelWidth, (uint)image.PixelHeight, 96.0, 96.0, pixels);
                    await encoder.FlushAsync();
                } 
            }
        }

        private void Save_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string result = (this.DataContext as RecipeViewModel).SaveData();
             if (result.Contains("Success"))
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private async void CameraImage_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            StorageFile file = await picker.PickSingleFileAsync();
            //TODO: get the image
            //(this.DataContext as RecipeViewModel).Recipe.ImageSource = file.Path;
            BBQImage.Visibility = Visibility.Visible;
            CameraImage.Visibility = Visibility.Collapsed;
        }

    }
}
