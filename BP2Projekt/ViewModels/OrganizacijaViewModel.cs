using BP2Projekt.Models;
using BP2Projekt.ViewModels;
using MvvmHelpers;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BP2Projekt
{
    class OrganizacijaViewModel : BazniDialogViewModel
    {
        private readonly DelegateCommand _dodajIliOsvjeziCommand;

        private ObservableCollection<OrganizacijaModel> _listaOrganizacija;

        public ObservableCollection<OrganizacijaModel> ListaOrganizacija
        {
            get => _listaOrganizacija;
            set => SetProperty(ref _listaOrganizacija, value);
        }

        public ICommand DodajIliOsvjeziCommand => _dodajIliOsvjeziCommand;
        public OrganizacijaModel Organizacija { get; set; }

        private int _idOrg;
        public int ID_Org
        {
            get => _idOrg;
            set => SetProperty(ref _idOrg, value);
        }

        public OrganizacijaViewModel()
        {
            _dodajIliOsvjeziCommand = new DelegateCommand(DodajIliOsvjezi);
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

                    Organizacija = new OrganizacijaModel()
                    {
                        ID_Organizacija = ID,
                        Naziv = reader["NazivOrganizacije"].ToString(),
                        Drzava = reader["Drzava"].ToString(),
                        Osnovana = reader["Osnovana"].ToString()
                    };
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno učitavanje organizacije iz baze, greška: {ex.Message}");
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

                if (Organizacija.ID_Organizacija == -1)
                    insert = @"INSERT INTO Organizacija (NazivOrganizacije, Osnovana, Drzava, BrojTimova) VALUES (@Naziv, @Osnovana, @Drzava, 0)";
                else
                    insert = @"UPDATE Organizacija SET NazivOrganizacije=@Naziv, Osnovana=@Osnovana, Drzava=@Drzava WHERE ID_org=@Id";

                insertSQL = new SQLiteCommand(insert, con);

                insertSQL.Parameters.AddWithValue("@Id", Organizacija.ID_Organizacija);
                insertSQL.Parameters.AddWithValue("@Naziv", Organizacija.Naziv);
                insertSQL.Parameters.AddWithValue("@Osnovana", Organizacija.Osnovana);
                insertSQL.Parameters.AddWithValue("@Drzava", Organizacija.Drzava);

                try
                {
                    insertSQL.ExecuteNonQuery();
                    MessageBox.Show("Organizacija dodana u bazu!", "Dodano!");

                    if (Organizacija.ID_Organizacija == -1)
                        ListaOrganizacija.Add(Organizacija);
                    else
                        ListaOrganizacija[ListaOrganizacija.IndexOf(ListaOrganizacija.FirstOrDefault(o =>o.ID_Organizacija == ID_Org))] = Organizacija;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno ubacivanje organizacije u bazu, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            ListaOrganizacija = parameters.GetValue<ObservableCollection<OrganizacijaModel>>("listaOrganizacija");
            ID_Org = parameters.GetValue<int>("idOrg");

            Organizacija = new OrganizacijaModel() { ID_Organizacija = ID_Org };

            UcitajOrganizaciju(ID_Org);
        }
    }
}
