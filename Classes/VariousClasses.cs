using System;
using System.Windows.Media;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace QuickLauncher.Classes
{
    public class VariousClasses
    {
        public class ButtonIcon
        {
            public Enums.Icon icon { get; set; }
            public string path { get; set; }


            public System.Windows.Shapes.Path GetPath(Brush brush)
            {
                return new System.Windows.Shapes.Path()
                {
                    Fill = brush,
                    Data = Geometry.Parse(path)
                };
            }
        }


        public class LanguageEntry
        {
            public string key { get; set; }

            public string nl { get; set; }
            public string en { get; set; }
            public string de { get; set; }
        }


        public class ShortCut
        {
            public string name { get; set; }
            public string executable_path { get; set; }
            public string executable_arguments { get; set; }
            public string icon { get; set; }
            private string _color_button;
            public string color_button
            {
                get
                {
                    if (string.IsNullOrEmpty(_color_button))
                        return "#687FBA";

                    return "#" + _color_button.Replace("#", "").ToUpper();
                }
                set
                {
                    _color_button = value;
                }
            }
            private string _color_icon;
            public string color_icon
            {
                get
                {
                    if (string.IsNullOrEmpty(_color_icon))
                        return "#000000";

                    return "#" + _color_icon.Replace("#", "").ToUpper();
                }
                set
                {
                    _color_icon = value;
                }
            }

            //made these string so a typo in the settings file will not cause a read error when loading the settings
            public string group { get; set; }
            public string index { get; set; }
            public string clicks { get; set; }
            public DateTime date_added { get; set; }
            public DateTime date_used { get; set; }

            [XmlIgnoreAttribute]
            public int group_int
            {
                get
                {
                    int _number = int.TryParse(group, out _number) ? _number : 0;

                    if (_number < 1)
                        _number = 1;
                    else if (_number > Classes.Globals.AppSettings.Rows)
                        _number = Classes.Globals.AppSettings.Rows;

                    group = _number.ToString();
                    return _number;
                }
            }
            [XmlIgnoreAttribute]
            public int index_int
            {
                get
                {
                    int _number = int.TryParse(index, out _number) ? _number : 0;

                    index = _number.ToString();
                    return _number;
                }
            }
            [XmlIgnoreAttribute]
            public int clicks_int
            {
                get
                {
                    int _number = int.TryParse(clicks, out _number) ? _number : 0;

                    clicks = _number.ToString();
                    return _number;
                }
                set
                {
                    clicks = value.ToString();
                }
            }
            [XmlIgnoreAttribute]
            public int id { get; set; }

            public ShortCut()
            {
                //need to set these variables to their defaults because they are strings, otherwise they would be missing in the settings xml
                index = "0";
                group = "1";
                clicks = "0";
                executable_arguments = "";
                date_added = DateTime.Now;
            }
        }


        public class ComboboxSortorder
        {
            public string name { get; set; }
            public Enums.SortOrder sortorder { get; set; }
        }


        public class ValidateResult
        {
            public string message { get; set; }
            public List<int> shorcuts_with_error { get; set; }
        }
    }
}
