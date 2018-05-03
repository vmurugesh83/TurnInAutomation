<System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082"), _
 System.SerializableAttribute(), _
 System.Diagnostics.DebuggerStepThroughAttribute(), _
 System.ComponentModel.DesignerCategoryAttribute("code"), _
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.bonton.com/MessageCodes", TypeName:="ApplicationMessages"), _
 System.Xml.Serialization.XmlRootAttribute([Namespace]:="http://www.bonton.com/MessageCodes", IsNullable:=False, ElementName:="ApplicationMessages")> _
Partial Public Class ApplicationMessages
    Private _messages As List(Of Message)

    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute("Message", Order:=0)> _
    Public Property Messages() As System.Collections.Generic.List(Of Message)
        Get
            Return _messages
        End Get
        Set(ByVal value As System.Collections.Generic.List(Of Message))
            _messages = value
        End Set
    End Property
End Class

<System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082"), _
 System.SerializableAttribute(), _
 System.Diagnostics.DebuggerStepThroughAttribute(), _
 System.ComponentModel.DesignerCategoryAttribute("code"), _
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.bonton.com/MessageCodes", TypeName:="ApplicationMessagesMessage")> _
Partial Public Class Message
    Private _code As String
    Private _category As String
    Private _description As String
    Private _severity As SeverityType

    <System.Xml.Serialization.XmlAttributeAttribute(AttributeName:="Code")> _
    Public Property Code() As String
        Get
            Return Me._code
        End Get
        Set(ByVal value As String)
            Me._code = value
        End Set
    End Property

    <System.Xml.Serialization.XmlAttributeAttribute(AttributeName:="Category")> _
    Public Property Category() As String
        Get
            Return Me._category
        End Get
        Set(ByVal value As String)
            Me._category = value
        End Set
    End Property

    <System.Xml.Serialization.XmlAttributeAttribute(AttributeName:="Description")> _
    Public Property Description() As String
        Get
            Return Me._description
        End Get
        Set(ByVal value As String)
            Me._description = value
        End Set
    End Property

    <System.Xml.Serialization.XmlAttributeAttribute(AttributeName:="Severity")> _
    Public Property Severity() As SeverityType
        Get
            Return Me._severity
        End Get
        Set(ByVal value As SeverityType)
            Me._severity = value
        End Set
    End Property

    ''' <summary>
    ''' Returns a formatted description which includes the Code and Description.
    ''' </summary>    
    Public ReadOnly Property CodeWithDescription(Optional ByVal includeErrorCode As Boolean = True) As String
        Get
            If Not (String.IsNullOrEmpty(Code) OrElse String.IsNullOrEmpty(Description)) Then
                Return CStr(IIf(includeErrorCode, String.Format("{0} - {1}", Code, Description), Description))
            Else
                Return String.Empty
            End If
        End Get
    End Property
End Class

<System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3082"), _
 System.SerializableAttribute(), _
 System.Xml.Serialization.XmlRootAttribute([Namespace]:="", IsNullable:=False, ElementName:="severityType"), _
 System.Xml.Serialization.XmlTypeAttribute(TypeName:="severityType")> _
Public Enum SeverityType
    <System.Xml.Serialization.XmlEnumAttribute(Name:="Undefined")> _
    Undefined

    <System.Xml.Serialization.XmlEnumAttribute(Name:="Error")> _
    [Error]

    <System.Xml.Serialization.XmlEnumAttribute(Name:="Informational")> _
    Informational

    <System.Xml.Serialization.XmlEnumAttribute(Name:="Warning")> _
    Warning
End Enum

''' <summary>
''' Specifies the message code to find the message.
''' </summary>
''' <remarks>Message Codes generated from CodeSmith by the Category, Severity, and Code attributes.</remarks>
Public Enum MessageCode
    ''' <summary>
    ''' Record Saved.
    ''' </summary>
    GenericInformational001 = 1

    ''' <summary>
    ''' Record Added.
    ''' </summary>
    GenericInformational002 = 2

    ''' <summary>
    ''' Record(s) Added.
    ''' </summary>
    GenericInformational003 = 3

    ''' <summary>
    ''' Record Modified.
    ''' </summary>
    GenericInformational004 = 4

    ''' <summary>
    ''' Record Deleted.
    ''' </summary>
    GenericInformational005 = 5

    ''' <summary>
    ''' Description is required.
    ''' </summary>
    GenericError006 = 6

    ''' <summary>
    ''' Inactive Date must be greater than todays date.
    ''' </summary>
    GenericError007 = 7

    ''' <summary>
    ''' GL Company/Account Unit is not valid.
    ''' </summary>
    GenericError008 = 8

    ''' <summary>
    ''' GL Account/ SubAccount is not valid.
    ''' </summary>
    GenericError009 = 9

    ''' <summary>
    ''' GL String is not valid.
    ''' </summary>
    GenericError010 = 10

    ''' <summary>
    ''' No records to Retrieve.
    ''' </summary>
    GenericInformational011 = 11

    ''' <summary>
    ''' No information available for this PO.
    ''' </summary>
    GenericError012 = 12

    ''' <summary>
    ''' At least one line must selected.
    ''' </summary>
    GenericError013 = 13

    ''' <summary>
    ''' PO is not valid.
    ''' </summary>
    GenericError014 = 14

    ''' <summary>
    ''' No records to retrieve.
    ''' </summary>
    GenericError015 = 15

    ''' <summary>
    ''' PO does not exist.
    ''' </summary>
    GenericError016 = 16

    ''' <summary>
    ''' PO must be entered.
    ''' </summary>
    GenericError017 = 17

    ''' <summary>
    ''' No data available.
    ''' </summary>
    GenericError018 = 18

    ''' <summary>
    ''' Selection Criteria must be entered.
    ''' </summary>
    GenericError020 = 20

    ''' <summary>
    ''' GL Account is not valid.
    ''' </summary>
    GenericError021 = 21

    ''' <summary>
    ''' Entire GL Account combination is not valid.
    ''' </summary>
    GenericError022 = 22

    ''' <summary>
    ''' Vendor is not valid.
    ''' </summary>
    GenericError023 = 23

    ''' <summary>
    ''' End Date cannot come before Start Date.
    ''' </summary>
    GenericError024 = 24

    ''' <summary>
    ''' Results were limited due to volume. Please refine search criteria.
    ''' </summary>
    GenericWarning025 = 25

    ''' <summary>
    ''' User Security for Queue not setup. Contact I.S.
    ''' </summary>
    GenericError026 = 26

    ''' <summary>
    ''' {0} Vendor class is not valid for the Transaction code.
    ''' </summary>
    GenericError027 = 27

    ''' <summary>
    ''' Record(s) Added. Contact I.S in order to ensure proper setup is completed.
    ''' </summary>
    GenericInformational028 = 28


    ''' <summary>
    ''' TranId not valid.
    ''' </summary>
    RetailTransactionSystemDetailsError050 = 50

    ''' <summary>
    ''' Line Code must be selected.
    ''' </summary>
    RetailTransactionSystemDetailsError051 = 51

    ''' <summary>
    ''' Department is not valid.
    ''' </summary>
    RetailTransactionSystemDetailsError052 = 52

    ''' <summary>
    ''' Department must be entered.
    ''' </summary>
    RetailTransactionSystemDetailsError053 = 53

    ''' <summary>
    ''' Store is not valid.
    ''' </summary>
    RetailTransactionSystemDetailsError054 = 54

    ''' <summary>
    ''' Store must be entered.
    ''' </summary>
    RetailTransactionSystemDetailsError055 = 55

    ''' <summary>
    ''' Class must be entered.
    ''' </summary>
    RetailTransactionSystemDetailsError056 = 56

    ''' <summary>
    ''' Department/Class combination is not valid.
    ''' </summary>
    RetailTransactionSystemDetailsError057 = 57

    ''' <summary>
    ''' Only {0} line types are valid..
    ''' </summary>
    RetailTransactionSystemDetailsError058 = 58

    ''' <summary>
    ''' Quantity is not valid.
    ''' </summary>
    RetailTransactionSystemDetailsError059 = 59

    ''' <summary>
    ''' TranId not valid.
    ''' </summary>
    RetailTransactionSystemDetailsError060 = 60

    ''' <summary>
    ''' Invalid line code.
    ''' </summary>
    RetailTransactionSystemDetailsError061 = 61

    ''' <summary>
    ''' Cost must be entered.
    ''' </summary>
    RetailTransactionSystemDetailsError062 = 62

    ''' <summary>
    ''' Freight must be entered.
    ''' </summary>
    RetailTransactionSystemDetailsError063 = 63

    ''' <summary>
    ''' Retail must be entered.
    ''' </summary>
    RetailTransactionSystemDetailsError064 = 64

    ''' <summary>
    ''' Discount must be entered.
    ''' </summary>
    RetailTransactionSystemDetailsError065 = 65

    ''' <summary>
    ''' Cost should be less than Retail.
    ''' </summary>
    RetailTransactionSystemDetailsError066 = 66

    ''' <summary>
    ''' Deletion is not allowed for this Batch.
    ''' </summary>
    RetailTransactionSystemDetailsError067 = 67

    ''' <summary>
    ''' Deletion is not allowed without Batch.
    ''' </summary>
    RetailTransactionSystemDetailsError068 = 68

    ''' <summary>
    ''' Line items cannot be deleted for this batch.
    ''' </summary>
    RetailTransactionSystemDetailsError069 = 69

    ''' <summary>
    ''' Running Cost does not match with header.
    ''' </summary>
    RetailTransactionSystemDetailsError070 = 70

    ''' <summary>
    ''' Running Freight does not match with header.
    ''' </summary>
    RetailTransactionSystemDetailsError071 = 71

    ''' <summary>
    ''' Running Retail does not match with header.
    ''' </summary>
    RetailTransactionSystemDetailsError072 = 72

    ''' <summary>
    ''' Running Discount does not match with header.
    ''' </summary>
    RetailTransactionSystemDetailsError073 = 73

    ''' <summary>
    ''' Header Freight does not match with Detail Freight amount.
    ''' </summary>
    RetailTransactionSystemDetailsError074 = 74

    ''' <summary>
    ''' Header Handling Charge does not match with Detail Handling Charge amount.
    ''' </summary>
    RetailTransactionSystemDetailsError075 = 75

    ''' <summary>
    ''' Header does not match with Detail Handling Charge amount.
    ''' </summary>
    RetailTransactionSystemDetailsError076 = 76

    ''' <summary>
    ''' Header cost does not match with Detail running cost amount.
    ''' </summary>
    RetailTransactionSystemDetailsError077 = 77

    ''' <summary>
    ''' Header Retail does not match with Detail Running retail amount.
    ''' </summary>
    RetailTransactionSystemDetailsError078 = 78

    ''' <summary>
    ''' Discount must be entered.
    ''' </summary>
    RetailTransactionSystemDetailsError079 = 79

    ''' <summary>
    ''' Discount must be entered.
    ''' </summary>
    RetailTransactionSystemDetailsError080 = 80

    ''' <summary>
    ''' Line item added.
    ''' </summary>
    RetailTransactionSystemDetailsInformational081 = 81

    ''' <summary>
    ''' Line item updated.
    ''' </summary>
    RetailTransactionSystemDetailsInformational082 = 82

    ''' <summary>
    ''' Line item(s) deleted.
    ''' </summary>
    RetailTransactionSystemDetailsInformational083 = 83

    ''' <summary>
    ''' Transaction Code is required.
    ''' </summary>
    TransactionCodeError100 = 100

    ''' <summary>
    ''' Transaction Type is required.
    ''' </summary>
    TransactionCodeError101 = 101

    ''' <summary>
    ''' Document Type is required.
    ''' </summary>
    TransactionCodeError102 = 102

    ''' <summary>
    ''' Match Flag can only be Yes when Transaction Type is set to 'M - MAPS'.
    ''' </summary>
    TransactionCodeError103 = 103

    ''' <summary>
    ''' Header Page is required.
    ''' </summary>
    TransactionCodeError104 = 104

    ''' <summary>
    ''' UC Screen must be entered if Header Page is Chargeback.
    ''' </summary>
    TransactionCodeError105 = 105

    ''' <summary>
    ''' Detail Screen must be entered if Header Page is Chargeback.
    ''' </summary>
    TransactionCodeError106 = 106

    ''' <summary>
    ''' Line Code must be Yes when Page Link is 'AP Adjustment'.
    ''' </summary>
    TransactionCodeError107 = 107

    ''' <summary>
    ''' Deletion can only occur if the Transaction code's Entered By date is the same as the current date.
    ''' </summary>
    TransactionCodeError108 = 108

    ''' <summary>
    ''' The selected transaction code does not exist.
    ''' </summary>
    TransactionCodeError109 = 109

    ''' <summary>
    ''' Select a transaction code.
    ''' </summary>
    TransactionCodeError110 = 110

    ''' <summary>
    ''' Transaction exists in header, change status to Inactive.
    ''' </summary>
    TransactionCodeError111 = 111

    ''' <summary>
    ''' Batch Option is required.
    ''' </summary>
    TransactionCodeError112 = 112

    ''' <summary>
    ''' If Header Page is 'Chargeback or AP Adjustment', the batching option can only be 'Manual'.
    ''' </summary>
    TransactionCodeError113 = 113

    ''' <summary>
    ''' At least one of the processing flags should be set to Y.
    ''' </summary>
    TransactionCodeError114 = 114

    ''' <summary>
    ''' Select AP or GL, not both.
    ''' </summary>
    TransactionCodeError115 = 115

    ''' <summary>
    ''' Select PJ or RL, not both.
    ''' </summary>
    TransactionCodeError116 = 116

    ''' <summary>
    ''' At least one line code should be selected.
    ''' </summary>
    TransactionCodeError117 = 117

    ''' <summary>
    ''' Multiple line codes are not allowed. Only one line code should be selected.
    ''' </summary>
    TransactionCodeError118 = 118

    ''' <summary>
    ''' Transaction Code already exists.
    ''' </summary>
    TransactionCodeError119 = 119

    ''' <summary>
    ''' Transaction Code is not valid.
    ''' </summary>
    TransactionCodeError120 = 120

    ''' <summary>
    ''' Reason Code is required.
    ''' </summary>
    ReasonCodeError125 = 125

    ''' <summary>
    ''' Header Page is required.
    ''' </summary>
    ReasonCodeError126 = 126

    ''' <summary>
    ''' At least one Tran code should be selected.
    ''' </summary>
    ReasonCodeError127 = 127

    ''' <summary>
    ''' Deletion can only occur if the reason code's Entered By date is the same as the current date.
    ''' </summary>
    ReasonCodeError128 = 128

    ''' <summary>
    ''' The selected reason code does not exist.
    ''' </summary>
    ReasonCodeError129 = 129

    ''' <summary>
    ''' Select a reason code.
    ''' </summary>
    ReasonCodeError130 = 130

    ''' <summary>
    ''' Reason code exists in header, change status to Inactive first.
    ''' </summary>
    ReasonCodeError131 = 131

    ''' <summary>
    ''' Reason Code already exists.
    ''' </summary>
    ReasonCodeError132 = 132

    ''' <summary>
    ''' Reason Code is not valid.
    ''' </summary>
    ReasonCodeError133 = 133

    ''' <summary>
    ''' Line Type is required.
    ''' </summary>
    LineCodeError150 = 150

    ''' <summary>
    ''' Deletion can only occur if the line code's Entered By date is the same as the current date.
    ''' </summary>
    LineCodeError151 = 151

    ''' <summary>
    ''' The selected line code does not exist.
    ''' </summary>
    LineCodeError152 = 152

    ''' <summary>
    ''' Line Code is required.
    ''' </summary>
    LineCodeError153 = 153

    ''' <summary>
    ''' At least one value must be selected.
    ''' </summary>
    LineCodeError154 = 154

    ''' <summary>
    ''' At least one of the four signs must be positive or negative.
    ''' </summary>
    LineCodeError155 = 155

    ''' <summary>
    ''' When Line Type is Merchandise, Account must be entered.
    ''' </summary>
    LineCodeError156 = 156

    ''' <summary>
    ''' Select a line code.
    ''' </summary>
    LineCodeError157 = 157

    ''' <summary>
    ''' Line Code cannot be deleted. It already exists on Detail table.
    ''' </summary>
    LineCodeError158 = 158

    ''' <summary>
    ''' Line Code already exists.
    ''' </summary>
    LineCodeError159 = 159

    ''' <summary>
    ''' Line Code is not valid.
    ''' </summary>
    LineCodeError160 = 160

    ''' <summary>
    ''' Vendor Assignment not found and cannot be updated.
    ''' </summary>
    MatcherVendorAssignmentError175 = 175

    ''' <summary>
    ''' Combination already Exists, cannot update record.
    ''' </summary>
    MatcherVendorAssignmentError176 = 176

    ''' <summary>
    ''' Combination already Exists, cannot insert record.
    ''' </summary>
    MatcherVendorAssignmentError177 = 177

    ''' <summary>
    ''' Used ID not valid.
    ''' </summary>
    MatcherVendorAssignmentError178 = 178

    ''' <summary>
    ''' Duplicate combinations Exist, not all records copied.
    ''' </summary>
    MatcherVendorAssignmentError179 = 179

    ''' <summary>
    ''' User Id required.
    ''' </summary>
    MatcherVendorAssignmentError180 = 180

    ''' <summary>
    ''' User Id is not valid.
    ''' </summary>
    MatcherVendorAssignmentError181 = 181

    ''' <summary>
    ''' Match Type required.
    ''' </summary>
    MatcherVendorAssignmentError182 = 182

    ''' <summary>
    ''' Match Type is not valid.
    ''' </summary>
    MatcherVendorAssignmentError183 = 183

    ''' <summary>
    ''' Vendor Range From required.
    ''' </summary>
    MatcherVendorAssignmentError184 = 184

    ''' <summary>
    ''' Vendor Range From is not valid.
    ''' </summary>
    MatcherVendorAssignmentError185 = 185

    ''' <summary>
    ''' Vendor Range To required.
    ''' </summary>
    MatcherVendorAssignmentError186 = 186

    ''' <summary>
    ''' Vendor Range To is not valid.
    ''' </summary>
    MatcherVendorAssignmentError187 = 187

    ''' <summary>
    ''' Vendor Range To must be higher than or equal to Range From.
    ''' </summary>
    MatcherVendorAssignmentError188 = 188

    ''' <summary>
    ''' Mode is required.
    ''' </summary>
    BatchControlError200 = 200

    ''' <summary>
    ''' Batch is required.
    ''' </summary>
    BatchControlError201 = 201

    ''' <summary>
    ''' Batch has successfully been saved.
    ''' </summary>
    BatchControlInformational202 = 202

    ''' <summary>
    ''' Batch was not successfully saved. Please try again.
    ''' </summary>
    BatchControlError203 = 203

    ''' <summary>
    ''' Batch cannot be released, document(s) for the batch is not in BAL status.
    ''' </summary>
    BatchControlError204 = 204

    ''' <summary>
    ''' Update to Batch was successful.
    ''' </summary>
    BatchControlInformational205 = 205

    ''' <summary>
    ''' Update to Batch was not successful. Please try again.
    ''' </summary>
    BatchControlError206 = 206

    ''' <summary>
    ''' The record was never saved. Cannot delete batch.
    ''' </summary>
    BatchControlError207 = 207

    ''' <summary>
    ''' Batch was successfully deleted.
    ''' </summary>
    BatchControlInformational208 = 208

    ''' <summary>
    ''' Batch was not successfully deleted. Please try again.
    ''' </summary>
    BatchControlError209 = 209

    ''' <summary>
    ''' Cannot delete batch with a status of  {0}.
    ''' </summary>
    BatchControlError210 = 210

    ''' <summary>
    ''' Posting Date cannot be back-dated.
    ''' </summary>
    BatchControlError211 = 211

    ''' <summary>
    ''' Hit Retrieve and verify batch information before delete.
    ''' </summary>
    BatchControlSpecialError215 = 215

    ''' <summary>
    ''' Invalid Batch No.
    ''' </summary>
    BatchControlSpecialError216 = 216

    ''' <summary>
    ''' Enter Batch and Retrieve before delete.
    ''' </summary>
    BatchControlSpecialError217 = 217

    ''' <summary>
    ''' Documents must have a value greater than zero.
    ''' </summary>
    BatchControlSpecialError218 = 218

    ''' <summary>
    ''' Batch Total Documents must match Calculated Total Documents.
    ''' </summary>
    BatchControlSpecialError219 = 219

    ''' <summary>
    ''' Batch Total Cost must match Calculated Total Cost.
    ''' </summary>
    BatchControlSpecialError220 = 220

    ''' <summary>
    ''' Batch Total Retail must match Calculated Total Retail.
    ''' </summary>
    BatchControlSpecialError221 = 221

    ''' <summary>
    ''' Batch Total Discount must match Calculated Total Discount.
    ''' </summary>
    BatchControlSpecialError222 = 222

    ''' <summary>
    ''' Batch Total Freight must match Calculated Total Frieght.
    ''' </summary>
    BatchControlSpecialError223 = 223

    ''' <summary>
    ''' Enter Batch No.
    ''' </summary>
    BatchControlSpecialError224 = 224

    ''' <summary>
    ''' Batch can't be modified due to Status.
    ''' </summary>
    BatchControlInformational225 = 225

    ''' <summary>
    ''' Date must be within the current GL Period/Year.
    ''' </summary>
    BatchControlListError230 = 230

    ''' <summary>
    ''' Batch Date must be within the current GL Period/Year.
    ''' </summary>
    BatchControlListError231 = 231

    ''' <summary>
    ''' Batch Start Date must be less than or equal to Batch End Date.
    ''' </summary>
    BatchControlListError232 = 232

    ''' <summary>
    ''' Approval Start Date must be less than or equal to Approval End Date.
    ''' </summary>
    BatchControlListError233 = 233

    ''' <summary>
    ''' Batch Extract Start Date must be less than or equal to Batch Extract End Date.
    ''' </summary>
    BatchControlListError234 = 234

    ''' <summary>
    ''' Modified Start Date must be less than or equal to Modified End Date.
    ''' </summary>
    BatchControlListError235 = 235

    ''' <summary>
    ''' Select Queue.
    ''' </summary>
    BatchApprovalError241 = 241

    ''' <summary>
    ''' No Batches Selected.
    ''' </summary>
    BatchApprovalError242 = 242

    ''' <summary>
    ''' All Selected Batches have been Approved.
    ''' </summary>
    BatchApprovalInformational243 = 243

    ''' <summary>
    ''' All Selected Batches have been Unapproved.
    ''' </summary>
    BatchApprovalInformational244 = 244

    ''' <summary>
    ''' All Selected Batches have been Rejected.
    ''' </summary>
    BatchApprovalInformational245 = 245

    ''' <summary>
    ''' Tran Code must be selected before you hit Retrieve.
    ''' </summary>
    APAdjustmentsError250 = 250

    ''' <summary>
    ''' Invoice must be entered before you hit Retrieve.
    ''' </summary>
    APAdjustmentsError251 = 251

    ''' <summary>
    ''' Vendor must be entered before you hit Retrieve.
    ''' </summary>
    APAdjustmentsError252 = 252

    ''' <summary>
    ''' Document not available. Try different search criteria.
    ''' </summary>
    APAdjustmentsError253 = 253

    ''' <summary>
    ''' Document marked complete.
    ''' </summary>
    APAdjustmentsInformational254 = 254

    ''' <summary>
    ''' AP Adjustment document added.
    ''' </summary>
    APAdjustmentHeaderInformational260 = 260

    ''' <summary>
    ''' AP Adjustment document modified.
    ''' </summary>
    APAdjustmentHeaderInformational261 = 261

    ''' <summary>
    ''' AP Adjustment document modification failed.
    ''' </summary>
    APAdjustmentHeaderError262 = 262

    ''' <summary>
    ''' Deletion is not allowed at this time.
    ''' </summary>
    APAdjustmentHeaderError263 = 263

    ''' <summary>
    ''' AP Adjustment document deleted.
    ''' </summary>
    APAdjustmentHeaderInformational264 = 264

    ''' <summary>
    ''' Invalid Mode.
    ''' </summary>
    APAdjustmentHeaderError265 = 265

    ''' <summary>
    ''' Invalid Batch Number.
    ''' </summary>
    APAdjustmentHeaderError266 = 266

    ''' <summary>
    ''' Reference Type is not valid.
    ''' </summary>
    APAdjustmentHeaderError267 = 267

    ''' <summary>
    ''' Invoice Date is not valid, should only be set to current date.
    ''' </summary>
    APAdjustmentHeaderError268 = 268

    ''' <summary>
    ''' Freight Flag cannot be changed with line items on the detail page.
    ''' </summary>
    APAdjustmentHeaderError269 = 269

    ''' <summary>
    ''' Deletion is not allowed without the Batch.
    ''' </summary>
    APAdjustmentHeaderError270 = 270

    ''' <summary>
    ''' AP Adjustment deletion is not allowed for the batch.
    ''' </summary>
    APAdjustmentHeaderError271 = 271

    ''' <summary>
    ''' Reference Vendor is required.
    ''' </summary>
    APAdjustmentHeaderError272 = 272

    ''' <summary>
    ''' Reference Invoice is required.
    ''' </summary>
    APAdjustmentHeaderError273 = 273

    ''' <summary>
    ''' Tran Code must be selected before you hit Retrieve.
    ''' </summary>
    ChargebackHeaderError300 = 300

    ''' <summary>
    ''' Invoice must be entered before you hit Retrieve.
    ''' </summary>
    ChargebackHeaderError301 = 301

    ''' <summary>
    ''' Vendor must be entered before you hit Retrieve.
    ''' </summary>
    ChargebackHeaderError302 = 302

    ''' <summary>
    ''' Document not available. Try different search criteria.
    ''' </summary>
    ChargebackHeaderError303 = 303

    ''' <summary>
    ''' Chargeback document deleted.
    ''' </summary>
    ChargebackHeaderInformational304 = 304

    ''' <summary>
    ''' Document marked complete.
    ''' </summary>
    ChargebackHeaderInformational305 = 305

    ''' <summary>
    ''' Chargeback document added.
    ''' </summary>
    ChargebackHeaderInformational306 = 306

    ''' <summary>
    ''' Chargeback document modified.
    ''' </summary>
    ChargebackHeaderInformational307 = 307

    ''' <summary>
    ''' Chargeback document modification failed.
    ''' </summary>
    ChargebackHeaderError308 = 308

    ''' <summary>
    ''' Deletion is not allowed for the Batch.
    ''' </summary>
    ChargebackHeaderError309 = 309

    ''' <summary>
    ''' Deletion is not allowed without the Batch.
    ''' </summary>
    ChargebackHeaderError310 = 310

    ''' <summary>
    ''' Chargeback\Payback type Deleted.
    ''' </summary>
    ChargebackHeaderInformational311 = 311

    ''' <summary>
    ''' Chargeback\Payback Deletion is not allowed.
    ''' </summary>
    ChargebackHeaderError312 = 312

    ''' <summary>
    ''' Invalid Mode.
    ''' </summary>
    ChargebackHeaderError313 = 313

    ''' <summary>
    ''' Invalid Batch Number.
    ''' </summary>
    ChargebackHeaderError314 = 314

    ''' <summary>
    ''' Header Handling Charge does not match with Detail Cost amount.
    ''' </summary>
    ChargebackHeaderError315 = 315

    ''' <summary>
    ''' Header Mdse cost does not match with Detail Cost amount.
    ''' </summary>
    ChargebackHeaderError316 = 316

    ''' <summary>
    ''' Header cost does not match with detail sum of VCHN and 999 cost amount.
    ''' </summary>
    ChargebackHeaderError317 = 317

    ''' <summary>
    ''' Cannot run EOD at this time. No document available to process.
    ''' </summary>
    ChargebackHeaderError318 = 318

    ''' <summary>
    ''' Header Total Freight amount does not match with Detail Freight amount.
    ''' </summary>
    ChargebackHeaderError319 = 319

    ''' <summary>
    ''' Invoice exist in Lawson.
    ''' </summary>
    ChargebackHeaderError320 = 320

    ''' <summary>
    ''' Invoice exist in RTS system.
    ''' </summary>
    ChargebackHeaderError321 = 321

    ''' <summary>
    ''' Transaction Code must be selected.
    ''' </summary>
    ChargebackHeaderError322 = 322

    ''' <summary>
    ''' Batch# must be selected.
    ''' </summary>
    ChargebackHeaderError323 = 323

    ''' <summary>
    ''' Reason code must be selected.
    ''' </summary>
    ChargebackHeaderError324 = 324

    ''' <summary>
    ''' Department must be entered.
    ''' </summary>
    ChargebackHeaderError325 = 325

    ''' <summary>
    ''' Department is not valid.
    ''' </summary>
    ChargebackHeaderError326 = 326

    ''' <summary>
    ''' Vendor# must be entered.
    ''' </summary>
    ChargebackHeaderError327 = 327

    ''' <summary>
    ''' Vendor class is not valid for the Transaction code.
    ''' </summary>
    ChargebackHeaderError328 = 328

    ''' <summary>
    ''' Vendor/Department combination is not valid.
    ''' </summary>
    ChargebackHeaderError329 = 329

    ''' <summary>
    ''' Either Mdse Cost or Handling Charge is required.
    ''' </summary>
    ChargebackHeaderError330 = 330

    ''' <summary>
    ''' Either Freight or Handling Charge is required.
    ''' </summary>
    ChargebackHeaderError331 = 331

    ''' <summary>
    ''' Vendor# cannot be edited with line items present.
    ''' </summary>
    ChargebackHeaderError332 = 332

    ''' <summary>
    ''' Department cannot be edited with line items present.
    ''' </summary>
    ChargebackHeaderError333 = 333

    ''' <summary>
    ''' Cost cannot be removed with line items present.
    ''' </summary>
    ChargebackHeaderError334 = 334

    ''' <summary>
    ''' Amounts cannot be edited with line items present.
    ''' </summary>
    ChargebackHeaderError335 = 335

    ''' <summary>
    ''' Chargeback selected, but Transaction Code is Payback.
    ''' </summary>
    ChargebackHeaderError336 = 336

    ''' <summary>
    ''' Payback selected, but Transaction Code is Chargeback.
    ''' </summary>
    ChargebackHeaderError337 = 337

    ''' <summary>
    ''' Select Chargeback.
    ''' </summary>
    ChargebackHeaderError338 = 338

    ''' <summary>
    ''' No Chargeback or Payback selection allowed.
    ''' </summary>
    ChargebackHeaderError339 = 339

    ''' <summary>
    ''' Batch is not allowed for this {0}.
    ''' </summary>
    ChargebackHeaderError340 = 340

    ''' <summary>
    ''' Reference Number must be entered.
    ''' </summary>
    ChargebackHeaderError341 = 341

    ''' <summary>
    ''' Reference Number is not valid without a Reference type.
    ''' </summary>
    ChargebackHeaderError342 = 342

    ''' <summary>
    ''' Invoice Date is required.
    ''' </summary>
    ChargebackHeaderError343 = 343

    ''' <summary>
    ''' If Retail is entered, then Cost is required.
    ''' </summary>
    ChargebackHeaderError344 = 344

    ''' <summary>
    ''' If only Discount is entered, Retail cannot be entered.
    ''' </summary>
    ChargebackHeaderError345 = 345

    ''' <summary>
    ''' If only Freight is entered, Retail cannot be entered.
    ''' </summary>
    ChargebackHeaderError346 = 346

    ''' <summary>
    ''' If Discount and Freight is entered, Cost is required.
    ''' </summary>
    ChargebackHeaderError347 = 347

    ''' <summary>
    ''' Atleast Cost or Discount or Freight must be entered. All three can be entered.
    ''' </summary>
    ChargebackHeaderError348 = 348

    ''' <summary>
    ''' Invoice Date is not valid, no future date allowed.
    ''' </summary>
    ChargebackHeaderError349 = 349

    ''' <summary>
    ''' GL accounts should be provided for line code: 999, Contact I.S.
    ''' </summary>
    ChargebackHeaderError350 = 350

    ''' <summary>
    ''' GL accounts should be provided for line code: 997, Contact I.S.
    ''' </summary>
    ChargebackHeaderError351 = 351

    ''' <summary>
    ''' GL accounts should be provided.
    ''' </summary>
    ChargebackHeaderError352 = 352

    ''' <summary>
    ''' Invalid DC.
    ''' </summary>
    ChargebackHeaderError353 = 353

    ''' <summary>
    ''' DC is required.
    ''' </summary>
    ChargebackHeaderError354 = 354

    ''' <summary>
    ''' The Handling Charge Account is not setup for the location. Contact I.S.
    ''' </summary>
    ChargebackHeaderError355 = 355

    ''' <summary>
    ''' The Handling Charge is required.
    ''' </summary>
    ChargebackHeaderError356 = 356


    ''' <summary>
    ''' Vendor Allowance document added.
    ''' </summary>
    VendorAllowanceInformational400 = 400

    ''' <summary>
    ''' Vendor Allowance document modified.
    ''' </summary>
    VendorAllowanceInformational401 = 401

    ''' <summary>
    ''' Vendor Allowance document modification not allowed.
    ''' </summary>
    VendorAllowanceError402 = 402

    ''' <summary>
    ''' Warning: YTD Markdowns are going to go negative. Vendor Allowance document added.
    ''' </summary>
    VendorAllowanceWarning403 = 403

    ''' <summary>
    ''' Deletion is not allowed for the Batch.
    ''' </summary>
    VendorAllowanceError404 = 404

    ''' <summary>
    ''' Deletion is not allowed without the Batch.
    ''' </summary>
    VendorAllowanceError405 = 405

    ''' <summary>
    ''' Vendor Allowance document deleted.
    ''' </summary>
    VendorAllowanceInformational406 = 406

    ''' <summary>
    ''' Vendor Allowance document deletion is not allowed.
    ''' </summary>
    VendorAllowanceError407 = 407

    ''' <summary>
    ''' Invalid Operation.
    ''' </summary>
    VendorAllowanceError408 = 408

    ''' <summary>
    ''' Invalid Batch Number.
    ''' </summary>
    VendorAllowanceError409 = 409

    ''' <summary>
    ''' Document rejected, sent to AP for approval.
    ''' </summary>
    VendorAllowanceError410 = 410

    ''' <summary>
    ''' Document approved automatically.
    ''' </summary>
    VendorAllowanceInformational411 = 411

    ''' <summary>
    ''' Batch totals does not match Header totals.
    ''' </summary>
    VendorAllowanceError412 = 412

    ''' <summary>
    ''' Vendor is not eligible for auto-approval.  AP will review for approval.
    ''' </summary>
    VendorAllowanceError413 = 413

    ''' <summary>
    ''' Tran Code must be selected before you hit Retrieve.
    ''' </summary>
    VendorAllowanceError414 = 414

    ''' <summary>
    ''' Invoice must be entered before you hit Retrieve.
    ''' </summary>
    VendorAllowanceError415 = 415

    ''' <summary>
    ''' Vendor must be entered before you hit Retrieve.
    ''' </summary>
    VendorAllowanceError416 = 416

    ''' <summary>
    ''' Document not available. Try different search criteria.
    ''' </summary>
    VendorAllowanceError417 = 417

    ''' <summary>
    ''' Vendor Allowance document deleted.
    ''' </summary>
    VendorAllowanceInformational418 = 418

    ''' <summary>
    ''' Document marked complete.
    ''' </summary>
    VendorAllowanceInformational419 = 419

    ''' <summary>
    ''' Invoice exist in Lawson.
    ''' </summary>
    VendorAllowanceError420 = 420

    ''' <summary>
    ''' Invoice exist in RTS system.
    ''' </summary>
    VendorAllowanceError421 = 421

    ''' <summary>
    ''' Invoice Date is not valid, no future date allowed.
    ''' </summary>
    VendorAllowanceError422 = 422

    ''' <summary>
    ''' Chargeback selected, but Transaction Code is Payback.
    ''' </summary>
    VendorAllowanceError423 = 423

    ''' <summary>
    ''' Payback selected, but Transaction Code is Chargeback.
    ''' </summary>
    VendorAllowanceError424 = 424

    ''' <summary>
    ''' Select Chargeback.
    ''' </summary>
    VendorAllowanceError425 = 425

    ''' <summary>
    ''' Select Payback.
    ''' </summary>
    VendorAllowanceError426 = 426

    ''' <summary>
    ''' No Chargeback or Payback selection allowed.
    ''' </summary>
    VendorAllowanceError427 = 427

    ''' <summary>
    ''' Invoice Date is required.
    ''' </summary>
    VendorAllowanceError428 = 428

    ''' <summary>
    ''' Batch is not allowed for this {0}.
    ''' </summary>
    VendorAllowanceError429 = 429

    ''' <summary>
    ''' Transaction Code must be selected.
    ''' </summary>
    VendorAllowanceError430 = 430

    ''' <summary>
    ''' Batch# must be selected.
    ''' </summary>
    VendorAllowanceError431 = 431

    ''' <summary>
    ''' Reason code must be selected.
    ''' </summary>
    VendorAllowanceError432 = 432

    ''' <summary>
    ''' Department must be entered.
    ''' </summary>
    VendorAllowanceError433 = 433

    ''' <summary>
    ''' Department is not valid.
    ''' </summary>
    VendorAllowanceError434 = 434

    ''' <summary>
    ''' Vendor# must be entered.
    ''' </summary>
    VendorAllowanceError435 = 435

    ''' <summary>
    ''' Vendor class is not valid for the Transaction code.
    ''' </summary>
    VendorAllowanceError436 = 436

    ''' <summary>
    ''' Vendor# is not valid.
    ''' </summary>
    VendorAllowanceError437 = 437

    ''' <summary>
    ''' Vendor/Department combination is not valid.
    ''' </summary>
    VendorAllowanceError438 = 438

    ''' <summary>
    ''' Cost must be entered.
    ''' </summary>
    VendorAllowanceError439 = 439

    ''' <summary>
    ''' Retail must be entered.
    ''' </summary>
    VendorAllowanceError440 = 440

    ''' <summary>
    ''' Cost should be less than Retail.
    ''' </summary>
    VendorAllowanceError441 = 441

    ''' <summary>
    ''' The question requires a 'Yes or No' answer to proceed.
    ''' </summary>
    VendorAllowanceError442 = 442

    ''' <summary>
    ''' The question requires a 'Yes' answer to proceed.
    ''' </summary>
    VendorAllowanceError443 = 443

    ''' <summary>
    ''' The question requires a 'No' answer to proceed.
    ''' </summary>
    VendorAllowanceError444 = 444

    ''' <summary>
    ''' Vendor Authorization Number field is required. If there is no authorization number, enter none and submit supporting documentation for Retail VA's to the Mdse Analysis office.
    ''' </summary>
    VendorAllowanceError445 = 445

    ''' <summary>
    ''' Name of Vendor Representative Issuing Authorization is Required.
    ''' </summary>
    VendorAllowanceError446 = 446

    ''' <summary>
    ''' Vendor# cannot be edited with line items present.
    ''' </summary>
    VendorAllowanceError447 = 447

    ''' <summary>
    ''' Dept cannot be edited with line items present.
    ''' </summary>
    VendorAllowanceError448 = 448

    ''' <summary>
    ''' Cannot be redirected to valid document header page.
    ''' </summary>
    DocumentListError475 = 475

    ''' <summary>
    ''' Batch Start Date must be less than or equal to Batch End Date.
    ''' </summary>
    DocumentListError476 = 476

    ''' <summary>
    ''' Approved Start Date must be less than or equal to Approved End Date.
    ''' </summary>
    DocumentListError477 = 477

    ''' <summary>
    ''' Document Start Date must be less than or equal to Invoice End Date.
    ''' </summary>
    DocumentListError478 = 478

    ''' <summary>
    ''' Modified Start Date must be less than or equal to Modified End Date.
    ''' </summary>
    DocumentListError479 = 479

    ''' <summary>
    ''' Select Transaction Type.
    ''' </summary>
    RTSDataImportError490 = 490

    ''' <summary>
    ''' Selected excel file name is not valid.
    ''' </summary>
    RTSDataImportError491 = 491

    ''' <summary>
    ''' RTS Data Processing Completed!
    ''' </summary>
    RTSDataImportInformational492 = 492

    ''' <summary>
    ''' Please close the excel file if it is open.
    ''' </summary>
    RTSDataImportError493 = 493

    ''' <summary>
    ''' Browse and Select excel file to process!
    ''' </summary>
    RTSDataImportError494 = 494

    ''' <summary>
    ''' The file does not exists.
    ''' </summary>
    RTSDataImportError495 = 495

    ''' <summary>
    ''' AP Terms Code not valid.
    ''' </summary>
    PurchaseOrderListError500 = 500

    ''' <summary>
    ''' AP Additional Terms not valid.
    ''' </summary>
    PurchaseOrderListError501 = 501

    ''' <summary>
    ''' AP Payment Type is not valid.
    ''' </summary>
    PurchaseOrderListError502 = 502

    ''' <summary>
    ''' PO cannot be entered with Vendor, Dept, or Vendor/Dept entry.
    ''' </summary>
    PurchaseOrderListError503 = 503

    ''' <summary>
    ''' PO not in MAPS.
    ''' </summary>
    PurchaseOrderListError504 = 504

    ''' <summary>
    ''' PO not valid.
    ''' </summary>
    PurchaseOrderListError505 = 505

    ''' <summary>
    ''' PO, Vendor, Dept or Vendor/Dept required.
    ''' </summary>
    PurchaseOrderListError506 = 506

    ''' <summary>
    ''' Vendor cannot be entered with PO entry.
    ''' </summary>
    PurchaseOrderListError507 = 507

    ''' <summary>
    ''' Dept cannot be entered with PO entry.
    ''' </summary>
    PurchaseOrderListError508 = 508

    ''' <summary>
    ''' Ship End Date cannot be selected with PO entry.
    ''' </summary>
    PurchaseOrderListError509 = 509

    ''' <summary>
    ''' Ship Start Date must be less than or equal to Ship End Date.
    ''' </summary>
    PurchaseOrderListError510 = 510

    ''' <summary>
    ''' List Blank AP Payment Code option cannot be selected with PO entry.
    ''' </summary>
    PurchaseOrderListError511 = 511

    ''' <summary>
    ''' List Blank AP Payment Code option cannot be selected with AP Payment type selection.
    ''' </summary>
    PurchaseOrderListError512 = 512

    ''' <summary>
    ''' Select at least one purchase order to flood.
    ''' </summary>
    PurchaseOrderListError513 = 513

    ''' <summary>
    ''' Status cannot be selected with PO entry.
    ''' </summary>
    PurchaseOrderListError514 = 514

    ''' <summary>
    ''' PO Terms Code cannot be selected with PO entry.
    ''' </summary>
    PurchaseOrderListError515 = 515

    ''' <summary>
    ''' AP Terms Code cannot be selected with PO entry.
    ''' </summary>
    PurchaseOrderListError516 = 516

    ''' <summary>
    ''' PO Payment Type cannot be selected with PO entry.
    ''' </summary>
    PurchaseOrderListError517 = 517

    ''' <summary>
    ''' AP Payment Type cannot be selected with PO entry.
    ''' </summary>
    PurchaseOrderListError518 = 518

    ''' <summary>
    ''' AP Match Type cannot be selected with PO entry.
    ''' </summary>
    PurchaseOrderListError519 = 519

    ''' <summary>
    ''' Ship Start Date cannot be selected with PO entry.
    ''' </summary>
    PurchaseOrderListError520 = 520

    ''' <summary>
    ''' No Audit Information available for the PO.
    ''' </summary>
    PurchaseOrderAuditListInformational521 = 521

    ''' <summary>
    ''' Please enter a valid PO Number.
    ''' </summary>
    PurchaseOrderAuditListError522 = 522

    ''' <summary>
    ''' PayToVendor, ShipDates and Status - all must be entered.
    ''' </summary>
    PurchaseOrderListError523 = 523

    ''' <summary>
    ''' PO already has a match, maintenance can’t be performed.
    ''' </summary>
    PurchaseOrderListInformational524 = 524

    ''' <summary>
    ''' PO does not contain any receipt information.
    ''' </summary>
    ReceiptListError530 = 530

    ''' <summary>
    ''' Either PO Number or Receipt Number must be entered.
    ''' </summary>
    ReceiptListError531 = 531

    ''' <summary>
    ''' PO doesn't exist.
    ''' </summary>
    ReceiptListError532 = 532

    ''' <summary>
    ''' Receipt doesn't exist.
    ''' </summary>
    ReceiptListError533 = 533

    ''' <summary>
    ''' PO is required.
    ''' </summary>
    ReceiptListError534 = 534

    ''' <summary>
    ''' Receipt details info data not retreived
    ''' </summary>
    ReceiptDetailsInformational535 = 535

    ''' <summary>
    ''' Transaction exist for AOC Code : {0} in Invoice Detail File.
    ''' </summary>
    AOCCodeListError550 = 550

    ''' <summary>
    ''' Active Flag must be Y or N.
    ''' </summary>
    AOCCodeDetailError551 = 551

    ''' <summary>
    ''' Reverse flag must by Y or N.
    ''' </summary>
    AOCCodeDetailError552 = 552

    ''' <summary>
    ''' EDI ignore flag must be Y or N.
    ''' </summary>
    AOCCodeDetailError553 = 553

    ''' <summary>
    ''' PJ Field must be C-Cost,F-Freight,D-Discount or N-NO PJ.
    ''' </summary>
    AOCCodeDetailError554 = 554

    ''' <summary>
    ''' AOC code can not be blank.
    ''' </summary>
    AOCCodeDetailError555 = 555

    ''' <summary>
    ''' Invalid characters in AOC Code.
    ''' </summary>
    AOCCodeDetailError556 = 556

    ''' <summary>
    ''' AOC code must be unique.
    ''' </summary>
    AOCCodeDetailError557 = 557

    ''' <summary>
    ''' Invalid Transaction code.
    ''' </summary>
    AOCCodeDetailError558 = 558

    ''' <summary>
    ''' Tran code already assigned to AOC code: {0}
    ''' </summary>
    AOCCodeDetailError559 = 559

    ''' <summary>
    ''' Invalid Reason code.
    ''' </summary>
    AOCCodeDetailError560 = 560

    ''' <summary>
    ''' Invalid Tran/Reason code combination.
    ''' </summary>
    AOCCodeDetailError561 = 561

    ''' <summary>
    ''' No Invoices found. Try with different search criteria.
    ''' </summary>
    InvoiceListError600 = 600

    ''' <summary>
    ''' Valid PO Number is required.
    ''' </summary>
    InvoiceDetailError605 = 605

    ''' <summary>
    ''' Invalid PO Number.
    ''' </summary>
    InvoiceDetailError606 = 606

    ''' <summary>
    ''' Store is required on Header.
    ''' </summary>
    InvoiceDetailError607 = 607

    ''' <summary>
    ''' Store is not valid.
    ''' </summary>
    InvoiceDetailError608 = 608

    ''' <summary>
    ''' Warning: Store is not part of the PO.
    ''' </summary>
    InvoiceDetailWarning609 = 609

    ''' <summary>
    ''' Invoice document added.
    ''' </summary>
    InvoiceDetailInformational610 = 610

    ''' <summary>
    ''' Invoice document modified.
    ''' </summary>
    InvoiceDetailInformational611 = 611

    ''' <summary>
    ''' Invoice No must be entered.
    ''' </summary>
    InvoiceDetailError612 = 612

    ''' <summary>
    ''' Transaction Code must be selected.
    ''' </summary>
    InvoiceDetailError613 = 613

    ''' <summary>
    ''' Invoice/TranCode/Vendor should be unique.
    ''' </summary>
    InvoiceDetailError614 = 614

    ''' <summary>
    ''' Invoice date must be selected.
    ''' </summary>
    InvoiceDetailError615 = 615

    ''' <summary>
    ''' Invoice Terms Code must be selected.
    ''' </summary>
    InvoiceDetailError616 = 616

    ''' <summary>
    ''' Duedate should not be less than Invoicedate.
    ''' </summary>
    InvoiceDetailError617 = 617

    ''' <summary>
    ''' Valid Store is required.
    ''' </summary>
    InvoiceDetailError618 = 618

    ''' <summary>
    ''' Store is not valid.
    ''' </summary>
    InvoiceDetailError619 = 619

    ''' <summary>
    ''' Reference Vendor# is required.
    ''' </summary>
    InvoiceDetailError620 = 620

    ''' <summary>
    ''' Reference Vendor# is not allowed for tran code.
    ''' </summary>
    InvoiceDetailError621 = 621

    ''' <summary>
    ''' Reference Vendor# is not valid.
    ''' </summary>
    InvoiceDetailError622 = 622

    ''' <summary>
    ''' Reference Invoice# is required.
    ''' </summary>
    InvoiceDetailError623 = 623

    ''' <summary>
    ''' Header AOC cost is not equal to Detail Total AOC cost.
    ''' </summary>
    InvoiceDetailError624 = 624

    ''' <summary>
    ''' Invoice Amount not equal to Merchandise Cost + AOC Cost.
    ''' </summary>
    InvoiceDetailError625 = 625

    ''' <summary>
    ''' Header-Merchandise Cost not equal to Detail-Extended Cost.
    ''' </summary>
    InvoiceDetailError626 = 626

    ''' <summary>
    ''' Selected file is not valid for attachment.
    ''' </summary>
    InvoiceDetailError627 = 627

    ''' <summary>
    ''' Invalid file. Please select file from -  \\BTSAN\CORPPUBLIC\ACCOUNTS PAYABLE\MAPS TEST INVOICES\
    ''' </summary>
    InvoiceDetailError628 = 628

    ''' <summary>
    ''' Vendor is not valid for PO.
    ''' </summary>
    InvoiceDetailError629 = 629

    ''' <summary>
    ''' Dept is not valid for PO.
    ''' </summary>
    InvoiceDetailError630 = 630

    ''' <summary>
    ''' Match is not allowed for the invoice status.
    ''' </summary>
    InvoiceDetailError631 = 631

    ''' <summary>
    ''' Valid style must be entered or use style lookup.
    ''' </summary>
    InvoiceDetailError632 = 632

    ''' <summary>
    ''' Invoice Qty must be entered.
    ''' </summary>
    InvoiceDetailError633 = 633

    ''' <summary>
    ''' Invoice Unit Cost must be entered.
    ''' </summary>
    InvoiceDetailError634 = 634

    ''' <summary>
    ''' Valid style must be entered or use style lookup.
    ''' </summary>
    InvoiceDetailError635 = 635

    ''' <summary>
    ''' Invoice Qty must be entered.
    ''' </summary>
    InvoiceDetailError636 = 636

    ''' <summary>
    ''' Invoice Unit Cost must be entered.
    ''' </summary>
    InvoiceDetailError637 = 637

    ''' <summary>
    ''' Line item updated.
    ''' </summary>
    InvoiceDetailInformational638 = 638

    ''' <summary>
    ''' AOC should be selected.
    ''' </summary>
    InvoiceDetailError639 = 639

    ''' <summary>
    ''' Cost must be entered.
    ''' </summary>
    InvoiceDetailError640 = 640

    ''' <summary>
    ''' Duplicate AOC code is not allowed.
    ''' </summary>
    InvoiceDetailError641 = 641

    ''' <summary>
    ''' Type should be selected.
    ''' </summary>
    InvoiceDetailError642 = 642

    ''' <summary>
    ''' Comment must be entered.
    ''' </summary>
    InvoiceDetailError643 = 643

    ''' <summary>
    ''' Select file before viewing.
    ''' </summary>
    InvoiceDetailError644 = 644

    ''' <summary>
    ''' Selected file is not valid for attachment. please read the instructions on the page.
    ''' </summary>
    InvoiceDetailError645 = 645

    ''' <summary>
    ''' Selected file is not valid for attachment. Please select valid file.
    ''' </summary>
    InvoiceDetailError646 = 646

    ''' <summary>
    ''' Invoice Terms Code or Due Date is required.
    ''' </summary>
    InvoiceDetailError647 = 647

    ''' <summary>
    ''' Invoice Terms Code is not valid.
    ''' </summary>
    InvoiceDetailError648 = 648

    ''' <summary>
    ''' Pay Thru Date required.
    ''' </summary>
    APQueryToolCashForecastListError674 = 674

    ''' <summary>
    ''' Enter PayVendor.
    ''' </summary>
    APQueryToolCashForecastListError675 = 675

    ''' <summary>
    ''' PayVendor should not exceed 7 digits.
    ''' </summary>
    APQueryToolCashForecastListError676 = 676

    ''' <summary>
    ''' PayVendor must be numeric and seperated with space.
    ''' </summary>
    APQueryToolCashForecastListError677 = 677

    ''' <summary>
    ''' Enter Check Number(s) and Multiple Check's are seperated with space.
    ''' </summary>
    APQueryToolCheckListError678 = 678

    ''' <summary>
    ''' Check numbers should not exceed 7 digits.
    ''' </summary>
    APQueryToolCheckListError679 = 679

    ''' <summary>
    ''' Check Numbers must be numeric and seperated with space.
    ''' </summary>
    APQueryToolCheckListError680 = 680

    ''' <summary>
    ''' Enter/Select Minimum Invoice Date.
    ''' </summary>
    APQueryToolInvoiceListError681 = 681

    ''' <summary>
    ''' Enter Invoice(s) seperated by spaces.
    ''' </summary>
    APQueryToolInvoiceListError682 = 682

    ''' <summary>
    ''' Enter PO Number(s) and Multiple PO's are seperated with space.
    ''' </summary>
    APQueryToolPOClassDetailError683 = 683

    ''' <summary>
    ''' PO Numbers must be numeric and seperated with space.
    ''' </summary>
    APQueryToolPOClassDetailError684 = 684

    ''' <summary>
    ''' PO Numbers should not exceed 7 digits.
    ''' </summary>
    APQueryToolPOClassDetailError685 = 685

    ''' <summary>
    ''' Enter/Select From Date.
    ''' </summary>
    VAAutoApprovalLogError686 = 686

    ''' <summary>
    ''' Enter/Select To Date.
    ''' </summary>
    VAAutoApprovalLogError687 = 687

    ''' <summary>
    ''' PO Number does not exist.
    ''' </summary>
    PMISummaryPageError750 = 750

    ''' <summary>
    ''' Vendor is required when Department is entered.
    ''' </summary>
    PMISummaryPageError751 = 751

    ''' <summary>
    ''' Vendor/Department combination does not exist.
    ''' </summary>
    PMISummaryPageError752 = 752

    ''' <summary>
    ''' Vendor does not exist.
    ''' </summary>
    PMISummaryPageError753 = 753

    ''' <summary>
    ''' Department is required when Vendor is entered.
    ''' </summary>
    PMISummaryPageError754 = 754

    ''' <summary>
    ''' Department/Vendor combination does not exist.
    ''' </summary>
    PMISummaryPageError755 = 755

    ''' <summary>
    ''' Department does not exist.
    ''' </summary>
    PMISummaryPageError756 = 756

    ''' <summary>
    ''' Match Set ID does not exist.
    ''' </summary>
    PMISummaryPageError757 = 757

    ''' <summary>
    ''' PO Number is required when Match Type is selected.
    ''' </summary>
    PMISummaryPageError758 = 758

    ''' <summary>
    ''' Invalid input combination.
    ''' </summary>
    PMISummaryPageError759 = 759

    ''' <summary>
    ''' At least one PO must be selected.
    ''' </summary>
    PMIPOListError760 = 760

    ''' <summary>
    ''' If Pay To Vendor is entered, no other fields can be entered with it.
    ''' </summary>
    PMISummaryPageError761 = 761

    ''' <summary>
    ''' Pay To Vendor does not have valid Vendors.
    ''' </summary>
    PMISummaryPageError762 = 762

    ''' <summary>
    ''' At least one Match Set ID must be selected.
    ''' </summary>
    PMIMatchSetIDListError765 = 765

    ''' <summary>
    ''' Invoice not valid without Vendor/TranCode.
    ''' </summary>
    MatchWorkListError800 = 800

    ''' <summary>
    ''' Invoice not valid.
    ''' </summary>
    MatchWorkListError801 = 801

    ''' <summary>
    ''' Vendor not valid.
    ''' </summary>
    MatchWorkListError802 = 802

    ''' <summary>
    ''' Invoice Date is not valid if Invoice w/o Receipts option is set to NO.
    ''' </summary>
    MatchWorkListError803 = 803

    ''' <summary>
    ''' Either Match Type or Invoice Date is required if Invoice w/o Receipts option is set to YES.
    ''' </summary>
    MatchWorkListError804 = 804

    ''' <summary>
    ''' Match type not valid with PO/Invoice.
    ''' </summary>
    MatchWorkListError805 = 805

    ''' <summary>
    ''' TranCode not valid without Invoice/Vendor.
    ''' </summary>
    MatchWorkListError806 = 806

    ''' <summary>
    ''' No Invoices qualify for matching.
    ''' </summary>
    MatchWorkListError807 = 807

    ''' <summary>
    ''' No Unmatched Receipts for this PO.
    ''' </summary>
    MatchWorkListError808 = 808

    ''' <summary>
    ''' Unmatched Receipts exist for this PO.
    ''' </summary>
    MatchWorkListError809 = 809

    ''' <summary>
    ''' No Unmatched Receipts for this Invoice.
    ''' </summary>
    MatchWorkListError810 = 810

    ''' <summary>
    ''' Unmatched Receipts exist for this Invoice.
    ''' </summary>
    MatchWorkListError811 = 811

    ''' <summary>
    ''' Vendor/Match Type for PO is not assigned to matcher.
    ''' </summary>
    MatchWorkListError812 = 812

    ''' <summary>
    ''' PO not available for Matching. Either already matched or Invoice/Receipts not available yet.
    ''' </summary>
    MatchWorkListError813 = 813

    ''' <summary>
    ''' Vendor/Match Type for Invoice is not assigned to matcher.
    ''' </summary>
    MatchWorkListError814 = 814

    ''' <summary>
    ''' Invoice not available for Matching. Either already matched or Invoice/Receipts not available yet.
    ''' </summary>
    MatchWorkListError815 = 815

    ''' <summary>
    ''' Vendor/Match Type for Vendor is not assigned to matcher.
    ''' </summary>
    MatchWorkListError816 = 816

    ''' <summary>
    ''' Vendor not available for Matching. Either already matched or Invoice/Receipts not available yet.
    ''' </summary>
    MatchWorkListError817 = 817

    ''' <summary>
    ''' No information available for this PO.
    ''' </summary>
    MatchPOStoreLevelError820 = 820

    ''' <summary>
    ''' Stores with Multiple Invoices can’t be used in the Match.
    ''' </summary>
    MatchPOStoreLevelError821 = 821

    ''' <summary>
    ''' Match can't be performed. Invoices exist with no details.
    ''' </summary>
    MatchPOStoreLevelError822 = 822

    ''' <summary>
    ''' Atleast one store should be selected before doing match.
    ''' </summary>
    MatchPOStoreLevelError823 = 823

    ''' <summary>
    ''' No lines selected for on-order lookup.
    ''' </summary>
    MatchPOStoreLevelError824 = 824

    ''' <summary>
    ''' PO doesn't exist in PO system, on-order information can't be displayed.
    ''' </summary>
    MatchPOStoreLevelError825 = 825

    ''' <summary>
    ''' Some lines are protected due to inv qty being zeros - must enter detail.
    ''' </summary>
    MatchPOStoreLevelInformational826 = 826

    ''' <summary>
    ''' Some lines are protected due to inv qty being zeros - use move selected invoices.
    ''' </summary>
    MatchPOStoreLevelError827 = 827

    ''' <summary>
    ''' Invoice working set grid is empty. Match cannot be performed.
    ''' </summary>
    MatchStoreInvoiceLevelError830 = 830

    ''' <summary>
    ''' Receipt working set grid is empty. Match cannot be performed.
    ''' </summary>
    MatchStoreInvoiceLevelError831 = 831

    ''' <summary>
    ''' Multiple stores exist that don’t match each other on working set grids.
    ''' </summary>
    MatchStoreInvoiceLevelError832 = 832

    ''' <summary>
    ''' Validation failed. Match cannot be performed.
    ''' </summary>
    MatchStoreInvoiceLevelError833 = 833

    ''' <summary>
    ''' No rows available for selection.
    ''' </summary>
    MatchStoreInvoiceLevelError834 = 834

    ''' <summary>
    ''' Only one style is required for this operation.
    ''' </summary>
    MatchStoreStyleLevelError839 = 839

    ''' <summary>
    ''' The quantity can only be reduced, cannot made larger than what is displayed on the line.
    ''' </summary>
    MatchStoreStyleLevelError840 = 840

    ''' <summary>
    ''' Invoice working set grid is empty. CreateSet cannot be created.
    ''' </summary>
    MatchStoreStyleLevelError841 = 841

    ''' <summary>
    ''' Receipt working set grid is empty. CreateSet cannot be created.
    ''' </summary>
    MatchStoreStyleLevelError842 = 842

    ''' <summary>
    ''' Validation failed. CreateSet cannot be created.
    ''' </summary>
    MatchStoreStyleLevelError843 = 843

    ''' <summary>
    ''' Match Set is not available to View.
    ''' </summary>
    MatchStoreStyleLevelError844 = 844

    ''' <summary>
    ''' Miniset cannot contain multiple mismatched Stores/Styles.
    ''' </summary>
    MatchStoreStyleLevelError845 = 845

    ''' <summary>
    ''' Invoices in Mini Set have some lines not matched yet. Match can’t occur
    ''' </summary>
    MatchStoreStyleLevelError846 = 846

    ''' <summary>
    ''' Miniset(s) are not created. Match can't occur.
    ''' </summary>
    MatchStoreStyleLevelError847 = 847

    ''' <summary>
    ''' Miniset cannot contain multiple mismatched Stores.
    ''' </summary>
    MatchStoreStyleLevelError848 = 848

    ''' <summary>
    ''' Miniset cannot contain multiple mismatched Styles.
    ''' </summary>
    MatchStoreStyleLevelError849 = 849


    ''' <summary>
    ''' MatchSetId not valid.
    ''' </summary>
    MatchMiniSetListError850 = 850

    ''' <summary>
    ''' Select only one check box.
    ''' </summary>
    MatchMiniSetListError851 = 851

    ''' <summary>
    ''' Unable to retrieve no unmatched receipt records due to missing parameters. This may have resulted from session timeout. Please navigate to the Match Work List and try again.
    ''' </summary>
    MatchImportInvLevelNoUnMtchRcptsError855 = 855

    ''' <summary>
    ''' Receipt details info data not retreived.
    ''' </summary>
    ReceiptWriteOffAccountError860 = 860

    ''' <summary>
    ''' PO not exist in Receipt Header.
    ''' </summary>
    ReceiptWriteOffAccountError861 = 861

    ''' <summary>
    ''' The PO's AP Match Type can't be spaces.
    ''' </summary>
    ReceiptWriteOffAccountError862 = 862

    ''' <summary>
    ''' No Unmatched Receipts for PO.
    ''' </summary>
    ReceiptWriteOffAccountError863 = 863

    ''' <summary>
    ''' Volume for Receipt Date > 5000, Please decrease the receipt date and retry.
    ''' </summary>
    ReceiptWriteOffAccountListError865 = 865

    ''' <summary>
    ''' Enter a valid Receipt Date.
    ''' </summary>
    ReceiptWriteOffAccountListError866 = 866

    ''' <summary>
    ''' Future date not allowed.
    ''' </summary>
    ReceiptWriteOffAccountListError867 = 867

    ''' <summary>
    ''' Receipt details info data not retreived.
    ''' </summary>
    ReceiptWriteOffInventoryError870 = 870

    ''' <summary>
    ''' PO/Store/Style does not exist.
    ''' </summary>
    ReceiptWriteOffInventoryError871 = 871

    ''' <summary>
    ''' PO/Store does not exist.
    ''' </summary>
    ReceiptWriteOffInventoryError872 = 872

    ''' <summary>
    ''' No Unmatched Receipts for PO.
    ''' </summary>
    ReceiptWriteOffInventoryError873 = 873

    ''' <summary>
    ''' No Unmatched Receipts for the filter criteria entered.
    ''' </summary>
    ReceiptWriteOffInventoryError874 = 874

    ''' <summary>
    ''' Receipt/Style does not exist.
    ''' </summary>
    ReceiptWriteOffInventoryError875 = 875

    ''' <summary>
    ''' Receipt Number does not exist.
    ''' </summary>
    ReceiptWriteOffInventoryError876 = 876

    ''' <summary>
    ''' No Unmatched Receipts for the filter criteria entered.
    ''' </summary>
    ReceiptWriteOffInventoryError877 = 877

    ''' <summary>
    ''' Not a Valid Selection Combination.
    ''' </summary>
    ReceiptWriteOffInventoryError878 = 878

    ''' <summary>
    ''' No Match Set Id can be Unmatched for the PO.
    ''' </summary>
    UnMatchListError880 = 880

    ''' <summary>
    ''' Match Set Id may not exist or can't be Unmatched.
    ''' </summary>
    UnMatchListError881 = 881

    ''' <summary>
    ''' Only one Match Set Id can be un-matched at a time.
    ''' </summary>
    UnMatchListError882 = 882

    ''' <summary>
    ''' A Match Set Id line must be selected to un-matched.
    ''' </summary>
    UnMatchListError883 = 883

    ''' <summary>
    ''' Please enter PO number or Match Set Id.
    ''' </summary>
    UnMatchListError884 = 884

    ''' <summary>
    ''' Only one option can be entered.
    ''' </summary>
    UnMatchListError885 = 885

    ''' <summary>
    ''' Selected excel file name is not valid.
    ''' </summary>
    InvoiceUploadError900 = 900

    ''' <summary>
    ''' Invoice(s) processing completed!
    ''' </summary>
    InvoiceUploadInformational901 = 901

    ''' <summary>
    ''' Please close the excel file if it is open.
    ''' </summary>
    InvoiceUploadError902 = 902

    ''' <summary>
    ''' Browse and Select excel file to process!
    ''' </summary>
    InvoiceUploadError903 = 903

    ''' <summary>
    ''' Invoice/TranCode/PO is not valid.
    ''' </summary>
    InvoiceUploadError904 = 904

    ''' <summary>
    ''' DueDate is required.
    ''' </summary>
    InvoiceUploadError905 = 905

    ''' <summary>
    ''' The file does not exists.
    ''' </summary>
    InvoiceUploadError906 = 906

    ''' <summary>
    ''' Invoice is required.
    ''' </summary>
    InvoiceUploadError907 = 907

    ''' <summary>
    ''' Invalid Invoice No.
    ''' </summary>
    InvoiceUploadError908 = 908

    ''' <summary>
    ''' Tran Code is required.
    ''' </summary>
    InvoiceUploadError909 = 909

    ''' <summary>
    ''' Invalid Tran Code.
    ''' </summary>
    InvoiceUploadError910 = 910

    ''' <summary>
    ''' Invalid Transaction code.
    ''' </summary>
    InvoiceUploadError911 = 911

    ''' <summary>
    ''' Invalid PO.
    ''' </summary>
    InvoiceUploadError912 = 912

    ''' <summary>
    ''' PO is required.
    ''' </summary>
    InvoiceUploadError913 = 913

    ''' <summary>
    ''' Invoice/Trancode/PO already exist in the header.
    ''' </summary>
    InvoiceUploadError914 = 914

    ''' <summary>
    ''' Invalid Invoice Date.
    ''' </summary>
    InvoiceUploadError915 = 915

    ''' <summary>
    ''' Invoice Date is required.
    ''' </summary>
    InvoiceUploadError916 = 916

    ''' <summary>
    ''' Invalid Store.
    ''' </summary>
    InvoiceUploadError917 = 917

    ''' <summary>
    ''' Invalid Term.
    ''' </summary>
    InvoiceUploadError918 = 918

    ''' <summary>
    ''' Invalid Due Date.
    ''' </summary>
    InvoiceUploadError919 = 919

    ''' <summary>
    ''' Invalid Reason code/Tran code.
    ''' </summary>
    InvoiceUploadError920 = 920

    ''' <summary>
    ''' Invalid Merch Amount.
    ''' </summary>
    InvoiceUploadError921 = 921

    ''' <summary>
    ''' Invalid AOC Amount.
    ''' </summary>
    InvoiceUploadError922 = 922

    ''' <summary>
    ''' Invalid Invoice Amount.
    ''' </summary>
    InvoiceUploadError923 = 923

    ''' <summary>
    ''' Invoice Amount should be equal to Merch Amount + AOC Amount.
    ''' </summary>
    InvoiceUploadError924 = 924

    ''' <summary>
    ''' Invalid Detail Seq No.
    ''' </summary>
    InvoiceUploadError925 = 925

    ''' <summary>
    ''' Detail Style is required.
    ''' </summary>
    InvoiceUploadError926 = 926

    ''' <summary>
    ''' Invalid Detail Style
    ''' </summary>
    InvoiceUploadError927 = 927

    ''' <summary>
    ''' Invalid Detail Invoice Qty.
    ''' </summary>
    InvoiceUploadError928 = 928

    ''' <summary>
    ''' Invalid Detail Unit Cost.
    ''' </summary>
    InvoiceUploadError929 = 929

    ''' <summary>
    ''' Invalid AOC Seq No.
    ''' </summary>
    InvoiceUploadError930 = 930

    ''' <summary>
    ''' AOC Code is required.
    ''' </summary>
    InvoiceUploadError931 = 931

    ''' <summary>
    ''' Invalid AOC Code.
    ''' </summary>
    InvoiceUploadError932 = 932

    ''' <summary>
    ''' Invalid AOC Cost.
    ''' </summary>
    InvoiceUploadError933 = 933

    ''' <summary>
    ''' Invalid Comment Seq No.
    ''' </summary>
    InvoiceUploadError934 = 934

    ''' <summary>
    ''' Comment Type is required.
    ''' </summary>
    InvoiceUploadError935 = 935

    ''' <summary>
    ''' Invalid Comment Type.
    ''' </summary>
    InvoiceUploadError936 = 936

    ''' <summary>
    ''' Comments cannot be blank.
    ''' </summary>
    InvoiceUploadError937 = 937

    ''' <summary>
    ''' Invoice Status is not valid for Due Date update.
    ''' </summary>
    InvoiceUploadError938 = 938

    ''' <summary>
    ''' Invoice/TranCode/Vendor is not valid.
    ''' </summary>
    InvoiceUploadError939 = 939

    ''' <summary>
    ''' PO or Vendor is required.
    ''' </summary>
    InvoiceUploadError940 = 940

    ''' <summary>
    ''' PO - APMatchType not valid for the Tran code.
    ''' </summary>
    InvoiceUploadError941 = 941

    ''' <summary>
    ''' For Lawson LookUp - Invoice or Invoice/Vendor is required.
    ''' </summary>
    InquirySearchError949 = 949

    ''' <summary>
    ''' PO is required to be provided by itself.
    ''' </summary>
    InquirySearchError950 = 950

    ''' <summary>
    ''' Vendor, Invoice Number, and Tran Code required to be provided by themselves.
    ''' </summary>
    InquirySearchError951 = 951

    ''' <summary>
    ''' Invoice Number, Tran Code, and Vendor required to be provided by themselves.
    ''' </summary>
    InquirySearchError952 = 952

    ''' <summary>
    ''' Tran Code, Vendor, and Invoice Number required to be provided by themselves.
    ''' </summary>
    InquirySearchError953 = 953

    ''' <summary>
    ''' Check number is required to be provided by itself.
    ''' </summary>
    InquirySearchError954 = 954

    ''' <summary>
    ''' From Date must be supplied with the To Date.
    ''' </summary>
    InquirySearchError955 = 955

    ''' <summary>
    ''' To Date must be supplied with the From Date.
    ''' </summary>
    InquirySearchError956 = 956

    ''' <summary>
    ''' A valid date range and another filter criteria is required.
    ''' </summary>
    InquirySearchError957 = 957

    ''' <summary>
    ''' Date range cannot be larger than a two-year timeframe.
    ''' </summary>
    InquirySearchError958 = 958

    ''' <summary>
    ''' No records exist in Lawson for the selection criteria entered.
    ''' </summary>
    InquirySearchError959 = 959

    ''' <summary>
    ''' No data to retrieve for provided Invoice Number.
    ''' </summary>
    EDIInvoiceSearchError960 = 960

    ''' <summary>
    ''' No data to retrieve for provided PO Number.
    ''' </summary>
    EDIInvoiceSearchError961 = 961

    ''' <summary>
    ''' No data to retrieve for provided Vendor.
    ''' </summary>
    EDIInvoiceSearchError962 = 962

    ''' <summary>
    ''' No data to retrieve for provided date range.
    ''' </summary>
    EDIInvoiceSearchError963 = 963

    ''' <summary>
    ''' At least one other input is required (Vendor, Invoice, PO).
    ''' </summary>
    EDIInvoiceSearchError964 = 964

    ''' <summary>
    ''' TurnIn Errors
    ''' </summary>
    TurnInError1001 = 1001
    TurnInError1002 = 1002
    TurnInError1003 = 1003
    TurnInError1004 = 1004
    TurnInError1005 = 1005
    TurnInError1006 = 1006
    TurnInError1007 = 1007
    TurnInError1008 = 1008
    TurnInError1009 = 1009
    TurnInError1010 = 1010
    TurnInError1011 = 1011
    TurnInError1012 = 1012
    TurnInError1013 = 1013
    TurnInError1014 = 1014
    TurnInError1015 = 1015
    TurnInError1016 = 1016
    TurnInError1017 = 1017
    TurnInError1018 = 1018
    TurnInError1019 = 1019
    TurnInError1020 = 1020
    TurnInError1021 = 1021
    TurnInError1022 = 1022
    TurnInError1023 = 1023
    TurnInError1024 = 1024
    TurnInError1025 = 1025
    TurnInError1026 = 1026
    TurnInError1027 = 1027
    TurnInError1028 = 1028
    TurnInError1029 = 1029
    TurnInError1030 = 1030

End Enum