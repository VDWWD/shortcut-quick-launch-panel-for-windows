using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ControlzEx;
using Hardcodet.Wpf.TaskbarNotification;
using MahApps.Metro.Controls;

namespace QuickLauncher
{
    public partial class MainWindow
    {
        /// <summary>
        /// On window maximize.
        /// </summary>
        public void NormalWindow()
        {
            this.Show();
            this.Activate();
            this.WindowState = WindowState.Normal;
        }


        /// <summary>
        /// When the app is going to close.
        /// </summary>
        private void CloseApp()
        {
            //save the settings before close
            Classes.Globals.AppSettings.SaveOnExit();

            //dispose the taskbar icon
            if (TaskbarIcon != null)
            {
                TaskbarIcon.Dispose();
            }

            Application.Current.Shutdown();
            Environment.Exit(0);
        }


        /// <summary>
        /// Create the taskbar icon with context menu.
        /// </summary>
        private void CreateTaskBarIcon()
        {
            //create the taskbar icon
            TaskbarIcon = new TaskbarIcon()
            {
                ToolTipText = this.Title,
                Icon = new Icon(Classes.ResourceController.GetStreamFromResource("favicon_taskbar.ico")),
                ContextMenu = new ContextMenu()
                {
                    HasDropShadow = false
                }
            };

            //add the click commands
            TaskbarIcon.DoubleClickCommand = new Classes.IconController.FaviconDoubleClickCommand();
            TaskbarIcon.LeftClickCommand = new Classes.IconController.FaviconDoubleClickCommand();

            //add items to the menu
            TaskbarIcon.ContextMenu.Items.Add(Classes.ResourceController.CreateContextMenuItem(Classes.Enums.Icon.Maximize, Localizer.GetLocalizedText("mainwindow-maximize"), Contextmenu_maximize_Click));
            TaskbarIcon.ContextMenu.Items.Add(Classes.ResourceController.CreateContextMenuItem(Classes.Enums.Icon.About, Localizer.GetLocalizedText("mainwindow-about"), Button_about_Click));
            TaskbarIcon.ContextMenu.Items.Add(new Separator());
            TaskbarIcon.ContextMenu.Items.Add(Classes.ResourceController.CreateContextMenuItem(Classes.Enums.Icon.Close, Localizer.GetLocalizedText("mainwindow-close"), Contextmenu_close_Click));

            //insert it at the top of the dockpanel
            MainDockPanel.Children.Insert(0, TaskbarIcon);
        }


        /// <summary>
        /// Create the 3 buttons in the window title bar (abount and pin/unpin)
        /// </summary>
        /// <returns>WindowCommands</returns>
        private WindowCommands CreateWindowCommands()
        {
            var commands = new WindowCommands();

            //about app button
            commands.Items.Add(CreateCommandButton("AboutAppHeader", Classes.IconController.GetIcon(Classes.Enums.Icon.About), Button_about_Click, Localizer.GetLocalizedText("mainwindow-about")));

            //unpin button
            commands.Items.Add(CreateCommandButton("UnPinned", Classes.IconController.GetIcon(Classes.Enums.Icon.Unpin), Button_pin_Click, Localizer.GetLocalizedText("mainwindow-ontop")));

            //pin button
            commands.Items.Add(CreateCommandButton("Pinned", Classes.IconController.GetIcon(Classes.Enums.Icon.Pin), Button_pin_Click, Localizer.GetLocalizedText("mainwindow-ontop"), Visibility.Collapsed));

            return commands;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">The unique name of the button.</param>
        /// <param name="icon">The icon for in the button.</param>
        /// <param name="handler">The click handler to be attached to the button.</param>
        /// <param name="text">The tooltip text.</param>
        /// <param name="visibility">The visibility of the button.</param>
        /// <returns></returns>
        private Button CreateCommandButton(string name, Classes.VariousClasses.ButtonIcon icon, RoutedEventHandler handler, string tooltiptext, Visibility visibility = Visibility.Visible)
        {
            var button = new Button
            {
                Name = name,
                Visibility = visibility
            };

            button.Click += handler;

            var panel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            var viewbox = new Viewbox
            {
                Width = 24,
                Height = 24
            };

            var canvas = new Canvas
            {
                Width = 24,
                Height = 24
            };

            var path = new Path
            {
                Data = Geometry.Parse(icon.path),
                Fill = (System.Windows.Media.Brush)FindResource("White")
            };

            canvas.Children.Add(path);
            viewbox.Child = canvas;
            panel.Children.Add(viewbox);

            button.Content = panel;

            var tooltip = new ToolTip
            {
                Name = $"tt_{name}",
                Content = tooltiptext,
                Style = (Style)FindResource("ToolTip_Normal")
            };

            //move the tooltip with the cursor
            ToolTipAssist.SetAutoMove(tooltip, true);

            button.ToolTip = tooltip;

            return button;
        }
    }
}
