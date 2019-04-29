package com.sturm;

import java.io.File;

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

    public Agent agent;

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

        if(TelnetIP != null && TelnetIP != "")
        {
            TelnetAPI telnet = new TelnetAPI(TelnetIP, TelnetUser, TelnetPass, "$");

            //telnet.executeCommand("");
        }
    }

    public void startAgent()
    {
        agent = new Agent(new File("SNMP4JTestAgentBC"+Port+".cfg"),
                new File("SNMP4JTestAgentConfig"+Port+".cfg"));
        agent.start(IP
                , Integer.parseInt(Port)
                , TrapAddress
                , TrapUser
                , TrapAuth
                , TrapPriv);

        System.out.println("Agent " + IP +"/"+ Port + " started!");
    }
}
