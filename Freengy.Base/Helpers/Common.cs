// Created by Laxale 25.10.2016
//
//

using System;
using System.IO;
using System.Linq;


namespace Freengy.Base.Helpers 
{
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

        public static bool HasInvalidSymbols(string arg) 
        {
            if (string.IsNullOrWhiteSpace(arg)) return false;

            var badSymbols = Path.GetInvalidFileNameChars().Union(Path.GetInvalidFileNameChars());

            bool hasInvalid = arg.Intersect(badSymbols).Any();

            return hasInvalid;
        }
    }
}