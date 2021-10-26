Imports JHSoftware.SimpleDNS
Imports JHSoftware.SimpleDNS.Plugin

Friend Module ModShared

  Function CtxToJS(ctx As IRequestContext) As String
    'qnameip ?
    Return "{" &
      "fromip:'" & ctx.FromIP.ToString() & "'," &
      "qname:'" & ctx.QName.ToString().Replace("'", "\'") & "'," &
      "qtype:'" & ctx.QType.Name() & "'," &
      "rd:" & If(ctx.RD, "true", "false") & "," &
      "ra:" & If(ctx.RA, "true", "false") & "," &
      "aa:" & If(ctx.AA, "true", "false") & "}"
  End Function


End Module

Public Class JSConsole
  Private DoLog As Action(Of String)
  Friend Sub New(doLog As Action(Of String))
    Me.DoLog = doLog
  End Sub

  Public Function log(v As String) As Object
    DoLog.Invoke(v)
    Return Nothing
  End Function
End Class