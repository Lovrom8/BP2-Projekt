using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BP2Projekt.Models
{
    public class IgraModel : INotifyPropertyChanged
    {
        private int id;
        private string zanr;
        private string naziv;
        private int idProizvodac;
        private string proizvodacNaziv;
        private int maxIgraca;

        public int ID_Igra
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
            get => naziv;
            set
            {
                if (naziv == value)
                    return;

                naziv = value;
                NotifyPropertyChanged();
            }
        }

        public string Zanr
        {
            get => zanr;
            set
            {
                if (zanr == value)
                    return;

                zanr = value;
                NotifyPropertyChanged();
            }
        }

        public int FK_Proizvodac
        {
            get => idProizvodac;
            set
            {
                if (idProizvodac == value)
                    return;

                idProizvodac = value;
                NotifyPropertyChanged();
            }
        }

        public string Proizvodac
        {
            get => proizvodacNaziv;
            set
            {
                if (proizvodacNaziv == value)
                    return;

                proizvodacNaziv = value;
                NotifyPropertyChanged();
            }
        }

        public int MaxIgraca
        {
            get => maxIgraca;
            set
            {
                if (maxIgraca == value)
                    return;

                maxIgraca = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
