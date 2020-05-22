﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using System.Threading;
using System.Net.NetworkInformation;
using CommonInformation;

namespace ServerSide
{
    static class Server
    {
        const int SERVERUDPPORT = 7744;
        const int SERVERTCPPORT = 7745;
        const int MAXNUMBEROFUSERS = 7;
        public const int NumberOfQuestions = 10;
        public const int NumberOfAnswers = 4;

        public static Question[] questions = new Question[NumberOfQuestions];
        public static Answer[,] answers = new Answer[NumberOfQuestions, NumberOfAnswers];
        public static int QuestionNumber = 0;
        public static List<Game> GamesList = new List<Game>();


        public static Dictionary<int, Socket> clientSockets = new Dictionary<int, Socket>();
        public static Dictionary<int, string> clientNames = new Dictionary<int, string>();
        public static List<string> MessageHistory = new List<string>();

        static MessageSerializer messageSerializer = new MessageSerializer();

        public static void GetQuestionsFromFile()
        {
            using (StreamReader streamReader = new StreamReader("Questions.txt", Encoding.Default))
            {
                for (int i = 0; i < NumberOfQuestions; i++)
                {
                    questions[i].Title = streamReader.ReadLine();
                    answers[i, 0].IsRight = true;

                    for (int j = 0; j < NumberOfAnswers; j++)
                        answers[i, j].Title = streamReader.ReadLine();
                }
            }
        }

        public static void SetTCPConnection()
        {
            IPAddress IPaddress = CommonInfo.GetHostsIPAddress();
            var IPendPoint = new IPEndPoint(IPaddress, SERVERTCPPORT);
            var socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketListener.Bind(IPendPoint);
            socketListener.Listen(MAXNUMBEROFUSERS);
            Console.WriteLine("TCP севрер готов!");
            while (true)
            {
                Socket listeningClientMessagesSocket = socketListener.Accept();
                ClientConnectionManager clientConnection = new ClientConnectionManager(listeningClientMessagesSocket);
            }
        }

        public static void SetUDPConnection()
        {
            var IPendPoint = new IPEndPoint(IPAddress.Any, SERVERUDPPORT);
            var socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socketListener.Bind(IPendPoint);
            Console.WriteLine("UDP севрер готов!");

            EndPoint remotePoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = new byte[8192];
            while (true)
            {
                int amount = socketListener.ReceiveFrom(data, ref remotePoint);
                Message message = messageSerializer.Deserialize(data, amount);
                if (message.messageType == Message.MessageType.SearchRequest)
                {
                    Message messageResponse = new Message()
                    { IPAdress = CommonInfo.GetHostsIPAddress().ToString(), messageType = Message.MessageType.SearchResponce, serverPort = SERVERTCPPORT };
                    var iPaddress = IPAddress.Parse(message.IPAdress);
                    IPEndPoint remoteEndPoint = new IPEndPoint(iPaddress, message.clientPort);
                    socketListener.SendTo(messageSerializer.Serialize(messageResponse), remoteEndPoint);
                }
            }
        }

        public static void StartListen()
        {
            Thread handleTCPThread = new Thread(SetTCPConnection);
            Thread handleUDPThread = new Thread(SetUDPConnection);
            handleTCPThread.Start();
            handleUDPThread.Start();
            GetQuestionsFromFile();
        }

        public static void SendMessage(Message message, Socket clientSocket)
        {
            clientSocket.Send(messageSerializer.Serialize(message));
        }

        public static void SendToAll(Message message)
        {
            foreach (Socket clientSocket in clientSockets.Values)
            {
                SendMessage(message, clientSocket);
            }
        }

        public static void RemoveClient(int key)
        {
            clientNames.Remove(key);
            clientSockets.Remove(key);
        }
    }
}

