Imports BonTon.DBUtility
Imports BonTon.DBUtility.DB2Helper
Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory
Imports TurnInAutomationBatch.BusinessEntities
Imports System.Configuration
Imports System.Xml

Public Class MerchandiseDAO
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private Shared _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")
    Public Function GetModelAttributes(ByVal DepartmentID As Integer, ByVal VendorID As Integer) As Model
        Dim modelAttributes As Model = Nothing

        Dim sql As String = _spSchema + ".TU1150SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@DeptID", DB2Type.Integer), New DB2Parameter("@VendorID", DB2Type.Integer)}

        parms(0).Value = DepartmentID
        parms(1).Value = VendorID

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                modelAttributes = New Model()
                While (rdr.Read())
                    With modelAttributes
                        If Common.HasColumn(rdr, "MODEL_CATEGORY_CDE") Then
                            .ModelCategoryCode = CStr(rdr("MODEL_CATEGORY_CDE")).Trim()
                        End If

                        If Common.HasColumn(rdr, "MDSE_FIGURE_CDE") Then
                            .MerchandiseFigureCode = CStr(rdr("MDSE_FIGURE_CDE")).Trim()
                        End If
                    End With
                End While
            End Using
            Return modelAttributes
        Catch ex As Exception
            Throw ex
        End Try
        Return modelAttributes
    End Function

    Public Function GetMerchLevelAttributesByISN(ByVal InternalStyleNumber As Integer, ByVal VendorStyleNumber As String,
                                                 ByVal ColorCode As Integer, ByVal DeptID As Integer) As MerchLevelAttribute
        Dim merchLevelAttributes As MerchLevelAttribute = Nothing

        Dim sql As String = _spSchema + ".TU1153SP"
        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@ISN", DB2Type.Integer),
                                                          New DB2Parameter("@VENDOR_STYLE_NUM", DB2Type.VarChar, 20),
                                                          New DB2Parameter("@COLOR_CODE", DB2Type.SmallInt),
                                                          New DB2Parameter("@DEPT_ID", DB2Type.Integer)}

        parms(0).Value = InternalStyleNumber
        parms(1).Value = VendorStyleNumber
        parms(2).Value = ColorCode
        parms(3).Value = DeptID

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                merchLevelAttributes = New MerchLevelAttribute()
                While (rdr.Read())
                    With merchLevelAttributes
                        If Common.HasColumn(rdr, "MERCH_LVL_NUM") Then
                            .MerchLevelNumber = CStr(rdr("MERCH_LVL_NUM")).Trim()
                        End If

                        If Common.HasColumn(rdr, "FAB_DTL_ID") Then
                            .FabricationDetailID = CStr(rdr("FAB_DTL_ID")).Trim()
                        End If

                        If Common.HasColumn(rdr, "FAB_DTL_DESC") Then
                            .FabricationDetailDesc = CStr(rdr("FAB_DTL_DESC")).Trim()
                        End If

                        If Common.HasColumn(rdr, "WC_VENDOR_STYLE_NUM") Then
                            .WebCatVendorStyleNumber = CStr(rdr("WC_VENDOR_STYLE_NUM")).Trim()
                        End If

                        If Common.HasColumn(rdr, "WC_FAB_DTL_DESC") Then
                            .WebCatFabricationDetailDesc = CStr(rdr("WC_FAB_DTL_DESC")).Trim()
                        End If

                        If Common.HasColumn(rdr, "IMAGE_ID") Then
                            .FeatureImageID = CStr(rdr("IMAGE_ID")).Trim()
                        End If

                    End With
                End While
            End Using
            Return merchLevelAttributes
        Catch ex As Exception
            Throw ex
        End Try
        Return merchLevelAttributes
    End Function
End Class
