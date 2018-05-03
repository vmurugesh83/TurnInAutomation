Imports PathToSiteReports.Common.Common
Public Class SKUDetail

    Public Property GMM As String

    Public Property DMM As String

    Public Property Buyer As String

    Public Property FOB As String

    Public Property Dept As String

    Public Property Dept_Class As String

    Public Property ISN As Decimal

    Public Property DeptID As Integer

    Public Property VendorID As Integer

    Public Property ClassID As Integer

    Public Property Color As Integer

    Public Property SKU As Decimal

    Public Property UPC As Decimal

    Public Property AdNumber As Integer

    Public Property AdDesc As String

    Public Property AdType As String

    Public Property SSSetupDate As Date

    Public Property SampleRequestDate As Date

    Public Property SampleReceiptDate As Date

    Public Property SampleDueDate As Date

    Public Property SampleStatusDate As Date

    Public Property SampleStatusDesc As String

    Public Property POApprovalDate As Date

    Public Property POShipDate As Date

    Public Property TurnInDate As Date

    Public Property ImageShotDate As Date

    Public Property FinalImageReadyDate As Date

    Public Property POReceiptDate As Date

    Public Property CopyReadyDate As Date

    Public Property ProductReadyDate As Date

    Public Property ProductActiveDate As Date

    Public Property ImageID As Integer

    Public Property Quantity As Integer

    Public Property OwnedPrice As Decimal

    Public Property ReportItemType As String

    Public Property RemoveMerchandiseFlag As String

    Public Property AdJobStepDueDate As Date

    Public Property SamplePrimaryLocationName As String

    Public Property ExceptionType As String

    Public Property SKUActiveDate As Date

    Public ReadOnly Property SKUCost As String
        Get
            Return Quantity * OwnedPrice
        End Get
    End Property

    Public ReadOnly Property SSSetupToSampleRequest As Long
        Get
            Return GetDateDiff("d", Me.SSSetupDate, Me.SampleRequestDate, FirstDayOfWeek.Sunday)
        End Get
    End Property
    Public ReadOnly Property SampleRequestToSampleReceipt As Long
        Get
            Return GetDateDiff("d", Me.SampleRequestDate, Me.SampleReceiptDate, FirstDayOfWeek.Sunday)
        End Get
    End Property
    Public ReadOnly Property SSSetupToPOApproval As Long
        Get
            Return GetDateDiff("d", Me.SSSetupDate, Me.POApprovalDate, FirstDayOfWeek.Sunday)
        End Get
    End Property

    Public ReadOnly Property SSSetupToTurnIn As Long
        Get
            Return GetDateDiff("d", Me.SSSetupDate, Me.TurnInDate, FirstDayOfWeek.Sunday)
        End Get
    End Property
    Public ReadOnly Property POApprovalToTurnIn As Long
        Get
            Return GetDateDiff("d", Me.POApprovalDate, Me.TurnInDate, FirstDayOfWeek.Sunday)
        End Get
    End Property
    Public ReadOnly Property SampleReceiptToTurnIn As Long
        Get
            Return GetDateDiff("d", Me.SampleReceiptDate, Me.TurnInDate, FirstDayOfWeek.Sunday)
        End Get
    End Property
    Public ReadOnly Property TurnInToPOReceipt As Long
        Get
            Return GetDateDiff("d", Me.POReceiptDate, Me.TurnInDate, FirstDayOfWeek.Sunday)
        End Get
    End Property
    Public ReadOnly Property TurnInToProductReady As Long
        Get
            Return GetDateDiff("d", Me.TurnInDate, Me.ProductReadyDate, FirstDayOfWeek.Sunday)
        End Get
    End Property
    Public ReadOnly Property TurnInToCopyReady As Long
        Get
            Return GetDateDiff("d", Me.TurnInDate, Me.CopyReadyDate, FirstDayOfWeek.Sunday)
        End Get
    End Property
    Public ReadOnly Property TurnInToProductActive As Long
        Get
            Return GetDateDiff("d", Me.TurnInDate, Me.ProductActiveDate, FirstDayOfWeek.Sunday)
        End Get
    End Property
    Public ReadOnly Property SSSetupToPOReceipt As Long
        Get
            Return GetDateDiff("d", Me.SSSetupDate, Me.POReceiptDate, FirstDayOfWeek.Sunday)
        End Get
    End Property
    Public ReadOnly Property SampleRequestToPOReceipt As Long
        Get
            Return GetDateDiff("d", Me.SampleRequestDate, Me.POReceiptDate, FirstDayOfWeek.Sunday)
        End Get
    End Property
    Public ReadOnly Property POApprovalToPOReceipt As Long
        Get
            Return GetDateDiff("d", Me.POApprovalDate, Me.POReceiptDate, FirstDayOfWeek.Sunday)
        End Get
    End Property
    Public ReadOnly Property SampleReceiptToPOReceipt As Long
        Get
            Return GetDateDiff("d", Me.SampleReceiptDate, Me.POReceiptDate, FirstDayOfWeek.Sunday)
        End Get
    End Property
    Public ReadOnly Property ImageShotToPOReceipt As Long
        Get
            Return GetDateDiff("d", Me.ImageShotDate, Me.POReceiptDate, FirstDayOfWeek.Sunday)
        End Get
    End Property
    Public ReadOnly Property ImageReadyToPOReceipt As Long
        Get
            Return GetDateDiff("d", Me.POReceiptDate, Me.ImageShotDate, FirstDayOfWeek.Sunday)
        End Get
    End Property
    Public ReadOnly Property CopyReadyToPOReceipt As Long
        Get
            Return GetDateDiff("d", Me.POReceiptDate, Me.CopyReadyDate, FirstDayOfWeek.Sunday)
        End Get
    End Property
    Public ReadOnly Property ProductReadyToPOReceipt As Long
        Get
            Return GetDateDiff("d", Me.POReceiptDate, Me.ProductReadyDate, FirstDayOfWeek.Sunday)
        End Get
    End Property
    Public ReadOnly Property TurnInToImageShot As Long
        Get
            Return GetDateDiff("d", Me.ImageShotDate, Me.TurnInDate, FirstDayOfWeek.Sunday)
        End Get
    End Property
    Public ReadOnly Property ImageShotToImageReady As Long
        Get
            Return GetDateDiff("d", Me.ImageShotDate, Me.FinalImageReadyDate, FirstDayOfWeek.Sunday)
        End Get
    End Property
    Public ReadOnly Property TurnInToImageReady As Long
        Get
            Return GetDateDiff("d", Me.TurnInDate, Me.FinalImageReadyDate, FirstDayOfWeek.Sunday)
        End Get
    End Property
    Public ReadOnly Property ImageReadyToCopyReady As Long
        Get
            Return GetDateDiff("d", Me.FinalImageReadyDate, Me.CopyReadyDate, FirstDayOfWeek.Sunday)
        End Get
    End Property
    Public ReadOnly Property FirstReceiptToProductReady As Long
        Get
            Return GetDateDiff("d", Me.FinalImageReadyDate, Me.ProductReadyDate, FirstDayOfWeek.Sunday)
        End Get
    End Property

End Class
