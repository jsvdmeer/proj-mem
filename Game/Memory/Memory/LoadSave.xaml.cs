using System;
using System.IO;
using System.Windows;

namespace Memory
{
    /// <summary>
    /// Interaction logic for LoadSave.xaml
    /// </summary>
    public partial class LoadSave : Window
    {
        private string delimiter = ";";

        public LoadSave()
        {
            InitializeComponent();
            GetAllSaves();
        }

        private void GetAllSaves()
        {
            string dir_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "MemoryGame");

            if (!Directory.Exists(dir_path))
            {
                Directory.CreateDirectory(dir_path);
            }

            string[] allSaves = Directory.GetFiles(dir_path);
            foreach (string save_file in allSaves)
            {
                Console.WriteLine(save_file);
                save_list.Items.Add(save_file);
            }
        }

        private void loadSave_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            string path = save_list.SelectedItem.ToString();
            if (!File.Exists(path))
            {
                File.WriteAllText(path, delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter);
            }
            new Spellenscherm(path).ShowDialog();
        }
    }
}
