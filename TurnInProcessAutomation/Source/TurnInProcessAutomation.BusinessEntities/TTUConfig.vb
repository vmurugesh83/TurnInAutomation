Imports System.ComponentModel

<Serializable>
Public Class TTUConfig

    <DataObjectField(True)> _
    Public Property ConfigKey() As String

    <DataObjectField(True)> _
    Public Property SecondLevelConfigKey() As String

    <DataObjectField(True)> _
    Public Property NumericConfigValue() As Decimal

    <DataObjectField(True)> _
    Public Property DateConfigValue() As Date

    <DataObjectField(True)> _
    Public Property StringConfigValue() As String

    <DataObjectField(True)> _
    Public Property ConfigDescription() As String

    <DataObjectField(True)> _
    Public Property LastModifiedBy() As String

    <DataObjectField(True)> _
    Public Property LastModifiedTimestamp() As DateTime

End Class
