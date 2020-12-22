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
        private int idUloga;
        private int idIgra;
        private string igraNaziv;
        private string naziv;

        public int ID_Uloga
        {
            get => idUloga;
            set
            {
                if (idUloga == value)
                    return;
                idUloga = value;

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

        public int ID_Igra
        {
            get => idIgra;
            set
            {
                if (idIgra == value)
                    return;
                idIgra = value;

                NotifyPropertyChanged();
            }
        }

        public string IgraNaziv 
        {
            get => igraNaziv;
            set
            {
                if (igraNaziv == value)
                    return;
                igraNaziv = value;

                NotifyPropertyChanged();
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
