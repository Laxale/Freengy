﻿// Created by Laxale 02.11.2016
//
//


namespace Freengy.Base.Interfaces 
{
    public interface IChatMessage  
    {
        string Text { get; }

        IUserAccount Author { get; }
    }
}