﻿using BP2Projekt.Models;
using MvvmHelpers;
using Prism.Commands;
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
    class TimViewModel : BaseViewModel
    {
        private readonly DelegateCommand _dodajIgracaCmd;
        private readonly DelegateCommand _obirisiIgracaCmd;

        public ICommand DodajIgracaCommand => _dodajIgracaCmd;
        public ICommand ObrisiIgracaCommand => _obirisiIgracaCmd;

        public IgracModel OdabraniIgrac { get; set; }
        public ObservableCollection<IgracModel> ListaIgraci { get; set; }
        public ObservableCollection<IgracModel> OstaliIgraci { get; set; }

        public TimViewModel(int TimID)
        {
            _dodajIgracaCmd = new DelegateCommand(DodajIgraca);
            _obirisiIgracaCmd = new DelegateCommand(ObrisiIgraca);

            PopuniListuIgraca(TimID);
        }

        private void PopuniListuIgraca(int TimID)
        {
            ListaIgraci = new ObservableCollection<IgracModel>();

            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                con.Open();

                var selectSQL = new SQLiteCommand(@"SELECT * FROM Igrac JOIN Uloga ON Igrac.FK_uloga=Uloga.ID_uloga WHERE FK_tim=@Id", con);
                selectSQL.Parameters.AddWithValue("@Id", TimID);

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    reader.Read();
                    if (!reader.HasRows)
                        return;

                    ListaIgraci.Clear();
                    var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        ListaIgraci.Add(new IgracModel()
                        {
                            ID_Sudionik = Convert.ToInt32(s["ID_igrac"].ToString()),
                            Nick = s["Nick"].ToString(),
                            Drzava = s["Drzava"].ToString(),
                            UlogaNaziv = s["Naziv"].ToString(),
                            ID_Uloga = Convert.ToInt32(s["FK_uloga"].ToString())
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno čitanje igrača iz baze, greška: {ex.Message}");
                }

                //TODO: zavrsi
                selectSQL = new SQLiteCommand(@"SELECT * FROM Igrac WHERE NOT FK_tim=@Id", con);
                selectSQL.Parameters.AddWithValue("@Id", TimID);

                try
                {
                    var reader = selectSQL.ExecuteReader();

                    reader.Read();
                    if (!reader.HasRows)
                        return;

                    OstaliIgraci.Clear();

                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        OstaliIgraci.Add(new IgracModel()
                        {
                            ID_Sudionik = Convert.ToInt32(s["ID_igrac"].ToString()),
                            Nick = s["Nick"].ToString(),
                            Drzava = s["Drzava"].ToString()
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno čitanje igrača iz baze, greška: {ex.Message}");
                }

                con.Close();
            }
        }

        private void DodajIgraca()
        {

        }

        private void ObrisiIgraca()
        {
            if (OdabraniIgrac == null)
                return;

            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                con.Open();

                var updateSQL = new SQLiteCommand(@"UPDATE Igrac SET FK_tim = NULL WHERE ID_igrac=@Id", con);
                updateSQL.Parameters.AddWithValue("@Id", OdabraniIgrac.ID_Sudionik);

                try
                {
                    updateSQL.ExecuteNonQuery(); // "Obrisi" iz baze

                    ListaIgraci.Remove(ListaIgraci.FirstOrDefault(igr => igr.ID_Sudionik == OdabraniIgrac.ID_Sudionik)); // Obrisi s liste 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno osvježavanje u bazi, greška: {ex.Message}");
                }

                con.Close();
            }
        }
    }
}
