// Created 20.10.2016
//
//


namespace Freengy.Base.Extensions 
{
    public static class StringExtensions
    {
        private static readonly char[] NewLineSymbols = { '\r', '\n' };


        public static string FilterNewLineSymbols(this string target) 
        {
            if (string.IsNullOrWhiteSpace(target)) return target;

            string[] split = target.Split(StringExtensions.NewLineSymbols);

            target = string.Concat(split);

            return target;
        }
    }
}