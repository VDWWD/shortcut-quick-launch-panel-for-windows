using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml.Serialization;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace QuickLauncher.Classes
{
    public class AppSetting
    {
        [XmlRoot(ElementName = "settings")]
        public class Settings
        {
            private bool settings_changed { get; set; }
            private int _rows { get; set; }
            private int _columns { get; set; }
            private int _buttonsize { get; set; }
            private int _rowspacing { get; set; }

            [XmlIgnoreAttribute]
            public int MinColumns
            {
                get
                {
                    return 5;
                }
            }
            [XmlIgnoreAttribute]
            public int MaxColumns
            {
                get
                {
                    return 32;
                }
            }
            public int MinRows
            {
                get
                {
                    return 1;
                }
            }
            [XmlIgnoreAttribute]
            public int MaxRows
            {
                get
                {
                    return 32;
                }
            }
            [XmlIgnoreAttribute]
            public int MinButtonSize
            {
                get
                {
                    return 18;
                }
            }
            [XmlIgnoreAttribute]
            public int MaxButtonSize
            {
                get
                {
                    return 256;
                }
            }
            [XmlIgnoreAttribute]
            public int MinGroupSeparatorSize
            {
                get
                {
                    return 5;
                }
            }
            [XmlIgnoreAttribute]
            public int MaxGroupSeparatorSize
            {
                get
                {
                    return 100;
                }
            }
            [XmlIgnoreAttribute]
            public string AppPathIconPackFile
            {
                get
                {
                    return string.Format(@"{0}\{1}.IconPack.json", Globals.AppPath, Globals.AppName);
                }
            }
            [XmlIgnoreAttribute]
            public string IconPackUrl
            {
                get
                {
                    return "https://github.com/Templarian/MaterialDesign/archive/refs/heads/master.zip";
                }
            }

            [XmlElement("always_on_top")]
            public bool AlwaysOnTop { get; set; }

            [XmlElement("sortorder")]
            public Enums.SortOrder SortOrder { get; set; }

            [XmlElement("rows")]
            public int Rows
            {
                get
                {
                    if (_rows < MinRows)
                        _rows = MinRows;
                    else if (_rows > MaxRows)
                        _rows = MaxRows;

                    return _rows;
                }
                set
                {
                    _rows = value;
                }
            }

            [XmlElement("columns")]
            public int Columns
            {
                get
                {
                    if (_columns < MinColumns)
                        _columns = MinColumns;
                    else if (_columns > MaxColumns)
                        _columns = MaxColumns;

                    return _columns;
                }
                set
                {
                    _columns = value;
                }
            }

            [XmlElement("button_size")]
            public int ButtonSize
            {
                get
                {
                    if (_buttonsize < MinButtonSize)
                        _buttonsize = MinButtonSize;
                    else if (_buttonsize > MaxButtonSize)
                        _buttonsize = MaxButtonSize;

                    return _buttonsize;
                }
                set
                {
                    _buttonsize = value;
                }
            }

            [XmlElement("row_spacing")]
            public int RowSpacing
            {
                get
                {
                    if (_rowspacing < MinGroupSeparatorSize)
                        _rowspacing = MinGroupSeparatorSize;
                    else if (_rowspacing > MaxGroupSeparatorSize)
                        _rowspacing = MaxGroupSeparatorSize;

                    return _rowspacing;
                }
                set
                {
                    _rowspacing = value;
                }
            }

            [XmlArray("shortcuts")]
            [XmlArrayItem("shortcut")]
            public List<VariousClasses.ShortCut> ShortCuts { get; set; }


            //constructor
            public Settings()
            {
                AlwaysOnTop = true;
                ShortCuts = new List<VariousClasses.ShortCut>();

                //default values
                _rows = 3;
                _columns = 6;
                _buttonsize = 36;
                _rowspacing = 20;
            }


            //load settings
            public Settings Load()
            {
                //check if there is a settings file
                if (File.Exists(Globals.AppPathSettingsFile))
                {
                    try
                    {
                        //load the settings xml and serialize
                        using (var stream = File.OpenRead(Globals.AppPathSettingsFile))
                        using (var reader = new StreamReader(stream))
                        {
                            string xml = reader.ReadToEnd();
                            var serializer = new XmlSerializer(typeof(Settings));

                            var rdr = new StringReader(xml);

                            //return the settings
                            return (Settings)serializer.Deserialize(rdr);
                        }
                    }
                    catch
                    {
                        //if incorrect xml then save again
                        Save();

                        MessageBox.Show(Localizer.GetLocalizedText("app-readsettings-error"), Localizer.GetLocalizedText("app-error"), MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    //create a new config file if none exists
                    Save();
                }

                return new Settings();
            }


            //save the settings
            public void SaveOnExit()
            {
                if (settings_changed)
                {
                    Save();
                }
            }
            public void Save()
            {
                try
                {
                    using (var writer = new StreamWriter(Globals.AppPathSettingsFile))
                    using (var sw = new StringWriter())
                    {
                        var serializer = new XmlSerializer(this.GetType());
                        serializer.Serialize(sw, this);

                        writer.Write(sw.ToString());
                        writer.Flush();
                    }
                }
                catch
                {
                    MessageBox.Show(Localizer.GetLocalizedText("app-writesettings-error"), Localizer.GetLocalizedText("app-error"), MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }


            //mark the settings as changed so they are saved on app exit
            public void SettingsChanged()
            {
                settings_changed = true;
            }


            //sort the icon list
            public void Sort()
            {
                if (SortOrder == Enums.SortOrder.SortIndex)
                {
                    ShortCuts = ShortCuts.OrderBy(x => x.index_int).ThenBy(x => x.name).ToList();
                }
                else if (SortOrder == Enums.SortOrder.MostUsed)
                {
                    ShortCuts = ShortCuts.OrderByDescending(x => x.clicks_int).ThenBy(x => x.name).ToList();
                }
                else
                {
                    ShortCuts = ShortCuts.OrderBy(x => x.name).ThenBy(x => x.index_int).ToList();
                }

                //make sure the shortcust atleast have a name and executable_path
                ShortCuts = ShortCuts.Where(x => !string.IsNullOrEmpty(x.executable_path) && !string.IsNullOrEmpty(x.name)).ToList();
            }


            //check if an icon is perhaps a svg or material icons and get the path
            public void CheckIconInShortCuts(List<MaterialDesignIcons.MaterialIcon> list)
            {
                bool changed = false;

                //if the list is null the try to load the iconpack and deserialize
                if (list == null)
                {
                    try
                    {
                        list = JSONSerializer<List<MaterialDesignIcons.MaterialIcon>>.DeSerialize(File.ReadAllText(AppPathIconPackFile));
                    }
                    catch
                    {
                    }
                }

                //loop all shortcuts
                foreach (var item in ShortCuts)
                {
                    if (string.IsNullOrEmpty(item.icon))
                    {
                        continue;
                    }

                    try
                    {
                        //check if the icon string is an svg file and try to get the path from it
                        if (item.icon.ToLower().Contains("svg") && item.icon.ToLower().Contains("d="))
                        {
                            try
                            {
                                item.icon = item.icon.Replace("'", "\"").Replace("\\", "").Split(new[] { "d=\"" }, StringSplitOptions.None)[1].Split('"').First();
                                changed = true;
                            }
                            catch
                            {
                                item.icon = "";
                            }
                        }

                        //check if the icon string is a material icon name and if so find the path
                        else if (!(item.icon.Contains(".") || item.icon.Contains(",") || item.icon.Contains(" ")) && list != null)
                        {
                            var materialicon = list.Where(x => x.name.ToLower() == item.icon.ToLower().Replace(".svg", "")).FirstOrDefault();

                            //icon found then store
                            if (materialicon != null)
                            {
                                item.icon = materialicon.path;
                                changed = true;
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                //if changed save again
                if (changed)
                {
                    Save();
                }
            }


            //for testing add random icons
            public void Test()
            {
                if (!Globals.IsTest || ShortCuts.Count() > 0 || !File.Exists(AppPathIconPackFile))
                {
                    return;
                }

                var rnd = new Random();
                var list = JSONSerializer<List<MaterialDesignIcons.MaterialIcon>>.DeSerialize(File.ReadAllText(AppPathIconPackFile));
                var colors_button = new List<string>()
                {
                    "#ff686e",
                    "#ff966e",
                    "#ffff6e",
                    "#6eff7c",
                    "#6e99ff"
                };

                for (int i = 1; i <= Rows; i++)
                {
                    for (int j = 0; j < rnd.Next(5, 18); j++)
                    {
                        //find a random icon
                        var icon = list[rnd.Next(0, list.Count)];

                        ShortCuts.Add(new VariousClasses.ShortCut()
                        {
                            id = i + j,
                            name = icon.name,
                            group = i.ToString(),
                            icon = icon.path,
                            executable_path = @"C:\Windows\system32\notepad.exe",
                            color_button = colors_button[rnd.Next(0, colors_button.Count)],
                            color_icon = j % 4 == 0 ? "#ffffff" : "#000000"
                        });
                    }
                }
            }


            //on app settings load
            public void OnAppSettingsLoad()
            {
                //does the icon pack exist, if not download
                if (!File.Exists(AppPathIconPackFile))
                {
                    MaterialDesignIcons.CreateIconList(this);
                }
                else
                {
                    //check if there are svg or material icons key names stored as icons and replace them with the path
                    CheckIconInShortCuts(null);
                }

                //sort the shortcuts
                Sort();

                //add an index to the shortcuts
                int count = 1;
                ShortCuts.ForEach(x => x.id = count++);
            }
        }


        /// <summary>
        /// Check all the icon if the shortcut is still correct. If not make red.
        /// </summary>
        /// <returns>ValidateResult class with message and list of icons with errors.</returns>
        public static async Task<VariousClasses.ValidateResult> ValidateShortcuts()
        {
            var result = new VariousClasses.ValidateResult()
            {
                shorcuts_with_error = new List<int>()
            };

            //loop all the shortcuts
            foreach (var item in Globals.AppSettings.ShortCuts)
            {
                if (Helpers.IsValidUrl(item.executable_path))
                {
                    try
                    {
                        //check if the url is correct
                        using (var client = new HttpClient())
                        using (var request = new HttpRequestMessage(HttpMethod.Head, item.executable_path))
                        {
                            try
                            {
                                var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                                if (!response.IsSuccessStatusCode)
                                {
                                    result.shorcuts_with_error.Add(item.id);
                                }
                            }
                            catch
                            {
                                result.shorcuts_with_error.Add(item.id);
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                //check if the executable exits
                else if (!Helpers.IsValidEmail(item.executable_path) && !File.Exists(item.executable_path))
                {
                    result.shorcuts_with_error.Add(item.id);
                }
            }

            //return the correct error message
            if (result.shorcuts_with_error.Count() == 0)
            {
                result.message = Localizer.GetLocalizedText("settings-checked-ok");
            }
            else
            {
                result.message = string.Format(Localizer.GetLocalizedText("settings-checked-error"), result.shorcuts_with_error.Count());
            }

            return result;
        }
    }
}
