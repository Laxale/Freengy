// Created 20.10.2016
//
//


namespace Freengy.Base.Extensions 
{
    public static class StringExtensions 
    {
        public static void FilterNewLineSymbols(this string target) 
        {
            if (string.IsNullOrWhiteSpace(target)) return;

            target = target.Replace(System.Environment.NewLine, string.Empty).Replace("\r", string.Empty);
        }
    }
}