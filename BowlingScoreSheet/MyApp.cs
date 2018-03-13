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

        /// <summary>
        /// Set the config file; It is in the Folder SolutionItems.
        /// </summary>
        /// <param name="file">Just the name: eg config.xml</param>
        void SetConfigFile(string file);
    }

    public class MyApp : IMyApp
    {
        private static MyApp me;

        private XPathNavigator xpathnavigator;        

        private IPersistence p;
        private string m_configFile;

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

        public IPersistence GetPersistence()
        {
            return p;
        }

        //public void Save(string json)
        //{
        //    p.Save(json);
        //}

        public string GetConfig(string xpath)
        {
            if (xpathnavigator == null)
            {
                xpathnavigator = ThisAndThat.LoadConfigFile(m_configFile);   
            }
            return xpathnavigator.SelectSingleNode(xpath).Value;             
        }

        public void SetConfigFile(string file)
        {
            m_configFile = file;
        }
    }
}
