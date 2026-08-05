using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using ControlzEx;
using System.Windows.Media;
using System.Collections.Generic;
using System.Diagnostics;

namespace QuickLauncher
{
    public partial class MainWindow
    {
        private int NextRow;
        private bool IsEditMode;


        /// <summary>
        /// App specific main window actions.
        /// </summary>
        /// <param name="from_settings_save">If the app is reloaded from the settings window or not.</param>
        public void InitAppSpecific(bool from_settings_save)
        {
            //declare some variables
            int dimensions = Classes.Globals.AppSettings.ButtonSize;
            int row_spacing = Classes.Globals.AppSettings.RowSpacing;
            int grid_margins = 10;
            int separator_width = 10;
            int header_height = 32;
            int button_row_height = 35;
            int min_height = (dimensions * 2) + row_spacing + (grid_margins * 2) + header_height;

            //on settings loaded do some stuff
            Classes.Globals.AppSettings.OnAppSettingsLoad();

            //create the bottom settings buttons
            button_settings.Content = Classes.IconController.GetButton(Classes.Enums.Icon.Settings, Classes.ResourceController.BrushGray, null, 0.5d);
            button_add.Content = Classes.IconController.GetButton(Classes.Enums.Icon.Add, Classes.ResourceController.BrushGray, null, 0.5d);
            button_edit.Content = Classes.IconController.GetButton(Classes.Enums.Icon.Edit, Classes.ResourceController.BrushGray, null, 0.5d);

            tt_Button_settings.Content = Localizer.GetLocalizedText("settings-tooltip-settings");
            tt_Button_add.Content = Localizer.GetLocalizedText("settings-tooltip-add");
            tt_Button_edit.Content = Localizer.GetLocalizedText("settings-tooltip-edit");

            //add button clicks
            button_settings.Click += Button_settings_Click;
            button_add.Click += Button_add_Click;
            button_edit.Click += Button_edit_Click;

            //clear the grid if the settings were saved
            if (from_settings_save)
            {
                grid_main.Children.Clear();

                //reset the rows and columns
                if (grid_main.ColumnDefinitions.Count() > 0)
                {
                    grid_main.ColumnDefinitions.RemoveRange(0, grid_main.ColumnDefinitions.Count());
                }
                if (grid_main.RowDefinitions.Count() > 0)
                {
                    grid_main.RowDefinitions.RemoveRange(0, grid_main.RowDefinitions.Count());
                }

                NextRow = 0;
            }

            //create the grid columns
            for (int i = 0; i < Classes.Globals.AppSettings.Columns; i++)
            {
                if (i > 0)
                {
                    grid_main.ColumnDefinitions.Add(new ColumnDefinition()
                    {
                        Width = new GridLength(separator_width)
                    });
                }

                grid_main.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(dimensions)
                });
            }

            //calculate the total number of rows
            int rows = 0;
            int height = 0;

            //create the grid rows
            for (int i = 0; i < Classes.Globals.AppSettings.Rows; i++)
            {
                //shortcuts per group
                var shortcuts = Classes.Globals.AppSettings.ShortCuts.Where(x => x.group_int == i + 1).ToList();

                //no items in group then skip
                if (shortcuts.Count() == 0)
                {
                    continue;
                }

                //how many rows are needed for this group
                var rows_per_group = Math.Ceiling((decimal)shortcuts.Count() / Classes.Globals.AppSettings.Columns);

                for (int j = 0; j < rows_per_group; j++)
                {
                    //row separator
                    if (j > 0)
                    {
                        grid_main.RowDefinitions.Add(new RowDefinition()
                        {
                            Height = new GridLength(separator_width)
                        });

                        height += separator_width;
                    }

                    //normal row
                    grid_main.RowDefinitions.Add(new RowDefinition()
                    {
                        Height = new GridLength(dimensions)
                    });

                    rows++;
                    height += dimensions;
                }

                //group separator row
                if (Classes.Globals.AppSettings.Rows > 1 && i + 1 < Classes.Globals.AppSettings.Rows)
                {
                    grid_main.RowDefinitions.Add(new RowDefinition()
                    {
                        Height = new GridLength(row_spacing)
                    });

                    height += row_spacing;
                }

                //create the shortcuts
                CreateShortCuts((int)rows_per_group, shortcuts);
            }

            //width of the app. The 2 px extra are needed because of the outer border
            this.Width = (Classes.Globals.AppSettings.Columns * (dimensions + separator_width) - separator_width) + (grid_margins * 2) + 2;
            this.MinWidth = this.Width;

            //height of the app
            height = height + (grid_margins * 2) + header_height + button_row_height;

            if (height < min_height)
            {
                height = min_height;
            }

            this.Height = height;
            this.MinHeight = height;

            //hide edit button if there are no shortcuts
            if (Classes.Globals.AppSettings.ShortCuts.Count() == 0)
            {
                button_edit.Visibility = Visibility.Hidden;
            }
            else
            {
                button_edit.Visibility = Visibility.Visible;
            }
        }


        /// <summary>
        /// Creates all the shortcut buttons in the grid.
        /// </summary>
        /// <param name="rows_per_group">The total number of rows needed for the group.</param>
        /// <param name="shortcuts">List of shortcuts of a group.</param>
        public void CreateShortCuts(int rows_per_group, List<Classes.VariousClasses.ShortCut> shortcuts)
        {
            //variables
            int row = NextRow;
            int col = 0;
            var icon_height = Math.Ceiling(Classes.Globals.AppSettings.ButtonSize * 0.666);
            var icon_question = Geometry.Parse(Classes.IconController.GetIcon(Classes.Enums.Icon.Questionmark).path);
            var icon_shortcut = icon_question;
            var color_icon = Classes.ResourceController.BrushBlack;
            var color_button = Classes.ResourceController.BrushDefaultButton;

            //some styles
            var style_tooltip = (Style)this.FindResource("ToolTip_ShortCut");
            var style_button = (Style)this.FindResource("Button_ShortCut");

            //the next group row
            NextRow += rows_per_group + 1;

            for (int i = 0; i < shortcuts.Count(); i++)
            {
                var item = shortcuts[i];

                //try to get the hex background color for the button
                if (!string.IsNullOrEmpty(item.color_button))
                {
                    try
                    {
                        color_button = (SolidColorBrush)new BrushConverter().ConvertFromString(item.color_button);
                    }
                    catch
                    {
                        //make button red if incorrect color
                        color_button = Classes.ResourceController.BrushRed;
                    }
                }

                //try to get the hex background color for the button
                if (!string.IsNullOrEmpty(item.color_icon))
                {
                    try
                    {
                        color_icon = (SolidColorBrush)new BrushConverter().ConvertFromString(item.color_icon);
                    }
                    catch
                    {
                    }
                }

                //try to get the icon path
                if (!string.IsNullOrEmpty(item.icon))
                {
                    try
                    {
                        icon_shortcut = Geometry.Parse(item.icon);
                    }
                    catch
                    {
                        icon_shortcut = icon_question;
                    }
                }

                //go to the next row if the max per row is reached
                if (col > Classes.Globals.AppSettings.Columns - 1)
                {
                    row = row + 2;
                    col = 0;

                    //if the group needs more rows then increment
                    NextRow++;
                }

                //create the tooltip
                var tooltip = new ToolTip()
                {
                    Content = item.name,
                    Style = style_tooltip,
                    Background = color_button,
                    BorderBrush = Classes.ResourceController.BrushBlack,
                    Foreground = color_icon
                };

                ToolTipAssist.SetAutoMove(tooltip, true);

                //create the icon
                var path = new System.Windows.Shapes.Path()
                {
                    Fill = color_icon,
                    Data = icon_shortcut
                };

                var viewBox = new Viewbox()
                {
                    Height = icon_height,
                    Width = icon_height
                };

                var canvas = new Canvas()
                {
                    Width = 24,
                    Height = 24
                };

                canvas.Children.Add(path);
                viewBox.Child = canvas;

                //create the button
                var button = new Button()
                {
                    Focusable = false,
                    ToolTip = tooltip,
                    Background = color_button,
                    Name = "button_" + item.id, //a name cannot be just a number, so a prefix is needed
                    Style = style_button
                };

                button.Content = viewBox;

                //add the click handler
                button.Click += Button_grid_Click;

                //add the button to the grid
                Grid.SetRow(button, row);
                Grid.SetColumn(button, col * 2);
                grid_main.Children.Add(button);

                col++;
            }
        }


        #region handlers

        /// <summary>
        /// Opens the app settings window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_settings_Click(object sender, EventArgs e)
        {
            var settings = new Settings()
            {
                Owner = Application.Current.MainWindow
            };

            settings.ShowDialog();
        }


        /// <summary>
        /// Opens the add new shortcut window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_add_Click(object sender, RoutedEventArgs e)
        {
            var editwindow = new EditItem(null)
            {
                Owner = Application.Current.MainWindow
            };

            editwindow.ShowDialog();
        }


        /// <summary>
        /// Sets the window in edit mode so when a shortcut is clicked the edit window pops up.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_edit_Click(object sender, RoutedEventArgs e)
        {
            if (IsEditMode)
            {
                button_edit.Content = Classes.IconController.GetButton(Classes.Enums.Icon.Edit, Classes.ResourceController.BrushGray, null, 0.5d);
            }
            else
            {
                //change the color of the edit button to indicate edit mode
                var brush = Classes.ResourceController.BrushBlack;
                if (Classes.Globals.IsDarkMode())
                {
                    brush = Classes.ResourceController.BrushWhite;
                }

                button_edit.Content = Classes.IconController.GetButton(Classes.Enums.Icon.Edit, Classes.ResourceController.BrushRed, null);
            }

            IsEditMode = !IsEditMode;
        }


        /// <summary>
        /// Handles the button clicks from the shortcuts in the grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_grid_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            var shortcut = Classes.Globals.AppSettings.ShortCuts.Where(x => x.id == Convert.ToInt32(button.Name.Replace("button_", ""))).FirstOrDefault();

            //is edit mode
            if (IsEditMode)
            {
                Button_edit_Click(null, null);

                //open the edit window
                var editwindow = new EditItem(shortcut)
                {
                    Owner = Application.Current.MainWindow
                };

                editwindow.ShowDialog();

                return;
            }

            string path = shortcut.executable_path;

            //check if email
            if (Classes.Helpers.IsValidEmail(path))
            {
                path = "mailto:" + path;
            }

            try
            {
                //start the external program
                Process.Start(path, string.IsNullOrEmpty(shortcut.executable_arguments) ? null : shortcut.executable_arguments);

                //update settings is a shortcut is clicked so the clicks and date are saved on app exit
                Classes.Globals.AppSettings.SettingsChanged();

                //update some stats
                shortcut.clicks_int++;
                shortcut.date_used = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\r\nPath: {shortcut.executable_path}", Localizer.GetLocalizedText("mainwindow-error"), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        #endregion
    }
}
