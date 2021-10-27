<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CtrlOptions
  Inherits JHSoftware.SimpleDNS.Plugin.OptionsUI

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing AndAlso components IsNot Nothing Then
      components.Dispose()
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Me.Label2 = New System.Windows.Forms.Label()
    Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
    Me.chkDebug = New System.Windows.Forms.CheckBox()
    Me.txtPort = New JHSoftware.SimpleDNS.TextBoxInt()
    Me.btnEdit = New System.Windows.Forms.Button()
    Me.Label1 = New System.Windows.Forms.Label()
    Me.radFile = New System.Windows.Forms.RadioButton()
    Me.radNoFile = New System.Windows.Forms.RadioButton()
    Me.txtFile = New System.Windows.Forms.TextBox()
    Me.btnBrowse = New System.Windows.Forms.Button()
    Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
    Me.SuspendLayout()
    '
    'Label2
    '
    Me.Label2.AutoSize = True
    Me.Label2.Location = New System.Drawing.Point(16, 183)
    Me.Label2.Margin = New System.Windows.Forms.Padding(23, 0, 0, 0)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(149, 13)
    Me.Label2.TabIndex = 8
    Me.Label2.Text = "For info on how to debug, visit"
    '
    'LinkLabel1
    '
    Me.LinkLabel1.AutoSize = True
    Me.LinkLabel1.Location = New System.Drawing.Point(165, 183)
    Me.LinkLabel1.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
    Me.LinkLabel1.Name = "LinkLabel1"
    Me.LinkLabel1.Size = New System.Drawing.Size(192, 13)
    Me.LinkLabel1.TabIndex = 9
    Me.LinkLabel1.TabStop = True
    Me.LinkLabel1.Text = "https://simpledns.plus/plugin-javascript"
    '
    'chkDebug
    '
    Me.chkDebug.AutoSize = True
    Me.chkDebug.Location = New System.Drawing.Point(0, 158)
    Me.chkDebug.Margin = New System.Windows.Forms.Padding(3, 3, 0, 8)
    Me.chkDebug.Name = "chkDebug"
    Me.chkDebug.Size = New System.Drawing.Size(143, 17)
    Me.chkDebug.TabIndex = 6
    Me.chkDebug.Text = "Enable debugging - Port:"
    Me.chkDebug.UseVisualStyleBackColor = True
    '
    'txtPort
    '
    Me.txtPort.Enabled = False
    Me.txtPort.Location = New System.Drawing.Point(143, 156)
    Me.txtPort.Margin = New System.Windows.Forms.Padding(0, 3, 3, 3)
    Me.txtPort.MaxLength = 11
    Me.txtPort.Minimum = 1
    Me.txtPort.Name = "txtPort"
    Me.txtPort.Size = New System.Drawing.Size(53, 20)
    Me.txtPort.TabIndex = 7
    Me.txtPort.Text = "9222"
    Me.txtPort.Value = 9222
    '
    'btnEdit
    '
    Me.btnEdit.Location = New System.Drawing.Point(0, 95)
    Me.btnEdit.Margin = New System.Windows.Forms.Padding(3, 3, 3, 18)
    Me.btnEdit.Name = "btnEdit"
    Me.btnEdit.Size = New System.Drawing.Size(143, 23)
    Me.btnEdit.TabIndex = 2
    Me.btnEdit.Text = "Edit JavaScript code..."
    Me.btnEdit.UseVisualStyleBackColor = True
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Location = New System.Drawing.Point(0, 0)
    Me.Label1.Margin = New System.Windows.Forms.Padding(3, 0, 3, 5)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(115, 13)
    Me.Label1.TabIndex = 0
    Me.Label1.Text = "Store JavaScript code:"
    '
    'radFile
    '
    Me.radFile.AutoSize = True
    Me.radFile.Location = New System.Drawing.Point(0, 44)
    Me.radFile.Margin = New System.Windows.Forms.Padding(3, 3, 0, 3)
    Me.radFile.Name = "radFile"
    Me.radFile.Size = New System.Drawing.Size(97, 17)
    Me.radFile.TabIndex = 3
    Me.radFile.Text = "In separate file:"
    Me.radFile.UseVisualStyleBackColor = True
    '
    'radNoFile
    '
    Me.radNoFile.AutoSize = True
    Me.radNoFile.Checked = True
    Me.radNoFile.Location = New System.Drawing.Point(0, 21)
    Me.radNoFile.Name = "radNoFile"
    Me.radNoFile.Size = New System.Drawing.Size(145, 17)
    Me.radNoFile.TabIndex = 1
    Me.radNoFile.TabStop = True
    Me.radNoFile.Text = "With plug-in configuration"
    Me.radNoFile.UseVisualStyleBackColor = True
    '
    'txtFile
    '
    Me.txtFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.txtFile.Enabled = False
    Me.txtFile.Location = New System.Drawing.Point(19, 64)
    Me.txtFile.Margin = New System.Windows.Forms.Padding(0, 0, 3, 8)
    Me.txtFile.Name = "txtFile"
    Me.txtFile.Size = New System.Drawing.Size(374, 20)
    Me.txtFile.TabIndex = 4
    '
    'btnBrowse
    '
    Me.btnBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.btnBrowse.Enabled = False
    Me.btnBrowse.Location = New System.Drawing.Point(399, 62)
    Me.btnBrowse.Name = "btnBrowse"
    Me.btnBrowse.Size = New System.Drawing.Size(27, 23)
    Me.btnBrowse.TabIndex = 5
    Me.btnBrowse.Text = "..."
    Me.btnBrowse.UseVisualStyleBackColor = True
    '
    'SaveFileDialog1
    '
    Me.SaveFileDialog1.CreatePrompt = True
    Me.SaveFileDialog1.DefaultExt = "js"
    Me.SaveFileDialog1.Filter = "JavaScript files|*.js|All files|*.*"
    Me.SaveFileDialog1.OverwritePrompt = False
    Me.SaveFileDialog1.Title = "Specify JavaScript file"
    '
    'CtrlOptions
    '
    Me.Controls.Add(Me.btnBrowse)
    Me.Controls.Add(Me.txtFile)
    Me.Controls.Add(Me.radNoFile)
    Me.Controls.Add(Me.radFile)
    Me.Controls.Add(Me.Label1)
    Me.Controls.Add(Me.btnEdit)
    Me.Controls.Add(Me.txtPort)
    Me.Controls.Add(Me.chkDebug)
    Me.Controls.Add(Me.LinkLabel1)
    Me.Controls.Add(Me.Label2)
    Me.Name = "CtrlOptions"
    Me.Size = New System.Drawing.Size(426, 217)
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents Label2 As Label
  Friend WithEvents LinkLabel1 As LinkLabel
  Friend WithEvents chkDebug As CheckBox
  Friend WithEvents txtPort As JHSoftware.SimpleDNS.TextBoxInt
  Friend WithEvents btnEdit As Button
  Friend WithEvents Label1 As Label
  Friend WithEvents radFile As RadioButton
  Friend WithEvents radNoFile As RadioButton
  Friend WithEvents txtFile As TextBox
  Friend WithEvents btnBrowse As Button
  Friend WithEvents SaveFileDialog1 As SaveFileDialog
End Class
