// Created by Laxale 25.10.2016
//
//


namespace Freengy.Base.Helpers 
{
    /// <summary>
    /// Defines string literal to use as filter (search pattern) when locating files
    /// </summary>
    public abstract class FileSearchFilterBase 
    {
        protected FileSearchFilterBase() 
        {
            
        }


        public abstract string SearchFilter { get; }
    }
}