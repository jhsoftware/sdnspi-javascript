Public Class CtrlOptions

  Public Overrides Sub LoadData(config As String)
    If String.IsNullOrEmpty(config) Then Exit Sub
    txtJS.Text = config
  End Sub

  Public Overrides Function SaveData() As String
    Return txtJS.Text.Trim
  End Function

  Public Overrides Function ValidateData() As Boolean
    Dim js = txtJS.Text.Trim()
    If js.Length = 0 Then MsgBox("JavaScript cannot be empty", MsgBoxStyle.Exclamation, "JavaScript plug-in") : Return False
    Return True
  End Function

  Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
    Try
      System.Diagnostics.Process.Start(LinkLabel1.Text)
    Catch ex As Exception
      MsgBox("Failed to open the following Internet address in your default Internet browser:" & vbCrLf &
                  LinkLabel1.Text & vbCrLf & vbCrLf &
                  "Error: " & ex.Message, MsgBoxStyle.Critical, "JavaScript plug-in")
    End Try
  End Sub

End Class
