using System.Windows;
namespace Memory
{
    /// <summary>
    /// Interaction logic for Highscores.xaml
    /// </summary>
    public partial class Highscores : Window
    {
        public Highscores()
        {
            InitializeComponent();

            // get all the highscores and display them
            vak_naam1.Content = Memory.Properties.Settings.Default.name1;
            vak_naam2.Content = Memory.Properties.Settings.Default.name2;
            vak_naam3.Content = Memory.Properties.Settings.Default.name3;
            vak_naam4.Content = Memory.Properties.Settings.Default.name4;
            vak_naam5.Content = Memory.Properties.Settings.Default.name5;
            vak_score1.Content = Memory.Properties.Settings.Default.highscore1;
            vak_score2.Content = Memory.Properties.Settings.Default.highscore2;
            vak_score3.Content = Memory.Properties.Settings.Default.highscore3;
            vak_score4.Content = Memory.Properties.Settings.Default.highscore4;
            vak_score5.Content = Memory.Properties.Settings.Default.highscore5;
        }

        private void menuknop_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
