﻿using System;
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
        private string igrac_od;
        private string trener_od;
        private string ime;
        private string prezime;
        private int idTim;
        private string tim;
        private int starost;

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

        public string IgracOd
        {
            get => igrac_od;
            set
            {
                if (igrac_od == value)
                    return;

                igrac_od = value;

                NotifyPropertyChanged();
            }
        }

        public string TrenerOd
        {
            get => trener_od;
            set
            {
                if (trener_od == value)
                    return;

                trener_od = value;

                NotifyPropertyChanged();
            }
        }

        public string Ime
        {
            get => ime;
            set
            {
                if (ime == value)
                    return;

                ime = value;

                NotifyPropertyChanged();
            }
        }

        public string Prezime
        {
            get => prezime; 
            set 
            {
                if (prezime == value)
                    return;

                prezime = value;

                NotifyPropertyChanged();
            }
        }

        public int FK_Tim
        {
            get => idTim;
            set
            {
                if (idTim == value)
                    return;

                idTim = value;

                NotifyPropertyChanged();
            }
        }

        public string Tim
        {
            get => tim;
            set
            {
                if (tim == value)
                    return;

                tim = value;

                NotifyPropertyChanged();
            }
        }

        public int Starost
        {
            get => starost;
            set
            {
                if (starost == value)
                    return;

                starost = value;

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
