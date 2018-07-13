
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;


namespace XenLib
{
    public class UDPSend
    {
        public const int MAX_SIZE_UDP = 64000;
        public const string DEFAULT_LOCAL_IP = "127.0.0.1";

        static private UdpClient _client;

        // Use this for initialization
        static public void InitialSend()
        {
            _client = new UdpClient();
        }

        static public IPEndPoint GetEndPoint(string sendIp, int sendPort)     //DEFAULT_LOCAL_IP
        {
            return new IPEndPoint(IPAddress.Parse(sendIp), sendPort);
        }

        // Update is called once per frame
        public static void SendData(byte[] sendData, IPEndPoint sendEndPoint)
        {
            try
            {
                _client.Send(sendData, sendData.Length, sendEndPoint);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Data is greater than: " + MAX_SIZE_UDP + " Data Size is: " + sendData.Length);
            }
        }
    }//class UDPSend



    public class UDPReceive
    {
        private UdpClient _client;
        private Thread _receiveThread;
        private IPEndPoint _endPoint;

        private ParseInputDataFunc _parseFunction;


        public delegate void ParseInputDataFunc(Stream s);


        public UDPReceive(int receiveProt, ParseInputDataFunc parseFunction)
        {

            _client = new UdpClient(receiveProt);
            _endPoint = new IPEndPoint(IPAddress.Any, receiveProt);
            _parseFunction = parseFunction;

            _receiveThread = new Thread(new ThreadStart(ReceiveData));
            _receiveThread.IsBackground = true;
            _receiveThread.Start();
        }



        private void ReceiveData()
        {
            while (true)
            {
                try
                {
                    byte[] data = _client.Receive(ref _endPoint);
                    Stream stream = new MemoryStream(data);
                    _parseFunction(stream);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }


        public void ClientClose()
        {
            if (_receiveThread != null && _receiveThread.IsAlive)
            {
                _receiveThread.Abort();
                _client.Close();
            }
                
        }


    }//class UDPReceive



}//namespace


