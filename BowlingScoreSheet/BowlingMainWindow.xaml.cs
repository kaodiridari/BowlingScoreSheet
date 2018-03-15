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
    /// Interaktionslogik für BowlingMainWindow.xaml
    /// </summary>
    public partial class BowlingMainWindow : Window
    {
        public BowlingMainWindow()
        {
            InitializeComponent();
            var myApp = MyApp.getInstance();
            myApp.SetConfigFile("config.xml");
            myApp.SetPersistence(new Mongo());
        }

        private void SearchIntervalCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SearchIntervalCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //Open
            var win = new SearchDialog();
            win.ShowDialog();
        } 
    }

    public static class CustomCommands
    {
        public static readonly RoutedUICommand SearchInterval = new RoutedUICommand
            (
                    "SearchInterval",
                    "SearchInterval",
                    typeof(CustomCommands),
                    new InputGestureCollection()
                    {
                                    new KeyGesture(Key.F1)
                    }
            );

        //Define more commands here, just like the one above
    }
}
