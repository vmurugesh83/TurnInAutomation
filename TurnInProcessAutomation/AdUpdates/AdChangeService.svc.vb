Imports System.Data.SqlClient
' NOTE: You can use the "Rename" command on the context menu to change the class name "Service1" in code, svc and config file together.
Public Class AdChange
    Implements IAd

    'Dim log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim log As log4net.ILog = log4net.LogManager.GetLogger(GetType(InvalidProgramException))
    Private _logPath As String = ConfigurationManager.AppSettings("LogPath")

    Public Sub New()
    End Sub

    Public Function GetAdChangeInfo(ByVal adNumber As Integer) As List(Of Ad) Implements IAd.GetAdChangeInfo
        log4net.Config.XmlConfigurator.Configure()
        log.Info(DateTime.Now.ToString() & "------------ Start ------------")
        log.Info(DateTime.Now.ToString() & " - Call for Ad : " & adNumber.ToString)
        Dim returnAds As New List(Of Ad)

        Try
            returnAds = GetAllTallyChanges(adNumber)
        Catch ex As Exception
            log.Error(DateTime.Now.ToString() & " - ERROR : " & ex.Message.ToString)
            log.Error(DateTime.Now.ToString() & " - StackTrace : " & ex.StackTrace.ToString)
        End Try

        log.Info(DateTime.Now.ToString() & "------------ End ------------")

        Return returnAds

    End Function

    Dim processedAdNumber As Integer
    Dim processedPageNumber As Integer
    Dim processedShotNumber As Integer
    Dim processedImageNumber As Integer
    Dim processedMerchNumber As Integer

    Private Sub CreateAd(ByRef a As Ad, ByVal adData As AdminFullData)
        'If IsNothing(a) Then
        Dim number As Integer

        a = New Ad With
             {
                .AdNumber = adData.AdNumber,
                .AdDescription = adData.AdDescription,
                .AdStart = adData.AdStart,
                .AdEnd = adData.AdEnd,
                .AdStatus = adData.AdStatus.Value,
                .AdVersion = If(adData.AdVersion, String.Empty),
                .AssocFirst = adData.AssocFirst,
                .AssocLast = adData.AssocLast,
                .AssocId = IIf(Integer.TryParse(adData.AssocId.Trim(), number), number, 0),
                .AssocPhone = adData.AssocPhone,
                .EventName = adData.EventName,
                .EventStart = adData.EventStart,
                .EventEnd = adData.EventEnd,
                .MediaDescription = adData.MediaDescription,
                .MediaType = adData.MediaType,
                .PhotoStart = adData.PhotoStart,
                .PhotoEnd = adData.PhotoEnd,
                .TurninDate = adData.TurnInDate
            }
        'End If
    End Sub
    Public Function GetAllTallyChanges(ByVal adNumber As Integer) As List(Of Ad)
        'Get all changes for the Ad from the Tally Table
        Dim workhorseAdminDao As New WorkhorseAdminDao()
        Dim workhorseChanges As List(Of WorkhorseAdminChange) = workhorseAdminDao.GetChangesForAd(adNumber)

        log.Info(DateTime.Now.ToString() & " - Tally Changes Found  : " & workhorseChanges.Count.ToString)

        'Get all Admin Data from the database
        Dim rawAdminData As List(Of AdminFullData)
        rawAdminData = workhorseAdminDao.GetFullDataForAd(adNumber)

        Dim returnAds As New List(Of Ad)

        If rawAdminData.Count = 0 Then
            log.Info(DateTime.Now.ToString() & " - Ad Information not returned.")
            Return returnAds
        End If

        '----------------------------------------------------------------------
        '                       AD RELATED INFORMATION
        '----------------------------------------------------------------------
        '  Create the base Ad Record for the Ad requested
        Dim BaseAD As New Ad
        Dim adRecord As AdminFullData = rawAdminData.First(Function(x) x.AdStatus.HasValue)
        If Not adRecord Is Nothing Then
            CreateAd(BaseAD, adRecord)
        End If

        If Not BaseAD Is Nothing Then ' -- We have Ad Information for the Ad that changed.
            For Each change In workhorseChanges

                processedAdNumber = change.wh_ad_nbr
                processedPageNumber = IIf(change.wh_sys_pg_nbr.HasValue, change.wh_sys_pg_nbr, -1)
                processedShotNumber = IIf(change.wh_shot_nbr.HasValue, change.wh_shot_nbr, -1)
                processedImageNumber = IIf(change.wh_img_id.HasValue, change.wh_img_id, -1)
                processedMerchNumber = IIf(change.wh_merch_id.HasValue, change.wh_merch_id, -1)

                Dim AuditString As String = "ad:{0};pg:{1};sh:{2};im:{3};mrch:{4} :: act:{5}"
                log.Info(DateTime.Now.ToString() & " - " & String.Format(AuditString,
                processedAdNumber, _
                processedPageNumber, _
                processedShotNumber, _
                processedImageNumber, _
                processedMerchNumber, _
                change.WHaction))

                '----------------------------------------------------------------------
                '                       PAGE RELATED INFORMATION
                '----------------------------------------------------------------------
                Dim processedAd As New Ad
                Dim AddAd As Boolean = False
                Dim AddPage As Boolean = False
                Dim AddImage As Boolean = False
                Dim AddShot As Boolean = False
                Dim AddMerch As Boolean = False

                ' -- Ad Number and Page Number combine to form JobId at Workhorse.
                If returnAds.Exists(Function(x) x.AdNumber = processedAdNumber And x.Page.PageNumber = processedPageNumber) Then
                    processedAd = returnAds.Find(Function(x) x.AdNumber = processedAdNumber And x.Page.PageNumber = processedPageNumber)
                Else
                    CreateAd(processedAd, adRecord)
                    AddAd = True
                End If

                ' When assigning Image Number, these are passed in as 0, with only the merchandise ID.
                ' If we include this in the loop, the page number is replaced by Page 1 on WH. 
                If processedPageNumber = 0 And processedImageNumber = 0 And processedShotNumber = 0 Then
                    Continue For
                End If

                ' Page does not exist in ad
                ' Has a page number to be processed
                ' Page details exist on the rawAdminData
                If processedAd.Page Is Nothing _
                    AndAlso processedPageNumber > 0 _
                    AndAlso rawAdminData.Exists(Function(x) x.PageNumber = processedPageNumber) _
                Then
                    Dim pageRecord As AdminFullData = rawAdminData.First(Function(x) x.PageNumber = processedPageNumber)
                    Select Case change.WHaction.ToUpper
                        Case "PAGEDELETECAT"
                            pageRecord.PageActiveIndicator = "K"
                        Case Else
                            pageRecord.PageActiveIndicator = "A"
                    End Select

                    CreatePage(processedAd.Page, pageRecord)
                    'Else
                    '    Continue For
                End If


                ' removed based on discussion with Param and Milan because the merch is just routed.
                'If change.WHaction.ToUpper = "MOVEDMERCH" Then
                '    KillMerch(change) 'change.old_ad_nbr, change.old_sys_pg_nbr, change.old_shot_nbr, change.old_img_id, change.old_merch_id))
                'End If

                Dim shotGroup As New ShotGroup
                If Not IsNothing(processedAd.Page) AndAlso processedAd.Page.ShotGroup.Exists(Function(x) x.ShotNumber = processedShotNumber) Then
                    shotGroup = processedAd.Page.ShotGroup.Find(Function(x) x.ShotNumber = processedShotNumber)
                Else
                    If processedShotNumber <> -1 Then
                        CreateShotGroup(processedAd.Page, shotGroup)
                        AddShot = True
                    Else
                        Continue For
                    End If

                End If

                Dim imageRecord As AdminFullData = rawAdminData.FirstOrDefault(Function(x) x.ImageNumber = processedImageNumber And x.ImageActiveIndicator.HasValue)
                Dim image As New Image
                If Not IsNothing(shotGroup.Images) AndAlso shotGroup.Images.Exists(Function(x) x.ImageNumber = processedImageNumber) Then
                    image = shotGroup.Images.Find(Function(x) x.ImageNumber = processedImageNumber)

                    Select Case change.WHaction.ToUpper
                        Case "IMAGEDELETE"
                            image.ActiveIndicator = ActiveIndicatorType.K
                        Case Else
                            image.ActiveIndicator = ActiveIndicatorType.A
                    End Select

                Else
                    If processedImageNumber <> -1 Then
                        CreateImage(image, change, imageRecord)
                        AddImage = True
                    Else
                        Continue For
                    End If
                End If

                Dim merch As New MerchForImage

                If Not IsNothing(image.Samples) AndAlso image.Samples.Exists(Function(x) x.MerchID = processedMerchNumber) Then

                    merch = image.Samples.Find(Function(x) x.MerchID = processedMerchNumber)

                    Select Case change.WHaction.ToUpper
                        Case "MERCHUNASSIGN"
                            If change.Action.ToUpper = "MERCHKILLED" Then
                                merch.ActiveIndicator = ActiveIndicatorType.K
                            End If
                        Case "MERCHDELETE"
                            merch.ActiveIndicator = ActiveIndicatorType.K
                    End Select
                Else
                    If processedMerchNumber <> -1 AndAlso rawAdminData.Exists(Function(x) x.MerchId = processedMerchNumber And x.MerchActiveIndicator.HasValue) Then
                        Dim merchRecord As AdminFullData = rawAdminData.First(Function(x) x.MerchId = processedMerchNumber And x.MerchActiveIndicator.HasValue)
                        'Dim merchRecord As New AdminFullData
                        CreateMerchandise(merch, change, merchRecord)
                        image.Samples.Add(merch)
                    End If
                End If


                If AddImage Then
                    shotGroup.Images.Add(image)
                End If

                If AddShot Then
                    processedAd.Page.ShotGroup.Add(shotGroup)
                End If

                If AddAd Or AddPage Then
                    returnAds.Add(processedAd)
                End If
            Next

        End If


        If returnAds Is Nothing Then
            log.Info(DateTime.Now.ToString() & " - 0 ads returned.")
        Else
            If returnAds.Count = 0 Then
                returnAds.Add(BaseAD)
            End If

            log.Info(DateTime.Now.ToString() & " - returns : " & returnAds.Count.ToString)
        End If

        Return returnAds

    End Function

    Private Sub CreateShotGroup(ByRef p As Page, ByRef shotGroup As ShotGroup)
        '----------------------------------------------------------------------
        '                       SHOT RELATED INFORMATION
        '----------------------------------------------------------------------


        'If p.ShotGroup.Exists(Function(x) x.ShotNumber = processedShotNumber) Then
        ' shotGroup = p.ShotGroup.Find(Function(x) x.ShotNumber = processedShotNumber)
        'Else
        'Get the shotgroup and add to the page.
        shotGroup = New ShotGroup With {.ShotNumber = processedShotNumber, .Images = New List(Of Image)}
        'p.ShotGroup.Add(shotGroup)
        'End If
    End Sub
    Private Sub CreateImage(ByRef image As Image, ByVal change As WorkhorseAdminChange, ByVal imageRecord As AdminFullData)
        '----------------------------------------------------------------------
        '                       IMAGE RELATED INFORMATION
        '----------------------------------------------------------------------
        If processedImageNumber <> -1 Then

            If change.Action.ToUpper = "IMAGEDELET" Then '- spelt without an E
                image = New Image With {.ImageNumber = processedImageNumber, .ActiveIndicator = ActiveIndicatorType.K, .Samples = New List(Of MerchForImage)}
            Else
                'Dim imageRecord As AdminFullData = rawAdminData.SingleOrDefault(Function(x) x.ImageNumber = processedImageNumber And x.ImageActiveIndicator.HasValue)
                If Not IsNothing(imageRecord) Then
                    image = New Image With
               {
                   .ImageNumber = imageRecord.ImageNumber,
                   .Description = imageRecord.ImageDescription,
                   .ImageClass = imageRecord.ImageClass,
                   .ImageNotes = imageRecord.ImageNotes,
                   .ImageSource = imageRecord.ImageSource,
                   .ImageSuffixType = imageRecord.ImageSuffixType,
                   .MediaType = imageRecord.ImageMediaType,
                   .ActiveIndicator = ActiveIndicatorType.A,
                   .Samples = New List(Of MerchForImage)
               }
                Else
                    image = New Image With {.ImageNumber = processedImageNumber, .ActiveIndicator = ActiveIndicatorType.A, .Samples = New List(Of MerchForImage)}
                End If

            End If
        Else
            ' for Merch which has not been assigned to an image
            image = New Image With {.ImageNumber = processedImageNumber, .ActiveIndicator = ActiveIndicatorType.A, .Samples = New List(Of MerchForImage)}
        End If
    End Sub
    Private Sub CreateMerchandise(ByRef merch As MerchForImage, ByVal change As WorkhorseAdminChange, ByVal MerchRecord As AdminFullData)
        '----------------------------------------------------------------------
        '                       MERCHANDISE RELATED INFORMATION
        '----------------------------------------------------------------------
        If processedMerchNumber <> -1 Then

            ' --- Handle the moved merch functionality first
            ' Delete from old ad, ad to the new

            'If change.WHaction.ToUpper = "MOVEDMERCH" Then
            '    KillMerch(change) 'change.old_ad_nbr, change.old_sys_pg_nbr, change.old_shot_nbr, change.old_img_id, change.old_merch_id))
            'End If


            ' --- Get the merchandise to work on ----

            'merch = New MerchForImage With {.MerchID = change.wh_ad_nbr, .ActiveIndicator = ActiveIndicatorType.A, .StylingNotes = "WH Upgrade", .MerchGroup = -1}
            If MerchRecord.MerchActiveIndicator.Value = "K" Then
                merch = New MerchForImage With {.MerchID = MerchRecord.MerchId, .ActiveIndicator = ActiveIndicatorType.K, .StylingNotes = MerchRecord.StylingNotes}
            Else
                merch = New MerchForImage With {.MerchID = MerchRecord.MerchId, .ActiveIndicator = ActiveIndicatorType.A, .StylingNotes = MerchRecord.StylingNotes}
            End If


            Select Case change.WHaction.ToUpper
                Case "MERCHUNASSIGN"
                    If change.Action.ToUpper = "MERCHKILLED" Then
                        merch.ActiveIndicator = ActiveIndicatorType.K
                    End If
                Case "MERCHDELETE"
                    merch.ActiveIndicator = ActiveIndicatorType.K
            End Select


        End If

    End Sub

    Private Sub CreatePage(ByRef p As Page, ByVal pData As AdminFullData)
        p = New Page With
        {
            .AdNumber = pData.AdNumber,
            .PageNumber = pData.PageNumber,
            .PageDescription = pData.PageDescription,
            .CoverPage = pData.CoverPage,
            .ActiveIndicator = IIf(pData.PageActiveIndicator = "K", ActiveIndicatorType.K, ActiveIndicatorType.A),
            .ShotGroup = New List(Of ShotGroup)
        }
    End Sub
    Public Enum WorkhorseChange
        Image
        Page
        Ad
    End Enum

    'Private Function MovedTo(ByVal Change As WorkhorseAdminChange) As WorkhorseChange
    '    If Change.wh_img_id = Change.old_img_id Then
    '        Return WorkhorseChange.Image
    '    End If
    '    If Change.wh_sys_pg_nbr = Change.old_sys_pg_nbr Then
    '        Return WorkhorseChange.Page
    '    End If
    '    If Change.wh_ad_nbr = Change.old_ad_nbr Then
    '        Return WorkhorseChange.Ad
    '    End If
    '    Return WorkhorseChange.Ad
    'End Function

    'Dim KilledProcessChangeRecord As New WorkhorseAdminChange

    'Private Sub KillMerch(ByVal change As WorkhorseAdminChange) 'ByVal AdNum As Integer, ByVal PageNum As Integer, ByVal ShotNum As Integer, ByVal imageNum As Integer, ByVal MerchId As Integer) As Ad
    '    KilledProcessChangeRecord = change
    '    'log.Info(DateTime.Now.ToString() & " - Killing for Move Start")

    '    Dim kAdNumber As Integer = change.old_ad_nbr
    '    Dim kPageNumber As Integer = change.old_sys_pg_nbr
    '    Dim kShotNumber As Integer = IIf(change.old_shot_nbr.HasValue, change.wh_shot_nbr, -1)
    '    Dim kImageNumber As Integer = IIf(change.old_img_id.HasValue, change.wh_img_id, -1)
    '    Dim kMerchNumber As Integer = IIf(change.old_merch_id.HasValue, change.wh_merch_id, -1)

    '    Dim AuditString As String = "ad:{0};pg:{1};sh:{2};im:{3};mrch:{4} :: act:{5}"

    '    'log.Info(DateTime.Now.ToString() & " - " & String.Format(AuditString,
    '                                                                          kAdNumber, _
    '                                                                          kPageNumber, _
    '                                                                          kShotNumber, _
    '                                                                          kImageNumber, _
    '                                                                          kMerchNumber, _
    '                                                                          change.WHaction
    '                                                                          ))

    '    Dim AdExists As Boolean = KilledBecauseOfMoveMerchandise.Exists(Function(x) x.AdNumber = kAdNumber And x.Page.PageNumber = kPageNumber)

    '    Dim returnAd As New Ad
    '    If AdExists Then
    '        returnAd = KilledBecauseOfMoveMerchandise.Find(Function(x) x.AdNumber = kAdNumber And x.Page.PageNumber = kPageNumber)

    '        Dim ShotExists As Boolean = returnAd.Page.ShotGroup.Exists(Function(x) x.ShotNumber = kShotNumber)
    '        If ShotExists Then
    '            Dim shot As ShotGroup = returnAd.Page.ShotGroup.Find(Function(x) x.ShotNumber = kShotNumber)

    '            Dim image As New Image
    '            AddImageToShot(shot, image, kImageNumber, ActiveIndicatorType.K)
    '            AddMerchandiseToImage(image, kMerchNumber, ActiveIndicatorType.K)
    '        End If

    '    Else
    '        Dim rawKilledAdData As List(Of AdminFullData)
    '        Dim workhorseDAO As New WorkhorseAdminDao
    '        rawKilledAdData = workhorseDAO.GetFullDataForAd(kAdNumber)

    '        Dim adRecord As AdminFullData = rawKilledAdData.First(Function(x) x.AdStatus.HasValue)
    '        If Not adRecord Is Nothing Then
    '            CreateAd(returnAd, adRecord)
    '        End If

    '        AddPageToAd(returnAd, kPageNumber, ActiveIndicatorType.A)
    '        KilledBecauseOfMoveMerchandise.Add(returnAd)
    '    End If
    'End Sub

    'Private Sub AddPageToAd(ByRef a As Ad, ByVal pID As Integer, ByVal status As ActiveIndicatorType)
    '    If IsNothing(a.Page) Then
    '        a.Page = New Page With {.PageNumber = pID, .AdNumber = a.AdNumber, .ActiveIndicator = status, .ShotGroup = New List(Of ShotGroup)}
    '    End If
    '    'If Not IsNothing(KilledProcessChangeRecord.old_shot_nbr) Then
    '    Dim shotNum As Integer = IIf(IsNothing(KilledProcessChangeRecord.old_shot_nbr), -1, KilledProcessChangeRecord.old_shot_nbr)
    '    AddShotToPage(a.Page, shotNum, ActiveIndicatorType.A)
    '    'End If

    'End Sub

    'Private Sub AddShotToPage(ByRef p As Page, ByVal sID As Integer, ByVal status As ActiveIndicatorType)
    '    Dim pExists As Boolean = p.ShotGroup.Exists(Function(x) x.ShotNumber = sID)
    '    Dim s As New ShotGroup
    '    If pExists Then
    '        s = p.ShotGroup.Find(Function(x) x.ShotNumber)
    '    Else
    '        s = New ShotGroup With {.ShotNumber = sID, .Images = New List(Of Image)}
    '        Dim i As New Image
    '        Dim imageNum As Integer = IIf(IsNothing(KilledProcessChangeRecord.old_img_id), -1, KilledProcessChangeRecord.old_img_id)
    '        AddImageToShot(s, i, imageNum, ActiveIndicatorType.A)
    '        p.ShotGroup.Add(s)
    '    End If
    'End Sub

    'Private Sub AddImageToShot(ByRef s As ShotGroup, ByRef image As Image, ByVal iID As Integer, ByVal status As ActiveIndicatorType)
    '    Dim iExists As Boolean = s.Images.Exists(Function(x) x.ImageNumber = iID)
    '    If iExists Then
    '        image = s.Images.Find(Function(x) x.ImageNumber)
    '    Else
    '        image = New Image With {.ImageNumber = iID, .ActiveIndicator = status, .Samples = New List(Of MerchForImage)}
    '        AddMerchandiseToImage(image, KilledProcessChangeRecord.old_merch_id, ActiveIndicatorType.K)
    '        s.Images.Add(image)
    '    End If
    'End Sub

    'Private Sub AddMerchandiseToImage(ByRef i As Image, ByVal mID As Integer, ByVal status As ActiveIndicatorType)
    '    Dim mExists As Boolean = i.Samples.Exists(Function(x) x.MerchID = mID)
    '    Dim m As MerchForImage
    '    If mExists Then
    '        m = i.Samples.Find(Function(x) x.MerchID)
    '        m.ActiveIndicator = status
    '    Else
    '        m = New MerchForImage With {.MerchID = mID, .ActiveIndicator = status}
    '        i.Samples.Add(m)
    '    End If
    'End Sub

End Class
