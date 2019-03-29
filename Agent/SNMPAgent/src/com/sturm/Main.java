package com.sturm;

import java.io.File;

/**
 * Created by: Péter Sturm
 * Date: 2019-03-29
 */

public class Main {
    public static void main(String[] args) {
        Agent agent = new Agent(new File("SNMP4JTestAgentBC.cfg"),
                new File("SNMP4JTestAgentConfig.cfg"));
        agent.start("127.0.0.1/161"
                ,"127.0.0.1/162"
                , "rsu"
                , "trapauthpass01"
                , "trapprivpass01");

        while (true) {
            try {
                agent.sendTrap();
                Thread.sleep(5000);
            }
            catch (InterruptedException ex1) {
                break;
            }
        }
    }
}
