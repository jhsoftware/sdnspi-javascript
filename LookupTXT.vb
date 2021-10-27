Option Strict Off
Imports JHSoftware.SimpleDNS
Imports JHSoftware.SimpleDNS.Plugin

Public Class LookupTXT
  Inherits LookupBase
  Implements ILookupTXT
  Implements IOptionsUI

  Public Overrides Function GetTypeInfo() As TypeInfo Implements IPlugInBase.GetTypeInfo
    Return New TypeInfo With {
      .Name = "JavaScript / TXT lookup",
      .Description = "Executes a custom JavaScript function look up a TXT-record value for a host name",
      .InfoURL = "https://simpledns.plus/plugin-javascript"
    }
  End Function

  Private Function LookupTXT(name As DomName, ctx As IRequestContext) As Task(Of LookupResult(Of String)) Implements ILookupTXT.LookupTXT
    LoadScript()
    Dim res = Engine.Evaluate("Lookup('" & name.ToString().Replace("'", "\'") & "'," & CtxToJS(ctx) & ")")
    If res Is Nothing Then Return Task.FromResult(Of LookupResult(Of String))(Nothing)
    Dim rv = New LookupResult(Of String)
    If TypeOf res Is String Then
      rv.Value = res
    Else
      Dim RValue = res.value
      Dim RTTL = res.ttl
      If TypeOf RValue Is Microsoft.ClearScript.Undefined Then Throw New Exception("Returned object does not have a 'value' property")
      rv.Value = res.value
      If Not TypeOf RTTL Is Microsoft.ClearScript.Undefined Then
        rv.TTL = RTTL
      End If
    End If
    Return Task.FromResult(rv)
  End Function

  Public Function GetOptionsUI(instanceID As Guid, dataPath As String) As OptionsUI Implements IOptionsUI.GetOptionsUI
    Dim ctrl = New CtrlOptions
    ctrl.TheScript = "// Sample script for JavaScript / TXT lookup plug-in" & vbCrLf &
                      "function Lookup(name, context) {" & vbCrLf &
                      "    if(name !== 'example.com') return null;" & vbCrLf &
                      "    return { value: 'Example text', ttl: 300 };" & vbCrLf &
                      "}" & vbCrLf

    Return ctrl
  End Function
End Class
