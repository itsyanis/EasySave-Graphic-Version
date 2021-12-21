using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EasySave_3.Models
{
    class Client_Socket
    {
        public static Socket SeConnecter()
        {
            Socket socket = null;

            try
            {
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remotEndPoint = new IPEndPoint(ipAddress, 11100);
                socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(remotEndPoint);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                // Trace.WriteLine("Socket connected to {0}", socket.RemoteEndPoint);  
                Trace.WriteLine("CONNECTED {0} ", socket.Connected.ToString());

            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }

            return socket;
        }


        public static void EcouterReseau(Socket client, string message)
        {
            byte[] bytes = new byte[2048];

            // Encode the data string into a byte array.  
            byte[] msg = Encoding.ASCII.GetBytes(message);


            // Send the data through the socket.  
            int bytesSent = client.Send(msg);
            Trace.WriteLine(bytesSent);

            // Receive the response from the remote device.  
            int bytesRec = client.Receive(bytes);
            Trace.WriteLine("recieved : " + bytesRec);
            Trace.WriteLine("Echoed test = {0}",
            Encoding.ASCII.GetString(bytes, 0, bytesRec));
        }

        public static void Deconnecter(Socket socket)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

    }
}
