using System;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using System.Windows.Media;
using System.Windows.Controls;

namespace QuickLauncher
{
    public partial class Settings : MetroWindow
    {
        /// <summary>
        /// On settings window open.
        /// </summary>
        public Settings()
        {
            InitializeComponent();

            //set the window title
            this.Title = Localizer.GetLocalizedText("settings-title");

            //set the inital window dimensions
            this.Width = 340;
            this.Height = 375;
            this.MinWidth = this.Width;
            this.MinHeight = this.Height;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.Background = (Brush)FindResource("White");

            //control label texts
            txt_label1.Text = Localizer.GetLocalizedText("settings-sortorder");
            txt_label2.Text = Localizer.GetLocalizedText("settings-columns");
            txt_label3.Text = Localizer.GetLocalizedText("settings-groups");
            txt_label4.Text = Localizer.GetLocalizedText("settings-separator");
            txt_label5.Text = Localizer.GetLocalizedText("settings-size");

            //slider parameters
            slider_columns.Minimum = Classes.Globals.AppSettings.MinColumns;
            slider_columns.Maximum = Classes.Globals.AppSettings.MaxColumns;
            slider_groups.Minimum = Classes.Globals.AppSettings.MinRows;
            slider_groups.Maximum = Classes.Globals.AppSettings.MaxRows;
            slider_size.Minimum = Classes.Globals.AppSettings.MinButtonSize;
            slider_size.Maximum = Classes.Globals.AppSettings.MaxButtonSize;
            slider_separator.Minimum = Classes.Globals.AppSettings.MinGroupSeparatorSize;
            slider_separator.Maximum = Classes.Globals.AppSettings.MaxGroupSeparatorSize;

            //combobox values
            combobox_sortorder.Items.Add(new Classes.VariousClasses.ComboboxSortorder()
            {
                sortorder = Classes.Enums.SortOrder.Name,
                name = Localizer.GetLocalizedText("settings-sorting-name")
            });
            combobox_sortorder.Items.Add(new Classes.VariousClasses.ComboboxSortorder()
            {
                sortorder = Classes.Enums.SortOrder.MostUsed,
                name = Localizer.GetLocalizedText("settings-sorting-mostused")
            });
            combobox_sortorder.Items.Add(new Classes.VariousClasses.ComboboxSortorder()
            {
                sortorder = Classes.Enums.SortOrder.SortIndex,
                name = Localizer.GetLocalizedText("settings-sorting-sortorder")
            });

            //set the values
            combobox_sortorder.SelectedValue = Classes.Globals.AppSettings.SortOrder;
            slider_columns.Value = Classes.Globals.AppSettings.Columns;
            slider_groups.Value = Classes.Globals.AppSettings.Rows;
            slider_size.Value = Classes.Globals.AppSettings.ButtonSize;
            slider_separator.Value = Classes.Globals.AppSettings.RowSpacing;

            //darkmode detected then change colors
            if (Classes.Globals.IsDarkMode())
            {
                this.Background = Classes.ResourceController.BrushDarkModeBackground;

                combobox_sortorder.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#acb6d2");

                txt_label1.Foreground = Classes.ResourceController.BrushDarkModeText;
                txt_label2.Foreground = Classes.ResourceController.BrushDarkModeText;
                txt_label3.Foreground = Classes.ResourceController.BrushDarkModeText;
                txt_label4.Foreground = Classes.ResourceController.BrushDarkModeText;
                txt_label5.Foreground = Classes.ResourceController.BrushDarkModeText;

                button_save.Content = Classes.IconController.GetButton(Classes.Enums.Icon.Save, Classes.ResourceController.BrushBlack, Localizer.GetLocalizedText("settings-save"));
                button_validate.Content = Classes.IconController.GetButton(Classes.Enums.Icon.Validate, Classes.ResourceController.BrushBlack, Localizer.GetLocalizedText("settings-validate"));
            }
            else
            {
                button_save.Content = Classes.IconController.GetButton(Classes.Enums.Icon.Save, Classes.ResourceController.BrushWhite, Localizer.GetLocalizedText("settings-save"));
                button_validate.Content = Classes.IconController.GetButton(Classes.Enums.Icon.Validate, Classes.ResourceController.BrushWhite, Localizer.GetLocalizedText("settings-validate"));
            }
        }


        /// <summary>
        /// Clears the combobox focus to set the border to the default style again.
        /// </summary>
        private void ClearComboBoxFocus()
        {
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(combobox_sortorder), null);
            Keyboard.ClearFocus();
        }


        /// <summary>
        /// Saves the settings and closes the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_save_Click(object sender, RoutedEventArgs e)
        {
            Classes.Globals.AppSettings.Columns = Convert.ToInt32(slider_columns.Value);
            Classes.Globals.AppSettings.Rows = Convert.ToInt32(slider_groups.Value);
            Classes.Globals.AppSettings.ButtonSize = Convert.ToInt32(slider_size.Value);
            Classes.Globals.AppSettings.RowSpacing = Convert.ToInt32(slider_separator.Value);
            Classes.Globals.AppSettings.SortOrder = ((Classes.VariousClasses.ComboboxSortorder)combobox_sortorder.SelectedItem).sortorder;

            //save and sort
            Classes.Globals.AppSettings.Save();
            Classes.Globals.AppSettings.Sort();

            //rebuild the shortcuts
            ((MainWindow)Application.Current.MainWindow).InitAppSpecific(true);

            this.DialogResult = true;
        }


        /// <summary>
        /// Combobox selection changed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Combobox_sortorder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearComboBoxFocus();
        }


        /// <summary>
        /// Window left mouse up event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ClearComboBoxFocus();
        }


        /// <summary>
        /// Check all the icon if the shortcut is still correct. If not make red.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_validate_Click(object sender, RoutedEventArgs e)
        {
            var result = await Classes.AppSetting.ValidateShortcuts();

            //paint the incorrect icons red
            foreach (var item in result.shorcuts_with_error)
            {
                //item.color_button = ResourceController.BrushRed.Color.ToString();
                var icon = ((MainWindow)Application.Current.MainWindow).grid_main.FindChild<Button>("button_" + item);

                icon.Background = Classes.ResourceController.BrushRed;
            }

            //show result
            MessageBox.Show(result.message);
        }


        /// <summary>
        /// Handles the keypresses in the window to close it on escape or enter.
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
