using BP2Projekt.Baza;
using BP2Projekt.Models;
using MvvmHelpers;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BP2Projekt.ViewModels
{
    class MecViewModel : BaseViewModel
    {
        private readonly DelegateCommand _promjeniTimoveCommand;
        public ICommand PromjeniTimoveCommand => _promjeniTimoveCommand;

        private readonly DelegateCommand _spremiPromjene;
        public ICommand SpremiPromjeneCommand => _spremiPromjene;

        private TimModel _timB;
        private TimModel _timA;

        public MecModel Mec { get; set; }
        public TimModel TimA
        {
            get => _timA;
            set
            {
                _timA = value;
                TimA.Igraci = new ObservableCollection<IgracModel>(IzvadiIgrace(value.ID_Tim));
                OnPropertyChanged("TimA");
            }
        }
        public TimModel TimB
        {
            get => _timB;
            set
            {
                _timB = value;
                TimB.Igraci = new ObservableCollection<IgracModel>(IzvadiIgrace(value.ID_Tim));
                OnPropertyChanged("TimB");
            }
        }

        public ObservableCollection<TimModel> ListaTimovi { get; set; } // PAZI: mora biti property!

        public MecViewModel(int MecID)
        {
            _promjeniTimoveCommand = new DelegateCommand(PromjeniTimove);
            _spremiPromjene = new DelegateCommand(SpremiPromjene);

            ListaTimovi = new ObservableCollection<TimModel>();

            PopuniInfo(MecID);
        }

        private void PopuniInfo(int MecID)
        {
            try
            {
                using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
                {
                    con.Open();

                    var selectSQL = new SQLiteCommand(@"SELECT m.*, t1.ID_tim AS ID_A, t2.ID_tim AS ID_B,
                                                        t1.Naziv AS NazivA, t2.Naziv AS NazivB
                                                        FROM Mec m 
                                                        JOIN Tim t1 ON t1.ID_tim = m.FK_timA 
                                                        JOIN Tim t2 ON t2.ID_tim = m.FK_timB
                                                        WHERE m.ID_mec=@Id ", con);
                    selectSQL.Parameters.AddWithValue("@Id", MecID);

                    var reader = selectSQL.ExecuteReader();

                    reader.Read();
                    if (!reader.HasRows)
                        return;

                    //var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();

                    Mec = new MecModel()
                    {
                        ID_Mec = MecID,
                        RezultatA = Convert.ToInt32(reader["rezultatA"]),
                        RezultatB = Convert.ToInt32(reader["rezultatB"]),
                        FK_Pobjednik = Convert.ToInt32(reader["FK_pobjednik"]),
                        FK_TimA = Convert.ToInt32(reader["FK_timA"]),
                        FK_TimB = Convert.ToInt32(reader["FK_timB"])
                    };

                    /*TimA = new TimModel()
                     {
                         ID_Tim = reader.GetInt32(reader.GetNthOrdinal("ID_tim", 1)), // U slučaju da nismo koristili aliase, imali bi duple sutpce
                         Naziv = reader.GetString(reader.GetNthOrdinal("Naziv", 1))
                     };

                     TimB = new TimModel()
                     {
                         ID_Tim = reader.GetInt32(reader.GetNthOrdinal("ID_tim", 2)),
                         Naziv = reader.GetString(reader.GetNthOrdinal("Naziv", 2))
                     };*/

                    TimA = new TimModel()
                    {
                        ID_Tim = Convert.ToInt32(reader["ID_A"]),
                        Naziv = reader["NazivA"].ToString()
                    };

                    TimB = new TimModel()
                    {
                        ID_Tim = Convert.ToInt32(reader["ID_B"]),
                        Naziv = reader["NazivB"].ToString()
                    };

                    con.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Neuspješno povezivanje na bazu, greška: {ex.Message}");
            }
        }

        private Visibility _vidljivost = Visibility.Collapsed;
        public Visibility Vidljivost
        {
            get => _vidljivost;
            set
            {
                _vidljivost = value;
                OnPropertyChanged("Vidljivost");
            }
        }

        private ObservableCollection<IgracModel> IzvadiIgrace(int timID)
        {
            var igraci = new ObservableCollection<IgracModel>();

            try
            {
                using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
                {
                    con.Open();
                    var selectSQL = new SQLiteCommand(@"SELECT * FROM Igrac m 
                                                        WHERE m.FK_tim=@Id ", con);
                    selectSQL.Parameters.AddWithValue("@Id", timID);

                    var reader = selectSQL.ExecuteReader();

                    reader.Read();
                    if (!reader.HasRows)
                        return igraci;

                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        igraci.Add(new IgracModel()
                        {
                            ID_Sudionik = Convert.ToInt32(s["ID_igrac"].ToString()),
                            Drzava = s["Drzava"].ToString(),
                            Nick = s["Nick"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Neuspješno povezivanje na bazu, greška: {ex.Message}");
            }

            return igraci;
        }

        private void PromjeniTimove()
        {
            try
            {
                using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
                {
                    con.Open();

                    var selectSQL = new SQLiteCommand(@"SELECT ID_tim, Naziv FROM Tim", con);

                    var reader = selectSQL.ExecuteReader();

                    reader.Read();
                    if (!reader.HasRows)
                        return;

                    ListaTimovi.Clear();

                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        ListaTimovi.Add(new TimModel()
                        {
                            ID_Tim = Convert.ToInt32(s["ID_tim"].ToString()),
                            Naziv = s["Naziv"].ToString()
                        });
                    }

                    con.Close();
                }

                Vidljivost = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Neuspješno povezivanje na bazu, greška: {ex.Message}");
            }
        }

        private void SpremiPromjene()
        {
            try
            {
                using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
                {
                    con.Open();

                    var updateSQL = new SQLiteCommand(@"UPDATE Mec SET FK_timA = @TimAId, FK_timB = @TimBId WHERE ID_mec=@MecID", con);
                    updateSQL.Parameters.AddWithValue("@TimAId", TimA.ID_Tim);
                    updateSQL.Parameters.AddWithValue("@TimBId", TimB.ID_Tim);
                    updateSQL.Parameters.AddWithValue("@MecID", Mec.ID_Mec);

                    updateSQL.ExecuteNonQuery();

                    con.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Neuspješno povezivanje na bazu, greška: {ex.Message}");
            }

            Vidljivost = Visibility.Collapsed;
        }
    }
}
