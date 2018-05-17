// Created by Laxale 17.05.2018
//
//

using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Freengy.Base.ViewModels 
{
    /// <summary>
    /// Простейший базовый класс вьюмодели.
    /// </summary>
    public abstract class BasicViewModel : INotifyPropertyChanged  
    {
        /// <summary>
        /// Событие изменения значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Вызов события изменения значения свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}