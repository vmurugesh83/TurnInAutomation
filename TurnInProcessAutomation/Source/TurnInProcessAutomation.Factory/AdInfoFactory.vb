Imports System.Data.SqlClient
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory.Common

Public Class AdInfoFactory

    Public Shared Function Construct(ByVal reader As SqlDataReader) As AdInfoInfo
        Dim AdinfoInfo As New AdInfoInfo()

        With AdinfoInfo
            .TurnInDate = ConvertToString(ReadColumn(reader, "job_step_due_dt"))
            .adnbr = CInt(ReadColumn(reader, "ad_nbr"))
            .addesc = ConvertToString(ReadColumn(reader, "ad_desc"))
            .adstatcd = ConvertToString(ReadColumn(reader, "ad_stat_cd"))
            .mediacd = ConvertToString(ReadColumn(reader, "media_cd"))
            .mediatypecd = ConvertToString(ReadColumn(reader, "media_type_cd"))
            .adrunstartdt = CDate(ReadColumn(reader, "ad_run_start_dt"))
            .adrunenddt = CDate(ReadColumn(reader, "ad_run_end_dt"))
            .eventnbr = CInt(ReadColumn(reader, "event_nbr"))
            .salenbr = CInt(ReadColumn(reader, "sale_nbr"))
            .fiscalyr = CInt(ReadColumn(reader, "fiscal_yr"))
            .seasoncd = ConvertToString(ReadColumn(reader, "season_cd"))
            .fiscalmthnbr = ConvertToString(ReadColumn(reader, "fiscal_mth_nbr"))
            .prdctnsrccd = ConvertToString(ReadColumn(reader, "prdctn_src_cd"))
            .distbnmethcd = ConvertToString(ReadColumn(reader, "distbn_meth_cd"))
            .adspontypecd = ConvertToString(ReadColumn(reader, "ad_spon_type_cd"))
            .adsponnbr = CInt(ReadColumn(reader, "ad_spon_nbr"))
            .finclspontypecd = ConvertToString(ReadColumn(reader, "fincl_spon_type_cd"))
            .finclsponnbr = CInt(ReadColumn(reader, "fincl_spon_nbr"))
            .coopamt = CDec(ReadColumn(reader, "coop_amt"))
            .cooppct = CDec(ReadColumn(reader, "coop_pct"))
            .adbdgtamt = CDec(ReadColumn(reader, "ad_bdgt_amt"))
            .leasefiscalyr = CInt(ReadColumn(reader, "lease_fiscal_yr"))
            .leasefiscalmth = ConvertToString(ReadColumn(reader, "lease_fiscal_mth"))
            .adpabpct = CDec(ReadColumn(reader, "ad_pab_pct"))
            .clsddscnryind = ConvertToString(ReadColumn(reader, "clsd_dscnry_ind"))
            .leaseclsdind = ConvertToString(ReadColumn(reader, "lease_clsd_ind"))
            .coopdscnryind = ConvertToString(ReadColumn(reader, "coop_dscnry_ind"))
            .coopadjmntind = ConvertToString(ReadColumn(reader, "coop_adjmnt_ind"))
            .mediachrgdnbr = ConvertToString(ReadColumn(reader, "media_chrgd_nbr"))
            .dateadded = CDate(ReadColumn(reader, "date_added"))
            .merchcriteriamin = CDec(ReadColumn(reader, "merch_criteria_min"))
            .merchcriteriamax = CDec(ReadColumn(reader, "merch_criteria_max"))
            .merchcriteriaind = ConvertToString(ReadColumn(reader, "merch_criteria_ind"))
            .sendtoc3 = ConvertToString(ReadColumn(reader, "send_to_c3"))
            .vrsntoc3 = ConvertToString(ReadColumn(reader, "vrsn_to_c3"))
            .datetoaprimo = CDate(ReadColumn(reader, "date_to_aprimo"))
            .datetoc3 = CDate(ReadColumn(reader, "date_to_c3"))
            .addesctoc3 = ConvertToString(ReadColumn(reader, "ad_desc_to_c3"))
        End With

        Return AdinfoInfo
    End Function
End Class

