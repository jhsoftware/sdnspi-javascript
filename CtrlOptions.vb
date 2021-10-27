Public Class CtrlOptions

  Friend TheScript As String

  Public Overrides Sub LoadData(config As String)
    If String.IsNullOrEmpty(config) Then Exit Sub
    Dim cfg = MyConfig.DeSerialize(config)
    If String.IsNullOrEmpty(cfg.File) Then
      TheScript = cfg.Script
      radNoFile.Checked = True
    Else
      radFile.Checked = True
      txtFile.Text = cfg.File
    End If
    chkDebug.Checked = cfg.Debug
    txtPort.Value = cfg.DebugPort
    txtPort.Enabled = cfg.Debug
  End Sub

  Public Overrides Function SaveData() As String
    Dim rv = New MyConfig
    If radNoFile.Checked Then
      rv.Script = TheScript
      rv.File = ""
    Else
      rv.Script = ""
      rv.File = txtFile.Text.Trim()
    End If
    rv.Debug = chkDebug.Checked
    rv.DebugPort = txtPort.Value
    Return rv.Serialize()
  End Function

  Public Overrides Function ValidateData() As Boolean
    If radFile.Checked Then
      If Not RemoteGUI AndAlso Not My.Computer.FileSystem.FileExists(txtFile.Text.Trim()) Then MsgBox("The specified file does not exist.", MsgBoxStyle.Exclamation, "JavaScript plug-in") : Return False
    Else
      If TheScript.Length = 0 Then MsgBox("JavaScript cannot be empty", MsgBoxStyle.Exclamation, "JavaScript plug-in") : Return False
    End If
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

  Private Sub chkDebug_CheckedChanged(sender As Object, e As EventArgs) Handles chkDebug.CheckedChanged
    txtPort.Enabled = chkDebug.Checked
  End Sub

  Private Sub radFile_CheckedChanged(sender As Object, e As EventArgs) Handles radFile.CheckedChanged, radNoFile.CheckedChanged
    txtFile.Enabled = radFile.Checked
    btnBrowse.Enabled = radFile.Checked
  End Sub

  Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
    If RemoteGUI Then MsgBox("Cannot browse for file on remote connection", MsgBoxStyle.Exclamation, "JavaScript plug-in") : Exit Sub
    SaveFileDialog1.FileName = txtFile.Text
    If SaveFileDialog1.ShowDialog() <> DialogResult.OK Then Exit Sub
    txtFile.Text = SaveFileDialog1.FileName
  End Sub

  Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
    Dim x As String = ""
    If radFile.Checked Then
      txtFile.Text = txtFile.Text.Trim()
      If RemoteGUI Then MsgBox("Cannot edit file on remote connection", MsgBoxStyle.Exclamation, "JavaScript plug-in") : Exit Sub
      If txtFile.Text.Length = 0 Then MsgBox("File name not specified", MsgBoxStyle.Exclamation, "JavaScript plug-in") : Exit Sub
      Try
        x = My.Computer.FileSystem.ReadAllText(txtFile.Text)
      Catch ex As System.IO.FileNotFoundException
        REM OK - proceed with blank
      Catch ex As Exception
        MsgBox("Error opening file:" & vbCrLf & ex.Message, MsgBoxStyle.Exclamation, "JavaScript plug-in") : Exit Sub
      End Try
    Else
      x = TheScript
    End If

    Dim frm = New frmScript
    frm.txtJS.Text = x
    If radFile.Checked Then
      frm.Text = "JavaScript - " & txtFile.Text
      frm.btnOK.Text = "Save"
    End If
mark1:
    If frm.ShowDialog(Me.ParentForm) <> DialogResult.OK Then Exit Sub

    x = frm.txtJS.Text
    If radFile.Checked Then
      Try
        My.Computer.FileSystem.WriteAllText(txtFile.Text, x, False)
      Catch ex As Exception
        MsgBox("Error saving file:" & vbCrLf & ex.Message, MsgBoxStyle.Exclamation, "JavaScript plug-in")
        GoTo mark1
      End Try
    Else
      TheScript = x
    End If

  End Sub

End Class
