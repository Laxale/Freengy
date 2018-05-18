// Created by Laxale 20.04.2018
//
//

using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

using Freengy.Base.Helpers;
using Freengy.Base.Attributes;
using Freengy.UI.ViewModels;

using Prism.Regions;


namespace Freengy.UI.Views 
{
    /// <summary>
    /// Interaction logic for MyAccountVisitView.xaml
    /// </summary>
    [RegionMemberLifetime(KeepAlive = false)]
    [HasViewModel(typeof(MyAccountVisitViewModel))]
    public partial class MyAccountVisitView 
    {
        private readonly ImageLoader loader = new ImageLoader();

        private Image avatarImage;


        public MyAccountVisitView() 
        {
            InitializeComponent();

            new ViewModelWierer().Wire(this);

            ContextGetter.PropertyChanged += OnPropertyChanged;
        }


        private MyAccountVisitViewModel ContextGetter => (MyAccountVisitViewModel) DataContext;


        private void AvatarImage_OnLoaded(object sender, RoutedEventArgs e) 
        {
            avatarImage = (Image) sender;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args) 
        {
            if (args.PropertyName != nameof(MyAccountVisitViewModel.CurrentAvatarPath))
            {
                return;
            }

            MyAccountVisitViewModel context = null;
            Dispatcher.Invoke(() => { context = ContextGetter; });

            if (!File.Exists(context.CurrentAvatarPath))
            {
                return;
            }

            var loadResult = loader.LoadBitmapImage(context.CurrentAvatarPath);
            if (loadResult.Success)
            {
                Dispatcher.Invoke(() => avatarImage.Source = loadResult.Value);
            }
        }

        private void TooltipImage_OnLoaded(object sender, RoutedEventArgs e) 
        {
            ((Image) sender).Source = avatarImage.Source;
        }
    }
}