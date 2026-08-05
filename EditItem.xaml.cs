using System;
using System.Windows;
using System.Linq;
using System.Windows.Input;
using MahApps.Metro.Controls;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;

namespace QuickLauncher
{
    public partial class EditItem : MetroWindow
    {
        private Classes.VariousClasses.ShortCut ShortCut { get; set; }
        private bool IsNewIcon { get; set; }


        /// <summary>
        /// On edit window open.
        /// </summary>
        /// <param name="shortcut"></param>
        public EditItem(Classes.VariousClasses.ShortCut shortcut)
        {
            InitializeComponent();

            ShortCut = shortcut;
            int height = 560;

            //is it an edit or a new shorcut
            if (ShortCut == null)
            {
                height = 480;
                IsNewIcon = true;
                ShortCut = new Classes.VariousClasses.ShortCut();

                //set the window title
                this.Title = Localizer.GetLocalizedText("editshortcut-new");

                //if new item hide delete button
                button_delete.Visibility = Visibility.Hidden;
            }
            else
            {
                //set the window title
                this.Title = Localizer.GetLocalizedText("editshortcut-edit") + ShortCut.name;
            }

            //set the inital window dimensions
            this.Width = 340;
            this.Height = height;
            this.MinWidth = this.Width;
            this.MinHeight = this.Height;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.Background = (Brush)FindResource("White");

            //control label texts
            txt_label1.Text = Localizer.GetLocalizedText("editshortcut-name");
            txt_label2.Text = Localizer.GetLocalizedText("editshortcut-exec");
            txt_label3.Text = Localizer.GetLocalizedText("editshortcut-args");
            txt_label4.Text = Localizer.GetLocalizedText("editshortcut-icon");
            txt_label5.Text = Localizer.GetLocalizedText("editshortcut-colorbutton");
            txt_label6.Text = Localizer.GetLocalizedText("editshortcut-coloricon");
            txt_label7.Text = Localizer.GetLocalizedText("editshortcut-index");
            txt_label8.Text = Localizer.GetLocalizedText("editshortcut-group");

            //slider
            slider_groups.Minimum = Classes.Globals.AppSettings.MinRows;
            slider_groups.Maximum = Classes.Globals.AppSettings.Rows;

            //tooltips
            tt_Button_browse.Content = Localizer.GetLocalizedText("editshortcut-tooltip-exec");
            tt_Button_icon.Content = Localizer.GetLocalizedText("editshortcut-tooltip-icon");
            tt_Button_colorpicker1.Content = Localizer.GetLocalizedText("editshortcut-tooltip-color");
            tt_Button_colorpicker2.Content = Localizer.GetLocalizedText("editshortcut-tooltip-color");

            //tooltip icon
            button_browse.Content = Classes.IconController.GetButton(Classes.Enums.Icon.Browse, Classes.ResourceController.BrushGray, null, 0.5d);
            button_icon.Content = Classes.IconController.GetButton(Classes.Enums.Icon.About, Classes.ResourceController.BrushGray, null, 0.5d);
            button_colorpicker1.Content = Classes.IconController.GetButton(Classes.Enums.Icon.Colorpicker, Classes.ResourceController.BrushGray, null, 0.5d);
            button_colorpicker2.Content = Classes.IconController.GetButton(Classes.Enums.Icon.Colorpicker, Classes.ResourceController.BrushGray, null, 0.5d);

            //set the values
            textbox_name.Text = ShortCut.name;
            textbox_exec.Text = ShortCut.executable_path;
            textbox_args.Text = ShortCut.executable_arguments;
            textbox_color_icon.Text = ShortCut.color_icon;
            textbox_index.Text = ShortCut.index;
            slider_groups.Value = ShortCut.group_int;
            if (!IsNewIcon)
            {
                textbox_color_button.Text = ShortCut.color_button;
                textbox_icon.Text = ShortCut.icon;
            }

            //show stats on edit
            if (ShortCut.id > 0)
            {
                txt_dateadded1.Text = Localizer.GetLocalizedText("editshortcut-date-added");
                txt_dateused1.Text = Localizer.GetLocalizedText("editshortcut-date-used");
                txt_usages1.Text = Localizer.GetLocalizedText("editshortcut-usage");

                txt_dateadded2.Text = $"{Classes.Helpers.GetDateWithoutWeekday(ShortCut.date_added)}, {ShortCut.date_added.ToShortTimeString()}";
                txt_usages2.Text = ShortCut.clicks;

                //if the login has never been used
                if (ShortCut.date_used.Year > 2000)
                {
                    txt_dateused2.Text = $"{Classes.Helpers.GetDateWithoutWeekday(ShortCut.date_used)}, {ShortCut.date_used.ToShortTimeString()}";
                }
            }
            else
            {
                txt_dateadded1.Text = "";
                txt_dateused1.Text = "";
                txt_usages1.Text = "";

                this.Height = this.Height - 85;
            }

            //darkmode detected then change colors
            if (Classes.Globals.IsDarkMode())
            {
                this.Background = Classes.ResourceController.BrushDarkModeBackground;

                txt_label1.Foreground = Classes.ResourceController.BrushDarkModeText;
                txt_label2.Foreground = Classes.ResourceController.BrushDarkModeText;
                txt_label3.Foreground = Classes.ResourceController.BrushDarkModeText;
                txt_label4.Foreground = Classes.ResourceController.BrushDarkModeText;
                txt_label5.Foreground = Classes.ResourceController.BrushDarkModeText;
                txt_label6.Foreground = Classes.ResourceController.BrushDarkModeText;
                txt_label7.Foreground = Classes.ResourceController.BrushDarkModeText;
                txt_label8.Foreground = Classes.ResourceController.BrushDarkModeText;

                txt_dateadded1.Foreground = Classes.ResourceController.BrushDarkModeText;
                txt_dateused1.Foreground = Classes.ResourceController.BrushDarkModeText;
                txt_usages1.Foreground = Classes.ResourceController.BrushDarkModeText;
                txt_dateadded2.Foreground = Classes.ResourceController.BrushDarkModeText;
                txt_dateused2.Foreground = Classes.ResourceController.BrushDarkModeText;
                txt_usages2.Foreground = Classes.ResourceController.BrushDarkModeText;

                textbox_name.Style = (Style)FindResource("TextBox_Normal_DarkMode");
                textbox_exec.Style = (Style)FindResource("TextBox_Normal_DarkMode");
                textbox_args.Style = (Style)FindResource("TextBox_Normal_DarkMode");
                textbox_icon.Style = (Style)FindResource("TextBox_Normal_DarkMode");
                textbox_color_button.Style = (Style)FindResource("TextBox_Normal_DarkMode");
                textbox_color_icon.Style = (Style)FindResource("TextBox_Normal_DarkMode");
                textbox_index.Style = (Style)FindResource("TextBox_Normal_DarkMode");

                button_delete.Content = Classes.IconController.GetButton(Classes.Enums.Icon.Delete, Classes.ResourceController.BrushBlack, Localizer.GetLocalizedText("editshortcut-delete"));
                button_save.Content = Classes.IconController.GetButton(Classes.Enums.Icon.Save, Classes.ResourceController.BrushBlack, Localizer.GetLocalizedText("editshortcut-save"));
            }
            else
            {
                button_delete.Content = Classes.IconController.GetButton(Classes.Enums.Icon.Delete, Classes.ResourceController.BrushWhite, Localizer.GetLocalizedText("editshortcut-delete"));
                button_save.Content = Classes.IconController.GetButton(Classes.Enums.Icon.Save, Classes.ResourceController.BrushWhite, Localizer.GetLocalizedText("editshortcut-save"));
            }

            //trigger the slider change event for the inital colors
            if (IsNewIcon)
            {
                Sslider_groups_ValueChanged(null, null);
            }
        }


        /// <summary>
        /// Saves the settings and closes the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_save_Click(object sender, RoutedEventArgs e)
        {
            bool errors = false;
            string name = textbox_name.Text.Trim();
            string exec = textbox_exec.Text.Trim();
            string icon = textbox_icon.Text.Trim();


            //if textbox is empty or the length is incorrect then make red
            if (string.IsNullOrEmpty(name) || name.Length < 3)
            {
                textbox_name.Style = Classes.ResourceController.StyleTextBoxError;
                errors = true;
            }

            //if textbox is empty or the length is incorrect then make red
            if (string.IsNullOrEmpty(exec) || exec.Length < 5)
            {
                textbox_exec.Style = Classes.ResourceController.StyleTextBoxError;
                errors = true;
            }

            //if textbox is empty or the length is incorrect then make red
            if (string.IsNullOrEmpty(icon) || icon.Length < 5)
            {
                textbox_icon.Style = Classes.ResourceController.StyleTextBoxError;
                errors = true;
            }

            //if there is an error then quit
            if (errors)
            {
                return;
            }

            //set the values
            ShortCut.name = textbox_name.Text.Trim();
            ShortCut.executable_path = textbox_exec.Text.Trim();
            ShortCut.executable_arguments = textbox_args.Text.Trim();
            ShortCut.icon = textbox_icon.Text.Trim();
            ShortCut.color_button = textbox_color_button.Text.Trim();
            ShortCut.color_icon = textbox_color_icon.Text.Trim();
            ShortCut.index = textbox_index.Text.Trim();
            ShortCut.group = Convert.ToInt32(slider_groups.Value).ToString();

            //if new shortcut add it to the list
            if (ShortCut.id == 0)
            {
                ShortCut.date_added = DateTime.Now;

                //check if the list is empty, if not set the id as the current highest + 1
                if (Classes.Globals.AppSettings.ShortCuts.Count() > 0)
                {
                    ShortCut.id = Classes.Globals.AppSettings.ShortCuts.Max(x => x.id) + 1;
                }
                else
                {
                    ShortCut.id = 1;
                }

                Classes.Globals.AppSettings.ShortCuts.Add(ShortCut);
                Classes.Globals.AppSettings.Sort();
            }

            //save
            Classes.Globals.AppSettings.Save();

            //rebuild the shortcuts
            ((MainWindow)Application.Current.MainWindow).InitAppSpecific(true);

            this.DialogResult = true;
        }


        /// <summary>
        /// Show login delete confirmation. When yes removes it and saves.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_delete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(string.Format(Localizer.GetLocalizedText("editshortcut-delete-confirm"), ShortCut.name), Localizer.GetLocalizedText("editshortcut-delete-title"), MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                //remove the shortcut
                Classes.Globals.AppSettings.ShortCuts.RemoveAll(x => x.id == ShortCut.id);

                //save
                Classes.Globals.AppSettings.Save();

                //rebuild the shortcuts
                ((MainWindow)Application.Current.MainWindow).InitAppSpecific(true);

                this.DialogResult = true;
            }
        }


        /// <summary>
        /// Changes the textbox style when something is types from red to the normal color it the textbox was empty.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Textbox_index_TextChanged(object sender, TextChangedEventArgs e)
        {
            textbox_index.Text = Regex.Replace(textbox_index.Text, "[^0-9-]", "");
        }


        /// <summary>
        /// Open the openfiledialog to select a file from the disk.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_browse_Click(object sender, RoutedEventArgs e)
        {
            //show save file dialog
            var dialog = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = dialog.ShowDialog();

            //dialog ok then save
            if (result == true)
            {
                textbox_exec.Text = dialog.FileName;
            }
        }


        /// <summary>
        /// Open the color picker window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_colorpicker_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            string hexcolor = textbox_color_icon.Text;
            bool for_button_color = false;
            var color = Colors.Black;

            //is the picker for the button or icon
            if (btn.Name == "button_colorpicker1")
            {
                for_button_color = true;
                hexcolor = textbox_color_button.Text;
            }

            //try to convert the hex to color
            try
            {
                color = (Color)ColorConverter.ConvertFromString(hexcolor);
            }
            catch
            {
            }

            //open the color picker
            var picker = new PickColor(for_button_color, color)
            {
                Owner = this
            };

            picker.ShowDialog();
        }


        /// <summary>
        /// Opens the materialdesign website.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_icon_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://materialdesignicons.com");
        }


        /// <summary>
        /// Changes the textbox style when something is types from red to the normal color it the textbox was empty.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;

            if (Classes.Globals.IsDarkMode())
            {
                tb.Style = Classes.ResourceController.StyleTextBoxDarkMode;
            }
            else
            {
                tb.Style = Classes.ResourceController.StyleTextBoxNormal;
            }

            //make sure that if a user pastes an svg image with multiple rows, the svg is made single line. And to prevent just pressing enter in the textbox that must accept multiline
            tb.Text = tb.Text.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
        }


        /// <summary>
        /// On group change find the color of the first icon in that group and use if for the new icon.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sslider_groups_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //do not change the color of existing shortcuts
            if (!IsNewIcon)
            {
                return;
            }

            var old_shortcut = Classes.Globals.AppSettings.ShortCuts.Where(x => x.group_int == Convert.ToInt32(slider_groups.Value)).FirstOrDefault();

            //if no shortcut is found in the group use the default values, otherwise use the existing colors
            if (old_shortcut == null)
            {
                textbox_color_button.Text = ShortCut.color_button;
                textbox_color_icon.Text = ShortCut.color_icon;
            }
            else
            {
                textbox_color_button.Text = old_shortcut.color_button;
                textbox_color_icon.Text = old_shortcut.color_icon;
            }
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
                Close();
            }
        }
    }
}
