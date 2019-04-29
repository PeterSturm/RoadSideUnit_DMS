package com.sturm;

import org.apache.commons.net.telnet.TelnetClient;
import java.io.InputStream;
import java.io.PrintStream;

//http://twit88.com/blog/2007/12/22/java-writing-an-automated-telnet-client/

public class TelnetAPI {
    private TelnetClient telnet;
    private InputStream input;
    private PrintStream output;
    private String prompt = "#";

    public static void main(String[] args) {
        TelnetAPI telnet = new TelnetAPI("192.168.0.178", "sturm", "Pass01", "$");
        System.out.println(telnet.executeCommand("ls"));
        try
        {
            System.console().readLine();
        }
        catch (Exception e)
        {
            e.printStackTrace();
        }
        finally
        {
            telnet.disconnect();
        }
    }

    public TelnetAPI(String server, String user, String password, String prompt)
    {
        telnet = new TelnetClient();
        try
        {
            telnet.connect(server,23);
            input = telnet.getInputStream();
            output = new PrintStream(telnet.getOutputStream());
            this.prompt = prompt;

            readUntil("login:");
            output.println(user);
            output.flush();
            readUntil("Password:");
            output.println(password);
            output.flush();
            readUntil(prompt + "");
        }
        catch (Exception e)
        {
            e.printStackTrace();
        }
    }

    public String executeCommand(String command)
    {
        try
        {
            output.println(command);
            output.flush();
            String result = readUntil(prompt + "");
            return result.substring(0,result.lastIndexOf("\n"));
        }
        catch (Exception e)
        {
            e.printStackTrace();
        }

        return null;
    }

    private String readUntil(String pattern)
    {
        try
        {
            char lastChar = pattern.charAt(pattern.length() - 1);
            StringBuffer sb = new StringBuffer();

            char ch = (char) input.read();
            while (true)
            {
                sb.append(ch);
                if (ch == lastChar)
                    if (sb.toString().endsWith(pattern))
                        return sb.toString();

                ch = (char) input.read();
            }
        }
        catch (Exception e)
        {
            e.printStackTrace();
        }
        return null;
    }

    public void disconnect()
    {
        try
        {
            telnet.disconnect();
        }
        catch (Exception e)
        {
            e.printStackTrace();
        }
    }
}
