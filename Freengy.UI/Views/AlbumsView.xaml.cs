// Created by Laxale 04.05.2018
//
//

using System.Windows.Controls;

using Freengy.Base.Attributes;
using Freengy.UI.ViewModels;


namespace Freengy.UI.Views 
{
    /// <summary>
    /// Вью коллекции альбомов.
    /// </summary>
    [HasViewModel(typeof(AlbumsViewModel))]
    public partial class AlbumsView : UserControl 
    {
        public AlbumsView() 
        {
            InitializeComponent();
        }
    }
}