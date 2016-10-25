// Created by Laxale 25.10.2016
//
//


namespace Freengy.Base.Helpers 
{
    using System;


    public static class Common 
    {
        public static void ThrowIfArgumentsHasNull(params object[] args) 
        {
            if (args == null) throw new ArgumentNullException(nameof(args));

            foreach (var arg in args)
            {
                var argumentCopy = arg;

                if (arg != null) continue;

                throw new ArgumentNullException(nameof(argumentCopy));
            }
        }
    }
}