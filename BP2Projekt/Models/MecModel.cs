using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace BP2Projekt.Models
{
    class MecModel : INotifyPropertyChanged
    {
        private int id;
        private DateTime datum;
        private int timA;
        private int timB;
        private int pobjednik;
        private int rezultatA;
        private int rezultatB;
        private int ligaID;

        private string igra;
        private string liga;
        private string nazivA;
        private string nazivB;
        private string pobjednikNaziv;

        public int ID_Mec
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

        public DateTime Datum
        {
            get => datum;
            set
            {
                if (datum == value)
                    return;

                datum = value;
                NotifyPropertyChanged();
            }
        }

        
        public int FK_TimA
        {
            get => timA;
            set
            {
                if (timA == value)
                    return;

                timA = value;
                NotifyPropertyChanged();
            }
        }

        public int FK_TimB
        {
            get => timB;
            set
            {
                if (timB == value)
                    return;

                timB = value;
                NotifyPropertyChanged();
            }
        }

        public int FK_Pobjednik
        {
            get => pobjednik;
            set
            {
                if (pobjednik == value)
                    return;

                pobjednik = value;
                NotifyPropertyChanged();
            }
        }

        public int RezultatA
        {
            get => rezultatA;
            set
            {
                if (rezultatA == value)
                    return;

                rezultatA = value;
                NotifyPropertyChanged();
            }
        }

        public int RezultatB
        {
            get => rezultatB;
            set
            {
                if (rezultatB == value)
                    return;

                rezultatB = value;
                NotifyPropertyChanged();
            }
        }

        public int FK_Liga
        {
            get => ligaID;
            set
            {
                if (ligaID == value)
                    return;

                ligaID = value;
                NotifyPropertyChanged();
            }
        }

        public string Liga
        {
            get => liga;
            set
            {
                if (liga == value)
                    return;

                liga = value;
                NotifyPropertyChanged();
            }
        }

        public string TimA
        {
            get => nazivA;
            set
            {
                if (nazivA == value)
                    return;

                nazivA = value;
                NotifyPropertyChanged();
            }
        }

        public string TimB
        {
            get => nazivB;
            set
            {
                if (nazivB == value)
                    return;

                nazivB = value;
                NotifyPropertyChanged();
            }
        }

        public string Pobjednik
        {
            get => pobjednikNaziv;
            set
            {
                if (pobjednikNaziv == value)
                    return;

                pobjednikNaziv = value;
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
