// Created 20.10.2016
//
//


namespace Freengy.UI.Helpers 
{
    using System.Windows;
    using System.Windows.Controls;


    public static class PasswordBoxBinder 
    {
        #region dependency prop

        public static readonly DependencyProperty BindPasswordProperty =
            DependencyProperty.RegisterAttached
            (
                "BindPassword",
                typeof(bool),
                typeof(PasswordBoxBinder),
                new PropertyMetadata(false, OnBindPasswordChanged)
            );

        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached
            (
                "BoundPassword",
                typeof(string),
                typeof(PasswordBoxBinder),
                new PropertyMetadata(string.Empty, OnBoundPasswordChanged)
            );

        private static readonly DependencyProperty UpdatingPasswordProperty =
            DependencyProperty.RegisterAttached
            (
                "UpdatingPassword",
                typeof(bool),
                typeof(PasswordBoxBinder),
                new PropertyMetadata(false)
            );

        #endregion dependency prop


        public static bool GetBindPassword(DependencyObject dp) 
        {
            return (bool)dp.GetValue(BindPasswordProperty);
        }

        public static string GetBoundPassword(DependencyObject dp) 
        {
            return (string)dp.GetValue(BoundPasswordProperty);
        }


        public static void SetBindPassword(DependencyObject dp, bool value) 
        {
            dp.SetValue(BindPasswordProperty, value);
        }

        public static void SetBoundPassword(DependencyObject dp, string value) 
        {
            dp.SetValue(BoundPasswordProperty, value);
        }


        private static bool GetUpdatingPassword(DependencyObject dp) 
        {
            return (bool)dp.GetValue(UpdatingPasswordProperty);
        }

        private static void SetUpdatingPassword(DependencyObject dp, bool value) 
        {
            dp.SetValue(UpdatingPasswordProperty, value);
        }


        private static void OnPasswordChanged(object sender, RoutedEventArgs e) 
        {
            var boxSender = sender as PasswordBox;

            if (boxSender == null) throw new System.ArgumentException("Not a passwordbox sender");

            // set a flag to indicate that we're updating the password
            SetUpdatingPassword(boxSender, true);
            // push the new password into the BoundPassword property
            SetBoundPassword(boxSender, boxSender.Password);
            SetUpdatingPassword(boxSender, false);
        }

        private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e) 
        {
            // when the BindPassword attached property is set on a PasswordBox,
            // start listening to its PasswordChanged event

            var box = dp as PasswordBox;

            if (box == null) return;


            if ((bool)e.OldValue)
            {
                box.PasswordChanged -= OnPasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                box.PasswordChanged += OnPasswordChanged;
            }
        }

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
        {
            var box = d as PasswordBox;

            // only handle this event when the property is attached to a PasswordBox
            // and when the BindPassword attached property has been set to true
            if (d == null || !GetBindPassword(d))
            {
                return;
            }

            // avoid recursive updating by ignoring the box's changed event
            box.PasswordChanged -= OnPasswordChanged;

            string newPassword = (string)e.NewValue;

            if (!GetUpdatingPassword(box))
            {
                box.Password = newPassword;
            }

            box.PasswordChanged += OnPasswordChanged;
        }
    }
}