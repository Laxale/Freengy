// Created by Laxale 05.12.2016
//
//


namespace Freengy.CommonResources.Controls 
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using System.Windows.Media.Animation;


    public class GifImage : Image 
    {
        #region vars 

        private bool isInitialized;
        private Int32Animation animation;
        private GifBitmapDecoder gifDecoder;

        #endregion vars


        #region dependency props

        public static readonly DependencyProperty FrameIndexProperty =
            DependencyProperty.Register
            (
                nameof(FrameIndex), 
                typeof(int), 
                typeof(GifImage), 
                new UIPropertyMetadata(0, GifImage.ChangingFrameIndex)
            );

        /// <summary>
        /// Frame rate in milliseconds
        /// </summary>
        public static readonly DependencyProperty FrameRateProperty =
            DependencyProperty.Register
            (
                nameof(FrameRate),
                typeof(int),
                typeof(GifImage),
                new UIPropertyMetadata(50)
            );

        public static readonly DependencyProperty IsAutoStartProperty =
            DependencyProperty.Register
            (
                nameof(IsAutoStart),
                typeof(bool), 
                typeof(GifImage), 
                new UIPropertyMetadata(false)
            );

        public static readonly DependencyProperty GifSourceProperty =
            DependencyProperty.Register
            (
                nameof(GifSource),
                typeof(string), 
                typeof(GifImage), 
                new UIPropertyMetadata(string.Empty)
            );

        #endregion dependency props


        static GifImage() 
        {
            UIElement.VisibilityProperty.OverrideMetadata(typeof(GifImage), new FrameworkPropertyMetadata(GifImage.VisibilityPropertyChanged));
        }
        public GifImage() 
        {
            base.Loaded += this.LoadedHandler;
        }


        public void StopAnimation() 
        {
            this.BeginAnimation(GifImage.FrameIndexProperty, null);
        }
        public void StartAnimation() 
        {
            if (!this.isInitialized) this.Initialize();

            this.BeginAnimation(FrameIndexProperty, this.animation);
        }


        public int FrameRate 
        {
            get { return (int)GetValue(FrameRateProperty); }
            set { SetValue(FrameRateProperty, value); }
        }
        public int FrameIndex 
        {
            get { return (int)GetValue(FrameIndexProperty); }
            set { SetValue(FrameIndexProperty, value); }
        }
        public bool IsAutoStart 
        {
            get { return (bool)GetValue(GifImage.IsAutoStartProperty); }
            set { SetValue(GifImage.IsAutoStartProperty, value); }
        }
        public string GifSource 
        {
            get { return (string)GetValue(GifSourceProperty); }
            set { SetValue(GifSourceProperty, value); }
        }


        private void Initialize() 
        {
            string uriString =
                this.GifSource.StartsWith("pack://application:,,,") ? 
                    this.GifSource : 
                    "pack://application:,,," + this.GifSource;

            var uri = new Uri(uriString);

            this.gifDecoder = new GifBitmapDecoder(uri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

            var duration = new TimeSpan(0, 0, 0, 0, this.gifDecoder.Frames.Count * this.FrameRate);

            this.animation = new Int32Animation(0, this.gifDecoder.Frames.Count - 1, duration)
            {
                RepeatBehavior = RepeatBehavior.Forever
            };

            this.Source = this.gifDecoder.Frames[0];

            this.isInitialized = true;
        }
        private void LoadedHandler(object sender, RoutedEventArgs args) 
        {
            this.Initialize();

            if (this.IsAutoStart) this.StartAnimation();
        }

        private static void ChangingFrameIndex(DependencyObject obj, DependencyPropertyChangedEventArgs ev) 
        {
            var gifImage = obj as GifImage;
            if (gifImage != null) gifImage.Source = gifImage.gifDecoder.Frames[(int)ev.NewValue];
        }
        private static void VisibilityPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) 
        {
            if ((Visibility)e.NewValue == Visibility.Visible)
            {
                ((GifImage)sender).StartAnimation();
            }
            else
            {
                ((GifImage)sender).StopAnimation();
            }
        }
    }
}