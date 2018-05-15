// Created by Laxale 11.05.2018
//
//

using System;
using System.Windows.Controls;

using Freengy.Base.Attributes;
using Freengy.Base.Helpers;
using Freengy.Base.Models.Update;
using Freengy.FriendList.ViewModels;


namespace Freengy.FriendList.Views 
{
    /// <summary>
    /// Interaction logic for FriendUpdatesView.xaml
    /// </summary>
    [HasViewModel(typeof(FriendUpdatesViewModel))]
    public partial class FriendUpdatesView : UserControl 
    {
        public FriendUpdatesView() 
        {
            InitializeComponent();

            new ViewModelWierer().Wire(this);

            if (DataContext is FriendUpdatesViewModel context)
            {
                context.GotNewUpdate += OnGotNewUpdate;
            }
        }


        private void OnGotNewUpdate() 
        {
            Scroller.ScrollToEnd();
        }
    }
}