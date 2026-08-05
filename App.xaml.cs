using System;
using System.Collections.Generic;
using Microsoft.Shell;
using System.Windows;
using System.Reflection;

namespace QuickLauncher
{
    public partial class App : Application, ISingleInstanceApp
    {
        //to create a single instance app: https://codereview.stackexchange.com/questions/20871/single-instance-wpf-application
        //Step 1: Add the file SingleInstance.cs to your project.
        //Step 2: Add a reference to your project: System.Runtime.Remoting
        //Step 3: Have your application class implement ISingleInstanceApp (defined in SingleInstance.cs).
        //Step 4: Define your own Main function that uses the single instance class (see below).
        //Step 5: Set new main entry point > Select Project Properties –> Application and set "Startup object" to your App class name instead of "(Not Set)".
        //Step 6: Cancel the default WPF main function > Right-click on App.xaml, Properties, set Build Action to "Page" instead of "Application Definition".

        private const string UniqueKey = "e80428df-6197-424f-8d7b-b66875d1a609";


        [STAThread]
        static void Main()
        {
            //if there is already an instance running then quit
            if (!SingleInstance<App>.InitializeAsFirstInstance(UniqueKey))
            {
                return;
            }

            //check for test mode, loading from embedded resources does not work when starting app in visual studio because it will throw an exception
            if (!Classes.Globals.IsTest)
            {
                AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                {
                    using (var stream = Classes.ResourceController.GetStreamFromResource(new AssemblyName(args.Name).Name + ".dll"))
                    {
                        var assemblyData = new byte[stream.Length];
                        stream.Read(assemblyData, 0, assemblyData.Length);

                        return Assembly.Load(assemblyData);
                    }
                };
            }

            var application = new App();
            application.InitializeComponent();
            application.Run();

            SingleInstance<App>.Cleanup();
        }


        /// <summary>
        /// This ensures that the current app is opened if the executable is clicked again.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>bool</returns>
        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            if (this.MainWindow.WindowState == WindowState.Minimized)
            {
                this.MainWindow.Show();
                this.MainWindow.WindowState = WindowState.Normal;
            }

            if (this.MainWindow.Topmost)
            {
                this.MainWindow.Activate();
            }
            else
            {
                this.MainWindow.Topmost = true;
                this.MainWindow.Activate();
                this.MainWindow.Topmost = false;
            }

            return true;
        }
    }
}
