// Created by Laxale 01.11.2016
//
//

using Prism.Modularity;

using Freengy.Base.Helpers;
using Freengy.Common.Helpers;
using Freengy.SampleGame.Views;
using Freengy.GamePlugin.Helpers;


namespace Freengy.SampleGame.Module 
{
    public class SampleGameModule : IModule 
    {
        public void Initialize() 
        {
            using (new StatisticsDeployer(nameof(SampleGameModule)))
            {
                var validator = new PluginSettingsValidator(Settings.Default);

                validator.EnsureMainViewIsRegistered(typeof(SampleGameUi));
            }
        }
    }
}