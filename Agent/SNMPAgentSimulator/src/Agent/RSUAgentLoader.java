package Agent;

import java.io.File;
import java.util.*;

public class RSUAgentLoader
{
    public static ArrayList<Boolean> processedLines;

    public static ArrayList<RSUAgent> Load(String filename)
    {
        ArrayList<RSUAgent> agents = new ArrayList<RSUAgent>();
        processedLines = new ArrayList<>();

        try
        {
            Scanner scanner = new Scanner(new File(filename));
            while(scanner.hasNextLine())
            {
                String line = scanner.nextLine();
                String[] data = line.split(";");

                if(data.length == 7) {
                    agents.add(new RSUAgent(data[0]
                            , data[1]
                            , data[2]
                            , data[3]
                            , data[4]
                            , data[5]
                            , data[6]
                            , null
                            , null));
                    processedLines.add(true);
                }
                else if (data.length == 20) {
                    String[] modules = {data[13],data[14],data[15],data[16],data[17],data[18],data[19]};
                    agents.add(new RSUAgent(data[0]
                            , data[1]
                            , data[2]
                            , data[3]
                            , data[4]
                            , data[5]
                            , Double.parseDouble(data[6])
                            , Double.parseDouble(data[7])
                            , Double.parseDouble(data[8])
                            ,Integer.parseInt(data[9])
                            ,Integer.parseInt(data[10])
                            ,Integer.parseInt(data[11])
                            ,Integer.parseInt(data[12])
                            ,modules));
                    processedLines.add(true);
                }
                else
                {
                    processedLines.add(false);
                }
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
