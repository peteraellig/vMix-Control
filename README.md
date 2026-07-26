# vMix Control

**A practical VB.NET reference for controlling vMix safely over HTTP or TCP**

Build commands once. Encode dynamic values correctly. Choose the transport
without changing application logic.

`Windows Forms` · `.NET Framework 4.8` · `vMix API` · `HTTP` · `TCP`

---

## Overview

vMix Control is a small demonstration and reference application that explains
the communication layer used by several broadcast tools in this GitHub
account, including SoccerClock, Tennis26, and Beach Volleyball Scorer.

The project addresses a deceptively common source of vMix integration errors:
manually concatenating long API command strings containing `&`, spaces, `%`,
`#`, accented characters, file paths, and field names. A single unescaped
character can split the command at the wrong position, select the wrong input,
truncate a value, display the wrong text, or make a command appear to do
nothing.

Instead, application code calls a small set of command-building functions.
Every dynamic component is encoded in one place, and the finished
protocol-neutral command can be sent through either HTTP or TCP.

<img src="example.png" alt="Example vMix title" width="200">

## The core design

```text
Application code
      |
      v
VmixCommandBuilder
      |
      |  Function=X&Input=Y&SelectedName=Z&Value=W
      |
      v
IVmixSender
   /       \
HTTP       TCP
sender     sender
   \       /
      v
     vMix
```

The application is split into four reusable files:

| File | Responsibility |
|---|---|
| `VmixCommandBuilder.vb` | Builds commands and encodes all dynamic parameters |
| `IVmixSender.vb` | Defines the common sender interface |
| `VmixHttpSender.vb` | Translates a command into an HTTP GET request |
| `VmixTcpSender.vb` | Translates a command into a vMix TCP protocol line and maintains the connection |

The form and business logic only call:

```vb
Dim result As String = sender.Send(command)
```

They do not need separate command-building code for HTTP and TCP.

## Why use a command builder?

A hand-built command often looks like this:

```vb
Dim command = "Function=SetText&Input=" & titleName &
              "&SelectedName=" & fieldName &
              "&Value=" & newText
```

This is fragile. If `newText` is:

```text
Team A & Co/100% Café #1
```

the ampersand is interpreted as the beginning of another API parameter.
Spaces, percent signs, hash signs, non-ASCII characters, and reserved URL
characters can cause similar problems.

The safe version is:

```vb
Dim command As String =
    BuildVmixSetCommand(
        "SetText",
        "My Title.gtzip",
        "HEADLINE.Text",
        "Team A & Co/100% Café #1")

sender.Send(command)
```

The builder encodes `Input`, `SelectedName`, and `Value`, not only the value.
This matters because vMix input names and GT title field names can also contain
spaces or special characters.

## Correct encoding

The project deliberately uses:

```vb
Uri.EscapeDataString(value)
```

It does **not** use `WebUtility.UrlEncode`. The latter follows HTML form
conventions and encodes a space as `+`. In this workflow, vMix can interpret
that plus sign literally. `Uri.EscapeDataString` produces `%20`, preserving the
intended space reliably.

Example:

```text
Raw:
Team A & Co

Encoded:
Team%20A%20%26%20Co
```

## Available command shapes

Not every vMix function needs all parameters. The builder provides focused
functions for the common combinations:

```vb
' Function + Input + SelectedName + Value
BuildVmixSetCommand("SetText", input, field, value)

' Function + Input + Value
BuildVmixCommand("TitleBeginAnimation", input, "Page1")

' Function + Input
BuildVmixInputCommand("OverlayInput1IN", input)

' Function + Input + SelectedName
BuildVmixSelectCommand("SetTextVisibleOff", input, field)

' Function + Value
BuildVmixValueOnlyCommand("Addinput", "Title|C:\vMix\example\example_title.gtzip")
```

The demonstration UI includes presets for:

- `SetText`
- `SetImage`
- `SetColor`
- `SetTextColour`
- Text and image visibility
- Title animations
- Overlay input and output
- Adding a new vMix input

Be careful with `Addinput`: each call adds another input to vMix rather than
replacing an existing one.

## HTTP and TCP

Both implementations accept the same command from the caller, but they have
different operational characteristics.

| | HTTP | TCP |
|---|---|---|
| Default port | `8088` | `8099` |
| vMix setting | Web Controller | TCP Controllers |
| Connection | New request per command | Persistent connection |
| Overhead | HTTP request/response overhead for every command | Lower overhead after connection |
| Best suited for | Simple integrations, diagnostics, occasional commands | Frequent commands and tightly sequenced live graphics |
| State query | Convenient XML status response through `/api/` | Command-oriented protocol |

### Why keep both?

- HTTP is easy to inspect, test in a browser, and troubleshoot.
- HTTP provides the complete vMix XML status document, including exact input
  and title-field names.
- TCP avoids opening a new HTTP connection for every command.
- A persistent TCP connection is efficient when many title fields are updated
  during live scoring.
- A shared interface lets each application choose the better transport without
  duplicating its graphics logic.
- Keeping HTTP available provides a useful fallback and diagnostic path even
  when TCP is the preferred live transport.

## Command order matters

vMix must receive related commands in the intended order. For example:

1. Update all text, image, and color fields.
2. Show the overlay.

TCP naturally preserves byte order on a single connection. Independent HTTP
requests can arrive or be processed out of order if they are launched in
parallel. For this reason, the applications using this system send related
commands synchronously and serially rather than firing them concurrently.

This rule is about ordering, not only performance.

## TCP connection lifetime and `Dispose`

`VmixTcpSender` keeps a `TcpClient` and `NetworkStream` open so they can be
reused. The sender implements `IDisposable`, and the application must dispose
it when the owning form or application closes:

```vb
Private ReadOnly tcpSender As New VmixTcpSender()

Private Sub MainForm_FormClosing(
    sender As Object,
    e As FormClosingEventArgs
) Handles MyBase.FormClosing
    tcpSender.Dispose()
End Sub
```

Disposal closes the network stream and TCP client and releases the socket
cleanly. Omitting it can leave connections and operating-system resources open
until process termination or garbage collection.

The sender also disconnects automatically after a communication error and
reconnects when the next command is sent.

## Thread safety

TCP sending is protected by `SyncLock`. This prevents two callers from writing
interleaved command bytes or reading each other's response. It does not mean
that a graphics workflow should send logically dependent operations in
parallel: related commands should still be issued serially in their intended
order.

## Finding exact vMix field names

The vMix Title Editor can make spaces in field names difficult to notice. For
example, `Logo Image.Source` and `LogoImage.Source` are different names.

The demo's **Fetch vMix state** function requests:

```text
http://<host>:<HTTP port>/api/?
```

and displays the returned XML in a separate viewer. The XML shows the exact
input and field names vMix expects for `Input` and `SelectedName`.

## Running the example

### Requirements

- Windows
- .NET Framework 4.8
- vMix
- Visual Studio with the Visual Basic/.NET desktop workload when building from
  source

### Install the example assets

The demonstration uses a fixed example path:

```text
C:\vMix\example\
```

Copy all files from [`vMixAssets/`](vMixAssets) into that directory:

```text
C:\vMix\example\example.vmix
C:\vMix\example\example_title.gtzip
C:\vMix\example\logo.png
```

Open `example.vmix` in vMix, or add `example_title.gtzip` manually.

### Enable a controller

For HTTP, enable the vMix Web Controller and normally use port `8088`.

For TCP, enable a controller under vMix **Settings > TCP Controllers** and
normally use port `8099`.

### Start the demo

1. Open `vMixControl.sln` in Visual Studio.
2. Build and run the application.
3. Enter the vMix host and ports.
4. Select HTTP or TCP.
5. Test the connection.
6. Choose a function and inspect the generated raw command, encoded value,
   VB.NET code sample, wire representation, and vMix response.
7. Try the special-character demonstration with encoding enabled and disabled.

## Reusing the communication layer

Copy these files into another VB.NET project:

```text
IVmixSender.vb
VmixCommandBuilder.vb
VmixHttpSender.vb
VmixTcpSender.vb
```

Create one or both sender instances as long-lived fields:

```vb
Private ReadOnly httpSender As New VmixHttpSender() With {
    .Ip = "127.0.0.1",
    .Port = 8088
}

Private ReadOnly tcpSender As New VmixTcpSender() With {
    .Ip = "127.0.0.1",
    .Port = 8099
}
```

Select the active implementation through `IVmixSender`:

```vb
Dim sender As IVmixSender =
    If(useTcp,
       DirectCast(tcpSender, IVmixSender),
       DirectCast(httpSender, IVmixSender))

Dim command =
    BuildVmixSetCommand(
        "SetText",
        "Scorebug.gtzip",
        "HOME.Text",
        homeTeamName)

Dim response = sender.Send(command)
```

If a TCP sender was created, dispose it during application shutdown.

## Repository contents

| Path | Contents |
|---|---|
| `VmixCommandBuilder.vb` | Safe vMix command construction and encoding |
| `IVmixSender.vb` | Protocol-independent sender contract |
| `VmixHttpSender.vb` | HTTP transport |
| `VmixTcpSender.vb` | Persistent TCP transport |
| `MainForm.vb` | Interactive examples and generated integration code |
| `StatusViewerForm.vb` | Raw vMix XML status viewer |
| [`vMixAssets/`](vMixAssets) | Ready-to-run vMix example project, GT title, and logo |
| `example.png` | Example title output shown above |

## License

This project is licensed under the GNU General Public License v3.0. See
[`LICENSE`](LICENSE) for the full license text.
