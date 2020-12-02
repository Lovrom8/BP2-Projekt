using BP2Projekt.Models;
using MvvmHelpers;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BP2Projekt
{
    class OrganizacijaViewModel : BaseViewModel
    {
        private readonly DelegateCommand _dodajIliOsvjeziCommand;
        public ICommand DodajIliOsvjeziCommand => _dodajIliOsvjeziCommand;
        public OrganizacijaModel Organizacija { get; }

        public OrganizacijaViewModel(int ID)
        {
            _dodajIliOsvjeziCommand = new DelegateCommand(DodajIliOsvjezi);
            Organizacija = new OrganizacijaModel();

            UcitajOrganizaciju(ID);
        }

        private void UcitajOrganizaciju(int ID)
        {
            if (ID == -1)
                return;

            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                con.Open();

                var selectSQL = new SQLiteCommand(@"SELECT * FROM Organizacija WHERE ID_org=@Id", con);
                selectSQL.Parameters.AddWithValue("@Id", ID);

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    if (!reader.HasRows)
                        return;

                    reader.Read();
                    Organizacija.ID_org = ID;
                    Organizacija.Naziv = reader["Naziv"].ToString();
                    Organizacija.Drzava = reader["Drzava"].ToString();
                    Organizacija.Osnovana = reader["Osnovana"].ToString();
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

                var insertSQL = new SQLiteCommand(@"INSERT OR REPLACE INTO Organizacija (ID_org, Naziv, Osnovana, Drzava) VALUES (@Id, @Naziv, @Osnovana, @Drzava)", con);

                insertSQL.Parameters.AddWithValue("@Id", Organizacija.ID_org);
                insertSQL.Parameters.AddWithValue("@Naziv", Organizacija.Naziv);
                insertSQL.Parameters.AddWithValue("@Osnovana", Organizacija.Osnovana);
                insertSQL.Parameters.AddWithValue("@Drzava", Organizacija.Drzava);

                try
                {
                    insertSQL.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno povezivanje na bazu, greška: {ex.Message}");
                }

                con.Close();
            }

            MessageBox.Show("Organizacija dodana u bazu!", "Dodano!");
        }
    }
}
