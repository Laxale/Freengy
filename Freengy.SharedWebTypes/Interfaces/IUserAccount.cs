// Created by Laxale 02.12.2016
//
//


namespace Freengy.SharedWebTypes.Interfaces 
{
    public interface IUserAccount : INamedObject, IObjectWithId 
    {
        int Level { get; }
    }
}