// Created by Laxale 18.05.2018
//
//

using System;


namespace Freengy.Base.Helpers 
{
    /// <summary>
    /// Простой класс для форсирования сборки мусора.
    /// </summary>
    public static class GcHelper 
    {
        /// <summary>
        /// Вызвать сборку мусора.
        /// </summary>
        public static void CollectGarbage() 
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}