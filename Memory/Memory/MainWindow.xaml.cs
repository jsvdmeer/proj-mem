﻿using System;
using System.IO;
using System.Windows;

namespace Memory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            System.IO.Stream str = Memory.Properties.Resources.lobby;
            System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
            snd.Play();
        }

        /// <summary>
        /// Open a new gamewindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            string delimiter = ";";
            this.WindowState = WindowState.Minimized;
            int save_number = 0;

            string dir_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "MemoryGame");

            if (!Directory.Exists(dir_path))
            {
                Directory.CreateDirectory(dir_path);
            }

            string path = Path.Combine(dir_path, "Save" + save_number + ".csv");

            while (File.Exists(path))
            {
                save_number++;
                path = Path.Combine(dir_path, "Save" + save_number + ".csv");
            }

            File.WriteAllText(path, delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter);
            new Spellenscherm(path).ShowDialog();
        }

        /// <summary>
        /// Resume a game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hervatten_Click(object sender, RoutedEventArgs e)
        {
            new LoadSave().ShowDialog();
        }

        /// <summary>
        /// Show the highscores window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Highscore_Click(object sender, RoutedEventArgs e)
        {
            new Highscores().ShowDialog();
        }

        /// <summary>
        /// Set the themes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Themas_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Close the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
