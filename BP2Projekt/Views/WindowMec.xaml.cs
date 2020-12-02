using BP2Projekt.ViewModels;
using MaterialDesignExtensions.Controls;
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
using System.Windows.Shapes;

namespace BP2Projekt
{
    public partial class WindowMec : MaterialWindow
    {
        public WindowMec()
        {
            InitializeComponent();

            int mecID = 2;
            var _mvm = new MecViewModel(mecID);
            DataContext = _mvm;
        }
    }
}
