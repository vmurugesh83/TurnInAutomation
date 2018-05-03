Public Class Enumerations
    Public Enum AdType
        WEB = 0
        TRANSFER = 1
        VENDOR_IMAGE = 2
    End Enum
    Public Enum MerchLevelType
        STYLE = 1
        COLORONLY = 2
        COLORSIZE = 3
        SIZEONLY = 4
    End Enum
    Public Enum ImageCategoryCode
        FEAT
        REND
        SWTCH
        STDALN
        SSF
    End Enum
    Public Enum TurnInUsageCode
        Print = 1
        Ecommerce = 2
    End Enum
    Public Enum BatchStatusCode
        S 'Success
        F 'Failure
        W 'Warning
    End Enum
    Public Enum TurnInItemType
        VendorImageID = 1
        MerchandiseID = 2
        InternalStyleNumber = 3
    End Enum
    Public Enum ImageKindType
        CR8 'Create
        DUP 'Duplicate
        [NEW] 'New
        NOMER 'No Merchandise
        PU 'Pick up
        VND 'Vendor
    End Enum
    Public Enum TMSParameterKey
        ENABLE_AUTO_TURNIN
        ENABLE_WEBCAT_AUTO_SUBMIT
    End Enum
    Public Enum LogEntryType
        Information
        Warning
        AppError
    End Enum
    Public Enum MerchandiseFigureCode
        OFF
        [ON]
    End Enum
End Class
