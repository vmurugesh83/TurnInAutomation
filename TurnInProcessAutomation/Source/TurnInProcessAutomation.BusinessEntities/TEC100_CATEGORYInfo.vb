
Imports System.ComponentModel
Imports System.Net

<Serializable()> _
Partial Public Class TEC100_CATEGORYInfo

    'Internal member variables
    Private _categoryCde As Integer
    Private _parentCde As Integer
    Private _startDte As DateTime
    Private _endDte As DateTime
    Private _activeFlg As String
    Private _activeDte As DateTime?
    Private _ordinalNum As Integer
    Private _imageIdNum As Integer
    Private _displayOnlyFlg As String
    Private _sesTitle As String
    Private _createDte As DateTime
    Private _modifyDte As DateTime
    Private _logonUser As String
    Private _parentDefaultFlg As String
    Private _sesUrlValue As String
    Private _inactiveTs As DateTime?
    Private _categoryDesc As String
    Private _categoryNme As String
    Private _sesMetaDesc As String
    Private _sesMetaKeyWords As String
    Private _templateId As Integer
    Private _revenueTierCde As Integer
    Private _dfltSeoTitleNme As String
    Private _cusSeoTitleNme As String
    Private _seoTitleCde As Integer
    Private _addTitleNameCde As Integer
    Private _defaultSeoDesc As String
    Private _customSeoDesc As String
    Private _seoDescCde As Integer
    Private _addDescNameCde As Integer
    Private _addShipInfoCde As Integer
    Private _seoShipInfoTxt As String
    Private _affilCategoryTxt As String
    Private _catgySrtOrdrNum As Integer
    Private _disColorFamCde As Integer
    Private _disSizeFamCde As Integer

    'Default constructor
    Public Sub New()
    End Sub

    'Constructor with initialized values
    Public Sub New(ByVal categoryCde As Integer, _
                      ByVal parentCde As Integer, _
                      ByVal startDte As DateTime, _
                      ByVal endDte As DateTime, _
                      ByVal activeFlg As String, _
                      ByVal activeDte As DateTime?, _
                      ByVal ordinalNum As Integer, _
                      ByVal imageIdNum As Integer, _
                      ByVal displayOnlyFlg As String, _
                      ByVal sesTitle As String, _
                      ByVal createDte As DateTime, _
                      ByVal modifyDte As DateTime, _
                      ByVal logonUser As String, _
                      ByVal parentDefaultFlg As String, _
                      ByVal sesUrlValue As String, _
                      ByVal inactiveTs As DateTime?, _
                      ByVal categoryDesc As String, _
                      ByVal categoryNme As String, _
                      ByVal sesMetaDesc As String, _
                      ByVal sesMetaKeyWords As String, _
                      ByVal templateId As Integer, _
                      ByVal revenueTierCde As Integer, _
                      ByVal dfltSeoTitleNme As String, _
                      ByVal cusSeoTitleNme As String, _
                      ByVal seoTitleCde As Integer, _
                      ByVal addTitleNameCde As Integer, _
                      ByVal defaultSeoDesc As String, _
                      ByVal customSeoDesc As String, _
                      ByVal seoDescCde As Integer, _
                      ByVal addDescNameCde As Integer, _
                      ByVal addShipInfoCde As Integer, _
                      ByVal seoShipInfoTxt As String, _
                      ByVal affilCategoryTxt As String, _
                      ByVal catgySrtOrdrNum As Integer, _
                      ByVal disColorFamCde As Integer, _
                      ByVal disSizeFamCde As Integer)
        Me._categoryCde = categoryCde
        Me._parentCde = parentCde
        Me._startDte = startDte
        Me._endDte = endDte
        Me._activeFlg = activeFlg
        Me._activeDte = activeDte
        Me._ordinalNum = ordinalNum
        Me._imageIdNum = imageIdNum
        Me._displayOnlyFlg = displayOnlyFlg
        Me._sesTitle = sesTitle
        Me._createDte = createDte
        Me._modifyDte = modifyDte
        Me._logonUser = logonUser
        Me._parentDefaultFlg = parentDefaultFlg
        Me._sesUrlValue = sesUrlValue
        Me._inactiveTs = inactiveTs
        Me._categoryDesc = categoryDesc
        Me._categoryNme = categoryNme
        Me._sesMetaDesc = sesMetaDesc
        Me._sesMetaKeyWords = sesMetaKeyWords
        Me._templateId = templateId
        Me._revenueTierCde = revenueTierCde
        Me._dfltSeoTitleNme = dfltSeoTitleNme
        Me._cusSeoTitleNme = cusSeoTitleNme
        Me._seoTitleCde = seoTitleCde
        Me._addTitleNameCde = addTitleNameCde
        Me._defaultSeoDesc = defaultSeoDesc
        Me._customSeoDesc = customSeoDesc
        Me._seoDescCde = seoDescCde
        Me._addDescNameCde = addDescNameCde
        Me._addShipInfoCde = addShipInfoCde
        Me._seoShipInfoTxt = seoShipInfoTxt
        Me._affilCategoryTxt = affilCategoryTxt
        Me._catgySrtOrdrNum = catgySrtOrdrNum
        Me._disColorFamCde = disColorFamCde
        Me._disSizeFamCde = disSizeFamCde
    End Sub

    <DataObjectField(True)> _
    Public Property CategoryCde() As Integer
        Get
            Return _categoryCde
        End Get
        Set(ByVal value As Integer)
            _categoryCde = value
        End Set
    End Property

    <DataObjectField(True)> _
    Public Property ParentCde() As Integer
        Get
            Return _parentCde
        End Get
        Set(ByVal value As Integer)
            _parentCde = value
        End Set
    End Property

    Public Property StartDte() As DateTime
        Get
            Return _startDte
        End Get
        Set(ByVal value As DateTime)
            _startDte = value
        End Set
    End Property

    Public Property EndDte() As DateTime
        Get
            Return _endDte
        End Get
        Set(ByVal value As DateTime)
            _endDte = value
        End Set
    End Property

    Public Property ActiveFlg() As String
        Get
            Return _activeFlg
        End Get
        Set(ByVal value As String)
            _activeFlg = value
        End Set
    End Property

    Public Property ActiveDte() As DateTime?
        Get
            Return _activeDte
        End Get
        Set(ByVal value As DateTime?)
            _activeDte = value
        End Set
    End Property

    Public Property OrdinalNum() As Integer
        Get
            Return _ordinalNum
        End Get
        Set(ByVal value As Integer)
            _ordinalNum = value
        End Set
    End Property

    Public Property ImageIdNum() As Integer
        Get
            Return _imageIdNum
        End Get
        Set(ByVal value As Integer)
            _imageIdNum = value
        End Set
    End Property

    Public Property DisplayOnlyFlg() As String
        Get
            Return _displayOnlyFlg
        End Get
        Set(ByVal value As String)
            _displayOnlyFlg = value
        End Set
    End Property

    Public Property SesTitle() As String
        Get
            Return _sesTitle
        End Get
        Set(ByVal value As String)
            _sesTitle = value
        End Set
    End Property

    Public Property CreateDte() As DateTime
        Get
            Return _createDte
        End Get
        Set(ByVal value As DateTime)
            _createDte = value
        End Set
    End Property

    Public Property ModifyDte() As DateTime
        Get
            Return _modifyDte
        End Get
        Set(ByVal value As DateTime)
            _modifyDte = value
        End Set
    End Property

    Public Property LogonUser() As String
        Get
            Return _logonUser
        End Get
        Set(ByVal value As String)
            _logonUser = value
        End Set
    End Property

    Public Property ParentDefaultFlg() As String
        Get
            Return _parentDefaultFlg
        End Get
        Set(ByVal value As String)
            _parentDefaultFlg = value
        End Set
    End Property

    Public Property SesUrlValue() As String
        Get
            Return _sesUrlValue
        End Get
        Set(ByVal value As String)
            _sesUrlValue = value
        End Set
    End Property

    Public Property InactiveTs() As DateTime?
        Get
            Return _inactiveTs
        End Get
        Set(ByVal value As DateTime?)
            _inactiveTs = value
        End Set
    End Property

    Public Property CategoryDesc() As String
        Get
            Return _categoryDesc
        End Get
        Set(ByVal value As String)
            _categoryDesc = value
        End Set
    End Property

    Public Property CategoryNme() As String
        Get
            Return _categoryNme
        End Get
        Set(ByVal value As String)
            _categoryNme = value
        End Set
    End Property

    Public Property SesMetaDesc() As String
        Get
            Return _sesMetaDesc
        End Get
        Set(ByVal value As String)
            _sesMetaDesc = value
        End Set
    End Property

    Public Property SesMetaKeyWords() As String
        Get
            Return _sesMetaKeyWords
        End Get
        Set(ByVal value As String)
            _sesMetaKeyWords = value
        End Set
    End Property

    Public Property TemplateId() As Integer
        Get
            Return _templateId
        End Get
        Set(ByVal value As Integer)
            _templateId = value
        End Set
    End Property

    Public Property RevenueTierCde() As Integer
        Get
            Return _revenueTierCde
        End Get
        Set(ByVal value As Integer)
            _revenueTierCde = value
        End Set
    End Property

    Public Property DfltSeoTitleNme() As String
        Get
            Return _dfltSeoTitleNme
        End Get
        Set(ByVal value As String)
            _dfltSeoTitleNme = value
        End Set
    End Property

    Public Property CusSeoTitleNme() As String
        Get
            Return _cusSeoTitleNme
        End Get
        Set(ByVal value As String)
            _cusSeoTitleNme = value
        End Set
    End Property

    Public Property SeoTitleCde() As Integer
        Get
            Return _seoTitleCde
        End Get
        Set(ByVal value As Integer)
            _seoTitleCde = value
        End Set
    End Property

    Public Property AddTitleNameCde() As Integer
        Get
            Return _addTitleNameCde
        End Get
        Set(ByVal value As Integer)
            _addTitleNameCde = value
        End Set
    End Property

    Public Property DefaultSeoDesc() As String
        Get
            Return _defaultSeoDesc
        End Get
        Set(ByVal value As String)
            _defaultSeoDesc = value
        End Set
    End Property

    Public Property CustomSeoDesc() As String
        Get
            Return _customSeoDesc
        End Get
        Set(ByVal value As String)
            _customSeoDesc = value
        End Set
    End Property

    Public Property SeoDescCde() As Integer
        Get
            Return _seoDescCde
        End Get
        Set(ByVal value As Integer)
            _seoDescCde = value
        End Set
    End Property

    Public Property AddDescNameCde() As Integer
        Get
            Return _addDescNameCde
        End Get
        Set(ByVal value As Integer)
            _addDescNameCde = value
        End Set
    End Property

    Public Property AddShipInfoCde() As Integer
        Get
            Return _addShipInfoCde
        End Get
        Set(ByVal value As Integer)
            _addShipInfoCde = value
        End Set
    End Property

    Public Property SeoShipInfoTxt() As String
        Get
            Return _seoShipInfoTxt
        End Get
        Set(ByVal value As String)
            _seoShipInfoTxt = value
        End Set
    End Property

    Public Property AffilCategoryTxt() As String
        Get
            Return _affilCategoryTxt
        End Get
        Set(ByVal value As String)
            _affilCategoryTxt = value
        End Set
    End Property

    Public Property CatgySrtOrdrNum() As Integer
        Get
            Return _catgySrtOrdrNum
        End Get
        Set(ByVal value As Integer)
            _catgySrtOrdrNum = value
        End Set
    End Property

    Public Property DisColorFamCde() As Integer
        Get
            Return _disColorFamCde
        End Get
        Set(ByVal value As Integer)
            _disColorFamCde = value
        End Set
    End Property

    Public Property DisSizeFamCde() As Integer
        Get
            Return _disSizeFamCde
        End Get
        Set(ByVal value As Integer)
            _disSizeFamCde = value
        End Set
    End Property

    Public ReadOnly Property CategoryNameDisplayOnlyText As String
        Get
            If DisplayOnlyFlg = "Y" Then
                Return WebUtility.HtmlDecode(CategoryNme) + " (Display Only)"
            Else
                Return WebUtility.HtmlDecode(CategoryNme)
            End If

        End Get
    End Property

End Class

