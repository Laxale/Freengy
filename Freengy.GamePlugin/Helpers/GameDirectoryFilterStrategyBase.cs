// Created by Laxale 07.12.2016
//
//


namespace Freengy.GamePlugin.Helpers 
{
    internal abstract class GameDirectoryFilterStrategyBase 
    {
        protected GameDirectoryFilterStrategyBase() 
        {
            
        }


        public abstract bool IsGameFolder(string folderPath, out string gameDllPath);
    }
}