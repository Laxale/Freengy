// Created by Laxale 10.05.2018
//
//

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Freengy.UI.ViewModels;
using Freengy.Base.Attributes;
using Freengy.Base.Extensions;
using Freengy.Base.Helpers;


namespace Freengy.UI.Views 
{
    /// <summary>
    /// Interaction logic for EditMyAccountView.xaml
    /// </summary>
    [HasViewModel(typeof(EditMyAccountViewModel))]
    public partial class EditMyAccountView : UserControl 
    {
        private readonly ImageLoader loader = new ImageLoader();

        private Image avatarImage;
        

        public EditMyAccountView() 
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty IsReadyToDropProperty = 
            DependencyProperty.Register(nameof(IsReadyToDrop), typeof(bool), typeof(EditMyAccountView));

        public static readonly DependencyProperty InformationProperty = 
            DependencyProperty.Register(nameof(Information), typeof(string), typeof(EditMyAccountView));

        public static readonly DependencyProperty CanDropProperty = 
            DependencyProperty.Register(nameof(CanDrop), typeof(bool), typeof(EditMyAccountView), new PropertyMetadata(true));

        public bool CanDrop 
        {
            get => (bool) GetValue(CanDropProperty);
            private set => SetValue(CanDropProperty, value);
        }

        public string Information 
        {
            get => (string) GetValue(InformationProperty);
            private set => SetValue(InformationProperty, value);
        }

        public bool IsReadyToDrop 
        {
            get => (bool) GetValue(IsReadyToDropProperty);
            private set => SetValue(IsReadyToDropProperty, value);
        }


        private EditMyAccountViewModel ViewModelGetter => (EditMyAccountViewModel) DataContext;

        private void AvatarImage_OnLoaded(object sender, RoutedEventArgs e) 
        {
            avatarImage = (Image) sender;

            var avatarResult = new ImageLoader().LoadBitmapImage(ViewModelGetter.AvatarPath);

            if (avatarResult.Success)
            {
                avatarImage.Source = avatarResult.Value;
            }
        }

        private void AvatarBorder_OnDragEnter(object sender, DragEventArgs e) 
        {
            IsReadyToDrop = true;

            using (var drager = new DragAndDropHelper(this, false))
            {
                string[] files = drager.GetDroppedPaths(e);

                string firstFile = files.First();

                if (!AlbumExtensions.IsImagePath(firstFile))
                {
                    Information = "File is not an image";
                    CanDrop = false;
                }
                else
                {
                    CanDrop = true;
                }
            }
        }

        private void AvatarBorder_OnDragLeave(object sender, DragEventArgs e) 
        {
            Information = null;
            CanDrop = true;
            IsReadyToDrop = false;
        }

        private void AvatarBorder_OnDrop(object sender, DragEventArgs e) 
        {
            if (!CanDrop)
            {
                Information = null;
                IsReadyToDrop = false;
                CanDrop = true;
                return;
            }

            using (var drager = new DragAndDropHelper(this, false))
            {
                string[] files = drager.GetDroppedPaths(e);

                string firstFile = files.First();

                if (!AlbumExtensions.IsImagePath(firstFile))
                {
                    Information = "File is not an image";
                }
                else
                {
                    var loadResult = loader.LoadBitmapImage(firstFile);

                    if (loadResult.Success)
                    {
                        Information = null;
                        avatarImage.Source = loadResult.Value;
                        ViewModelGetter.AvatarPath = firstFile;
                    }
                    else
                    {
                        Information = loadResult.Error.Message;
                    }
                }
            }
        }
    }
}