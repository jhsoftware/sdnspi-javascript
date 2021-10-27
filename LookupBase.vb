Imports Microsoft.ClearScript.V8
Imports JHSoftware.SimpleDNS
Imports JHSoftware.SimpleDNS.Plugin
Imports System.IO

Public Class LookupBase
  Implements IPlugInBase

  Friend Cfg As MyConfig
  Friend Engine As V8ScriptEngine
  Friend Script As String
  Private ScriptLoaded As Boolean
  WithEvents FSW As IO.FileSystemWatcher

  Public Property Host As IHost Implements IPlugInBase.Host

  Public Sub LoadConfig(config As String, instanceID As Guid, dataPath As String) Implements IPlugInBase.LoadConfig
    Cfg = MyConfig.DeSerialize(config)
  End Sub

  Public Function StartService() As Task Implements IPlugInBase.StartService
    If Cfg.Debug Then
      Engine = New V8ScriptEngine(V8ScriptEngineFlags.EnableDebugging, Cfg.DebugPort)
    Else
      Engine = New V8ScriptEngine()
    End If
    Engine.AddHostObject("console", New JSConsole(AddressOf Host.LogLine))
    ScriptLoaded = False
    If String.IsNullOrEmpty(Cfg.File) Then
      Script = Cfg.Script
    Else
      Script = My.Computer.FileSystem.ReadAllText(Cfg.File)
      FSW = New FileSystemWatcher()
      FSW.Path = Cfg.File.Substring(0, Cfg.File.LastIndexOf("\"))
      FSW.Filter = Cfg.File.Substring(Cfg.File.LastIndexOf("\") + 1)
      FSW.IncludeSubdirectories = False
      FSW.NotifyFilter = NotifyFilters.LastWrite
      FSW.EnableRaisingEvents = True
    End If
    Return Task.CompletedTask
  End Function

  Public Sub StopService() Implements IPlugInBase.StopService
    Engine.Dispose()
    Engine = Nothing
    If FSW IsNot Nothing Then
      FSW.EnableRaisingEvents = False
      FSW.Dispose()
      FSW = Nothing
    End If
  End Sub

  Protected Sub LoadScript()
    REM Used to ensure that script is executed only once - but not before first DNS request, to allow debugger to connect first
    If ScriptLoaded Then Exit Sub
    Engine.Execute(Script)
    ScriptLoaded = True
  End Sub

  Private Sub FSW_Changed(sender As Object, e As FileSystemEventArgs) Handles FSW.Changed
    If ScriptLoaded Then
      Host.LogLine("JavaScript file changed - Restarting")
      StopService()
      StartService()
    Else
      Host.LogLine("JavaScript file changed - Reloading")
      Script = My.Computer.FileSystem.ReadAllText(Cfg.File)
    End If
  End Sub

  Public Overridable Function GetTypeInfo() As TypeInfo Implements IPlugInBase.GetTypeInfo
    REM overridden in inheritors
    Throw New NotImplementedException()
  End Function

  Protected Function CtxToJS(ctx As IRequestContext) As String
    'qnameip ?
    Return "{" &
      "fromip:'" & ctx.FromIP.ToString() & "'," &
      "qname:'" & ctx.QName.ToString().Replace("'", "\'") & "'," &
      "qtype:'" & ctx.QType.Name() & "'," &
      "rd:" & If(ctx.RD, "true", "false") & "," &
      "ra:" & If(ctx.RA, "true", "false") & "," &
      "aa:" & If(ctx.AA, "true", "false") & "}"
  End Function

  Public Function InstanceConflict(config1 As String, config2 As String, ByRef errorMsg As String) As Boolean Implements IPlugInBase.InstanceConflict
    Return False
  End Function

End Class

Friend Class MyConfig
  Public Script As String = ""
  Public File As String = ""
  Public Debug As Boolean = False
  Public DebugPort As Integer = 9222

  Friend Function Serialize() As String
    Dim o = New Json.Object
    If Not String.IsNullOrEmpty(File) Then
      o.Add("file", File)
    Else
      o.Add("script", Script)
    End If
    o.Add("debug", Debug)
    o.Add("debugport", DebugPort)
    Return o.EncodeJson(False)
  End Function

  Friend Shared Function DeSerialize(v As String) As MyConfig
    If Not v.Trim.StartsWith("{") Then Return New MyConfig With {.Script = v}
    Dim o = DirectCast(Json.Parse(v), Json.Object)
    Dim rv = New MyConfig
    If o.HasMember("file") Then
      rv.File = o.GetString("file")
    Else
      rv.Script = o.GetString("script")
    End If
    rv.Debug = o.GetBool("debug")
    rv.DebugPort = o.GetInt("debugport")
    Return rv
  End Function

End Class

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

