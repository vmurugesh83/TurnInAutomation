Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class TEC100_CATEGORYFactory

    Public Shared Function Construct(ByVal reader As DB2DataReader) As TEC100_CATEGORYInfo
        Dim tec100CategoryInfo As New TEC100_CATEGORYInfo()
    
        With tec100CategoryInfo
            .CategoryCde = CInt(ReadColumn(reader, "CATEGORY_CDE"))
            .ParentCde = CInt(ReadColumn(reader, "PARENT_CDE"))
            .StartDte = CDate(ReadColumn(reader, "START_DTE"))
            .EndDte = CDate(ReadColumn(reader, "END_DTE"))
            .ActiveFlg = CStr(ReadColumn(reader, "ACTIVE_FLG"))
            .ActiveDte = CDate(ReadColumn(reader, "ACTIVE_DTE"))
            .OrdinalNum = CInt(ReadColumn(reader, "ORDINAL_NUM"))
            .ImageIdNum = CInt(ReadColumn(reader, "IMAGE_ID_NUM"))
            .DisplayOnlyFlg = CStr(ReadColumn(reader, "DISPLAY_ONLY_FLG"))
            .SesTitle = CStr(ReadColumn(reader, "SES_TITLE"))
            .CreateDte = CDate(ReadColumn(reader, "CREATE_DTE"))
            .ModifyDte = CDate(ReadColumn(reader, "MODIFY_DTE"))
            .LogonUser = CStr(ReadColumn(reader, "LOGON_USER"))
            .ParentDefaultFlg = CStr(ReadColumn(reader, "PARENT_DEFAULT_FLG"))
            .SesUrlValue = CStr(ReadColumn(reader, "SES_URL_VALUE"))
            .InactiveTs = CType(ReadColumn(reader, "INACTIVE_TS"), DateTime)
            .CategoryDesc = CStr(ReadColumn(reader, "CATEGORY_DESC"))
            .CategoryNme = CStr(ReadColumn(reader, "CATEGORY_NME"))
            .SesMetaDesc = CStr(ReadColumn(reader, "SES_META_DESC"))
            .SesMetaKeyWords = CStr(ReadColumn(reader, "SES_META_KEY_WORDS"))
            .TemplateId = CInt(ReadColumn(reader, "TEMPLATE_ID"))
            .RevenueTierCde = CInt(ReadColumn(reader, "REVENUE_TIER_CDE"))
            .DfltSeoTitleNme = CStr(ReadColumn(reader, "DFLT_SEO_TITLE_NME"))
            .CusSeoTitleNme = CStr(ReadColumn(reader, "CUS_SEO_TITLE_NME"))
            .SeoTitleCde = CInt(ReadColumn(reader, "SEO_TITLE_CDE"))
            .AddTitleNameCde = CInt(ReadColumn(reader, "ADD_TITLE_NAME_CDE"))
            .DefaultSeoDesc = CStr(ReadColumn(reader, "DEFAULT_SEO_DESC"))
            .CustomSeoDesc = CStr(ReadColumn(reader, "CUSTOM_SEO_DESC"))
            .SeoDescCde = CInt(ReadColumn(reader, "SEO_DESC_CDE"))
            .AddDescNameCde = CInt(ReadColumn(reader, "ADD_DESC_NAME_CDE"))
            .AddShipInfoCde = CInt(ReadColumn(reader, "ADD_SHIP_INFO_CDE"))
            .SeoShipInfoTxt = CStr(ReadColumn(reader, "SEO_SHIP_INFO_TXT"))
            .AffilCategoryTxt = CStr(ReadColumn(reader, "AFFIL_CATEGORY_TXT"))
            .CatgySrtOrdrNum = CInt(ReadColumn(reader, "CATGY_SRT_ORDR_NUM"))
            .DisColorFamCde = CInt(ReadColumn(reader, "DIS_COLOR_FAM_CDE"))
            .DisSizeFamCde = CInt(ReadColumn(reader, "DIS_SIZE_FAM_CDE"))
        End With
        
        Return tec100CategoryInfo
    End Function
End Class

