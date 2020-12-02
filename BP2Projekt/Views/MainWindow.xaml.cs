using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using BP2Projekt.ViewModels;
using BP2Projekt.Util;
using MaterialDesignExtensions.Controls;

namespace BP2Projekt
{
    // IKONA: https://www.flaticon.com/free-icon/sport-and-competion_3565531?term=esports&page=1&position=8
    public partial class MainWindow : MaterialWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new GlavniViewModel();

            ProzorManager.Registriraj<WindowSudionik>("ProzorSudionici");
            ProzorManager.Registriraj<WindowOrganizator>("ProzorOrganizator");
            ProzorManager.Registriraj<WindowTim>("ProzorTim");
            ProzorManager.Registriraj<WindowOrganizacija>("ProzorOrganizacija");
            ProzorManager.Registriraj<WindowOrganizator>("ProzorOrganizator");
        }
    }
}
