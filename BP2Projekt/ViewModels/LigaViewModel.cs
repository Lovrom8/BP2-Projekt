﻿using BP2Projekt.Models;
using MvvmHelpers;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BP2Projekt.ViewModels
{
    class LigaViewModel : BazniDialogViewModel
    {
        private readonly DelegateCommand _dodajIliOsvjeziCommand;
        public ICommand DodajIliOsvjeziCommand => _dodajIliOsvjeziCommand;

        private OrganizatorModel organizator;
        private IgraModel igra;

        public LigaModel Liga { get; set; }

        public OrganizatorModel Organizator
        {
            get => organizator;
            set
            {
                organizator = value;
                Liga.FK_Organizator = value.ID_Organizator;
                Liga.Organizator = value.Naziv;
                //OnPropertyChanged("Organizator");
            }
        }

        public IgraModel Igra
        {
            get => igra;
            set
            {
                igra = value;
                Liga.FK_Igra = value.ID_Igra;
                Liga.Igra = value.Naziv;
                //OnPropertyChanged("Igra");
            }
        }

        public ObservableCollection<OrganizatorModel> ListaOrganizatori { get; set; }
        public ObservableCollection<IgraModel> ListaIgre { get; set; }
        public ObservableCollection<LigaModel> ListaLiga { get; set; }
        public int ID_Liga { get; set; }

        public LigaViewModel()
        {
            _dodajIliOsvjeziCommand = new DelegateCommand(DodajIliOsvjezi);
        }

        private void UcitajLigu(int LigaID)
        {
            if (LigaID == -1)
                return;

            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                con.Open();

                var selectSQL = new SQLiteCommand(@"SELECT * FROM Liga L JOIN Organizator O ON O.ID_organizator = L.FK_organizator
                                                    JOIN Igra I ON I.ID_igra = L.FK_igra
                                                    WHERE ID_liga=@Id", con);
                selectSQL.Parameters.AddWithValue("@Id", LigaID);

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    if (!reader.HasRows)
                        return;

                    reader.Read();
                    var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                    Liga = new LigaModel()
                    {
                        Naziv = reader["NazivLige"].ToString(),
                        Organizator = reader["NazivOrganizatora"].ToString(),
                        FK_Organizator = Convert.ToInt32(reader["FK_organizator"]),
                        Igra = reader["NazivIgre"].ToString(),
                        FK_Igra = Convert.ToInt32(reader["FK_igra"])
                    };

                    Igra.ID_Igra = Liga.FK_Igra;
                    Igra.Naziv = Liga.Igra;

                    Organizator.ID_Organizator = Liga.FK_Organizator;
                    Organizator.Naziv = Liga.Organizator;

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno povezivanje na bazu, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void UcitajIgre()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var selectSQL = new SQLiteCommand(@"SELECT * FROM Igra", con);

                con.Open();

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    reader.Read();
                    if (!reader.HasRows)
                        return;

                    ListaIgre.Clear();

                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        ListaIgre.Add(new IgraModel()
                        {
                            ID_Igra = Convert.ToInt32(s["ID_igra"].ToString()),
                            Naziv = s["NazivIgre"].ToString(),
                            Zanr = s["Zanr"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno povezivanje na bazu, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void UcitajOrganizatore()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                var selectSQL = new SQLiteCommand(@"SELECT * FROM Organizator", con);

                con.Open();

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    reader.Read();
                    if (!reader.HasRows)
                        return;

                    ListaOrganizatori.Clear();

                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        ListaOrganizatori.Add(new OrganizatorModel()
                        {
                            ID_Organizator = Convert.ToInt32(s["ID_organizator"].ToString()),
                            Naziv = s["NazivOrganizatora"].ToString()
                        });
                    }
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

                if (Liga.ID == -1)
                    insert = @"INSERT INTO Liga (ID_liga, NazivLige, FK_organizator, FK_igra) VALUES (@ID, @Naziv, @FK_Organizator, @FK_Igra)";
                else
                    insert = @"UPDATE Liga SET NazivLige=@Naziv, FK_organizator=@FK_Organizator, FK_igra=@FK_igra WHERE ID_liga=@Id";

                insertSQL = new SQLiteCommand(insert, con);
                insertSQL.Parameters.AddWithValue("@Id", Organizator.ID_Organizator);
                insertSQL.Parameters.AddWithValue("@Naziv", Organizator.Naziv);
                insertSQL.Parameters.AddWithValue("@Osnovan", Organizator.Osnovan);
                insertSQL.Parameters.AddWithValue("@Drzava", Organizator.Drzava);

                try
                {
                    insertSQL.ExecuteNonQuery();
                    MessageBox.Show("Liga dodana u bazu!", "Dodano!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno ubacivanje lige u bazu, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            ListaLiga = parameters.GetValue<ObservableCollection<LigaModel>>("listaLiga");
            ID_Liga = parameters.GetValue<int>("idLiga");

            Igra = new IgraModel();
            Igra.ID_Igra = ID_Liga;
            Organizator = new OrganizatorModel();

            ListaIgre = new ObservableCollection<IgraModel>();
            ListaOrganizatori = new ObservableCollection<OrganizatorModel>();

            UcitajLigu(ID_Liga);
            UcitajOrganizatore();
            UcitajIgre();
        }
    }
}
