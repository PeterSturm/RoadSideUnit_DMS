package com.sturm;

import java.io.File;
import java.util.LinkedList;
import java.util.Scanner;

public class RSUAgentLoader
{
    public static LinkedList<RSUAgent> Load(String filename)
    {
        LinkedList<RSUAgent> agents = new LinkedList<RSUAgent>();

        try
        {
            Scanner scanner = new Scanner(new File(filename));
            while(scanner.hasNextLine())
            {
                String line = scanner.nextLine();
                String[] data = line.split(";");

                if(data.length == 8)
                    agents.add(new RSUAgent(data[0]
                            , data[1]
                            , data[2]
                            , data[3]
                            , data[4]
                            , data[5]
                            , data[6]
                            , data[7]
                            , data[8]));
                else
                    agents.add(new RSUAgent(data[0]
                            , data[1]
                            , data[2]
                            , data[3]
                            , data[4]
                            , data[5]
                            , null
                            , null
                            , null));
            }
            scanner.close();
        }
        catch (Exception e)
        {
            e.printStackTrace();
        }

        return agents;

    }
}
