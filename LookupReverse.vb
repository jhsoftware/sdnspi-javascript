﻿Option Strict Off
Imports JHSoftware.SimpleDNS
Imports JHSoftware.SimpleDNS.Plugin
Imports Microsoft.ClearScript.V8

Public Class LookupReverse
  Implements ILookupReverse
  Implements IOptionsUI

  Public Property Host As IHost Implements IPlugInBase.Host
  Private CfgJS As String
  Private Engine As V8ScriptEngine

  Public Function GetPlugInTypeInfo() As IPlugInBase.PlugInTypeInfo Implements IPlugInBase.GetTypeInfo
    Return New IPlugInBase.PlugInTypeInfo With {
      .Name = "JavaScript / Reverse lookup",
      .Description = "Executes a custom JavaScript function to look up a host name for an IP address",
      .InfoURL = "https://simpledns.plus/plugin-javascript"
    }
  End Function

  Public Sub LoadConfig(config As String, instanceID As Guid, dataPath As String) Implements IPlugInBase.LoadConfig
    CfgJS = config
  End Sub

  Public Function StartService() As Task Implements IPlugInBase.StartService
    Engine = New V8ScriptEngine()
    Engine.AddHostObject("console", New JSConsole(AddressOf Host.LogLine))
    Engine.Execute(CfgJS)
    Return Task.CompletedTask
  End Function

  Private Function Lookup(ip As SdnsIP, ctx As IRequestContext) As Task(Of LookupResult(Of DomName)) Implements ILookupReverse.LookupReverse
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

  Public Sub StopService() Implements IPlugInBase.StopService
    Engine.Dispose()
    Engine = Nothing
  End Sub

  Public Function InstanceConflict(config1 As String, config2 As String, ByRef errorMsg As String) As Boolean Implements IPlugInBase.InstanceConflict
    Return False
  End Function

  Public Function SaveState() As String Implements IPlugInBase.SaveState
    Return Nothing
  End Function

  Public Sub LoadState(state As String) Implements IPlugInBase.LoadState
    REM nothing
  End Sub

  Public Function GetOptionsUI(instanceID As Guid, dataPath As String) As OptionsUI Implements IOptionsUI.GetOptionsUI
    Dim ctrl = New CtrlOptions
    ctrl.txtJS.Text = "// Sample script for JavaScript / Reverse lookup plug-in" & vbCrLf &
                      "function Lookup(ip, context) {" & vbCrLf &
                      "    if(ip !== '1.2.3.4') return null;" & vbCrLf &
                      "    return { value: 'example.com', ttl: 300 };" & vbCrLf &
                      "}" & vbCrLf
    Return ctrl
  End Function

End Class