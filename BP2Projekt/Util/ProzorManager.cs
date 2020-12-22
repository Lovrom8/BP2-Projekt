using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BP2Projekt.Util
{
    public static class ProzorManager
    {
        private static Hashtable _Prozori = new Hashtable();

        public static void Registriraj<T>(string key)
        {
            if (!_Prozori.Contains(key))
                _Prozori.Add(key, typeof(T));
        }

        public static void Registriraj(string key, Type t)
        {
            if (!_Prozori.Contains(key))
                _Prozori.Add(key, t);
        }

        public static void Izbrisi(string key)
        {
            if (_Prozori.ContainsKey(key))
                _Prozori.Remove(key);
        }

        //public static void Prikazi(string key, ObservableCollection /*object VM = null*/)
        public static void Prikazi(string key, object VM = null)
        {
            if (_Prozori.ContainsKey(key))
            {
                var win = (Window)Activator.CreateInstance((Type)_Prozori[key]);
                //win.DataContext = VM;
                win.Owner = Application.Current.MainWindow;
                win.Show();
            }
        }
    }
}
