// Created by Laxale 13.11.2016
//
//

using System.Windows;

using Freengy.UI.Helpers;
using Freengy.UI.ViewModels;
using Freengy.Base.Attributes;


namespace Freengy.UI.Views 
{
    [HasViewModel(typeof(RecoverPasswordViewModel))]
    public partial class RecoverPasswordView 
    {
        private CommonResources.Controls.FlipPanel flipPanel;


        public RecoverPasswordView() 
        {
            InitializeComponent();
        }


        private void FlipPanel_Loaded(object sender, RoutedEventArgs e) 
        {
            // this bullshit is needed to 'bind' IsCodeSent property to IsFrontPanelActive
            // because normal binding doesnt work
            flipPanel = (CommonResources.Controls.FlipPanel)sender;

            if (!(this.DataContext is RecoverPasswordViewModel dataContext))
            {
                return;
            }

            dataContext.PropertyChanged += (propSender, args) =>
            {
                if (args.PropertyName != nameof(dataContext.IsCodeSent)) return;

                UiDispatcher.Invoke(() => flipPanel.IsFrontPanelActive = !dataContext.IsCodeSent);
            };
        }
    }
}