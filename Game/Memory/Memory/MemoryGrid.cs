using Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace Memory
{
    public class MemoryGrid
    {
        // defines the grid
        private Grid grid;

        // save file path
        private string path;

        // creates 2 players
        public static string Player1 { get; set; }
        public static string Player2 { get; set; }

        // define the number of rows and cols
        private int rows, cols;

        // variable for the number of clicks
        static int numberOfClicks = 0;

        // the total scores which will be displayed to the players
        public static int scoreName1Tot;
        public static int scoreName2Tot;

        // variables to count if there is made a new point, and to make it easier to keep track of the turns
        static int scoreName1;
        static int scoreName2;

        // a variable to count the number of pairs, and to know when the game is over
        static int numberOfPairs;

        // a bool to check if the game needs to wait and if currently waiting
        private bool hasDelay;

        // bools that control the turns
        private bool turnName1 = true;
        private bool turnName2 = false;

        // Images the are displayed
        private Image card;
        private Image Image1;
        private Image Image2;

        // Folder where images are stored
        public static string Folder { get; set; }

        // Bool to check if sound is muted
        public static bool Mute { get; set; }

        /// <summary>
        /// Initialize the grid and assign the images to the grid
        /// </summary>
        /// <param name="grid">Defines the grid</param>
        /// <param name="rows">How many rows there are</param>
        /// <param name="cols">How many cols there are</param>
        public MemoryGrid(Grid grid, int rows, int cols, string path)
        {
            this.grid = grid;
            this.rows = rows;
            this.cols = cols;
            this.path = path;

            InitializeGrid();
            AddImages();
        }

        /// <summary>
        /// Initialises the GameGrid
        /// </summary>
        private void InitializeGrid()
        {
            for (int i = 0; i < rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < cols; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        /// <summary>
        /// Adds images to the grid
        /// </summary>
        private void AddImages()
        {

            List<List<string>> data = GetDataFromFile();

            if (data[0][2] == "SaveReady")
            {
                //Loads the score
                scoreName1Tot = Convert.ToInt32(data[1][0]);
                scoreName2Tot = Convert.ToInt32(data[1][1]);
                UpdateScore();

                //Loads player turn
                if (data[0][3] == "P2")
                {
                    turnName1 = false;
                    turnName2 = true;
                }
                ShowTurn();

                //Loads in player names
                if (data[0][0].Length > 0)
                {
                    Spellenscherm.main.name1.Content = data[0][0];
                    Spellenscherm.main.set1.Visibility = Visibility.Collapsed;
                }
                if (data[0][1].Length > 0)
                {
                    Spellenscherm.main.name2.Content = data[0][1];
                    Spellenscherm.main.set2.Visibility = Visibility.Collapsed;
                }

                //Loads in cards
                List<ImageSource> images = GetImagesList();
                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < cols; col++)
                    {
                        if (data[row + 2][col].Length > 0)
                        {
                            // assign the back of the image
                            Image back = new Image();
                            back.Source = new BitmapImage(new Uri(Folder + "/back.png", UriKind.RelativeOrAbsolute));

                            // when one of the players click on a card
                            back.MouseDown += new System.Windows.Input.MouseButtonEventHandler(CardClick);

                            // set the cards
                            back.Tag = images.First();
                            images.RemoveAt(0);
                            Grid.SetColumn(back, col);
                            Grid.SetRow(back, row);
                            grid.Children.Add(back);
                        }
                    }
                }
                numberOfPairs = Convert.ToInt32(data[1][2]);
            }
            else
            {
                List<ImageSource> images = GetImagesList();
                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < cols; col++)
                    {
                        // assign the back of the image
                        Image back = new Image();
                        back.Source = new BitmapImage(new Uri(Folder + "/back.png", UriKind.RelativeOrAbsolute));

                        // when one of the players click on a card
                        back.MouseDown += new System.Windows.Input.MouseButtonEventHandler(CardClick);

                        // set the cards
                        back.Tag = images.First();
                        images.RemoveAt(0);
                        Grid.SetColumn(back, col);
                        Grid.SetRow(back, row);
                        grid.Children.Add(back);
                    }
                }
            }
        }

        /// <summary>
        /// Give each card a random image
        /// </summary>
        /// <returns>Return the images</returns>
        public List<ImageSource> GetImagesList()
        {
            // a list that holds the images
            List<ImageSource> images = new List<ImageSource>();

            // two lists that keep track of the used images, so there are only 2 cards of the sort
            List<int> placed_images = new List<int>();

            List<string> cells = new List<string>();

            // variables used for saving the grind into a savefile
            for (int i = 0; i < 16; i++)
            {
                if (i < 8)
                {
                    cells.Add(null);
                }
                else
                {
                    cells.Add("");
                }
            }

            List<List<string>> data = GetDataFromFile();

            string delimiter = ";";

            //Places savedata inside variables
            if (data[0][2] == "SaveReady")
            {
                cells[0] = data[2][0];
                cells[1] = data[2][1];
                cells[2] = data[2][2];
                cells[3] = data[2][3];
                cells[4] = data[3][0];
                cells[5] = data[3][1];
                cells[6] = data[3][2];
                cells[7] = data[3][3];
                cells[8] = data[4][0];
                cells[9] = data[4][1];
                cells[10] = data[4][2];
                cells[11] = data[4][3];
                cells[12] = data[5][0];
                cells[13] = data[5][1];
                cells[14] = data[5][2];
                cells[15] = data[5][3];

                //Places cards in the right position and skips the ones already gone
                for (int i = 0; i < 16; i++)
                {
                    if(cells[i].Length > 0)
                    {
                        ImageSource source = new BitmapImage(new Uri(Folder + "/" + cells[i] + ".png", UriKind.RelativeOrAbsolute));
                        images.Add(source);
                    }
                }
            }
            else
            {
                // randomizer
                for (int i = 0; i < 16; i++)
                {
                    // generate a random int between 1 and 8
                    Random rnd = new Random();

                    // a variable that represents the image that is going to be used
                    int imageNR = rnd.Next(1, 9);

                    var dic = new Dictionary<double, int>();
                    foreach (var imageNumber in placed_images)
                    {
                        if (dic.ContainsKey(imageNumber))
                        {
                            dic[imageNumber]++;
                        }
                        else
                        {
                            dic[imageNumber] = 1;
                        }
                    }

                    bool tooMany = false;
                    foreach (KeyValuePair<double, int> element in dic)
                    {
                        if (imageNR == element.Key && element.Value >= 2)
                        {
                            tooMany = true;
                        }
                    }

                    // if the genrated number already exists (in the list 'random1'), generate a new number
                    if (tooMany)
                    {
                        i--;
                    }
                    // if the generated number does not exists (in the list 'random1'), grab the image with that number
                    else
                    {
                        placed_images.Add(imageNR);
                        ImageSource source = new BitmapImage(new Uri(Folder + "/" + imageNR + ".png", UriKind.RelativeOrAbsolute));
                        images.Add(source);

                        cells[i] = Convert.ToString(imageNR);
                    }
                }
            }
            if (data[0][2] == "SaveReady")
            {
                //writes the variables into the savefile
                File.WriteAllText(path, data[0][0] + delimiter + data[0][1] + delimiter + "SaveReady" + delimiter + data[0][3] + Environment.NewLine + data[1][0] + delimiter + data[1][1] + delimiter + data[1][2] + delimiter + data[1][3] + Environment.NewLine + cells[0] + delimiter + cells[1] + delimiter + cells[2] + delimiter + cells[3] + Environment.NewLine + cells[4] + delimiter + cells[5] + delimiter + cells[6] + delimiter + cells[7] + Environment.NewLine + cells[8] + delimiter + cells[9] + delimiter + cells[10] + delimiter + cells[11] + Environment.NewLine + cells[12] + delimiter + cells[13] + delimiter + cells[14] + delimiter + cells[15] + Environment.NewLine);
            }
            else
            {
                //writes the variables into the savefile
                File.WriteAllText(path, data[0][0] + delimiter + data[0][1] + delimiter + "SaveReady" + delimiter + data[0][3] + Environment.NewLine + "0" + delimiter + "0" + delimiter + 0 + delimiter + data[1][3] + Environment.NewLine + cells[0] + delimiter + cells[1] + delimiter + cells[2] + delimiter + cells[3] + Environment.NewLine + cells[4] + delimiter + cells[5] + delimiter + cells[6] + delimiter + cells[7] + Environment.NewLine + cells[8] + delimiter + cells[9] + delimiter + cells[10] + delimiter + cells[11] + Environment.NewLine + cells[12] + delimiter + cells[13] + delimiter + cells[14] + delimiter + cells[15] + Environment.NewLine);
            }
            return images;
        }

        /// <summary>
        /// GetDataFromFile
        /// </summary>
        /// <returns></returns>
        private List<List<string>> GetDataFromFile()
        {
            StreamReader reader = new StreamReader(File.OpenRead(path));
            List<List<string>> data = new List<List<string>>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(';');

                data.Add(new List<string> { values[0], values[1], values[2], values[3] });
            }
            reader.Close();
            return data;
        }

        /// <summary>
        /// Turns the card when clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardClick(object sender, MouseButtonEventArgs e)
        {
            // wait with showing the card if this is true, the previous turn needs to finish first
            if (hasDelay)
            {
                return;
            }

            // if the game does not have to wait, show the card
            Image card = (Image)sender;
            ImageSource front = (ImageSource)card.Tag;
            card.Source = front;
            numberOfClicks++;

            CheckCards(card);
        }

        /// <summary>
        /// Shows who's turn it is under the players name
        /// </summary>
        private void ShowTurn()
        {
            // if its player1's turn, show 'Aan de beurt' under their name
            if (turnName1)
            {
                Spellenscherm.main.SetTurn1 = "Aan de beurt";
                Spellenscherm.main.SetTurn2 = "";
            }
            // if its player2's turn, show 'Aan de beurt' under their name
            else if (turnName2)
            {
                Spellenscherm.main.SetTurn1 = "";
                Spellenscherm.main.SetTurn2 = "Aan de beurt";
            }
        }

        /// <summary>
        /// Grab the clicked card
        /// </summary>
        /// <param name="card">The card that has been clicked</param>
        private void CheckCards(Image card)
        {
            this.card = card;

            // card the cards that have been clicked
            if (numberOfClicks <= 2)
            {
                if (Image1 == null)
                {
                    Image1 = card;
                }
                else if (Image2 == null)
                {
                    Image2 = card;
                }
            }

            // when to cards have been clicked, check if they are a pair
            if (numberOfClicks == 2)
            {
                // if the same image is not clicked
                if (Image1 != Image2)
                {
                    CheckPair(Image1, Image2);

                    // reset the variables for the next turn
                    numberOfClicks = 0;
                    Image1 = null;
                    Image2 = null;
                }
                else
                {
                    numberOfClicks--;
                    Image2 = null;
                }
            }
        }

        /// <summary>
        /// Check if the two clicked cards have the same image (source)
        /// </summary>
        /// <param name="card1">The first card that has been clicked</param>
        /// <param name="card2">The second card that has been clicked</param>
        public void CheckPair(Image card1, Image card2)
        {
            Image1 = card1;
            Image2 = card2;

            // if 2 images are clicked, there are the same and the same card is not clicked twice
            if (Convert.ToString(card1.Source) == Convert.ToString(card2.Source) && (card1 != card2))
            {
                PlaySound("pair");
                GetPoint(card1, card2);

                string cardnr = null;

                List<List<string>> data = GetDataFromFile();

                //turns the card's name into a number
                for (int b = 1; b < 9; b++)
                {
                    if (Convert.ToString(card1.Source).Contains(b + ".png"))
                    {
                        cardnr = Convert.ToString(b);
                    }
                }

                //removes the 2 clicked cards from the savefile
                string delimiter = ";";
                int i;
                int x;
                for (i = 2; i < 6; i++)
                {
                    for (x = 0; x < 4; x++)
                    {
                        if (data[i][x] == cardnr)
                        {
                            data[i][x] = null;
                        }
                    }
                }

                File.WriteAllText(path, data[0][0] + delimiter + data[0][1] + delimiter + data[0][2] + delimiter + data[0][3] + Environment.NewLine + data[1][0] + delimiter + data[1][1] + delimiter + data[1][2] + delimiter + data[1][3] + Environment.NewLine + data[2][0] + delimiter + data[2][1] + delimiter + data[2][2] + delimiter + data[2][3] + Environment.NewLine + data[3][0] + delimiter + data[3][1] + delimiter + data[3][2] + delimiter + data[3][3] + Environment.NewLine + data[4][0] + delimiter + data[4][1] + delimiter + data[4][2] + delimiter + data[4][3] + Environment.NewLine + data[5][0] + delimiter + data[5][1] + delimiter + data[5][2] + delimiter + data[5][3] + Environment.NewLine);
            }
            // if 2 images are clicked, they are not the same and the same card is not clicked twice
            else
            {
                PlaySound("fail");
                ResetCards(Image1, Image2);
            }

            CheckTurn();

            // if the same card is clicked twice, the player keeps their turn
            if (Convert.ToString(card1.Source) == Convert.ToString(card2.Source) && (card1 == card2))
            {
                PlaySound("huh");
                StayTurn();
            }

            UpdateScore();

            // if all the pair have been found
            if (numberOfPairs == 8)
            {
                CheckWinner();
            }

            // reset the variables for the next turn
            scoreName1 = 0;
            scoreName2 = 0;
            ShowTurn();
        }

        /// <summary>
        /// Gives turns
        /// </summary>
        private void CheckTurn()
        {
            string delimiter = ";";

            List<List<string>> data = GetDataFromFile();

            // check if its player1's turn
            if (turnName1)
            {
                // if player 1 has a point, they keep their turn and their score increases with one
                if (scoreName1 == 1)
                {
                    scoreName1Tot = scoreName1 + scoreName1Tot;
                }
                // if player 1 does not have a point, they lose their turn and their score stays the same
                else if (scoreName1 == 0)
                {
                    turnName1 = false;
                    turnName2 = true;

                    File.WriteAllText(path, data[0][0] + delimiter + data[0][1] + delimiter + data[0][2] + delimiter + "P2" + Environment.NewLine + data[1][0] + delimiter + data[1][1] + delimiter + data[1][2] + delimiter + data[1][3] + Environment.NewLine + data[2][0] + delimiter + data[2][1] + delimiter + data[2][2] + delimiter + data[2][3] + Environment.NewLine + data[3][0] + delimiter + data[3][1] + delimiter + data[3][2] + delimiter + data[3][3] + Environment.NewLine + data[4][0] + delimiter + data[4][1] + delimiter + data[4][2] + delimiter + data[4][3] + Environment.NewLine + data[5][0] + delimiter + data[5][1] + delimiter + data[5][2] + delimiter + data[5][3] + Environment.NewLine);
                }
            }
            // check if its player2's turn
            else if (turnName2)
            {
                // if player 2 has a point, they keep their turn and their score increases with one
                if (scoreName2 == 1)
                {
                    scoreName2Tot = scoreName2 + scoreName2Tot;
                }
                // if player 2 does not have a point, they lose their turn and their score stays the same
                else if (scoreName2 == 0)
                {
                    turnName2 = false;
                    turnName1 = true;

                    File.WriteAllText(path, data[0][0] + delimiter + data[0][1] + delimiter + data[0][2] + delimiter + "P1" + Environment.NewLine + data[1][0] + delimiter + data[1][1] + delimiter + data[1][2] + delimiter + data[1][3] + Environment.NewLine + data[2][0] + delimiter + data[2][1] + delimiter + data[2][2] + delimiter + data[2][3] + Environment.NewLine + data[3][0] + delimiter + data[3][1] + delimiter + data[3][2] + delimiter + data[3][3] + Environment.NewLine + data[4][0] + delimiter + data[4][1] + delimiter + data[4][2] + delimiter + data[4][3] + Environment.NewLine + data[5][0] + delimiter + data[5][1] + delimiter + data[5][2] + delimiter + data[5][3] + Environment.NewLine);
                }
            }
        }

        /// <summary>
        /// When the same card is doubleclicked, the turn is passed on
        /// </summary>
        private void StayTurn()
        {
            // check if its player1's turn, give the turn to player 2
            if (turnName2 == true)
            {
                turnName1 = false;
                turnName2 = true;
            }
            // check if its player2's turn and give the turn to player 1
            else if (turnName2 == false)
            {
                turnName1 = true;
                turnName2 = false;
            }
        }

        /// <summary>
        /// Updates Score Labels
        /// </summary>
        private void UpdateScore()
        {
            Spellenscherm.main.Score1 = "Score: " + scoreName1Tot;
            Spellenscherm.main.Score2 = "Score: " + scoreName2Tot;
        }

        /// <summary>
        /// Give points and remove pairs from GameGrid
        /// </summary>
        /// <param name="card1">The first card that has been clicked</param>
        /// <param name="card2">The second card that has been clicked</param>
        private async void GetPoint(Image card1, Image card2)
        {
            string delimiter = ";";

            List<List<string>> data = GetDataFromFile();

            // if its player1's turn, increase their score
            if (turnName1)
            {
                numberOfPairs++;
                scoreName1++;

                data[1][0] = Convert.ToString(Convert.ToInt32(data[1][0]) + 1);

                File.WriteAllText(path, data[0][0] + delimiter + data[0][1] + delimiter + data[0][2] + delimiter + data[0][3] + Environment.NewLine + data[1][0] + delimiter + data[1][1] + delimiter + numberOfPairs + delimiter + data[1][3] + Environment.NewLine + data[2][0] + delimiter + data[2][1] + delimiter + data[2][2] + delimiter + data[2][3] + Environment.NewLine + data[3][0] + delimiter + data[3][1] + delimiter + data[3][2] + delimiter + data[3][3] + Environment.NewLine + data[4][0] + delimiter + data[4][1] + delimiter + data[4][2] + delimiter + data[4][3] + Environment.NewLine + data[5][0] + delimiter + data[5][1] + delimiter + data[5][2] + delimiter + data[5][3] + Environment.NewLine);
            }
            // if its player2's turn, increase their score
            else if (turnName2)
            {
                numberOfPairs++;
                scoreName2++;

                data[1][1] = Convert.ToString(Convert.ToInt32(data[1][1]) + 1);

                File.WriteAllText(path, data[0][0] + delimiter + data[0][1] + delimiter + data[0][2] + delimiter + data[0][3] + Environment.NewLine + data[1][0] + delimiter + data[1][1] + delimiter + numberOfPairs + delimiter + data[1][3] + Environment.NewLine + data[2][0] + delimiter + data[2][1] + delimiter + data[2][2] + delimiter + data[2][3] + Environment.NewLine + data[3][0] + delimiter + data[3][1] + delimiter + data[3][2] + delimiter + data[3][3] + Environment.NewLine + data[4][0] + delimiter + data[4][1] + delimiter + data[4][2] + delimiter + data[4][3] + Environment.NewLine + data[5][0] + delimiter + data[5][1] + delimiter + data[5][2] + delimiter + data[5][3] + Environment.NewLine);
            }

            // wait a third of a second, show the second card first
            hasDelay = true;
            await Task.Delay(300);

            // remove the cards from the board
            card1.Source = new BitmapImage(new Uri("", UriKind.RelativeOrAbsolute));
            card2.Source = new BitmapImage(new Uri("", UriKind.RelativeOrAbsolute));

            hasDelay = false;
        }

        /// <summary>
        /// Reset the cards
        /// </summary>
        /// <param name="card1">The first card that has been clicked</param>
        /// <param name="card2">The second card that has been clicked</param>
        private async void ResetCards(Image card1, Image card2)
        {
            Image1 = card1;
            Image2 = card2;

            // wait a second, show the card first before showing its back again
            hasDelay = true;
            await Task.Delay(1000);

            // show the back of the card again.
            card1.Source = new BitmapImage(new Uri(Folder + "/back.png", UriKind.RelativeOrAbsolute));
            card2.Source = new BitmapImage(new Uri(Folder + "/back.png", UriKind.RelativeOrAbsolute));
            hasDelay = false;
        }

        /// <summary>
        /// Announce the winner of the game
        /// </summary>
        private void CheckWinner()
        {
            SetHighscore();

            // when the scores of player1 and player2 are the same
            if (scoreName1Tot == scoreName2Tot)
            {
                if (Mute == false)
                {

                    Stream str = Properties.Resources.even;
                    System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
                    snd.Play();
                }
                MessageBox.Show("Gelijkspel!");
            }
            // if the scores of player1 and player2 are not the same, announce the winner, who is the player with the most points
            else
            {
                if (Mute == false)
                {

                    Stream str = Properties.Resources.win;
                    System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
                    snd.Play();
                }
                string winner = (scoreName1Tot > scoreName2Tot) ? Player1 : Player2;
                MessageBox.Show(winner + " heeft gewonnen!");
            }
            scoreName1Tot = 0;
            scoreName2Tot = 0;
            numberOfPairs = 0;
            string delimiter = ";";
            File.WriteAllText(path, delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter + Environment.NewLine + delimiter + delimiter + delimiter);
        }

        /// <summary>
        /// Play a soundeffect
        /// </summary>
        private void PlaySound(string mood)
        {
            if (Mute == false)
            {
                Stream str = null;
                switch (mood)
                {
                    case "pair":
                        str = Properties.Resources.pair;
                        break;
                    case "fail":
                        str = Properties.Resources.fail;
                        break;
                    case "huh":
                        str = Properties.Resources.huh;
                        break;
                    default:
                        break;
                }

                try
                {
                    System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
                    snd.Play();
                }
                catch(Exception e)
                {
                    Console.WriteLine("Cannot find sound", e);
                }
            }
        }

        /// <summary>
        /// Return the scores and playernames and put them on the scoreboard
        /// </summary>
        private void SetHighscore()
        {
            // grad the current highscores
            int OldHighscore1 = Memory.Properties.Settings.Default.highscore1;
            int OldHighscore2 = Memory.Properties.Settings.Default.highscore2;
            int OldHighscore3 = Memory.Properties.Settings.Default.highscore3;
            int OldHighscore4 = Memory.Properties.Settings.Default.highscore4;
            int OldHighscore5 = Memory.Properties.Settings.Default.highscore5;

            // rearrange the highscore board, use player 1
            if (scoreName1Tot > OldHighscore1 && scoreName1Tot != OldHighscore1)
            {
                Memory.Properties.Settings.Default.highscore2 = Memory.Properties.Settings.Default.highscore1;
                Memory.Properties.Settings.Default.highscore3 = Memory.Properties.Settings.Default.highscore2;
                Memory.Properties.Settings.Default.highscore4 = Memory.Properties.Settings.Default.highscore3;
                Memory.Properties.Settings.Default.highscore5 = Memory.Properties.Settings.Default.highscore4;

                Memory.Properties.Settings.Default.name2 = Memory.Properties.Settings.Default.name1;
                Memory.Properties.Settings.Default.name3 = Memory.Properties.Settings.Default.name2;
                Memory.Properties.Settings.Default.name4 = Memory.Properties.Settings.Default.name3;
                Memory.Properties.Settings.Default.name5 = Memory.Properties.Settings.Default.name4;

                Memory.Properties.Settings.Default.highscore1 = scoreName1Tot;
                Memory.Properties.Settings.Default.name1 = Player1;
            }
            else if (scoreName1Tot > OldHighscore2 && scoreName1Tot != OldHighscore2)
            {
                Memory.Properties.Settings.Default.highscore3 = Memory.Properties.Settings.Default.highscore2;
                Memory.Properties.Settings.Default.highscore4 = Memory.Properties.Settings.Default.highscore3;
                Memory.Properties.Settings.Default.highscore5 = Memory.Properties.Settings.Default.highscore4;

                Memory.Properties.Settings.Default.name3 = Memory.Properties.Settings.Default.name2;
                Memory.Properties.Settings.Default.name4 = Memory.Properties.Settings.Default.name3;
                Memory.Properties.Settings.Default.name5 = Memory.Properties.Settings.Default.name4;

                Memory.Properties.Settings.Default.highscore2 = scoreName1Tot;
                Memory.Properties.Settings.Default.name2 = Player1;
            }
            else if (scoreName1Tot > OldHighscore3 && scoreName1Tot != OldHighscore3)
            {
                Memory.Properties.Settings.Default.highscore4 = Memory.Properties.Settings.Default.highscore3;
                Memory.Properties.Settings.Default.highscore5 = Memory.Properties.Settings.Default.highscore4;

                Memory.Properties.Settings.Default.name4 = Memory.Properties.Settings.Default.name3;
                Memory.Properties.Settings.Default.name5 = Memory.Properties.Settings.Default.name4;

                Memory.Properties.Settings.Default.highscore3 = scoreName1Tot;
                Memory.Properties.Settings.Default.name3 = Player1;
            }
            else if (scoreName1Tot > OldHighscore4 && scoreName1Tot != OldHighscore4)
            {
                Memory.Properties.Settings.Default.highscore5 = Memory.Properties.Settings.Default.highscore4;

                Memory.Properties.Settings.Default.name5 = Memory.Properties.Settings.Default.name4;

                Memory.Properties.Settings.Default.highscore4 = scoreName1Tot;
                Memory.Properties.Settings.Default.name4 = Player1;
            }
            else if (scoreName1Tot > OldHighscore5 && scoreName1Tot != OldHighscore5)
            {
                Memory.Properties.Settings.Default.highscore5 = scoreName1Tot;
                Memory.Properties.Settings.Default.name5 = Player1;
            }
            // rearrange the highscore board, use player 2
            if (scoreName2Tot > OldHighscore1 && scoreName2Tot != OldHighscore1)
            {
                Memory.Properties.Settings.Default.highscore2 = Memory.Properties.Settings.Default.highscore1;
                Memory.Properties.Settings.Default.highscore3 = Memory.Properties.Settings.Default.highscore2;
                Memory.Properties.Settings.Default.highscore4 = Memory.Properties.Settings.Default.highscore3;
                Memory.Properties.Settings.Default.highscore5 = Memory.Properties.Settings.Default.highscore4;

                Memory.Properties.Settings.Default.name2 = Memory.Properties.Settings.Default.name1;
                Memory.Properties.Settings.Default.name3 = Memory.Properties.Settings.Default.name2;
                Memory.Properties.Settings.Default.name4 = Memory.Properties.Settings.Default.name3;
                Memory.Properties.Settings.Default.name5 = Memory.Properties.Settings.Default.name4;

                Memory.Properties.Settings.Default.highscore1 = scoreName2Tot;
                Memory.Properties.Settings.Default.name1 = Player2;
            }
            else if (scoreName2Tot > OldHighscore2 && scoreName2Tot != OldHighscore2)
            {
                Memory.Properties.Settings.Default.highscore3 = Memory.Properties.Settings.Default.highscore2;
                Memory.Properties.Settings.Default.highscore4 = Memory.Properties.Settings.Default.highscore3;
                Memory.Properties.Settings.Default.highscore5 = Memory.Properties.Settings.Default.highscore4;

                Memory.Properties.Settings.Default.name3 = Memory.Properties.Settings.Default.name2;
                Memory.Properties.Settings.Default.name4 = Memory.Properties.Settings.Default.name3;
                Memory.Properties.Settings.Default.name5 = Memory.Properties.Settings.Default.name4;

                Memory.Properties.Settings.Default.highscore2 = scoreName2Tot;
                Memory.Properties.Settings.Default.name2 = Player2;
            }
            else if (scoreName2Tot > OldHighscore3 && scoreName2Tot != OldHighscore3)
            {
                Memory.Properties.Settings.Default.highscore4 = Memory.Properties.Settings.Default.highscore3;
                Memory.Properties.Settings.Default.highscore5 = Memory.Properties.Settings.Default.highscore4;

                Memory.Properties.Settings.Default.name4 = Memory.Properties.Settings.Default.name3;
                Memory.Properties.Settings.Default.name5 = Memory.Properties.Settings.Default.name4;

                Memory.Properties.Settings.Default.highscore3 = scoreName2Tot;
                Memory.Properties.Settings.Default.name3 = Player2;
            }
            else if (scoreName2Tot > OldHighscore4 && scoreName2Tot != OldHighscore4)
            {
                Memory.Properties.Settings.Default.highscore5 = Memory.Properties.Settings.Default.highscore4;

                Memory.Properties.Settings.Default.name5 = Memory.Properties.Settings.Default.name4;

                Memory.Properties.Settings.Default.highscore4 = scoreName2Tot;
                Memory.Properties.Settings.Default.name4 = Player2;
            }
            else if (scoreName2Tot > OldHighscore5 && scoreName2Tot != OldHighscore5)
            {
                Memory.Properties.Settings.Default.highscore5 = scoreName2Tot;
                Memory.Properties.Settings.Default.name5 = Player2;
            }
            Memory.Properties.Settings.Default.Save();
            Memory.Properties.Settings.Default.Reload();
        }
    }
}
