Imports TurnInProcessAutomation.BusinessEntities
Public Class TUMerchandiseSample


    Public Shared Function GenerateNewMerchandiseSampleID(ByRef TUmerch As EcommSetupCreateInfo) As String
        Dim WHmerch As New MerchandiseSample
        WHmerch.TurninMerchandiseID = TUmerch.TurnInMerchId
        WHmerch.ISN = TUmerch.ISN
        WHmerch.ISNDescription = TUmerch.ISNDesc
        WHmerch.ColorCode = TUmerch.ColorCode
        WHmerch.ColorDesc = TUmerch.ColorDesc

        SendToNERVE(WHmerch)

        Return WHmerch.TurninMerchandiseID
    End Function

    Private Shared Sub SendToNERVE(ByVal WHMerch As MerchandiseSample)

    End Sub
    Public Sub New()

    End Sub
End Class
