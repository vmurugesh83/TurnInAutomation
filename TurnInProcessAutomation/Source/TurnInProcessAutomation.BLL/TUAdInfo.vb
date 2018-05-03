Imports TurnInProcessAutomation.SqlDAL
Imports TurnInProcessAutomation.BusinessEntities

Public Class TUAdInfo
    Private dal As SqlDAL.AdInfo = New SqlDAL.AdInfo
    Private dalDB2 As MainframeDAL.BatchDao = New MainframeDAL.BatchDao

    Public Function GetAllFromAdInfoFiltered(ByVal IsEcommerce As Boolean) As IList(Of AdInfoInfo)
        Try
            Return dal.GetAllFromAdInfoFiltered(IsEcommerce)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetTransferAds(ByVal IsEcommerce As Boolean) As IList(Of AdInfoInfo)
        Try
            Return dal.GetTransferAds(IsEcommerce)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetAdInfoByAdNbr(ByVal AdNbr As Integer) As AdInfoInfo
        Try
            Return dal.GetAdInfoByadnbr(AdNbr)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetAdsForMaintenance() As IList(Of AdInfoInfo)
        Return (From db2Ads In dalDB2.GetAdsForMaintenance Join sqlAds In dal.GetAllFromAdInfo(True) On sqlAds.adnbr Equals db2Ads.adnbr Select New AdInfoInfo With {.adnbr = sqlAds.adnbr, .addesc = sqlAds.addesc}).ToList
    End Function

    Public Function GetAdsForQueryTool() As IList(Of AdInfoInfo)
        Return (From db2Ads In dalDB2.GetAdsForMeetingOrQuery("") Join sqlAds In dal.GetAllFromAdInfo(True) On sqlAds.adnbr Equals db2Ads.adnbr Select New AdInfoInfo With {.adnbr = sqlAds.adnbr, .addesc = sqlAds.addesc, .TurnInDate = sqlAds.TurnInDate}).ToList
    End Function

    Public Function GetAdsForMeeting() As IList(Of AdInfoInfo)
        Return (From db2Ads In dalDB2.GetAdsForMeetingOrQuery("RDFM") Join sqlAds In dal.GetAllFromAdInfo(True) On sqlAds.adnbr Equals db2Ads.adnbr Select New AdInfoInfo With {.adnbr = sqlAds.adnbr, .addesc = sqlAds.addesc}).ToList
    End Function

    Public Function GetAdsForPrioritization() As IList(Of AdInfoInfo)
        Return (From db2Ads In dalDB2.GetAdsForMeetingOrQuery("RDFM") Join sqlAds In dal.GetAllFromAdInfo(True) On sqlAds.adnbr Equals db2Ads.adnbr Select New AdInfoInfo With {.adnbr = sqlAds.adnbr, .addesc = sqlAds.addesc}).ToList
    End Function
End Class
