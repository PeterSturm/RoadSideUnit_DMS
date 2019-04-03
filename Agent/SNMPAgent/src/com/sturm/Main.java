package com.sturm;

import com.sun.xml.internal.ws.policy.privateutil.PolicyUtils;

import java.io.Console;
import java.io.File;
import java.util.LinkedList;
import java.util.List;

/**
 * Created by: Péter Sturm
 * Date: 2019-03-29
 */

public class Main {
    public static void main(String[] args) {

        int rsuCount = 10;
        int startingPort = 2000;
        LinkedList<Agent> agents = new LinkedList<>();
        for (int i = 0; i < rsuCount; i++) {
            agents.add(new Agent(new File("SNMP4JTestAgentBC"+i+".cfg"),
                    new File("SNMP4JTestAgentConfig"+i+".cfg")));
            agents.get(i).start("127.0.0.1"
                    , 2000+i
                    ,"127.0.0.1/162"
                    , "rsu"
                    , "trapauthpass01"
                    , "trapprivpass01");
            System.out.println(agents.get(i).address);
        }
        while (true) {
            try {
                for (int i = 0; i < rsuCount; i++) {
                    agents.get(i).sendTrap();
                }
                Thread.sleep(5000);
            }
            catch (InterruptedException ex1) {
                break;
            }
        }
    }
}
