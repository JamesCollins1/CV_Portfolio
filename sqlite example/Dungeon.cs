
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

// This is the dungeon class. It holds all of the data for the dungeon and also deals with the messaging and moving around the dungeon



namespace server
{
    public class Dungeon
    {        
       static string spawnRoom = "Cave Entrance";

        sqliteCommand command;

        //This will fill the database with all the dungeon's rooms
        public void Init(string database, sqliteConnection connection)
        {


        {
            var room = new Room("Cave Entrance", "You are standing at the entrance to a dark cave. You get the funny feeling a grand adventure awaits you inside.");
                room.north = "Large Cavern";
                var sql = "insert into " + "table_dungeon" + " (name, description, north, south, east, west, up, down) values";
            sql += "('" + room.name + "'";
            sql += ",";
            sql += "'" + room.desc + "'";
            sql += ",";
            sql += "'" + room.north + "'";
            sql += ",";
            sql += "'" + room.south + "'";
            sql += ",";
            sql += "'" + room.east + "'";
            sql += ",";
            sql += "'" + room.west + "'";
            sql += ",";
            sql += "'" + room.up + "'";
            sql += ",";
            sql += "'" + room.down + "'";
            sql += ")";
            command = new sqliteCommand(sql, connection);
            command.ExecuteNonQuery();
         
        }

        {
            var room = new Room("Large Cavern", "You squeeze through the narrow tunnel, and find it opens out into a large cavern with several other paths leading off from it.");
                room.south = "Cave Entrance";
                room.west = "Waterfall";
                room.east = "Rock Face";
                var sql = "insert into " + "table_dungeon" + " (name, description, north, south, east, west, up, down) values";
                sql += "('" + room.name + "'";
                sql += ",";
                sql += "'" + room.desc + "'";
                sql += ",";
                sql += "'" + room.north + "'";
                sql += ",";
                sql += "'" + room.south + "'";
                sql += ",";
                sql += "'" + room.east + "'";
                sql += ",";
                sql += "'" + room.west + "'";
                sql += ",";
                sql += "'" + room.up + "'";
                sql += ",";
                sql += "'" + room.down + "'";
                sql += ")";
                command = new sqliteCommand(sql, connection);
                command.ExecuteNonQuery();
               
            }

        {
            var room = new Room("Rock Face", "You find yourself before a large cliff face. There is a rope up its north face.");
                room.up = "Small Crevice";
                var sql = "insert into " + "table_dungeon" + " (name, description, north, south, east, west, up, down) values";
                sql += "('" + room.name + "'";
                sql += ",";
                sql += "'" + room.desc + "'";
                sql += ",";
                sql += "'" + room.north + "'";
                sql += ",";
                sql += "'" + room.south + "'";
                sql += ",";
                sql += "'" + room.east + "'";
                sql += ",";
                sql += "'" + room.west + "'";
                sql += ",";
                sql += "'" + room.up + "'";
                sql += ",";
                sql += "'" + room.down + "'";
                sql += ")";
                command = new sqliteCommand(sql, connection);
                command.ExecuteNonQuery();
               
            }

        {
            var room = new Room("Waterfall", "You follow the path which leads to a small but deep body of water being fed by a waterfall.");
                room.east = "Large Cavern";
                var sql = "insert into " + "table_dungeon" + " (name, description, north, south, east, west, up, down) values";
                sql += "('" + room.name + "'";
                sql += ",";
                sql += "'" + room.desc + "'";
                sql += ",";
                sql += "'" + room.north + "'";
                sql += ",";
                sql += "'" + room.south + "'";
                sql += ",";
                sql += "'" + room.east + "'";
                sql += ",";
                sql += "'" + room.west + "'";
                sql += ",";
                sql += "'" + room.up + "'";
                sql += ",";
                sql += "'" + room.down + "'";
                sql += ")";
                command = new sqliteCommand(sql, connection);
                command.ExecuteNonQuery();
                
            }

        {
            var room = new Room("Small Crevice", "You arrive at a dead end. However, after a quick inspection it seems you can knock a few rocks loose and squeeze through the small crevice.");
                room.down = "Rock Face";
                room.west = "Small Cavern";
                var sql = "insert into " + "table_dungeon" + " (name, description, north, south, east, west, up, down) values";
                sql += "('" + room.name + "'";
                sql += ",";
                sql += "'" + room.desc + "'";
                sql += ",";
                sql += "'" + room.north + "'";
                sql += ",";
                sql += "'" + room.south + "'";
                sql += ",";
                sql += "'" + room.east + "'";
                sql += ",";
                sql += "'" + room.west + "'";
                sql += ",";
                sql += "'" + room.up + "'";
                sql += ",";
                sql += "'" + room.down + "'";
                sql += ")";
                command = new sqliteCommand(sql, connection);
                command.ExecuteNonQuery();
                
            }

        {
            var room = new Room("Small Cavern", "You arrive in a cavern similar to the first, but much smaller. There are signs of previous adventures but there is no one else around.");
                room.west = "Mysterious Mooring";
                room.east = "Small Crevice";
                room.north = "Large Ravine";
                var sql = "insert into " + "table_dungeon" + " (name, description, north, south, east, west, up, down) values";
                sql += "('" + room.name + "'";
                sql += ",";
                sql += "'" + room.desc + "'";
                sql += ",";
                sql += "'" + room.north + "'";
                sql += ",";
                sql += "'" + room.south + "'";
                sql += ",";
                sql += "'" + room.east + "'";
                sql += ",";
                sql += "'" + room.west + "'";
                sql += ",";
                sql += "'" + room.up + "'";
                sql += ",";
                sql += "'" + room.down + "'";
                sql += ")";
                command = new sqliteCommand(sql, connection);
                command.ExecuteNonQuery();
                
            }

        {
            var room = new Room("Mysterious Mooring", "You hear the sound of running water and decide to find it. As you approach the underwater river you notice a small pier and a rowing boat moored to it.");
                room.north = "Natural Spring";
                room.east = "Small Cavern";
                room.south = "Waterfall";
                var sql = "insert into " + "table_dungeon" + " (name, description, north, south, east, west, up, down) values";
                sql += "('" + room.name + "'";
                sql += ",";
                sql += "'" + room.desc + "'";
                sql += ",";
                sql += "'" + room.north + "'";
                sql += ",";
                sql += "'" + room.south + "'";
                sql += ",";
                sql += "'" + room.east + "'";
                sql += ",";
                sql += "'" + room.west + "'";
                sql += ",";
                sql += "'" + room.up + "'";
                sql += ",";
                sql += "'" + room.down + "'";
                sql += ")";
                command = new sqliteCommand(sql, connection);
                command.ExecuteNonQuery();
                
            }

        {
            var room = new Room("Natural Spring", "After travelling up the river, you come to its source. Theres nowhere else to go, but maybe the spring comes from somewhere else?");
                room.south = "Mysterious Mooring";
                var sql = "insert into " + "table_dungeon" + " (name, description, north, south, east, west, up, down) values";
                sql += "('" + room.name + "'";
                sql += ",";
                sql += "'" + room.desc + "'";
                sql += ",";
                sql += "'" + room.north + "'";
                sql += ",";
                sql += "'" + room.south + "'";
                sql += ",";
                sql += "'" + room.east + "'";
                sql += ",";
                sql += "'" + room.west + "'";
                sql += ",";
                sql += "'" + room.up + "'";
                sql += ",";
                sql += "'" + room.down + "'";
                sql += ")";
                command = new sqliteCommand(sql, connection);
                command.ExecuteNonQuery();
                
            }

        {
            var room = new Room("Large Ravine", "After following the path you almost fall into a huge ravine. There is several small ledges leading to the north along one of its sides.");
                room.north = "Treasure Cave";
                room.south = "Small Cavern";
                var sql = "insert into " + "table_dungeon" + " (name, description, north, south, east, west, up, down) values";
                sql += "('" + room.name + "'";
                sql += ",";
                sql += "'" + room.desc + "'";
                sql += ",";
                sql += "'" + room.north + "'";
                sql += ",";
                sql += "'" + room.south + "'";
                sql += ",";
                sql += "'" + room.east + "'";
                sql += ",";
                sql += "'" + room.west + "'";
                sql += ",";
                sql += "'" + room.up + "'";
                sql += ",";
                sql += "'" + room.down + "'";
                sql += ")";
                command = new sqliteCommand(sql, connection);
                command.ExecuteNonQuery();
                
            }

        {
            var room = new Room("Treasure Cave",
                    "Finally, you have traversed all the cave has to offer and have arrived in a small cavern with a large chest in the middle. You walk to the chest and claim its treasure as your own. As you take your first handfull of gold, " +
                    "the path you walked along to get here crumbles. Theres no going back, but how do you escape? You see some water pouring from some fallen boulders in the back of the hollow, perhaps there could be a water spring nearby?");
                room.west = "Mysterious Mooring";
                var sql = "insert into " + "table_dungeon" + " (name, description, north, south, east, west, up, down) values";
                sql += "('" + room.name + "'";
                sql += ",";
                sql += "'" + room.desc + "'";
                sql += ",";
                sql += "'" + room.north + "'";
                sql += ",";
                sql += "'" + room.south + "'";
                sql += ",";
                sql += "'" + room.east + "'";
                sql += ",";
                sql += "'" + room.west + "'";
                sql += ",";
                sql += "'" + room.up + "'";
                sql += ",";
                sql += "'" + room.down + "'";
                sql += ")";
                command = new sqliteCommand(sql, connection);
                command.ExecuteNonQuery();
                
            }

            Console.WriteLine("Finished Rebuilding the Dungeon.");
        }

        public string SetRoom()
        {
           return spawnRoom;
        }

        // This displays all the details for the current room to the player
        public void RoomInfo(Socket UserSocket, SQLiteConnection connection, Dictionary<Socket, Character> clientDictonary)
        {

            ASCIIEncoding encoder = new ASCIIEncoding();
            string returnMessage = "";

            Character character = clientDictonary[UserSocket];

            command = new sqliteCommand("select * from " + "table_characters" + " where name = " + "'" + character.name + "'", connection);

            var characterSearch = command.ExecuteReader();

            while(characterSearch.Read())
            {
                command = new sqliteCommand("select * from " + "table_dungeon" + " where name = " + "'" + characterSearch["room"] + "'", connection);
            }
            characterSearch.Close();

            var dungeonSearch = command.ExecuteReader();

            while (dungeonSearch.Read())
            {
               
                returnMessage += "-------------------------------";
                returnMessage += "\nName: " + dungeonSearch["name"];
                returnMessage += "\nDescription: " + dungeonSearch["description"];
                returnMessage += "\nNorth: " + dungeonSearch["North"];
                returnMessage += "\nSouth: " + dungeonSearch["South"];
                returnMessage += "\nEast: " + dungeonSearch["East"];
                returnMessage += "\nWest: " + dungeonSearch["West"];
                returnMessage += "\nUp: " + dungeonSearch["Up"];
                returnMessage += "\nDown: " + dungeonSearch["Down"];
                returnMessage += "\n-------------------------------";

            }

            byte[] sendbuffer = encoder.GetBytes(returnMessage);

            int bytesSent = UserSocket.Send(sendbuffer);


        }
        // This deals with any requests the player may have such as: movement, messages adn will display the help info
        public void Process(Dictionary<String, server.ReceiveThreadLaunchInfo> ConnectedClients, Character ClientCharacter, String Key, Socket UserSocket, Dictionary<Socket, Character> ClientDictonary, Dictionary<string,Socket> SocketDictonary, SQLiteConnection Connection)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();

            byte[] sendbuffer;

            int bytesSent;

            string returnMessage = "";

            var input = Key.Split(' ');

            command = new sqliteCommand("select * from " + "table_characters" + " where name = " + "'" + ClientCharacter.name + "'", Connection);

            var characterSearch = command.ExecuteReader();

            while (characterSearch.Read())
            {


                switch (input[0].ToLower())
                {
                    // This is used to display the different actions the player can make.
                    case "help":
                        returnMessage += ("\nCommands are ....");
                        returnMessage += ("\nhelp - for this screen");
                        returnMessage += ("\nlook - to look around");
                        returnMessage += ("\ngo [north | south | east | west | up | down]  - to travel between locations");
                        returnMessage += ("\nPress any key to continue");
                        Console.ReadKey(true);                        
                        break;

                    // This is called when the user wants to speak to other users in his area.
                    case "say":

                        returnMessage += ClientCharacter.name + " said ";

                        for (var i = 1; i < input.Length; i++)
                        {
                            returnMessage += (input[i] + " ");
                        }

                        command = new sqliteCommand("select * from " + "table_characters" + " where room = " + "'" + characterSearch["room"] + "'", Connection);

                        sendbuffer = encoder.GetBytes(returnMessage);
                        var sameRoomSearch = command.ExecuteReader();

                        while (sameRoomSearch.Read())
                        {
                            if (sameRoomSearch["name"] != ClientDictonary[UserSocket])
                            {
                                var clientName = sameRoomSearch["name"] as String;
                                
                                
                                foreach (KeyValuePair<String, server.ReceiveThreadLaunchInfo> pair in ConnectedClients)
                                {
                                    try
                                    {
                                        bytesSent = pair.Value.socket.Send(sendbuffer);
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Failed to send message to others in room");
                                    }
                                }
                                return;
                            }
                            
                        }
                        break;

                    // This is called by the user to travel between rooms if it's possible to do so.
                    case "go":

                        command = new sqliteCommand("select * from " + "table_dungeon" + " where name = " + "'" + characterSearch["room"] + "'", Connection);

                        var dungeonSearch = command.ExecuteReader();

                        while (dungeonSearch.Read())
                        {


                            // is arg[1] sensible?
                            if ((input[1].ToLower() != null))
                            {
                                command = new sqliteCommand("update table_characters set room = " + "'" + dungeonSearch[input[1].ToLower()] + "'" + " where name = " + "'" + characterSearch["name"] + "'", Connection);
                                command.ExecuteNonQuery();
                            }

                            else
                            {
                                //handle error
                                returnMessage += ("\nERROR");
                                returnMessage += ("\nCan not go " + input[1] + " from here");
                                returnMessage += ("\nPress any key to continue");
                            }
                                            
                        }
                        dungeonSearch.Close();
                        characterSearch.Close();
                        return;
                }

            }

            // Send the message for the user.
           sendbuffer = encoder.GetBytes(returnMessage);

           bytesSent = UserSocket.Send(sendbuffer);

        }
    }
}
