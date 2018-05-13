using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;

using System.IO;
using System.Security.Cryptography;

namespace Client
{
    class client
    { 

         
        // Helper Function to keep character creation clean and then send the information to the server.
        
        static void characterCreationLogin(Socket serverSocket)
        {
            string login;

            string serverSalt = "";

            bool saltArrived = false;

            ConsoleKeyInfo passwordInterceptor;

            StringBuilder password = new StringBuilder();

            bool loginCompleted = false;
            //Displays login Instructions 
            while (loginCompleted == false)
            {
                Console.WriteLine("Please Login");
                Console.WriteLine("Your Username can only be up to 20 characters long.");
                Console.WriteLine("It mustn't conatain any special characters");

                login = Console.ReadLine();

  

                Console.Clear();
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] logindetails = encoder.GetBytes(login);

                try
                {
                    int bytesSent = serverSocket.Send(logindetails);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex);
                }

                while (saltArrived == false)
                {
                    try
                    {
                        byte[] buffer = new byte[4096];
                        int result;

                        result = serverSocket.Receive(buffer);

                        if (result > 0)
                        {
                            serverSalt = encoder.GetString(buffer, 0, result);
                            saltArrived = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Clear();
                        Console.WriteLine("Waiting for Server Response");
                    }
                }



                //Displays password instructions
                Console.WriteLine("Please enter your password");
                Console.WriteLine("It will be hidden while typing");

                passwordInterceptor = Console.ReadKey(true);

                while (passwordInterceptor.Key != ConsoleKey.Enter)
                {
                    password.Append(passwordInterceptor.KeyChar);
                    passwordInterceptor = Console.ReadKey(true);
                }

                Console.Clear();

                loginCompleted = SpecialCharacterCheck(login);
                loginCompleted = SpecialCharacterCheck(password.ToString());

                if (loginCompleted == true)
                {
                    byte[] userdetails = encoder.GetBytes(login);

                    try
                    {
                        string stringword = password.ToString();
                        login += " " + GenerateSaltedHash(encoder.GetBytes(stringword), encoder.GetBytes(serverSalt)) + " " + serverSalt.ToString();
                        logindetails = encoder.GetBytes(login);
                        int bytesSent = serverSocket.Send(logindetails);
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }

           
        }

        
         // This is where the salt and password are used to make a hashed password for use in the server.
         
        static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        
         // This  thread  collects information sent from the server and then display it for the client.
          
        static void clientRecieve(object o)
        {
            bool ServerConnection = true;

            Socket morseClient = (Socket)o;

            IPEndPoint ipLocal = new IPEndPoint(IPAddress.Parse("138.68.189.227"), 8221);

            while (ServerConnection == true)
            {

                try
                {
                    byte[] buffer = new byte[4096];
                    int result;

                    result = morseClient.Receive(buffer);

                    if (result > 0)
                    {
                        ASCIIEncoding encoder = new ASCIIEncoding();
                        String recdMsg = encoder.GetString(buffer, 0, result);

                        Console.WriteLine(recdMsg);

                    }
                }
                catch (Exception)
                {
                    ServerConnection = false;
                    Console.WriteLine("Server Connection Terminated!");
                }

            }
		
	        while (ServerConnection == false) 
		    {
		    	try 
		    	{
	    			morseClient.Connect (ipLocal);
                    ServerConnection = true;
	    		} 
	    		catch (Exception) 
	    		{
	    			Thread.Sleep (1000);
	    		}
	    	}
        }

        
         // This stops the player from typing and special characters
         
        static bool SpecialCharacterCheck(string userInput)
        {
            string specialCharacters = "`¬¦!\"£$%^&*()-_=+[{]}'#;:'#/?.,<>\\|";

            foreach (var character in specialCharacters)
            {
                if (userInput.Contains(character))
                {
                    Console.WriteLine("Special Characters aren't allowed.");
                    return false;
                }

            }

            return true;
        }

        //This is the entry point for the client.  
        static void Main(string[] args)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ipLocal = new IPEndPoint(IPAddress.Parse("138.68.189.227"), 8221);

            bool connected = false;
            //Tries to connect to the server
			while (connected == false) 
			{
				try 
				{
					s.Connect (ipLocal);
					connected = true;
				} 
				catch (Exception) 
				{
					Thread.Sleep (1000);
				}
			}
            //Goes through character creation
            characterCreationLogin(s);

            Thread myThread = new Thread(clientRecieve);
            myThread.Start(s);

            int ID = 0;

            while (true)
            {
                String Msg = Console.ReadLine();
                ID++;
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] buffer = encoder.GetBytes(Msg);
                if (SpecialCharacterCheck(Msg))
                {
                    try
                    {
                        int bytesSent = s.Send(buffer);
                        Console.Clear();
                        Console.WriteLine(Msg + "\nSent Request to Server");
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
                

                Thread.Sleep(1000);
            }
        }
    }
}
