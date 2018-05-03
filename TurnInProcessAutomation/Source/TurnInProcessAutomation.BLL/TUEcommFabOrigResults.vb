Imports TurnInProcessAutomation.BLL.CommonBO
Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.SqlDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient
Imports System.Xml.Linq

Public Class TUEcommFabOrig
    Private dalDB2 As MainframeDAL.EcommFabOrigDao = New MainframeDAL.EcommFabOrigDao

    Public Function GetAllEcommFabOrigResultsByPOShipDate(ByVal DMMID As Integer, ByVal BuyerID As Integer, ByVal DepartmentId As Int16, ByVal ClassId As Int16, _
                                                         ByVal VendorID As Integer, ByVal VendorStyleNum As List(Of String), ByVal StartShipDate As Date, ByVal IncludeOnlyApprovedItems As Boolean) As IList(Of EcommFabOrigInfo)
        Dim VendorIdsString As String = String.Empty
        Dim VendorStylesString As String = String.Empty
        Dim DB2Results As IList(Of EcommFabOrigInfo)
        Try
            Select Case VendorStyleNum.Count
                Case Is > 0
                    VendorStylesString = RTrim("|" & String.Join("|", VendorStyleNum) & "|".ToString())
                Case Else
                    VendorStylesString = String.Empty
            End Select

            DB2Results = dalDB2.GetAllEcommFabOrigResultsByPOShipDate(DMMID, BuyerID, DepartmentId, ClassId, VendorID, VendorStylesString, StartShipDate, IncludeOnlyApprovedItems).ToList

            Dim eCommResults = (From results In DB2Results
                                Group By results.ISN
                                Into fabOrigResults = Group
                                Select fabOrigResults.OrderByDescending(Function(x) x.ISN).ThenByDescending(Function(x) x.UPC).First()).Take(700)

            UpdateSampleAvailableAndActiveOnWebStatus(eCommResults)

            Return eCommResults.OrderByDescending(Function(a) a.ISN).Cast(Of EcommFabOrigInfo).ToList()
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function GetAllEcommFabOrigResultsByHeirarchy(ByVal StatusCodes As List(Of String), ByVal DepartmenttId As Int16, ByVal ClassId As Int16, _
                                                             ByVal SubClassId As Int16, ByVal VendorID As Integer, ByVal VendorStyleNum As List(Of String), ByVal ACode1 As String, _
                                                             ByVal ACode2 As String, ByVal ACode3 As String, ByVal ACode4 As String, ByVal SellYear As Int16, ByVal SeasonId As Integer, _
                                                             ByVal CreatedSince As Date?, ByVal TurnInFilter As TUFilter) As List(Of EcommFabOrigInfo)
        Dim VendorIdsString As String = String.Empty
        Dim VendorStylesString As String = String.Empty
        Dim DB2Results As IList(Of EcommFabOrigInfo)
        Try
            Select Case VendorStyleNum.Count
                Case Is > 0
                    VendorStylesString = RTrim("|" & String.Join("|", VendorStyleNum) & "|".ToString())
                Case Else
                    VendorStylesString = String.Empty
            End Select

            DB2Results = dalDB2.GetAllFabOrigResultsByHierarchy(String.Join("|", StatusCodes), DepartmenttId, ClassId, SubClassId, VendorID, VendorStylesString, ACode1, ACode2, ACode3, ACode4, SellYear, SeasonId, CreatedSince).ToList

            Dim eCommResults = (From results In DB2Results
                    Group By results.ISN
                    Into fabOrigResults = Group
                    Select fabOrigResults.OrderByDescending(Function(x) x.ISN).ThenByDescending(Function(x) x.UPC).First()).Take(700)

            ' Sets the "Available For Turn-in" and "Active on Web" value for each sample
            UpdateSampleAvailableAndActiveOnWebStatus(eCommResults)

            Return ApplyTurnInFilters(eCommResults, TurnInFilter)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetAllEcommFabOrigResultsByISNs(ByVal ISNs As List(Of String), ByVal Upc As String, ByVal ReserveISNs As List(Of String), ByVal TurnInFilter As TUFilter) As IList(Of EcommFabOrigInfo)
        Try
            Dim DB2Results As IList(Of EcommFabOrigInfo) = dalDB2.GetAllEcommFabOrigResultsByISNs(String.Join(",", ISNs), Upc, String.Join(",", ReserveISNs)).ToList

            Dim eCommResults = (From results In DB2Results
                                Group By results.ISN
                                Into fabOrigResults = Group
                                Select fabOrigResults.OrderByDescending(Function(x) x.ISN).ThenByDescending(Function(x) x.UPC).First()).Take(700)

            UpdateSampleAvailableAndActiveOnWebStatus(eCommResults)

            Return ApplyTurnInFilters(eCommResults, TurnInFilter)

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetISNExists(ByVal ISN As Decimal, ByVal IsReserve As Boolean) As Boolean
        Try
            Return dalDB2.GetISNExists(ISN, IsReserve)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetUPCSKUExists(ByVal UPC As Decimal) As Boolean
        Try
            Return dalDB2.GetUPCSKUExists(UPC)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub UpdateFabOrigByISN(ByVal ISN As Decimal, LabelId As Integer, ByVal Fabrication As String, ByVal Origination As String, FabSrce As String, OrigSrce As String, ByVal UserId As String)
        Try
            dalDB2.UpdateFabOrigByISN(ISN, LabelId, Fabrication, Origination, FabSrce, OrigSrce, UserId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Applies the Turn In Filters in the ecommReulsts collection and returns the records which satisfy the search filters
    ''' </summary>
    ''' <param name="eCommResults">Collection of eComm results</param>
    ''' <param name="turnInFilters">Turn in filters selected by user</param>
    ''' <returns>eComm results which satisfy the search filters</returns>
    ''' <remarks></remarks>
    Private Function ApplyTurnInFilters(ByVal eCommResults As IEnumerable(Of EcommFabOrigInfo), ByVal turnInFilters As TUFilter) As List(Of EcommFabOrigInfo)
        Dim turnInSearchResults = (From eCommResult In eCommResults
                            Where (Not turnInFilters.NotTurnedIn OrElse eCommResult.IsTurnedInEcomm = "N")
                            Where IIf(turnInFilters.AvailableForTurnIn AndAlso turnInFilters.NotAvailableForTurnIn, True, _
                                      IIf(turnInFilters.NotAvailableForTurnIn, eCommResult.AvailableForTurnIn = "N", _
                                          IIf(turnInFilters.AvailableForTurnIn, eCommResult.AvailableForTurnIn = "Y", True)))
                            Where IIf(turnInFilters.ActiveOnWeb AndAlso turnInFilters.NotActiveOnWeb, True, _
                                      IIf(turnInFilters.NotActiveOnWeb, eCommResult.ActiveOnWeb = "N", _
                                          IIf(turnInFilters.ActiveOnWeb, eCommResult.ActiveOnWeb = "Y", True)))
                            Select eCommResult).ToList()

        Return turnInSearchResults.Cast(Of EcommFabOrigInfo).ToList()
    End Function

    ''' <summary>
    ''' Sets the "Available For Turn-in" and "Active on Web" value for each sample in the eCommSamples collection
    ''' </summary>
    ''' <param name="eCommSamples">Collection of Samples</param>
    ''' <remarks>
    ''' **Available for Turn-in**
    ''' Available for Turn in should be "Y" if it meets the following conditions
    ''' Sample Approval Flag equals to "Y" and Sample status description is not empty 
    ''' and sample status description not in "REQUESTED", "DISPOSED" and "RETURNED".
    ''' Available for Turn in should be "N" in all other cases
    ''' **Active on Web**
    ''' Active on Web should be "Y" if it meets the following conditions
    ''' Active UPC Flag equals to "Y" and active flag is not empty and active flag equals to "A"
    ''' and color code not equals to zero.
    ''' Active on Web should be "N" in all other cases
    ''' </remarks>
    Private Sub UpdateSampleAvailableAndActiveOnWebStatus(ByRef eCommSamples As IEnumerable(Of EcommFabOrigInfo))
        Dim sampleStatusDescription() As String = {"REQUESTED", "DISPOSED", "RETURNED"}
        For Each eCommSample As EcommFabOrigInfo In eCommSamples
            With eCommSample
                ' Available for Turn-in
                If .SampleDetails.SampleApprovalFlag.ToString().Trim().Equals("Y") AndAlso (Not String.IsNullOrEmpty(.SampleDetails.SampleStatusDesc)) _
                    AndAlso Array.IndexOf(sampleStatusDescription, .SampleDetails.SampleStatusDesc.ToUpper()) = -1 Then
                    .AvailableForTurnIn = "Y"
                Else
                    .AvailableForTurnIn = "N"
                End If

                'Active on Web
                If .ActiveUPCFlag.Equals("Y") AndAlso Not String.IsNullOrEmpty(.ActiveFlag) AndAlso .ActiveFlag.Equals("A") Then
                    .ActiveOnWeb = "Y"
                Else
                    .ActiveOnWeb = "N"
                End If
            End With
        Next
    End Sub

End Class
