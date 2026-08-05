using System;
using System.Windows;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;

namespace QuickLauncher
{
    public partial class MainWindow
    {
        /// <summary>
        /// Maximizes the window from the context menu.
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void Contextmenu_maximize_Click(object Sender, EventArgs e)
        {
            NormalWindow();
        }


        /// <summary>
        /// Opens the about windows from the context menu or title bar button
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void Button_about_Click(object Sender, EventArgs e)
        {
            NormalWindow();

            var about = new AboutApp()
            {
                Owner = Application.Current.MainWindow
            };

            about.ShowDialog();
        }


        /// <summary>
        /// Closes the app from the context menu.
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void Contextmenu_close_Click(object Sender, EventArgs e)
        {
            //if the window is hidden show it first before showing the app close confirm box
            if (WindowState == WindowState.Minimized && Classes.Globals.AppConfirmClose)
            {
                NormalWindow();
            }

            CloseApp();
        }


        /// <summary>
        /// Pin / unpin the app on top of all other programs.
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void Button_pin_Click(object Sender, EventArgs e)
        {
            //find the buttons
            var pinnedButton = this.RightWindowCommands.Items.OfType<Button>().FirstOrDefault(b => b.Name == "Pinned");
            var unPinnedButton = this.RightWindowCommands.Items.OfType<Button>().FirstOrDefault(b => b.Name == "UnPinned");

            //show or hide the correct pin button
            if (this.Topmost)
            {
                this.Topmost = false;

                pinnedButton.Visibility = Visibility.Collapsed;
                unPinnedButton.Visibility = Visibility.Visible;
            }
            else
            {
                this.Topmost = true;

                pinnedButton.Visibility = Visibility.Visible;
                unPinnedButton.Visibility = Visibility.Collapsed;
            }

            //mark settings as changed
            if (Sender != null)
            {
                Classes.Globals.AppSettings.AlwaysOnTop = this.Topmost;
                Classes.Globals.AppSettings.SettingsChanged();
            }
        }


        /// <summary>
        /// Minimize the main window.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized && Classes.Globals.AppConfirmClose)
            {
                this.Hide();
            }

            base.OnStateChanged(e);
        }


        /// <summary>
        /// On app close show confirm box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var response = new MessageBoxResult();

            //check if the confirm box must be shown
            if (!Classes.Globals.IsTest && Classes.Globals.AppConfirmClose)
            {
                response = MessageBox.Show(this, Localizer.GetLocalizedText("mainwindow-closeapp"), Localizer.GetLocalizedText("mainwindow-closing"), MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.OK);
            }
            else
            {
                response = MessageBoxResult.Yes;
            }

            //the confirm box result
            if (response == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                CloseApp();
            }
        }
    }
}
