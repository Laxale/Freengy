// Created by Laxale 01.11.2016
//
//

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Interop;
using System.Runtime.InteropServices;


namespace Freengy.CommonResources.Styles 
{
    using System.Windows.Controls;


    public static class LocalExtensions 
    {
        internal static IntPtr GetWindowHandle(this Window window) 
        {
            WindowInteropHelper helper = new WindowInteropHelper(window);
            return helper.Handle;
        }

        internal static void ForWindowFromChild(this object childDependencyObject, Action<Window> action) 
        {
            var element = childDependencyObject as DependencyObject;

            while (element != null)
            {
                element = VisualTreeHelper.GetParent(element);
                if (element is Window) { action(element as Window); break; }
            }
        }

        internal static void ForWindowFromTemplate(this object templateFrameworkElement, Action<Window> action) 
        {
            Window window = ((FrameworkElement)templateFrameworkElement).TemplatedParent as Window;
            if (window != null) action(window);
        }
    }


    public partial class StylishWindowStyle 
    {
        public static event Action<ContentControl> ToolbarRegionLoadedEvent = hostControl => { };


        #region sizing event handlers

        private void OnSizeSouth(object sender, MouseButtonEventArgs e) { this.OnSize(sender, SizingAction.South); }
        private void OnSizeNorth(object sender, MouseButtonEventArgs e) { this.OnSize(sender, SizingAction.North); }
        private void OnSizeEast(object sender, MouseButtonEventArgs e) { this.OnSize(sender, SizingAction.East); }
        private void OnSizeWest(object sender, MouseButtonEventArgs e) { this.OnSize(sender, SizingAction.West); }
        private void OnSizeNorthWest(object sender, MouseButtonEventArgs e) { this.OnSize(sender, SizingAction.NorthWest); }
        private void OnSizeNorthEast(object sender, MouseButtonEventArgs e) { this.OnSize(sender, SizingAction.NorthEast); }
        private void OnSizeSouthEast(object sender, MouseButtonEventArgs e) { this.OnSize(sender, SizingAction.SouthEast); }
        private void OnSizeSouthWest(object sender, MouseButtonEventArgs e) { this.OnSize(sender, SizingAction.SouthWest); }

        private void OnSize(object sender, SizingAction action) 
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                sender.ForWindowFromTemplate(w =>
                {
                    if (w.WindowState == WindowState.Normal)
                    {
                        StylishWindowStyle.DragSize(w.GetWindowHandle(), action);
                    }
                });
            }
        }

        private void IconMouseLeftButtonDown(object sender, MouseButtonEventArgs e) 
        {
            if (e.ClickCount > 1)
            {
                sender.ForWindowFromTemplate(w => w.Close());
            }
            else
            {
                sender.ForWindowFromTemplate(w => StylishWindowStyle.SendMessage(w.GetWindowHandle(), StylishWindowStyle.WmSyscommand, (IntPtr)StylishWindowStyle.ScKeymenu, (IntPtr)' '));
            }
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e) 
        {
            sender.ForWindowFromTemplate(w => w.Close());
        }

        private void MinButtonClick(object sender, RoutedEventArgs e) 
        {
            sender.ForWindowFromTemplate(w => w.WindowState = WindowState.Minimized);
        }

        private void MaxButtonClick(object sender, RoutedEventArgs e) 
        {
            sender.ForWindowFromTemplate(w =>
            {
                w.WindowState = w.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            });
        }

        private void TitleBarMouseLeftButtonDown(object sender, MouseButtonEventArgs e) 
        {
            if (e.ClickCount > 1)
            {
                this.MaxButtonClick(sender, e);
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                sender.ForWindowFromTemplate(w => w.DragMove());
            }
        }

        private void TitleBarMouseMove(object sender, MouseEventArgs e) 
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                sender.ForWindowFromTemplate(w =>
                {
                    if (w.WindowState == WindowState.Maximized)
                    {
                        w.BeginInit();
                        double adjustment = 40.0;
                        var mouse1 = e.MouseDevice.GetPosition(w);
                        var width1 = Math.Max(w.ActualWidth - 2 * adjustment, adjustment);
                        w.WindowState = WindowState.Normal;
                        var width2 = Math.Max(w.ActualWidth - 2 * adjustment, adjustment);
                        w.Left = (mouse1.X - adjustment) * (1 - width2 / width1);
                        w.Top = -7;
                        w.EndInit();
                        w.DragMove();
                    }
                });
            }
        }

        #endregion


        private const int ScSize = 0xF000;
        private const int ScKeymenu = 0xF100;
        private const int WmSyscommand = 0x112;

        public enum SizingAction
        {
            North = 3,
            South = 6,
            East = 2,
            West = 1,
            NorthEast = 5,
            NorthWest = 4,
            SouthEast = 8,
            SouthWest = 7
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        private static void DragSize(IntPtr handle, SizingAction sizingAction)
        {
            StylishWindowStyle.SendMessage(handle, StylishWindowStyle.WmSyscommand, (IntPtr)(StylishWindowStyle.ScSize + sizingAction), IntPtr.Zero);
            StylishWindowStyle.SendMessage(handle, 514, IntPtr.Zero, IntPtr.Zero);
        }


        private void ToolbarHost_OnLoaded(object sender, RoutedEventArgs e) 
        {
            var control = (ContentControl)sender;

            ToolbarRegionLoadedEvent.Invoke(control);
        }
    }
}