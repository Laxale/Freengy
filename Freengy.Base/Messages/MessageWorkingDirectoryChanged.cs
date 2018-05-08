// Created by Laxale 31.10.2016
//
//


using Freengy.Base.Messages.Base;

namespace Freengy.Base.Messages 
{
    using System;
    using System.IO;


    /// <summary>
    /// Message says that smth changed in working directory - created, renamed, etc
    /// </summary>
    public class MessageWorkingDirectoryChanged : MessageBase 
    {
        public MessageWorkingDirectoryChanged(FileSystemEventArgs changedArgs) 
        {
            if (changedArgs == null) throw new ArgumentNullException(nameof(changedArgs));

            this.ChangedArgs = changedArgs;
        }


        public FileSystemEventArgs ChangedArgs { get; }
    }
}