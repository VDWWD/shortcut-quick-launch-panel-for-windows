using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using System.Windows.Media;

namespace QuickLauncher
{
    public partial class AboutApp : MetroWindow
    {
        /// <summary>
        /// Initializes the About App window
        /// </summary>
        public AboutApp()
        {
            InitializeComponent();

            //set the window title
            this.Title = Localizer.GetLocalizedText("mainwindow-about");

            //set the inital window dimensions
            this.Width = 340;
            this.Height = 320;
            this.MinWidth = this.Width;
            this.MinHeight = this.Height;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.Background = (Brush)FindResource("White");

            //set items in the about window
            txt_copyright2.Text = string.Format("{0}, {1}", DateTime.Now.Year.ToString(), Classes.Globals.AppCopyright);
            txt_website.Text = Classes.Globals.AppUrl.Replace("https://", "");
            txt_email.Text = Classes.Globals.AppEmail;
            txt_versie.Text = string.Format("{0} {1}", Localizer.GetLocalizedText("about-version"), Classes.Globals.AppVersion.ToString().Replace(",", "."));
            txt_button.Text = Localizer.GetLocalizedText("about-ok");

            //add the vdwwd logo from the embedded resources
            Vdwwdlogo.Source = Classes.ResourceController.GetImageFromResource("vdwwd.png");

            //on darkmode change colors and styles
            if (Classes.Globals.IsDarkMode())
            {
                var textcolor = (Brush)FindResource("DarkMode_Text");
                var buttoncolor = (Brush)FindResource("Black");
                this.Background = (Brush)FindResource("DarkMode_Background");

                txt_copyright1.Foreground = textcolor;
                txt_copyright2.Foreground = textcolor;
                txt_website.Foreground = textcolor;
                txt_email.Foreground = textcolor;
                txt_versie.Foreground = textcolor;
                txt_button.Foreground = textcolor;

                txt_button.Foreground = buttoncolor;
                icon1.Fill = buttoncolor;
            }

            //add the handlers and button clicks
            this.KeyDown += OnKeyDownHandler;
            Hyperlink1.Click += Hyperlink1_Click;
            Hyperlink2.Click += Hyperlink2_Click;
            Button1.Click += Button1_Click;
        }


        /// <summary>
        /// The OK button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        /// <summary>
        /// Closes the about windows on the keys Enter, Return, Back and Escape.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter || e.Key == Key.Escape || e.Key == Key.Back)
            {
                Button1_Click(null, null);
            }
        }


        /// <summary>
        /// Opem the app url when the link is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink1_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Classes.Globals.AppUrl);
        }


        /// <summary>
        /// Trigger an mailto action to the app email when the link is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink2_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("mailto:" + Classes.Globals.AppEmail);
        }
    }
}
