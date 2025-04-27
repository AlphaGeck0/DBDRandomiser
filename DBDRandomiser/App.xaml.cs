using System.Configuration;
using System.Data;
using System.Windows;
using System;
using System.IO;

namespace DBDRandomiser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                File.WriteAllText("error.log", e.ExceptionObject.ToString());
            };

            this.DispatcherUnhandledException += (sender, e) =>
            {
                File.WriteAllText("error.log", e.Exception.ToString());
                e.Handled = true;
            };
        }
    }

}
