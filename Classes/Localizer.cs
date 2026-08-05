using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickLauncher
{
    public class Localizer
    {
        private static List<Classes.VariousClasses.LanguageEntry> LocalizedTexts { get; set; }


        /// <summary>
        /// Get the localized text based on a key.
        /// </summary>
        /// <param name="key">The key to identify the correct text.</param>
        /// <returns>The localized text.</returns>
        public static string GetLocalizedText(string key)
        {
            string localized_text = "";

            //create the localized text on first time
            if (LocalizedTexts == null)
            {
                CreateLanguageData();
            }

            //get the text by key
            var text = LocalizedTexts.Where(x => x.key == key?.ToLower()).FirstOrDefault();

            //text not found with the key
            if (text == null)
            {
                return "KEY-NOT-FOUND";
            }

            //get the correct language
            if (Classes.Globals.AppLanguage == "nl-NL" || Classes.Globals.AppLanguage == "nl-BE")
            {
                localized_text = text.nl;
            }
            else if (Classes.Globals.AppLanguage == "de-DE")
            {
                localized_text = text.de;
            }
            else
            {
                localized_text = text.en;
            }

            //check if the text was found and not empty
            if (string.IsNullOrEmpty(localized_text))
            {
                return "VALUE-NOT-FOUND";
            }
            else
            {
                return localized_text;
            }
        }


        /// <summary>
        /// Generare a list of localized texts.
        /// </summary>
        public static void CreateLanguageData()
        {
            LocalizedTexts = new List<Classes.VariousClasses.LanguageEntry>()
            {
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "mainwindow-error",
                    nl = "Kritieke fout!",
                    en = "Critical error!",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "mainwindow-closeapp",
                    nl = "Weet je zeker dat je {APPNAME} wilt afsluiten?",
                    en = "Are you sure you want to close {APPNAME}?",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "mainwindow-closing",
                    nl = "Afsluiten...",
                    en = "Closing...",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "mainwindow-close",
                    nl = "{APPNAME} afsluiten",
                    en = "Exit {APPNAME}",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "mainwindow-restore",
                    nl = "{APPNAME} openen",
                    en = "Restore {APPNAME}",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "mainwindow-minimize",
                    nl = "{APPNAME} minimaliseren",
                    en = "Minimize {APPNAME}",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "mainwindow-maximize",
                    nl = "{APPNAME} maximaliseren",
                    en = "Maximize {APPNAME}",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "mainwindow-about",
                    nl = "Over {APPNAME}",
                    en = "About {APPNAME}",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "mainwindow-ontop",
                    nl = "Bovenste venster aan/uit",
                    en = "Toggle always on top",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "about-version",
                    nl = "Versie",
                    en = "Version",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "about-ok",
                    nl = "OK",
                    en = "OK",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "mainwindow-cancel",
                    nl = "Annuleren",
                    en = "Cancel",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "app-readsettings-error",
                    nl = "Het laden van de instellingen is mislukt.",
                    en = "The settings failed to load.",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "app-error",
                    nl = $"{Classes.Globals.AppName} Fout",
                    en = $"{Classes.Globals.AppName} Error",
                    de = ""
                },

                //app specific

                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "settings-iconpack-error",
                    nl = $"Het aanmaken van de Icon Pack is mislukt.",
                    en = $"There was an error creating the Icon Pack.",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "settings-tooltip-settings",
                    nl = string.Format("Instellingen wijzigen van {0}.", Classes.Globals.AppName),
                    en = string.Format("Change the {0} settings.", Classes.Globals.AppName),
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "settings-tooltip-add",
                    nl = "Snelkoppeling toevoegen.",
                    en = "Add a shortcut.",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "settings-tooltip-edit",
                    nl = "Snelkoppelingen bewerken.",
                    en = "Edit shortcuts.",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "settings-title",
                    nl = "Instellingen",
                    en = "Settings",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "settings-sortorder",
                    nl = "Sorteervolgorde",
                    en = "Sort order",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "settings-separator",
                    nl = "Groep scheidingshoogte",
                    en = "Group separator height",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "settings-columns",
                    nl = "Aantal kolommone",
                    en = "Number of columns",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "settings-groups",
                    nl = "Aantal groepen",
                    en = "Number of groups",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "settings-size",
                    nl = "Button grootte",
                    en = "Button size",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "settings-sorting-mostused",
                    nl = "Meest gebruikt",
                    en = "Most used",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "settings-sorting-name",
                    nl = "Naam",
                    en = "Name",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "settings-save",
                    nl = "Opslaan",
                    en = "Save",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "settings-validate",
                    nl = "Check",
                    en = "Check",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "settings-checked-ok",
                    nl = "Alle snelkoppelingen zijn gecontroleerd. Er zijn geen problemen gevonden.",
                    en = "All shortcuts were checked. No broken shortcuts found.",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "settings-checked-error",
                    nl = "Er zijn {0} ontbrekende snelkoppelingen gevonden. Deze zijn rood gemarkeerd.",
                    en = "There are {0} missing shortcuts found. They are marked red.",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "settings-sorting-sortorder",
                    nl = "Index nummer",
                    en = "Index number",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "editshortcut-save",
                    nl = "Opslaan",
                    en = "Save",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "editshortcut-delete",
                    nl = "Verwijder",
                    en = "Delete",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "editshortcut-new",
                    nl = "Nieuwe snelkoppeling toevoegen",
                    en = "Add New Shortcut",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "editshortcut-edit",
                    nl = "Bewerk snelkoppeling: ",
                    en = "Edit Shortcut: ",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "editshortcut-name",
                    nl = "Naam",
                    en = "Name",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "editshortcut-group",
                    nl = "Groep",
                    en = "Group",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "editshortcut-exec",
                    nl = "Executable pad",
                    en = "Executable path",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "editshortcut-args",
                    nl = "Extra argumenten",
                    en = "Extra arguments",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "editshortcut-icon",
                    nl = "Icoon path",
                    en = "Icon path",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "editshortcut-colorbutton",
                    nl = "Knop kleur",
                    en = "Button color",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "editshortcut-coloricon",
                    nl = "Icoon kleur",
                    en = "Icon color",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "editshortcut-index",
                    nl = "Index nummer",
                    en = "Index number",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "editshortcut-delete-confirm",
                    nl = "Weet je zeker dat je snelkoppeling \"{0}\" wil verwijderden?",
                    en = "Are you sure you want to delete the shortcut \"{0}\"?",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "editshortcut-delete-title",
                    nl = "Snelkoppeling verwijderen?",
                    en = "Remove shortcut?",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "editshortcut-tooltip-exec",
                    nl = "Selecteer een bestand van de schijf.",
                    en = "Select a file from disk.",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "editshortcut-tooltip-icon",
                    nl = "Zoek een icoontje.",
                    en = "Search for an icon.",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "editshortcut-tooltip-color",
                    nl = "Open de kleurenkiezer.",
                    en = "Open the color picker.",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "colorpicker-title",
                    nl = "Kleurenkiezer",
                    en = "Color Picker",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "editshortcut-date-added",
                    nl = "Datum toegevoegd:",
                    en = "Date added:",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "editshortcut-date-used",
                    nl = "Datum laatst gebruikt:",
                    en = "Date last used:",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "editshortcut-usage",
                    nl = "Aantal keer gebruikt:",
                    en = "Numer of times used:",
                    de = ""
                },
                new Classes.VariousClasses.LanguageEntry()
                {
                    key = "icon-download-error",
                    nl = $"De Material Design Icons Pack kon niet gedownload worden.{Environment.NewLine}Het bijgeleverde icon pack zal gebruikt worden.",
                    en = $"The Material Design Icons Pack could not be downloaded.{Environment.NewLine}The included icon pack will be used.",
                    de = ""
                }
            };

            //replace tokens
            LocalizedTexts.ForEach(x => x.nl = x.nl.Replace("{APPNAME}", Classes.Globals.AppName));
            LocalizedTexts.ForEach(x => x.en = x.en.Replace("{APPNAME}", Classes.Globals.AppName));
            LocalizedTexts.ForEach(x => x.de = x.de.Replace("{APPNAME}", Classes.Globals.AppName));
        }


        public class LanguageEntry
        {
            public string key { get; set; }
            public string nl { get; set; }
            public string en { get; set; }
            public string de { get; set; }
        }
    }
}