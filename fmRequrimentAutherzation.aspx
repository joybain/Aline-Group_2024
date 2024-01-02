<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="fmRequrimentAutherzation.aspx.cs" Inherits="fmRequrimentAutherzation" %>

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

<table  id="pageFooterWrapper">
 <tr>
	<td align="center">
    
	    <asp:Button  ID="BtnDelete" runat="server"  ToolTip="Delete Record"   
            
            onclientclick="javascript:return window.confirm('are u really want to delete  these data')" Text="Delete" 
        Height="35px" Width="100px" BorderStyle="Outset" />
 </td>	
    <td align="center">
        &nbsp;</td>
	
	<td align="center">
        <asp:Button ID="txtBtnSave" runat="server" 
            UseSubmitBehavior="false" ToolTip="Save or Update Record" 
             Text="Autheriz"  
        Height="35px" Width="100px" BorderStyle="Outset" onclick="BtnSave_Click"  /></td>
         <td align="center">
             &nbsp;</td> 
	<td align="center">
        <asp:Button ID="BtnReset" runat="server" ToolTip="Clear Form" 
             Text="Clear" 
        Height="35px" Width="100px" BorderStyle="Outset" onclick="BtnReset_Click"   />
    </td>    
   
	</tr>		
</table>
<div id="frmMainDiv" style="background-color:White; width:100%;">
    <asp:HiddenField ID="hfMstID" runat="server"/>
       <ajaxToolkit:TabContainer ID="pnl" runat="server" ActiveTabIndex="1" 
            Width="100%">
                        <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Finish Goods">
                         
                            <HeaderTemplate>Autherization</HeaderTemplate>
                         
                            <ContentTemplate>   

                              <div id="mainDiv"><table style="width: 100%; margin-top:15px;"><tr><td style="width: 10%"></td><td style="width: 80%; "><fieldset style="vertical-align: top; border: solid 1px #8BB381;"><legend style="color: maroon; font-weight: 700;">AutherZation Requirment <b></b></legend><table style="width: 100%"><tr><td style="width: 10%; text-align: center;"><b>Project Name</b></td><td style="width: 5%; text-align: center;">:</td><td style="width: 20%"><asp:DropDownList ID="ddlProjectName" runat="server" AutoPostBack="True" 
                                    Font-Size="8pt" Height="26px"                                                      
                                    Width="100%" onselectedindexchanged="ddlProjectName_SelectedIndexChanged"></asp:DropDownList></td><td style="width: 10%; text-align: center;"><b>&nbsp;&nbsp;&nbsp; Address </b></td>
                                    <td style="width: 5%; text-align: center;">:</td>
                                    <td style="width: 20%"><asp:TextBox ID="txtSite" runat="server" Width="146px"></asp:TextBox></td></tr><tr><td style="width: 19%; text-align: center;">Requisition Code</td><td style="width: 5%; text-align: center;">:</td>
                                                           <td colspan="4"><table style="width: 100%"><tr><td style="width: 50%">
                                                            <asp:TextBox ID="txtItemSearch" runat="server" AutoPostBack="True" 
                                                             placeholder="..."  ontextchanged="txtItemSearch_TextChanged" 
                                                            Width="96%" Enabled="False"></asp:TextBox>
                                                            <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" 
                                                            CompletionInterval="20" CompletionSetCount="12" DelimiterCharacters="" 
                                                            Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetItemCostProject1" 
                                                            ServicePath="AutoComplete.asmx" TargetControlID="txtItemSearch" />
                                                            </td><td align="left" style="width: 30%;  height: 23px; font-weight: 700;">
                                                                     <span style="color: #009933">Total Quantity: </span><asp:Label ID="lblQuantity" runat="server" Text="0"></asp:Label>
                                                                 </td></tr></table></td></tr><tr><td style="width: 19%; text-align: center;">&nbsp;</td><td style="width: 5%; text-align: center;">&nbsp;</td><td style="width: 20%">&nbsp;</td><td style="width: 10%; text-align: center;">&nbsp;</td><td style="width: 5%; text-align: center;">&nbsp;</td><td style="width: 20%">&nbsp;</td></tr></table></fieldset> <fieldset style="vertical-align: top; border: solid 1px #8BB381;"><legend style="color: maroon; font-weight: 700;">Requiment Authenation <b 
                          style="text-align: center"></b></legend><table style="width: 100%"><tr><td style="width: 100%; text-align: center; font-size: large;">
                                      <asp:HiddenField ID="hidMstId" runat="server" />
                                      </td><td style="width: 100%; text-align: center;">&nbsp;</td></tr><tr><td style="width: 100%" colspan="2">
   
   <asp:GridView ID="dgPODetailsDtl" runat="server" AutoGenerateColumns="False" 
                                                    BorderColor="LightGray" CssClass="mGrid" Font-Size="9pt" 
                                                     Width="100%"  
                                                     AllowPaging="True" 
                                                     PageSize="20" onrowdatabound="dgPODetailsDtl_RowDataBound" 
                                                    onrowdeleting="dgPODetailsDtl_RowDeleting" ><AlternatingRowStyle CssClass="alt" /><Columns><asp:TemplateField>
 <ItemTemplate><asp:ImageButton ID="lbDelete" runat="server"  CommandName="Delete" ImageAlign="Middle" ImageUrl="~/img/delete.png" Text="Delete" /></ItemTemplate><ItemStyle HorizontalAlign="Center" Width="8%" />
                                                                                                                                               </asp:TemplateField><asp:BoundField DataField="id" HeaderText="ID" />
                                                                                                                                               <asp:BoundField DataField="item_Code" HeaderText="item_Code" />
                                                                                                                                               <asp:BoundField DataField="item_desc" HeaderText="Item Name" />
                                                    <asp:BoundField DataField="Total_Requisition" HeaderText="Requisition Qty" />
                                                    <asp:TemplateField HeaderText="Autheriz  Qty">
                                                        <ItemTemplate><asp:TextBox ID="txtQnty" runat="server" AutoPostBack="true" CssClass="tbc" 
                                                                    Font-Size="8pt" onFocus="this.select()" 
                                                                    SkinId="tbPlain" Text='<%#Eval("qnty")%>'  ontextchanged="txtQnty_TextChanged" 
                                                                    ></asp:TextBox></ItemTemplate><ItemStyle HorizontalAlign="Center" Width="20%" />
                                                    </asp:TemplateField><asp:TemplateField HeaderText="Remarks">
                                                                            <ItemTemplate><asp:TextBox ID="txtRemarks" runat="server"  CssClass="tbc" 
                                                                    Font-Size="8pt" onFocus="this.select()" 
                                                                    SkinId="tbPlain" Text='<%#Eval("Remarks")%>' Width="93%" ontextchanged="txtRemarks_TextChanged" 
                                                                    ></asp:TextBox></ItemTemplate><ItemStyle HorizontalAlign="Center" Width="20%" />
                                                                        </asp:TemplateField></Columns><HeaderStyle Font-Bold="True" Font-Size="9pt" ForeColor="White" />
                                                                        <PagerStyle CssClass="pgr" ForeColor="White" HorizontalAlign="Center" /><RowStyle BackColor="White" /></asp:GridView>
                                                                                                            </td></tr></table></fieldset> </td><td style="width: 10%"></td></tr></table></div><asp:Panel ID="pnlVoucher" runat="server"  CssClass="modalPopup" 
                            Style=" display:none; background-color: White; width:800px; height:auto; "><fieldset style=" text-align:left; vertical-align: top; border: solid 1px #8BB381;line-height:1.5em;"><legend style="color: maroon;"><b>View Voucher Information</b></legend><asp:Label runat="server" ID="lblPartuculars"></asp:Label><br/><asp:button id="btnCancel" runat="server" Text="Cancel" Font-Bold="True" OnClientClick="HideModalDiv();" 
                                Font-Size="20pt" /></fieldset> </asp:Panel></ContentTemplate>
   </ajaxToolkit:TabPanel>

   <ajaxToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="History">
                            <HeaderTemplate>History</HeaderTemplate>
                           <ContentTemplate><table style="width:100%"><tr><td style="width:5%"></td><td style="width:90%"><fieldset style="vertical-align: top; border: solid 1px #8BB381;"><legend style="color: maroon;">History</legend><div class="mydiv" style="margin-top:0;"><asp:GridView 
                                   ID="dgHistory" runat="server" AutoGenerateColumns="False" 
                                   BorderColor="LightGray" CssClass="mGrid" Font-Size="9pt" 
                                                     Width="100%" onrowdatabound="dgHistory_RowDataBound" 
                                   onselectedindexchanged="dghostoryChangeItem"  ><Columns><asp:CommandField ShowSelectButton="True" />
                                   <asp:BoundField DataField="id" HeaderText="ID" />
                                   <asp:BoundField DataField="RequisitionNo" HeaderText="Requisition No" />
                                   <asp:BoundField DataField="RequisitionDate" HeaderText="Requisition Date" 
                                           DataFormatString="{0:dd-MM-yyyy}" SortExpression="RequisitionDate" />
                                           <asp:BoundField DataField="ProjectName" HeaderText="Project Name" />
                                           <asp:BoundField DataField="Address" HeaderText="Site Location" />
                                           
                                              <asp:BoundField DataField="Status" HeaderText="Authoriz Status">
                                                   <ControlStyle BackColor="Red" />
                                                   <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                   </asp:BoundField>
                                          
                                           </Columns>
                                           </asp:GridView>
                                           </div>
                                           </fieldset> </td><td style="width:5%"></td></tr></table></ContentTemplate>
                          </ajaxToolkit:TabPanel>
                   </ajaxToolkit:TabContainer>
 
        </div>
        <div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;
        top: 0; left:0; background-color: Black; ; -moz-opacity: 0.8;-webkit-opacity: 0.8;display:none">
    </div>

</asp:Content>


