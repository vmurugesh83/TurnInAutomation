Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Xml.Serialization
Imports System.Text
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Xml

Public Class Helper
    Public Shared Function Serialize(ByVal obj As Object) As String

        Dim stream As New MemoryStream
        Dim ds As New XmlSerializer(GetType(TurnInProcessAutomation.BusinessEntities.MerchandiseSample), "http://schema.bonton.com/Schema/SampleSchema")
        ds.Serialize(stream, obj)
        Dim xmlString As String = Encoding.UTF8.GetString(stream.ToArray())
        stream.Close()
        Return xmlString

        'Dim settings As New XmlWriterSettings()
        'Dim stream As New MemoryStream
        'Dim writer As XmlWriter = XmlWriter.Create(stream, settings)
        'Dim names As New XmlSerializerNamespaces
        'names.Add("", "")
        'names.Add("xmlns", "http://schema.bonton.com/Schema/SampleSchema")
        'Dim ds As New XmlSerializer(GetType(MerchandiseSample))
        'ds.Serialize(writer, obj, names)
        'stream.Flush()
        'stream.Seek(0, SeekOrigin.Begin)
        'Dim xmlString As String = Encoding.UTF8.GetString(stream.ToArray())
        'stream.Close()
        'Return xmlString
    End Function

    Public Shared Function SerializeHeader(ByVal obj As Object) As String
        Dim settings As New XmlWriterSettings()
        settings.OmitXmlDeclaration = True
        Dim stream As New MemoryStream
        Dim writer As XmlWriter = XmlWriter.Create(stream, settings)
        Dim ds As New XmlSerializer(obj.GetType)
        Dim names As New XmlSerializerNamespaces
        names.Add("", "")
        ds.Serialize(writer, obj, names)
        stream.Flush()
        stream.Seek(0, SeekOrigin.Begin)

        Dim reader As New StreamReader(stream)
        stream.Position = 0  ' Seek to start of stream
        Return reader.ReadToEnd()
    End Function

    Public Shared Function SerializeDetail(ByVal obj As Object) As String
        Dim settings As New XmlWriterSettings()
        settings.OmitXmlDeclaration = True
        Dim stream As New MemoryStream
        Dim writer As XmlWriter = XmlWriter.Create(stream, settings)
        Dim ds As New XmlSerializer(obj.GetType)
        Dim names As New XmlSerializerNamespaces
        names.Add("", "")
        ds.Serialize(writer, obj, names)
        stream.Flush()
        stream.Seek(0, SeekOrigin.Begin)

        Dim reader As New StreamReader(stream)
        stream.Position = 0  ' Seek to start of stream
        Return reader.ReadToEnd()
    End Function

    Public Shared Function SerializeList(Of T)(ByVal obj As T) As String
        Dim XmlBuddy As New System.Xml.Serialization.XmlSerializer(GetType(T))
        Dim MySettings As New System.Xml.XmlWriterSettings()
        MySettings.Indent = True
        MySettings.CloseOutput = True
        MySettings.OmitXmlDeclaration = True

        Dim stream As New MemoryStream
        Dim MyWriter As System.Xml.XmlWriter = System.Xml.XmlWriter.Create(stream, MySettings)
        XmlBuddy.Serialize(MyWriter, obj)
        stream.Flush()
        stream.Seek(0, SeekOrigin.Begin)
        Dim xmlString As String = Encoding.UTF8.GetString(stream.ToArray())
        MyWriter.Flush()
        MyWriter.Close()
        stream.Close()
        Return xmlString
    End Function

    Public Shared Function Deserialize(ByVal XMLString As String) As TurnInProcessAutomation.BusinessEntities.MerchandiseSample
        Dim xRoot As New XmlRootAttribute
        xRoot.Namespace = "http://workhorse.qa.bonton.com/Schema/BonTon_WhMessage"
        Dim ser As New XmlSerializer(GetType(TurnInProcessAutomation.BusinessEntities.MerchandiseSample), xRoot)
        Dim stream As New MemoryStream(Encoding.UTF8.GetBytes(XMLString))
        Dim merchandise As TurnInProcessAutomation.BusinessEntities.MerchandiseSample = ser.Deserialize(stream)
        Return merchandise
    End Function

    Public Shared Function Serialize1(ByVal obj As Object) As String

        Dim stream As New MemoryStream
        Dim ds As New XmlSerializer(GetType(TurnInProcessAutomation.BusinessEntities.MerchandiseSample))
        ds.Serialize(stream, obj)
        Dim xmlString As String = Encoding.UTF8.GetString(stream.ToArray())
        stream.Close()
        Return xmlString

    End Function


    Public Shared Function Deserialize1(ByVal jsonString As String) As TurnInProcessAutomation.BusinessEntities.MerchandiseSample

        Dim ser As New XmlSerializer(GetType(TurnInProcessAutomation.BusinessEntities.MerchandiseSample))
        Dim stream As New MemoryStream(Encoding.UTF8.GetBytes(jsonString))
        Dim merchandise As TurnInProcessAutomation.BusinessEntities.MerchandiseSample = ser.Deserialize(stream)
        Return merchandise

    End Function
End Class
