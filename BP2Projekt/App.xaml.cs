using BP2Projekt.ViewModels;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using System.Windows;

namespace BP2Projekt
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainWindow, GlavniViewModel>();
           
            containerRegistry.RegisterDialog<WindowOrganizacija, OrganizacijaViewModel>("OrganizacijaProzor");
            containerRegistry.RegisterDialog<WindowIgra, IgraViewModel>("IgraProzor");
            containerRegistry.RegisterDialog<WindowLiga, LigaViewModel>("LigaProzor");
            containerRegistry.RegisterDialog<WindowMec, MecViewModel>("MecProzor");
            containerRegistry.RegisterDialog<WindowOrganizator, OrganizatorViewModel>("OrganizatorProzor");
            containerRegistry.RegisterDialog<WindowProizvodac, ProizvodacViewModel>("ProizvodacProzor");
            containerRegistry.RegisterDialog<WindowSudionik, SudionikViewModel>("SudionikProzor");
            containerRegistry.RegisterDialog<WindowTim, TimViewModel>("TimProzor");

        }
    }
}
