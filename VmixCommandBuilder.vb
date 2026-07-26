' ============================================================================
' Builds vMix API commands and URL-encodes ALL dynamic parts.
' ============================================================================
' The vMix API (whether via HTTP or TCP) understands commands in the form
'   Function=<function name>&Input=<title/input name>&SelectedName=<field name>&Value=<value>
' It's not only the Value part that can contain problematic characters -
' Input (the title's file name) often has spaces too, e.g. when a title was
' inserted from the vMix library and never renamed ("Title 1- Element
' Red.gtzip" is a vMix default name). ALL three parts (Input, SelectedName,
' Value) therefore need to be encoded, not just Value - otherwise vMix splits
' the query string at the wrong point and doesn't even find the Input
' (SetImage/SetText then appear to do nothing, with no error message).
'
' IMPORTANT: use Uri.EscapeDataString (not WebUtility.UrlEncode!).
' WebUtility.UrlEncode encodes a space as "+" (old HTML form convention) -
' that arrives at vMix as a LITERAL plus sign, not a space ("Team A+B"
' instead of "Team A B"). Uri.EscapeDataString correctly produces "%20"
' instead.
Public Module VmixCommandBuilder

    Public Function EncodeVmixValue(value As String) As String
        Return Uri.EscapeDataString(If(value, ""))
    End Function

    ' The "normal" case: Function + Input + SelectedName + Value, e.g.
    '   BuildVmixSetCommand("SetText", "My Title.gtzip", "HEADLINE.Text", "Team A & Co")
    ' produces "Function=SetText&Input=My%20Title.gtzip&SelectedName=HEADLINE.Text&Value=Team%20A%20%26%20Co"
    Public Function BuildVmixSetCommand(func As String, input As String, selectedName As String, value As String) As String
        Return "Function=" & func & "&Input=" & EncodeVmixValue(input) & "&SelectedName=" & EncodeVmixValue(selectedName) & "&Value=" & EncodeVmixValue(value)
    End Function

    ' Commands without SelectedName, e.g. switching a title animation:
    '   BuildVmixCommand("TitleBeginAnimation", "My Title.gtzip", "Page1")
    Public Function BuildVmixCommand(func As String, input As String, value As String) As String
        Return "Function=" & func & "&Input=" & EncodeVmixValue(input) & "&Value=" & EncodeVmixValue(value)
    End Function

    ' Plain overlay-toggle commands without Value/SelectedName, e.g. showing
    ' or hiding a title:
    '   BuildVmixInputCommand("OverlayInput1IN", "My Title.gtzip")
    '   BuildVmixInputCommand("OverlayInput1Out", "")   ' some functions need no Input at all
    Public Function BuildVmixInputCommand(func As String, input As String) As String
        If String.IsNullOrEmpty(input) Then
            Return "Function=" & func
        End If
        Return "Function=" & func & "&Input=" & EncodeVmixValue(input)
    End Function

    ' Toggle a field's visibility without changing its value:
    '   BuildVmixSelectCommand("SetTextVisibleOff", "My Title.gtzip", "HEADLINE.Text")
    Public Function BuildVmixSelectCommand(func As String, input As String, selectedName As String) As String
        Return "Function=" & func & "&Input=" & EncodeVmixValue(input) & "&SelectedName=" & EncodeVmixValue(selectedName)
    End Function

    ' Commands with a Value but no Input/SelectedName - e.g. adding a new
    ' input to the running vMix instance:
    '   BuildVmixValueOnlyCommand("Addinput", "Title|C:\vmix\example\example_title.gtzip")
    ' Careful with Addinput: every call adds a NEW input (it doesn't replace
    ' anything) - repeated testing quickly creates several copies in vMix.
    Public Function BuildVmixValueOnlyCommand(func As String, value As String) As String
        Return "Function=" & func & "&Value=" & EncodeVmixValue(value)
    End Function

End Module
