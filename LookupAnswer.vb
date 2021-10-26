Option Strict Off
Imports JHSoftware.SimpleDNS
Imports JHSoftware.SimpleDNS.Plugin
Imports Microsoft.ClearScript.V8

Public Class LookupAnswer
  Implements ILookupAnswer
  Implements IOptionsUI

  Public Property Host As IHost Implements IPlugInBase.Host
  Private CfgJS As String
  Private Engine As V8ScriptEngine

  Public Function GetPlugInTypeInfo() As IPlugInBase.PlugInTypeInfo Implements IPlugInBase.GetTypeInfo
    Return New IPlugInBase.PlugInTypeInfo With {
      .Name = "JavaScript / Answer lookup",
      .Description = "Executes a custom JavaScript function to do a DNS lookup",
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

  Public Sub StopService() Implements IPlugInBase.StopService
    Engine.Dispose()
    Engine = Nothing
  End Sub

  Private Function ILookupAnswer_LookupAnswer(ctx As IRequestContext) As Task(Of DNSAnswer) Implements ILookupAnswer.LookupAnswer
    Dim res = Engine.Evaluate("Lookup(" & CtxToJS(ctx) & ")")
    If res Is Nothing Then Return Task.FromResult(Of DNSAnswer)(Nothing)
    Dim rv = New DNSAnswer
    Dim RAA = res.aa
    Dim RRCode = res.rcode
    Dim RAnswer = res.answer
    Dim RAuthority = res.authority
    Dim RAdditional = res.additional
    If TypeOf RAA IsNot Microsoft.ClearScript.Undefined Then rv.AA = RAA
    If TypeOf RRCode Is Microsoft.ClearScript.Undefined Then
      rv.RCode = 0
    ElseIf TypeOf RRCode Is String Then
      rv.RCode = ParseRCode(RRCode)
    Else
      rv.RCode = RRCode
    End If
    If TypeOf RAnswer IsNot Microsoft.ClearScript.Undefined Then ProcSection(RAnswer, rv.Answer)
    If TypeOf RAuthority IsNot Microsoft.ClearScript.Undefined Then ProcSection(RAuthority, rv.Authority)
    If TypeOf RAdditional IsNot Microsoft.ClearScript.Undefined Then ProcSection(RAdditional, rv.Additional)
    Return Task.FromResult(rv)
  End Function

  Private Function ParseRCode(v As String) As Byte
    Select Case v.ToLower()
      Case "noerror"
        Return 0
      Case "formerr"
        Return 1
      Case "servfail"
        Return 2
      Case "nxdomain"
        Return 3
      Case "notimp"
        Return 4
      Case "refused"
        Return 5
      Case "ignore"
        Return 255
      Case Else
        Throw New Exception("Unknown RCode string value: " & v)
    End Select
  End Function

  Private Sub ProcSection(obj As Object, rvCol As List(Of DNSRecord))
    For i = 0 To obj.length - 1
      rvCol.Add(ProcRecord(obj(i)))
    Next
  End Sub

  Private Function ProcRecord(obj As Object) As DNSRecord
    Dim rv = New DNSRecord
    rv.Name = DomName.Parse(obj.name)
    Dim RType = obj.type
    If TypeOf RType Is Microsoft.ClearScript.Undefined Then
      Throw New Exception("Record .type property is missing")
    ElseIf TypeOf RType Is String Then
      If Not TryParseDNSRecType(RType, rv.RRType) Then Throw New Exception("Unknown record type: " & RType)
    Else
      rv.RRType = RType
    End If
    Dim RTTL = obj.ttl
    If TypeOf RTTL IsNot Microsoft.ClearScript.Undefined Then rv.TTL = RTTL
    rv.Data = obj.data
    Return rv
  End Function

  Public Function SaveState() As String Implements IPlugInBase.SaveState
    Return Nothing
  End Function

  Public Sub LoadState(state As String) Implements IPlugInBase.LoadState
    REM nothing
  End Sub

  Public Function InstanceConflict(config1 As String, config2 As String, ByRef errorMsg As String) As Boolean Implements IPlugInBase.InstanceConflict
    Return False
  End Function

  Public Function GetOptionsUI(instanceID As Guid, dataPath As String) As OptionsUI Implements IOptionsUI.GetOptionsUI
    Dim ctrl = New CtrlOptions
    ctrl.txtJS.Text =
"// Sample script for JavaScript / Answer lookup plug-in" & vbCrLf &
"function Lookup(context) {" & vbCrLf &
"  if(context.qname !== 'example.com') return null;" & vbCrLf &
"  if(context.qtype !== 'MX') return null;" & vbCrLf &
"  return { aa: true," & vbCrLf &
"           answer: [" & vbCrLf &
"             { name: context.qname," & vbCrLf &
"               type: 'MX'," & vbCrLf &
"               ttl: 300," & vbCrLf &
"               data: '10 mail.example.com.' }" & vbCrLf &
"           ]};" & vbCrLf &
"}"
    Return ctrl
  End Function

End Class
