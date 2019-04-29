package com.sturm;

import com.sun.xml.internal.ws.policy.privateutil.PolicyUtils;
import sun.awt.image.ImageWatched;

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

        LinkedList<RSUAgent> rsus = RSUAgentLoader.Load("rsus.txt");

        for(RSUAgent rsu : rsus)
            rsu.startAgent();

        while (true) {
            try {
                for (RSUAgent rsu : rsus)
                    rsu.agent.sendTrap();

                Thread.sleep(5000);
            }
            catch (InterruptedException ex1) {
                break;
            }
        }
    }
}
