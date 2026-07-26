Partial Class MainForm
    Inherits System.Windows.Forms.Form

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    Friend WithEvents lblIntro As Label
    Friend WithEvents grpConnection As GroupBox
    Friend WithEvents lblIp As Label
    Friend WithEvents txtIp As TextBox
    Friend WithEvents lblHttpPort As Label
    Friend WithEvents txtHttpPort As TextBox
    Friend WithEvents lblTcpPort As Label
    Friend WithEvents txtTcpPort As TextBox
    Friend WithEvents rbHttp As RadioButton
    Friend WithEvents rbTcp As RadioButton
    Friend WithEvents btnTestConnection As Button
    Friend WithEvents btnFetchState As Button
    Friend WithEvents lblConnectionStatus As Label
    Friend WithEvents lblConnHint As Label

    Friend WithEvents grpBuilder As GroupBox
    Friend WithEvents lblFunction As Label
    Friend WithEvents cmbFunction As ComboBox
    Friend WithEvents lblInput As Label
    Friend WithEvents txtInput As TextBox
    Friend WithEvents lblSelectedName As Label
    Friend WithEvents txtSelectedName As TextBox
    Friend WithEvents lblValue As Label
    Friend WithEvents txtValue As TextBox
    Friend WithEvents chkRawValue As CheckBox
    Friend WithEvents btnBuildAndSend As Button
    Friend WithEvents btnDemoSpecialChars As Button
    Friend WithEvents btnDemoSetImage As Button
    Friend WithEvents btnDemoOverlay As Button
    Friend WithEvents btnDemoTransparent As Button
    Friend WithEvents lblBuilderHint As Label

    Friend WithEvents grpVisualize As GroupBox
    Friend WithEvents lblRawTitle As Label
    Friend WithEvents txtRawCommand As TextBox
    Friend WithEvents lblEncodingTitle As Label
    Friend WithEvents txtEncodingDemo As TextBox
    Friend WithEvents lblProtocolTitle As Label
    Friend WithEvents txtProtocolTranslation As TextBox
    Friend WithEvents lblResponseTitle As Label
    Friend WithEvents txtResponse As TextBox
    Friend WithEvents lblCodeTitle As Label
    Friend WithEvents txtCodeSnippet As TextBox

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.lblIntro = New System.Windows.Forms.Label()
        Me.grpConnection = New System.Windows.Forms.GroupBox()
        Me.lblIp = New System.Windows.Forms.Label()
        Me.txtIp = New System.Windows.Forms.TextBox()
        Me.lblHttpPort = New System.Windows.Forms.Label()
        Me.txtHttpPort = New System.Windows.Forms.TextBox()
        Me.lblTcpPort = New System.Windows.Forms.Label()
        Me.txtTcpPort = New System.Windows.Forms.TextBox()
        Me.rbHttp = New System.Windows.Forms.RadioButton()
        Me.rbTcp = New System.Windows.Forms.RadioButton()
        Me.btnTestConnection = New System.Windows.Forms.Button()
        Me.btnFetchState = New System.Windows.Forms.Button()
        Me.lblConnectionStatus = New System.Windows.Forms.Label()
        Me.lblConnHint = New System.Windows.Forms.Label()
        Me.grpBuilder = New System.Windows.Forms.GroupBox()
        Me.lblFunction = New System.Windows.Forms.Label()
        Me.cmbFunction = New System.Windows.Forms.ComboBox()
        Me.lblInput = New System.Windows.Forms.Label()
        Me.txtInput = New System.Windows.Forms.TextBox()
        Me.lblSelectedName = New System.Windows.Forms.Label()
        Me.txtSelectedName = New System.Windows.Forms.TextBox()
        Me.lblValue = New System.Windows.Forms.Label()
        Me.txtValue = New System.Windows.Forms.TextBox()
        Me.chkRawValue = New System.Windows.Forms.CheckBox()
        Me.btnBuildAndSend = New System.Windows.Forms.Button()
        Me.btnDemoSpecialChars = New System.Windows.Forms.Button()
        Me.btnDemoSetImage = New System.Windows.Forms.Button()
        Me.btnDemoOverlay = New System.Windows.Forms.Button()
        Me.btnDemoTransparent = New System.Windows.Forms.Button()
        Me.lblBuilderHint = New System.Windows.Forms.Label()
        Me.grpVisualize = New System.Windows.Forms.GroupBox()
        Me.lblRawTitle = New System.Windows.Forms.Label()
        Me.txtRawCommand = New System.Windows.Forms.TextBox()
        Me.lblEncodingTitle = New System.Windows.Forms.Label()
        Me.txtEncodingDemo = New System.Windows.Forms.TextBox()
        Me.lblProtocolTitle = New System.Windows.Forms.Label()
        Me.txtProtocolTranslation = New System.Windows.Forms.TextBox()
        Me.lblResponseTitle = New System.Windows.Forms.Label()
        Me.txtResponse = New System.Windows.Forms.TextBox()
        Me.lblCodeTitle = New System.Windows.Forms.Label()
        Me.txtCodeSnippet = New System.Windows.Forms.TextBox()
        Me.grpConnection.SuspendLayout()
        Me.grpBuilder.SuspendLayout()
        Me.grpVisualize.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblIntro
        '
        Me.lblIntro.Location = New System.Drawing.Point(12, 10)
        Me.lblIntro.Name = "lblIntro"
        Me.lblIntro.Size = New System.Drawing.Size(970, 100)
        Me.lblIntro.TabIndex = 0
        '
        'grpConnection
        '
        Me.grpConnection.Controls.Add(Me.lblIp)
        Me.grpConnection.Controls.Add(Me.txtIp)
        Me.grpConnection.Controls.Add(Me.lblHttpPort)
        Me.grpConnection.Controls.Add(Me.txtHttpPort)
        Me.grpConnection.Controls.Add(Me.lblTcpPort)
        Me.grpConnection.Controls.Add(Me.txtTcpPort)
        Me.grpConnection.Controls.Add(Me.rbHttp)
        Me.grpConnection.Controls.Add(Me.rbTcp)
        Me.grpConnection.Controls.Add(Me.btnTestConnection)
        Me.grpConnection.Controls.Add(Me.btnFetchState)
        Me.grpConnection.Controls.Add(Me.lblConnectionStatus)
        Me.grpConnection.Controls.Add(Me.lblConnHint)
        Me.grpConnection.Location = New System.Drawing.Point(12, 120)
        Me.grpConnection.Name = "grpConnection"
        Me.grpConnection.Size = New System.Drawing.Size(970, 110)
        Me.grpConnection.TabIndex = 1
        Me.grpConnection.TabStop = False
        Me.grpConnection.Text = "1) Connection"
        '
        'lblIp
        '
        Me.lblIp.Location = New System.Drawing.Point(15, 28)
        Me.lblIp.Name = "lblIp"
        Me.lblIp.Size = New System.Drawing.Size(25, 20)
        Me.lblIp.TabIndex = 0
        Me.lblIp.Text = "IP:"
        '
        'txtIp
        '
        Me.txtIp.Location = New System.Drawing.Point(45, 25)
        Me.txtIp.Name = "txtIp"
        Me.txtIp.Size = New System.Drawing.Size(110, 20)
        Me.txtIp.TabIndex = 1
        Me.txtIp.Text = "127.0.0.1"
        '
        'lblHttpPort
        '
        Me.lblHttpPort.Location = New System.Drawing.Point(170, 28)
        Me.lblHttpPort.Name = "lblHttpPort"
        Me.lblHttpPort.Size = New System.Drawing.Size(65, 20)
        Me.lblHttpPort.TabIndex = 2
        Me.lblHttpPort.Text = "HTTP port:"
        '
        'txtHttpPort
        '
        Me.txtHttpPort.Location = New System.Drawing.Point(240, 25)
        Me.txtHttpPort.Name = "txtHttpPort"
        Me.txtHttpPort.Size = New System.Drawing.Size(45, 20)
        Me.txtHttpPort.TabIndex = 3
        Me.txtHttpPort.Text = "8088"
        '
        'lblTcpPort
        '
        Me.lblTcpPort.Location = New System.Drawing.Point(300, 28)
        Me.lblTcpPort.Name = "lblTcpPort"
        Me.lblTcpPort.Size = New System.Drawing.Size(60, 20)
        Me.lblTcpPort.TabIndex = 4
        Me.lblTcpPort.Text = "TCP port:"
        '
        'txtTcpPort
        '
        Me.txtTcpPort.Location = New System.Drawing.Point(365, 25)
        Me.txtTcpPort.Name = "txtTcpPort"
        Me.txtTcpPort.Size = New System.Drawing.Size(45, 20)
        Me.txtTcpPort.TabIndex = 5
        Me.txtTcpPort.Text = "8099"
        '
        'rbHttp
        '
        Me.rbHttp.Checked = True
        Me.rbHttp.Location = New System.Drawing.Point(425, 26)
        Me.rbHttp.Name = "rbHttp"
        Me.rbHttp.Size = New System.Drawing.Size(60, 20)
        Me.rbHttp.TabIndex = 6
        Me.rbHttp.TabStop = True
        Me.rbHttp.Text = "HTTP"
        '
        'rbTcp
        '
        Me.rbTcp.Location = New System.Drawing.Point(495, 26)
        Me.rbTcp.Name = "rbTcp"
        Me.rbTcp.Size = New System.Drawing.Size(55, 20)
        Me.rbTcp.TabIndex = 7
        Me.rbTcp.Text = "TCP"
        '
        'btnTestConnection
        '
        Me.btnTestConnection.Location = New System.Drawing.Point(565, 23)
        Me.btnTestConnection.Name = "btnTestConnection"
        Me.btnTestConnection.Size = New System.Drawing.Size(150, 25)
        Me.btnTestConnection.TabIndex = 8
        Me.btnTestConnection.Text = "Test connection"
        Me.btnTestConnection.UseVisualStyleBackColor = True
        '
        'btnFetchState
        '
        Me.btnFetchState.Location = New System.Drawing.Point(725, 23)
        Me.btnFetchState.Name = "btnFetchState"
        Me.btnFetchState.Size = New System.Drawing.Size(230, 25)
        Me.btnFetchState.TabIndex = 11
        Me.btnFetchState.Text = "Fetch vMix status (field names)"
        Me.btnFetchState.UseVisualStyleBackColor = True
        '
        'lblConnectionStatus
        '
        Me.lblConnectionStatus.Location = New System.Drawing.Point(15, 58)
        Me.lblConnectionStatus.Name = "lblConnectionStatus"
        Me.lblConnectionStatus.Size = New System.Drawing.Size(940, 20)
        Me.lblConnectionStatus.TabIndex = 9
        '
        'lblConnHint
        '
        Me.lblConnHint.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblConnHint.Location = New System.Drawing.Point(15, 80)
        Me.lblConnHint.Name = "lblConnHint"
        Me.lblConnHint.Size = New System.Drawing.Size(940, 20)
        Me.lblConnHint.TabIndex = 10
        '
        'grpBuilder
        '
        Me.grpBuilder.Controls.Add(Me.lblFunction)
        Me.grpBuilder.Controls.Add(Me.cmbFunction)
        Me.grpBuilder.Controls.Add(Me.lblInput)
        Me.grpBuilder.Controls.Add(Me.txtInput)
        Me.grpBuilder.Controls.Add(Me.lblSelectedName)
        Me.grpBuilder.Controls.Add(Me.txtSelectedName)
        Me.grpBuilder.Controls.Add(Me.lblValue)
        Me.grpBuilder.Controls.Add(Me.txtValue)
        Me.grpBuilder.Controls.Add(Me.chkRawValue)
        Me.grpBuilder.Controls.Add(Me.btnBuildAndSend)
        Me.grpBuilder.Controls.Add(Me.btnDemoSpecialChars)
        Me.grpBuilder.Controls.Add(Me.btnDemoSetImage)
        Me.grpBuilder.Controls.Add(Me.btnDemoOverlay)
        Me.grpBuilder.Controls.Add(Me.btnDemoTransparent)
        Me.grpBuilder.Controls.Add(Me.lblBuilderHint)
        Me.grpBuilder.Location = New System.Drawing.Point(12, 240)
        Me.grpBuilder.Name = "grpBuilder"
        Me.grpBuilder.Size = New System.Drawing.Size(970, 230)
        Me.grpBuilder.TabIndex = 2
        Me.grpBuilder.TabStop = False
        Me.grpBuilder.Text = "2) Build a command by hand (Command Builder)"
        '
        'lblFunction
        '
        Me.lblFunction.Location = New System.Drawing.Point(15, 30)
        Me.lblFunction.Name = "lblFunction"
        Me.lblFunction.Size = New System.Drawing.Size(65, 20)
        Me.lblFunction.TabIndex = 0
        Me.lblFunction.Text = "Function:"
        '
        'cmbFunction
        '
        Me.cmbFunction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFunction.Location = New System.Drawing.Point(85, 27)
        Me.cmbFunction.Name = "cmbFunction"
        Me.cmbFunction.Size = New System.Drawing.Size(190, 21)
        Me.cmbFunction.TabIndex = 1
        '
        'lblInput
        '
        Me.lblInput.Location = New System.Drawing.Point(290, 30)
        Me.lblInput.Name = "lblInput"
        Me.lblInput.Size = New System.Drawing.Size(45, 20)
        Me.lblInput.TabIndex = 2
        Me.lblInput.Text = "Input:"
        '
        'txtInput
        '
        Me.txtInput.Location = New System.Drawing.Point(340, 27)
        Me.txtInput.Name = "txtInput"
        Me.txtInput.Size = New System.Drawing.Size(190, 20)
        Me.txtInput.TabIndex = 3
        '
        'lblSelectedName
        '
        Me.lblSelectedName.Location = New System.Drawing.Point(15, 60)
        Me.lblSelectedName.Name = "lblSelectedName"
        Me.lblSelectedName.Size = New System.Drawing.Size(95, 20)
        Me.lblSelectedName.TabIndex = 4
        Me.lblSelectedName.Text = "SelectedName:"
        '
        'txtSelectedName
        '
        Me.txtSelectedName.Location = New System.Drawing.Point(115, 57)
        Me.txtSelectedName.Name = "txtSelectedName"
        Me.txtSelectedName.Size = New System.Drawing.Size(190, 20)
        Me.txtSelectedName.TabIndex = 5
        '
        'lblValue
        '
        Me.lblValue.Location = New System.Drawing.Point(320, 60)
        Me.lblValue.Name = "lblValue"
        Me.lblValue.Size = New System.Drawing.Size(45, 20)
        Me.lblValue.TabIndex = 6
        Me.lblValue.Text = "Value:"
        '
        'txtValue
        '
        Me.txtValue.Location = New System.Drawing.Point(370, 57)
        Me.txtValue.Name = "txtValue"
        Me.txtValue.Size = New System.Drawing.Size(400, 20)
        Me.txtValue.TabIndex = 7
        '
        'chkRawValue
        '
        Me.chkRawValue.Location = New System.Drawing.Point(780, 58)
        Me.chkRawValue.Name = "chkRawValue"
        Me.chkRawValue.Size = New System.Drawing.Size(175, 35)
        Me.chkRawValue.TabIndex = 13
        Me.chkRawValue.Text = "Don't encode Value (test, e.g. for image paths)"
        Me.chkRawValue.UseVisualStyleBackColor = True
        '
        'btnBuildAndSend
        '
        Me.btnBuildAndSend.Location = New System.Drawing.Point(15, 95)
        Me.btnBuildAndSend.Name = "btnBuildAndSend"
        Me.btnBuildAndSend.Size = New System.Drawing.Size(190, 30)
        Me.btnBuildAndSend.TabIndex = 8
        Me.btnBuildAndSend.Text = "Build and send command"
        Me.btnBuildAndSend.UseVisualStyleBackColor = True
        '
        'btnDemoSpecialChars
        '
        Me.btnDemoSpecialChars.Location = New System.Drawing.Point(215, 95)
        Me.btnDemoSpecialChars.Name = "btnDemoSpecialChars"
        Me.btnDemoSpecialChars.Size = New System.Drawing.Size(190, 30)
        Me.btnDemoSpecialChars.TabIndex = 9
        Me.btnDemoSpecialChars.Text = "Example: Special characters"
        Me.btnDemoSpecialChars.UseVisualStyleBackColor = True
        '
        'btnDemoSetImage
        '
        Me.btnDemoSetImage.Location = New System.Drawing.Point(415, 95)
        Me.btnDemoSetImage.Name = "btnDemoSetImage"
        Me.btnDemoSetImage.Size = New System.Drawing.Size(190, 30)
        Me.btnDemoSetImage.TabIndex = 10
        Me.btnDemoSetImage.Text = "Example: Set image"
        Me.btnDemoSetImage.UseVisualStyleBackColor = True
        '
        'btnDemoOverlay
        '
        Me.btnDemoOverlay.Location = New System.Drawing.Point(615, 95)
        Me.btnDemoOverlay.Name = "btnDemoOverlay"
        Me.btnDemoOverlay.Size = New System.Drawing.Size(190, 30)
        Me.btnDemoOverlay.TabIndex = 11
        Me.btnDemoOverlay.Text = "Example: Overlay show/hide"
        Me.btnDemoOverlay.UseVisualStyleBackColor = True
        '
        'btnDemoTransparent
        '
        Me.btnDemoTransparent.Location = New System.Drawing.Point(15, 135)
        Me.btnDemoTransparent.Name = "btnDemoTransparent"
        Me.btnDemoTransparent.Size = New System.Drawing.Size(190, 30)
        Me.btnDemoTransparent.TabIndex = 12
        Me.btnDemoTransparent.Text = "Example: Transparent (Alpha 00)"
        Me.btnDemoTransparent.UseVisualStyleBackColor = True
        '
        'lblBuilderHint
        '
        Me.lblBuilderHint.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblBuilderHint.Location = New System.Drawing.Point(15, 175)
        Me.lblBuilderHint.Name = "lblBuilderHint"
        Me.lblBuilderHint.Size = New System.Drawing.Size(940, 50)
        Me.lblBuilderHint.TabIndex = 13
        '
        'grpVisualize
        '
        Me.grpVisualize.Controls.Add(Me.lblRawTitle)
        Me.grpVisualize.Controls.Add(Me.txtRawCommand)
        Me.grpVisualize.Controls.Add(Me.lblEncodingTitle)
        Me.grpVisualize.Controls.Add(Me.txtEncodingDemo)
        Me.grpVisualize.Controls.Add(Me.lblProtocolTitle)
        Me.grpVisualize.Controls.Add(Me.txtProtocolTranslation)
        Me.grpVisualize.Controls.Add(Me.lblResponseTitle)
        Me.grpVisualize.Controls.Add(Me.txtResponse)
        Me.grpVisualize.Controls.Add(Me.lblCodeTitle)
        Me.grpVisualize.Controls.Add(Me.txtCodeSnippet)
        Me.grpVisualize.Location = New System.Drawing.Point(12, 480)
        Me.grpVisualize.Name = "grpVisualize"
        Me.grpVisualize.Size = New System.Drawing.Size(970, 500)
        Me.grpVisualize.TabIndex = 3
        Me.grpVisualize.TabStop = False
        Me.grpVisualize.Text = "3) What happens under the hood? (Visualization)"
        '
        'lblRawTitle
        '
        Me.lblRawTitle.Location = New System.Drawing.Point(15, 25)
        Me.lblRawTitle.Name = "lblRawTitle"
        Me.lblRawTitle.Size = New System.Drawing.Size(500, 18)
        Me.lblRawTitle.TabIndex = 0
        Me.lblRawTitle.Text = "vMix command (Function=...), still protocol-neutral:"
        '
        'txtRawCommand
        '
        Me.txtRawCommand.Location = New System.Drawing.Point(15, 45)
        Me.txtRawCommand.Name = "txtRawCommand"
        Me.txtRawCommand.ReadOnly = True
        Me.txtRawCommand.Size = New System.Drawing.Size(940, 20)
        Me.txtRawCommand.TabIndex = 1
        '
        'lblEncodingTitle
        '
        Me.lblEncodingTitle.Location = New System.Drawing.Point(15, 72)
        Me.lblEncodingTitle.Name = "lblEncodingTitle"
        Me.lblEncodingTitle.Size = New System.Drawing.Size(600, 18)
        Me.lblEncodingTitle.TabIndex = 2
        Me.lblEncodingTitle.Text = "Value: raw input  vs.  after EncodeVmixValue (Uri.EscapeDataString):"
        '
        'txtEncodingDemo
        '
        Me.txtEncodingDemo.Location = New System.Drawing.Point(15, 92)
        Me.txtEncodingDemo.Name = "txtEncodingDemo"
        Me.txtEncodingDemo.ReadOnly = True
        Me.txtEncodingDemo.Size = New System.Drawing.Size(940, 20)
        Me.txtEncodingDemo.TabIndex = 3
        '
        'lblProtocolTitle
        '
        Me.lblProtocolTitle.Location = New System.Drawing.Point(15, 119)
        Me.lblProtocolTitle.Name = "lblProtocolTitle"
        Me.lblProtocolTitle.Size = New System.Drawing.Size(600, 18)
        Me.lblProtocolTitle.TabIndex = 4
        Me.lblProtocolTitle.Text = "Actually sent (HTTP URL or TCP protocol line):"
        '
        'txtProtocolTranslation
        '
        Me.txtProtocolTranslation.Location = New System.Drawing.Point(15, 139)
        Me.txtProtocolTranslation.Name = "txtProtocolTranslation"
        Me.txtProtocolTranslation.ReadOnly = True
        Me.txtProtocolTranslation.Size = New System.Drawing.Size(940, 20)
        Me.txtProtocolTranslation.TabIndex = 5
        '
        'lblResponseTitle
        '
        Me.lblResponseTitle.Location = New System.Drawing.Point(15, 166)
        Me.lblResponseTitle.Name = "lblResponseTitle"
        Me.lblResponseTitle.Size = New System.Drawing.Size(200, 18)
        Me.lblResponseTitle.TabIndex = 6
        Me.lblResponseTitle.Text = "Response from vMix:"
        '
        'txtResponse
        '
        Me.txtResponse.Location = New System.Drawing.Point(15, 186)
        Me.txtResponse.Name = "txtResponse"
        Me.txtResponse.ReadOnly = True
        Me.txtResponse.Size = New System.Drawing.Size(940, 20)
        Me.txtResponse.TabIndex = 7
        '
        'lblCodeTitle
        '
        Me.lblCodeTitle.Location = New System.Drawing.Point(15, 213)
        Me.lblCodeTitle.Name = "lblCodeTitle"
        Me.lblCodeTitle.Size = New System.Drawing.Size(700, 18)
        Me.lblCodeTitle.TabIndex = 8
        Me.lblCodeTitle.Text = "Sample code for your own project (button click that does exactly what's built abo" &
    "ve):"
        '
        'txtCodeSnippet
        '
        Me.txtCodeSnippet.Font = New System.Drawing.Font("Consolas", 10.0!)
        Me.txtCodeSnippet.Location = New System.Drawing.Point(15, 233)
        Me.txtCodeSnippet.Multiline = True
        Me.txtCodeSnippet.Name = "txtCodeSnippet"
        Me.txtCodeSnippet.ReadOnly = True
        Me.txtCodeSnippet.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtCodeSnippet.Size = New System.Drawing.Size(940, 250)
        Me.txtCodeSnippet.TabIndex = 9
        Me.txtCodeSnippet.WordWrap = False
        '
        'MainForm
        '
        Me.ClientSize = New System.Drawing.Size(1000, 995)
        Me.Controls.Add(Me.lblIntro)
        Me.Controls.Add(Me.grpConnection)
        Me.Controls.Add(Me.grpBuilder)
        Me.Controls.Add(Me.grpVisualize)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "vMixControl - Sample: HTTP/TCP control, command builder, encoding"
        Me.grpConnection.ResumeLayout(False)
        Me.grpConnection.PerformLayout()
        Me.grpBuilder.ResumeLayout(False)
        Me.grpBuilder.PerformLayout()
        Me.grpVisualize.ResumeLayout(False)
        Me.grpVisualize.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

End Class
