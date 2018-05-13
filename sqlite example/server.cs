using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;

#if TARGET_LINUX
using Mono.Data.Sqlite;
using sqliteConnection 	=Mono.Data.Sqlite.SqliteConnection;
using sqliteCommand 	=Mono.Data.Sqlite.SqliteCommand;
using sqliteDataReader	=Mono.Data.Sqlite.SqliteDataReader;
#endif

#if TARGET_WINDOWS
using System.Data.SQLite;
using sqliteConnection = System.Data.SQLite.SQLiteConnection;
using sqliteCommand = System.Data.SQLite.SQLiteCommand;
using sqliteDataReader = System.Data.SQLite.SQLiteDataReader;
#endif


namespace server
{
    public class server
    {
        // Declaring my Variables for later
        static bool active = true;
        static LinkedList<string> incommingMessages = new LinkedList<string>();
        static Dictionary<String, bool> ActiveSockets = new Dictionary<string, bool>();
        static Dictionary<String, ReceiveThreadLaunchInfo> ConnectedClients = new Dictionary<string, ReceiveThreadLaunchInfo>();
        static Dictionary<Socket, Character> CharacterbySocket = new Dictionary<Socket, Character>();
        static Dictionary<string, Socket> SocketbyCharacter = new Dictionary<string, Socket>();
        static Dungeon MyDungeon = new Dungeon();

        static sqliteConnection connection;

        static string databaseName = "database.database";

       
          //this takes care of the clients info while they connect
         
        public class ReceiveThreadLaunchInfo
        {
            public ReceiveThreadLaunchInfo(int ID, Socket socket, Character newCharacter, int UserState)
            {
                this.ID = ID;
                this.socket = socket;
                this.clientCharacter = newCharacter;
            }

            public int ID;
            public Socket socket;
            public Character clientCharacter;
            public int UserState;

        }

        
         // This thread is created to listen for client messages constantly.
         
        static void clientReceiveThread(object obj)
        {

            ReceiveThreadLaunchInfo receiveInfo = obj as ReceiveThreadLaunchInfo;
            bool socketactive = true;
            ASCIIEncoding encoder = new ASCIIEncoding();

            // Add the client to dictonaries for use in Dungeon.Process()
            CharacterbySocket.Add(receiveInfo.socket, receiveInfo.clientCharacter);
            SocketbyCharacter.Add(receiveInfo.clientCharacter.name, receiveInfo.socket);

            // While the client exists and the program is running try and get the clients message.
            while ((active == true) && (socketactive == true))
            {
                // Clear the last message.
                byte[] buffer = new byte[4094];

                try
                {
                    //Listen for the user.
                    int result = receiveInfo.socket.Receive(buffer);

                    // If something arrives.
                    if (result > 0)
                    {
                        
                        // Lock incommingmessages to prevent conflicts.
                        lock (incommingMessages)
                        {
                            // Decode the clients message
                            string message = encoder.GetString(buffer, 0, result);

                            // If the client has finished character creation and logging in the move to the MUD.
                            if (receiveInfo.clientCharacter.PlayerLoginDetails(ref receiveInfo.UserState, message, receiveInfo.socket, connection, ref receiveInfo.clientCharacter.name) == false)
                            { 
                                // Deals with the clients requests for movement or chatting
                                MyDungeon.Process(ConnectedClients, receiveInfo.clientCharacter, message, receiveInfo.socket, CharacterbySocket, SocketbyCharacter, connection);
                            }
                            // Display information.
                            MyDungeon.RoomInfo(receiveInfo.socket,connection, CharacterbySocket);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    ConnectedClients.Remove(receiveInfo.clientCharacter.name);
                    ActiveSockets.Remove(receiveInfo.clientCharacter.name);
                    //receiveInfo.socket.Send(encoder.GetBytes("Server Error has caused disconnection"));
                    socketactive = false;
                }
            }


        }

        /*
         * This is created to accept clients and is always running after server boots up.
         */ 
        static void acceptClientThread(object obj)
        {
            Socket s = obj as Socket;

            int ID = 0;

            while (active == true)
            {
                var NewClientSocket = s.Accept();

                

                var MyThread = new Thread(clientReceiveThread);

                string ClientID = "" + ID;

                var newCharacter = new Character(ClientID);

                var UserState = 0;

                var ThreadLaunchInfo = new ReceiveThreadLaunchInfo(ID, NewClientSocket, newCharacter, UserState);
                //sets the characters name
                String CharacterName = ThreadLaunchInfo.clientCharacter.name;

                ThreadLaunchInfo.clientCharacter.SetPlayerRoom(MyDungeon.SetRoom(), ThreadLaunchInfo.socket);
               //Adds this client to the dictionary of connected clients
                ConnectedClients.Add(CharacterName, ThreadLaunchInfo);
                //Adds this socket to the acitve socket dictitonary
                ActiveSockets.Add(CharacterName, true);

                MyThread.Start(ThreadLaunchInfo);

                ID++;

                Console.WriteLine("Client Joined " + ClientID);
            }

        }

        // This is the entry point for the server
        static void Main(string[] args)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ipLocal = new IPEndPoint(IPAddress.Parse("138.68.189.227"), 8221);

            s.Bind(ipLocal);
            s.Listen(4);

            connection = new sqliteConnection("Data Source=" + databaseName + ";Version=3;FailIfMissing=True");

            sqliteCommand command;

            // Check if the database can be opened and if it exists.
            try
            {
                connection.Open();
            }

            // If database can't be opened then create a new database and set up all of the tables for use later.
            catch (Exception ex)
            {

                sqliteConnection.CreateFile(databaseName);

                connection = new sqliteConnection("Data Source=" + databaseName + ";Version=3;FailIfMissing=True");

                connection.Open();

                command = new sqliteCommand("create table table_users (login varchar(20), password varchar(200), salt varchar(200), player varchar(20))", connection);

                command.ExecuteNonQuery();

                command = new sqliteCommand("create table table_characters (name varchar(18), room varchar(20))", connection);

                command.ExecuteNonQuery();

                command = new sqliteCommand("create table table_dungeon (name varchar(60), description varchar(600), north varchar(20), south varchar(20), east varchar(20), west varchar(20), up varchar(20), down varchar(20))", connection);

                command.ExecuteNonQuery();

                // Populate the dungeon table with all of the room information.
                MyDungeon.Init(databaseName, connection);

            }

            
        Console.WriteLine("Waiting for client ...");
            // Create the thread which will accept all new clients.
        var myThread = new Thread(acceptClientThread);
            myThread.Start(s);

            int itemsProcessed = 0;
            string tempID = "" + 0;

            // While true keep processing items and don't allow the program to close.
            while (true)
            {
                String labelToPrint = "";


                lock (incommingMessages)
                {
                    if (incommingMessages.First != null)
                    {
                        labelToPrint = incommingMessages.First.Value;
                        incommingMessages.RemoveFirst();

                        itemsProcessed++;
                    }
                }

                Thread.Sleep(1);

            }

            }
    }

}
