// Created by Laxale 04.05.2018
//
//

using System.Windows;
using System.Windows.Media;


namespace Freengy.Base.Helpers 
{
    /// <summary>
    /// Содержит вспомогательные функции для работы с визуальным деревом контролов.
    /// </summary>
    public class VisualTreeSearcher 
    {
        /// <summary>
        /// Найти по его типу родительский элемент в визуальном дереве контрола.
        /// </summary>
        /// <typeparam name="T">Тип родительского элемента.</typeparam>
        /// <param name="target">Контрол, родителя которого нужно найти.</param>
        /// <returns>Родитель типа <see cref="T"/> или null.</returns>
        public T FindParentOfType<T>(DependencyObject target) where T : class
        {
            DependencyObject parent = target;

            while ((parent = VisualTreeHelper.GetParent(parent)) != null)
            {
                if (parent is T) return parent as T;
            }

            return null;
        }
    }
}