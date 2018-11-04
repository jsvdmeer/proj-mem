﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Memory
{
    /// <summary>
    /// Interaction logic for LoadSave.xaml
    /// </summary>
    public partial class LoadSave : Window
    {
        public LoadSave()
        {
            InitializeComponent();
        }

        private void Save1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            new Spellenscherm1().ShowDialog();
        }

        private void Save2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            new Spellenscherm2().ShowDialog();
        }

        private void Save3_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            new Spellenscherm3().ShowDialog();
        }
    }
}
