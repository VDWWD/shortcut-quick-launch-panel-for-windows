using System;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace QuickLauncher
{
    public partial class PickColor : MetroWindow
    {
        public bool ForButtonColor { get; set; }

        /// <summary>
        /// On color picker window open.
        /// </summary>
        /// <param name="for_button_color"></param>
        /// <param name="color"></param>
        public PickColor(bool for_button_color, Color color)
        {
            InitializeComponent();

            ForButtonColor = for_button_color;

            //set the window title
            this.Title = Localizer.GetLocalizedText("colorpicker-title");

            //set the inital window dimensions
            this.Width = 200;
            this.Height = 280;
            this.MinWidth = this.Width;
            this.MinHeight = this.Height;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.Background = (Brush)FindResource("White");

            //darkmode detected then change colors
            if (Classes.Globals.IsDarkMode())
            {
                this.Background = Classes.ResourceController.BrushDarkModeBackground;

                button_ok.Content = Classes.IconController.GetButton(Classes.Enums.Icon.Check, Classes.ResourceController.BrushBlack, Localizer.GetLocalizedText("about-ok"));
            }
            else
            {
                button_ok.Content = Classes.IconController.GetButton(Classes.Enums.Icon.Check, Classes.ResourceController.BrushWhite, Localizer.GetLocalizedText("about-ok"));
            }

            //set the color
            color_picker.SelectedColor = color;

            //add the handlers and button clicks
            this.KeyDown += OnKeyDownHandler;
            button_ok.Click += Bbutton_ok_Click;
        }


        /// <summary>
        /// Closes the window and sets the color to the correct textboxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bbutton_ok_Click(object sender, RoutedEventArgs e)
        {
            var owner = ((EditItem)Owner);
            string hexcolor = string.Format("#{0:X2}{1:X2}{2:X2}", color_picker.SelectedColor.R, color_picker.SelectedColor.G, color_picker.SelectedColor.B);

            if (ForButtonColor)
            {
                owner.textbox_color_button.Text = hexcolor;
            }
            else
            {
                owner.textbox_color_icon.Text = hexcolor;
            }

            this.DialogResult = true;
        }


        /// <summary>
        /// Closes the about windows on the keys Enter, Return, Back and Escape
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                Bbutton_ok_Click(null, null);
            }
            else if (e.Key == Key.Escape || e.Key == Key.Back)
            {
                Close();
            }
        }
    }
}
