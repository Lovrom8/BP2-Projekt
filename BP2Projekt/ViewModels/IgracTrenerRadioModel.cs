using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP2Projekt.ViewModels
{
    public class IgracTrenerRadioModel : BaseViewModel
    {
        private bool _jeIgrac = true;
        public bool JeIgrac
        {
            get => _jeIgrac;
            set
            {
                _jeIgrac = value;
                OnPropertyChanged("RadioIgrac");
            }
        }

        private bool _jeTrener = false;
        public bool JeTrener
        {
            get => _jeTrener;
            set
            {
                _jeTrener = value;
                OnPropertyChanged("RadioTrener");
            }
        }
    }
}
