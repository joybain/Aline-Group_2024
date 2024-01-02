<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmDistrubation.aspx.cs" Inherits="frmDistrubation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <script src='<%= ResolveUrl("~/Scripts/valideDate.js") %>' type="text/javascript"></script>
<script language="javascript" type="text/javascript" >
    function OpenWindow(Url) {
        var testwindow = window.open(Url, '', 'width=600px,height=400px,top=100,left=300,scrollbars=1');
    }

    function setValueItem(item, iname, msr, rate) {
        $('input:text[id$=txtItemCode]').val(item);
        $('input:text[id$=txtQnty]').focus();
    }

    function remLink() {
        if (window.testwindow && window.testwindow.open && !window.testwindow.closed)
            window.testwindow.opener = null;
    }
    function IsEmpty(aTextField) {
        if ((aTextField.value.length == 0) || (aTextField.value == null)) {
            return true;
        }
        else {
            return false;
        }
    }
    function onListPopulated() {
        var completionList = $find("AutoCompleteEx").get_completionList();
        completionList.style.width = 'auto';
    }
       
        function UncheckOthers(objchkbox) {
        //Get the parent control of checkbox which is the checkbox list
        var objchkList = objchkbox.parentNode.parentNode.parentNode;
        //Get the checkbox controls in checkboxlist
        var chkboxControls = objchkList.getElementsByTagName("input");
        //Loop through each check box controls
        for (var i = 0; i < chkboxControls.length; i++) {
            //Check the current checkbox is not the one user selected
            if (chkboxControls[i] != objchkbox && objchkbox.checked) {
                //Uncheck all other checkboxes
                chkboxControls[i].checked = false;
            }
        }
    }
    
//    function Remarks() {

    //        var txtRemarks = document.getElementById("<%=txtRemark.ClientID %>");
//        txtRemarks.focus();
//    }    	
</script>
<script language="javascript" type="text/javascript" >
    function setDecimal(abc) {
        var dt = document.getElementById(abc).value;
        if (dt.length > 0) {
            document.getElementById(abc).value = parseFloat(dt).toFixed(2);
        }
    }
    function isNumber(evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    }

    onblur = function () {
        setTimeout('self.focus()', 100);
    }

    function LoadModalDiv() {

        var bcgDiv = document.getElementById("divBackground");
        bcgDiv.style.display = "block";

    }
    function HideModalDiv() {

        var bcgDiv = document.getElementById("divBackground");
        bcgDiv.style.display = "none";

    }

</script>

<div id="frmMainDiv" style="background-color:White; width:100%;">
    <table  id="pageFooterWrapper">
  <tr>  
        <td align="center">
        <asp:Button ID="btnDelete" runat="server" ToolTip="Delete"
            
                onclientclick="javascript:return window.confirm('are u really want to delete these data')" Text="Delete" 
        Height="30px" Width="110px" BorderStyle="Outset"  />
        </td>       
        <td align="center" >
            &nbsp;</td>       
        <td align="center" >
        <asp:Button ID="btnSave" runat="server" OnClientClick="this.disabled=true;" 
                UseSubmitBehavior="false" ToolTip="Save Purchase Record" 
                Text="Save" 
        Height="30px" Width="110px" BorderStyle="Outset" onclick="btnSave_Click"  />
        </td>
        <td align="center" >
        <asp:Button ID="btnNew" runat="server" ToolTip="New"   Text="New" 
        Height="30px" Width="110px" BorderStyle="Outset" onclick="btnNew_Click"  /> 
        </td>
        <td align="center" >
        <asp:Button ID="btnClear" runat="server"  ToolTip="Clear"  Text="Clear" 
        Height="30px" Width="110px" BorderStyle="Outset" onclick="btnClear_Click"  />
        </td>
        <td align="center" >
        <asp:Button ID="btnPrint" runat="server" ToolTip="Print PO" Text="Print" 
        Height="30px" Width="110px" BorderStyle="Outset" onclick="btnPrint_Click"   />
        </td>            
        
   </tr>
   </table> 
   <br>
   
    <table style="width:100%;">
        <tr>
            <td style="width: 2%"></td>
            <td style="width: 96%">
                <asp:UpdatePanel ID="PVI_UP" runat="server" UpdateMode="Conditional">
                     <ContentTemplate>
                <fieldset style="vertical-align: top; border: solid 1px #8BB381; text-align:left;"><legend style="color: maroon;"><b>Distribution</b> </legend>
                <asp:Panel ID="pnl" runat="server">

                                                <table style="width: 100%">
                                                   
                                                    <tr>
                                                        <td style="width: 6%; height: 29px; font-weight: 700;">
                                                            &nbsp;</td>
                                                        <td align="center" style="width: 1%; height: 29px;">
                                                            </td>
                                                        <td style="height: 29px;" colspan="3">
                                                            <asp:UpdatePanel ID="UPItemType" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:CheckBox ID="checkbox" runat="server" AutoPostBack="True" 
                                                                        oncheckedchanged="checkbox_CheckedChanged" Text="Transfer Matrial" />
                                                                </ContentTemplate>
                </asp:UpdatePanel>
                                                        </td>
                                                        <td style="width: 10%; height: 29px;">
                                                           
                                                            <asp:Label ID="lblReqNo" runat="server" Visible="False"></asp:Label>
                                                            <asp:TextBox ID="txtID" runat="server" AutoPostBack="False" CssClass="tbc" Font-Size="8pt" SkinId="tbPlain" Visible="False" Width="60%"></asp:TextBox>
                                                           
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td style="width: 6%; height: 29px; font-weight: 700;">
                                                            Requisition Code</td>
                                                        <td align="center" style="width: 1%; height: 29px;">
                                                            :</td>
                                                        <td style="width: 13%; height: 29px;">
                                                            <asp:TextBox ID="txtRequisitionCode" runat="server" Width="60%"></asp:TextBox>
                                                            <asp:Label ID="lblPrintFlag" runat="server" Visible="False"></asp:Label>
                                                        </td>
                                                        <td align="right" style="width: 10%; height: 29px;">
                                                            <asp:Label ID="Label9" runat="server" Text="Challan No"></asp:Label>
                                                        </td>
                                                        <td align="center" style="width: 2%; height: 29px;">
                                                            &nbsp;</td>
                                                        <td style="width: 10%; height: 29px;">
                                                            <asp:TextBox ID="txtChallanNo" runat="server" Enabled="False" Width="53%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 6%; height: 29px;">
                                                            <asp:Label ID="Label5" runat="server" Text="Project Name"></asp:Label>
                                                        </td>
                                                        <td align="center" style="width: 1%; height: 29px;">
                                                            <asp:Label ID="Label3" runat="server">:</asp:Label></td>
                                                        <td style="width: 13%; height: 29px;">
                                                            <asp:DropDownList ID="ddlProjectName" runat="server" AutoPostBack="True" 
                                                                Font-Size="8pt" Height="26px" 
                                                                 SkinId="ddlPlain" 
                                                                Width="93%" onselectedindexchanged="ddlProjectName_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="right" style="width: 10%; height: 29px;">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td style="width: 50%;text-align: center">
                                                                        <asp:CheckBox ID="chkAllItems" runat="server" Text="All Items" 
                                                                            Visible="False" />
                                                                    </td>
                                                                    <td style="width: 50%">
                                                                        <asp:Label ID="lblID" runat="server" Visible="False"></asp:Label>
                                                                        <asp:Label ID="Label8" runat="server" Text="Transfer Date"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="center" style="width: 2%; height: 29px;">
                                                            :</td>
                                                        <td style="width: 10%; height: 29px;">
                                                            <asp:TextBox ID="txtTfDate" runat="server" CssClass="tbc" Font-Size="8pt" 
                                                                PlaceHolder="dd/MM/yyyy" SkinId="tbPlain" style="text-align:center;" 
                                                                Width="93%"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="Calendarextender1" runat="server" 
                                                                Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtTfDate" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 6%; height: 29px;">
                                                            <asp:Label ID="Label1" runat="server" Text="Transfer Project Name"></asp:Label>
                                                        </td>
                                                        <td align="center" style="width: 1%; height: 29px;">
                                                            <asp:Label ID="Label2" runat="server">:</asp:Label></td>
                                                        <td style="width: 13%; height: 29px;">
                                                            <asp:DropDownList ID="ddlTransferProject" runat="server" AutoPostBack="True" 
                                                                Font-Size="8pt" Height="26px" 
                                                                 SkinId="ddlPlain" 
                                                                Width="93%" Enabled="False" >
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="right" style="width: 10%; height: 29px;">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td style="width: 50%;text-align: center">
                                                                        
                                                                    </td>
                                                                    <td style="width: 50%">
                                                                       
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="center" style="width: 2%; height: 29px;">
                                                            &nbsp;</td>
                                                        <td style="width: 10%; height: 29px;">
                                                           
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 6%; height: 25px;">Remarks</td>
                                                        <td align="center" style="width: 1%; height: 25px;">:</td>
                                                        <td colspan="4" style="height: 25px;">
                                                            <asp:TextBox ID="txtRemark" runat="server" Width="98%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>

                </fieldset>
                     </ContentTemplate> 
                </asp:UpdatePanel>
                

                
            <asp:Panel ID="pnlItemDtl" runat="server">
                                            <fieldset style="vertical-align: top; border: solid 1px #8BB381;text-align:left;">
                                                <legend style="color: maroon;"><b>Items Details </b></legend>
                                                <div ID="Div2" runat="server">
                                                    <table style="width: 100%">
                                                        <tr>
                                                        <td align="right" style="width: 25%; height: 16px; color: #FF0000;">
                                                            <asp:Label ID="lblSerchRequisitionNo" runat="server" Visible="False"><strong>Search Requisition No&nbsp;&nbsp;&nbsp; </strong></asp:Label>
                                                             </td>
                                                        
                                                            
                                                            <td style="width: 25%; height: 16px; margin-left: 40px;">
                                                                <asp:TextBox ID="txtRequisitionNo" runat="server" AutoPostBack="True" 
                                                                    CssClass="tbc" Font-Size="8pt" 
                                                                    PlaceHolder="Search Requisition No.." SkinId="tbPlain" style="text-align:center; height: 20px;" 
                                                                    Width="93%" ontextchanged="txtRequisitionNo_TextChanged1" Visible="False" ></asp:TextBox>


                                                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" 
                                                                    CompletionInterval="20" CompletionSetCount="12" DelimiterCharacters="" 
                                                                    Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetItemsSearch" 
                                                                    ServicePath="AutoComplete.asmx" TargetControlID="txtRequisitionNo" />
                                                            </td>
                                                            <td style="width: 15%; height: 16px; color: #339933; font-weight: 700;" 
                                                                align="right">
                                                                <asp:Label ID="Label4" runat="server"><strong>Search Item </strong></asp:Label>
                                                            </td>
                                                            <td style="width: 40%; height: 16px;">
                                                                            
                                                                <asp:TextBox ID="txtItemSearch" runat="server" AutoPostBack="True" 
                                                                    placeholder="Search by Item.." 
                                                                    Width="96%" Visible="False"></asp:TextBox>
                                                                <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" 
                                                                    CompletionInterval="20" CompletionSetCount="12" DelimiterCharacters="" 
                                                                    Enabled="True" MinimumPrefixLength="1" ServiceMethod="getItemdistrubationHeadOffice" 
                                                                    ServicePath="AutoComplete.asmx" TargetControlID="txtItemSearch">
                                                                </ajaxToolkit:AutoCompleteExtender>
                                                                            
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                         <td align="right" style="width: 25%; height: 16px;">
                                                           <asp:Label ID="lblSerchitem" runat="server" Visible="False"><strong>Search Item &nbsp;&nbsp;&nbsp; </strong></asp:Label>  &nbsp;</td>
                                                        
                                                            
                                                            <td style="width: 25%; height: 16px;">
                                                              
                                                                <asp:TextBox ID="txtSerchItem" runat="server" 
                                                                    AutoPostBack="True" CssClass="tbc" Font-Size="8pt" 
                                                                
                                                                    PlaceHolder="Search Item No.." 
                                                                    SkinId="tbPlain" style="text-align:center;" Width="93%" Visible="False" 
                                                                    ontextchanged="txtSerchItem_TextChanged"></asp:TextBox>
                                                                <ajaxToolkit:AutoCompleteExtender 
                                                                    ID="txtSerchItem_AutoCompleteExtender" runat="server" 
                                                                    CompletionInterval="20" CompletionSetCount="12" 
                                                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" 
                                                                    ServiceMethod="getItemdistrubationHeadOffice" ServicePath="AutoComplete.asmx" 
                                                                    TargetControlID="txtSerchItem" />
                                                              
                                                            </td>
                                                           <td style="width: 15%; height: 16px; font-weight: 700;" align="right">
                                                               <span style="color: #0099CC">Total Stock Quantity:</span>&nbsp;</td>
                                                            <td style="width: 40%; height: 16px; font-weight: 700;">
                                                               <asp:Label ID="lblQuantity" runat="server" Text="0"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                 <asp:UpdatePanel ID="MRIesms_UP" runat="server"  UpdateMode="Conditional">
                <ContentTemplate>
                
                                                <div ID="ItemsDetails" runat="server">
                                                   
                                                            <asp:GridView ID="dgDistribution" runat="server" AutoGenerateColumns="False" 
                                                                BorderColor="LightGray" CssClass="mGrid" Font-Size="9pt" 
                                                               
                                                                Width="100%"  
                                                                onrowdeleting="dgDistribution_RowDeleting" 
                                                                onrowdatabound="dgDistribution_RowDataBound1">
                                                                <AlternatingRowStyle CssClass="alt" />
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="lbDelete" runat="server" CausesValidation="False" 
                                                                                CommandName="Delete" ImageAlign="Middle" ImageUrl="~/img/delete.png" 
                                                                                Text="Delete" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle Font-Size="8pt" HorizontalAlign="Center" Width="4%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Code">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtCode" runat="server" AutoPostBack="true" Enabled="true" 
                                                                                Font-Size="8pt" MaxLength="15" onFocus="this.select()" 
                                                                                SkinId="tbPlain" 
                                                                                Text='<%#Eval("item_code")%>' Width="90%"></asp:TextBox>
                                                                          
                                                                        </ItemTemplate>
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <ItemStyle HorizontalAlign="Center" Width="11%" />
                                                                    </asp:TemplateField>
                                                                           
                                                                    <asp:TemplateField HeaderText="Items Name">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtItemName" runat="server" AutoPostBack="True" 
                                                                                 
                                                                                placeHolder="Search Items Code/Style No./Name" Text='<%#Eval("item_desc")%>' 
                                                                                Width="96%" ontextchanged="txtItemName_TextChanged"></asp:TextBox>
                                                                            <ajaxToolkit:AutoCompleteExtender ID="autoComplete1" runat="server" 
                                                                                CompletionInterval="20" CompletionSetCount="12" EnableCaching="true" 
                                                                                MinimumPrefixLength="1" ServiceMethod="GetItemListSerch" 
                                                                                ServicePath="AutoComplete.asmx" TargetControlID="txtItemName" />
                                                                          <%--  <asp:Label ID="lblItemsID" runat="server" style="display:none;" 
                                                                                Text='<%#Eval("ItemId")%>'></asp:Label>--%>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <ItemStyle HorizontalAlign="Center" Width="25%" />
                                                                    </asp:TemplateField>
                                                                      <asp:TemplateField HeaderText="Unit">
                                 
                                     <ItemTemplate>
                                           <asp:DropDownList ID="ddlMeasure" 
                                     runat="server" AutoPostBack="true" 
           DataSource="<%#PopulateMeasure()%>" DataTextField="Name" DataValueField="ID" Font-Size="8pt" 
           SelectedValue='<%#Eval("msr_unit_code")%>' SkinId="ddlPlain" Width="95%" Height="26px" 
                                     Enabled="False"></asp:DropDownList>
                                     </ItemTemplate>
                                 
                                 </asp:TemplateField>
                                                                    
                                                                    <asp:TemplateField HeaderText="Requisition Qty">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtRequisitionQnty" runat="server" AutoPostBack="true" 
                                                                                CssClass="6xtVisibleAlign" Enabled="False" Font-Size="8pt" 
                                                                                onFocus="this.select()" onkeypress="return isNumber(event)" placeHolder="0.00" 
                                                                                SkinId="tbPlain" style="text-align:right;" Text='<%#Eval("requisition_qnty")%>' 
                                                                                Width="90%"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:TemplateField>
                                                                      <asp:TemplateField HeaderText="Deu Requisition Qty">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtDueRequisitionQnty" runat="server" AutoPostBack="true" 
                                                                                CssClass="6xtVisibleAlign" Enabled="False" Font-Size="8pt" 
                                                                                onFocus="this.select()" onkeypress="return isNumber(event)" placeHolder="0.00" 
                                                                                SkinId="tbPlain" style="text-align:right;" Text='<%#Eval("duerequisition_qnty")%>' 
                                                                                Width="90%"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:TemplateField>
                                                                     <asp:TemplateField HeaderText="Stock Qty">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtStockQuantity" runat="server" AutoPostBack="False" 
                                                                                CssClass="txtVisibleAlign" Enabled="false" Font-Size="8pt" 
                                                                                onFocus="this.select()" onkeypress="return isNumber(event)" placeHolder="0.00" 
                                                                                SkinId="tbPlain" style="text-align:right;" Text='<%#Eval("present_Stock")%>' 
                                                                                Width="90%">
           </asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" Width="8%" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:TemplateField>
                                                             
                                                                  
                                                                    <asp:TemplateField HeaderText="This Time Qty">
                                                                        <ItemTemplate>
                                                                            <asp:UpdatePanel ID="Uptr" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:TextBox ID="txtPresentQty" runat="server" AutoPostBack="true" 
                                                                                        CssClass="tbc" Font-Size="8pt" onFocus="this.select()" 
                                                                                        onkeypress="return isNumber(event)" 
                                                                                        placeHolder="0" 
                                                                                        SkinId="tbPlain" style="text-align:right;" Text='<%#Eval("this_time_requisition_qnty")%>' 
                                                                                        Width="90%" ontextchanged="txtPresentQty_TextChanged"></asp:TextBox>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:TemplateField>
                                                                     <asp:TemplateField HeaderText="Total Stock">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtTotalQnty" runat="server" AutoPostBack="true" 
                                                                                CssClass="txtVisibleAlign" Enabled="False" Font-Size="8pt" 
                                                                                onFocus="this.select()" onkeypress="return isNumber(event)" placeHolder="0.00" 
                                                                                SkinId="tbPlain" style="text-align:right;" Text='<%#Eval("total_Stock")%>' 
                                                                                Width="90%"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:TemplateField>
                                                                      <asp:TemplateField HeaderText="Remarks">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtRemarks" runat="server" AutoPostBack="False" 
                                                                                CssClass="txtVisibleAlign" Enabled="True" Font-Size="8pt" 
                                                                                onFocus="this.select()" 
                                                                                SkinId="tbPlain" style="text-align:right;" Text='<%#Eval("Remarksany")%>' 
                                                                                Width="90%">
           </asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" Width="8%" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:TemplateField>
                                                                    
                                                                                                  <asp:TemplateField HeaderText="ID">
                                  <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" 
                                     Font-Size="8pt" Width="95%" Text='<%#Eval("ID")%>'></asp:Label>                                            
                                    </ItemTemplate></asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle Font-Bold="True" Font-Size="9pt" ForeColor="White" />
                                                                <PagerStyle CssClass="pgr" ForeColor="White" HorizontalAlign="Center" />
                                                                <RowStyle BackColor="White" />
                                                            </asp:GridView>
                                                       
                                                </div>
                                                </ContentTemplate>
                </asp:UpdatePanel>
                                            </fieldset>
                                            
                            <asp:GridView ID="dgRequistionHistory" runat="server" 
                         AutoGenerateColumns="False" CssClass="mGrid" Width="100%" 
                                                onrowdatabound="dgRequistionHistory_RowDataBound" onselectedindexchanged="dgRequistionHistory_SelectedIndexChanged" 
                         >                       
                         <Columns>
                             <asp:CommandField ShowSelectButton="True" />
                             <asp:BoundField DataField="ID" HeaderText="Id" />
                             <asp:BoundField DataField="Code" HeaderText="Distrubation Code" />
                             <asp:BoundField DataField="Requisition_Code" HeaderText="Requisition Code" />
                             <asp:BoundField DataField="ProjectName" HeaderText="Project Name" />
                             <asp:BoundField DataField="Requisition_by" HeaderText="Requisition by" />
                             <asp:BoundField DataField="CreateDate" HeaderText="Create Date" />
                             <asp:BoundField DataField="RequisitionId" HeaderText="RequisitionId" />
                                    <asp:BoundField DataField="Status" HeaderText="Status">
                                                <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" 
                                                    Width="10%" />
                                                </asp:BoundField>
                         </Columns>
                     </asp:GridView>
                                        </asp:Panel>    

            </td>
            <td style="width: 2%"></td>
        </tr>
    </table>
</div>
</asp:Content>

