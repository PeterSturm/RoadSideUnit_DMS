using System;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
using System.Net;
using Lextm.SharpSnmpLib;

namespace SNMPManager.Trap
{
    class Service
    {
        static void Main(string[] args)
        {
            try
            {
                var users = new UserRegistry();  
                users.Add(new OctetString("SecurityUserName"),  
                new DefaultPrivacyProvider(new MD5AuthenticationProvider(new OctetString("AuthenticationPassword"))));
                Listener listener = new Listener { Users = users};
                listener.AddBinding(new IPEndPoint(IPAddress.Any, 162));
                listener.MessageReceived += MessageReceived;
                listener.StartAsync();
                Console.WriteLine("Press to stop...");
                Console.Read();
                listener.Stop();



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static private void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
