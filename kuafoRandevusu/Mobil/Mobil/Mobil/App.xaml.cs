using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mobil.Pages;

[assembly: ExportFont("Magneto.ttf", Alias = "Magneto", EmbeddedFontResourceId ="Magneto")]

namespace Mobil
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage (new SInPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
