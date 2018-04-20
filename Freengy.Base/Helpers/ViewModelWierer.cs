// Created by Laxale 20.04.2018
//
//

using System.Windows;
using System.ComponentModel;

using Prism.Mvvm;


namespace Freengy.Base.Helpers 
{
    /// <summary>
    /// Helps to autowire viewmodel in Prism in case XAML fails to design viewmodel.
    /// </summary>
    /// <para>
    /// Like this bullshit:
    /// </para>
    /// <para>
    /// The specified type 'Freengy.Base.Interfaces.ITaskWrapper' is not registered or could not be constructed. Please register type before using it.
    /// </para>
    public class ViewModelWierer 
    {
        /// <summary>
        /// Set the Prism AutoWireViewModel DependencyProperty on the view.
        /// </summary>
        /// <param name="view">Viewmodel owner view.</param>
        public void Wire(DependencyObject view) 
        {
            if (!DesignerProperties.GetIsInDesignMode(view))
            {
                // костыль. привязка лажает из разметки - "ITaskWrapper is interface and cannot be constructed"
                ViewModelLocator.SetAutoWireViewModel(view, true);
            }
        }
    }
}