Imports System.Net.Sockets
Imports System.Text

' ============================================================================
' Sends vMix commands via the TCP API (Settings > TCP Controllers, port
' usually 8099) instead of HTTP.
' ============================================================================
' Difference to HTTP: the connection stays open (a TcpClient is reused)
' instead of being opened and closed for every command - this saves the
' HTTP handshake when sending many commands in quick succession. The
' protocol itself is simpler than HTTP: a single text line
'   FUNCTION <function name> <remaining parameters>\r\n
' e.g. "Function=SetText&Input=X&SelectedName=Y&Value=Z" becomes the line
'   FUNCTION SetText Input=X&SelectedName=Y&Value=Z
' vMix acknowledges every command with a response line, e.g. "FUNCTION OK"
' or "FUNCTION FAILED reason...".
'
' IMPORTANT (a non-obvious vMix quirk): commands must arrive in the order
' they were sent - e.g. set the text fields first, only then reveal the
' overlay. Over a single TCP connection that's automatically guaranteed (TCP
' is an ordered byte-stream connection). Over HTTP it would NOT be
' guaranteed if multiple commands were sent at the same time/asynchronously -
' every HTTP request is an independent connection that can arrive at and be
' processed by the server in any order. That's why the real broadcast tools
' (Tennis26, SoccerClock, Volleyball24) always send their commands
' synchronously/serially, never in parallel/asynchronously - see the main
' form for a visible example of this quirk.
Public Class VmixTcpSender
    Implements IVmixSender, IDisposable

    Public Property Ip As String = "127.0.0.1"
    Public Property Port As Integer = 8099

    Private client As TcpClient
    Private stream As NetworkStream
    Private ReadOnly connectionLock As New Object()
    Private lastCommandValue As String = ""

    Public ReadOnly Property LastCommand As String Implements IVmixSender.LastCommand
        Get
            Return lastCommandValue
        End Get
    End Property

    ' Makes sure a connection exists - (re)connects if needed, e.g. on the
    ' very first command, after vMix was closed/restarted in the meantime, or
    ' if the IP/port changed.
    Private Sub EnsureConnected()
        If client IsNot Nothing AndAlso client.Connected Then Return

        DisconnectInternal()

        client = New TcpClient()
        client.Connect(Ip, Port)
        stream = client.GetStream()
    End Sub

    Public Function Send(command As String) As String Implements IVmixSender.Send
        SyncLock connectionLock
            Try
                EnsureConnected()

                ' "Function=SetText&Input=...&Value=..." -> "FUNCTION SetText Input=...&Value=..."
                Dim parts = command.Split({"&"c}, 2)
                Dim functionName As String = parts(0)
                If functionName.StartsWith("Function=") Then
                    functionName = functionName.Substring("Function=".Length)
                End If
                Dim remainder As String = If(parts.Length > 1, parts(1), "")

                Dim line As String = If(remainder = "", $"FUNCTION {functionName}", $"FUNCTION {functionName} {remainder}")
                lastCommandValue = line

                Dim bytes = Encoding.ASCII.GetBytes(line & vbCrLf)
                stream.Write(bytes, 0, bytes.Length)

                Return ReadResponseLine()
            Catch ex As Exception
                DisconnectInternal()
                Return "Error (is vMix running? is a controller active under Settings > TCP Controllers, usually port 8099?): " & ex.Message
            End Try
        End SyncLock
    End Function

    ' Short timeout when reading the response, so that a command vMix
    ' doesn't answer for some reason doesn't block the application.
    Private Function ReadResponseLine() As String
        stream.ReadTimeout = 1000
        Dim buffer(1023) As Byte
        Try
            Dim bytesRead = stream.Read(buffer, 0, buffer.Length)
            Return Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim()
        Catch ex As IO.IOException
            Return "(no response within 1 second)"
        End Try
    End Function

    Private Sub DisconnectInternal()
        Try
            If stream IsNot Nothing Then stream.Close()
            If client IsNot Nothing Then client.Close()
        Catch ex As Exception
            ' Errors while disconnecting are irrelevant here - the connection
            ' counts as closed afterward either way.
        End Try
        stream = Nothing
        client = Nothing
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        SyncLock connectionLock
            DisconnectInternal()
        End SyncLock
    End Sub

End Class
