Imports TurnInAutomationBatch.BLL.Enumerations
Imports System.Configuration
Imports System.IO
Imports System.Text.RegularExpressions

Public Class LogHelper

#Region "Logging"
    ''' <summary>
    ''' Checks whether log file for today is present or not.  If its present, then it appends the text to the log file.
    ''' If not, it creates the log file and writes the text.
    ''' </summary>
    ''' <param name="logText"></param>
    ''' <remarks></remarks>
    Public Shared Sub WriteToLogFile(ByVal logEntryType As LogEntryType, ByVal logText As String)
        Dim dirInfo As DirectoryInfo = Nothing
        Dim fileStream As FileStream = Nothing
        Dim streamWriter As StreamWriter = Nothing
        Dim logFileInfo As FileInfo = Nothing
        Dim logPath As String = String.Empty
        Dim fileName As String = String.Empty

        Try
            logPath = ConfigurationManager.AppSettings("LogFilePath").ToString()
            fileName = String.Concat("TurnInAutomationBatch_", DateTime.Today.ToString("yyyyMMdd"), ".log")
            logFileInfo = New FileInfo(Path.Combine(logPath, fileName))
            dirInfo = New DirectoryInfo(logFileInfo.DirectoryName)

            If Not dirInfo.Exists Then
                dirInfo.Create()
            End If
            If Not logFileInfo.Exists Then
                fileStream = logFileInfo.Create()
            Else
                fileStream = New FileStream(Path.Combine(logPath, fileName), FileMode.Append)
            End If

            streamWriter = New StreamWriter(fileStream)
            logText = String.Concat([Enum].GetName(GetType(LogEntryType), logEntryType), ": ", DateTime.Now().ToString("yyyy-MM-dd HH:mm:ss: "), logText)
            streamWriter.WriteLine(logText)
        Catch ex As Exception
            Throw ex
        Finally
            If Not streamWriter Is Nothing Then
                streamWriter.Close()
            End If
            If Not fileStream Is Nothing Then
                fileStream.Close()
            End If
        End Try
    End Sub
    ''' <summary>
    ''' Writes the log messages to the console only if it is in debug mode
    ''' </summary>
    ''' <param name="logText"></param>
    ''' <remarks></remarks>
    Public Shared Sub WriteToConsole(ByVal logText As String)
        If Debugger.IsAttached Then
            Console.WriteLine(logText)
        End If
    End Sub
    ''' <summary>
    ''' Deletes the log files that were modified before the threshold date
    ''' </summary>
    ''' <param name="intNumberOfDays"></param>
    ''' <remarks></remarks>
    Public Shared Sub DeleteOldLogs(ByVal intNumberOfDays As Integer)
        Dim dtDateThreshold As DateTime = DateTime.MinValue
        Dim dtCurrentDate As Date = DateTime.Now
        Dim directoryInfo As DirectoryInfo = Nothing

        'If the host application has requested a number more than 1, delete files older than that number of days
        If intNumberOfDays > 0 Then
            Dim Deleted As Integer = 0

            'Figure out the date on and before which all files should be deleted
            dtDateThreshold = dtCurrentDate.AddDays(intNumberOfDays * -1)
            WriteToLogFile(LogEntryType.Information, "Deleting files that are more than " & intNumberOfDays & " days old (Files before: " & dtDateThreshold.ToShortDateString & ")")
            WriteToConsole("Deleting files that are more than " & intNumberOfDays & " days old (Files before: " & dtDateThreshold.ToShortDateString & ")")

            'Get a list of files in the current log directory that qualify as log files from this application
            directoryInfo = New System.IO.DirectoryInfo(ConfigurationManager.AppSettings("LogFilePath").ToString())
            For Each file As FileInfo In directoryInfo.GetFiles
                If file.Name.StartsWith("TurnInAutomationBatch_") AndAlso file.Extension.Equals(".log") AndAlso file.LastWriteTime <= dtDateThreshold Then
                    Try
                        file.Delete()
                        WriteToLogFile(LogEntryType.Information, " Deleted: " & vbTab & file.Name)
                    Catch ex As Exception
                        WriteToLogFile(LogEntryType.AppError, " Issue Deleting: " & vbTab & file.Name & vbTab & ex.Message)
                    End Try
                End If
            Next
        End If
    End Sub

#End Region
End Class
