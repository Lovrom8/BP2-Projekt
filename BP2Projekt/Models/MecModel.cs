using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BP2Projekt.Models
{
    class MecModel : INotifyPropertyChanged
    {
        private int id;
        private string datum;
        private int timA;
        private int timB;
        private int pobjednik;
        private int rezultatA;
        private int rezultatB;

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

        public string Datum
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
