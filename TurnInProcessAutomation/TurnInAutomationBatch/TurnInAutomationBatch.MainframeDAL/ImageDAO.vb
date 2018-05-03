Imports BonTon.DBUtility
Imports BonTon.DBUtility.DB2Helper
Imports IBM.Data.DB2
Imports TurnInProcessAutomation.BusinessEntities
Imports TurnInProcessAutomation.Factory
Imports System.Configuration
Imports System.Xml
Public Class ImageDAO
    'Static constants 
    Private Shared _spSchema As String = ConfigurationManager.AppSettings("SPSchema")
    Private Shared _dbSchema As String = ConfigurationManager.AppSettings("DB2Schema")

    Public Function CreateImageRequest(ByVal ImageRequest As XmlNode) As Integer
        Dim imageRequestID As Integer = 0
        Dim sql As String = _spSchema + ".TU1147SP"
        Dim turnInImageIDParameter As New DB2Parameter("@IMAGE_REQ_ID", DB2Type.Integer)

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@PPM_IMAGE_REQ_ID", DB2Type.Integer), _
                                                  New DB2Parameter("@TURN_IN_USAGE_CDE", DB2Type.SmallInt), _
                                                  New DB2Parameter("@IMGE_CLR_CORCT_FLG", DB2Type.Char, 1), _
                                                  New DB2Parameter("@MODELS_REQUIRE_QTY", DB2Type.SmallInt), _
                                                  New DB2Parameter("@PICKUP_IMAGE_FLG", DB2Type.Char, 1), _
                                                  New DB2Parameter("@IMAGE_KIND_TYP", DB2Type.Char, 5), _
                                                  New DB2Parameter("@IMAGE_CATEGORY_CDE", DB2Type.Char, 6), _
                                                  New DB2Parameter("@IMAGE_SHOT_NUM", DB2Type.SmallInt), _
                                                  New DB2Parameter("@IMAGE_NME", DB2Type.VarChar, 51), _
                                                  New DB2Parameter("@DUP_IMAGE_STYL_NUM", DB2Type.VarChar, 20), _
                                                  New DB2Parameter("@ADMIN_IMAGE_NUM", DB2Type.Integer), _
                                                  New DB2Parameter("@ADMIN_IMGENOTE_TXT", DB2Type.VarChar, 35), _
                                                  New DB2Parameter("@ADMIN_IMAGE_DESC", DB2Type.VarChar, 30), _
                                                  New DB2Parameter("@LAST_MOD_ID", DB2Type.VarChar, 30), _
                                                  turnInImageIDParameter}

        turnInImageIDParameter.Direction = ParameterDirection.Output

        For counter As Integer = 0 To parms.Length - 1
            parms(counter).Value = ImageRequest.ChildNodes(counter).InnerText
        Next

        Try
            ExecuteNonQuery(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
            imageRequestID = CInt(turnInImageIDParameter.Value)
        Catch ex As Exception
            Throw ex
        End Try
        Return imageRequestID
    End Function
    Public Function GetImageCategoryCodeByDeptAndVendor(ByVal DepartmentID As Integer, ByVal VendorID As Integer) As String
        Dim imageCategoryCode As String = String.Empty
        Dim sql As String = _spSchema + ".TU1154SP"

        Dim parms As DB2Parameter() = New DB2Parameter() {New DB2Parameter("@DEPT_ID", DB2Type.Integer), _
                                                  New DB2Parameter("@VENDOR_ID", DB2Type.Integer)}


        parms(0).Value = DepartmentID
        parms(1).Value = VendorID

        Try
            Using rdr As DB2DataReader = ExecuteReader(ConnectionStringLocalTransaction, CommandType.StoredProcedure, sql, parms)
                If (rdr.HasRows) Then
                    rdr.Read()
                    If Common.HasColumn(rdr, "IMAGE_CATEGORY_CDE") Then
                        imageCategoryCode = CStr(rdr("IMAGE_CATEGORY_CDE")).Trim()
                    End If
                End If
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return imageCategoryCode
    End Function
End Class
