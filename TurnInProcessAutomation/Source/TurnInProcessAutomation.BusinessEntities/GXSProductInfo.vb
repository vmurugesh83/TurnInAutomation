
Public Class GXSProductInfo

    'Column 1
    Private _ProductVendorStyle As String
    Public Property ProductVendorStyle As String
        Get
            Return _ProductVendorStyle
        End Get
        Set(ByVal value As String)
            _ProductVendorStyle = value
        End Set
    End Property

    Private _FullProductName As String
    Public Property FullProductName As String
        Get
            Return _FullProductName
        End Get
        Set(ByVal value As String)
            _FullProductName = value
        End Set
    End Property

    Private _UPC As String
    Public Property UPC As String
        Get
            Return _UPC
        End Get
        Set(ByVal value As String)
            _UPC = value
        End Set
    End Property

    Private _ColorDescription As String
    Public Property ColorDescription As String
        Get
            Return _ColorDescription
        End Get
        Set(ByVal value As String)
            _ColorDescription = value
        End Set
    End Property

    Private _NRFColorCode As String
    Public Property NRFColorCode As String
        Get
            Return _NRFColorCode
        End Get
        Set(ByVal value As String)
            _NRFColorCode = value
        End Set
    End Property

    Private _SizeDescription As String
    Public Property SizeDescription As String
        Get
            Return _SizeDescription
        End Get
        Set(ByVal value As String)
            _SizeDescription = value
        End Set
    End Property

    Private _NRFSizeCode As Integer
    Public Property NRFSizeCode As Integer
        Get
            Return _NRFSizeCode
        End Get
        Set(ByVal value As Integer)
            _NRFSizeCode = value
        End Set
    End Property

    Private _BrandName As String
    Public Property BrandName As String
        Get
            Return _BrandName
        End Get
        Set(ByVal value As String)
            _BrandName = value
        End Set
    End Property

    Private _VendorCollectionName As String
    Public Property VendorCollectionName As String
        Get
            Return _VendorCollectionName
        End Get
        Set(ByVal value As String)
            _VendorCollectionName = value
        End Set
    End Property

    Private _TeamName As String
    Public Property TeamName As String
        Get
            Return _TeamName
        End Get
        Set(ByVal value As String)
            _TeamName = value
        End Set
    End Property

    Private _ConsumerQtyOfUnitInPkg As String
    Public Property ConsumerQtyOfUnitInPkg As String
        Get
            Return _ConsumerQtyOfUnitInPkg
        End Get
        Set(ByVal value As String)
            _ConsumerQtyOfUnitInPkg = value
        End Set
    End Property

    Private _CountryOfOrigin As String
    Public Property CountryOfOrigin As String
        Get
            Return _CountryOfOrigin
        End Get
        Set(ByVal value As String)
            _CountryOfOrigin = value
        End Set
    End Property

    Private _FeaturesBenefitsMarketingMessage As String
    Public Property FeaturesBenefitsMarketingMessage As String
        Get
            Return _FeaturesBenefitsMarketingMessage
        End Get
        Set(ByVal value As String)
            _FeaturesBenefitsMarketingMessage = value
        End Set
    End Property

    Private _FabricOfMaterialDescription As String
    Public Property FabricOfMaterialDescription As String
        Get
            Return _FabricOfMaterialDescription
        End Get
        Set(ByVal value As String)
            _FabricOfMaterialDescription = value
        End Set
    End Property

    Private _LiningMaterial As String
    Public Property LiningMaterial As String
        Get
            Return _LiningMaterial
        End Get
        Set(ByVal value As String)
            _LiningMaterial = value
        End Set
    End Property

    'Column 2
    'Apparel
    Private _CollarType As String
    Public Property CollarType As String
        Get
            Return _CollarType
        End Get
        Set(ByVal value As String)
            _CollarType = value
        End Set
    End Property

    Private _PantInseamLength As String
    Public Property PantInseamLength As String
        Get
            Return _PantInseamLength
        End Get
        Set(ByVal value As String)
            _PantInseamLength = value
        End Set
    End Property

    Private _SleeveMeasurement As String
    Public Property SleeveMeasurement As String
        Get
            Return _SleeveMeasurement
        End Get
        Set(ByVal value As String)
            _SleeveMeasurement = value
        End Set
    End Property

    Private _SleeveType As String
    Public Property SleeveType As String
        Get
            Return _SleeveType
        End Get
        Set(ByVal value As String)
            _SleeveType = value
        End Set
    End Property

    'Footwear
    Private _HeelHeight As String
    Public Property HeelHeight As String
        Get
            Return _HeelHeight
        End Get
        Set(ByVal value As String)
            _HeelHeight = value
        End Set
    End Property

    Private _PlatformHeight As String
    Public Property PlatformHeight As String
        Get
            Return _PlatformHeight
        End Get
        Set(ByVal value As String)
            _PlatformHeight = value
        End Set
    End Property

    Private _SoleType As String
    Public Property SoleType As String
        Get
            Return _SoleType
        End Get
        Set(ByVal value As String)
            _SoleType = value
        End Set
    End Property

    Private _BootLegCircumference As String
    Public Property BootLegCircumference As String
        Get
            Return _BootLegCircumference
        End Get
        Set(ByVal value As String)
            _BootLegCircumference = value
        End Set
    End Property

    Private _BootShaftHeight As String
    Public Property BootShaftHeight As String
        Get
            Return _BootShaftHeight
        End Get
        Set(ByVal value As String)
            _BootShaftHeight = value
        End Set
    End Property

    'Jewelry
    Private _GoldKarat As String
    Public Property GoldKarat As String
        Get
            Return _GoldKarat
        End Get
        Set(ByVal value As String)
            _GoldKarat = value
        End Set
    End Property

    Private _StoneDetails As String
    Public Property StoneDetails As String
        Get
            Return _StoneDetails
        End Get
        Set(ByVal value As String)
            _StoneDetails = value
        End Set
    End Property

    Private _EarringsDrop As String
    Public Property EarringsDrop As String
        Get
            Return _EarringsDrop
        End Get
        Set(ByVal value As String)
            _EarringsDrop = value
        End Set
    End Property

    Private _WatchBandWidth As String
    Public Property WatchBandWidth As String
        Get
            Return _WatchBandWidth
        End Get
        Set(ByVal value As String)
            _WatchBandWidth = value
        End Set
    End Property

    Private _WatchCaseSize As String
    Public Property WatchCaseSize As String
        Get
            Return _WatchCaseSize
        End Get
        Set(ByVal value As String)
            _WatchCaseSize = value
        End Set
    End Property

    'Handbags & Accessories
    Private _HandbagShoulderDrop As String
    Public Property HandbagShoulderDrop As String
        Get
            Return _HandbagShoulderDrop
        End Get
        Set(ByVal value As String)
            _HandbagShoulderDrop = value
        End Set
    End Property

    'Beauty
    Private _KeyActiveIngredient As String
    Public Property KeyActiveIngredient As String
        Get
            Return _KeyActiveIngredient
        End Get
        Set(ByVal value As String)
            _KeyActiveIngredient = value
        End Set
    End Property

    'Home
    Private _ConsumerItemLength As String
    Public Property ConsumerItemLength As String
        Get
            Return _ConsumerItemLength
        End Get
        Set(ByVal value As String)
            _ConsumerItemLength = value
        End Set
    End Property

    Private _ConsumerItemWidth As String
    Public Property ConsumerItemWidth As String
        Get
            Return _ConsumerItemWidth
        End Get
        Set(ByVal value As String)
            _ConsumerItemWidth = value
        End Set
    End Property

    Private _ConsumerItemDepth As String
    Public Property ConsumerItemDepth As String
        Get
            Return _ConsumerItemDepth
        End Get
        Set(ByVal value As String)
            _ConsumerItemDepth = value
        End Set
    End Property

    Private _ConsumerItemHeight As String
    Public Property ConsumerItemHeight As String
        Get
            Return _ConsumerItemHeight
        End Get
        Set(ByVal value As String)
            _ConsumerItemHeight = value
        End Set
    End Property

    Private _ConsumerPackageDepth As String
    Public Property ConsumerPackageDepth As String
        Get
            Return _ConsumerPackageDepth
        End Get
        Set(ByVal value As String)
            _ConsumerPackageDepth = value
        End Set
    End Property

    Private _ConsumerPackageHeight As String
    Public Property ConsumerPackageHeight As String
        Get
            Return _ConsumerPackageHeight
        End Get
        Set(ByVal value As String)
            _ConsumerPackageHeight = value
        End Set
    End Property

    Private _ConsumerPackageWidth As String
    Public Property ConsumerPackageWidth As String
        Get
            Return _ConsumerPackageWidth
        End Get
        Set(ByVal value As String)
            _ConsumerPackageWidth = value
        End Set
    End Property

    Private _ConsumerPackageGrossWeight As String
    Public Property ConsumerPackageGrossWeight As String
        Get
            Return _ConsumerPackageGrossWeight
        End Get
        Set(ByVal value As String)
            _ConsumerPackageGrossWeight = value
        End Set
    End Property

    Private _WarrantyDescription As String
    Public Property WarrantyDescription As String
        Get
            Return _WarrantyDescription
        End Get
        Set(ByVal value As String)
            _WarrantyDescription = value
        End Set
    End Property

    Private _ConsumerProductCapacityOrVolume As String
    Public Property ConsumerProductCapacityOrVolume As String
        Get
            Return _ConsumerProductCapacityOrVolume
        End Get
        Set(ByVal value As String)
            _ConsumerProductCapacityOrVolume = value
        End Set
    End Property

    Private _DoesNotContain As String
    Public Property DoesNotContain As String
        Get
            Return _DoesNotContain
        End Get
        Set(ByVal value As String)
            _DoesNotContain = value
        End Set
    End Property

    Private _AerosolProduct As String
    Public Property AerosolProduct As String
        Get
            Return _AerosolProduct
        End Get
        Set(ByVal value As String)
            _AerosolProduct = value
        End Set
    End Property

    'Shared
    Private _CareInfo As String
    Public Property CareInfo As String
        Get
            Return _CareInfo
        End Get
        Set(ByVal value As String)
            _CareInfo = value
        End Set
    End Property

    Private _Closure As String
    Public Property Closure As String
        Get
            Return _Closure
        End Get
        Set(ByVal value As String)
            _Closure = value
        End Set
    End Property

    Private _FauxFur As String
    Public Property FauxFur As String
        Get
            Return _FauxFur
        End Get
        Set(ByVal value As String)
            _FauxFur = value
        End Set
    End Property

    Private _FurAnimalName As String
    Public Property FurAnimalName As String
        Get
            Return _FurAnimalName
        End Get
        Set(ByVal value As String)
            _FurAnimalName = value
        End Set
    End Property

    Private _FurCountryOfOrigin As String
    Public Property FurCountryOfOrigin As String
        Get
            Return _FurCountryOfOrigin
        End Get
        Set(ByVal value As String)
            _FurCountryOfOrigin = value
        End Set
    End Property

    Private _FurTreatment As String
    Public Property FurTreatment As String
        Get
            Return _FurTreatment
        End Get
        Set(ByVal value As String)
            _FurTreatment = value
        End Set
    End Property
End Class
