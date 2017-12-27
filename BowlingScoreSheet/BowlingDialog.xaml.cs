using System.Windows;
using System.Reflection;
using System.Text;

namespace BowlingScoreSheet
{
    /// <summary>
    /// The Bowling Score Sheet dialog.
    /// </summary>
    public partial class BowlingDialog : Window
    {
        private BowlingDialogModel m_bowlingDialogModel;
        private BowlingDialogControler m_bowlingDialogControler;
        //private BowlingScoreController bowling;

        public BowlingDialog()
        {
            InitializeComponent();
            var tmp = new string[] {"Peter Müller", "Heiner Müller", "Üzgül Özalü"};
            
            m_bowlingDialogModel = new BowlingDialogModel(tmp);
            
            //Create Controlers for the submodels.
            TODO
            m_bowlingDialogControler = new BowlingDialogControler(m_bowlingDialogModel);

            this.DataContext = m_bowlingDialogModel;            
        }

        private void Button0_Click(object sender, RoutedEventArgs e)
        {
            rules(0);
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            rules(1);
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            rules(2);
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            rules(3);
        }           

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            rules(4);
        }
        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            rules(5);
        }

        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            rules(6);
        }

        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            rules(7);
        }

        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            rules(8);
        }

        private void Button9_Click(object sender, RoutedEventArgs e)
        {
            rules(9);
        }

        private void Button10_Click(object sender, RoutedEventArgs e)
        {
            rules(10);
        }        

        /// <summary>
        /// Clears all Controls -> new game starts. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            m_bowlingDialogControler.Clear();
        }

        private void rules(int i)
        {
            m_bowlingDialogControler.JustAnotherBallThrown(i);
        }
    }
}
