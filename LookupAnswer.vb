Option Strict Off
Imports JHSoftware.SimpleDNS
Imports JHSoftware.SimpleDNS.Plugin

Public Class LookupAnswer
  Inherits LookupBase
  Implements ILookupAnswer
  Implements IOptionsUI

  Public Overrides Function GetTypeInfo() As TypeInfo Implements IPlugInBase.GetTypeInfo
    Return New TypeInfo With {
      .Name = "JavaScript / Answer lookup",
      .Description = "Executes a custom JavaScript function to do a DNS lookup returning multiple DNS records and/or special response properties (AA flag / RCode value)",
      .InfoURL = "https://simpledns.plus/plugin-javascript"
    }
  End Function

  Private Function ILookupAnswer_LookupAnswer(ctx As IRequestContext) As Task(Of DNSAnswer) Implements ILookupAnswer.LookupAnswer
    LoadScript()
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

  Public Function GetOptionsUI(instanceID As Guid, dataPath As String) As OptionsUI Implements IOptionsUI.GetOptionsUI
    Dim ctrl = New CtrlOptions
    ctrl.TheScript =
"// Sample script for JavaScript / Answer lookup plug-in" & vbCrLf &
"function Lookup(context) {" & vbCrLf &
"  if(context.qname !== 'example.com') return null;" & vbCrLf &
"  if(context.qtype !== 'MX') return null;" & vbCrLf &
"  return { aa: true," & vbCrLf &
"           answer: [" & vbCrLf &
"             { name: context.qname, type: 'MX', ttl: 300, data: '10 server1.example.com.' }," & vbCrLf &
"             { name: context.qname, type: 'MX', ttl: 300, data: '20 server2.example.com.' }" & vbCrLf &
"           ]};" & vbCrLf &
"}"
    Return ctrl
  End Function

End Class
