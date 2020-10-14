using BP2Projekt.Models;
using MvvmHelpers;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BP2Projekt.ViewModels
{
    class TimViewModel : BaseViewModel
    {
        private readonly DelegateCommand _dodajIgracaCmd;
        private readonly DelegateCommand _obirisiIgracaCmd;

        public ICommand DodajIgracaCommand => _dodajIgracaCmd;
        public ICommand ObrisiIgracaCommand => _obirisiIgracaCmd;

        public ObservableCollection<IgracModel> Igraci;

        public TimViewModel(int TimID)
        {
            _dodajIgracaCmd = new DelegateCommand(DodajIgraca);
            _obirisiIgracaCmd = new DelegateCommand(ObrisiIgraca);

            PopuniListuIgraca(TimID);
        }

        private void PopuniListuIgraca(int TimID)
        {
            Igraci = new ObservableCollection<IgracModel>();

            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                con.Open();

                var selectSQL = new SQLiteCommand(@"SELECT * FROM Igraci WHERE FK_tim=@Id", con);
                selectSQL.Parameters.AddWithValue("@Id", TimID);

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    reader.Read();

                    if (!reader.HasRows)
                        return;

                  /*  Organizacija.ID_org = ID;
                    Organizacija.Naziv = reader["Naziv"].ToString();
                    Organizacija.Drzava = reader["Drzava"].ToString();
                    Organizacija.Osnovana = reader["Osnovana"].ToString();*/
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno povezivanje na bazu, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void DodajIgraca()
        {

        }

        private void ObrisiIgraca()
        {

        }
    }
}
