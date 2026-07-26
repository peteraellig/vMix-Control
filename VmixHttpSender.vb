Imports System.Net

' ============================================================================
' Sends vMix commands via HTTP GET to the vMix web API.
' ============================================================================
' By default (as long as vMix has it enabled under Settings > Web
' Controller), vMix answers HTTP GET requests of the form:
'   http://<IP>:<Port>/api/?Function=...&Input=...&SelectedName=...&Value=...
' The default port is 8088. A new HTTP connection is opened and closed for
' every single command (no keep-alive needed, but also somewhat slower than
' TCP when sending many commands in quick succession - see VmixTcpSender.vb).
Public Class VmixHttpSender
    Implements IVmixSender

    Public Property Ip As String = "127.0.0.1"
    Public Property Port As Integer = 8088

    Private lastCommandValue As String = ""

    Public ReadOnly Property LastCommand As String Implements IVmixSender.LastCommand
        Get
            Return lastCommandValue
        End Get
    End Property

    Public Function Send(command As String) As String Implements IVmixSender.Send
        Dim url As String = "http://" & Ip & ":" & Port.ToString() & "/api/?" & command
        lastCommandValue = url

        Try
            Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
            request.Method = "GET"
            ' 3 second timeout - a real vMix request usually takes under 5ms,
            ' but this value should still work comfortably over a somewhat
            ' slower network.
            request.Timeout = 3000

            Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                Using reader As New IO.StreamReader(response.GetResponseStream())
                    Return reader.ReadToEnd()
                End Using
            End Using
        Catch ex As Exception
            Return "Error (is vMix running? is the Web Controller enabled under Settings > Web Controller?): " & ex.Message
        End Try
    End Function

End Class
