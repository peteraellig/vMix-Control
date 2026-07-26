' ============================================================================
' Shows the raw XML response from "http://IP:Port/api/?" (with no Function
' parameter) - vMix lists every currently loaded input there with its exact
' field names (<text name="...">, <image name="...">, ...).
' ============================================================================
' Reason for this dedicated window: this exact field-name comparison solved
' Peter's SetImage problem - "Logo Image.Source" (with a space) looked like
' "LogoImage.Source" (without) in the vMix Title Editor. The Title Editor
' barely shows spaces in field names, whereas the XML response shows the
' name exactly as SelectedName needs to match it.
Public Class StatusViewerForm

    Public Sub New(xml As String)
        InitializeComponent()
        txtXml.Text = xml
    End Sub

End Class
