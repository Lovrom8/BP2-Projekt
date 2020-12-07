using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace BP2Projekt
{
    public class SudionikModel : INotifyPropertyChanged
    {
        private int ID_sudionik;
        private int ID_tim;
        private int ID_uloga;
        private string drzava;
        private string nick;
        private string ulogaNaziv;
        private string timNaziv;
        private bool jeOdabran;
        private bool aktivan;

        public int ID_Sudionik
        {
            get => ID_sudionik;
            set
            {
                if (ID_sudionik == value)
                    return;
                ID_sudionik = value;

                NotifyPropertyChanged();
            }
        }
        public int ID_Tim
        {
            get => ID_tim;
            set
            {
                if (ID_tim == value)
                    return;

                ID_tim = value;

                NotifyPropertyChanged();
            }
        }
        public string Drzava 
        {
            get => drzava;
            set
            {
                if (drzava == value)
                    return;
                drzava = value;

                NotifyPropertyChanged();
            }
        }
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
        public string UlogaNaziv
        {
            get => ulogaNaziv;
            set
            {
                if (ulogaNaziv == value)
                    return;
                ulogaNaziv = value;

                NotifyPropertyChanged();
            }
        }

        public string TimNaziv
        {
            get => timNaziv;
            set
            {
                if (timNaziv == value)
                    return;
                timNaziv = value;

                NotifyPropertyChanged();
            }
        }

        public string Nick
        {
            get => nick;
            set
            {
                if (nick == value)
                    return;
                nick = value;

                NotifyPropertyChanged();
            }
        }

        public bool Aktivan
        {
            get => aktivan;
            set
            {
                if (aktivan == value)
                    return;
                aktivan = value;

                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    class IgracModel : SudionikModel
    {

    }

    class TrenerModel : SudionikModel
    {

    }
}
