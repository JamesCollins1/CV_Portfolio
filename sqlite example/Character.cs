using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

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

using System.Security.Cryptography;

namespace server
{
    /* 
     * This is used to store information about the client when they are connected and it's also used to store if and how far into character creation they still are.
     * 
     * 
     */
    public class Character
    {
        ASCIIEncoding encoder = new ASCIIEncoding();

        public String name = "";
        public string playerRoom;

        public Character(String name)
        {
            this.name = name;

            
        }

        public void SetPlayerRoom(string SpawnRoom, Socket ClientSocket)
        {
            this.playerRoom = SpawnRoom;
        }

        /*
         * This is the function used to determine and guide the player through character creation.
         */ 
        public bool PlayerLoginDetails(ref int userState, string userMessage, Socket ClientSocket, SQLiteConnection connection, ref string clientname)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();

            byte[] sendbuffer = null;

            int bytesSent = 0;

            var messageParts = userMessage.Split(' ');

            // Check where the user is with the login process.
            if (userState == 0)
            {
                Console.WriteLine("Executing userState 0");
                SQLiteCommand command = new sqliteCommand("Select * From table users where login = '" + messageParts[0] + "'");
                try
                {
                    var userSearch = command.ExecuteReader();

                    while (userSearch.Read())
                    {
                        var buffer = userSearch["salt"];
                        sendbuffer = encoder.GetBytes(buffer.ToString());
                    }
                }

                catch
                {
                    sendbuffer = GenerateSalt();
                }

                try
                {
                    bytesSent = ClientSocket.Send(sendbuffer);
                    Console.WriteLine("Salt has been sent");
                    userState++;
                }

                catch
                {
                    Console.WriteLine("Failed to send stuff.");
                }
                return true;
            }


            // Waiting to recieve the encrypted username and password.
            if (userState == 1)
            {
                Console.WriteLine("Executing userState 1");
                SQLiteCommand command = new sqliteCommand("Select * From table users where login = '" + messageParts[0] + "' and password = '" + messageParts[1] + "'");
                try
                {
                    var userSearch = command.ExecuteReader();

                    while (userSearch.Read())
                    {
                        userState = 4;
                        clientname = userSearch["player"].ToString();
                        return false;
                    }
                }
                catch
                {
                    var sql = "insert into " + "table_users" + " (login, password, salt, player) values";
                    sql += "('" + messageParts[0] + "'";
                    sql += ",";
                    sql += "'" + messageParts[1].ToString() + "'";
                    sql += ",";
                    sql += "'" + messageParts[2] + "'";
                    sql += ",";
                    sql += "'temp'";
                    sql += ")";
                    command = new sqliteCommand(sql, connection);

                    try
                    {
                        command.ExecuteNonQuery();
                        userState++;
                    }
                    catch
                    {
                        Console.WriteLine("Failed to perform simple addition.");
                    }

                }
            }

                // If the user has an account but check if they have a character, or if they need to create one and then setup their character.
                if (userState == 2)
                {
                    Console.WriteLine("Executing userState 2");
                SQLiteCommand command = new sqliteCommand("Select * From table users where login = '" + messageParts[0] + "'");
                    try
                    {
                        var userSearch = command.ExecuteReader();

                        while (userSearch.Read())
                        {
                            if (userSearch["player"].ToString() != "temp")
                            {
                                clientname = userSearch["player"].ToString();
                                userState = 4;
                            }
                        }
                    }
                    catch
                    {
                        sendbuffer = encoder.GetBytes("Please enter your new characters name");
                        bytesSent = ClientSocket.Send(sendbuffer);
                        userState++;
                        
                    }
                
                    return true;
                }

                // If they have a new account get them to send a name for their character.
                if (userState == 3)
                {
                    Console.WriteLine("Executing userState 3");
                    SQLiteCommand command = new sqliteCommand("update table_users set player = '" + messageParts[0] + "' where player = 'temp'", connection);
                    try
                    {
                        clientname = messageParts[0];
                        command.ExecuteNonQuery();
                        var sql = "insert into " + "table_characters" + " (name, room) values";
                        sql += "('" + messageParts[0] + "'";
                        sql += ",";
                        sql += "'Cave Entrance'";
                        sql += ")";
                        command = new sqliteCommand(sql, connection);
                        command.ExecuteNonQuery();
                        userState++;
                    }
                    catch
                    {
                        return true;
                    }

                    return false;
                }


                return false;
            }

        /*
         * This is called to generate some new salt for the user.
         */ 
        public byte[] GenerateSalt()
        {
            var rngCSP = new RNGCryptoServiceProvider();

            byte[] salt = new byte[16];

            rngCSP.GetBytes(salt);

            return salt;

        }


    }

}
