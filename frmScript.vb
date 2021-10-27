Public Class frmScript

  Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
    If txtJS.Text.Trim().Length = 0 Then
      MsgBox("JavaScript cannot be empty", MsgBoxStyle.Exclamation, "JavaScript plug-in")
      Exit Sub
    End If
    Me.DialogResult = DialogResult.OK
  End Sub

  Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
    Try
      System.Diagnostics.Process.Start(LinkLabel1.Text)
    Catch ex As Exception
      MsgBox("Failed to open the following Internet address in your default Internet browser:" & vbCrLf &
                  LinkLabel1.Text & vbCrLf & vbCrLf &
                  "Error: " & ex.Message, MsgBoxStyle.Critical, "JavaScript plug-in")
    End Try
  End Sub

  Private Sub frmScript_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    txtJS.Select(0, 0)
  End Sub

End Class