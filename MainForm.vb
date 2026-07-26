' ============================================================================
' Main window of the vMix Control example
' ============================================================================
' This form helps new programmers understand how a Windows Forms application
' can send commands to vMix.
'
' Basic VB.NET words used in this file:
'
' Class
'   A class contains data and code. MainForm represents the main window.
'
' Sub
'   A Sub performs an action but does not return a value.
'
' Function
'   A Function performs an action and returns a result.
'
' Handles
'   Handles connects an event to a Sub. For example, a button Click event
'   runs the matching Button_Click Sub.
'
' Control
'   A control is an item on the form, such as a button, label, or text box.
'
' Common control name prefixes:
'   btn = Button       lbl = Label        txt = TextBox
'   cmb = ComboBox     rb  = RadioButton  chk = CheckBox
'
' The form performs these main tasks:
'   1. Read the connection settings.
'   2. Let the user select a vMix function.
'   3. Build a safe command.
'   4. Send the command using HTTP or TCP.
'   5. Show the sent command and the response from vMix.
Public Class MainForm

    ' ------------------------------------------------------------------------
    ' Example files and sender objects
    ' ------------------------------------------------------------------------

    ' Name and folder of the supplied example title.
    Private Const ExampleTitle As String = "example_title.gtzip"
    Private Const ExampleFolder As String = "C:\vmix\example\"

    ' These objects send commands to vMix.
    ' The program keeps both objects ready so the user can change or choose a protocol.
    Private ReadOnly httpSender As New VmixHttpSender()
    Private ReadOnly tcpSender As New VmixTcpSender()

    ' Remembers if the example title is currently visible.
    Private overlayShown As Boolean = False

    ' ------------------------------------------------------------------------
    ' Available vMix functions
    ' ------------------------------------------------------------------------

    ' Stores the information needed for one vMix function.
    '
    ' For example, SetText needs an input, a field name, and a new value.
    ' OverlayInput1Out only needs the function name.
    Private Class FunctionSpec
        Public NeedsInput As Boolean = True
        Public NeedsSelectedName As Boolean = False
        Public NeedsValue As Boolean = False
        Public PresetSelectedName As String = ""
        Public PresetValue As String = ""
    End Class

    ' Contains the small set of functions that have working UI examples.
    '
    ' VmixFunctionCatalog.Functions contains the complete vMix 29 reference.
    Private ReadOnly functionSpecs As Dictionary(Of String, FunctionSpec) = BuildDemoFunctionSpecs()

    ' Creates the small list of functions demonstrated by this form.
    '
    ' Each entry also contains example values for example_title.gtzip.
    Private Function BuildDemoFunctionSpecs() As Dictionary(Of String, FunctionSpec)
        Dim specs As New Dictionary(Of String, FunctionSpec)

        ' These functions change a field and therefore need a new value.
        specs("SetText") = New FunctionSpec With {.NeedsSelectedName = True, .NeedsValue = True, .PresetSelectedName = "name.Text", .PresetValue = "Test Text"}
        specs("SetImage") = New FunctionSpec With {.NeedsSelectedName = True, .NeedsValue = True, .PresetSelectedName = "logo.Source", .PresetValue = ExampleFolder & "logo.png"}
        specs("SetColor") = New FunctionSpec With {.NeedsSelectedName = True, .NeedsValue = True, .PresetSelectedName = "bg.Fill.Color", .PresetValue = "#FF2E8966"}
        ' SetTextColour changes the font color of a text field.
        ' SetColor changes a separate color or fill layer.
        specs("SetTextColour") = New FunctionSpec With {.NeedsSelectedName = True, .NeedsValue = True, .PresetSelectedName = "name.Text", .PresetValue = "white"}

        ' These functions show or hide a field without changing its value.
        specs("SetTextVisibleOn") = New FunctionSpec With {.NeedsSelectedName = True, .PresetSelectedName = "name.Text"}
        specs("SetTextVisibleOff") = New FunctionSpec With {.NeedsSelectedName = True, .PresetSelectedName = "name.Text"}
        specs("SetImageVisibleOn") = New FunctionSpec With {.NeedsSelectedName = True, .PresetSelectedName = "logo.Source"}
        specs("SetImageVisibleOff") = New FunctionSpec With {.NeedsSelectedName = True, .PresetSelectedName = "logo.Source"}

        ' This function starts one page of a title animation.
        specs("TitleBeginAnimation") = New FunctionSpec With {.NeedsValue = True, .PresetValue = "Page1"}

        ' These functions show or hide an input on Overlay 1.
        specs("OverlayInput1IN") = New FunctionSpec()
        specs("OverlayInput1Out") = New FunctionSpec With {.NeedsInput = False}

        ' Addinput adds a new input to vMix.
        ' Repeating this command creates another copy of the input.
        specs("Addinput") = New FunctionSpec With {.NeedsInput = False, .NeedsValue = True, .PresetValue = "Title|" & ExampleFolder & "example_title.gtzip"}

        Return specs
    End Function

    ' ------------------------------------------------------------------------
    ' Form startup and shutdown
    ' ------------------------------------------------------------------------

    ' Creates the form and all controls placed on it in the Designer.
    Public Sub New()
        InitializeComponent()
    End Sub

    ' Runs when the main window opens.
    '
    ' It applies the connection settings, fills the function list, and
    ' selects the first example.
    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplyConnectionSettings()

        ' Add the commands shown in the Function list.
        cmbFunction.Items.Clear()
        cmbFunction.Items.AddRange({"SetText", "SetImage", "SetColor", "SetTextColour", "SetTextVisibleOn", "SetTextVisibleOff", "SetImageVisibleOn", "SetImageVisibleOff", "TitleBeginAnimation", "OverlayInput1IN", "OverlayInput1Out", "Addinput"})

        ' Select the first example.
        ' This also updates the fields and command preview.
        cmbFunction.SelectedIndex = 0
    End Sub

    ' Runs when the main window closes.
    '
    ' The TCP sender keeps a network connection open while the program runs.
    ' Dispose closes this connection and releases its resources.
    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        tcpSender.Dispose()
    End Sub

    ' ------------------------------------------------------------------------
    ' Function selection
    ' ------------------------------------------------------------------------

    ' Runs when the user selects another vMix function.
    '
    ' It enables the required fields, inserts example values, and updates
    ' the command preview.
    Private Sub cmbFunction_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFunction.SelectedIndexChanged
        UpdateFieldAvailability()
        ApplyFunctionPreset()
        UpdatePreview()
    End Sub

    ' Disables fields that are not needed by the selected vMix function.
    Private Sub UpdateFieldAvailability()
        Dim spec = functionSpecs(cmbFunction.Text)
        txtInput.Enabled = spec.NeedsInput
        txtSelectedName.Enabled = spec.NeedsSelectedName
        txtValue.Enabled = spec.NeedsValue
    End Sub

    ' Inserts working example values for the selected vMix function.
    Private Sub ApplyFunctionPreset()
        Dim spec = functionSpecs(cmbFunction.Text)
        txtInput.Text = If(spec.NeedsInput, ExampleTitle, "")
        txtSelectedName.Text = spec.PresetSelectedName
        txtValue.Text = spec.PresetValue
    End Sub

    ' ------------------------------------------------------------------------
    ' Connection settings
    ' ------------------------------------------------------------------------

    ' Copies the IP address and port numbers from the form into both senders.
    '
    ' This does not contact vMix. A connection is made later when a command
    ' is sent or when the user tests the connection.
    Private Sub ApplyConnectionSettings()
        httpSender.Ip = txtIp.Text.Trim()
        tcpSender.Ip = txtIp.Text.Trim()

        Dim httpPort As Integer
        If Integer.TryParse(txtHttpPort.Text.Trim(), httpPort) Then httpSender.Port = httpPort

        Dim tcpPort As Integer
        If Integer.TryParse(txtTcpPort.Text.Trim(), tcpPort) Then tcpSender.Port = tcpPort
    End Sub

    ' Runs when the user changes the IP address, a port, or the protocol.
    Private Sub ConnectionField_Changed(sender As Object, e As EventArgs) Handles txtIp.TextChanged, txtHttpPort.TextChanged, txtTcpPort.TextChanged, rbHttp.CheckedChanged, rbTcp.CheckedChanged
        ApplyConnectionSettings()
    End Sub

    ' Returns the sender selected by the HTTP or TCP radio button.
    Private Function CurrentSender() As IVmixSender
        If rbHttp.Checked Then
            Return httpSender
        Else
            Return tcpSender
        End If
    End Function

    ' Returns the name of the protocol selected on the form.
    Private Function ProtocolLabel() As String
        Return If(rbHttp.Checked, "HTTP", "TCP")
    End Function

    ' Runs when the user clicks "Test connection".
    '
    ' It sends an empty request. If vMix answers, the selected controller
    ' is available.
    Private Sub btnTestConnection_Click(sender As Object, e As EventArgs) Handles btnTestConnection.Click
        Dim activeSender = CurrentSender()
        Dim result As String = activeSender.Send("")
        Dim ok As Boolean = Not result.StartsWith("Error")

        lblConnectionStatus.Text = If(ok, $"vMix found via {ProtocolLabel()}.", result)
        lblConnectionStatus.ForeColor = If(ok, Color.Green, Color.Red)
    End Sub

    ' Fetches the current vMix status and shows it in a new window.
    '
    ' The returned XML contains the exact names of inputs and title fields.
    ' This helps when a space is difficult to see in the vMix Title Editor.
    '
    ' The status document is always requested through HTTP.
    Private Sub btnFetchState_Click(sender As Object, e As EventArgs) Handles btnFetchState.Click
        Dim xml As String = httpSender.Send("")
        If xml.StartsWith("Error") Then
            MessageBox.Show(xml, "Could not fetch vMix status", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Using viewer As New StatusViewerForm(xml)
            viewer.ShowDialog(Me)
        End Using
    End Sub

    ' ------------------------------------------------------------------------
    ' Command builder and preview
    ' ------------------------------------------------------------------------

    ' Runs when the user changes one of the command fields.
    '
    ' It updates the preview only. It does not send anything to vMix.
    Private Sub BuilderField_Changed(sender As Object, e As EventArgs) Handles txtInput.TextChanged, txtSelectedName.TextChanged, txtValue.TextChanged, chkRawValue.CheckedChanged
        UpdatePreview()
    End Sub

    ' Shows the command, encoding result, and example VB.NET code.
    '
    ' This method does not send anything to vMix.
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

    ' Builds the command required by the selected vMix function.
    '
    ' Different functions need different parameters. This method chooses
    ' the correct builder function.
    Private Function BuildRawCommand(spec As FunctionSpec, func As String, input As String, selectedName As String, value As String, skipValueEncoding As Boolean) As String
        If spec.NeedsSelectedName AndAlso spec.NeedsValue Then
            If skipValueEncoding Then
                ' Test mode: send Value without encoding it.
                ' Input and SelectedName are still encoded.
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

    ' Creates the VB.NET example shown at the bottom of the form.
    '
    ' The example changes when the user selects another vMix function.
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

    ' Adds quotation marks correctly when text is shown as VB.NET code.
    Private Function EscapeForVbString(s As String) As String
        Return If(s, "").Replace("""", """""")
    End Function

    ' ------------------------------------------------------------------------
    ' Sending commands and example buttons
    ' ------------------------------------------------------------------------

    ' Runs when the user clicks "Build and send command".
    Private Sub btnBuildAndSend_Click(sender As Object, e As EventArgs) Handles btnBuildAndSend.Click
        SendCurrentBuilderCommand()
    End Sub

    ' Sends the current command to vMix.
    '
    ' It also displays what was sent and the response returned by vMix.
    Private Sub SendCurrentBuilderCommand()
        UpdatePreview()
        Dim rawCommand As String = txtRawCommand.Text

        Dim activeSender = CurrentSender()
        Dim result As String = activeSender.Send(rawCommand)
        txtProtocolTranslation.Text = activeSender.LastCommand
        txtResponse.Text = result
    End Sub

    ' Runs when the user clicks the special-character example.
    '
    ' The example contains characters that must be encoded in an API command.
    Private Sub btnDemoSpecialChars_Click(sender As Object, e As EventArgs) Handles btnDemoSpecialChars.Click
        cmbFunction.SelectedItem = "SetText"
        txtValue.Text = "Team A & Co/100% Café #1"
        SendCurrentBuilderCommand()
    End Sub

    ' Runs when the user clicks the "Set image" example button.
    Private Sub btnDemoSetImage_Click(sender As Object, e As EventArgs) Handles btnDemoSetImage.Click
        cmbFunction.SelectedItem = "SetImage"
        SendCurrentBuilderCommand()
    End Sub

    ' Shows the example title when it is hidden and hides it when it is shown.
    Private Sub btnDemoOverlay_Click(sender As Object, e As EventArgs) Handles btnDemoOverlay.Click
        cmbFunction.SelectedItem = If(overlayShown, "OverlayInput1Out", "OverlayInput1IN")
        SendCurrentBuilderCommand()
        overlayShown = Not overlayShown
    End Sub

    ' Makes the example color layer fully transparent.
    Private Sub btnDemoTransparent_Click(sender As Object, e As EventArgs) Handles btnDemoTransparent.Click
        cmbFunction.SelectedItem = "SetColor"
        txtValue.Text = "#00000000"
        SendCurrentBuilderCommand()
    End Sub

End Class
