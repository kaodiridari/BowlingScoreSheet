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
    public class Interval
    {
        private string _start = "Start date yyyy.mm.dd";
        private string _end = "End date yyyy.mm.dd";

        public string Start
        {
            get
            {
                return _start;
            }

            set
            {
                this._start = value;
            }
        }

        public string End
        {
            get
            {
                return _end;
            }

            set
            {
                this._end = value;
            }
        }
    }

    /// <summary>
    /// Interaktionslogik für SearchDialog.xaml
    /// </summary>
    public partial class SearchDialog : Window
    {
        Interval _interval;
        public SearchDialog()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _interval = new Interval();
            this.DataContext = _interval;
        }

        private void StartSearchButton_Click(object sender, RoutedEventArgs e)
        {   
            //
            var startOut = new DateTime();
            bool startOutIsValid = DateTime.TryParse(_interval.Start, out startOut);
            var endOut = new DateTime();
            bool endOutIsValid = DateTime.TryParse(_interval.End, out endOut);

            //try it -- later more service-like  ?Anwendung muß hochlaufen?
            var p = MyApp.getInstance().GetPersistence();
            string[] games = p.Get(startOut, endOut);
            //try it show the json-line
            foreach (var aGame in games)
            {
                Games.Items.Add(aGame);
            }            
        }
    }
}
