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

    public boolean Real;
    public boolean Running;
    public boolean SendTrap;

    public RSUAgent(String ip, String port, String trapaddress, String trapuser, String trapauth, String trappriv, double lat, double lon, double elv, int freqdef, int freqsec, int bandwdef, int bandwsec, String[] modules) {
        IP = ip;
        Port = port;
        TrapAddress = trapaddress;
        TrapUser = trapuser;
        TrapAuth = trapauth;
        TrapPriv = trappriv;

        mibObjects = new ArrayList<MibObject>();
        //GPS
        mibObjects.add(new MibObject("6", MibObject.SNMPType.Integer32, (int)(lat * 1000000)));
        mibObjects.add(new MibObject("7", MibObject.SNMPType.Integer32, (int)(lon * 1000000)));
        mibObjects.add(new MibObject("8", MibObject.SNMPType.Integer32, (int)(elv * 1000000)));
        //Frequency
        mibObjects.add(new MibObject("9", MibObject.SNMPType.Integer32, freqdef));
        mibObjects.add(new MibObject("10", MibObject.SNMPType.Integer32, freqsec));
        //Bandwidth
        mibObjects.add(new MibObject("13", MibObject.SNMPType.Integer32, bandwdef));
        mibObjects.add(new MibObject("14", MibObject.SNMPType.Integer32, bandwsec));
        //Modules
        int i = 15;
        for (String module : modules)
            mibObjects.add(new MibObject(String.format("%d", i++), MibObject.SNMPType.OctetString, module));

        Running = false;
        SendTrap = false;
        Real = false;
    }

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
            Real = true;

            TelnetAPI telnet = new TelnetAPI(TelnetIP, TelnetUser, TelnetPass, "ate>");

            String result;

            result = telnet.executeCommand("stp-print");
            ArrayList<String> infos  = new ArrayList<String>(Arrays.asList(result.split("\r\n")));

            for (String info : infos)
            {
                // GPS
                if (info.contains("**manual-latitude"))
                    mibObjects.add(new MibObject("6"
                            , MibObject.SNMPType.Integer32
                            , Integer.parseInt(info.split("=")[1])));
                else if (info.contains("**manual-longitude"))
                    mibObjects.add(new MibObject("7"
                            , MibObject.SNMPType.Integer32
                            , Integer.parseInt(info.split("=")[1])));
                else if (info.contains("  manual-altitude"))
                    mibObjects.add(new MibObject("8"
                            , MibObject.SNMPType.Integer32
                            , Integer.parseInt(info.split("=")[1])));
                // Frequencies
                else if (info.contains(("  if1-frequency")))
                    mibObjects.add(new MibObject("9"
                            , MibObject.SNMPType.Integer32
                            , Integer.parseInt(info.split("=")[1])));
                else if (info.contains(("  if2-frequency")))
                    mibObjects.add(new MibObject("10"
                            , MibObject.SNMPType.Integer32
                            , Integer.parseInt(info.split("=")[1])));
                // Bandwidth
                else if (info.contains(("  if1-if3-bandwidth")))
                    mibObjects.add(new MibObject("13"
                            , MibObject.SNMPType.Integer32
                            , Integer.parseInt(info.split("=")[1])));
                else if (info.contains(("  if2-if4-bandwidth")))
                    mibObjects.add(new MibObject("14"
                            , MibObject.SNMPType.Integer32
                            , Integer.parseInt(info.split("=")[1])));
                // Modules
                else if (info.contains(("  module-cam")))
                    mibObjects.add(new MibObject("15"
                            , MibObject.SNMPType.OctetString
                            , Integer.parseInt(info.split("=")[1])));
                else if (info.contains(("  module-denm")))
                    mibObjects.add(new MibObject("16"
                            , MibObject.SNMPType.OctetString
                            , Integer.parseInt(info.split("=")[1])));
                else if (info.contains(("  module-map")))
                    mibObjects.add(new MibObject("17"
                            , MibObject.SNMPType.OctetString
                            , Integer.parseInt(info.split("=")[1])));
                else if (info.contains(("  module-spat")))
                    mibObjects.add(new MibObject("18"
                            , MibObject.SNMPType.OctetString
                            , Integer.parseInt(info.split("=")[1])));
                else if (info.contains(("  module-bsm")))
                    mibObjects.add(new MibObject("19"
                            , MibObject.SNMPType.OctetString
                            , Integer.parseInt(info.split("=")[1])));
                else if (info.contains(("  module-ldm")))
                    mibObjects.add(new MibObject("20"
                            , MibObject.SNMPType.OctetString
                            , Integer.parseInt(info.split("=")[1])));
                else if (info.contains(("  module-wsm")))
                    mibObjects.add(new MibObject("21"
                            , MibObject.SNMPType.OctetString
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
