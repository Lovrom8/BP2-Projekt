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
        private static Hashtable _RegisterWindow = new Hashtable();

        public static void Registriraj<T>(string key)
        {
            if (!_RegisterWindow.Contains(key))
                _RegisterWindow.Add(key, typeof(T));
        }

        public static void Registriraj(string key, Type t)
        {
            if (!_RegisterWindow.Contains(key))
                _RegisterWindow.Add(key, t);
        }

        public static void Izbrisi(string key)
        {
            if (_RegisterWindow.ContainsKey(key))
                _RegisterWindow.Remove(key);
        }

        public static void Prikazi(string key, object VM = null)
        {
            if (_RegisterWindow.ContainsKey(key))
            {
                var win = (Window)Activator.CreateInstance((Type)_RegisterWindow[key]);
                win.DataContext = VM;
                win.Show();
            }
        }
    }
}
