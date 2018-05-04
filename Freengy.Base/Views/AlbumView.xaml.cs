// Created by Laxale 04.05.2018
//
//

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using Freengy.Base.Attributes;
using Freengy.Base.Helpers;
using Freengy.Base.Messages;
using Freengy.Base.ViewModels;
using Freengy.Common.Models;

using Catel.Messaging;
using Freengy.Common.Helpers;
using Freengy.Common.Helpers.Result;


namespace Freengy.Base.Views 
{
    /// <summary>
    /// Interaction logic for AlbumView.xaml
    /// </summary>
    [HasViewModel(typeof(AlbumViewModel))]
    public partial class AlbumView : UserControl 
    {
        public AlbumView() 
        {
            InitializeComponent();
        }


        private void Image_OnLoaded(object sender, RoutedEventArgs e) 
        {
            var imageSender = (Image) sender;

            var context = (ImageModel)imageSender.DataContext;
            string imgSource;

            if (!string.IsNullOrWhiteSpace(context.LocalUrl))
            {
                imgSource = context.LocalUrl;
            }
            else if(!string.IsNullOrWhiteSpace(context.RemoteUrl))
            {
                string extension = context.RemoteUrl.Split('.').Last();
                var folderHelper = new FolderHelper();
                imgSource = Path.Combine(folderHelper.TempDirectoryPath, $"freengy_img_{context.Id}.{ extension }");
                using (var fileStream = File.Create(imgSource))
                {
                    Result<Stream> downloadResult = new Downloader().DownloadContent(context.RemoteUrl);

                    if (downloadResult.Success)
                    {
                        using (downloadResult.Value)
                        {
                            downloadResult.Value.CopyTo(fileStream);
                        }
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Image source is empty. Both local and remote");
            }

            imageSender.Source = LoadBitmapImage(imgSource);
        }

        private void AlbumView_OnKeyDown(object sender, KeyEventArgs e) 
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                return;
            }

            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                new KeyHandler(e).ExecuteOnKeyPressed(Key.V, HandleImageInput);
            }
        }


        private void HandleImageInput() 
        {
            string inputText = Clipboard.GetText();

            if (string.IsNullOrWhiteSpace(inputText)) return;

            MessageMediator.Default.SendMessage(new MessageAddImageRequest(inputText));
        }

        private BitmapImage LoadBitmapImage(string imagePath) 
        {
            using (var stream = new FileStream(imagePath, FileMode.Open))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // just in case you want to load the image in another thread

                return bitmapImage;
            }
        }
    }
}