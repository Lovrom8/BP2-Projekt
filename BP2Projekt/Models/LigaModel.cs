using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BP2Projekt.Models
{
    class LigaModel : INotifyPropertyChanged
    {
        private int id;
        private int idIgra;
        private int organizator;
        private string organizatorNaziv;
        private string nazivLige;
        private string igra;
        private bool lan;

        public int ID_Liga
        {
            get => id;
            set
            {
                if (id == value)
                    return;

                id = value;
                NotifyPropertyChanged();
            }
        }

        public string Naziv
        {
            get => nazivLige;
            set
            {
                if (nazivLige == value)
                    return;

                nazivLige = value;
                NotifyPropertyChanged();
            }
        }

        public bool LAN
        {
            get => lan;
            set
            {
                if (lan == value)
                    return;

                lan = value;
                NotifyPropertyChanged();
            }
        }

        public int FK_Organizator
        {
            get => organizator;
            set
            {
                if (organizator == value)
                    return;

                organizator = value;
                NotifyPropertyChanged();
            }
        }

        public string Organizator
        {
            get => organizatorNaziv;
            set
            {
                if (organizatorNaziv == value)
                    return;

                organizatorNaziv = value;
                NotifyPropertyChanged();
            }
        }

        public int FK_Igra
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

        public string Igra
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
