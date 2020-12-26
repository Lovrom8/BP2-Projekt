using BP2Projekt.Models;
using MvvmHelpers;
using Prism.Commands;
using System;
using System.Collections.Generic;
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
        public OrganizatorModel Organizator { get; }

        public OrganizatorViewModel(int ID)
        {
            _dodajIliOsvjeziCommand = new DelegateCommand(DodajIliOsvjezi);
            Organizator = new OrganizatorModel();
            Organizator.ID_Organizator = ID;

            UcitajOrganizatora(ID);
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
                    Organizator.ID_Organizator = ID;
                    Organizator.Naziv = reader["NazivOrganizatora"].ToString();
                    Organizator.Drzava = reader["Drzava"].ToString();
                    Organizator.Osnovan = reader["Osnovan"].ToString();
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

                //var insertSQL = new SQLiteCommand(@"INSERT OR REPLACE INTO Organizator (ID_organizator, NazivOrganizatora, Osnovan, Drzava) VALUES (NULL, @Naziv, @Osnovan, @Drzava)", con);
                //var insertSQL = new SQLiteCommand(@"INSERT OR REPLACE INTO Organizator (ID_organizator, NazivOrganizatora, Osnovan, Drzava) VALUES (@ID, @Naziv, @Osnovan, @Drzava)", con);
                if (Organizator.ID_Organizator == -1)
                    insert = @"INSERT INTO Organizator (ID_organizator, NazivOrganizatora, Osnovan, Drzava) VALUES (@ID, @Naziv, @Osnovan, @Drzava)";
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
    }
}
