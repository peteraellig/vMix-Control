' ============================================================================
' vMixControl - Reference/learning sample for controlling vMix
' ============================================================================
' Shows on a single form how the three building blocks work together, the
' same ones used in the real broadcast tools (Tennis26, SoccerClock,
' Volleyball24):
'   1. VmixCommandBuilder.vb  - builds "Function=X&Input=Y&SelectedName=Z&Value=W"
'                               (or one of the other shapes, see FunctionSpec
'                               below) and URL-encodes all dynamic parts.
'   2. IVmixSender.vb / VmixHttpSender.vb / VmixTcpSender.vb
'                             - translate that protocol-neutral string into
'                               either an HTTP GET request or a TCP protocol
'                               line.
'   3. This form                - assembles commands through the UI, makes
'                               every step visible (raw -> encoded ->
'                               actually sent -> response), AND shows the
'                               VB.NET code you'd write to reproduce exactly
'                               that in your own project.
Public Class MainForm

    ' The sample title this demo project works with - lives at
    ' C:\vmix\example\example_title.gtzip with the fields name.Text,
    ' function.Text, logo.Source, bg.Fill.Color, bg_round.Fill.Color.
    Private Const ExampleTitle As String = "example_title.gtzip"
    Private Const ExampleFolder As String = "C:\vmix\example\"

    Private ReadOnly httpSender As New VmixHttpSender()
    Private ReadOnly tcpSender As New VmixTcpSender()

    ' Remembers, for the overlay demo button, whether the title is currently
    ' shown, so a click reliably toggles between show/hide.
    Private overlayShown As Boolean = False

    ' Describes which of the three fields (Input/SelectedName/Value) a given
    ' vMix function actually needs, plus matching preset values for
    ' example_title.gtzip. One function table instead of four parallel
    ' If/Select blocks in UpdateFieldAvailability/ApplyFunctionPreset/
    ' BuildRawCommand/BuildCodeSnippet - add a new command here and the rest
    ' of the form follows automatically.
    Private Class FunctionSpec
        Public NeedsInput As Boolean = True
        Public NeedsSelectedName As Boolean = False
        Public NeedsValue As Boolean = False
        Public PresetSelectedName As String = ""
        Public PresetValue As String = ""
    End Class

    Private ReadOnly functionSpecs As Dictionary(Of String, FunctionSpec) = BuildFunctionSpecs()

    Private Function BuildFunctionSpecs() As Dictionary(Of String, FunctionSpec)
        Dim specs As New Dictionary(Of String, FunctionSpec)

        ' Function+Input+SelectedName+Value (BuildVmixSetCommand)
        specs("SetText") = New FunctionSpec With {.NeedsSelectedName = True, .NeedsValue = True, .PresetSelectedName = "name.Text", .PresetValue = "Test Text"}
        specs("SetImage") = New FunctionSpec With {.NeedsSelectedName = True, .NeedsValue = True, .PresetSelectedName = "logo.Source", .PresetValue = ExampleFolder & "logo.png"}
        specs("SetColor") = New FunctionSpec With {.NeedsSelectedName = True, .NeedsValue = True, .PresetSelectedName = "bg.Fill.Color", .PresetValue = "#FF2E8966"}
        ' SetTextColour sets a text field's font color (not to be confused
        ' with SetColor, which sets a dedicated Color/Fill layer) - accepts
        ' both simple names ("white"/"red") and hex values.
        specs("SetTextColour") = New FunctionSpec With {.NeedsSelectedName = True, .NeedsValue = True, .PresetSelectedName = "name.Text", .PresetValue = "white"}

        ' Function+Input+SelectedName, no Value (BuildVmixSelectCommand)
        specs("SetTextVisibleOn") = New FunctionSpec With {.NeedsSelectedName = True, .PresetSelectedName = "name.Text"}
        specs("SetTextVisibleOff") = New FunctionSpec With {.NeedsSelectedName = True, .PresetSelectedName = "name.Text"}
        specs("SetImageVisibleOn") = New FunctionSpec With {.NeedsSelectedName = True, .PresetSelectedName = "logo.Source"}
        specs("SetImageVisibleOff") = New FunctionSpec With {.NeedsSelectedName = True, .PresetSelectedName = "logo.Source"}

        ' Function+Input+Value, no SelectedName (BuildVmixCommand)
        specs("TitleBeginAnimation") = New FunctionSpec With {.NeedsValue = True, .PresetValue = "Page1"}

        ' Function+Input, no SelectedName/Value (BuildVmixInputCommand)
        specs("OverlayInput1IN") = New FunctionSpec()
        ' Function alone, no Input/SelectedName/Value
        specs("OverlayInput1Out") = New FunctionSpec With {.NeedsInput = False}

        ' Function+Value, no Input/SelectedName (BuildVmixValueOnlyCommand) -
        ' adds a NEW input, doesn't reference an existing one.
        specs("Addinput") = New FunctionSpec With {.NeedsInput = False, .NeedsValue = True, .PresetValue = "Title|" & ExampleFolder & "example_title.gtzip"}

        Return specs
    End Function

    Public Sub New()
        ' This call is required by the designer - without it, every control
        ' (lblIntro, txtIp, ...) stays Nothing, since they're only
        ' instantiated here in MainForm.Designer.vb.
        InitializeComponent()
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblIntro.Text =
            "This sample project shows how vMix control works in Peter's broadcast tools:" & vbCrLf &
            "vMix is controlled via HTTP GET (Web Controller, usually port 8088) OR via TCP (TCP Controller, usually port 8099) - both understand the same" & vbCrLf &
            "command syntax ""Function=X&Input=Y&SelectedName=Z&Value=W"". Below you can build commands by hand, see the generated code, and send it live." & vbCrLf &
            "Important: always send vMix commands synchronously/serially (never in parallel/asynchronously) - vMix must process them in the order they were sent (e.g. set the text first, only then reveal the overlay). Details in the comment in VmixTcpSender.vb."

        lblConnHint.Text = "Enable this in vMix under Settings > Web Controller (HTTP) resp. Settings > TCP Controllers (TCP). ""Test connection"" sends an empty command - vMix still responds if it's reachable."

        lblBuilderHint.Text =
            "Choosing a Function automatically fills Input/SelectedName/Value with a matching example for example_title.gtzip - just overwrite them for your own tests." & vbCrLf &
            "Greyed-out fields aren't needed by the selected function (see FunctionSpec in MainForm.vb - just add new commands there)." & vbCrLf &
            """Don't encode Value"" applies to any function with a Value field - useful e.g. to compare Addinput paths with/without encoding."

        ApplyConnectionSettings()

        cmbFunction.Items.Clear()
        cmbFunction.Items.AddRange({"SetText", "SetImage", "SetColor", "SetTextColour", "SetTextVisibleOn", "SetTextVisibleOff", "SetImageVisibleOn", "SetImageVisibleOff", "TitleBeginAnimation", "OverlayInput1IN", "OverlayInput1Out", "Addinput"})
        ' Triggers cmbFunction_SelectedIndexChanged (ComboBox.SelectedIndex
        ' starts at -1) - that fills the fields, availability, and preview in
        ' one go, no separate call needed here.
        cmbFunction.SelectedIndex = 0
    End Sub

    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        tcpSender.Dispose()
    End Sub

    Private Sub cmbFunction_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFunction.SelectedIndexChanged
        UpdateFieldAvailability()
        ApplyFunctionPreset()
        UpdatePreview()
    End Sub

    ' Greys out the fields that the currently selected function doesn't use
    ' at all - makes it immediately visible which vMix commands need which
    ' parameters.
    Private Sub UpdateFieldAvailability()
        Dim spec = functionSpecs(cmbFunction.Text)
        txtInput.Enabled = spec.NeedsInput
        txtSelectedName.Enabled = spec.NeedsSelectedName
        txtValue.Enabled = spec.NeedsValue
    End Sub

    ' Suggests a sensible example for the selected function, matching
    ' example_title.gtzip (C:\vmix\example\). Deliberately overwrites the
    ' fields on every function change, so a working example is always shown
    ' instead of leftovers from the previous choice.
    Private Sub ApplyFunctionPreset()
        Dim spec = functionSpecs(cmbFunction.Text)
        txtInput.Text = If(spec.NeedsInput, ExampleTitle, "")
        txtSelectedName.Text = spec.PresetSelectedName
        txtValue.Text = spec.PresetValue
    End Sub

    ' Pushes IP/port from the textboxes into both senders. Called once at
    ' startup (see MainForm_Load) and again whenever one of the connection
    ' fields actually changes (see ConnectionField_Changed below) - NOT on
    ' every single send. Re-parsing the same strings before every command
    ' would be pointless work when nothing changed since the last one; the
    ' real broadcast tools apply IP/port/protocol at clear points too (form
    ' load, "Save Settings"), not inline in the send path.
    Private Sub ApplyConnectionSettings()
        httpSender.Ip = txtIp.Text.Trim()
        tcpSender.Ip = txtIp.Text.Trim()

        Dim httpPort As Integer
        If Integer.TryParse(txtHttpPort.Text.Trim(), httpPort) Then httpSender.Port = httpPort

        Dim tcpPort As Integer
        If Integer.TryParse(txtTcpPort.Text.Trim(), tcpPort) Then tcpSender.Port = tcpPort
    End Sub

    Private Sub ConnectionField_Changed(sender As Object, e As EventArgs) Handles txtIp.TextChanged, txtHttpPort.TextChanged, txtTcpPort.TextChanged, rbHttp.CheckedChanged, rbTcp.CheckedChanged
        ApplyConnectionSettings()
    End Sub

    ' Both senders are always kept up to date by ApplyConnectionSettings -
    ' picking one here is just a matter of which protocol is currently
    ' selected, no parsing needed.
    Private Function CurrentSender() As IVmixSender
        If rbHttp.Checked Then
            Return httpSender
        Else
            Return tcpSender
        End If
    End Function

    Private Function ProtocolLabel() As String
        Return If(rbHttp.Checked, "HTTP", "TCP")
    End Function

    Private Sub btnTestConnection_Click(sender As Object, e As EventArgs) Handles btnTestConnection.Click
        Dim activeSender = CurrentSender()
        ' An empty command is enough as a reachability test - vMix still
        ' answers an empty Function value as long as the controller is
        ' reachable; only if the connection itself fails does the "Error
        ' ..." message from the respective sender come back.
        Dim result As String = activeSender.Send("")
        Dim ok As Boolean = Not result.StartsWith("Error")

        lblConnectionStatus.Text = If(ok, $"vMix found via {ProtocolLabel()}.", result)
        lblConnectionStatus.ForeColor = If(ok, Color.Green, Color.Red)
    End Sub

    ' Fetches the current vMix status and shows it in its own window -
    ' ".../api/?" with no Function parameter at all returns an XML list of
    ' every loaded input with its exact field names (<text name="...">,
    ' <image name="...">, ...) - the Title Editor barely shows spaces in
    ' there, the XML response shows them exactly. Deliberately always uses
    ' HTTP, regardless of the HTTP/TCP choice above.
    Private Sub btnFetchState_Click(sender As Object, e As EventArgs) Handles btnFetchState.Click
        ' httpSender.Ip/Port are already current (see ApplyConnectionSettings) -
        ' no need to re-read the textboxes here.
        Dim xml As String = httpSender.Send("")
        If xml.StartsWith("Error") Then
            MessageBox.Show(xml, "Could not fetch vMix status", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Using viewer As New StatusViewerForm(xml)
            viewer.ShowDialog(Me)
        End Using
    End Sub

    ' Runs on every change to Function/Input/SelectedName/Value/the encoding
    ' checkbox - updates the raw command, encoding preview, and code sample
    ' IMMEDIATELY, without contacting vMix at all. Only clicking "Build and
    ' send command" actually sends anything.
    Private Sub BuilderField_Changed(sender As Object, e As EventArgs) Handles txtInput.TextChanged, txtSelectedName.TextChanged, txtValue.TextChanged, chkRawValue.CheckedChanged
        UpdatePreview()
    End Sub

    Private Sub UpdatePreview()
        Dim func As String = cmbFunction.Text
        Dim input As String = txtInput.Text
        Dim selectedName As String = txtSelectedName.Text
        Dim value As String = txtValue.Text
        Dim spec = functionSpecs(func)

        Dim rawCommand As String = BuildRawCommand(spec, func, input, selectedName, value, chkRawValue.Checked)
        txtRawCommand.Text = rawCommand

        If Not spec.NeedsValue Then
            txtEncodingDemo.Text = "(this function has no Value)"
        ElseIf chkRawValue.Checked Then
            txtEncodingDemo.Text = $"""{value}""   ->   (not encoded, sent raw)"
        Else
            txtEncodingDemo.Text = $"""{value}""   ->   ""{EncodeVmixValue(value)}"""
        End If

        txtCodeSnippet.Text = BuildCodeSnippet(spec, func, input, selectedName, value)
    End Sub

    ' Builds the protocol-neutral "Function=...&..." string matching the
    ' FunctionSpec - which of the four BuildVmix... shapes gets used only
    ' depends on which fields the function actually needs.
    Private Function BuildRawCommand(spec As FunctionSpec, func As String, input As String, selectedName As String, value As String, skipValueEncoding As Boolean) As String
        If spec.NeedsSelectedName AndAlso spec.NeedsValue Then
            If skipValueEncoding Then
                ' Test mode: append Value unchanged - Input/SelectedName
                ' still get encoded; a space in the title/field name is a
                ' separate, independent problem (see VmixCommandBuilder.vb).
                Return "Function=" & func & "&Input=" & EncodeVmixValue(input) & "&SelectedName=" & EncodeVmixValue(selectedName) & "&Value=" & value
            End If
            Return BuildVmixSetCommand(func, input, selectedName, value)

        ElseIf spec.NeedsSelectedName Then
            Return BuildVmixSelectCommand(func, input, selectedName)

        ElseIf spec.NeedsValue AndAlso spec.NeedsInput Then
            If skipValueEncoding Then
                Return "Function=" & func & "&Input=" & EncodeVmixValue(input) & "&Value=" & value
            End If
            Return BuildVmixCommand(func, input, value)

        ElseIf spec.NeedsValue Then
            If skipValueEncoding Then
                Return "Function=" & func & "&Value=" & value
            End If
            Return BuildVmixValueOnlyCommand(func, value)

        ElseIf spec.NeedsInput Then
            Return BuildVmixInputCommand(func, input)

        Else
            Return BuildVmixInputCommand(func, "")
        End If
    End Function

    ' Shows not just the current action as code, but the complete
    ' integration: which files you need, how to set up a sender, and how a
    ' button click builds and sends a command from that - exactly the
    ' questions someone new to this system asks first.
    Private Function BuildCodeSnippet(spec As FunctionSpec, func As String, input As String, selectedName As String, value As String) As String
        Dim i As String = EscapeForVbString(input)
        Dim sName As String = EscapeForVbString(selectedName)
        Dim v As String = EscapeForVbString(value)

        Dim builderCall As String
        If spec.NeedsSelectedName AndAlso spec.NeedsValue Then
            builderCall = $"BuildVmixSetCommand(""{func}"", ""{i}"", ""{sName}"", ""{v}"")"
        ElseIf spec.NeedsSelectedName Then
            builderCall = $"BuildVmixSelectCommand(""{func}"", ""{i}"", ""{sName}"")"
        ElseIf spec.NeedsValue AndAlso spec.NeedsInput Then
            builderCall = $"BuildVmixCommand(""{func}"", ""{i}"", ""{v}"")"
        ElseIf spec.NeedsValue Then
            builderCall = $"BuildVmixValueOnlyCommand(""{func}"", ""{v}"")"
        ElseIf spec.NeedsInput Then
            builderCall = $"BuildVmixInputCommand(""{func}"", ""{i}"")"
        Else
            builderCall = $"BuildVmixInputCommand(""{func}"", """")"
        End If

        ' This mirrors exactly what MainForm.vb itself does (httpSender/
        ' tcpSender fields, ApplyConnectionSettings() applied at startup and
        ' on change events rather than on every send, CurrentSender() just
        ' picking by rbHttp.Checked) - not a simplified stand-in. Swap in
        ' your own controls' names, or replace them with fixed values/
        ' settings if your project has no such UI.
        Return "' 1) Copy these 4 files into your project:" & vbCrLf &
            "'    IVmixSender.vb, VmixHttpSender.vb, VmixTcpSender.vb, VmixCommandBuilder.vb" & vbCrLf &
            "'" & vbCrLf &
            "' 2) Keep one sender per protocol alive as fields:" & vbCrLf &
            "Private ReadOnly httpSender As New VmixHttpSender()" & vbCrLf &
            "Private ReadOnly tcpSender As New VmixTcpSender()" & vbCrLf &
            vbCrLf &
            "' 3) Push IP/port into both senders once at startup, and again whenever" & vbCrLf &
            "'    they actually change - NOT on every single send (here: txtIp/" & vbCrLf &
            "'    txtHttpPort/txtTcpPort/rbHttp - same control names as in this project):" & vbCrLf &
            "Private Sub ApplyConnectionSettings()" & vbCrLf &
            "    httpSender.Ip = txtIp.Text.Trim()" & vbCrLf &
            "    tcpSender.Ip = txtIp.Text.Trim()" & vbCrLf &
            "    Dim httpPort As Integer" & vbCrLf &
            "    If Integer.TryParse(txtHttpPort.Text.Trim(), httpPort) Then httpSender.Port = httpPort" & vbCrLf &
            "    Dim tcpPort As Integer" & vbCrLf &
            "    If Integer.TryParse(txtTcpPort.Text.Trim(), tcpPort) Then tcpSender.Port = tcpPort" & vbCrLf &
            "End Sub" & vbCrLf &
            "' Call ApplyConnectionSettings() once in Form_Load, and again from a" & vbCrLf &
            "' TextChanged/CheckedChanged handler on those same controls (see" & vbCrLf &
            "' ConnectionField_Changed here) - then picking a sender is just:" & vbCrLf &
            "Private Function CurrentSender() As IVmixSender" & vbCrLf &
            "    Return If(rbHttp.Checked, CType(httpSender, IVmixSender), tcpSender)" & vbCrLf &
            "End Function" & vbCrLf &
            "' (dispose tcpSender when your form closes, see MainForm_FormClosing here)" & vbCrLf &
            vbCrLf &
            "' 4) In a button click, build the command and send it:" & vbCrLf &
            "Private Sub btnMyButton_Click(sender As Object, e As EventArgs) Handles btnMyButton.Click" & vbCrLf &
            "    Dim command As String = " & builderCall & vbCrLf &
            "    CurrentSender().Send(command)" & vbCrLf &
            "End Sub"
    End Function

    Private Function EscapeForVbString(s As String) As String
        Return If(s, "").Replace("""", """""")
    End Function

    Private Sub btnBuildAndSend_Click(sender As Object, e As EventArgs) Handles btnBuildAndSend.Click
        SendCurrentBuilderCommand()
    End Sub

    ' The heart of the demo: refreshes the preview (in case it hasn't run
    ' yet), sends the command via the currently selected sender, and finally
    ' shows what actually went over the wire as well as vMix's response.
    Private Sub SendCurrentBuilderCommand()
        UpdatePreview()
        Dim rawCommand As String = txtRawCommand.Text

        Dim activeSender = CurrentSender()
        Dim result As String = activeSender.Send(rawCommand)
        txtProtocolTranslation.Text = activeSender.LastCommand
        txtResponse.Text = result
    End Sub

    Private Sub btnDemoSpecialChars_Click(sender As Object, e As EventArgs) Handles btnDemoSpecialChars.Click
        ' Deliberately chosen characters that have a special meaning in a
        ' URL/query string (&, #, %, space, accented character) - shows
        ' exactly why EncodeVmixValue is needed: without encoding, the "&"
        ' alone would split the command into a new parameter mid-Value.
        cmbFunction.SelectedItem = "SetText"
        txtValue.Text = "Team A & Co/100% Café #1"
        SendCurrentBuilderCommand()
    End Sub

    Private Sub btnDemoSetImage_Click(sender As Object, e As EventArgs) Handles btnDemoSetImage.Click
        cmbFunction.SelectedItem = "SetImage"
        SendCurrentBuilderCommand()
    End Sub

    Private Sub btnDemoOverlay_Click(sender As Object, e As EventArgs) Handles btnDemoOverlay.Click
        cmbFunction.SelectedItem = If(overlayShown, "OverlayInput1Out", "OverlayInput1IN")
        SendCurrentBuilderCommand()
        overlayShown = Not overlayShown
    End Sub

    Private Sub btnDemoTransparent_Click(sender As Object, e As EventArgs) Handles btnDemoTransparent.Click
        ' Shows how to make a color fully transparent (alpha 00) - e.g. to
        ' "hide" a Color/Fill layer. Unlike text or image fields, there's no
        ' dedicated VisibleOff command for that (SetColorVisibleOff doesn't
        ' exist) - the usual approach is to make the color itself transparent.
        cmbFunction.SelectedItem = "SetColor"
        txtValue.Text = "#00000000"
        SendCurrentBuilderCommand()
    End Sub

End Class
