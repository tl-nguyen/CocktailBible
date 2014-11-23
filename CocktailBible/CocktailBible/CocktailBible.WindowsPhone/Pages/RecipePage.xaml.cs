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
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;

using CocktailBible.Common;
using CocktailBible.Models;
using CocktailBible.ViewModels;
using Windows.UI.Popups;

namespace CocktailBible.Pages
{
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
            if (e.Parameter == null)
            {
                (this.DataContext as RecipeViewModel).Recipe = new Recipe();
                PageTitle.Text = "New Recipe";
                BBQImage.Visibility = Visibility.Collapsed;
                CameraImage.Visibility = Visibility.Visible;
            }
            else
            {
                CameraImage.Visibility = Visibility.Collapsed;
                (this.DataContext as RecipeViewModel).Recipe = e.Parameter as Recipe;
            }

            if (String.IsNullOrEmpty((this.DataContext as RecipeViewModel).Recipe.IsLocal) &&
                !String.IsNullOrEmpty((this.DataContext as RecipeViewModel).Recipe.Name))
            {
                this.BarMenu.Visibility = Visibility.Collapsed;
            }

            navigationHelper.OnNavigatedTo(e);
        }

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

        public async void ContinueFileOpenPicker(FileOpenPickerContinuationEventArgs fileOpenPickerContinuationEventArgs)
        {
            if (fileOpenPickerContinuationEventArgs.Files != null)
            {
                StorageFile file = fileOpenPickerContinuationEventArgs.Files[0];

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

                        SaveImageToParse(file);
                    }
                }     
                BBQImage.Visibility = Visibility.Visible;
                CameraImage.Visibility = Visibility.Collapsed;
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            bool result = await (this.DataContext as RecipeViewModel).SaveData();

            if (!result)
            {
                MessageDialog dialog = new MessageDialog("Please fill all the fields first");
                await dialog.ShowAsync();
            }
            else
            {
                MessageDialog dialog = new MessageDialog("You have successfuly added new recipe");
                await dialog.ShowAsync();
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private void SnapPicture_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.Pickers.FileOpenPicker openPicker = new Windows.Storage.Pickers.FileOpenPicker()
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                ViewMode = PickerViewMode.Thumbnail
            };

            openPicker.FileTypeFilter.Clear();
            openPicker.FileTypeFilter.Add(".bmp");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".jpg");

            openPicker.PickSingleFileAndContinue();
        }

        private async void SaveImageToParse(StorageFile file)
        {
            if (file != null)
            {
                using (IRandomAccessStream stream = await file.OpenReadAsync())
                {
                    // Save to Parse procedure
                    RandomAccessStreamReference rasr = RandomAccessStreamReference.CreateFromStream(stream);
                    var streamWithContent = await rasr.OpenReadAsync();
                    byte[] buffer = new byte[streamWithContent.Size];

                    try
                    {
                        await streamWithContent.ReadAsync(buffer.AsBuffer(), (uint)streamWithContent.Size, InputStreamOptions.None);
                        var data = buffer;

                        if (data != null)
                        {
                            var recipePhoto = new Parse.ParseFile(file.Name, data);
                            await recipePhoto.SaveAsync();
                            (this.DataContext as RecipeViewModel).Recipe.ImageSource = recipePhoto;
                        }
                    }
                    catch (Exception e)
                    {
                        //TODO:
                    }
                }
            }
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
