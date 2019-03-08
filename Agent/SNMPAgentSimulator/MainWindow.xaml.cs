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
using System.Net;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;

namespace SNMPAgentSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Settings settings = new Settings();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = settings;
        }

        public class Settings
        {
            public Settings()
            {
                port = 162;
                ip = "127.0.0.1";
            }
            public int port { get; set; }
            public IPAddress _ip;
            public string ip {
                get
                {
                    return _ip.ToString();
                }
                set
                {
                    _ip = IPAddress.Parse(value);
                }
            }
        }

        private void SendTrap(object sender, RoutedEventArgs e)
        {
            var trap = new TrapV2Message(
                                          Lextm.SharpSnmpLib.VersionCode.V3,
                                          528732060,
                                          1905687779,
                                          new OctetString("SecurityUserName"),
                                          new ObjectIdentifier("1.3.6"),
                                          0,
                                          new List<Variable>(),
                                          DefaultPrivacyProvider.DefaultPair,
                                          0x10000,
                                          new OctetString(ByteTool.Convert("80001F8880E9630000D61FF449")),
                                          0,
                                          0);
            trap.Send(new IPEndPoint(settings._ip, settings.port));
        }
    }
}
