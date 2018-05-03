using System;

namespace CatalogEstimating
{
    public enum UserRights
    {
        SuperAdmin = 4,
        Admin      = 3,
        Create     = 2,
        ReadOnly   = 1,
        Exclude    = 0
    }

    public enum DatabaseType
    {
        Live     = 1,
        Seasonal = 2,
        Test     = 3,
        Admin    = 4
    }

    public enum EstimateStatus
    {
        Active   = 1,
        Uploaded = 2,
        Killed   = 3
    }

    public enum FiscalMonths
    {
        February  = 1,
        March     = 2,
        April     = 3,
        May       = 4,
        June      = 5,
        July      = 6,
        August    = 7,
        September = 8,
        October   = 9,
        November  = 10,
        December  = 11,
        January   = 12
    }

    public enum FiscalSeasons
    {
        Spring = 1,
        Fall = 2
    }

    public enum InsertDOW
    {
        Sunday    = 1,
        Monday    = 2,
        Tuesday   = 3,
        Wednesday = 4,
        Thursday  = 5,
        Friday    = 6,
        Saturday  = 7
    }

    public enum PrinterRateType
    {
        StitchIn          = 1,
        BlowIn            = 2,
        Carton            = 3,
        StitcherMakeready = 4,
        DigitalHandlePrep = 5,
        PBMakeready       = 6,
        PBBag             = 7,
        Plates            = 8,
        Onsert            = 9
    }

    public enum ReportExecutionStatus
    {
        InvalidSearchCriteria = -1,
        NoDataReturned        = 0,
        Success               = 1
    }

}
