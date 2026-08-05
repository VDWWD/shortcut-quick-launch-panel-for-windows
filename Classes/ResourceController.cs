using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QuickLauncher.Classes
{
    public class ResourceController
    {
        /// <summary>
        /// Get an image from the embedded resources.
        /// </summary>
        /// <param name="image">The name of the image.</param>
        /// <returns>BitmapImage</returns>
        public static BitmapImage GetImageFromResource(string image)
        {
            var bitmap = new BitmapImage();

            //add image (NOTE > image properties in resources must be set to 'embedded resource')
            using (var stream = GetStreamFromResource(image))
            {
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.EndInit();
            }

            return bitmap;
        }


        /// <summary>
        /// Stream a file from the embedded resources.
        /// </summary>
        /// <param name="file">The name of the file.</param>
        /// <returns>Stream</returns>
        public static Stream GetStreamFromResource(string file)
        {
            var assembly = Assembly.GetExecutingAssembly();

            return assembly.GetManifestResourceStream(string.Format("{0}.Resources.{1}", assembly.GetName().Name, file));
        }


        /// <summary>
        /// Create a ContextMenu Item.
        /// </summary>
        /// <param name="icon">The icon for the menu item.</param>
        /// <param name="text">The text for the menu item.</param>
        /// <param name="handler">The RoutedEventHandler for the menu item.</param>
        /// <returns>MenuItem</returns>
        public static MenuItem CreateContextMenuItem(Enums.Icon icon, string text, RoutedEventHandler handler)
        {
            //get the icon from the list
            var iconpath = IconController.GetIcon(icon, BrushWhite);

            var canvas = new Canvas()
            {
                Width = 24,
                Height = 24
            };

            canvas.Children.Add(iconpath);

            var item = new MenuItem()
            {
                Header = text,
                Icon = new Viewbox()
                {
                    Width = 22,
                    Height = 22,
                    Margin = new Thickness(0, 1, 0, 0),
                    Child = canvas
                }
            };

            //add the click handler
            item.Click += handler;

            return item;
        }


        public static Style StyleTextBoxNormal
        {
            get
            {
                return (Style)Application.Current.FindResource("TextBox_Normal");
            }
        }
        public static Style StyleTextBoxError
        {
            get
            {
                return (Style)Application.Current.FindResource("TextBox_Error");
            }
        }
        public static Style StyleTextBoxDarkMode
        {
            get
            {
                return (Style)Application.Current.FindResource("TextBox_Normal_DarkMode");
            }
        }

        public static Brush BrushWhite
        {
            get
            {
                return (Brush)Application.Current.FindResource("White");
            }
        }
        public static Brush BrushBlack
        {
            get
            {
                return (Brush)Application.Current.FindResource("Black");
            }
        }
        public static Brush BrushDarkModeText
        {
            get
            {
                return (Brush)Application.Current.FindResource("DarkMode_Text");
            }
        }
        public static Brush BrushDarkModeBackground
        {
            get
            {
                return (Brush)Application.Current.FindResource("DarkMode_Background");
            }
        }
        public static Brush BrushGray
        {
            get
            {
                return (Brush)Application.Current.FindResource("TekstKleur");
            }
        }
        public static SolidColorBrush BrushDefaultButton
        {
            get
            {
                return (SolidColorBrush)new BrushConverter().ConvertFromString("#687fba");
            }
        }
        public static SolidColorBrush BrushRed
        {
            get
            {
                return (SolidColorBrush)new BrushConverter().ConvertFromString("#bf1616");
            }
        }
    }
}
