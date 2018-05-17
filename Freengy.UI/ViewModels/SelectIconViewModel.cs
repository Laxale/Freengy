// Created by Laxale 17.05.2018
//
//

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;

using Freengy.Base.Models.Readonly;
using Freengy.Base.ViewModels;
using Freengy.Common.Models;
using Freengy.Networking.Interfaces;


namespace Freengy.UI.ViewModels 
{
    internal class SelectIconViewModel : WaitableViewModel 
    {
        private readonly ObservableCollection<SelectableIconViewModel> allIcons = new ObservableCollection<SelectableIconViewModel>();


        public SelectIconViewModel() 
        {
            Icons = CollectionViewSource.GetDefaultView(allIcons);

            MyLevel = ServiceLocator.Resolve<ILoginController>().MyAccountState.Account.Level;

            FillDebugIcons();
        }


        public uint MyLevel { get; }

        public ICollectionView Icons { get; }


        private void FillDebugIcons() 
        {
            var achievableIcon1 = new AchievableIconModel
            {
                RequiredLevel = 3,
                Blob = File.ReadAllBytes(@"C:\Users\yaroshenko.ilia\Desktop\heroes\1.png")
            };
            var icon1 = new UserIconModel(achievableIcon1);
            var testViewModel1 = new SelectableIconViewModel(icon1, MyLevel, 3);
            
            var achievableIcon2 = new AchievableIconModel
            {
                RequiredLevel = 40,
                Blob = File.ReadAllBytes(@"C:\Users\yaroshenko.ilia\Desktop\heroes\2.png")
            };
            var icon2 = new UserIconModel(achievableIcon2);
            var testViewModel2 = new SelectableIconViewModel(icon2, MyLevel, 50);

            allIcons.Add(testViewModel1);
            allIcons.Add(testViewModel2);
        }
    }
}