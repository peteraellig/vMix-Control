' ============================================================================
' vMixControl - Sample project for controlling vMix via HTTP and TCP
' ============================================================================
' This file shows the core idea: no matter whether a command is sent to vMix
' via HTTP or TCP, the CALLER always builds the same simple string
' "Function=X&Input=Y&SelectedName=Z&Value=W" (see VmixCommandBuilder.vb).
' Only the concrete IVmixSender implementation (VmixHttpSender or
' VmixTcpSender) translates that string into the actual protocol.
'
' Benefit of this separation: the rest of the code (buttons, business logic)
' never needs to know whether HTTP or TCP is currently in use - it always
' just calls sender.Send(command). This exact abstraction is used in the
' real broadcast tools (Tennis26, SoccerClock, Volleyball24).
Public Interface IVmixSender

    ' What actually went over the wire last - the full URL for HTTP, the sent
    ' protocol line for TCP. Purely informational, to make what happens
    ' "under the hood" visible in the demo form.
    ReadOnly Property LastCommand As String

    ' command: "Function=X&Input=Y&SelectedName=Z&Value=W" as built by
    ' VmixCommandBuilder. Return value: vMix's response (or an error message
    ' if vMix isn't reachable) - deliberately never throws an exception, so a
    ' single failed command doesn't crash the caller.
    Function Send(command As String) As String

End Interface
