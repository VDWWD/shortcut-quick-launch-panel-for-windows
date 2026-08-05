using System;
using System.Windows;
using MahApps.Metro.Controls;
using Hardcodet.Wpf.TaskbarNotification;
using System.Windows.Media;

namespace QuickLauncher
{
    public partial class MainWindow : MetroWindow
    {
        private TaskbarIcon TaskbarIcon;


        /// <summary>
        /// Initializes the Main Window.
        /// </summary>
        public MainWindow()
        {
            try
            {
                InitializeComponent();

                //on main window load
                InitializeMainWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Localizer.GetLocalizedText("mainwindow-error"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Actions on Main Window load.
        /// </summary>
        private void InitializeMainWindow()
        {
            //init the global variables
            Classes.Globals.InitGlobals();

            //create the taskbar icon
            CreateTaskBarIcon();

            //set the app title
            this.Title = string.Format("{0} {1}", Classes.Globals.AppDeveloper, Classes.Globals.AppName);

            //add testing to the title
            if (Classes.Globals.IsTest)
            {
                this.Title += " | TESTMODE";
            }

            //set the inital window dimensions
            this.Width = 300;
            this.Height = 330;
            this.MinWidth = this.Width;
            this.MinHeight = this.Height;
            this.ResizeMode = ResizeMode.CanResize;
            this.Background = (Brush)FindResource("White");

            //on dark mode change background
            if (Classes.Globals.IsDarkMode())
            {
                this.Background = (Brush)FindResource("DarkMode_Background");
            }

            //tooltip text in title bar
            WindowButtons.Minimize = Localizer.GetLocalizedText("mainwindow-minimize");
            WindowButtons.Maximize = Localizer.GetLocalizedText("mainwindow-maximize");
            WindowButtons.Restore = Localizer.GetLocalizedText("mainwindow-restore");
            WindowButtons.Close = Localizer.GetLocalizedText("mainwindow-close");

            //add the handlers and button clicks
            this.Closing += Window_Closing;

            //add buttons to the app header bar (about and pin/unpin)
            RightWindowCommands = CreateWindowCommands();

            //make the window snap to the edges of the screen
            Classes.SnapHelper.EnableEdgeSnapping(this);

            //pin the app if stored in settings
            if (Classes.Globals.AppSettings.AlwaysOnTop)
            {
                Button_pin_Click(null, null);
            }

            //app specieke init code
            InitAppSpecific(false);
        }
    }
}
