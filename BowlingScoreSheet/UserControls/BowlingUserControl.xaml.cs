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

namespace BowlingScoreSheet
{
    /// <summary>
    /// Interaktionslogik für BowlingScoreControl.xaml
    /// </summary>
    public partial class BowlingScoreControl : UserControl
    {
        private IBowlingScoreControlModel m_bowlingScoreControlModel;

        public BowlingScoreControl()
        {
            //wo kommt model her?
            InitializeComponent();
        }

        //public BowlingScoreControl(IBowlingScoreControlModel m)
        //{
        //    DataContext = m;
        //    m_bowlingScoreControlModel = m;
        //    InitializeComponent();
        //}
    }
}
