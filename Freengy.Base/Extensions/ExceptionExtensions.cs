// Created 19.10.2016
//
//


namespace Freengy.Base.Extensions 
{
    using System;
    

    public static class ExceptionExtensions 
    {
        /// <summary>
        /// For unknown reason existing methods doesnt give you really rootest exception
        /// </summary>
        /// <param name="sourceException"></param>
        /// <returns>Root exception (if aggregated incame)</returns>
        public static Exception GetReallyRootException(this Exception sourceException) 
        {
            if (sourceException == null) throw new ArgumentNullException(nameof(sourceException));

            Exception rootException = sourceException;

            while (rootException.InnerException != null)
            {
                rootException = rootException.InnerException;
            }

            return rootException;
        }
    }
}