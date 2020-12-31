using BP2Projekt.Models;
using MvvmHelpers;
using Prism.Commands;
using Prism.Services.Dialogs;
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
    class ProizvodacViewModel : BazniDialogViewModel
    {
        private readonly DelegateCommand _dodajIliOsvjeziCommand;
        public ICommand DodajIliOsvjeziCommand => _dodajIliOsvjeziCommand;
        public ProizvodacModel Proizvodac { get; set; }
        public ObservableCollection<ProizvodacModel> ListaProizvodaca { get; private set; }
        public int ID_Proizvodac { get; private set; }

        public ProizvodacViewModel()
        {
            _dodajIliOsvjeziCommand = new DelegateCommand(DodajIliOsvjezi);
        }

        private void UcitajProizvodaca(int ID_proizvodac)
        {
            if (ID_proizvodac == -1)
                return;

            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                con.Open();

                var selectSQL = new SQLiteCommand(@"SELECT * FROM Proizvodac WHERE ID_proizvodac=@Id ", con);
                selectSQL.Parameters.AddWithValue("@Id", ID_proizvodac);

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    if (!reader.HasRows)
                        return;

                    Proizvodac = new ProizvodacModel()
                    {
                        ID_Proizvodac = Convert.ToInt32(reader["ID_proizvodac"]),
                        Drzava = reader["Drzava"].ToString(),
                        Naziv = reader["NazivProizvodaca"].ToString()
                    };

                    reader.Read();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno povezivanje na bazu, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void DodajIliOsvjezi()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                con.Open();

                string insert;
                SQLiteCommand insertSQL;

                if (Proizvodac.ID_Proizvodac == -1)
                    insert = @"INSERT INTO Proizvodac (ID_proizvodac, Naziv, Drzava, FK_igra) VALUES (@ID, @Naziv, @Drzava)";
                else
                    insert = @"UPDATE Proizvodac SET NazivProizvodaca=@Naziv, Drzava=@Drzava WHERE ID_proizvodac=@Id";

                insertSQL = new SQLiteCommand(insert, con);
                insertSQL.Parameters.AddWithValue("@Id", Proizvodac.ID_Proizvodac);
                insertSQL.Parameters.AddWithValue("@Naziv", Proizvodac.Naziv);
                insertSQL.Parameters.AddWithValue("@Drzava", Proizvodac.Drzava);

                try
                {
                    insertSQL.ExecuteNonQuery();
                    MessageBox.Show("Proizvodac dodan u bazu!", "Dodano!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno ubacivanje proizvodača u bazu, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            ListaProizvodaca = parameters.GetValue<ObservableCollection<ProizvodacModel>>("listaProizvodaca");
            ID_Proizvodac = parameters.GetValue<int>("idProizvodac");

            Proizvodac = new ProizvodacModel() { ID_Proizvodac = -1 };
            UcitajProizvodaca(ID_Proizvodac);
        }
    }
}
