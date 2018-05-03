Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class TTUConfigFactory
    Public Shared Function Construct(ByVal reader As DB2DataReader) As TTUConfig
        Dim TurnInConfiguration As New TTUConfig()

        With TurnInConfiguration
            .ConfigKey = CStr(ReadColumn(reader, "CONFIG_ATTRIB_NME"))
            .SecondLevelConfigKey = CStr(ReadColumn(reader, "CONFIG_2ND_LVL_NME"))
            .NumericConfigValue = CDec(ReadColumn(reader, "CONFIG_ATTRIB_NUM"))
            .DateConfigValue = CDate(ReadColumn(reader, "CONFIG_ATTRIB_DTE"))
            .ConfigDescription = CStr(ReadColumn(reader, "CONFIG_ATTRIB_DESC"))
            .StringConfigValue = CStr(ReadColumn(reader, "CONFIG_ATTRIB_TXT"))
            .LastModifiedBy = CStr(ReadColumn(reader, "LAST_MOD_ID"))
            .LastModifiedTimestamp = DirectCast(ReadColumn(reader, "LAST_MOD_TS"), DateTime)
        End With

        Return TurnInConfiguration
    End Function
End Class

