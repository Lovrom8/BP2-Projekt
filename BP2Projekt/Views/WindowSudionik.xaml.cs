using BP2Projekt.ViewModels;
using MaterialDesignExtensions.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace BP2Projekt
{
    public partial class WindowSudionik : MaterialWindow
    {
        public WindowSudionik()
        {
            InitializeComponent();

            var _svm = new SudionikViewModel("Niko");
            DataContext = _svm;
            
        }
    }
}
