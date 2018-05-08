// Created by Laxale 23.10.2016
//
//

using System.Threading.Tasks;

using Freengy.GamePlugin.Interfaces;
using Freengy.GamePlugin.DefaultImpl;
using Freengy.Base.DefaultImpl;
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
               MyServiceLocator.Instance.RegisterInstance<IGameDispatcher>(GameDispatcher.Instance);

                // убийственно долгая енумерация единственного объекта. Вероятно, EF раздупляется со страшным скрипом
                Task.Run(() =>MyServiceLocator.Instance.RegisterInstance<IGameListProvider>(GameListProvider.Instance));
            }
        }
    }
}