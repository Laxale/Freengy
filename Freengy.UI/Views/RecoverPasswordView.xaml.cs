// Created by Laxale 13.11.2016
//
//


namespace Freengy.UI.Views 
{
    using System.Windows;

    using Freengy.UI.Helpers;
    using Freengy.CommonResources.XamlResources;

    using CatelControl = Catel.Windows.Controls.UserControl;


    public partial class RecoverPasswordView : CatelControl
    {
        private CommonResources.Controls.FlipPanel flipPanel;


        public RecoverPasswordView() 
        {
            this.InitializeComponent();
        }


        private void FlipPanel_Loaded(object sender, RoutedEventArgs e) 
        {
            // this bullshit is needed to 'bind' IsCodeSent property to IsFrontPanelActive
            // because normal binding doesnt work
            this.flipPanel = (CommonResources.Controls.FlipPanel)sender;

            var dataContext = this.DataContext as ViewModels.RecoverPasswordViewModel;

            if (dataContext == null) return;

            dataContext.PropertyChanged += (propSender, args) =>
            {
                if (args.PropertyName != nameof(dataContext.IsCodeSent)) return;

                UiDispatcher.Invoke(() => this.flipPanel.IsFrontPanelActive = !dataContext.IsCodeSent);
            };
        }
    }
}