<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="FrmBtoBTransfer.aspx.cs" Inherits="FrmBtoBTransfer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">


<script type="text/javascript">

    $(document).ready(function () {
        setTimeout(function () {
            if ($("#MainContent_pnl_TabPanel1_tab").hasClass('ajax__tab_active')) {
                $("#MainContent_pnl_TabPanel1_txtItemSearch").focus();
            }


        }, 100);

    });
    function LoadModalDiv() {

        var bcgDiv = document.getElementById("divBackground");
        bcgDiv.style.display = "block";
        frmMainDiv.style.display = "block";


    }
    function HideModalDiv() {

        var bcgDiv = document.getElementById("divBackground");
        bcgDiv.style.display = "none";

    }
</script>

    <div id="frmMainDiv" style="background-color:White; width:100%;">
    <asp:HiddenField ID="hfMstID" runat="server"/>
       <ajaxToolkit:TabContainer ID="pnl" runat="server" ActiveTabIndex="0" 
            Width="100%">
                        <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Finish Goods">
                         
                            <HeaderTemplate>
                                Project To project Transfer
                            </HeaderTemplate>
                         
                            <ContentTemplate>   
   
<table style="width: 100%">
                                    <tr>
                                        <td style="width: 10%">
                                            &nbsp;</td>
                                        <td style="width: 80%">
                                            
                                            
                                            
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%; height: 17px;">
                                        </td>
                                        <td style="width: 80%; height: 17px;">
                                        <fieldset style="vertical-align: top; border: solid 1px #8BB381;"><legend style="color: maroon;"><b> 
                                            &nbsp;Project Transfer Qty</b></legend>

                                            <table style="width: 100%">
                                                <tr>
                                                    <td  style="width: 12%; height: 23px;" align="right">
                                                        <asp:Label ID="Label3" runat="server" Text="Transfer Code"></asp:Label>
                                                    </td>
                                                    <td style="width: 3%; color: #FF0000; height: 23px;" align="center">
                                                        &nbsp;</td>
                                                    <td style="width: 18%; height: 23px;">
                                                        <asp:TextBox ID="txtTransCode" runat="server" Width="98%" Enabled="False"></asp:TextBox>
                                                    </td>
                                                     <td  style="width: 12%; height: 23px;" align="right">
                                                         transfer Date :</td>
                                                    <td style="width: 3%; color: #FF0000; height: 23px;" align="center">
                                                        <asp:Label ID="lblID" runat="server" Visible="False"></asp:Label>
                                                    </td>
                                                    <td style="width: 18%; height: 23px;">
                                                         <asp:TextBox ID="txtTranstDate" runat="server" Width="98%"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" 
                                                            Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtTranstDate">
                                                        </ajaxToolkit:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" style="width: 12%; height: 23px;">
                                                        From Project </td>
                                                    <td align="center" style="width: 3%; color: #FF0000; height: 23px;">
                                                        *</td>
                                                    <td style="width: 18%; height: 23px;">
                               
                           
                                                        <asp:DropDownList ID="ddlProjectName" runat="server" AutoPostBack="True" 
                                                            Font-Size="8pt" Height="26px"                                                      
                                                            Width="100%" onselectedindexchanged="ddlProjectName_SelectedIndexChanged">
                                                        </asp:DropDownList>
                               
                           
                                                    </td>
                                                    <td align="right" style="width: 12%; height: 23px;">
                                                        <asp:Label ID="Label8" runat="server" Text="To Project"></asp:Label>
                                                    </td>
                                                    <td align="center" style="width: 3%; color: #FF0000; height: 23px;">
                                                        *</td>
                                                    <td style="width: 18%; height: 23px;">

                                                          <asp:DropDownList ID="ddlProjectNameTo" runat="server" AutoPostBack="True" 
                                                            Font-Size="8pt" Height="26px"                                                      
                                                            Width="100%">
                                                        </asp:DropDownList>

                                                       
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" style="width: 12%">
                                                        Remarks</td>
                                                    <td align="center" style="width: 3%; color: #FF0000; height: 23px;">
                                                        &nbsp;</td>
                                                    <td colspan="4">
                                                        <asp:TextBox ID="txtRemarks" runat="server" Width="100%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                         <asp:TextBox runat="server" ID="txtID"  Visible="False" />

                                                    </td>
                                                </tr>
                                            </table>
                                            </fieldset>
                                        </td>
                                        <td style="width: 10%; height: 17px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                            &nbsp;</td>
                                        <td style="width: 80%">
                                       
                                            <fieldset style="vertical-align: top; border: solid 1px #8BB381;">
                                                <legend style="color: maroon;"><b>&nbsp;Add Item</b></legend>
                                                 <div>
                                       
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 18%" align="right">
                                                        Search Item</td>
                                                    <td align="center" style="width: 5%; color: #FF0000; height: 23px;">
                                                        *</td>
                                                    <td style="width: 50%">
                                                        <asp:TextBox ID="txtItemSearch" runat="server" AutoPostBack="True" 
                                                             placeholder="Search by Item.." 
                                                            Width="96%" Enabled="False" ontextchanged="txtItemSearch_TextChanged"></asp:TextBox>
                                                        <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" 
                                                            CompletionInterval="20" CompletionSetCount="12" DelimiterCharacters="" 
                                                            Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetItemCostProject" 
                                                            ServicePath="AutoComplete.asmx" TargetControlID="txtItemSearch" />
                                                    </td>
                                                      <td align="left" style="width: 30%;  height: 23px; font-weight: 700;">
                                                          <span style="color: #009933">Total Quantity: </span>
                                                          <asp:Label ID="lblQuantity" runat="server" Text="0"></asp:Label>
                                                        </td>

                                                          
                                                </tr>
                                            </table>
                                            
                                        
                                        </div>
                                         
                                                <asp:GridView ID="dgPODetailsDtl" runat="server" AutoGenerateColumns="False" 
                                                    BorderColor="LightGray" CssClass="mGrid" Font-Size="9pt" 
                                                     Width="100%"  
                                                     AllowPaging="True" 
                                                     PageSize="20" onrowdatabound="dgPODetailsDtl_RowDataBound" 
                                                    onrowdeleting="dgPODetailsDtl_RowDeleting" >
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                       <asp:TemplateField>
                                                <ItemTemplate>
                                                <asp:ImageButton ID="lbDelete" runat="server"  CommandName="Delete" ImageAlign="Middle" ImageUrl="~/img/delete.png" Text="Delete" />
                                                  
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="8%" />
                                            </asp:TemplateField>
                                                        <asp:BoundField DataField="ID" HeaderText="ItemID" />
                                                     
                                                        <asp:BoundField DataField="Code" HeaderText="Code" />
                                                        <asp:BoundField DataField="Name" HeaderText="Item Name" />
                                                        
                                                        <asp:BoundField DataField="Uom" HeaderText="UOM" />
                                                         <asp:BoundField DataField="UomId" HeaderText="UOMId" />
                                                        <asp:TemplateField HeaderText="Quantity">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtQnty" runat="server" AutoPostBack="true" CssClass="tbc" 
                                                                    Font-Size="8pt" onFocus="this.select()" 
                                                                    SkinId="tbPlain" Text='<%#Eval("qnty")%>'  ontextchanged="txtQnty_TextChanged" 
                                                                    ></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                        </asp:TemplateField>
                                                       
                                                       <asp:TemplateField HeaderText="Remarks">
                                                            <ItemTemplate>
                                                                  <asp:TextBox ID="txtRemarks" runat="server"  CssClass="tbc" 
                                                                    Font-Size="8pt" onFocus="this.select()" 
                                                                    SkinId="tbPlain" Text='<%#Eval("Remarks")%>' Width="93%" 
                                                                   ontextchanged="txtRemarks_TextChanged" ></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="20%" />
                                                        </asp:TemplateField>

                                                         
                                                    </Columns>
                                                    <HeaderStyle Font-Bold="True" Font-Size="9pt" ForeColor="White" />
                                                    <PagerStyle CssClass="pgr" ForeColor="White" HorizontalAlign="Center" />
                                                    <RowStyle BackColor="White" />
                                                </asp:GridView>
                                               
                                            </fieldset>
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%; height: 16px;">
                                            </td>
                                        <td style="width: 80%; height: 16px;">
                                            </td>
                                        <td style="width: 10%; height: 16px;">
                                            </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="center">
                                            
                                            <table ID="pageFooterWrapper">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Button ID="btnDelete" runat="server" BorderStyle="Outset" Height="35px" 
                                                            
                                                            onclientclick="javascript:return window.confirm('are u really want to delete these data')" 
                                                            Text="Delete" ToolTip="Delete" Width="110px" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Button ID="btnView" runat="server" BorderStyle="Outset" Height="35px" 
                                                            Text="Print" Width="110px" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Button ID="btnFindLost" runat="server" BorderStyle="Outset" Height="35px" 
                                                             Text="Find" Width="110px" Visible="False" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Button ID="btnSaveLost" runat="server" BorderStyle="Outset" Height="35px" 
                                                             Text="Transfer " ToolTip="Save Purchase Record" 
                                                            Width="110px" onclick="btnProjectranfer_Click" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Button ID="btnUpdate" runat="server" BorderStyle="Outset" Height="35px" 
                                                             Text="Update Trans" ToolTip="Save Purchase Record" 
                                                            Width="110px"  />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Button ID="btnClearLost" runat="server" BorderStyle="Outset" Height="35px" 
                                                           
                                                            onclientclick="javascript:return window.confirm('are u really want to Clear these data')" 
                                                            Text="Clear" ToolTip="Clear" Width="110px" onclick="btnClearLost_Click" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:Button ID="btnPrintLost" runat="server" BorderStyle="Outset" Height="35px" 
                                                            OnClientClick="LoadModalDiv();" Text="Preview" 
                                                            ToolTip="Print PO" Width="110px" Visible="False" />
                                                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderLogin" runat="server" 
                                                            BackgroundCssClass="modalBackground" DropShadow="True" DynamicServicePath="" 
                                                            Enabled="True" PopupControlID="LoginPanel" TargetControlID="btnCancel">
                                                        </ajaxToolkit:ModalPopupExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Panel ID="LoginPanel" runat="server" CssClass="modalPopup" 
                                                Style=" display:none;padding: 15px 15px 15px 15px; background-color: White; border: 1px solid black;" 
                                                Width="80%" ScrollBars="Both">

                    <table style="width: 100%;">
                        <tr>
                            <td colspan="3">
                            <div id="output-tbl" style="max-width: 100%; float: left; overflow: auto; position: relative;">

                            <asp:PlaceHolder ID = "PlaceHolder1" runat="server" />
        </div>
                                </td>
                            
                        </tr>
                        <tr>
                         <td style="width: 40%">
                                
                            </td>
                            <td style="width: 30%; text-align:center;">
                                <asp:Button ID="CancelBtn" runat="server" Font-Size="8pt" 
                                     OnClientClick="HideModalDiv();" Text="Cancel" 
                                    Width="80px" />
                            </td>
                            
                            <td style="width: 40%">
                                <asp:Button ID="btnPrintd" runat="server" Font-Size="8pt" 
                                     OnClientClick="HideModalDiv();" 
                                    Text="Print"  Width="80px" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel></td>
                                        
                                    </tr>
                                </table>
  <asp:Panel ID="pnlVoucher" runat="server"  CssClass="modalPopup" 
                            Style=" display:none; background-color: White; width:800px; height:auto; ">
                            <fieldset style=" text-align:left; vertical-align: top; border: solid 1px #8BB381;line-height:1.5em;"><legend style="color: maroon;"><b>View Voucher Information</b></legend>
                            <asp:Label runat="server" ID="lblPartuculars"></asp:Label>
                                 
                                <br/>
                                <asp:button id="btnCancel" runat="server" Text="Cancel" Font-Bold="True" OnClientClick="HideModalDiv();" 
                                Font-Size="20pt" />
                            </fieldset>
                        </asp:Panel>
   </ContentTemplate>
   </ajaxToolkit:TabPanel>

   <ajaxToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="History">
                            <HeaderTemplate>
                                Transfer History
                            </HeaderTemplate>
                           <ContentTemplate>
                           
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 5%">
                                            &nbsp;</td>
                                        <td style="width: 90%">
                                            
                                            &nbsp;</td>
                                        <td style="width: 5%">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 5%">
                                            &nbsp;</td>
                                        <td align="right" style="width: 90%">
                                            &nbsp;<fieldset style="vertical-align: top; border: solid 1px #8BB381;">
                                                <legend style="color: maroon;">Search History</legend>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 15%; font-weight: 700;">
                                                            Date</td>
                                                        <td align="left" style="width: 25%">
                                                            <asp:TextBox ID="txtsearchDatefrom" runat="server" Width="98%"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="txtReceiveddate0_CalendarExtender" 
                                                                runat="server" Enabled="True" Format="dd/MM/yyyy" 
                                                                TargetControlID="txtsearchDatefrom">
                                                            </ajaxToolkit:CalendarExtender>
                                                        </td>
                                                        <td align="center" style="width: 10%; font-weight: 700;">
                                                            To</td>
                                                        <td align="left" style="width: 25%">
                                                            <asp:TextBox ID="txtsearchDateTo" runat="server" Width="98%"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="txtReceiveddate1_CalendarExtender" 
                                                                runat="server" Enabled="True" Format="dd/MM/yyyy" 
                                                                TargetControlID="txtsearchDateTo">
                                                            </ajaxToolkit:CalendarExtender>
                                                        </td>
                                                        <td style="width: 3%">
                                                            &nbsp;</td>
                                                        <td style="width: 15%">
                                                            &nbsp;</td>
                                                        <td style="width: 10%">
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 15%; font-weight: 700;">
                                                            Project Name:</td>
                                                        <td align="left" colspan="3">
                                                            <asp:DropDownList ID="ddlProjectNameSerch" runat="server" AutoPostBack="True" 
                                                                Font-Size="8pt" Height="26px" SkinID="ddlPlain" Width="100%">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="width: 3%">
                                                            &nbsp;</td>
                                                        <td align="left" style="width: 15%">
                                                            <asp:Button ID="btnSearch" runat="server" BorderStyle="Outset" Height="25px" 
                                                                 Text="Search" ToolTip="Authorize" Width="100px" 
                                                                 />
                                                        </td>
                                                        <td style="width: 10%">
                                                            <asp:Button ID="btnClearAll" runat="server" BorderStyle="Outset" Height="25px" 
                                                               Text="Refresh" ToolTip="Authorize" Width="100px" 
                                                                 />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 15%; font-weight: 700;">
                                                            &nbsp;</td>
                                                        <td align="left" colspan="3">
                                                            &nbsp;</td>
                                                        <td style="width: 3%">
                                                            &nbsp;</td>
                                                        <td align="left" style="width: 15%">
                                                            <asp:HiddenField ID="DgHistoryId" runat="server" />
                                                        </td>
                                                        <td style="width: 10%">
                                                            &nbsp;</td>
                                                    </tr>
                                                </table>
                                            </fieldset></td>
                                        <td style="width: 5%">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 5%">
                                            &nbsp;</td>
                                        <td align="right" style="width: 90%">
                                 
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 25%; height: 17px;">
                                                        </td>
                                                    <td style="width: 25%; height: 17px;">
                                                        </td>
                                                    <td style="width: 25%; height: 17px;">
                                                        </td>
                                                    <td style="width: 25%; height: 17px;">
                                                        </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 5%">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 5%">
                                            &nbsp;</td>
                                        <td style="width: 90%">
                                            <fieldset style="vertical-align: top; border: solid 1px #8BB381;">
                                                <legend style="color: maroon;">History</legend>

                                                <asp:GridView ID="dgHistory" runat="server" AutoGenerateColumns="False" 
                                                    BorderColor="LightGray" CssClass="mGrid" Font-Size="9pt" 
                                                    Width="100%" 
                                                     AllowPaging="True" 
                                                     PageSize="20" onselectedindexchanged="dgHistory_SelectedIndexChanged"    onrowdatabound="dgHistory_RowDataBound" >
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                       

                                                      
                                                        <asp:CommandField ShowSelectButton="True" />
                                                       

                                                      
                                                        <asp:BoundField DataField="ID" HeaderText="ID" />
                                                        
                                                        <asp:BoundField DataField="TransferCode" HeaderText="Transfer Code" >
                                                        <ItemStyle HorizontalAlign="Center" Width="25%" /></asp:BoundField>
                                                        
                                                       
                                                        <asp:BoundField DataField="TransferDate" HeaderText="Transfer Date" >
                                                        <ItemStyle HorizontalAlign="Center" Width="15%" /></asp:BoundField>
                                                       
                                                        <asp:BoundField DataField="FromProjectid" HeaderText="From Project Name" >
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" /></asp:BoundField>
                                                        <asp:BoundField DataField="ToProjectid" HeaderText="To Project Name" >
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" /></asp:BoundField>
                                                        <asp:BoundField DataField="AddBy" HeaderText="Add By" >
                                                        <ItemStyle HorizontalAlign="Center" Width="10%" /></asp:BoundField>
                                                         <asp:BoundField DataField="TotalQuantity" HeaderText="Total Quantity" >
                                                            <ItemStyle HorizontalAlign="Center" Width="8%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" >
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" /></asp:BoundField>
                                                    </Columns>
                                                    <HeaderStyle Font-Bold="True" Font-Size="9pt" ForeColor="White" />
                                                    <PagerStyle CssClass="pgr" ForeColor="White" HorizontalAlign="Center" />
                                                    <RowStyle BackColor="White" />
                                                </asp:GridView>
                                                
                                            </fieldset>&nbsp;</td>
                                        <td style="width: 5%">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 5%">
                                            &nbsp;</td>
                                        <td style="width: 90%">
                                            &nbsp;</td>
                                        <td style="width: 5%">
                                            &nbsp;</td>
                                    </tr>
                                </table>
                                
    
                            </ContentTemplate>
                        </ajaxToolkit:TabPanel>
   </ajaxToolkit:TabContainer>
 
        </div>
        <div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;
        top: 0; left:0; background-color: Black; ; -moz-opacity: 0.8;-webkit-opacity: 0.8;display:none">
    </div>
   
</asp:Content>

