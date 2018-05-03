Public Class Enumerations
    Public Enum AccountKey
        Account
        AccountUnit
        SubAccount
    End Enum

    Public Enum HeaderScreenType
        Undefined
        APAdjustment
        ChargeBack
        VendorAllowance
        InvoiceDetails
    End Enum

    Public Enum InvoiceMatchOption
        A 'All
        M 'Matched
        U 'Unmatched
    End Enum

    Public Enum Modes
        ADD
        CREATE
        MAINTENANCE
        INQUIRY
        SPECIAL
    End Enum

    Public Enum PotentialMatchSearchInputType
        Undefined
        PurchaseOrderId
        Vendor
        VendorDepartment
        MatchSetId
        PayToVendor
    End Enum

    Public Enum RequestReleaseOptions
        Y 'Yes
        N 'No
    End Enum

    Public Enum GridRowLevel
        LevelUp
        LevelDown
    End Enum
End Class
