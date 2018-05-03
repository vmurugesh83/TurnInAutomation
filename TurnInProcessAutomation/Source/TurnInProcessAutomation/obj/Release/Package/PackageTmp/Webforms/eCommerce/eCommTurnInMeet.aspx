<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/ContentPage.Master" CodeBehind="eCommTurnInMeet.aspx.vb" Inherits="TurnInProcessAutomation.eCommTurnInMeet" ValidateRequest="false" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ MasterType VirtualPath="~/ContentPage.Master" %>
<%@ Reference Control="~/WebUserControls/eCommerce/eCommTurnInMeetCtrl.ascx" %>
<%@ Register TagPrefix="rts" TagName="Modal" Src="~/WebUserControls/ModalPopupControl.ascx" %>
<%@ Register TagPrefix="TurnIn" TagName="ModalOrderColors" Src="~/WebUserControls/ModalOrderColors.ascx" %>
<asp:Content ID="eCommTurnInMaintForm" ContentPlaceHolderID="ContentArea" runat="Server">
    <telerik:RadSplitter ID="rseCommTurnInMaint" runat="server" SkinID="pageSplitter">
        <telerik:RadPane ID="rpHeader" runat="server" SkinID="pageHeaderAreaPane">
            <TurnIn:ModalOrderColors ID="tuModalOrderColors" runat="server" />
            <div id="pageActionBar">
                <telerik:RadToolBar ID="rtbeCommTurnInMeet" runat="server" OnClientButtonClicking="OnClientRadToolBarClick"
                    OnClientLoad="clientLoad" CssClass="SeparatedButtons">
                    <Items>
                        <telerik:RadToolBarButton runat="server" CommandName="Sort" DisabledImageUrl="~/Images/List_d.gif"
                            ImageUrl="~/Images/List.gif" Text="Sort" CssClass="rightAligned" Visible="true">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Export" DisabledImageUrl="~/Images/Export_d.gif"
                            ImageUrl="~/Images/Export.gif" Text="Export" CssClass="rightAligned" CausesValidation="false">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Submit" DisabledImageUrl="~/Images/Submit.gif"
                            ImageUrl="~/Images/Submit.gif" Text="Submit" CssClass="rightAligned">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Delete" DisabledImageUrl="~/Images/Delete_d.gif"
                            ImageUrl="~/Images/Delete.gif" Text="Delete" CssClass="rightAligned">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Reject" DisabledImageUrl="~/Images/Submit.gif"
                            ImageUrl="~/Images/Submit.gif" Text="Reject" CssClass="rightAligned">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="CancelAll" DisabledImageUrl="~/Images/Cancel_d.gif"
                            ImageUrl="~/Images/Cancel.gif" Text="Cancel All" CssClass="rightAligned" Visible="false">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="SaveAll" DisabledImageUrl="~/Images/Save_d.gif"
                            ImageUrl="~/Images/Save.gif" Text="Save All" CssClass="rightAligned" Visible="false">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="EditAll" DisabledImageUrl="~/Images/Edit3_d.gif"
                            ImageUrl="~/Images/Edit3.gif" Text="Edit All" CssClass="rightAligned" Visible="false">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Retrieve" DisabledImageUrl="~/Images/Retrieve1_d.gif"
                            ImageUrl="~/Images/Retrieve1.gif" Text="Retrieve" CssClass="rightAligned">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Back" DisabledImageUrl="~/Images/BackButton.gif"
                            ImageUrl="~/Images/BackButton.gif" Text="Back">
                        </telerik:RadToolBarButton>
                    </Items>
                </telerik:RadToolBar>
            </div>
            <div id="pageHeader">
                <asp:Label ID="lblPageHeader" runat="server" Text="E-Comm Turn-In Meeting" />
            </div>
            <bonton:MessagePanel ID="mpeCommTurnInMaint" runat="server" />
            <%--<div id="DiveComFrame" style="display: none">
                <iframe id="IframeWebcat" frameborder="0" width="940px" height="640px" scrolling="auto">
                </iframe>
            </div>
            <rts:Modal ID="Modalwebcats" runat="server" CancelControlID="btnpopcomCancel" OkControlID="btnOK"
                TargetControlID="BtnComtrgt" PopupControlID="DiveComFrame" Drag="True" DynamicServicePath=""
                Enabled="True" />--%>
        </telerik:RadPane>
        <telerik:RadPane ID="RadPane1" runat="server">
            <asp:Panel ID="pnlTabStrip" runat="server" CssClass="pageTabStrip" Style="margin: 0;">
                <telerik:RadTabStrip ID="rtsTurnInMeet" runat="server" MultiPageID="rmpTurnInMeet"
                    SelectedIndex="0" OnClientTabSelecting="OnClientRadTabStripClick">
                    <Tabs>
                        <telerik:RadTab runat="server" Text="eMM" PageViewID="pveMM" Font-Bold="True" 
                            Selected="True"/>
                        <telerik:RadTab runat="server" Text="Creative Coord" PageViewID="pvCC" 
                            Font-Bold="True" />
                        <telerik:RadTab runat="server" Text="Copy Writer" PageViewID="pvCopyWriter" 
                            Font-Bold="True" />
                    </Tabs>
                </telerik:RadTabStrip>
            </asp:Panel>
            <telerik:RadMultiPage ID="rmpTurnInMeet" SelectedIndex="0" runat="server" Height="92%"
                BorderWidth="1">
                <telerik:RadPageView ID="pveMM" runat="server" Height="98%">
                    <table id="tblFloodEMM" runat="server" visible="false">
                        <tr>
                            <td>
                                <asp:ImageButton ID="btnResetFloodEMM" runat="server" ToolTip="Reset" ImageUrl="~/Images/Reset.gif" />
                            </td>
                            <td>
                                <telerik:RadButton ID="btnFloodEMM" runat="server" Text="Flood" Font-Bold="true" Width="60px"
                                    Height="25px" OnClientClicking="OnClientFloodClick" />
                            </td>
                            <td>
                                <asp:HiddenField ID="hfFloodWebCatCde" runat="server" Value='0' />
                                <telerik:RadTextBox ID="rtxtFloodWebCategories" runat="server" EmptyMessageStyle-Font-Italic="true"
                                    Width="173px" EmptyMessage="Primary Web Category" ToolTip="Primary Web Category">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadButton ID="btnOpenWebCat" runat="server" Width="18px" Image-ImageUrl="~/Images/Add.gif"
                                    Height="18px" OnClientClicking="OpenWebCategoriesForAll" ToolTip="Add Primary Web Category">
                                </telerik:RadButton>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="rtxtFloodFrndPrdDesc" ToolTip="Friendly Product Description"
                                    EmptyMessageStyle-Font-Italic="true" EmptyMessage="Friendly Product Description"
                                    runat="server" MaxLength="255" Width="180px">
                                    <ClientEvents OnBlur="ValidateText" />
                                </telerik:RadTextBox>
                                <bonton:ToolTipValidator ID="valFloodFrndPrdDesc" runat="server" ControlToEvaluate="rtxtFloodFrndPrdDesc"
                                    ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                    ValidationGroup="ValidateFloodEMM" />
                            </td>
                            <td>
                                <telerik:RadTextBox ID="rtxtFloodFriendlyColor" runat="server" ToolTip="Friendly Color"
                                    EmptyMessageStyle-Font-Italic="true" EmptyMessage="Friendly Color" Width="84px"
                                    MaxLength="255">
                                    <ClientEvents OnBlur="ValidateText" />
                                </telerik:RadTextBox>
                                <bonton:ToolTipValidator ID="valFriendlyColor" runat="server" ControlToEvaluate="rtxtFloodFriendlyColor"
                                    ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                    ValidationGroup="ValidateFloodEMM" />
                            </td>
                            <td>
                                <telerik:RadComboBox ID="rcbFloodSizeCategory" runat="server" Width="105" MarkFirstMatch="true"
                                    ToolTip="Size Category" EmptyMessageStyle-Font-Italic="true" EmptyMessage="Size Category"
                                    OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista" AllowCustomText="false"
                                    AppendDataBoundItems="False" Filter="StartsWith">
                                    <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="rtxtFloodEMMNotes" runat="server" ToolTip="EMM Notes" EmptyMessageStyle-Font-Italic="true"
                                    EmptyMessage="EMM Notes" Width="160px" TextMode="SingleLine" MaxLength="2000">
                                    <ClientEvents OnBlur="ValidateAsciiChars" />
                                </telerik:RadTextBox>
                                <bonton:ToolTipValidator ID="valFloodEMMNotes" runat="server" ControlToEvaluate="rtxtFloodEMMNotes"
                                    ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                    ValidationGroup="ValidateFloodEMM" />
                            </td>
                        </tr>
                    </table>
                    <telerik:RadGrid ID="grdEMM" runat="server" SkinID="CenteredWithScroll" AllowPaging="true"
                        Visible="false" Height="95%" AutoGenerateColumns="false" AllowMultiRowSelection="True"
                        ShowFooter="false" AllowMultiRowEdit="true">
                        <ClientSettings>
                            <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" SaveScrollPosition="true" />
                            <ClientEvents OnCommand="OnGridCommandClickEMM" />
                        </ClientSettings>
                        <MasterTableView DataKeyNames="ISN,turnInMerchID,MerchID,RemoveMerchFlag,CategoryCode,WebCatgyList,LabelId,BrandId,PrimaryThumbnailUrl,PrimaryMediumUrl,SecondaryMediumUrl"
ClientDataKeyNames="ISN,turnInMerchID,MerchID,RemoveMerchFlag,CategoryCode,WebCatgyList,LabelId,BrandId, PrimaryThumbnailUrl,PrimaryMediumUrl,SecondaryMediumUrl" EditMode="InPlace">
                            <GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="VendorStyleSequence" SortOrder="Ascending">
                                        </telerik:GridGroupByField>
                                        <telerik:GridGroupByField FieldName="VendorStyleNumber" SortOrder="None"></telerik:GridGroupByField>
                                    </GroupByFields>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldAlias="VendorStyleNumber" FieldName="VendorStyleNumber"
                                            HeaderText="Vendor Style"></telerik:GridGroupByField>
                                    </SelectFields>
                                </telerik:GridGroupByExpression>
                            </GroupByExpressions>
                            <NoRecordsTemplate>
                                no records retrieved</NoRecordsTemplate>
                            <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                            <Columns>
                                <telerik:GridClientSelectColumn UniqueName="selColumn">
                                    <ItemStyle Width="30px" />
                                    <HeaderStyle Width="30px" />
                                </telerik:GridClientSelectColumn>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                                    EditImageUrl="~/Images/Edit1.gif" EditText="Edit" CancelImageUrl="~/Images/Delete.gif"
                                    CancelText="Are you sure you want cancel? Unsaved data will be lost" UpdateImageUrl="~/Images/CheckMark.gif"
                                    UpdateText="Update">
                                    <HeaderStyle Width="60px" />
                                    <ItemStyle Width="60px" />
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" UniqueName="DeleteColumn"
                                    ImageUrl="~/Images/Delete.gif">
                                    <HeaderStyle HorizontalAlign="Center" Width="40px" />
                                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                                </telerik:GridButtonColumn>
                                 <telerik:GridTemplateColumn UniqueName="Thumbnail" HeaderText="Thumbnail">
                                    <HeaderStyle HorizontalAlign="Center" Width="108" />
                                    <ItemStyle HorizontalAlign="Center" Width="108" />
                                    <ItemTemplate>
                                     <asp:ImageButton ID="ibThumbnail" runat="server" ImageUrl='<%# Eval("PrimaryThumbnailUrl") %>' AlternateText='<%# Eval("MerchId") %>'
                                    Width="100px" Height="80px" ImageAlign="Middle" OnClientClick="return eMMShowImage(this);" ToolTip='<%# Eval("MerchId") %>' />
                                    </ItemTemplate>
                                      </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="ISN" HeaderText="ISN" UniqueName="ISN" Visible="false"
                                    ReadOnly="true" HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Center" Width="100px" />
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="TurnInMerchId" HeaderText="Line Id" UniqueName="LineId"
                                    Visible="true" ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Center" Width="60px" />
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                </telerik:GridBoundColumn>
                              <telerik:GridTemplateColumn HeaderText="Web Categories" UniqueName="WebCategories" Visible="true"  ItemStyle-Wrap= "true" ItemStyle-Width = "180px" Display ="true" Resizable = "true">
                                    <HeaderStyle HorizontalAlign="Center" Width="180px" Font-Bold="True"  />
                                    <ItemStyle HorizontalAlign="Center" Width="180px" />
                                    <ItemTemplate>
                                       <asp:Literal runat="server" ID="ltrPrimaryWebCategory" Visible ="true"></asp:Literal>
                                      </ItemTemplate>
                                <EditItemTemplate>
                                 <asp:HiddenField ID="hfCategoryCode" runat="server" Value='<%#Server.HTMLEncode( Eval("CategoryCode")) %>' />
                                 <telerik:RadTextBox ID="rtxtWebCategories" runat="server" Width="200px"  Wrap ="true" Visible="true" Display="true" />
                                  </EditItemTemplate>
                               </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Label" UniqueName="Label">
                                    <HeaderStyle HorizontalAlign="Center" Width="90px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="90px" />
                                    <ItemTemplate>
                                        <%# Server.HtmlEncode(Eval("Label"))%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="hfLabel" runat="server" Value='<%#Server.HTMLEncode( Eval("LabelId")) %>' />
                                        <telerik:RadComboBox ID="rcbLabel" runat="server" Width="100" MarkFirstMatch="true"
                                            OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista" AllowCustomText="false"
                                            AppendDataBoundItems="False" Filter="Contains">
                                            <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="LabelID" HeaderText="LabelID" UniqueName="LabelID"
                                    Visible="false" HtmlEncode="true">
                                    <HeaderStyle />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Friendly Prod Desc" UniqueName="FriendlyProdDesc">
                                    <HeaderStyle HorizontalAlign="Center" Width="160px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="160px" />
                                    <ItemTemplate>
                                        <%# Server.HtmlEncode(Eval("FriendlyProdDesc"))%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="rtxtFriendlyProdDesc" runat="server" Text='<%# Server.HTMLEncode( CStr(Eval("FriendlyProdDesc")))%>'
                                            Width="160px" MaxLength="255">
                                            <ClientEvents OnBlur="ValidateText" />
                                        </telerik:RadTextBox>
                                        <bonton:ToolTipValidator ID="valFriendlyProdDesc" runat="server" ControlToEvaluate="rtxtFriendlyProdDesc"
                                            ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                            ValidationGroup="Update" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="ISNDesc" HeaderText="Style Desc" UniqueName="ISNDesc"
                                    ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Center" Width="180px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="180px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Friendly Color" UniqueName="FriendlyColor">
                                    <HeaderStyle HorizontalAlign="Center" Width="60px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    <ItemTemplate>
                                        <%# Server.HtmlEncode(Eval("FriendlyColor"))%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="rtxtFriendlyColor" runat="server" Text='<%# Server.HTMLEncode(Eval("FriendlyColor")) %>'
                                            Width="60px" MaxLength="255">
                                            <ClientEvents OnBlur="ValidateText" />
                                        </telerik:RadTextBox>
                                        <bonton:ToolTipValidator ID="valFriendlyColor" runat="server" ControlToEvaluate="rtxtFriendlyColor"
                                            ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                            ValidationGroup="Update" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Size Category" UniqueName="SizeCategory">
                                    <HeaderStyle HorizontalAlign="Center" Width="100px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    <ItemTemplate>
                                        <%# Server.HtmlEncode(Eval("SizeCategory"))%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="hfSizeCategory" runat="server" Value='<%# Eval("SizeCategory") %>' />
                                        <telerik:RadComboBox ID="rcbSizeCategory" runat="server" Width="100" MarkFirstMatch="true"
                                            OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista" AllowCustomText="false"
                                            AppendDataBoundItems="False" Filter="StartsWith">
                                            <CollapseAnimation Type="OutQuint" Duration="200"></CollapseAnimation>
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="IsReserve" HeaderText="IsReserve" UniqueName="IsReserve"
                                    Visible="false" HtmlEncode="true">
                                    <HeaderStyle />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="BuyerId" HeaderText="Buyer ID" UniqueName="BuyerId"
                                    Visible="false" HtmlEncode="true">
                                    <HeaderStyle />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DeptID" HeaderText="Dept ID" UniqueName="DeptID"
                                    Visible="false" HtmlEncode="true">
                                    <HeaderStyle />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VendorStyleNumber" HeaderText="Vendor Style"
                                    UniqueName="VendorStyleNumber" ReadOnly="true" HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="80px" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FeatureSwatch" HeaderText="Feature / Render/ Swatch"
                                    UniqueName="FeatureSwatch" ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Center" Width="84px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="84px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ImageKindDescription" HeaderText="Image Kind"
                                    UniqueName="ImageKind" ReadOnly="true" HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="60px" />
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="SampleSize" HeaderText="Sample Size" UniqueName="SampleSize"
                                    ReadOnly="true" HtmlEncode="true" Visible="false">
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="60px" />
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Color Correct?" UniqueName="ColorCorrect"
                                    ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="60px" />
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    <ItemTemplate>
                                        <%# Server.HtmlEncode(IIf(Eval("ColorCorrect").ToString = "Y", "Yes", "No"))%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="RoutefromAd" HeaderText="Route From Ad" UniqueName="RoutefromAd"
                                    Visible="true" HtmlEncode="true" ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Center" Width="60px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="MerchantNotes" HeaderText="Merchant Notes" UniqueName="MerchantNotes"
                                    ReadOnly="true" HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="80px" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="EMM Notes" UniqueName="EMMNotes">
                                    <HeaderStyle HorizontalAlign="Center" Width="160px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="160px" />
                                    <ItemTemplate>
                                        <%# Server.HtmlEncode(Eval("EMMNotes"))%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="rtxtEMMNotes" runat="server" Text='<%# CStr(Eval("EMMNotes"))%>'
                                            Width="160px" TextMode="MultiLine" MaxLength="2000">
                                            <ClientEvents OnBlur="ValidateAsciiChars" />
                                        </telerik:RadTextBox>
                                        <bonton:ToolTipValidator ID="valEMMNotes" runat="server" ControlToEvaluate="rtxtEMMNotes"
                                            ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                            ValidationGroup="Update" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Follow Up Flag" UniqueName="FollowUpFlag">
                                    <HeaderStyle HorizontalAlign="Center" Width="50px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="cbxFollowUpFlag" Checked='<%# IF(Eval("EMMFollowUpFlag")="Y",true,false)%>'
                                            Enabled="true" AutoPostBack="true" OnCheckedChanged="cbxEMMFollowUpFlag_CheckedChanged" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox runat="server" ID="cbxFollowUpFlag" Checked='<%# IF(Eval("EMMFollowUpFlag")="Y",true,false)%>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </telerik:RadPageView>
                <telerik:RadPageView ID="pvCC" runat="server" Height="98%">
                    <table id="tblFloodCC" runat="server" visible="false">
                        <tr>
                            <td>
                                <asp:ImageButton ID="btnResetFloodCC" runat="server" ToolTip="Reset" ImageUrl="~/Images/Reset.gif" />
                            </td>
                            <td>
                                <telerik:RadButton ID="btnFloodCC" runat="server" Text="Flood" Font-Bold="true" Width="60px"
                                    Height="25px" OnClientClicking="OnClientFloodClick" />
                            </td>
                            <td>
                                <telerik:RadTextBox ID="rtxtFloodImageNotes" runat="server" ToolTip="Image Notes"
                                    EmptyMessageStyle-Font-Italic="true" EmptyMessage="Image Notes"
                                    Width="160px" TextMode="SingleLine" MaxLength="35">
                                    <ClientEvents OnBlur="ValidateAsciiChars" />
                                </telerik:RadTextBox>
                                <bonton:ToolTipValidator ID="ToolTipValidator1" runat="server" ControlToEvaluate="rtxtFloodImageNotes"
                                    ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                    ValidationGroup="ValidateFloodImage" />
                            </td>
                            <td>
                                <telerik:RadTextBox ID="rtxtFloodStylingNotes" runat="server" ToolTip="Styling Notes"
                                    EmptyMessageStyle-Font-Italic="true" EmptyMessage="Styling Notes" Width="160px"
                                    TextMode="SingleLine" MaxLength="80">
                                    <ClientEvents OnBlur="ValidateAsciiChars" />
                                </telerik:RadTextBox>
                                <bonton:ToolTipValidator ID="ToolTipValidator2" runat="server" ControlToEvaluate="rtxtFloodStylingNotes"
                                    ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                    ValidationGroup="ValidateFloodImage" />
                            </td>
                            <td>
                                <telerik:RadComboBox ID="cmbFloodFeatureRenderSwatch" runat="server" Width="160px" OnClientSelectedIndexChanged="OnChangeFRSFloodCC"
                                    Height="100px" DropDownWidth="100px" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                    AllowCustomText="false" EmptyMessage="Feature/Render/Swatch" ToolTip="Feature/Render/Swatch">
                                    <CollapseAnimation Duration="200" Type="OutQuint" />
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="rcbFloodImgType" AppendDataBoundItems="False" runat="server"
                                    Width="100px" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false"
                                    EmptyMessage="On/Off Figure" ToolTip="On/Off Figure" OnClientSelectedIndexChanged="OnChangeOnOffFlood">
                                    <CollapseAnimation Duration="200" Type="OutQuint" />
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="rcbFloodModelCategory" AppendDataBoundItems="False" runat="server"
                                    Width="110px" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false"
                                    EmptyMessage="Model Category" ToolTip="Model Category">
                                    <CollapseAnimation Duration="200" Type="OutQuint" />
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="rcbFloodAlternateView" AppendDataBoundItems="False" runat="server"
                                    Width="105px" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false"
                                    EmptyMessage="Alternate View" ToolTip="Alternate View">
                                    <CollapseAnimation Duration="200" Type="OutQuint" />
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                    </table>
                    <telerik:RadGrid ID="grdCC" runat="server" SkinID="CenteredWithScroll" AllowPaging="true" AllowMultiRowEdit="true"
                        AllowMultiRowSelection="True" Height="95%" Visible="false" ShowFooter="false">
                        <ClientSettings>
                            <Resizing AllowColumnResize="true" />
                            <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" SaveScrollPosition="true" />
                            <ClientEvents OnCommand="OnGridCommandClickCC" />
                        </ClientSettings>
                        <MasterTableView EditMode="InPlace" EnableHeaderContextMenu="true"                                         DataKeyNames="ISN,turnInMerchID,ColorCode,MerchID,RemoveMerchFlag,PageNumber,WebCatgyList,CategoryCode,RoutefromAd,SampleSize,PrimaryThumbnailUrl,PrimaryMediumUrl,SecondaryMediumUrl"                                       ClientDataKeyNames="ISN,turnInMerchID,ColorCode,MerchID,RemoveMerchFlag,PageNumber,WebCatgyList,CategoryCode,RoutefromAd,SampleSize,PrimaryThumbnailUrl,PrimaryMediumUrl,SecondaryMediumUrl">
                            <GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="VendorStyleSequence" SortOrder="Ascending">
                                        </telerik:GridGroupByField>
                                        <telerik:GridGroupByField FieldName="VendorStyleNumber" SortOrder="None"></telerik:GridGroupByField>
                                    </GroupByFields>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldAlias="VendorStyleNumber" FieldName="VendorStyleNumber"
                                            HeaderText="Vendor Style"></telerik:GridGroupByField>
                                    </SelectFields>
                                </telerik:GridGroupByExpression>
                            </GroupByExpressions>
                            <NoRecordsTemplate>
                                no records retrieved</NoRecordsTemplate>
                            <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                            <Columns>
                                <telerik:GridClientSelectColumn UniqueName="selColumn">
                                    <ItemStyle Width="30px" />
                                    <HeaderStyle Width="30px" />
                                </telerik:GridClientSelectColumn>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn" 
                                    EditImageUrl="~/Images/Edit1.gif" EditText="Edit" CancelImageUrl="~/Images/Delete.gif"
                                    CancelText="Are you sure you want cancel? Unsaved data will be lost." UpdateImageUrl="~/Images/CheckMark.gif"
                                    UpdateText="Update">
                                    <HeaderStyle HorizontalAlign="Center" Width="60px" />
                                    <ItemStyle Width="60px" />
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" UniqueName="DeleteColumn"
                                    ImageUrl="~/Images/Delete.gif">
                                    <HeaderStyle HorizontalAlign="Center" Width="40px" />
                                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                                </telerik:GridButtonColumn>
                                <telerik:GridBoundColumn DataField="ISN" HeaderText="" UniqueName="ISN" Visible="false">
                                    <HeaderStyle />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="IsReserve" HeaderText="IsReserve" UniqueName="IsReserve"
                                    Visible="false" HtmlEncode="true">
                                    <HeaderStyle />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="turnInMerchID" HeaderText="TurnInMerchID" UniqueName="turnInMerchID" Visible="false">
                                    <HeaderStyle HorizontalAlign="Center" Width="60" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                      </telerik:GridBoundColumn>  
                                           
                                <telerik:GridTemplateColumn UniqueName="Thumbnail" HeaderText="Thumbnail">
                                    <HeaderStyle HorizontalAlign="Center" Width="108" />
                                    <ItemStyle HorizontalAlign="Center" Width="108" />
                                    <ItemTemplate>                                        
                                        <asp:ImageButton ID="ibThumbnail"  runat="server" ImageUrl='<%# Eval("PrimaryThumbnailUrl") %>' AlternateText='<%# Eval("PrimaryThumbnailUrlAltText") %>'  ToolTip='<%# Eval("MerchID") %>' Width="100px" Height="80px" OnClientClick="return ccShowImage(this);" ImageAlign="Middle" />
                                      <telerik:RadTextBox ID="hfMerchId" runat="server" Width="1" Text='<%# Eval("MerchID") %>' Display="false" />
                                     </ItemTemplate>
                                    <EditItemTemplate>
                                    <asp:ImageButton ID = "ibThumbnail" runat = "server"  ImageUrl='<%# Eval("PrimaryThumbnailUrl") %>' AlternateText='<%# Eval("PrimaryThumbnailUrlAltText") %>' ToolTip='<%# Eval("MerchID") %>' OnClientClick = "PickSample(); return false;" Width="100px" Height="80px">
                                     </asp:ImageButton>
                                    <telerik:RadTextBox ID="hfMerchId" runat="server" Width="1" Text='<%# Eval("MerchID") %>' Display="false" />
                                    <telerik:RadTextBox ID="hrtbImageKind" runat="server" Width="1" Text='<%#Server.HTMLEncode( Eval("ImageKindCode")) %>' Display="false" />
                                    <bonton:ToolTipValidator ID="valImage" runat="server" ErrorMessage="Image Error." OnServerValidate="valImageKind_ServerValidate" ControlToEvaluate = "hrtbImageKind" ValidationGroup = "Update" />  
                                     </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="TurnInMerchId" HeaderText="Line Id" UniqueName="LineId"
                                    Visible="true" ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Center" Width="60px" />
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                </telerik:GridBoundColumn>
                                 <telerik:GridTemplateColumn HeaderText="Web Categories" UniqueName="WebCategories">
                                    <HeaderStyle HorizontalAlign="Center" Width="180px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="180px" />
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="ltrPrimaryWebCategory"></asp:Literal>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="Label" HeaderText="Label" UniqueName="Label"
                                    Visible="true" ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Center" Width="90px" />
                                    <ItemStyle HorizontalAlign="Center" Width="90px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ISNDesc" HeaderText="Style Desc" UniqueName="ISNDesc"
                                    ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Center" Width="180px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="180px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FriendlyProdDesc" HeaderText="Friendly Product Desc"
                                    UniqueName="FriendlyProdDesc" ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Center" Width="80px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VendorStyleNumber" HeaderText="Vendor Style"
                                    UniqueName="VendorStyleNumber" ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Center" Width="80" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="80" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn  HeaderText="Friendly Color" UniqueName="FriendlyColor"
                                    ReadOnly="false">
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="96px" />
                                    <ItemStyle HorizontalAlign="Center" Width="96px" />
                                    <ItemTemplate>
                                    <%# Eval("FriendlyColor")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>                                    
                                    <telerik:RadTextBox ID="radtxtFriendlyColor" runat="server" Width="110px" MaxLength="255" Text='<%# Eval("FriendlyColor") %>'/> 
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Image Group" UniqueName="ImageGrp">
                                    <HeaderStyle HorizontalAlign="Center" Width="60px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    <ItemTemplate>
                                        <%# Eval("ImageGrp")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadNumericTextBox ID="rtxtImageGrp" runat="server" Text='<%# CStr(Eval("ImageGrp"))%>'
                                            Width="80px" MaxLength="6" MinValue="0" MaxValue="999999">
                                            <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                        </telerik:RadNumericTextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Image Desc" UniqueName="ImageDesc">
                                    <HeaderStyle HorizontalAlign="Center" Width="160px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="160px" />
                                    <ItemTemplate>
                                        <%# Eval("ImageDesc")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="rtxtImageDesc" runat="server" Text='<%# CStr(Eval("ImageDesc"))%>'
                                            Width="160px" MaxLength="30">
                                            <ClientEvents OnBlur="ValidateAsciiChars" />
                                        </telerik:RadTextBox>
                                        <bonton:ToolTipValidator ID="valImageDesc" runat="server" ControlToEvaluate="rtxtImageDesc"
                                            ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                            ValidationGroup="Update" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Image Notes" UniqueName="ImageNotes">
                                    <HeaderStyle HorizontalAlign="Center" Width="160px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="160px" />
                                    <ItemTemplate>
                                        <%# Eval("ImageNotes")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="rtxtImageNotes" runat="server" Text='<%# CStr(Eval("ImageNotes"))%>'
                                            Width="160px" TextMode="MultiLine" MaxLength="35">
                                            <ClientEvents OnBlur="ValidateAsciiChars" />
                                        </telerik:RadTextBox>
                                        <bonton:ToolTipValidator ID="valImageNotes" runat="server" ControlToEvaluate="rtxtImageNotes"
                                            ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                            ValidationGroup="Update" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Styling Notes" UniqueName="StylingNotes">
                                    <HeaderStyle HorizontalAlign="Center" Width="160px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="160px" />
                                    <ItemTemplate>
                                        <%# Eval("StylingNotes")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="rtxtStylingNotes" runat="server" Text='<%# CStr(Eval("StylingNotes"))%>'
                                            Width="160px" TextMode="MultiLine" MaxLength="80">
                                            <ClientEvents OnBlur="ValidateAsciiChars" />
                                        </telerik:RadTextBox>
                                        <bonton:ToolTipValidator ID="valStylingNotes" runat="server" ControlToEvaluate="rtxtStylingNotes"
                                            ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                            ValidationGroup="Update" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Image Kind" UniqueName="ImageKind">
                                    <HeaderStyle HorizontalAlign="Center" Width="60px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    <ItemTemplate>
                                        <%# Server.HtmlEncode(Eval("ImageKindDescription"))%>
                                        <asp:HiddenField ID="hfImageKind" runat="server" Value='<%#Server.HTMLEncode( Eval("ImageKindCode")) %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="hfImageKind" runat="server" Value='<%#Server.HTMLEncode( Eval("ImageKindCode")) %>' />
                                        <telerik:RadComboBox ID="rcbImageKind" runat="server" Width="60px" DropDownWidth="120"
                                            Height="100px" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista" 
                                            AllowCustomText="false">
                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                        </telerik:RadComboBox>                                                                  
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn DataField="FeatureSwatch" HeaderText="Feature / Render / Swatch"
                                    UniqueName="FeatureSwatch">
                                    <HeaderStyle HorizontalAlign="Center" Width="80px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                    <ItemTemplate>
                                        <%# Server.HtmlEncode(Eval("FeatureSwatch"))%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="hfFeatureRenderSwatch" runat="server" Value='<%# Eval("FeatureSwatch") %>' />
                                        <telerik:RadComboBox ID="rcbFeatureRenderSwatch" runat="server" Width="70px" Height="100px"
                                            DropDownWidth="100px" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler"
                                            AllowCustomText="false">
                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="On/ Off Figure" UniqueName="OnOff">
                                    <HeaderStyle HorizontalAlign="Center" Width="80px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                    <ItemTemplate>
                                        <%# Eval("OnOff")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="hfOnOff" runat="server" Value='<%# Eval("OnOff") %>' />
                                        <telerik:RadComboBox ID="rcbOnOff" runat="server" Width="80px" Height="100px" MarkFirstMatch="true"
                                            OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista" AllowCustomText="false">
                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Model Category" UniqueName="ModelCategory">
                                    <HeaderStyle HorizontalAlign="Center" Width="80px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                    <ItemTemplate>
                                        <%# Eval("ModelCategory")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="hfModelCategory" runat="server" Value='<%# Eval("ModelCategory") %>' />
                                        <telerik:RadComboBox ID="rcbModelCategory" runat="server" Width="80px" Height="180px"
                                            MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista"
                                            AllowCustomText="false" DropDownWidth="110px">
                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                        </telerik:RadComboBox>
                                        <bonton:ToolTipValidator ID="valModelCategory" runat="server" ControlToEvaluate="rcbModelCategory"
                                            OnServerValidate="valModelCategory_ServerValidate" ValidationGroup="Update" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Alternate View" UniqueName="AlternateView">
                                    <HeaderStyle HorizontalAlign="Center" Width="70px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# Eval("AltView")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="hfAlternateView" runat="server" Value='<%# Eval("AltView") %>' />
                                        <telerik:RadComboBox ID="rcbAlternateView" runat="server" Width="70px" DropDownWidth="120px"
                                            Height="100px" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" AllowCustomText="false">
                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="pickup" HeaderText="P/U" UniqueName="pickup"
                                    ReadOnly="true" HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="60px" />
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Pickup Image ID" UniqueName="PickupImageID">
                                    <HeaderStyle HorizontalAlign="Center" Width="80px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                    <ItemTemplate>
                                        <%# If(Eval("PickupImageID") = "0", "", Eval("PickupImageID"))%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadNumericTextBox ID="rtxtPickupImageID" runat="server" Text='<%# If(Eval("PickupImageID") = "0", "", Eval("PickupImageID"))%>'
                                            Width="80px" MaxLength="6" MinValue="0" MaxValue="999999">
                                            <NumberFormat GroupSeparator="" DecimalDigits="0" />
                                        </telerik:RadNumericTextBox>
                                        <bonton:ToolTipValidator ID="valPickupImageID" runat="server" ControlToEvaluate="rtxtPickupImageID"
                                            OnServerValidate="valPickupImageID_ServerValidate" ValidationGroup="Update" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Color Correct" UniqueName="ColorCorrect">
                                    <HeaderStyle HorizontalAlign="Center" Width="80px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                    <ItemTemplate>
                                        <%# Eval("ColorCorrect")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox ID="rcbColorCorrect" runat="server" Width="60px" Height="100px"
                                            MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista"
                                            AllowCustomText="false" Text='<%# Eval("ColorCorrect")%>'>
                                            <Items>
                                                <telerik:RadComboBoxItem Text="" Value=""></telerik:RadComboBoxItem>
                                                <telerik:RadComboBoxItem Text="Y" Value="Y"></telerik:RadComboBoxItem>
                                                <telerik:RadComboBoxItem Text="N" Value="N"></telerik:RadComboBoxItem>
                                            </Items>
                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Rush Sample" UniqueName="HotListCDE">
                                    <HeaderStyle HorizontalAlign="Center" Width="80px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                    <ItemTemplate>
                                        <%# Eval("HotListCDE")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox ID="rcbHotItem" runat="server" Width="80px" Text='<%# Eval("HotListCDE")%>'
                                            Height="100px" MarkFirstMatch="true" OnClientBlur="OnClientBlurHandler" class="RadComboBox_Vista"
                                            AllowCustomText="false">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Y" Value="Y"></telerik:RadComboBoxItem>
                                                <telerik:RadComboBoxItem Text="N" Value="N"></telerik:RadComboBoxItem>
                                            </Items>
                                            <CollapseAnimation Duration="200" Type="OutQuint" />
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="FeatureID" HeaderText="" UniqueName="FeatureID"
                                    Visible="false">
                                    <HeaderStyle />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="MerchantNotes" HeaderText="Merchant Notes" UniqueName="MerchantNotes"
                                    ReadOnly="true" HtmlEncode="true">
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="80px" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="RoutefromAd" HeaderText="Route From Ad" UniqueName="RoutefromAd"
                                    Visible="true" HtmlEncode="true" ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Center" Width="80px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                               <telerik:GridTemplateColumn HeaderText="Follow Up Flag" UniqueName="FollowUpFlag">
                                    <HeaderStyle HorizontalAlign="Center" Width="50px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="cbxFollowUpFlag" Checked='<%# IF(Eval("CCFollowUpFlag")="Y",true,false)%>'
                                            Enabled="true" AutoPostBack="true" OnCheckedChanged="cbxCCFollowUpFlag_CheckedChanged" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                       <asp:CheckBox runat="server" ID="cbxFollowUpFlag" Checked='<%# IF(Eval("CCFollowUpFlag")="Y",true,false)%>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </telerik:RadPageView>
                <telerik:RadPageView ID="pvCopyWriter" runat="server" Height="98%">
                    <telerik:RadGrid ID="grdCopyWriter" runat="server" SkinID="CenteredWithScroll" AllowPaging="true" AllowMultiRowEdit="true"
                        AllowMultiRowSelection="True" Height="100%" Visible="false" ShowFooter="false">
                        <ClientSettings>
                            <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" SaveScrollPosition="true" />
                            <ClientEvents OnCommand="OnGridCommandClickCopyWriter" />
                        </ClientSettings>
                        <MasterTableView EditMode="InPlace"
                                        DataKeyNames="ISN,turnInMerchID,RemoveMerchFlag,PrimaryThumbnailUrl,PrimaryMediumUrl,SecondaryMediumUrl"
                                        ClientDataKeyNames="ISN, turnInMerchID, RemoveMerchFlag,PrimaryThumbnailUrl,PrimaryMediumUrl,SecondaryMediumUrl">
                            <GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="VendorStyleSequence" SortOrder="Ascending">
                                        </telerik:GridGroupByField>
                                        <telerik:GridGroupByField FieldName="VendorStyleNumber" SortOrder="None"></telerik:GridGroupByField>
                                    </GroupByFields>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldAlias="VendorStyleNumber" FieldName="VendorStyleNumber"
                                            HeaderText="Vendor Style"></telerik:GridGroupByField>
                                    </SelectFields>
                                </telerik:GridGroupByExpression>
                            </GroupByExpressions>
                            <NoRecordsTemplate>
                                no records retrieved</NoRecordsTemplate>
                            <FooterStyle HorizontalAlign="Right" Font-Bold="True" />
                            <Columns>
                                <telerik:GridClientSelectColumn UniqueName="selColumn">
                                    <ItemStyle Width="30px" />
                                    <HeaderStyle Width="30px" />
                                </telerik:GridClientSelectColumn>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                                    EditImageUrl="~/Images/Edit1.gif" EditText="Edit" CancelImageUrl="~/Images/Delete.gif"
                                    CancelText="Are you sure you want cancel? Unsaved data will be lost." UpdateImageUrl="~/Images/CheckMark.gif"
                                    UpdateText="Update">
                                    <HeaderStyle Width="60px" />
                                    <ItemStyle Width="60px" />
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" UniqueName="DeleteColumn"
                                    ImageUrl="~/Images/Delete.gif">
                                    <HeaderStyle HorizontalAlign="Center" Width="30px" />
                                    <ItemStyle HorizontalAlign="Center" Width="30px" />
                                </telerik:GridButtonColumn>
                                <telerik:GridBoundColumn DataField="ISN" HeaderText="" UniqueName="ISN" Visible="false">
                                    <HeaderStyle />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="IsReserve" HeaderText="IsReserve" UniqueName="IsReserve"
                                    Visible="false" HtmlEncode="true">
                                    <HeaderStyle />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="turnInMerchID" HeaderText="Turn In Merch ID"
                                    UniqueName="turnInMerchID" Visible="false">
                                    <HeaderStyle />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Follow Up Flag" UniqueName="FollowUpFlag">
                                    <HeaderStyle HorizontalAlign="Center" Width="50px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="cbxFollowUpFlag" Checked='<%# IF(Eval("CWFollowUpFlag")="Y",true,false)%>'
                                            Enabled="true" AutoPostBack="true" OnCheckedChanged="cbxCWFollowUpFlag_CheckedChanged" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox runat="server" ID="cbxFollowUpFlag" Checked='<%# IF(Eval("CWFollowUpFlag")="Y",true,false)%>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Thumbnail" HeaderText="Thumbnail">
                                    <HeaderStyle HorizontalAlign="Center" Width="108" />
                                    <ItemStyle HorizontalAlign="Center" Width="108" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibThumbnail" runat="server" ImageUrl='<%# Eval("PrimaryThumbnailUrl") %>' AlternateText='<%# Eval("MerchId") %>'
                                                            Width="100px" Height="80px" OnClientClick="return copyWriterShowImage(this);" ImageAlign="Middle" ToolTip='<%# Eval("MerchId") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="TurnInMerchId" HeaderText="Line Id" UniqueName="LineId"
                                    Visible="true" ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Center" Width="60px" />
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn DataField="FeatureSwatch" HeaderText="Feature / Render / Swatch"
                                    UniqueName="FeatureSwatch">
                                    <HeaderStyle HorizontalAlign="Center" Width="60px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="60px" />
                                    <ItemTemplate>
                                        <%# Server.HtmlEncode(Eval("FeatureSwatch"))%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="VendorStyleNumber" HeaderText="Vendor Style"
                                    ReadOnly="true" UniqueName="VendorStyleNumber">
                                    <HeaderStyle HorizontalAlign="Center" Width="80px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Label" HeaderText="Label" ReadOnly="true" UniqueName="Label">
                                    <HeaderStyle HorizontalAlign="Center" Width="80px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FriendlyProdDesc" HeaderText="Friendly Prod Desc"
                                    ReadOnly="true" UniqueName="FriendlyProdDesc">
                                    <HeaderStyle HorizontalAlign="Center" Width="100px" Font-Bold="True" />
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FeaturesBenefits" HeaderText="Features/Benefits"
                                    UniqueName="FeaturesBenefits" ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="160px" />
                                    <ItemStyle HorizontalAlign="Center" Width="160px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Copy Notes" UniqueName="CpyNotes">
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="250px" />
                                    <ItemStyle HorizontalAlign="Left" Width="250px" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblCopyNotes" runat="server" Width="250px"><%# Eval("CpyNotes").ToString.Replace(",", "<br />")%></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="rtxtCpyNotes" runat="server" Text='<%# Regex.Replace((CStr(Eval("CpyNotes")).Replace(",", vbCrLf)), "<(.|\n)*?>", "")%>'
                                            Rows="8" TextMode="MultiLine" MaxLength="2000" Width="250px" Height="200px">
                                        </telerik:RadTextBox>
                                        <bonton:ToolTipValidator ID="valCpyNotes" runat="server" ControlToEvaluate="rtxtCpyNotes"
                                            ErrorMessage="Invalid Character Found." OnServerValidate="valAsciiChars_ServerValidate"
                                            ValidationGroup="Update" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="MerchantNotes" HeaderText="Merchant Notes" UniqueName="MerchantNotes"
                                    ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="96px" />
                                    <ItemStyle HorizontalAlign="Center" Width="96px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Fabrication" HeaderText="Fabrication" UniqueName="Fabrication"
                                    ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="140px" />
                                    <ItemStyle HorizontalAlign="Center" Width="140px" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FriendlyColor" HeaderText="Friendly Color" UniqueName="FriendlyColor"
                                    ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Width="96px" />
                                    <ItemStyle HorizontalAlign="Center" Width="96px" />
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
            <rts:Modal ID="piModalExport" runat="server" />
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Vista">
                <Windows>
                    <telerik:RadWindow Skin="Vista" ID="UserDialog" runat="server" Title="Web Categories"
                        Height="400px" Width="1200px" Left="150px" Modal="true" DestroyOnClose="true"
                        VisibleTitlebar="true" OnClientClose="refreshGrid" />
                    <telerik:RadWindow Skin="Vista" ID="FloodWindow" runat="server" Title="Primary Web Categories"
                        Height="400px" Width="1200px" Left="150px" Modal="true" VisibleTitlebar="true"
                        OnClientClose="AddWebCategory" AutoSize="false" />
                    <telerik:RadWindow ID="PreviewWindow" runat="server" VisibleStatusbar="false" VisibleTitlebar="true" Behaviors="Close, Move" Modal="true"
                        AutoSize="false" Width="624px" Height="680px" KeepInScreenBounds="true" Skin="Vista">
                        <Shortcuts>
                            <telerik:WindowShortcut CommandName="Close" Shortcut="Esc" />
                        </Shortcuts>
                        <ContentTemplate>
                        <table>
                        <tr><td align="center">
                            <img src="javascript:void(0);" alt="Image holder" id="imageHolder" width="600px" height="600px" />
                        </td></tr>
                        <tr><td align="center">
                            <telerik:RadButton ID="btnImageFlipper" ToggleType="CustomToggle" ButtonType="StandardButton" runat="server" 
                                Width="24px" Height="24px" AutoPostBack="false" OnClientClicked="flipImage">
                                <ToggleStates>
                                    <telerik:RadButtonToggleState ImageUrl="../../Images/arrowright.gif" Text="Next" Selected="true" />
                                    <telerik:RadButtonToggleState ImageUrl="../../Images/arrowleft.gif" Text="Prev" />
                                </ToggleStates>
                            </telerik:RadButton>
                        </td></tr>
                        </table>
                        </ContentTemplate>
                    </telerik:RadWindow>
                    <telerik:RadWindow ID="SamplePicker" runat="server" NavigateUrl="SamplePicker.aspx" Skin="Vista" Behaviors="Close, Move"
                        Modal="True" MinWidth="1000px" MinHeight="600px" OnClientClose="clientClose" OnClientShow="clientShow" ReloadOnShow="true">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
        </telerik:RadPane>
    </telerik:RadSplitter>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            (function (global, undefined) {
                var demo = {};
                var $ = $telerik.$;

                function sizePreviewWindow() {
                    demo.previewWin.center();
                }

                function openWin(sampleImages) {
                    if (sampleImages.secondaryImageUrl != null) {
                        if (sampleImages.secondaryImageUrl.length > 0) {
                            demo.btnFlipper.set_selectedToggleStateIndex(0);
                            demo.btnFlipper.set_visible(true);
                        } else {
                            demo.btnFlipper.set_visible(false);
                        }
                    } else {
                        demo.btnFlipper.set_visible(false);
                      }
                    demo.previewWin.show();
                    demo.imgHolder.src = sampleImages.primaryImageUrl;
                    window.focus();
                }

                function clickFlip(sender, args) {
                    if (demo.btnFlipper.get_visible() == true) {
                        demo.btnFlipper.set_checked(!demo.btnFlipper.get_checked());
                        demo.btnFlipper.click();
                    }
                }

                global.$autoSizeDemo = demo;
                global.sizePreviewWindow = sizePreviewWindow;
                global.openWin = openWin;
                global.clickFlip = clickFlip;
            })(window);

            Sys.Application.add_load(function () {
                $autoSizeDemo.previewWin = $find("<%= PreviewWindow.ClientID %>");
                //get a reference to the image tag in the preview window
                $autoSizeDemo.imgHolder = $get("imageHolder");
                $autoSizeDemo.btnFlipper = $find("<%= btnImageFlipper.ClientID %>");
                //add onload event for the image in the preview window
                $addHandler($autoSizeDemo.imgHolder, "load", sizePreviewWindow);
                $addHandler($autoSizeDemo.imgHolder, "click", clickFlip);
            })

            function SampleImageInfo(primaryImageUrl, secondaryImageUrl, merchId) {
                this.primaryImageUrl = primaryImageUrl;
                this.secondaryImageUrl = secondaryImageUrl;
                this.MerchId = merchId;
            }

            // Find the window
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow)
                    oWindow = window.radWindow;
                else if (window.frameElement.radWindow)
                    oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            var sampleImages = {};

            function eMMShowImage(sender) {
                return ShowTheImage(sender, $find("<%= grdEMM.ClientID %>"));
            }

            function ccShowImage(sender) {
              return ShowTheImage(sender, $find("<%= grdCC.ClientID %>"));
            }

<%--            function ccSimpleShowImage(sender) {
                return ShowTheImage(sender, $find("<%= grdCCSimple.ClientID %>"));
            }--%>

            function copyWriterShowImage(sender) {
                return ShowTheImage(sender, $find("<%= grdCopyWriter.ClientID %>"));
            }

            // show the image viewer dialog, with image
            function ShowTheImage(sender, grid) {
                sampleImages = new SampleImageInfo();

                var masterTable = grid.get_masterTableView(); // get mastertableview
                var dataItems = masterTable.get_dataItems();
                for (var i = 0; i < dataItems.length; i++) {
                    var knownOffset = sender.src.lastIndexOf('/');
                    var subjectOffset = dataItems[i].getDataKeyValue("PrimaryThumbnailUrl").lastIndexOf('/')
                    if (dataItems[i].getDataKeyValue("PrimaryThumbnailUrl").substring(subjectOffset) === sender.src.substring(knownOffset)) {
                        sampleImages = new SampleImageInfo(dataItems[i].getDataKeyValue("PrimaryMediumUrl"),
                                                                dataItems[i].getDataKeyValue("SecondaryMediumUrl"),
                                                                dataItems[i].getDataKeyValue("SampleMerchId"));
                        break;
                    }
                }

                if (sampleImages.primaryImageUrl && sampleImages.primaryImageUrl.length > 0) {
                    setTimeout(function () { window.openWin(sampleImages); }, 0);
                }
                return false;
            }

            function flipImage(sender, args) {
                if ($autoSizeDemo.imgHolder.src == sampleImages.primaryImageUrl) {
                    $autoSizeDemo.imgHolder.src = sampleImages.secondaryImageUrl;
                } else {
                    $autoSizeDemo.imgHolder.src = sampleImages.primaryImageUrl;
                }
            }

            var samplePickinRow = null;

           function PickSample(sender, eventArgs) {
               var grid = $find("<%= grdCC.ClientID %>"); // get grid
               var MasterTable = grid.get_masterTableView(); // get mastertableview
               var gridDataItems = MasterTable.get_editItems();
<%--               if ( gridDataItems.length == 0 ) {
                   var grid = $find("<%= grdCCSimple.ClientID %>"); // get grid
                   var MasterTable = grid.get_masterTableView(); // get mastertableview
                   var gridDataItems = MasterTable.get_editItems();
               }--%>

               samplePickinRow = null;
                 if (gridDataItems.length > 0) {
                for (var i = 0; i < gridDataItems.length, samplePickinRow == null; i++) {
                    //if (gridDataItems[i].findElement("LineId") != null) {
                        samplePickinRow = gridDataItems[i];
                     //  }
                        var routed = samplePickinRow.getDataKeyValue("RoutefromAd");
                        var imgkindctl = samplePickinRow.findControl("rcbImageKind");
                        var imgkind = imgkindctl.get_value();
                        var isn = samplePickinRow.getDataKeyValue("ISN"); // get ISN value for selected edititem
                        var MerchID = samplePickinRow.getDataKeyValue("MerchID");
                        var turninMerchNum = samplePickinRow.getDataKeyValue("turnInMerchID"); // get SampleMerchId value for selected edititem
                        var colorId = samplePickinRow.getDataKeyValue("ColorCode");
                        window.focus(); // move the focus away otherwise the window opens again once the webcategories are changed.
//                        if (MerchID >  0) {
//                            isn = 0;
//                        }
                        if (imgkind != 'NEW' || routed > 0) {
                            return false;
                        }
                        else {
                            var oWnd = window.radopen("SamplePicker.aspx?ISN=" + isn + "&ColorId=" + colorId + "&MerchId=" + MerchID, "SamplePicker", 1000, 600);
                            oWnd.center();
                             }
                        }
                    }
                }
          //  }
            // page global
//            var fireOnOff = true;

//            function OnChangeOnOff(sender, eventArgs) {
//                if (fireOnOff == true) {
//                    var selectedText = sender.get_text();

//                    if (selectedText == '') {
//                        alert('Based on your selection of OFF for "On/Off Figure", Model Category will be blank.');
//                    }
//                }
//                else {
//                    fireOnOff == true
//                }
//            }


            function clientShow(sender, eventArgs) {
                var hfMerchId = samplePickinRow.findControl("hfMerchId");
                 sender.argument = hfMerchId.get_value();
             }
            
            function clientClose(sender, args) {
                if (args.get_argument() != null) {
                    var selectedMerchSample = args.get_argument();
                  //  var colorCodeIm = parseInt(selectedMerchSample.colorCode);
                  samplePickinRow.findControl("hfMerchId").set_value(selectedMerchSample.sampleMerchId);
                  //samplePickinRow.findControl("hfColorCode").set_value(colorCodeIm);
                  //samplePickinRow.findControl("FriendlyColor").set_value(selectedMerchSample.colorDesc);
                  var thmb = samplePickinRow.findElement("Thumbnail");
                  thmb.style.backgroundcolor = "Transparent";
                  thmb.src = (selectedMerchSample.primaryThumbnailUrl);
                  thmb.alt = (selectedMerchSample.sampleMerchId);

                var imgkindctl = samplePickinRow.findControl("rcbImageKind");
                var imgkind = imgkindctl._text;
                    if (imgkind == "NEW" || imgkind == "new" || imgkind == "New") {
                           
                }

                 }

                samplePickinRow = null;
            }
        </script>

        <script type ="text/javascript">
        function setAltText(sender, args){
               var ccgrid = $find(gridName); // get grid
               var MasterTable = ccgrid.get_masterTableView(); // get mastertableview
               var gridDataItem = MasterTable.get_dataItems()[rowId]; // get edititem
               var imagecodectl = gridDataItem.findControl("ImageKindCode"); 
               var imagecode = imagecodectl.get_text()
               var routeadctl = gridDataItem.findControl("RoutefromAd"); 
               var routead = routeadctl.get_text();
               if (imagecode != "NEW") {
                   var alttext = "Merchandise not required";
               }
               //               else if (routead > 0) {
               //                var alttext = "<%#Eval("PrimaryThumbnailUrl")%>";
               //               }
               else {
                   var alttext = "nothing";
               }
               return alttext;
               }
        </script>

        <script type="text/javascript">

            var fireOnOff = true;
            var fireOnOffFlood = true;
            var fireOnOffFloodCC2 = true;

            function OnChangeOnOffFlood(sender, eventArgs) {
                if (fireOnOffFlood == true) { 
                    var selectedValue = sender.get_value();
                    var rcbMCFlood = $find("<%=rcbFloodModelCategory.ClientID%>");

                    if (selectedValue == 'OFF') {
                        rcbMCFlood.set_text("");
                        return alert('Based on your selection of OFF for "On/Off Figure", Model Category will be blank.');
                    }
                }
                else {
                    fireOnOffFlood == true
                }
            }

            function OnChangeOnOff(sender, eventArgs, gridName, rowId) {
                if (fireOnOff == true) { 
                    var selectedText = sender.get_text();
                    var grid = $find(gridName); // get grid
                    var MasterTable = grid.get_masterTableView(); // get mastertableview
                    var gridDataItem = MasterTable.get_dataItems()[rowId]; // get edititem
                    var rcbModelCategory = gridDataItem.findControl("rcbModelCategory");
                    var valctl = sender.findControl("");
                    if (selectedText == 'OFF' && rcbModelCategory.get_text() != "") {
                        rcbModelCategory.set_text("");
                        
                        return alert('Based on your selection of OFF for "On/Off Figure", Model Category has been defaulted to blank.');
                    }

                }
                else {
                    fireOnOff == true
                }
            }

            function OnChangeImageKind(sender, eventArgs, gridName, rowId) {
                var selectedValue = sender.get_value();
                
                var grid = $find(gridName); // get grid
                var MasterTable = grid.get_masterTableView(); // get mastertableview
                var gridDataItem = MasterTable.get_dataItems()[rowId]; // get edititem
                var rcbFeatureRenderSwatch = gridDataItem.findControl("rcbFeatureRenderSwatch");
                var rcbOnOff = gridDataItem.findControl("rcbOnOff");
                var onOff = rcbOnOff.get_text();
                var rcbModelCategory = gridDataItem.findControl("rcbModelCategory");
                var pageNumber = gridDataItem.getDataKeyValue("PageNumber");
                                
                  //5-20-2014 3:05:25 PM	ISSUE 459 - Vendor Images should always be OFF 
               
                if (selectedValue == "VND") {
                    fireOnOff = false;
                    rcbOnOff.findItemByText("OFF").select();
                    rcbOnOff.disable();
                } else {
                    rcbOnOff.enable();
                }

               
                if (selectedValue == "CR8" || selectedValue == "DUP" || selectedValue == "VND" || selectedValue == "NOMER" || selectedValue == "PU") {
                    if (onOff != "OFF") {
                        alert('Based on your selection of ' + selectedValue + ' for "Image Kind", we have defaulted "On/Off Figure" to OFF and Model Category has been defaulted to blank.');
                        fireOnOff = false;
                        rcbOnOff.findItemByText("OFF").select();
                        rcbModelCategory.set_text("");
                        var modelvalctl = gridDataItem.findControl("valModelCategory");
                    }
                }

                else if (selectedValue != "PU" && selectedValue != "DUP") {
                    if (gridDataItem.findControl("rtxtPickupImageID").get_text != "") {
                        gridDataItem.findControl("rtxtPickupImageID").set_value("");
                    }
                    if ((pageNumber <= 4) && (onOff != "ON")) {
                        rcbOnOff.findItemByText("ON").select();
                        alert('Based on your selection of NEW for "Image Kind" and because page number is ' + pageNumber + ', we have defaulted "On/Off Figure" to ON.');
                  
                    }
                    else {
                        rcbOnOff.findItemByText("OFF").select();
                        alert('Based on your selection of NEW for "Image Kind" and because page number is ' + pageNumber + ', we have defaulted "On/Off Figure" to OFF.');
                        var modelvalctl = gridDataItem.findControl("valModelCategory");
                                           }
                }

//                else if (selectedValue == "NEW" && selectedValue != "DUP") {
//                    if (gridDataItem.findControl("rtxtPickupImageID").get_text != "") {
//                        gridDataItem.findControl("rtxtPickupImageID").set_value("");
//                    }
//                }

                else {

                    if (rcbFeatureRenderSwatch.get_value() != "SWTCH" && rcbFeatureRenderSwatch.get_value() != "SWTBOX" && selectedValue == "NEW") {
                        //Change the selection to ON if Image Kind is New and FRS is NOT Swatch or Static Swatch and if page number is 1-4 and ON isn't already selected.
                        if ((onOff != "ON") && (pageNumber < 5)) {
                            rcbOnOff.findItemByText("ON").select();
                            alert('Based on your selection of NEW for "Image Kind" and because page number is ' + pageNumber + ', we have defaulted "On/Off Figure" to ON.');
                        }
                        //Change the selection to OFF if Image Kind is New and FRS is NOT Swatch or Static Swatch and if page number is 5+ and OFF isn't already selected.
                        if ((onOff != "OFF") && (pageNumber >= 5)) {
                            rcbOnOff.findItemByText("OFF").select();
                            alert('Based on your selection of NEW for "Image Kind" and because page number is ' + pageNumber + ', we have defaulted "On/Off Figure" to OFF.');
                        }
                    }
                                     }
            }

            function OnChangeFRS(sender, eventArgs, gridName, rowId) {
                var selectedValue = sender.get_value();

                var grid = $find(gridName); // get grid
                var MasterTable = grid.get_masterTableView(); // get mastertableview
                var gridDataItem = MasterTable.get_dataItems()[rowId]; // get edititem
                var rcbImageKind = gridDataItem.findControl("rcbImageKind");
                var rcbOnOff = gridDataItem.findControl("rcbOnOff");
                var onOff = rcbOnOff.get_text();
                var rcbModelCategory = gridDataItem.findControl("rcbModelCategory");
                var pageNumber = gridDataItem.getDataKeyValue("PageNumber");

                if (selectedValue == "SWTCH" || selectedValue == "SWTBOX") {
                    if (onOff != "OFF") {
                        alert('Based on your selection of ' + selectedValue + ' for "Feature/Render/Swatch", we have defaulted "On/Off Figure" to OFF and Model Category has been defaulted to blank.');
                        fireOnOff = false;
                        rcbOnOff.findItemByText("OFF").select();
                        rcbModelCategory.set_text("");
                    }
                }
                else {
                    if (onOff != "ON" && rcbImageKind != "CR8" && rcbImageKind != "DUP" && rcbImageKind != "VND" && rcbImageKind != "NOMER" && rcbImageKind != "PU") {
                        if (pageNumber < 5) {
                            rcbOnOff.findItemByText("ON").select();
                            return alert('Based on your selection of ' + selectedValue + ' for "Feature/Render/Swatch", we have defaulted "On/Off Figure" to ON.');
                        }
                        else {
                            rcbOnOff.findItemByText("OFF").select();
                            return alert('Based on your selection of ' + selectedValue + ' for "Feature/Render/Swatch", we have defaulted "On/Off Figure" to OFF.');
                        }
                    }
                }
            }

            function OnChangeFRSFloodCC(sender, eventArgs) {
                var selectedValue = sender.get_value();
                var rcbOnOff = $find("<%=rcbFloodImgType.ClientID%>");
                var onOff = rcbOnOff.get_text();
                var rcbMCFlood = $find("<%=rcbFloodModelCategory.ClientID%>");

                if (selectedValue == "SWTCH" || selectedValue == "SWTBOX") {
                    if (onOff != "OFF") {
                        alert('Based on your selection of ' + selectedValue + ' for "Feature/Render/Swatch", we have defaulted "On/Off Figure" to OFF and Model Category has been defaulted to blank.');
                        fireOnOffFlood = false;
                        rcbOnOff.findItemByText("OFF").select();
                        rcbMCFlood.set_text("");
                    }
                }
            }

            function OpenWebCategoriesForAll(sender, eventArgs) {
                eventArgs.set_cancel(!confirm('Changes made to other rows will be lost. If you have made changes to more than one row, use the Save All button at the top first. Do you want to continue ?'));
                var oWnd = radopen("WebCategories.aspx?ID=0", "FloodWindow"); //open in radwindow
                eventArgs.set_cancel(true);
            }

            function AddWebCategory(oWnd, eventArgs) {
                var arg = eventArgs.get_argument(); //get the transferred arguments
                var txtBoxWebCatDesc = $find("<%= rtxtFloodWebCategories.ClientID %>");
                if (arg) {
                    if (oWnd.get_name() == "FloodWindow") {
                        document.getElementById('<%= hfFloodWebCatCde.ClientID %>').value = arg.WebCatCde;
                        txtBoxWebCatDesc.set_value(arg.WebCatDesc);
                    }
                }
            }

            function ValidateText(sender, eventArgs) {
                var str = sender.get_value();
                var isValid = true;

                for (var i = 0; i < str.length; i++) {
                    asciiNum = str.charCodeAt(i);
                    if ((asciiNum == 10) || (asciiNum == 13) || (asciiNum > 31 && asciiNum < 94) || (asciiNum > 94 && asciiNum < 127)) {
                    }
                    else {
                        isValid = false;
                    }
                }
                //sender.get_id()
                if (!isValid) {
                    alert("Invalid Character found!");
                    sender.focus();
                }
                sender.set_value(str.replace(/([^a-zA-Z\d\s:]*[^\s:\-])([^\s:\-]*)/g, function ($0, $1, $2) { return $1.toUpperCase() + $2.toLowerCase(); }));
                return isValid;
            }

            function refreshGrid() {
               $("body").css({
                    "cursor": "wait"
                })
          
                    var masterTable = $find("<%=grdEMM.clientId%>").get_masterTableView();
                    masterTable.rebind();
                }

            function OnGridCommandClickEMM(sender, eventArgs) {
                var command = eventArgs.get_commandName();
                if (command == "Update") {
                    var radGrid = $find("<%=grdEMM.ClientID %>");
                    if (radGrid) {
                        var MasterTable = radGrid.get_masterTableView();
                        var editItems = MasterTable.get_editItems();
                        if (editItems.length > 1) {
                            eventArgs.set_cancel(!confirm('Are you sure you want to save? Changes made to other rows will be lost. If you have made changes to more than one row, use the Save All button at the top.'));
                        }
					}
                }
                if (command == "Delete") {
                    if (!CheckForRowsInEditMode()) {
                        eventArgs.set_cancel(true);
                    } else {
                        eventArgs.set_cancel(!confirm('Delete/Activate this record?'));
                    }
                }
                if (command == "Page" || command == "PageSize") {
                    if (!CheckForRowsInEditMode()) {
                        eventArgs.set_cancel(true);
                    }
                }
            }

            function OnGridCommandClickCC(sender, eventArgs) {
                var command = eventArgs.get_commandName();
                if (command == "Update") {
                    var radGrid = $find("<%=grdCC.ClientID %>");
                    if (radGrid) {
                        var MasterTable = radGrid.get_masterTableView();
                        var editItems = MasterTable.get_editItems();
                        if (editItems.length > 1) {
                            eventArgs.set_cancel(!confirm('Are you sure you want to save? Changes made to other rows will be lost. If you have made changes to more than one row, use the Save All button at the top.'));
                        }
					}
                }
                if (command == "Delete") {
                    if (!CheckForRowsInEditMode()) {
                        eventArgs.set_cancel(true);
                    } else {
                        eventArgs.set_cancel(!confirm('Delete/Activate this record?'));
                    }
                }
                if (command == "Page" || command == "PageSize") {
                    if (!CheckForRowsInEditMode()) {
                        eventArgs.set_cancel(true);
                    }
                }
            }

            function OnGridCommandClickCopyWriter(sender, eventArgs) {
                var command = eventArgs.get_commandName();
                if (command == "Update") {
                    var radGrid = $find("<%=grdCopyWriter.ClientID %>");
                    if (radGrid) {
                        var MasterTable = radGrid.get_masterTableView();
                        var editItems = MasterTable.get_editItems();
                        if (editItems.length > 1) {
                            eventArgs.set_cancel(!confirm('Are you sure you want to save? Changes made to other rows will be lost. If you have made changes to more than one row, use the Save All button at the top.'));
                        }
					}
                }
                if (command == "Delete") {
                    if (!CheckForRowsInEditMode()) {
                        eventArgs.set_cancel(true);
                    } else {
                        eventArgs.set_cancel(!confirm('Delete/Activate this record?'));
                    }
                }
                if (command == "Page" || command == "PageSize") {
                    if (!CheckForRowsInEditMode()) {
                        eventArgs.set_cancel(true);
                    }
                }
            }

            function OnClientFloodClick(sender, eventArgs) {
                if (!CheckForRowsInEditMode()) {
                    eventArgs.set_cancel(true);
                } else {
                    eventArgs.set_cancel(!confirm('Are you sure? This action will automatically save data for every row selected.'));
                }
            }

            function CheckForRowsInEditMode() {
                var radGridEMM = $find("<%=grdEMM.ClientID %>");
                var radGridCC = $find("<%=grdCC.ClientID %>");
                var radGridCW = $find("<%=grdCopyWriter.ClientID %>");
                if (radGridEMM && radGridCC && radGridCW) {

                    var MasterTableEMM = radGridEMM.get_masterTableView();
                    var MasterTableCC = radGridCC.get_masterTableView();
                    var MasterTableCW = radGridCW.get_masterTableView();

                    var editItemsEMM = MasterTableEMM.get_editItems();
                    var editItemsCC = MasterTableCC.get_editItems();
                    var editItemsCW = MasterTableCW.get_editItems();

                    if (editItemsEMM.length > 0 || editItemsCC.length > 0 || editItemsCW.length > 0) {
                        alert("A row is in Edit mode. Please Save/Cancel changes before performing any action.");
                        return false;
                    } else {
                        return true;
                    }
                } else {
                    return true;
                }
            }

            function OnClientRadToolBarClick(sender, eventArgs) {
                var button = eventArgs.get_item();
                if (button.get_text() != "Save All" && button.get_text() != "Cancel All") {
                    if (CheckForRowsInEditMode()) {
                        if (button.get_text() == "Sort") {
                            ShowModalOrderColors();
                            eventArgs.set_cancel(true);
                        }
                        if (button.get_text() == "Export") {
                            ShowModalMessage();
                            eventArgs.set_cancel(true);
                        }
                        if (button.get_text() == "Reject") {
                            eventArgs.set_cancel(!confirm('Are you sure? This action will reject the current batch.'));
                        }
                        if (button.get_text() == "Delete") {
                            eventArgs.set_cancel(!confirm('Are you sure? This action will delete/kill the selected rows.'));
                        }
                    } else {
                        eventArgs.set_cancel(true);
                    }
                } else {
                    if (button.get_text() == "Save All") {
                        eventArgs.set_cancel(!confirm('Are you sure? This action will save all rows.'));
                    }
                    if (button.get_text() == "Cancel All") {
                        eventArgs.set_cancel(!confirm('Are you sure? This action will cancel all rows.'));
                    }
                }
            }

            function OnClientRadTabStripClick(sender, eventArgs) {
                if (!CheckForRowsInEditMode()) {
                    eventArgs.set_cancel(true);
                } else {
                    var tab = eventArgs.get_tab();
                    if (tab.get_text() == "Creative Coord" || tab.get_text() == "Copy Writer") {
                        var radGrid = $find("<%=grdEMM.ClientID %>");
                        if (!radGrid) {
                            alert("Retrieve data before navigating to other tabs.");
                            eventArgs.set_cancel(true);
                        }
                    }
                }
            }
        </script>        
    </telerik:RadCodeBlock>
</asp:Content>
