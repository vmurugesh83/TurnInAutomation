Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities
Imports System.Data.SqlClient

Public Class TUTMS900PARAMETER
    Private dal As MainframeDAL.TMS900PARAMETERDao = New MainframeDAL.TMS900PARAMETERDao

    Public Function GetAllSizeCategories() As IList(Of TMS900PARAMETERInfo)
        Try
            Return dal.GetAllFromTMS900PARAMETER("ISN_SIZE_CATGY_CDE", 0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetAllModelCategories(ByVal DeptId As Integer) As IList(Of TMS900PARAMETERInfo)
        Try
            Return dal.GetAllFromTMS900PARAMETER("MODEL_CATEGORY_CDE", DeptId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetAllModelCategories() As IList(Of TMS900PARAMETERInfo)
        Try
            Return dal.GetAllFromTMS900PARAMETER("MODEL_CATEGORY_CDE", 0).OrderBy(Function(x) x.ShortDesc).ToList
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetAllMDSEFigureCodes() As IList(Of TMS900PARAMETERInfo)
        Try
            Return dal.GetAllFromTMS900PARAMETER("MDSE_FIGURE_CDE", 0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Function GetAllImageKindValues() As IList(Of TMS900PARAMETERInfo)
        Try
            Return dal.GetAllFromTMS900PARAMETER("IMAGE_KIND_CDE", 0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetAllFeatureRenderSwatchValues() As IList(Of TMS900PARAMETERInfo)
        Try
            Return dal.GetAllFromTMS900PARAMETER("IMAGE_CATEGORY_CDE", 0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetAllImageTypeValues() As IList(Of TMS900PARAMETERInfo)
        Try
            Return dal.GetAllFromTMS900PARAMETER("MDSE_FIGURE_CDE", 0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetAllAltViewValues() As IList(Of TMS900PARAMETERInfo)
        Try
            Return dal.GetAllFromTMS900PARAMETER("ALT_PRDCT_VIEW_CDE", 0).OrderBy(Function(x) x.LIST_SEQ_NUM).ThenBy(Function(x) x.ShortDesc).ToList
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetAllTurnInTypeValues() As IList(Of TMS900PARAMETERInfo)
        Try
            Return dal.GetAllFromTMS900PARAMETER("AD_TURN_IN_CDE", 0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetAllWaistDownClasses() As IList(Of TMS900PARAMETERInfo)
        Try
            Return dal.GetAllFromTMS900PARAMETER("WAIST_DOWN_ELIGIBLE", 0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Function GetAllSize1And2Classes(ByVal DeptID As Integer, ByVal ClassID As Integer) As IList(Of TMS900PARAMETERInfo)
        Try
            Return dal.GetAllFromTMS900PARAMETERByIntColumns("SSKU_STYLE_DESC_ELIGIBLE", DeptID, ClassID)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetAllSKUUse() As IList(Of TMS900PARAMETERInfo)
        Try
            Return dal.GetAllFromTMS900PARAMETER("SKU_USE_CDE", 0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
