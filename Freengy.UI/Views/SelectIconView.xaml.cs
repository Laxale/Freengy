// Created by Laxale 17.05.2018
//
//

using System.Windows;
using System.Windows.Controls;

using Freengy.Base.Attributes;
using Freengy.Base.Helpers;
using Freengy.UI.ViewModels;


namespace Freengy.UI.Views 
{
    /// <summary>
    /// Interaction logic for SelectIconView.xaml
    /// </summary>
    [HasViewModel(typeof(SelectIconViewModel))]
    public partial class SelectIconView : UserControl 
    {
        public SelectIconView() 
        {
            InitializeComponent();
        }


        private void IconImage_OnLoaded(object sender, RoutedEventArgs e) 
        {
            var image = (Image) sender;
            var context = (SelectableIconViewModel) image.DataContext;

            image.Source = new ImageLoader().LoadBitmapImage(context.IconModel.IconBlob).Value;
        }
    }
}