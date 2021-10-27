Option Strict Off
Imports JHSoftware.SimpleDNS
Imports JHSoftware.SimpleDNS.Plugin

Public Class LookupHost
  Inherits LookupBase
  Implements ILookupHost
  Implements IOptionsUI

  Public Overrides Function GetTypeInfo() As TypeInfo Implements IPlugInBase.GetTypeInfo
    Return New TypeInfo With {
      .Name = "JavaScript / Host lookup",
      .Description = "Executes a custom JavaScript function to look up an IP address for a host name",
      .InfoURL = "https://simpledns.plus/plugin-javascript"
    }
  End Function

  Private Function ILookupHost_LookupHost(name As DomName, ipv6 As Boolean, ctx As IRequestContext) As Task(Of LookupResult(Of SdnsIP)) Implements ILookupHost.LookupHost
    LoadScript()
    Dim res = Engine.Evaluate("Lookup('" & name.ToString().Replace("'", "\'") & "'," & If(ipv6, "true", "false") & "," & CtxToJS(ctx) & ")")
    If res Is Nothing Then Return Task.FromResult(Of LookupResult(Of SdnsIP))(Nothing)
    Dim rv = New LookupResult(Of SdnsIP)
    If TypeOf res Is String Then
      rv.Value = SdnsIP.Parse(res)
    Else
      Dim RValue = res.value
      Dim RTTL = res.ttl
      If TypeOf RValue Is Microsoft.ClearScript.Undefined Then Throw New Exception("Returned object does not have a 'value' property")
      rv.Value = SdnsIP.Parse(res.value)
      If Not TypeOf RTTL Is Microsoft.ClearScript.Undefined Then
        rv.TTL = RTTL
      End If
    End If
    Return Task.FromResult(rv)
  End Function

  Public Function GetOptionsUI(instanceID As Guid, dataPath As String) As OptionsUI Implements IOptionsUI.GetOptionsUI
    Dim ctrl = New CtrlOptions
    ctrl.TheScript = "// Sample script for JavaScript / Host lookup plug-in" & vbCrLf &
                      "function Lookup(name, ipv6, context) {" & vbCrLf &
                      "    if(name !== 'example.com') return null;" & vbCrLf &
                      "    return { value: ipv6 ? '1234::1234' : '1.2.3.4', ttl: 300 };" & vbCrLf &
                      "}" & vbCrLf
    Return ctrl
  End Function

End Class
