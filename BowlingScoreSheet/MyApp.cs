using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.XPath;

namespace BowlingScoreSheet
{
    public interface IMyApp
    {
        string[] Players { get; set; }

        void SetPersistence(IPersistence p);
    }

    public class MyApp : IMyApp
    {
        private static MyApp me;

        private XPathNavigator m_config;        

        private IPersistence p;

        private MyApp()
        {
        }

        public static MyApp getInstance()
        {
            if (me == null)
            {
                me = new MyApp();
            }

            return me;
        }

        public string[] Players{get; set;}

        public void PlayersDialogDone()
        {
            //Open the BowlingDialog
            var bd = new BowlingDialog();
            bd.Show(); ;
        }

        public void SetPersistence(IPersistence p)
        {
            this.p = p;
        }

        public void Save(string json)
        {
            p.Save(json);
        }

        internal string GetConfig(string xpath)
        {
            if (m_config == null)
            {
                m_config = ThisAndThat.LoadConfigFile();   
            }
            return m_config.SelectSingleNode(xpath).Value;             
        }
    }
}
