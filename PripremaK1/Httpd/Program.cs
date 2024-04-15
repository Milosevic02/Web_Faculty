using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Httpd
{
    class Program
    {
        public static List<Club> clubs = new List<Club>();

        public static void StartListening()
        {

            IPAddress ipAddress = IPAddress.Loopback;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 8080);

            // Create a TCP/IP socket.
            Socket serverSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and 
            // listen for incoming connections.
            try
            {
                serverSocket.Bind(localEndPoint);
                serverSocket.Listen(10);

                // Start listening for connections.
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.
                    Socket socket = serverSocket.Accept();

                    Task t = Task.Factory.StartNew(() => Run(socket));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        private static void Run(Socket socket)
        {

            NetworkStream stream = new NetworkStream(socket);
            StreamReader sr = new StreamReader(stream);
            StreamWriter sw = new StreamWriter(stream) { NewLine = "\r\n", AutoFlush = true };

            string resource = GetResource(sr);
            if (resource != null)
            {
                if (resource.Equals(""))
                    resource = "index.html";

                Console.WriteLine("Request from " + socket.RemoteEndPoint + ": "
                        + resource + "\n");

                if (resource.Contains("add?name="))
                {
                    string[] club = resource.Split(new string[] { "name=", "cities=", "active=" }, StringSplitOptions.None);
                    string responseText = "HTTP/1.0 200 OK\r\n\r\n";
                    sw.Write(responseText);

                    var name = GetPropertyValue(club[1]);
                    var cities = GetPropertyValue(club[2]);
                    var activeStr = "off";
                    try
                    {
                        activeStr = GetPropertyValue(club[3]);
                    }
                    catch (Exception e) { }
                    bool active = false;

                    if (activeStr == "on")
                    {
                        active = true;
                    }

                    sw.Write("<html><body>");
                    if (String.IsNullOrEmpty(name))
                    {
                        sw.WriteLine(GetAllClubs());
                    }
                    else
                    {
                        Club tempClub = new Club(name, cities, active);
                        if (clubs.Contains(tempClub))
                        {
                            sw.Write($"<h1>User with:{name} already exists.</h1>");
                            sw.WriteLine(GetAllClubs());
                        }
                        else
                        {
                            clubs.Add(tempClub);
                            sw.Write($"<h1 style=\"color:blue\">Table</h1>");
                            sw.WriteLine(GetAllClubs());
                        }
                    }
                    sw.WriteLine("<a href=\"/index.html\">Add New Club</a><br/>");
                    sw.WriteLine("<a href=\"/bestClub\">Show Best Club</a>");
                    sw.Write($"<h1 style=\"color:blue\">Input Points</h1>");
                    sw.WriteLine("<form accept-charset=\"UTF - 8\" action=\"http://localhost:8080/add\">");
                    sw.WriteLine("<table><tr><td>Club </td> <td>");
                    sw.WriteLine("<select name=\"club\">");
                    foreach (Club c in clubs)
                    {
                        sw.WriteLine("<option value=\"" + c.Name + "\">" + c.Name + "</option>");

                    }
                    sw.WriteLine("<br/></select></td></tr><tr><td>Points</td><td><input type=\"number\" name=\"points\"></td>");
                    sw.WriteLine("<tr><td></td><td><input type=\"submit\" value=\"Submit\" /></td></tr></table></form>");

                    sw.WriteLine("</body></html>");
                }
                else if (resource.Contains("add?club="))
                {
                    string[] club = resource.Split(new string[] { "club=", "points=" }, StringSplitOptions.None);
                    string responseText = "HTTP/1.0 200 OK\r\n\r\n";
                    sw.Write(responseText);

                    var name = GetPropertyValue(club[1]);
                    var points = GetPropertyValue(club[2]);
                    sw.Write("<html><body>");
                    if (String.IsNullOrEmpty(name))
                    {
                        sw.WriteLine(GetAllClubs());
                    }
                    else
                    {
                        foreach (Club c in clubs)
                        {
                            if (c.Name == name)
                            {
                                c.Points = Int32.Parse(points);
                                sw.WriteLine("Successfully added points");
                                break;
                            }
                        }
                    }
                    sw.WriteLine(GetAllClubs());

                    sw.WriteLine("<a href=\"/index.html\">Add New Club</a><br/>");
                    sw.WriteLine("<a href=\"/bestClub\">Show Best Club</a>");
                    sw.Write($"<h1 style=\"color:blue\">Input Points</h1>");
                    sw.WriteLine("<form accept-charset=\"UTF - 8\" action=\"http://localhost:8080/add\">");
                    sw.WriteLine("<table><tr><td>Club </td> <td>");
                    sw.WriteLine("<select name=\"club\">");
                    foreach (Club c in clubs)
                    {
                        sw.WriteLine("<option value=\"" + c.Name + "\">" + c.Name + "</option>");

                    }
                    sw.WriteLine("<br/></select></td></tr><tr><td>Points</td><td><input type=\"number\" name=\"points\"></td>");
                    sw.WriteLine("<tr><td></td><td><input type=\"submit\" value=\"Submit\" /></td></tr></table></form>");

                    sw.WriteLine("</body></html>");
                }
                else if (resource.Contains("edit?"))
                {
                    string[] club = resource.Split(new string[] { "name=", "cities=", "active=", "points" }, StringSplitOptions.None);
                    string responseText = "HTTP/1.0 200 OK\r\n\r\n";
                    sw.Write(responseText);

                    var name = GetPropertyValue(club[1]);
                    var cities = GetPropertyValue(club[2]);
                    var activeStr = "off";
                    try
                    {
                        activeStr = GetPropertyValue(club[3]);
                    }
                    catch (Exception e) { }
                    bool active = false;
                    var points = GetPropertyValue(club[4]);
                    if (activeStr == "on")
                    {
                        active = true;
                    }

                    sw.Write("<html><body>");
                    var index = 0;
                    foreach (Club c in clubs)
                    {
                        if (c.Name.Equals(name))
                        {
                            break;
                        }
                        index++;
                    }

                    clubs.RemoveAt(index);
                    sw.WriteLine("<h1 style=\"color:green\">Edit Data</h1>");
                    sw.WriteLine("<form accept-charset=\"UTF-8\" action=\"http://localhost:8080/add\"><table><tr><td>Naziv:</td>" +
                        "<td><input value=\"" + naziv + "\" type=\"text\" name=\"naziv\"></td></tr><tr><td>Grad:</td><td><select value=\"" + grad + "\" name=\"gradovi\"><option value=\"Novi Sad\">Novi Sad</option>" +
                        "<option value=\"Beograd\">Beograd</option><option value=\"Nis\">Nis</option></select></td></tr><tr><td>Aktivan:</td><td><input " + check + " type=\"checkbox\" name=\"aktivan\"></td>" +
                        "</tr><tr><td></td><td><input type=\"submit\" value=\"Izmeni\" /></td></tr></table></form>");


                }
                else
                {
                    SendResponse(resource, socket, sw);
                }
            }
            sr.Close();
            sw.Close();
            stream.Close();

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            //return 0;
        }

        private static string GetPropertyValue(string field)
        {
            var newField = field.Split('&')[0];
            newField = Uri.UnescapeDataString(newField);
            newField = newField.Replace("+", " ");

            return newField;
        }

        private static string GetAllClubs()
        {
            int count = 1;  
            string result = "<table border=\"1\"";

            if(clubs.Count == 0)
            {
                result = "<h3>List is empty!<h3>";
                return result;
            }
            result += "tr align=\"center\">" + "<td>#</td>" + "<td>Club</td>"
                                            + "<td>Points</td>" + "<td>Actions</td>" + "</tr>";
            foreach(Club club in clubs)
            {
                result += "<tr align=\"center\">" + "<td>" + count++.ToString() + "</td>"
                                 + "<td>" + club.Name + "</td>"
                                 + "<td>" + club.Points + "</td>"
                                 + "<td><a href=\"/edit?name=" + club.Name + "&cities=" + club.City + "&active=" + club.Active + "&points=" + club.Points + "\">Edit data</a><br/><br/></td>"
                        + "</tr>";
            }

            result += "</table><br/>";

            return result;
        }

        private static string GetResource(StreamReader sr)
        {
            string line = sr.ReadLine();

            if (line == null)
                return null;

            String[] tokens = line.Split(' ');

            // prva linija HTTP zahteva: METOD /resurs HTTP/verzija
            // obradjujemo samo GET metodu
            string method = tokens[0];
            if (!method.Equals("GET"))
            {
                return null;
            }

            string rsrc = tokens[1];

            // izbacimo znak '/' sa pocetka
            rsrc = rsrc.Substring(1);

            // ignorisemo ostatak zaglavlja
            string s1;
            while (!(s1 = sr.ReadLine()).Equals(""))
                Console.WriteLine(s1);
            Console.WriteLine("Request: " + line);
            return rsrc;
        }

        private static void SendResponse(string resource, Socket socket, StreamWriter sw)
        {
            // ako u resource-u imamo bilo šta što nije slovo ili cifra, možemo da
            // konvertujemo u "normalan" oblik
            //resource = Uri.UnescapeDataString(resource);

            // pripremimo putanju do našeg web root-a
            resource = "../../../" + resource;
            FileInfo fi = new FileInfo(resource);

            string responseText;
            if (!fi.Exists)
            {
                // ako datoteka ne postoji, vratimo kod za gresku
                responseText = "HTTP/1.0 404 File not found\r\n"
                        + "Content-type: text/html; charset=UTF-8\r\n\r\n<b>404 Нисам нашао:"
                        + fi.Name + "</b>";
                sw.Write(responseText);
                Console.WriteLine("Could not find resource: " + fi.Name);
                return;
            }

            // ispisemo zaglavlje HTTP odgovora
            responseText = "HTTP/1.0 200 OK\r\nContent-type: text/html; charset=UTF-8\r\n\r\n";
            sw.Write(responseText);

            // a, zatim datoteku
            socket.SendFile(resource);
        }

        public static int Main(String[] args)
        {
            StartListening();
            return 0;
        }
    }
}
