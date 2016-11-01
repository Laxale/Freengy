// Created by Laxale 01.11.2016
//
//


namespace Freengy.SampleGame.Module 
{
    using Prism.Modularity;

    using Freengy.SampleGame.Views;
    using Freengy.GamePlugin.Helpers;

    using Settings = SampleGame.Settings;


    public class SampleGameModule : IModule 
    {
        public void Initialize() 
        {
            var validator = new PluginSettingsValidator(Settings.Default);

            validator.EnsureMainViewIsRegistered(typeof(SampleGameUi));
        }
    }
}