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

        public MecModel Mec { get; set; }
        public TimModel TimA { get; set; }
        public TimModel TimB { get; set; }
        public TimModel NoviA { get; set; }

        public ObservableCollection<TimModel> ListaTimovi { get; set; } // PAZI: mora biti property!

        public MecViewModel(int MecID)
        {
            _promjeniTimoveCommand = new DelegateCommand(PromjeniTimove);
            _spremiPromjene = new DelegateCommand(SpremiPromjene);

            ListaTimovi = new ObservableCollection<TimModel>();

            PopuniInfo(MecID);
            IzvadiIgrace(TimA.ID_Tim);
            IzvadiIgrace(TimB.ID_Tim);
        }

        private void PopuniInfo(int MecID)
        {
            try
            {
                using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
                {
                    con.Open();

                    var selectSQL = new SQLiteCommand(@"SELECT * FROM Mec m 
                                                        JOIN Tim t1 ON t1.ID_tim = m.FK_timA 
                                                        JOIN Tim t2 ON t2.ID_tim = m.FK_timB
                                                        WHERE m.ID_mec=@Id ", con);
                    selectSQL.Parameters.AddWithValue("@Id", MecID);

                    var reader = selectSQL.ExecuteReader();

                    reader.Read();
                    if (!reader.HasRows)
                        return;

                    var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();

                    Mec = new MecModel()
                    {
                        ID_Mec = MecID,
                        RezultatA = Convert.ToInt32(reader["rezultatA"]),
                        RezultatB = Convert.ToInt32(reader["rezultatB"]),
                        FK_Pobjednik = Convert.ToInt32(reader["FK_pobjednik"]),
                        FK_TimA = Convert.ToInt32(reader["FK_timA"]),
                        FK_TimB = Convert.ToInt32(reader["FK_timB"])
                    };

                    TimA = new TimModel()
                    {
                        ID_Tim = reader.GetInt32(reader.GetNthOrdinal("ID_tim", 1)),
                        Naziv = reader.GetString(reader.GetNthOrdinal("Naziv", 1)) // Dva su sa ID_tim, svaki za jedan tim
                    };

                    TimB = new TimModel()
                    {
                        ID_Tim = reader.GetInt32(reader.GetNthOrdinal("ID_tim", 2)),
                        Naziv = reader.GetString(reader.GetNthOrdinal("Naziv", 2))
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
            get
            {
                return _vidljivost;
            }
            set
            {
                _vidljivost = value;
                OnPropertyChanged("Vidljivost");
            }
        }

        private List<IgracModel> IzvadiIgrace(SQLiteConnection con, int timID)
        {
            var igraci = new List<IgracModel>();

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

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Neuspješno povezivanje na bazu, greška: {ex.Message}");
            }

            Vidljivost = Visibility.Collapsed;
        }
    }
}
