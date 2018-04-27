// Created by Laxale 23.10.2016
//
//

using System.Threading.Tasks;
using Freengy.Base.Helpers;
using Freengy.GamePlugin.Interfaces;
using Freengy.GamePlugin.DefaultImpl;

using Catel.IoC;
using Freengy.Common.Helpers;
using Prism.Modularity;


namespace Freengy.GamePlugin.Module 
{
    /// <summary>
    /// Exposes GamePlugin assembly <see cref="IModule"/> implementation
    /// </summary>
    public class GamePluginModule : IModule 
    {
        /// <summary>
        /// Registers GamePlugin internal types to service locator
        /// </summary>
        public void Initialize() 
        {
            using (new StatisticsDeployer(nameof(GamePluginModule)))
            {
                ServiceLocator.Default.RegisterInstance<IGameDispatcher>(GameDispatcher.Instance);

                // убийственно долгая енумерация единственного объекта. Вероятно, EF раздупляется со страшным скрипом
                Task.Run(() => ServiceLocator.Default.RegisterInstance<IGameListProvider>(GameListProvider.Instance));
            }
        }
    }
}