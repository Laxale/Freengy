// Created by Laxale 17.05.2018
//
//

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;


namespace Freengy.CommonResources.GrayScale 
{
    /// <summary>
    /// Эффект перевода изображения в серые тона.
    /// </summary>
    public class GrayscaleEffect : ShaderEffect 
    {
        /// <summary>
        /// Конструктор <see cref="GrayscaleEffect"/>.
        /// </summary>
        public GrayscaleEffect() 
        {
            Uri pixelShaderUri = new Uri("pack://application:,,,/Freengy.CommonResources;component/GrayScale/GrayscaleEffect.ps", UriKind.RelativeOrAbsolute);

            PixelShader = new PixelShader
            {
                UriSource = pixelShaderUri
            };

            UpdateShaderValue(InputProperty);
        }


        /// <summary>
        /// DependencyProperty свойства Input.
        /// </summary>
        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty(nameof(Input), typeof(GrayscaleEffect), 0 /* assigned to sampler register S0 */);

        /// <summary>
        /// Входная <see cref="ImageBrush"/> для наложения на неё эффекта.
        /// </summary>
        public Brush Input 
        {
            get => (Brush)GetValue(InputProperty);

            set => SetValue(InputProperty, value);
        }
    }
}