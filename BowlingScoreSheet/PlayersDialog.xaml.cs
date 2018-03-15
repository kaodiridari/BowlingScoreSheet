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
using System.Windows.Shapes;

namespace BowlingScoreSheet
{
    /// <summary>
    /// Interaktionslogik für PlayersDialog.xaml
    /// </summary>
    public partial class PlayersDialog : Window
    {
        public PlayersDialog()
        {
            InitializeComponent();
        }

        private void AddPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            PlayersList.Items.Add(Player.Text);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            //Write to Model
            var li = new List<string>();
            foreach (var item in PlayersList.Items)
            {
                li.Add(item.ToString());
            }
            string[] p = li.ToArray();
            var doc = MyApp.getInstance();
            doc.Players = p;
            //doc.PlayersDialogDone();            
            this.Close();
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            string[] p = {"Peter Handke", "Günther Grass", "Siegfried Lenz", "Heinrich Böll", "Wolfgang Borchert", "Sten Nadolny", "Wolf Haas"};
            p = new string[] { "Elmer Fudd", "Bugs Bunny", "Daffy Duck", "Porky Pig"};
            var doc = MyApp.getInstance();
            doc.Players = p;
            //doc.PlayersDialogDone();
            this.Close();
        }
    }
}
