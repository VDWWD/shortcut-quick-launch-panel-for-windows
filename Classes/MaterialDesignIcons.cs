using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Windows;

namespace QuickLauncher.Classes
{
    public class MaterialDesignIcons
    {
        /// <summary>
        /// Download the materialdesignicons master.zip from github.com. Then read the zip to get all the icon names and their path and store them on the disk in json format.
        /// This is the only time the app will try to connect to the internet. Delete the json to trigger the download again.
        /// </summary>
        /// <param name="settings">The app settings class.</param>
        public async static void CreateIconList(AppSetting.Settings settings)
        {
            var list = new List<MaterialIcon>();

            try
            {
                using (var client = new HttpClient())
                {
                    //download the material icons from github
                    var data = await client.GetByteArrayAsync(settings.IconPackUrl);

                    //read the downloaded zip
                    using (var zip = new ZipArchive(new MemoryStream(data), ZipArchiveMode.Read))
                    {
                        //get all the svg's from the correct directory. skip 1 is needed to exclude the folder name itself
                        foreach (var entry in zip.Entries.Where(x => x.FullName.Contains("MaterialDesign-master/svg/")).Skip(1))
                        {
                            //the filename without extension
                            string name = Path.GetFileNameWithoutExtension(entry.FullName).ToLower();

                            using (var reader = new StreamReader(entry.Open()))
                            {
                                string svg = reader.ReadToEnd();

                                //try to get the path from the svg
                                var path = svg.Replace("'", "\"").Split(new[] { "path d=\"" }, StringSplitOptions.None).Last().Split('"').First();

                                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(path))
                                {
                                    continue;
                                }

                                //add the icon to the list
                                list.Add(new MaterialIcon()
                                {
                                    name = name,
                                    path = path
                                });
                            }
                        }
                    }
                }
            }
            catch
            {
                //unpack the included icon pack
                UnpackDefaultIconPack();

                MessageBox.Show(Localizer.GetLocalizedText("icon-download-error"), Localizer.GetLocalizedText("app-error"), MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            //if the list is empty then do not save
            if (list.Count() == 0)
            {
                return;
            }

            //serialize the list
            string json = JSONSerializer<List<MaterialIcon>>.Serialize(list);

            //write the file to disk
            File.WriteAllText(Globals.AppSettings.AppPathIconPackFile, json);

            //check for material icons in the shortcuts list
            settings.CheckIconInShortCuts(list);
        }


        /// <summary>
        /// Reads the included icon pack json in the embedded zipfile and writes it to disk if the normal download failed.
        /// </summary>
        private static void UnpackDefaultIconPack()
        {
            //get the zipfile from the embedded resources
            using (var zip = new ZipArchive(ResourceController.GetStreamFromResource("QuickLauncher.IconPack.Default.zip"), ZipArchiveMode.Read))
            {
                using (var reader = new StreamReader(zip.Entries[0].Open()))
                {
                    //write the file to disk
                    File.WriteAllText(Globals.AppSettings.AppPathIconPackFile, reader.ReadToEnd());
                }
            }
        }


        [DataContract]
        public class MaterialIcon
        {
            [DataMember]
            public string name { get; set; }
            [DataMember]
            public string path { get; set; }
        }
    }
}
