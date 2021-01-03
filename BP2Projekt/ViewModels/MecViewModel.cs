using BP2Projekt.Baza;
using BP2Projekt.Models;
using MvvmHelpers;
using Prism.Commands;
using Prism.Services.Dialogs;
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
    class MecViewModel : BazniDialogViewModel
    {
        private readonly DelegateCommand _promjeniTimoveCommand;
        public ICommand PromjeniTimoveCommand => _promjeniTimoveCommand;

        private readonly DelegateCommand _spremiPromjene;
        public ICommand SpremiPromjeneCommand => _spremiPromjene;

        private TimModel _timB;
        private TimModel _timA;

        public TimABRadioModel RadioModel { get; set; }

        public MecModel Mec { get; set; }
        public TimModel TimA
        {
            get => _timA;
            set
            {
                SetProperty(ref _timA, value);

                TimA.Igraci = new ObservableCollection<IgracModel>(IzvadiIgrace(value.ID_Tim));
            }
        }
        public TimModel TimB
        {
            get => _timB;
            set
            {
                SetProperty(ref _timB, value);

                TimB.Igraci = new ObservableCollection<IgracModel>(IzvadiIgrace(value.ID_Tim));
            }
        }

        public ObservableCollection<TimModel> ListaTimovi { get; set; } // PAZI: mora biti property!

        public MecViewModel()
        {
            _promjeniTimoveCommand = new DelegateCommand(PromjeniTimove);
            _spremiPromjene = new DelegateCommand(SpremiPromjene);
            RadioModel = new TimABRadioModel();
        }

        private void PopuniInfo(int MecID)
        {
            if (MecID == -1)
                return;

            try
            {
                using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
                {
                    con.Open();

                    var selectSQL = new SQLiteCommand(@"SELECT m.*, p.FK_pobjednik, t1.ID_tim AS ID_A, t2.ID_tim AS ID_B,
                                                        t1.NazivTima AS NazivA, t2.NazivTima AS NazivB
                                                        FROM Mec m 
                                                        JOIN Tim t1 ON t1.ID_tim = m.FK_timA 
                                                        JOIN Tim t2 ON t2.ID_tim = m.FK_timB
                                                        JOIN PobjednikMeca p ON p.FK_mec = m.ID_mec
                                                        WHERE m.ID_mec=@Id", con);
                    selectSQL.Parameters.AddWithValue("@Id", MecID);

                    var reader = selectSQL.ExecuteReader();

                    reader.Read();
                    if (!reader.HasRows)
                        return;

                    Mec = new MecModel()
                    {
                        ID_Mec = MecID,
                        RezultatA = Convert.ToInt32(reader["rezultatA"]),
                        RezultatB = Convert.ToInt32(reader["rezultatB"]),
                        FK_Pobjednik = Convert.ToInt32(reader["FK_pobjednik"]),
                        FK_TimA = Convert.ToInt32(reader["FK_timA"]),
                        FK_TimB = Convert.ToInt32(reader["FK_timB"]),
                        Datum = DateTime.Parse(reader["Datum"].ToString())
                    };

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
            set => SetProperty(ref _vidljivost, value);
        }

        public ObservableCollection<MecModel> ListaMecevi { get; private set; }
        public int ID_Mec { get; private set; }

        private ObservableCollection<IgracModel> IzvadiIgrace(int timID)
        {
            var igraci = new ObservableCollection<IgracModel>();

            try
            {
                using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
                {
                    con.Open();
                    var selectSQL = new SQLiteCommand(@"SELECT * FROM Sudionik S 
                                                        WHERE S.FK_tim=@Id AND EXISTS ( SELECT * FROM Igrac WHERE FK_sudionik = S.ID_sudionik)", con);
                    selectSQL.Parameters.AddWithValue("@Id", timID);

                    var reader = selectSQL.ExecuteReader();

                    if (!reader.HasRows)
                        return igraci;

                    foreach (DbDataRecord s in reader.Cast<DbDataRecord>())
                    {
                        igraci.Add(new IgracModel()
                        {
                            ID_Sudionik = Convert.ToInt32(s["ID_sudionik"].ToString()),
                            Drzava = s["Drzava"].ToString(),
                            Nick = s["Nadimak"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Neuspješno čitanje igrača iz baze, greška: {ex.Message}");
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

                    var selectSQL = new SQLiteCommand(@"SELECT ID_tim, NazivTima FROM Tim", con);

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
                            Naziv = s["NazivTima"].ToString()
                        });
                    }

                    con.Close();
                }

                Vidljivost = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Neuspješna promjena timova, greška: {ex.Message}");
            }
        }

        private void SpremiPromjene()
        {
            using (var con = new SQLiteConnection(SQLPostavke.ConnectionStr))
            {
                con.Open();

                SQLiteCommand updateSQL;
                string sqlStr;

                if (Mec.ID_Mec != -1) // Popuni tablicu Mec
                    sqlStr = @"UPDATE Mec SET FK_timA = @TimAId, FK_timB = @TimBId, Datum = @Datum, RezultatA = @RezA, RezultatB = @RezB WHERE ID_mec=@MecID";
                else
                    sqlStr = @"INSERT INTO Mec (FK_timA, FK_timB, Datum, RezultatA, RezultatB) VALUES (@TimAId, @TimBId, @Datum, @RezA, @RezB)";

                updateSQL = new SQLiteCommand(sqlStr, con);
                updateSQL.Parameters.AddWithValue("@TimAId", TimA.ID_Tim);
                updateSQL.Parameters.AddWithValue("@TimBId", TimB.ID_Tim);
                updateSQL.Parameters.AddWithValue("@MecID", Mec.ID_Mec);
                updateSQL.Parameters.AddWithValue("@Datum", Mec.Datum.GetSQLiteDateTime());
                updateSQL.Parameters.AddWithValue("@RezA", Mec.RezultatA);
                updateSQL.Parameters.AddWithValue("@RezB", Mec.RezultatB);

                try
                {
                    updateSQL.ExecuteNonQuery();

                    if (ID_Mec == -1)
                        ListaMecevi.Add(Mec);
                    else
                        ListaMecevi[ListaMecevi.IndexOf(ListaMecevi.FirstOrDefault(o => o.ID_Mec == ID_Mec))] = Mec;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno spremanje timova, greška: {ex.Message}");
                    con.Close();
                    return;
                }

                if (Mec.ID_Mec != -1) //Popuni tablicu PobjednikMeca
                    sqlStr = @"UPDATE PobjednikMeca SET FK_pobjednik=@PobjednikId WHERE FK_mec=@MecID";
                else
                    sqlStr = @"INSERT INTO PobjednikMeca (FK_mec, FK_pobjednik) VALUES (@MecId, @PobjednikId)";

                updateSQL = new SQLiteCommand(sqlStr, con);
                if (RadioModel.PobjednikA)
                    updateSQL.Parameters.AddWithValue("@PobjednikId", TimA.ID_Tim);
                else
                    updateSQL.Parameters.AddWithValue("@PobjednikId", TimB.ID_Tim); 
                updateSQL.Parameters.AddWithValue("@MecId", Mec.ID_Mec);

                try
                {
                    updateSQL.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspješno spremanje timova, greška: {ex.Message}");
                }

                con.Close();
            }

            Vidljivost = Visibility.Collapsed;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            ListaMecevi = parameters.GetValue<ObservableCollection<MecModel>>("listaMeceva");
            ID_Mec = parameters.GetValue<int>("idMec");

            ListaTimovi = new ObservableCollection<TimModel>();

            PopuniInfo(ID_Mec);
        }
    }

    class TimABRadioModel : BaseViewModel
    {
        private bool _pobjednikA = true;
        public bool PobjednikA
        {
            get => _pobjednikA;
            set
            {
                _pobjednikA = value;
                OnPropertyChanged("RadioTimA");
            }
        }

        private bool _pobjednikB = false;
        public bool PobjednikB
        {
            get => _pobjednikB;
            set
            {
                _pobjednikB = value;
                OnPropertyChanged("RadioTimB");
            }
        }
    }
}
