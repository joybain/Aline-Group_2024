﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmManafactureRequisition.aspx.cs" Inherits="frmManafactureRequisition" %>

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
        <asp:Button ID="btnDelete" runat="server" ToolTip="Delete" onclick="Delete_Click"
            
                onclientclick="javascript:return window.confirm('are u really want to delete these data')" Text="Delete" 
        Height="30px" Width="110px" BorderStyle="Outset"  />
        </td>       
        <td align="center" >
            &nbsp;</td>       
        <td align="center" >
        
            <asp:Button ID="btnSaveinf" runat="server" onclick="btnSave_Click" 
                Text="Save" Height="30px" Width="110px" BorderStyle="Outset"  />
        
        </td>
        <td align="center" >
        <asp:Button ID="btnNew" runat="server" ToolTip="New" onclick="btnNew_Click"  Text="New" 
        Height="30px" Width="110px" BorderStyle="Outset"  /> 
        </td>
        <td align="center" >
        <asp:Button ID="btnClear" runat="server"  ToolTip="Clear" onclick="Clear_Click" Text="Clear" 
        Height="30px" Width="110px" BorderStyle="Outset"  />
        </td>
        <td align="center" >
        <asp:Button ID="btnPrint" runat="server" ToolTip="Print PO" Text="Print" 
        Height="30px" Width="110px" BorderStyle="Outset" onclick="btnPrint_Click"  />
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
	<tr >
	<td style="width: 13%; " align="left">
	<asp:Label ID="lblQuotNo" runat="server" Font-Size="9pt">Code No</asp:Label></td>
	<td style="width: 30%; " align="left">
    <asp:TextBox SkinId="tbPlain" ID="txtPONo" runat="server" Width="40%"  
            CssClass="tbc" style="text-align:center;"
            AutoPostBack="False"  Font-Size="8pt" ReadOnly="True"></asp:TextBox>    
    </td>
    <td style=" width:5%;">
        <asp:Label ID="lbLId" runat="server" Font-Size="9pt" Visible="False"></asp:Label>
        </td>
	<td style="width: 15%; " align="left">
	<asp:Label ID="lblQuotDate" runat="server" Font-Size="9pt"> Date</asp:Label></td>
	<td style="width: 30%; " align="left">
    <asp:TextBox SkinId="tbPlain" ID="txtPoDate" runat="server" Width="40%" style="text-align:center;"
            CssClass="tbc"  AutoPostBack="False"  Font-Size="8pt"></asp:TextBox>
    <ajaxtoolkit:calendarextender runat="server" ID="Calendarextender1" 
            TargetControlID="txtPoDate" Format="dd/MM/yyyy"/>
    </td>    
    </tr>

    <tr >
	<td style="width: 13%; " align="left">
    <asp:Label ID="LblSuppNo" runat="server" Font-Size="9pt" >Requisition By</asp:Label></td>
	<td style="width: 30%; " align="left">
        <asp:DropDownList ID="ddlManufacturedBy" runat="server" Width="98%">
        </asp:DropDownList>
    </td>
    <td style=" width:5%;" align="left">
        &nbsp;</td>
	<td style="width: 15%; " align="left">
	    &nbsp;</td>
	<td style="width: 30%; " align="left">
        &nbsp;</td>    
    </tr>

    <tr>
    <td style="width: 13%; " align="left">
    <asp:Label ID="Label5" runat="server" Font-Size="9pt" Visible="False" >Exp. Delivery Date</asp:Label></td>
    <td style="width: 30%;" align="left">
        <asp:TextBox ID="txtDelDate" runat="server" AutoPostBack="False" CssClass="tbc" 
            Font-Size="8pt" SkinId="tbPlain" style="text-align:center;" Width="40%" 
            Visible="False"></asp:TextBox>
        <ajaxToolkit:CalendarExtender ID="Calendarextender2" runat="server" 
            Format="dd/MM/yyyy" TargetControlID="txtDelDate" />
    </td>    
    <td style=" width:5%;">&nbsp;</td>    
    <td style="width: 15%; " align="left">
        &nbsp;</td>
    <td style="width: 30%; " align="left">
        &nbsp;</td>   
    </tr>    

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
    <tr>
        <td align="left" colspan="5">
            <asp:GridView ID="dgPODetailsDtl" runat="server" AutoGenerateColumns="False" 
                BorderColor="LightGray" CssClass="mGrid" Font-Size="9pt" 
                OnRowDataBound="dgPurDtl_RowDataBound" OnRowDeleting="dgPurDtl_RowDeleting" 
                Width="100%">
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
                    <asp:TemplateField HeaderText="Item Code">
                        <ItemTemplate>
                            <asp:TextBox ID="txtItemCode" runat="server" AutoPostBack="true" 
                                Font-Size="8pt" MaxLength="15" onFocus="this.select()" 
                                ontextchanged="txtItemCode_TextChanged" SkinId="tbPlain" 
                                Text='<%#Eval("item_code")%>' Width="93%"></asp:TextBox>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="12%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:TextBox ID="txtItemDesc" runat="server" autocomplete="off" 
                                AutoPostBack="true" Font-Size="8pt" onFocus="this.select()" 
                                OnTextChanged="txtItemDesc_TextChanged" SkinId="tbPlain" 
                                Text='<%#Eval("item_desc")%>' Width="97%">
                            </asp:TextBox>
                            <ajaxToolkit:AutoCompleteExtender ID="autoComplete1" runat="server" 
                                CompletionInterval="10" CompletionSetCount="12" EnableCaching="true" 
                                MinimumPrefixLength="1" ServiceMethod="GetItemListBarcode" 
                                ServicePath="AutoComplete.asmx" TargetControlID="txtItemDesc" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" Width="30%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Msr Unit">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlMeasure" runat="server" AutoPostBack="true" 
                                DataSource="<%#PopulateMeasure()%>" DataTextField="Name" DataValueField="ID" 
                                Enabled="False" Font-Size="8pt" Height="26px" 
                                SelectedValue='<%#Eval("msr_unit_code")%>' SkinId="ddlPlain" Width="95%">
                            </asp:DropDownList>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Item Rate">
                        <ItemTemplate>
                            <asp:TextBox ID="txtItemRate" runat="server" AutoPostBack="False" 
                                CssClass="tbr" Font-Size="8pt" onFocus="this.select()" 
                                onkeypress="return isNumber(event)" SkinId="tbPlain" 
                                Text='<%#Eval("item_rate")%>' Width="93%">
                            </asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:TextBox ID="txtQnty" runat="server" AutoPostBack="true" CssClass="tbc" 
                                Font-Size="8pt" onFocus="this.select()" onkeypress="return isNumber(event)" 
                                OnTextChanged="txtQnty_TextChanged" SkinId="tbPlain" Text='<%#Eval("qnty")%>' 
                                Width="93%"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total">
                        <ItemTemplate>
                            <asp:Label ID="lblTotal" runat="server" Font-Size="8pt" Width="95%"> </asp:Label>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ID">
                        <ItemTemplate>
                            <asp:Label ID="lblid0" runat="server" Font-Size="8pt" Text='<%#Eval("ID")%>' 
                                Width="95%"></asp:Label>
                        </ItemTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Right" Width="10%" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle Font-Bold="True" Font-Size="9pt" ForeColor="White" />
                <PagerStyle CssClass="pgr" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="White" />
            </asp:GridView>
        </td>
    </tr>
    </table>  
    <table ID="Table1" style="width:100%;">
        <tr>
            <td align="left">
                &nbsp;</td>
        </tr>
    </table>
    </fieldset>
    </ContentTemplate></asp:UpdatePanel>
     <br /> 

    <%--<ajaxtoolkit:TabContainer ID="tabVch" runat="server" Width="99%" ActiveTabIndex="0" 
         Font-Size="8pt">
 <ajaxtoolkit:TabPanel ID="tpVchDtl" runat="server" HeaderText="Items Details">
     <ContentTemplate>--%>
 </td>
 <td style="width:1%;" align="center"></td>
</tr>
<tr>
<td style="width:1%;" align="center">&nbsp;</td>
<td style="width:98%;" align="center"> 	
    <%--<ajaxtoolkit:TabContainer ID="tabVch" runat="server" Width="99%" ActiveTabIndex="0" 
         Font-Size="8pt">
 <ajaxtoolkit:TabPanel ID="tpVchDtl" runat="server" HeaderText="Items Details">
     <ContentTemplate>--%>

<div style="font-size: 8pt;" align="center">
<asp:UpdatePanel ID="PVIesms_UP" runat="server"  UpdateMode="Conditional">
 <ContentTemplate>
<%--<ajaxtoolkit:TabContainer ID="tabVch" runat="server" Width="99%" ActiveTabIndex="0" 
         Font-Size="8pt">
 <ajaxtoolkit:TabPanel ID="tpVchDtl" runat="server" HeaderText="Items Details">
     <ContentTemplate>--%>
         <table style="width:100%;"><tr>
             <td colspan="2" align="center">
                 &nbsp;</td></tr>
             <tr>
             <td>
             <asp:Panel ID="pnlClient" runat="server" CssClass="modalPopup1" Style="padding:15px 15px 15px 15px; display:none; background-color:White; border:1px solid black;" Width="700px">
  <fieldset style="vertical-align: top; border: solid 1px #8BB381;">
                        <legend style="color: maroon;"><b>Save Data</b></legend>     
<table style="width:100%;">
<tr>
<td style="width:15%;" align="left">
    &nbsp;</td>
<td style="width:16%;" align="right"> 
    <asp:Label ID="Label3" runat="server" Text="Type"></asp:Label></td>
<td style=" width:4%;" >&nbsp;</td>
<td style="width:41%;" align="left">
    <asp:DropDownList SkinID="ddlPlain" ID="ddlType" runat="server"  
                    Font-Size="8" Width="104%" TabIndex="2" Height="26px">  
  <asp:ListItem Value="S">Supplier</asp:ListItem>
  </asp:DropDownList></td>
<td style="width:25%;" align="left" > 
    &nbsp;</td>
</tr>
<tr>
<td style="width:15%;" align="left">
    &nbsp;</td>
<td style="width:16%;" align="right"> 
    <asp:Label ID="Label4" runat="server" Text="Name"></asp:Label>
    </td>
<td style=" width:4%;" >&nbsp;</td>
<td style="width:41%;" align="left">
    <asp:TextBox ID="txtvalue" runat="server" Width="100%"></asp:TextBox>
    
</td>
<td style="width:25%;" align="left" > 
    &nbsp;</td>
</tr>
    <tr>
        <td align="left" style="width:15%;">
            &nbsp;</td>
        <td align="right" style="width:16%;">
            <asp:Label ID="Label38" runat="server" Text="Mobile"></asp:Label>
        </td>
        <td style=" width:4%;">
            &nbsp;</td>
        <td align="left" style="width:41%;">
            <asp:TextBox ID="txtMobile" runat="server" Width="100%"></asp:TextBox>
        </td>
        <td align="left" style="width:25%;">
            &nbsp;</td>
    </tr>
    <tr>
        <td align="left" style="width:15%;">
            &nbsp;</td>
        <td align="right" style="width:16%;">
            <asp:Label ID="Label39" runat="server" Text="E-mail"></asp:Label>
        </td>
        <td style=" width:4%;">
            &nbsp;</td>
        <td align="left" style="width:41%;">
            <asp:TextBox ID="txtEmail" runat="server" Width="100%"></asp:TextBox>
        </td>
        <td align="left" style="width:25%;">
            &nbsp;</td>
    </tr>
    <tr>
        <td align="left" style="width:15%;">
            &nbsp;</td>
        <td align="right" style="width:16%;">
            &nbsp;</td>
        <td style=" width:4%;">
            &nbsp;</td>
        <td align="left" style="width:41%;">
            &nbsp;</td>
        <td align="left" style="width:25%;">
            &nbsp;</td>
    </tr>
<tr>
<td style="width:100%;" colspan="5">
<table style="width:100%;">
  <tr>  
  <td style="width:5%;"></td>
  <td align="right" >
       <asp:Button ID="btnClientSave" runat="server" ToolTip="OK" 
           OnClientClick="HideModalDiv();" Text="OK" 
        Height="30px" Width="100px" BorderStyle="Outset"  Font-Size="9pt" onclick="btnClientSave_Click" 
            />
       </td>   
       <td style="width:20px;"></td>
       <td align="left" >
       <asp:Button ID="btnClientQuit" runat="server" ToolTip="Quit Client" Text="Quit" OnClientClick="HideModalDiv();"
        Height="30px" Width="100px" BorderStyle="Outset"  Font-Size="9pt" />
       </td>        
       <td style="width:5%;"></td>       
   </tr>
   </table>
</td>
</tr>
</table>   
</fieldset> 
    </asp:Panel>
             
             </td>
                 <td></td></tr>
             <tr>
                 <td colspan="2">
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
                                         <ajaxToolkit:CalendarExtender ID="Calendarextender3" runat="server" 
                                             Format="dd/MM/yyyy" TargetControlID="txtFromDate" />
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
                                 <tr>
                                     <td style="width: 10%">
                                         &nbsp;</td>
                                     <td align="right" style="width: 12%">
                                         &nbsp;</td>
                                     <td style="width: 3%">
                                         &nbsp;</td>
                                     <td style="width: 15%">
                                         &nbsp;</td>
                                     <td align="right" style="width: 12%">
                                         &nbsp;</td>
                                     <td style="width: 3%">
                                         &nbsp;</td>
                                     <td style="width: 15%">
                                         &nbsp;</td>
                                     <td align="center" style="width: 15%">
                                         &nbsp;</td>
                                     <td style="width: 15%">
                                         &nbsp;</td>
                                 </tr>
                                 <tr>
                                     <td colspan="9">
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
                                                 <asp:CommandField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px" 
                                                     ShowSelectButton="True">
                                                     <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                 </asp:CommandField>
                                                 <asp:BoundField DataField="ID" HeaderText="ID" ItemStyle-HorizontalAlign="Left" 
                                                     ItemStyle-Width="80px">
                                                     <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                 </asp:BoundField>
                                                 <asp:BoundField DataField="Code" HeaderText="Code" 
                                                     ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                                                     <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                 </asp:BoundField>
                                                 <asp:BoundField DataField="Date" DataFormatString="{0:dd/MM/yyyy}" 
                                                     HeaderText="Date" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                                                     <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                 </asp:BoundField>
                                                 <asp:BoundField DataField="RequisitionBy" HeaderText="Requisition By" 
                                                     ItemStyle-Height="20" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200px">
                                                     <ItemStyle Height="20px" HorizontalAlign="Left" Width="200px" />
                                                 </asp:BoundField>
                                             </Columns>
                                             <RowStyle BackColor="White" />
                                             <SelectedRowStyle BackColor="" Font-Bold="True" />
                                             <PagerStyle BackColor="LightGray" ForeColor="Black" HorizontalAlign="Center" />
                                             <AlternatingRowStyle BackColor="" />
                                         </asp:GridView>
                                     </td>
                                 </tr>
                             </table>
                         </fieldset></asp:Panel>
                 </td>
             </tr>
             <tr>
                 <td colspan="2">
                     &nbsp;</td>
             </tr>
             <tr>
                 <td>
                     &nbsp;</td>
                 <td>
                     &nbsp;</td>
             </tr>
     </table></ContentTemplate><%--</ajaxtoolkit:TabPanel></ajaxtoolkit:TabContainer>--%>
                 </asp:UpdatePanel></div>
 </td>
 <td style="width:1%;" align="center">&nbsp;</td>
</tr>
</table>
</div>
<div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;
        top: 0; left:0; background-color: Black; filter: alpha(opacity=60); opacity: 0.6; -moz-opacity: 0.8;-webkit-opacity: 0.8;display:none">
    </div>
</asp:Content>

