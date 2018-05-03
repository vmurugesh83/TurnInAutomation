Module Module1

    Sub Main()
        Dim returnAds As New List(Of AdUpdates.Ad)
        Dim adUpdatesClient As New AdUpdatesClient.AdUpdates.AdClient()
        returnAds = adUpdatesClient.GetAdChangeInfo(93).ToList()

        Console.ReadLine()
    End Sub

End Module
