using System;
using System.Net.Sockets;
using System.IO;
using System.Collections.Generic;

public class Whois
{
    static void Main(string[] args)
    {
        
        ///  list of variables names being used inside the programs 
       

        string server = "whois.net.dcs.hull.ac.uk";
        int port = 43;
        string protocol = "whois";
        string username = null;  // set the variable username as null
        string location = null;  // set the variable location as null
        List<string> list = new List<string>(); // creating   list of arguemnts


        try
        {



            for (int i = 0; i < args.Length; ++i)
            {
                switch (args[i])
                {
                    case "-h":
                        server = args[++i];
                        break;
                    case "-p":
                        port = int.Parse(args[++i]);
                        break;
                    case "-h9": protocol = args[i]; break;

                    case "-h0": protocol = args[i]; break;

                    case "-h1": protocol = args[i]; break;
                    default:
                        if (username == null)
                        {
                            username = args[i]; // return the username and argument
                        }
                        else if (location == null)
                        {
                            location = args[i];
                        }
                        else
                        {
                            Console.WriteLine("many arguments"); // error message 
                        }
                        break;

                }
            }
            if (username == null)
            {
                Console.WriteLine("few arguments");
                return;

            }
            TcpClient client = new TcpClient();  //starts  TCPClient 
            client.Connect(server, port); // connects the client to the whois at port 43
            StreamWriter sw = new StreamWriter(client.GetStream());   // starts the stream writer and puts the variable sw.
            StreamReader sr = new StreamReader(client.GetStream());   // starts the stream reader and puts  the variable sr.


            client.SendTimeout = 1000;
            client.ReceiveTimeout = 1000;

           
            ///  loops through the list of arguments and protocols 
            ///  protocol /h,/p,/h0.9, /h0, /h1
            


            switch (protocol)
            {
                case "whois":
                    if (location == null)
                    {

                        sw.WriteLine(username);
                        sw.Flush();
                        Console.WriteLine(username + " is " + sr.ReadToEnd());

                    }
                    else
                    {

                        sw.WriteLine(username + " " + location); // writes out a line with username and the location space included
                        sw.Flush();   // flushes  the stream to the file to clear the buffer. 
                        string reply = sr.ReadLine();
                        if (reply == ("OK")) // if reply reads Ok then the location of the person can be changed
                        {
                            Console.WriteLine(username + " location changed to be " + location); // shows the username and where the person is
                        }
                        else // otherwise
                        {
                            Console.WriteLine("ERROR: no entries found: " + reply);  // error message is displayed.
                        }
                    }
                    break;
                case "-h9":
                    if (location == null)
                    {
                        sw.WriteLine("GET /" + username);  // writes the protocol request for /h0.9 to the stream writer. 
                        sw.Flush(); //flushes the request  to clear the buffer.
                        string line1 = sr.ReadLine();
                        line1 = sr.ReadLine();
                        line1 = sr.ReadLine();
                        Console.WriteLine(username + " is " + sr.ReadToEnd());
                    }
                    else
                    {
                        sw.WriteLine("PUT /" + username); // writes put th the name
                        sw.WriteLine(); // optional blank line 
                        sw.WriteLine(location);// writes out a line with location
                        sw.Flush(); // flushes the request to avoid buffer
                        string reply = sr.ReadLine();
                        if (reply.EndsWith("OK")) // if reply returns a line that ends with the work ok
                        {
                            Console.WriteLine(username + " location changed to be " + location); // then outputs the username plus the location of the person 
                        }
                        else // otherwise
                        {
                            Console.WriteLine("ERROR: no entries found: " + reply); // error message
                        }
                    }
                    break;
                case "-h0":
                    if (location == null)
                    {
                        sw.WriteLine("GET /?" + username + " HTTP/1.0\r\n"); // writes get for the  name and protocol to the stream writer.
                        sw.Flush(); // flushes the line to avoid buffer
                        string line1 = sr.ReadLine();
                        line1 = sr.ReadLine(); // read the string line1
                        line1 = sr.ReadLine();  // read the string line1
                        Console.WriteLine(username + " is " + sr.ReadToEnd()); // writes out the username
                    }
                    else
                    {
                        int length = location.Length;  // seting length to the length of the location args.
                        sw.WriteLine("POST /" + username + " HTTP/1.0"); // writes post for the  name and protocol to the stream writer.
                        sw.WriteLine("Content-Length: " + length);
                        sw.WriteLine(); // optional blank line
                        sw.WriteLine(location); // prints out the location
                        sw.Flush();  // flushes the request to clear the buffer
                        string reply = sr.ReadLine();   // reply is  line read in the stream reader.
                        if (reply.EndsWith("HTTP/1.0 200 OK"))  // if the reply responds/ends with this string
                        {
                            Console.WriteLine(username + " location changed to be " + location); //outputs the username plus the location of the person 
                        }
                        else
                        {
                            Console.WriteLine("ERROR: no entries found: " + reply); // outputs error message
                        }
                    }
                    break;


                case "-h1":

                    if (location == null)
                    {
                        sw.WriteLine("GET /?name=" + username + " HTTP/1.1"); // writes get for the  name , username and protocol to the stream writer.
                        sw.WriteLine("Host: " + server); // writes a line with host and server
                        sw.WriteLine();  // a blank optional line created
                        sw.Flush();  // flushes the request to clear buffer
                        string reply = null;


                        using (sr)
                        {

                            try
                            {
                                while (!sr.EndOfStream)
                                {
                                    reply = sr.ReadLine();
                                    list.Add(reply);
                                }

                            }

                            catch { }
                        }

                        int read = list.IndexOf("");
                        if (read >= list.Count - 1)
                        {
                            Console.WriteLine(username + " is " + reply);
                        }


                        else
                        {
                            Console.WriteLine(username + " is " + list[read + 1]);

                            for (int i = read + 2; i < list.Count; i++)
                            {
                                Console.WriteLine(list[i]);
                            }
                        }


                    }
                    else
                    {
                        int length = location.Length + username.Length + 15;
                        //string cat;
                        // cat = "name=" + username + "&location=" + location;
                        sw.WriteLine("POST /" + " HTTP/1.1");
                        sw.WriteLine("Host: " + server);
                        sw.WriteLine("Content-Length: " + length);
                        sw.WriteLine();
                        sw.WriteLine("name=" + username + "&location=" + location); // 
                        sw.Flush();
                        string reply = null;// if the string reply is empty

                        try
                        {
                            reply = sr.ReadLine(); // reading the reply line
                        }
                        catch
                        {
                            Console.WriteLine("ERROR: no entries found: " + reply);  // prints out error message
                        }


                        if (reply.EndsWith("HTTP/1.1 200 OK")) // if the reply responds/ends with this string
                        {
                            Console.WriteLine(username + " location changed to be " + location);// writes out the username and the new location set
                        }
                        else //otherwise
                        {
                            Console.Write("ERROR: Unexpected response: " + reply); // prints out error message
                            try
                            {
                                reply = sr.ReadLine();
                                while (reply != null)
                                {
                                    Console.WriteLine(reply);// prints out the reply line
                                    reply = sr.ReadLine();// reads the reply line
                                }
                            }
                            catch
                            {
                                Console.Write("ERROR: Unexpected reply: " + reply); // error message caught
                            }

                        }
                    }
                    break;


            }
            client.Close(); // closes the connections


        }
        catch (Exception e)   // if none of the provided if statements are reached an exception is thrown and caught here. 
        {
            Console.WriteLine("ERROR: no entries found: " + e.Message); //error message  stating reason of the exception being thrown.
        }



    }


}
