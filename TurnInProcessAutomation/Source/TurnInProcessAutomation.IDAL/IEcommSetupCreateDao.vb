Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.OleDb

Public Interface IEcommSetupCreateDao

    Function GetAllEcommSetupCreateResultsByHierarchy(ByVal StatusCodes As String, ByVal DeptId As Int16, ByVal ClassId As Int16, ByVal SubClassId As Int16, ByVal VendorId As Integer, ByVal VendorStyleNum As String, ByVal ACode As String, ByVal SellYear As Int16, ByVal SeasonId As Integer, ByVal CreatedSince As Date?) As IList(Of EcommSetupCreateInfo)

    Function GetAllEcommSetupCreateResultsByISNs(ByVal ISNs As String, ByVal ReserveISNs As String) As IList(Of EcommSetupCreateInfo)

    Function GetAllEcommSetupCreateDetail(ByVal ISN As Decimal) As IList(Of EcommSetupCreateInfo)

    Function GetISNExists(ByVal ISN As Decimal, ByVal IsReserve As Boolean) As Boolean

End Interface