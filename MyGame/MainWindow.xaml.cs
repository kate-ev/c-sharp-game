using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // 1. Neobligātais Mājasdarbs kursā DatZ1091 : Lietotņu izstrāde .NET vidē
        // Mājasdarba autors: Jekaterina Jevtejeva
        // Studenta apliecības numurs: jj19021
        // WPF lietotne pabeigta: 05/01/2021

        // Es esmu izveidojusi divas spēles: Memory Training Game un "Don't Get Hit!" spēli.
        // Veidojot atmiņas spēli, es gribēju uzlabot savas zināšanas un prasmes par WPF lietotņu kontroļiem,
        // savukārt, veidojot "Don't Get Hit!" spēli, koncentrējos uz dinamikas, objektu pozīcijām, dināmisku objektu izveidi;
        // kā arī gribēju pamēģināt izmantot taimerus (abās spēlēs) WPF lietotnē. 

        // privātie lauki spēļu instrukcijām 
        private string memoryGameInstructions = "There are 8 pairs of symbols hidden in the grid." + Environment.NewLine +
            "Your task is to match them all!" + Environment.NewLine + "HOW TO PLAY: Click any of the 16 icons to see its content." 
            + Environment.NewLine + "It will remain open until you click another one. " +
            "If the two icons you've opened do match, they both will remain open. If they do not match, they both will close. " 
            + Environment.NewLine + "Game is finished when all pairs are matched.";

        private string dontGetHitInstructions = "Your task is to avoid getting hit by the yellow falling objects." + Environment.NewLine +
            "HOW TO PLAY: Use right and left arrow keys to move." + Environment.NewLine +
            "Once you have been hit, the game is over. To win the game, you have to gain the score of 50.";
        public MainWindow()
        {
            InitializeComponent();
        }

        // notikums: pogas "Play!" noklikšķināšana (spēle: Memory Training Game)
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Game game = new Game();
            game.Show();
        }

        // notikums: pogas "Play!" noklikšķināšana (spēle: Don't Get Hit!)
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Game2 game = new Game2();
            game.Show();
        }

        // instrukcijas atmiņas spēlei
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(memoryGameInstructions);
        }

        // instrukcijas Don't Get Hit spēlei
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(dontGetHitInstructions);
        }
    }
}
