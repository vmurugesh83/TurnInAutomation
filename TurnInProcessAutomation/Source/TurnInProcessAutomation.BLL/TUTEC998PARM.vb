Imports TurnInProcessAutomation.MainframeDAL
Imports TurnInProcessAutomation.BusinessEntities

Public Class TUTEC998PARM
    Private dal As MainframeDAL.TEC998PARMDao = New MainframeDAL.TEC998PARMDao

    ''' <summary>
    ''' Retrieves all the drop down values for Drop Ship IDs.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllDropShipValues() As IList(Of TEC998PARMInfo)
        Try
            Return dal.GetAll998DropShipValues("DS_DISTRIBUTOR")
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Retrieves all the drop down values for Internal Return Instructions.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllIntReturnInstructions() As IList(Of TEC998PARMInfo)
        Try
            Return dal.GetAll998ParmValues("I")
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Retrieves all the drop down values for External Return Instructions.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllExtReturnInstructions() As IList(Of TEC998PARMInfo)
        Try
            Return dal.GetAll998ParmValues("E")
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Retrieves all the drop down values for Age.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllAgeCodes() As IList(Of TEC998PARMInfo)
        Try
            Return dal.GetAll998ParmValues("AGE")
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Retrieves all the drop down values for Gender.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllGenderCodes() As IList(Of TEC998PARMInfo)
        Try
            Return dal.GetAll998ParmValues("GENDER")
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
