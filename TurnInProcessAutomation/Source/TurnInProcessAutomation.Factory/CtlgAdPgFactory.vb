Imports System.Data.SqlClient
Imports TurnInProcessAutomation.BusinessEntities

Public Class CtlgAdPgFactory

    Public Shared Function ConstructBasic(ByVal reader As SqlDataReader) As CtlgAdPgInfo
        Dim CtlgAdPgInfoInfo As New CtlgAdPgInfo()

        Dim AdNbrIndex As Integer = reader.GetOrdinal("ad_nbr")
        If Not reader.Item(AdNbrIndex) Is Nothing Then
            CtlgAdPgInfoInfo.adnbr = CInt(reader.GetValue(AdNbrIndex))
        End If

        Dim SysPgNbrIndex As Integer = reader.GetOrdinal("sys_pg_nbr")
        If Not reader.Item(SysPgNbrIndex) Is Nothing Then
            CtlgAdPgInfoInfo.syspgnbr = CInt(reader.GetValue(SysPgNbrIndex))
        End If

        Dim PgDescIndex As Integer = reader.GetOrdinal("pg_desc")
        If Not reader.Item(PgDescIndex) Is Nothing Then
            CtlgAdPgInfoInfo.pgdesc = CStr(reader.GetValue(PgDescIndex))
        End If

        Dim PgNbrIndex As Integer = reader.GetOrdinal("pg_nbr")
        If Not reader.Item(PgNbrIndex) Is Nothing Then
            CtlgAdPgInfoInfo.pgnbr = CInt(reader.GetValue(PgNbrIndex))
        End If

        Return CtlgAdPgInfoInfo
    End Function

    Public Shared Function Construct(ByVal reader As SqlDataReader) As CtlgAdPgInfo
        Dim CtlgadpgInfo As New CtlgAdPgInfo()

        Dim AdNbrIndex As Integer = reader.GetOrdinal("ad_nbr")
        If Not reader.Item(AdNbrIndex) Is Nothing Then
            CtlgadpgInfo.adnbr = CInt(reader.GetValue(AdNbrIndex))
        End If

        Dim SysPgNbrIndex As Integer = reader.GetOrdinal("sys_pg_nbr")
        If Not reader.Item(SysPgNbrIndex) Is Nothing Then
            CtlgadpgInfo.syspgnbr = CInt(reader.GetValue(SysPgNbrIndex))
        End If

        Dim PgDescIndex As Integer = reader.GetOrdinal("pg_desc")
        If Not reader.Item(PgDescIndex) Is Nothing Then
            CtlgadpgInfo.pgdesc = CStr(reader.GetValue(PgDescIndex))
        End If

        Dim PgNbrIndex As Integer = reader.GetOrdinal("pg_nbr")
        If Not reader.Item(PgNbrIndex) Is Nothing Then
            CtlgadpgInfo.pgnbr = CInt(reader.GetValue(PgNbrIndex))
        End If

        Dim CvrPgIndIndex As Integer = reader.GetOrdinal("cvr_pg_ind")
        If Not reader.Item(CvrPgIndIndex) Is Nothing Then
            CtlgadpgInfo.cvrpgind = CStr(reader.GetValue(CvrPgIndIndex))
        End If

        Dim LogoPgIndIndex As Integer = reader.GetOrdinal("logo_pg_ind")
        If Not reader.Item(LogoPgIndIndex) Is Nothing Then
            CtlgadpgInfo.logopgind = CStr(reader.GetValue(LogoPgIndIndex))
        End If

        Dim ColorCdIndex As Integer = reader.GetOrdinal("color_cd")
        If Not reader.Item(ColorCdIndex) Is Nothing Then
            CtlgadpgInfo.colorcd = CStr(reader.GetValue(ColorCdIndex))
        End If

        Dim PaprStkDescIndex As Integer = reader.GetOrdinal("papr_stk_desc")
        If Not reader.Item(PaprStkDescIndex) Is Nothing Then
            CtlgadpgInfo.paprstkdesc = CStr(reader.GetValue(PaprStkDescIndex))
        End If

        Dim PaprWghtDescIndex As Integer = reader.GetOrdinal("papr_wght_desc")
        If Not reader.Item(PaprWghtDescIndex) Is Nothing Then
            CtlgadpgInfo.paprwghtdesc = CStr(reader.GetValue(PaprWghtDescIndex))
        End If

        Dim BleedHghtQtyIndex As Integer = reader.GetOrdinal("bleed_hght_qty")
        If Not reader.Item(BleedHghtQtyIndex) Is Nothing Then
            CtlgadpgInfo.bleedhghtqty = CDec(reader.GetValue(BleedHghtQtyIndex))
        End If

        Dim BleedWdthQtyIndex As Integer = reader.GetOrdinal("bleed_wdth_qty")
        If Not reader.Item(BleedWdthQtyIndex) Is Nothing Then
            CtlgadpgInfo.bleedwdthqty = CDec(reader.GetValue(BleedWdthQtyIndex))
        End If

        Dim LiveHghtQtyIndex As Integer = reader.GetOrdinal("live_hght_qty")
        If Not reader.Item(LiveHghtQtyIndex) Is Nothing Then
            CtlgadpgInfo.livehghtqty = CDec(reader.GetValue(LiveHghtQtyIndex))
        End If

        Dim LiveWdthQtyIndex As Integer = reader.GetOrdinal("live_wdth_qty")
        If Not reader.Item(LiveWdthQtyIndex) Is Nothing Then
            CtlgadpgInfo.livewdthqty = CDec(reader.GetValue(LiveWdthQtyIndex))
        End If

        Dim TrimHghtQtyIndex As Integer = reader.GetOrdinal("trim_hght_qty")
        If Not reader.Item(TrimHghtQtyIndex) Is Nothing Then
            CtlgadpgInfo.trimhghtqty = CDec(reader.GetValue(TrimHghtQtyIndex))
        End If

        Dim TrimWdthQtyIndex As Integer = reader.GetOrdinal("trim_wdth_qty")
        If Not reader.Item(TrimWdthQtyIndex) Is Nothing Then
            CtlgadpgInfo.trimwdthqty = CDec(reader.GetValue(TrimWdthQtyIndex))
        End If

        Dim FlatHghtQtyIndex As Integer = reader.GetOrdinal("flat_hght_qty")
        If Not reader.Item(FlatHghtQtyIndex) Is Nothing Then
            CtlgadpgInfo.flathghtqty = CDec(reader.GetValue(FlatHghtQtyIndex))
        End If

        Dim FlatWdthQtyIndex As Integer = reader.GetOrdinal("flat_wdth_qty")
        If Not reader.Item(FlatWdthQtyIndex) Is Nothing Then
            CtlgadpgInfo.flatwdthqty = CDec(reader.GetValue(FlatWdthQtyIndex))
        End If

        Dim PrepressJobNbrIndex As Integer = reader.GetOrdinal("prepress_job_nbr")
        If Not reader.Item(PrepressJobNbrIndex) Is Nothing Then
            CtlgadpgInfo.prepressjobnbr = CStr(reader.GetValue(PrepressJobNbrIndex))
        End If

        Return CtlgadpgInfo
    End Function

    Public Shared Function ConstructImgNotes(ByVal reader As SqlDataReader) As AdminImageNotesInfo
        Dim CtlgImgNotesInfo As New AdminImageNotesInfo()

        Dim AdNbrIndex As Integer = reader.GetOrdinal("AD_NBR_IMG_NBR")
        If Not reader.Item(AdNbrIndex) Is Nothing Then
            CtlgImgNotesInfo.AdNbrAdminImgNbr = CStr(reader.GetValue(AdNbrIndex))
        End If

        Dim ImageNotesIndex As Integer = reader.GetOrdinal("IMG_NOTES")
        If Not reader.Item(ImageNotesIndex) Is Nothing Then
            CtlgImgNotesInfo.ImageNotes = CStr(reader.GetValue(ImageNotesIndex))
        End If

        Dim ImageSuffixIndex As Integer = reader.GetOrdinal("IMG_SUFFIX")
        If Not reader.Item(ImageSuffixIndex) Is Nothing Then
            'Image Suffix of X denotes killed items in Admin database.
            CtlgImgNotesInfo.ImageSuffix = If(CStr(reader.GetValue(ImageSuffixIndex)).Trim.ToUpper = "X", "Y", "N")
        End If

        Dim ImageSuffixDescIndex As Integer = reader.GetOrdinal("IMG_SUFFIX")
        If Not reader.Item(ImageSuffixDescIndex) Is Nothing Then
            CtlgImgNotesInfo.ImageSuffixDesc = CStr(reader.GetValue(ImageSuffixDescIndex)).Trim
        End If

        Return CtlgImgNotesInfo
    End Function
End Class

