Partial Class StatusViewerForm
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

    Friend WithEvents txtXml As TextBox
    Friend WithEvents lblHint As Label

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.txtXml = New System.Windows.Forms.TextBox()
        Me.lblHint = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblHint
        '
        Me.lblHint.Location = New System.Drawing.Point(10, 8)
        Me.lblHint.Name = "lblHint"
        Me.lblHint.Size = New System.Drawing.Size(770, 20)
        Me.lblHint.TabIndex = 0
        Me.lblHint.Text = "name=""..."" is the exact SelectedName - spaces in it are visible here, often not in the Title Editor."
        '
        'txtXml
        '
        Me.txtXml.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtXml.Font = New System.Drawing.Font("Consolas", 9.0!)
        Me.txtXml.Location = New System.Drawing.Point(10, 32)
        Me.txtXml.Multiline = True
        Me.txtXml.Name = "txtXml"
        Me.txtXml.ReadOnly = True
        Me.txtXml.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtXml.Size = New System.Drawing.Size(770, 555)
        Me.txtXml.TabIndex = 1
        Me.txtXml.WordWrap = False
        '
        'StatusViewerForm
        '
        Me.ClientSize = New System.Drawing.Size(792, 600)
        Me.Controls.Add(Me.txtXml)
        Me.Controls.Add(Me.lblHint)
        Me.MinimumSize = New System.Drawing.Size(500, 300)
        Me.Name = "StatusViewerForm"
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "vMix Status (current inputs & field names)"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

End Class
