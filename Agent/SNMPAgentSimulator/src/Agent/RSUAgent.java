package Agent;

import com.sun.webkit.graphics.WCTextRunImpl;
import org.snmp4j.smi.Integer32;

import java.io.File;
import java.util.ArrayList;
import java.util.Arrays;

/**
 * Created by: Péter Sturm
 * Date: 2019-04-25
 */

public class RSUAgent
{
    public String IP;
    public String Port;
    public String TrapAddress;
    public String TrapUser;
    public String TrapAuth;
    public String TrapPriv;

    public String TelnetIP;
    public String TelnetUser;
    public String TelnetPass;

    ArrayList<MibObject> mibObjects;

    public Agent agent;

    public boolean Running;
    public boolean SendTrap;

    public RSUAgent(String ip, String port, String trapaddress, String trapuser, String trapauth, String trappriv, String telnetip, String telnetuser, String telnetpass)
    {
        IP = ip;
        Port = port;
        TrapAddress = trapaddress;
        TrapUser = trapuser;
        TrapAuth = trapauth;
        TrapPriv = trappriv;
        TelnetIP = telnetip;
        TelnetUser = telnetuser;
        TelnetPass = telnetpass;
        mibObjects = new ArrayList<MibObject>();

        Running = false;
        SendTrap = false;

        if(TelnetIP != null && TelnetIP != "")
        {
            TelnetAPI telnet = new TelnetAPI(TelnetIP, TelnetUser, TelnetPass, "ate>");

            String result;

            result = telnet.executeCommand("stp-print");
            ArrayList<String> infos  = new ArrayList<String>(Arrays.asList(result.split("\r\n")));

            for (String info : infos)
            {
                if (info.contains("**manual-latitude"))
                    mibObjects.add(new MibObject("6"
                            , MibObject.SNMPType.Integer32
                            , Integer.parseInt(info.split("=")[1])));
                if (info.contains("**manual-longitude"))
                    mibObjects.add(new MibObject("7"
                            , MibObject.SNMPType.Integer32
                            , Integer.parseInt(info.split("=")[1])));
                if (info.contains("  manual-altitude"))
                    mibObjects.add(new MibObject("8"
                            , MibObject.SNMPType.Integer32
                            , Integer.parseInt(info.split("=")[1])));
            }

            telnet.disconnect();
        }
    }

    public void startAgent()
    {
        if(!Running)
        {
            agent = new Agent(new File("SNMP4JTestAgentBC" + Port + ".cfg"),
                    new File("SNMP4JTestAgentConfig" + Port + ".cfg"));

            agent.setMibObjects(mibObjects);

            agent.start(IP
                    , Integer.parseInt(Port)
                    , TrapAddress
                    , TrapUser
                    , TrapAuth
                    , TrapPriv);

            System.out.println("Agent " + IP + "/" + Port + " started!");

            Running = true;
        }
    }

    public void stopAgent()
    {
        if(Running)
        {
            agent.stop();
            Running = false;
        }

    }
}
