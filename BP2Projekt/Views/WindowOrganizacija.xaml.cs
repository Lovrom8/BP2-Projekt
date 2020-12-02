﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Globalization;
using System.Data.SQLite;
using BP2Projekt.Models;
using Prism.Commands;
using System.Windows.Input;
using MaterialDesignExtensions.Controls;

namespace BP2Projekt
{
    public partial class WindowOrganizacija : MaterialWindow
    {
        public WindowOrganizacija()
        {
            InitializeComponent();

            int ID = 2;
            var _ovm = new OrganizacijaViewModel(ID);
            DataContext = _ovm;
        }
    }
}
