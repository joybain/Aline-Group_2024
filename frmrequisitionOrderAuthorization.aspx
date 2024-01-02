<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmrequisitionOrderAuthorization.aspx.cs" Inherits="frmrequisitionOrderAuthorization" %>

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
            &nbsp;</td>       
        <td align="center" >
            &nbsp;</td>       
        <td align="center" >
        <asp:Button ID="btnAuthorize" runat="server" OnClientClick="this.disabled=true;" 
                UseSubmitBehavior="false" ToolTip="Authorize Requisition" Text="Authorize" 
        Height="30px" Width="110px" BorderStyle="Outset" onclick="btnAuthorize_Click"  />
        </td>
        <td align="center" >
            &nbsp;</td>
        <td align="center" >
            &nbsp;</td>
        <td align="center" >
          <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" 
                                                            BackgroundCssClass="modalBackground"  DynamicServicePath="" 
                                                            Enabled="True" PopupControlID="LoginPanel1" TargetControlID="btnCancel">
                                                        </ajaxToolkit:ModalPopupExtender>
        </td>            
        
   </tr>
   </table>
   <br />
     
 <table style="width:100%;">
<tr>
<td style="width:1%;" align="center"></td>
<td style="width:98%;" align="center"> 	
<br />
 <asp:UpdatePanel ID="PVI_UP" runat="server" UpdateMode="Conditional"> 
 <ContentTemplate>
<fieldset style="vertical-align: top; border: solid 1px #8BB381; text-align:left;">
    <legend style="color: maroon; font-weight: 700;">Requisition<b> Order</b> </legend>
<table id="Table1" style="width:100%;">

    <tr>
        <td align="left" style="width: 13%; ">
            &nbsp;</td>
        <td align="left" style="width: 30%;">
            &nbsp;</td>
        <td style=" width:5%;">
            &nbsp;</td>
        <td align="left" style="width: 15%; ">
            &nbsp;</td>
        <td align="left" style="width: 30%; ">
            &nbsp;</td>
    </tr>
    </table>  
    <asp:Panel ID="Panel1" runat="server">
        <fieldset style="border: solid 1px #8BB381;">
            <legend style="color: #FF0000">Search Option</legend>
            <table style="width: 100%">
                <tr>
                    <td style="width: 10%">
                        &nbsp;</td>
                    <td align="right" style="width: 12%">
                        Code No</td>
                    <td style="width: 3%">
                        &nbsp;</td>
                    <td style="width: 15%">
                        <asp:TextBox ID="txtOrderNo" runat="server" AutoPostBack="True" Width="98%"></asp:TextBox>
                        <ajaxToolkit:AutoCompleteExtender ID="autoComplete3" runat="server" 
                            CompletionInterval="1000" CompletionSetCount="12" EnableCaching="true" 
                            MinimumPrefixLength="1" ServiceMethod="GetPONoForSearch" 
                            ServicePath="AutoComplete.asmx" TargetControlID="txtOrderNo" />
                    </td>
                    <td align="right" style="width: 12%">
                        &nbsp;</td>
                    <td style="width: 3%">
                        &nbsp;</td>
                    <td style="width: 15%">
                        &nbsp;</td>
                    <td align="right" style="width: 15%">
                        &nbsp;</td>
                    <td style="width: 15%">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 10%">
                        &nbsp;</td>
                    <td align="right" style="width: 12%">
                        <asp:Label ID="Label40" runat="server" Text="From Date"></asp:Label>
                    </td>
                    <td style="width: 3%">
                        &nbsp;</td>
                    <td style="width: 15%">
                        <asp:TextBox ID="txtFromDate" runat="server" Width="98%"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="Calendarextender3" runat="server" Format="dd/MM/yyyy" 
                            TargetControlID="txtFromDate" />
                    </td>
                    <td align="right" style="width: 12%">
                        <asp:Label ID="Label41" runat="server" Text="To Date"></asp:Label>
                    </td>
                    <td style="width: 3%">
                        &nbsp;</td>
                    <td style="width: 15%">
                        <asp:TextBox ID="txtToDAte" runat="server" Width="98%"></asp:TextBox>
                        
                        <ajaxToolkit:CalendarExtender ID="txtFromDate0_calendarextender" runat="server" 
                            Format="dd/MM/yyyy" TargetControlID="txtToDAte" />
                    </td>
                    <td align="center" style="width: 15%">
                        <asp:LinkButton ID="lbSearch" runat="server" BorderColor="#33CCFF" 
                            BorderStyle="Solid" Font-Bold="True" Font-Size="12pt" Height="22px" 
                            onclick="lbSearch_Click" Style="text-align: center" Width="120px">Search</asp:LinkButton>
                    </td>
                    <td style="width: 15%">
                        <asp:LinkButton ID="lbClear" runat="server" BorderColor="#33CCFF" 
                            BorderStyle="Solid" Font-Bold="True" Font-Size="12pt" Height="22px" 
                            onclick="lbClear_Click" Style="text-align: center" Width="120px">Clear</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </fieldset></asp:Panel>
    <table ID="Table1" style="width:100%;">
        <tr>
            <td align="left">
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:GridView ID="dgPOrderMst" runat="server" AllowPaging="True" 
                    AllowSorting="True" AlternatingRowStyle-CssClass="alt" 
                    AutoGenerateColumns="False" BackColor="White" BorderColor="LightGray" 
                    BorderStyle="Solid" BorderWidth="1px" CellPadding="2" CssClass="mGrid" 
                    Font-Size="9pt" onpageindexchanging="dgPOrderMst_PageIndexChanging" 
                    onrowdatabound="dgPOrderMst_RowDataBound" 
                    onselectedindexchanged="dgPOrderMst_SelectedIndexChanged" 
                    PagerStyle-CssClass="pgr" PageSize="30" Width="100%">
                    <HeaderStyle BackColor="LightGray" Font-Bold="True" Font-Size="9pt" 
                        ForeColor="Black" HorizontalAlign="center" />
                    <Columns>
                     <asp:TemplateField HeaderText="">
                             <ItemTemplate>
                                  <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" 
                                                                    
                                      Checked='<%# Eval("check").ToString().Equals("1") %>' 
                                      oncheckedchanged="chkSelect_CheckedChanged"/>
                                 </ItemTemplate>
                             <FooterStyle HorizontalAlign="Center" />
                             <ItemStyle HorizontalAlign="Center" Width="12%" /></asp:TemplateField>

                     
                        <asp:BoundField DataField="ID" HeaderText="ID" ItemStyle-HorizontalAlign="Left" 
                            ItemStyle-Width="80px">
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Code" HeaderText="Code" 
                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Date" DataFormatString="{0:dd/MM/yyyy}" 
                            HeaderText="Date" ItemStyle-HorizontalAlign="Center" 
                            ItemStyle-Width="100px">
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="RequisitionBy" HeaderText="Requisition By" 
                            ItemStyle-Height="20" ItemStyle-HorizontalAlign="Left" 
                            ItemStyle-Width="200px">
                            <ItemStyle Height="20px" HorizontalAlign="Left" Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Status" 
                            HeaderText="Status">
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:BoundField>
                           <asp:CommandField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px" 
                            ShowSelectButton="True" SelectText="View">
                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                        </asp:CommandField>
                    </Columns>
                    <RowStyle BackColor="White" />
                    <SelectedRowStyle BackColor="" Font-Bold="True" />
                    <PagerStyle BackColor="LightGray" ForeColor="Black" HorizontalAlign="Center" />
                    <AlternatingRowStyle BackColor="" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="left">
                &nbsp;</td>
        </tr>
    </table>
    </fieldset>
    </ContentTemplate></asp:UpdatePanel>
     <br /> 

<%--<div runat="server" id="PVDetails">--%>
 </td>
 <td style="width:1%;" align="center"></td>
</tr>
<tr>
<td style="width:1%;" align="center">&nbsp;</td>
<td style="width:98%;" align="center"> 	
<%--<div runat="server" id="PVDetails">--%>

    <asp:Panel ID="LoginPanel1" runat="server" CssClass="modalPopup" 
        Style="padding: 15px 15px 15px 15px; background-color: White; border: 1px solid black;" 
        Width="70%">
        <table align="center" style="width: 100%;">
            <tr>
                <td colspan="3">
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="3" style="text-align:center;">
                   
                    <asp:GridView ID="dgDetails" runat="server" AutoGenerateColumns="False" 
                        BorderColor="LightGray" CssClass="mGrid" Font-Size="9pt"                         
                        Width="100%">
                        <AlternatingRowStyle CssClass="alt" />
                        <Columns>
                        
                            <asp:TemplateField HeaderText="Item Code">
                                <ItemTemplate>
                                <asp:Label ID="lblItemCode" runat="server" Font-Size="8pt" Text='<%#Eval("item_code")%>' 
                                        Width="95%"></asp:Label>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" Width="12%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>

                                 <asp:Label ID="lbl" runat="server" Font-Size="8pt" Text='<%#Eval("item_desc")%>' 
                                        Width="95%"></asp:Label>                                 
                                  
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" Width="30%" />
                            </asp:TemplateField>
                          
                            <asp:TemplateField HeaderText="Item Rate">
                                <ItemTemplate>
                                <asp:Label ID="lbl" runat="server" Font-Size="8pt" Text='<%#Eval("item_rate")%>' 
                                        Width="95%"></asp:Label>                                   
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Quantity">
                                <ItemTemplate>
                                <asp:Label ID="lbl" runat="server" Font-Size="8pt" Text='<%#Eval("qnty")%>' 
                                        Width="95%"></asp:Label>
                                   
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle Font-Bold="True" Font-Size="9pt" ForeColor="White" />
                        <PagerStyle CssClass="pgr" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="White" />
                    </asp:GridView>
                   
                </td>
            </tr>
            <tr>
                <td style="width: 30%; text-align:center;">
                    <asp:Button ID="btnCanceltd" runat="server" Font-Size="8pt" 
                       OnClientClick="HideModalDiv();" Text="Cancel" 
                        Width="80px" />
                </td>
                
                <td style="width: 40%">
                </td>
                <td style="width: 40%">
                </td>
            </tr>
        </table>
    </asp:Panel>

 </td>
 <td style="width:1%;" align="center">&nbsp;</td>
</tr>
<tr>
<td style="width:1%;" align="center">&nbsp;</td>
<td style="width:98%;" align="center"> 	
    &nbsp;</td>
 <td style="width:1%;" align="center">&nbsp;</td>
</tr>
<tr>
<td style="width:1%;" align="center">&nbsp;</td>
<td style="width:98%;" align="center"> 	
     <asp:Panel ID="pnlVoucher" runat="server"  CssClass="modalPopup" 
                            Style=" display:none; background-color: White; width:800px; height:auto; ">
                            <fieldset style=" text-align:left; vertical-align: top; border: solid 1px #8BB381;line-height:1.5em;"><legend style="color: maroon;"><b>View Voucher Information</b></legend>
                            <asp:Label runat="server" ID="lblPartuculars"></asp:Label>
                                 
                                <br/>
                                <asp:button id="btnCancel" runat="server" Text="Cancel" Font-Bold="True"  
                                Font-Size="20pt" />
                            </fieldset>
                        </asp:Panel></td>
 <td style="width:1%;" align="center">&nbsp;</td>
</tr>
</table>
</div>
<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;
        top: 0; left:0; background-color: Black; filter: alpha(opacity=60); opacity: 0.6; -moz-opacity: 0.8;-webkit-opacity: 0.8;display:none">
    </div>
</asp:Content>

