﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

namespace CAN_client
{
    public static class Program
    {
        static Form1 affForm1;

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            affForm1 = new Form1();
            affForm1.ShowDialog();
            Application.Run();
        }

        public static TcpClient client;

       
        public static void OpenConnection(string HOST, int PORT)
        {
            if (client != null)
            {
                return;
            }
            else
            {
                try
                {
                    client = new TcpClient();
                    client.Connect(HOST, PORT);
                    SendData("LOGIN|" + Form1.login + "|" + Form1.passwd);
                }
                catch (Exception Ex)
                {
                 MessageBox.Show("Une erreur est survenue Motha fucka..\n\n" + Ex, "Oups, le serveur semble indisponible",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                 client = null;
                }
            }

        }

        public static void CloseConnection()
        {
            if (client == null)
            {
                return;
            }
            try
            {
                client.Close();
                client = null;
            }
            catch (Exception ex)
            {
                client = null;
            }

        }

        static void receiveData()
        {
            NetworkStream nwStream = client.GetStream();
            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
            if (Encoding.UTF8.GetString(bytesToRead, 0, bytesRead) == "ERREURLOGIN")
            {
                MessageBox.Show("Le Login / Mot de passe est incorrecte", "Erreur d'identification",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Form2 affForm2 = new Form2();
                
                Program.affForm1.Hide();
                affForm2.ShowDialog();
            }
        }
        public static void SendData(string data)
        {
            if (client == null)
            {
                return;
            }

            byte[] bytesToSend = UTF8Encoding.UTF8.GetBytes(data);
            NetworkStream nwStream = client.GetStream();
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            
        string[] s2 = data.Split('|');
        string instruction = s2[0];
        
        if (instruction == "LOGIN")             
            receiveData();

        }
    }
}
