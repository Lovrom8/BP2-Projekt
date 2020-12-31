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
    class OrganizatorViewModel : BazniDialogViewModel
    {
        private readonly DelegateCommand _dodajIliOsvjeziCommand;
        public ICommand DodajIliOsvjeziCommand => _dodajIliOsvjeziCommand;
        public OrganizatorModel Organizator { get; set; }
        public ObservableCollection<OrganizatorModel> ListaOrganizatora { get; private set; }
        public int ID_organizatora { get; private set; }

        public OrganizatorViewModel()
        {
            _dodajIliOsvjeziCommand = new DelegateCommand(DodajIliOsvjezi);
        }

        private void UcitajOrganizatora(int ID)
        {
            if (ID == -1)
                return;

            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                con.Open();

                var selectSQL = new SQLiteCommand(@"SELECT * FROM Organizator WHERE ID_organizator=@Id", con);
                selectSQL.Parameters.AddWithValue("@Id", ID);

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    if (!reader.HasRows)
                        return;

                    reader.Read();

                    Organizator = new OrganizatorModel()
                    {
                        ID_Organizator = ID,
                        Naziv = reader["NazivOrganizatora"].ToString(),
                        Drzava = reader["Drzava"].ToString(),
                        Osnovan = reader["Osnovan"].ToString(),
                    };
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno čitanje organizatora iz baze, greška: {ex.Message}");
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

                if (Organizator.ID_Organizator == -1)
                    insert = @"INSERT INTO Organizator (NazivOrganizatora, Osnovan, Drzava) VALUES (@Naziv, @Osnovan, @Drzava)";
                else
                    insert = @"UPDATE Organizator SET NazivOrganizatora=@Naziv, Osnovan=@Osnovan, Drzava=@Drzava WHERE ID_organizator=@Id";

                insertSQL = new SQLiteCommand(insert, con);
                insertSQL.Parameters.AddWithValue("@Id", Organizator.ID_Organizator);
                insertSQL.Parameters.AddWithValue("@Naziv", Organizator.Naziv);
                insertSQL.Parameters.AddWithValue("@Osnovan", Organizator.Osnovan);
                insertSQL.Parameters.AddWithValue("@Drzava", Organizator.Drzava);

                try
                {
                    insertSQL.ExecuteNonQuery();
                    MessageBox.Show("Organizator dodan u bazu!", "Dodano!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno ubacivanje organizatora u bazu, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            ListaOrganizatora = parameters.GetValue<ObservableCollection<OrganizatorModel>>("listaOrganizatora");
            ID_organizatora = parameters.GetValue<int>("idOrganizator");

            Organizator = new OrganizatorModel() { ID_Organizator = ID_organizatora};
            UcitajOrganizatora(ID_organizatora);
        }
    }
}
