using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BP2Projekt.Models 
{
    public class UlogaModel : INotifyPropertyChanged
    {
        private int ID_uloga;
        private int igra;
        private string naziv;

        public int ID_Uloga
        {
            get => ID_uloga;
            set
            {
                if (ID_uloga == value)
                    return;
                ID_uloga = value;

                NotifyPropertyChanged();
            }
        }

        public string Naziv
        {
            get => naziv;
            set
            {
                if (naziv == value)
                    return;
                naziv = value;

                NotifyPropertyChanged();
            }
        }

        public int Igra 
        {
            get => igra;
            set
            {
                if (igra == value)
                    return;
                igra = value;

                NotifyPropertyChanged();
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
