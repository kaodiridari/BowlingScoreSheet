using System.Windows;
using System.Reflection;
using System.Text;
using System;
using System.Collections.ObjectModel;

namespace BowlingScoreSheet
{
    /// <summary>
    /// The Bowling Score Sheet dialog.
    /// </summary>
    public partial class BowlingDialog : Window, IBowlingDialog
    {
        private BowlingDialogModel m_bowlingDialogModel;
        private BowlingDialogControler m_bowlingDialogControler;
        private BowlingScoreControlModel[] m_BowlingScoreControlModels;
        private BowlingScoreControlControler[] m_BowlingScoreControlControlers;
        //private string[] players;

        ObservableCollection<BowlingScoreControlModel> items = new ObservableCollection<BowlingScoreControlModel>();

        public BowlingDialog()
        {
            InitializeComponent();
            new ConfigBowlingDialog(this, MyApp.getInstance());
            this.DataContext = m_bowlingDialogModel;

            for (int i = 0; i < m_BowlingScoreControlModels.Length; i++)
            {
                items.Add(m_BowlingScoreControlModels[i]);
            } 
            bowlingScoreListBox.ItemsSource = items;
        }



        #region setters
        public void SetBowlingDialogControler(BowlingDialogControler bowlingDialogControler)
        {
            m_bowlingDialogControler = bowlingDialogControler;
        }

        public void SetBowlingDialogModel(BowlingDialogModel bowlingDialogModel)
        {
            m_bowlingDialogModel = bowlingDialogModel;
        }

        public void SetBowlingScoreControlControlers(BowlingScoreControlControler[] bowlingScoreControlControlers)
        {
            m_BowlingScoreControlControlers = bowlingScoreControlControlers;
        }

        public void SetBowlingScoreControlModels(BowlingScoreControlModel[] bowlingScoreControlModels)
        {
            m_BowlingScoreControlModels = bowlingScoreControlModels;
        }

        public void SetPlayers(string[] players)
        {

        }
        #endregion

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

            //zum probieren von liste auskommentiert; m_bowlingDialogControler.Clear();
            
        }

        private void rules(int i)
        {
            m_bowlingDialogControler.JustAnotherBallThrown(i);
        }

        private void lb_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int i = bowlingScoreListBox.SelectedIndex;
            BowlingScoreControlModel item = (BowlingScoreControlModel)bowlingScoreListBox.SelectedItem;
            m_bowlingDialogModel.ActiveBowlingScoreControl = item.PlayersInitials;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            m_bowlingDialogControler.Save();
        }
    }
}
