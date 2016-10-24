// Created by Laxale 24.10.2016
//
//


namespace Freengy.Base.Interfaces 
{
    /// <summary>
    /// Represents object that has a name
    /// </summary>
    public interface INamedObject 
    {
        string Name { get; }

        string DisplayedName { get; }
    }
}