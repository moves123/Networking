using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace locationserver
{

    public class Whois
    {
        public serverinfo Dictionary;

        static void Main(string[] args)
        {
            runServer();
        }

        static void runServer()
        {
            TcpListener listener;
            Socket connection;
            handler Requesthandler;
            //NetworkStream socketStream;
            try
            {
                listener = new TcpListener(IPAddress.Any, 43);
                listener.Start();
                Console.WriteLine("Server starts listening");
                while (true)
                {
                    connection = listener.AcceptSocket();
                    Requesthandler = new handler();
                    Thread t = new Thread(() => Requesthandler.doRequest(connection));
                    t.Start();
                }
            }
            catch
            (Exception e)
            {
                Console.WriteLine("Exception:" + e.ToString());
            }
        }
    }


    class handler // a threading class
    {
        public void doRequest(Socket connection)
        {
            NetworkStream socketStream;
            socketStream = new NetworkStream(connection);
            Console.WriteLine("Connected");

            try
            {
               


                List<string> menu = new List<string>();
                string clientdetails = string.Empty;

                Dictionary<string, string> i = new
                    Dictionary<string, string>();
                socketStream.ReadTimeout = 1000;// timeout read
                socketStream.WriteTimeout = 1000; //writesout timeout

                StreamWriter sw = new StreamWriter(socketStream);
                StreamReader sr = new StreamReader(socketStream);

                string newinfo = sr.ReadLine();
                i = serverinfo.Accessdictionary();
                menu.Add(newinfo);



                string[] split = menu[0].Split(' ');// spliting anyting from the string that contains a space
                string location = string.Empty;
                clientdetails = split[0];
                /*try
                {
                    List<string> hello = new List<string>();
                    int here = 0;

                    while (sr.Peek() != -1)// changed it from endofstream
                    {
                        hello.Add(sr.ReadLine());
                        here = hello.Count;
                        Console.WriteLine(hello[here - 1]);
                    }
                } catch { }*/

                if (newinfo.Contains("GET")) // checking if the get method is included
                {
                    if (newinfo.Contains("1.0")) //checks if the 1.0 protocol is included
                    {
                        //http1.0
                        //location = newinfo[clientdetails];
                        string[] addplit = newinfo.Split('?'); // splitting lines with a ?
                        string put = addplit[1];
                        Console.WriteLine(put);// prints out the line
                        string[] putsplit = put.Split(' ');// split another lines with a space in them
                        clientdetails = putsplit[0];
                       // Console.WriteLine("GET /?" + clientdetails);//here

                        if (i.ContainsKey(clientdetails)) // checks in the dictionary for the name
                        {
                            //Console.WriteLine("1");
                            sw.WriteLine("HTTP/1.0 200 OK");
                            sw.WriteLine("Content-Type: text/plain");
                            sw.WriteLine("");
                            sw.WriteLine(i[clientdetails]);
                            sw.Flush();
                        }

                        else
                        {
                           // Console.WriteLine("2");
                            sw.WriteLine("HTTP/1.0 404 Not Found");
                            sw.WriteLine("Content-Type: text/plain");
                            sw.WriteLine();
                            sw.Flush();
                        }

                    }


                    else if (newinfo.Contains("1.1"))
                    {
                        string[] addsplit = newinfo.Split('=');
                        string put = addsplit[1];

                        //Console.WriteLine(put);
                        string[] putsplit = put.Split(' ');
                        clientdetails = putsplit[0];
                        string line = sr.ReadLine();
                        // Console.WriteLine("GET /?name" + clientdetails);

                        if (i.ContainsKey(clientdetails))
                        {
                            sw.WriteLine("HTTP/1.1 200 Not Found");
                            sw.WriteLine("Content-Type: text/plain");
                            sw.WriteLine("");
                            sw.WriteLine(i[clientdetails]);
                            sw.Flush();

                        }

                        else
                        {
                            sw.WriteLine("HTTP/1.1 404 Not Found");
                            sw.WriteLine("Content-Type: text/plain");
                            sw.WriteLine();
                            sw.Flush();
                        }

                    }
                    else
                    {
                        string[] addsplit = newinfo.Split('/');
                        clientdetails = addsplit[1];
                        Console.WriteLine(clientdetails);
                        // clientdetails = clientdetails.Trim();

                        //string[] putsplit = put.Split(' ');
                        //look = putsplit[0];
                        //Console.WriteLine("0.9 GET" + look);// add string
                            if (i.ContainsKey(clientdetails))
                            {
                                // Console.WriteLine("0");
                                sw.WriteLine("HTTP/0.9 200 OK"); // printing out the message
                              sw.WriteLine("Content-Type: text/plain"); // printing out the message
                              sw.WriteLine(""); // a blank line is outputed/printed
                                sw.WriteLine(i[clientdetails]); // prints out the name that is in the dictionary
                                sw.Flush();
                            }

                                    else
                                    {
                                        sw.WriteLine("HTTP/0.9 404 Not Found"); // printing out the message
                                        sw.WriteLine("Content-Type: text/plain");// printing out the message
                                        sw.WriteLine(); // a blank line is outputed/printed
                                        sw.Flush();
                                    }
                    }
                }
                

                else if (newinfo.Contains("PUT")) // checking if it contains the put method
                {
                    // http 0.9
                    string[] addsplit = newinfo.Split('/'); // splitting the string with a / character
                    clientdetails = addsplit[1];
                    sr.ReadLine();// read the line
                    location = sr.ReadLine();

                    if (i.ContainsKey(clientdetails))// ifthe dictionary in this case i contains clientdetails

                    {
                        i[clientdetails] = location;// then add the location to the clientdetails
                    }
                    else // otherwise
                    {
                        i.Add(clientdetails, location);// add both clientdetails and the location
                        sw.Flush();// then flush them so it doesnt cause a buffer
                    }
                    sw.WriteLine("HTTP/0.9 200 OK"); // printing out the message
                    sw.WriteLine("Content-Type: text/plain");// printing out the message
                    sw.WriteLine();// printing out a blank line
                    sw.Flush();// then flush them so it doesnt cause a buffer
                }

                else if (newinfo.Contains("POST"))
                {
                    // http 1.0
                    if (newinfo.Contains("1.0"))
                    {
                        string[] addsplit = newinfo.Split('/');
                        string put = addsplit[1];
                        // string putsplit = put[1];
                        string[] putsplit = put.Split(' ');
                        clientdetails = putsplit[0];
                        //clientdetails = names1split[1];
                        //string[] loc1split = loc1.Split('-');
                        // location = loc1split[1];

                        string info = sr.ReadLine();
                        string get = sr.ReadLine();
                        clientdetails = sr.ReadLine();

                        Console.WriteLine("1.0" + clientdetails);
                        Console.WriteLine("1.0" + location);



                        if (i.ContainsKey(clientdetails))

                        {
                            i[clientdetails] = location;
                        }
                        else
                        {
                            i.Add(clientdetails, location);
                            sw.Flush();
                        }
                        sw.WriteLine("HTTP/1.0 200 OK");
                        sw.WriteLine("Content-Type: text/plain");
                        sw.WriteLine();
                        sw.Flush();

                    }
                    else
                    {
                        // http 1.1
                        string info;
                        string get = sr.ReadLine();
                        Console.WriteLine(get);
                        string infonew = sr.ReadLine();
                        Console.WriteLine(infonew);
                        string[] getnew = infonew.Split(' ');
                        int length = int.Parse(getnew[1]);
                        
                        info = sr.ReadLine();
                        string loc = sr.ReadLine();
                        Console.WriteLine("loc" + loc);

                        string[] locsplit = loc.Split('&');
                        Console.WriteLine("Name" + locsplit[0]);
                        Console.WriteLine("Location" + locsplit[1]);
                        string name1 = locsplit[0];
                        string loc1 = locsplit[1];
                        string[] name1split = name1.Split('=');
                        clientdetails = name1split[1];
                        string[] loc1split = loc.Split('=');
                        location = loc1split[1];


                        if (i.ContainsKey(clientdetails))
                        {
                            i[clientdetails] = location;
                        }
                        else
                        {
                            i.Add(clientdetails, location);
                            sw.Flush();
                        }
                        sw.WriteLine("HTTP/1.1 200 OK");
                        sw.WriteLine("Content-Type: text/plain");
                        sw.WriteLine();
                        sw.Flush();

                    }
                }
               /* else
                {
                    
                    if (newinfo.Contains(" "))
                    {
                        string[] newinfo1 = newinfo.Split(' ');
                        for (int s = 1; s < newinfo1.Length; s++)
                        {

                            clientdetails = newinfo1[0];
                            location = location + newinfo1[s] + " ";
                        }
                        location = location.Trim();
                    }


                    else
                    {
                        clientdetails = newinfo;
                    }
                }*/
                
                //Console.WriteLine("Error");

                    else if (split.Length == 1)
                    {
                    try
                    {
                        location = serverinfo.getlocation(clientdetails);
                    }
                    catch
                    {
                        location = null;
                    }

                    if (location == null)
                    {
                        sw.WriteLine("Error: no entries found");
                    }
                    if (location != null)
                    {
                        sw.WriteLine(location);
                        //sw.Flush();
                    }

                    sw.Flush();
                    sw.Close();
                    }
                else if (split.Length > 1)
                {
                    for (int j = 1; j < split.Length; j++)
                    {
                        location += split[j] + " ";
                    }

                    location = location.TrimEnd(' ');// cutting any space character at the end of location string
                    sw.WriteLine("OK");// prints out this line
                    sw.Flush();
                    serverinfo.setlocation(clientdetails, location);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);// a more detailed error message with a line that casuing the problem
            }
            finally
            {
                socketStream.Close();// all the sockets connection are flushed out
                connection.Close();// closses the connection //timeout etc..
            }
        }
    }
}