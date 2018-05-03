Imports TurnInProcessAutomation.BusinessEntities
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Text
Imports System.IO
Imports System.Xml.Serialization
Imports IBM.Data.DB2
Imports Microsoft.VisualBasic.FileIO
Imports System.Data.SqlClient
Imports System.Web
Imports System.Xml
Imports System.Net

Public Class WorkhorseHelper

    Public Shared ReadOnly workhorseAddress As String = "http://workhorse.test.bonton.com/MessageHandler.cfm"

    Enum Operation
        MerchCreate
        MerchUpdate
        MerchGet
        JobCreateOrUpdate
    End Enum

    Public Shared Function SendRequest(ByVal WorkHorseXML As String) As String
        Try
            
            If WorkHorseXML.Length = 0 Then
                Console.WriteLine(" No sample for XML found ")
                Return ""
            End If

            Dim bytes As Byte() = Encoding.UTF8.GetBytes(WorkHorseXML)

            Dim request As HttpWebRequest = DirectCast(WebRequest.Create(workhorseAddress), HttpWebRequest)
            request.Method = "POST"
            request.ContentLength = bytes.Length
            request.ContentType = "text/xml"
            Using requestStream As Stream = request.GetRequestStream()
                requestStream.Write(bytes, 0, bytes.Length)
            End Using


            Dim ReceiveStream As Stream
            Dim encode As Encoding
            Dim sr As StreamReader

            Using myresponse As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                If myresponse.StatusCode <> HttpStatusCode.OK Then
                    Dim message As String = [String].Format("POST failed. Received HTTP {0}", myresponse.StatusCode)
                    Throw New ApplicationException(message)
                End If
                ReceiveStream = myresponse.GetResponseStream()
                encode = System.Text.Encoding.GetEncoding("utf-8")
                sr = New StreamReader(ReceiveStream)

                Dim responsString As String = sr.ReadToEnd
                Return responsString
            End Using

        Catch ex As Exception
            Console.Write(ex)
            Return ""
        End Try
    End Function

    Public Shared Function CreateRequest(ByVal m As MerchandiseSample, ByVal Op As Operation) As String
        If Not IsNothing(m) Then
            Dim whHeader As New RequestHeader

            Dim whDetail As New RequestDetail
            whDetail.MerchInfo.Add(m)

            Dim sb As New StringBuilder()
            sb.Append("<?xml version='1.0'?>")
            sb.Append("<WorkhorseRequest xmlns='http://workhorse.qa.bonton.com/Schema/BonTon_WhMessage' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:schemaLocation='http://workhorse.qa.bonton.com/Schema/BonTon_WhMessage http://workhorse.qa.bonton.com/Schema/BonTon_WhMessage.xsd'>")
            sb.Append("<RequestHeader>")
            sb.Append("<SecurityInfo>")
            sb.Append("<workhorseLogin>msg</workhorseLogin>")
            sb.Append("<encryptedPassword>341A10747C4533634CA4F0588767AECDB39F2F5B5CD77C6640258CB8D47DF149</encryptedPassword>")
            sb.Append("<mutualToken>msg</mutualToken>")
            sb.Append("</SecurityInfo>")
            sb.Append("<Operation>" & Op.ToString & "</Operation>")
            sb.Append("</RequestHeader>")

            sb.Append(Helper.SerializeDetail(whDetail))


            sb.Append("</WorkhorseRequest>")
            Return sb.ToString
        Else
            Return String.Empty
        End If

    End Function

    Public Shared Function CreateRequest(ByVal Op As Operation, ByVal MerchXML As String) As String

        Dim whHeader As New RequestHeader

        'Dim whDetail As New RequestDetail
        'whDetail.MerchInfo.Add(m)

        Dim sb As New StringBuilder()
        sb.Append("<?xml version='1.0'?>")
        sb.Append("<WorkhorseRequest xmlns='http://workhorse.qa.bonton.com/Schema/BonTon_WhMessage' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:schemaLocation='http://workhorse.qa.bonton.com/Schema/BonTon_WhMessage http://workhorse.qa.bonton.com/Schema/BonTon_WhMessage.xsd'>")
        sb.Append("<RequestHeader>")
        sb.Append("<SecurityInfo>")
        sb.Append("<workhorseLogin>msg</workhorseLogin>")
        sb.Append("<encryptedPassword>341A10747C4533634CA4F0588767AECDB39F2F5B5CD77C6640258CB8D47DF149</encryptedPassword>")
        sb.Append("<mutualToken>msg</mutualToken>")
        sb.Append("</SecurityInfo>")
        sb.Append("<Operation>" & Op.ToString & "</Operation>")
        sb.Append("</RequestHeader>")
        sb.Append("<RequestDetail>")
        sb.Append(MerchXML)
        sb.Append("</RequestDetail>")

        sb.Append("</WorkhorseRequest>")
        Return sb.ToString
        

    End Function

    Public Shared Function GetMerchRequest(ByVal merchId As String) As String
        Return "<?xml version='1.0' encoding='UTF-8'?>" & _
         "<WorkhorseRequest xmlns:out='http://workhorse.qa.bonton.com/Schema/BonTon_WhMessage' xmlns='http://workhorse.qa.bonton.com/Schema/BonTon_WhMessage' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:schemaLocation='http://workhorse.qa.bonton.com/Schema/BonTon_WhMessage http://workhorse.qa.bonton.com/Schema/BonTon_WhMessage.xsd'>" & _
            "<RequestHeader>" & _
                "<SecurityInfo>" & _
                    "<workhorseLogin>msg</workhorseLogin>" & _
            "<encryptedPassword>341A10747C4533634CA4F0588767AECDB39F2F5B5CD77C6640258CB8D47DF149</encryptedPassword>" & _
            "<mutualToken>BonTon</mutualToken>" & _
        "</SecurityInfo>" & _
        "<Operation>MerchGet</Operation>" & _
    "</RequestHeader>" & _
    "<RequestDetail>" & _
      "  <MerchInfo>" & _
     "       <MerchandiseSample>" & _
    "            <MerchID>merchId</MerchID>" & _
   "         </MerchandiseSample>" & _
  "      </MerchInfo>" & _
 "   </RequestDetail>" & _
"</WorkhorseRequest>"
    End Function

    Public Shared Sub ProcessReponse(ByVal WHResponse As String, ByRef m As MerchandiseSample)
        'Console.Write(responsString)
        Dim myXML As New XmlDocument
        myXML.LoadXml(WHResponse)
        Dim response As XmlNodeList = myXML.GetElementsByTagName("ResponseCode")
        Dim responseMsg As XmlNodeList = myXML.GetElementsByTagName("ResponseMessage")

        Dim MerchandiseObject As XmlNodeList = myXML.GetElementsByTagName("MerchInfo")

        m = Helper.Deserialize(MerchandiseObject.Item(0).InnerXml)

        If response.Item(0).InnerText = "WHG-0000" Then
            Console.WriteLine("Response from Workhorse : " + response.Item(0).InnerText)
        Else
            Console.WriteLine(String.Format("Workhorse Returned Error: {0}", response.Item(0).InnerText))
            Console.WriteLine(String.Format("Response : {0}", responseMsg.Item(0).InnerText))
        End If
    End Sub
End Class
