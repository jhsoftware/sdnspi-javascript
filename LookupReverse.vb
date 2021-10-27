Option Strict Off
Imports JHSoftware.SimpleDNS
Imports JHSoftware.SimpleDNS.Plugin

Public Class LookupReverse
  Inherits LookupBase
  Implements ILookupReverse
  Implements IOptionsUI

  Public Overrides Function GetTypeInfo() As TypeInfo Implements IPlugInBase.GetTypeInfo
    Return New TypeInfo With {
      .Name = "JavaScript / Reverse lookup",
      .Description = "Executes a custom JavaScript function to look up the host name for an IP address",
      .InfoURL = "https://simpledns.plus/plugin-javascript"
    }
  End Function

  Private Function Lookup(ip As SdnsIP, ctx As IRequestContext) As Task(Of LookupResult(Of DomName)) Implements ILookupReverse.LookupReverse
    LoadScript()
    Dim res = Engine.Evaluate("Lookup('" & ip.ToString() & "'," & CtxToJS(ctx) & ")")
    If res Is Nothing Then Return Task.FromResult(Of LookupResult(Of DomName))(Nothing)
    Dim rv = New LookupResult(Of DomName)
    If TypeOf res Is String Then
      rv.Value = DomName.Parse(res)
    Else
      Dim RValue = res.value
      Dim RTTL = res.ttl
      If TypeOf RValue Is Microsoft.ClearScript.Undefined Then Throw New Exception("Returned object does not have a 'value' property")
      rv.Value = DomName.Parse(res.value)
      If Not TypeOf RTTL Is Microsoft.ClearScript.Undefined Then
        rv.TTL = RTTL
      End If
    End If
    Return Task.FromResult(rv)
  End Function

  Public Function GetOptionsUI(instanceID As Guid, dataPath As String) As OptionsUI Implements IOptionsUI.GetOptionsUI
    Dim ctrl = New CtrlOptions
    ctrl.TheScript = "// Sample script for JavaScript / Reverse lookup plug-in" & vbCrLf &
                      "function Lookup(ip, context) {" & vbCrLf &
                      "    if(ip !== '1.2.3.4') return null;" & vbCrLf &
                      "    return { value: 'example.com', ttl: 300 };" & vbCrLf &
                      "}" & vbCrLf
    Return ctrl
  End Function

End Class
