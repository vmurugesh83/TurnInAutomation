Imports System.ComponentModel

<Serializable()> _
Public Class AdInfoInfo

    'Internal member variables
    Private _AdNbr As Integer
    Private _AdDesc As String
    Private _AdStatCd As String
    Private _MediaCd As String
    Private _MediaTypeCd As String
    Private _AdRunStartDt As DateTime
    Private _AdRunEndDt As DateTime
    Private _EventNbr As Integer
    Private _SaleNbr As Integer
    Private _FiscalYr As Integer
    Private _SeasonCd As String
    Private _FiscalMthNbr As String
    Private _PrdctnSrcCd As String
    Private _DistbnMethCd As String
    Private _AdSponTypeCd As String
    Private _AdSponNbr As Integer
    Private _FinclSponTypeCd As String
    Private _FinclSponNbr As Integer
    Private _CoopAmt As Decimal
    Private _CoopPct As Decimal
    Private _AdBdgtAmt As Decimal
    Private _LeaseFiscalYr As Integer
    Private _LeaseFiscalMth As String
    Private _AdPabPct As Decimal
    Private _ClsdDscnryInd As String
    Private _LeaseClsdInd As String
    Private _CoopDscnryInd As String
    Private _CoopAdjmntInd As String
    Private _MediaChrgdNbr As String
    Private _DateAdded As DateTime
    Private _MerchCriteriaMin As Decimal
    Private _MerchCriteriaMax As Decimal
    Private _MerchCriteriaInd As String
    Private _SendToC3 As String
    Private _VrsnToC3 As String
    Private _DateToAprimo As DateTime
    Private _DateToC3 As DateTime
    Private _AdDescToC3 As String
    Private _turnInDate As String

    'Calculated Values
    Private _MasterPages As Integer
    Private _BasePages As Integer

    'Default constructor
    Public Sub New()
    End Sub

    'Constructor with initialized values
    Public Sub New(ByVal AdNbr As Integer, _
                      ByVal AdDesc As String, _
                      ByVal AdStatCd As String, _
                      ByVal MediaCd As String, _
                      ByVal MediaTypeCd As String, _
                      ByVal AdRunStartDt As DateTime, _
                      ByVal AdRunEndDt As DateTime, _
                      ByVal EventNbr As Integer, _
                      ByVal SaleNbr As Integer, _
                      ByVal FiscalYr As Integer, _
                      ByVal SeasonCd As String, _
                      ByVal FiscalMthNbr As String, _
                      ByVal PrdctnSrcCd As String, _
                      ByVal DistbnMethCd As String, _
                      ByVal AdSponTypeCd As String, _
                      ByVal AdSponNbr As Integer, _
                      ByVal FinclSponTypeCd As String, _
                      ByVal FinclSponNbr As Integer, _
                      ByVal CoopAmt As Decimal, _
                      ByVal CoopPct As Decimal, _
                      ByVal AdBdgtAmt As Decimal, _
                      ByVal LeaseFiscalYr As Integer, _
                      ByVal LeaseFiscalMth As String, _
                      ByVal AdPabPct As Decimal, _
                      ByVal ClsdDscnryInd As String, _
                      ByVal LeaseClsdInd As String, _
                      ByVal CoopDscnryInd As String, _
                      ByVal CoopAdjmntInd As String, _
                      ByVal MediaChrgdNbr As String, _
                      ByVal DateAdded As DateTime, _
                      ByVal MerchCriteriaMin As Decimal, _
                      ByVal MerchCriteriaMax As Decimal, _
                      ByVal MerchCriteriaInd As String, _
                      ByVal SendToC3 As String, _
                      ByVal VrsnToC3 As String, _
                      ByVal DateToAprimo As DateTime, _
                      ByVal DateToC3 As DateTime, _
                      ByVal AdDescToC3 As String)
        Me._AdNbr = AdNbr
        Me._AdDesc = AdDesc
        Me._AdStatCd = AdStatCd
        Me._MediaCd = MediaCd
        Me._MediaTypeCd = MediaTypeCd
        Me._AdRunStartDt = AdRunStartDt
        Me._AdRunEndDt = AdRunEndDt
        Me._EventNbr = EventNbr
        Me._SaleNbr = SaleNbr
        Me._FiscalYr = FiscalYr
        Me._SeasonCd = SeasonCd
        Me._FiscalMthNbr = FiscalMthNbr
        Me._PrdctnSrcCd = PrdctnSrcCd
        Me._DistbnMethCd = DistbnMethCd
        Me._AdSponTypeCd = AdSponTypeCd
        Me._AdSponNbr = AdSponNbr
        Me._FinclSponTypeCd = FinclSponTypeCd
        Me._FinclSponNbr = FinclSponNbr
        Me._CoopAmt = CoopAmt
        Me._CoopPct = CoopPct
        Me._AdBdgtAmt = AdBdgtAmt
        Me._LeaseFiscalYr = LeaseFiscalYr
        Me._LeaseFiscalMth = LeaseFiscalMth
        Me._AdPabPct = AdPabPct
        Me._ClsdDscnryInd = ClsdDscnryInd
        Me._LeaseClsdInd = LeaseClsdInd
        Me._CoopDscnryInd = CoopDscnryInd
        Me._CoopAdjmntInd = CoopAdjmntInd
        Me._MediaChrgdNbr = MediaChrgdNbr
        Me._DateAdded = DateAdded
        Me._MerchCriteriaMin = MerchCriteriaMin
        Me._MerchCriteriaMax = MerchCriteriaMax
        Me._MerchCriteriaInd = MerchCriteriaInd
        Me._SendToC3 = SendToC3
        Me._VrsnToC3 = VrsnToC3
        Me._DateToAprimo = DateToAprimo
        Me._DateToC3 = DateToC3
        Me._AdDescToC3 = AdDescToC3
    End Sub

    <DataObjectField(True)> _
    Public Property adnbr() As Integer
        Get
            Return _AdNbr
        End Get
        Set(ByVal value As Integer)
            _AdNbr = value
        End Set
    End Property

    Public Property addesc() As String
        Get
            Return _AdDesc
        End Get
        Set(ByVal value As String)
            _AdDesc = value
        End Set
    End Property

    Public ReadOnly Property AdNumberDesc As String
        Get
            Return adnbr & " - " & addesc
        End Get
    End Property

    Public Property TurnInDate() As String
        Get
            Return _turnInDate
        End Get
        Set(ByVal value As String)
            _turnInDate = value
        End Set
    End Property

    Public Property adstatcd() As String
        Get
            Return _AdStatCd
        End Get
        Set(ByVal value As String)
            _AdStatCd = value
        End Set
    End Property

    Public Property mediacd() As String
        Get
            Return _MediaCd
        End Get
        Set(ByVal value As String)
            _MediaCd = value
        End Set
    End Property

    Public Property mediatypecd() As String
        Get
            Return _MediaTypeCd
        End Get
        Set(ByVal value As String)
            _MediaTypeCd = value
        End Set
    End Property

    Public Property adrunstartdt() As DateTime
        Get
            Return _AdRunStartDt
        End Get
        Set(ByVal value As DateTime)
            _AdRunStartDt = value
        End Set
    End Property

    Public Property adrunenddt() As DateTime
        Get
            Return _AdRunEndDt
        End Get
        Set(ByVal value As DateTime)
            _AdRunEndDt = value
        End Set
    End Property

    Public Property eventnbr() As Integer
        Get
            Return _EventNbr
        End Get
        Set(ByVal value As Integer)
            _EventNbr = value
        End Set
    End Property

    Public Property salenbr() As Integer
        Get
            Return _SaleNbr
        End Get
        Set(ByVal value As Integer)
            _SaleNbr = value
        End Set
    End Property

    Public Property fiscalyr() As Integer
        Get
            Return _FiscalYr
        End Get
        Set(ByVal value As Integer)
            _FiscalYr = value
        End Set
    End Property

    Public Property seasoncd() As String
        Get
            Return _SeasonCd
        End Get
        Set(ByVal value As String)
            _SeasonCd = value
        End Set
    End Property

    Public Property fiscalmthnbr() As String
        Get
            Return _FiscalMthNbr
        End Get
        Set(ByVal value As String)
            _FiscalMthNbr = value
        End Set
    End Property

    Public Property prdctnsrccd() As String
        Get
            Return _PrdctnSrcCd
        End Get
        Set(ByVal value As String)
            _PrdctnSrcCd = value
        End Set
    End Property

    Public Property distbnmethcd() As String
        Get
            Return _DistbnMethCd
        End Get
        Set(ByVal value As String)
            _DistbnMethCd = value
        End Set
    End Property

    Public Property adspontypecd() As String
        Get
            Return _AdSponTypeCd
        End Get
        Set(ByVal value As String)
            _AdSponTypeCd = value
        End Set
    End Property

    Public Property adsponnbr() As Integer
        Get
            Return _AdSponNbr
        End Get
        Set(ByVal value As Integer)
            _AdSponNbr = value
        End Set
    End Property

    Public Property finclspontypecd() As String
        Get
            Return _FinclSponTypeCd
        End Get
        Set(ByVal value As String)
            _FinclSponTypeCd = value
        End Set
    End Property

    Public Property finclsponnbr() As Integer
        Get
            Return _FinclSponNbr
        End Get
        Set(ByVal value As Integer)
            _FinclSponNbr = value
        End Set
    End Property

    Public Property coopamt() As Decimal
        Get
            Return _CoopAmt
        End Get
        Set(ByVal value As Decimal)
            _CoopAmt = value
        End Set
    End Property

    Public Property cooppct() As Decimal
        Get
            Return _CoopPct
        End Get
        Set(ByVal value As Decimal)
            _CoopPct = value
        End Set
    End Property

    Public Property adbdgtamt() As Decimal
        Get
            Return _AdBdgtAmt
        End Get
        Set(ByVal value As Decimal)
            _AdBdgtAmt = value
        End Set
    End Property

    Public Property leasefiscalyr() As Integer
        Get
            Return _LeaseFiscalYr
        End Get
        Set(ByVal value As Integer)
            _LeaseFiscalYr = value
        End Set
    End Property

    Public Property leasefiscalmth() As String
        Get
            Return _LeaseFiscalMth
        End Get
        Set(ByVal value As String)
            _LeaseFiscalMth = value
        End Set
    End Property

    Public Property adpabpct() As Decimal
        Get
            Return _AdPabPct
        End Get
        Set(ByVal value As Decimal)
            _AdPabPct = value
        End Set
    End Property

    Public Property clsddscnryind() As String
        Get
            Return _ClsdDscnryInd
        End Get
        Set(ByVal value As String)
            _ClsdDscnryInd = value
        End Set
    End Property

    Public Property leaseclsdind() As String
        Get
            Return _LeaseClsdInd
        End Get
        Set(ByVal value As String)
            _LeaseClsdInd = value
        End Set
    End Property

    Public Property coopdscnryind() As String
        Get
            Return _CoopDscnryInd
        End Get
        Set(ByVal value As String)
            _CoopDscnryInd = value
        End Set
    End Property

    Public Property coopadjmntind() As String
        Get
            Return _CoopAdjmntInd
        End Get
        Set(ByVal value As String)
            _CoopAdjmntInd = value
        End Set
    End Property

    Public Property mediachrgdnbr() As String
        Get
            Return _MediaChrgdNbr
        End Get
        Set(ByVal value As String)
            _MediaChrgdNbr = value
        End Set
    End Property

    Public Property dateadded() As DateTime
        Get
            Return _DateAdded
        End Get
        Set(ByVal value As DateTime)
            _DateAdded = value
        End Set
    End Property

    Public Property merchcriteriamin() As Decimal
        Get
            Return _MerchCriteriaMin
        End Get
        Set(ByVal value As Decimal)
            _MerchCriteriaMin = value
        End Set
    End Property

    Public Property merchcriteriamax() As Decimal
        Get
            Return _MerchCriteriaMax
        End Get
        Set(ByVal value As Decimal)
            _MerchCriteriaMax = value
        End Set
    End Property

    Public Property merchcriteriaind() As String
        Get
            Return _MerchCriteriaInd
        End Get
        Set(ByVal value As String)
            _MerchCriteriaInd = value
        End Set
    End Property

    Public Property sendtoc3() As String
        Get
            Return _SendToC3
        End Get
        Set(ByVal value As String)
            _SendToC3 = value
        End Set
    End Property

    Public Property vrsntoc3() As String
        Get
            Return _VrsnToC3
        End Get
        Set(ByVal value As String)
            _VrsnToC3 = value
        End Set
    End Property

    Public Property datetoaprimo() As DateTime
        Get
            Return _DateToAprimo
        End Get
        Set(ByVal value As DateTime)
            _DateToAprimo = value
        End Set
    End Property

    Public Property datetoc3() As DateTime
        Get
            Return _DateToC3
        End Get
        Set(ByVal value As DateTime)
            _DateToC3 = value
        End Set
    End Property

    Public Property addesctoc3() As String
        Get
            Return _AdDescToC3
        End Get
        Set(ByVal value As String)
            _AdDescToC3 = value
        End Set
    End Property

    Public ReadOnly Property TurnInWeek() As String
        Get
            Return _turnInDate.Trim
        End Get
    End Property

    Public Property masterpages() As Integer
        Get
            Return _MasterPages
        End Get
        Set(ByVal value As Integer)
            _MasterPages = value
        End Set
    End Property

    Public Property basepages() As Integer
        Get
            Return _BasePages
        End Get
        Set(ByVal value As Integer)
            _BasePages = value
        End Set
    End Property

End Class

