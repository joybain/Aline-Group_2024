<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="frmProjectSetup.aspx.cs" Inherits="frmProjectSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
<table  id="pageFooterWrapper">
 <tr>
	<td align="center">
    
	    <asp:Button  ID="BtnDelete" runat="server"  ToolTip="Delete Record"   
            
            onclientclick="javascript:return window.confirm('are u really want to delete  these data')" Text="Delete" 
        Height="35px" Width="100px" BorderStyle="Outset" onclick="BtnDelete_Click"  />
 </td>	
    <td align="center">
        &nbsp;</td>
	
	<td align="center">
        <asp:Button ID="BtnSave" runat="server" OnClientClick="this.disabled=true;" 
            UseSubmitBehavior="false" ToolTip="Save or Update Record" 
             Text="Save"  
        Height="35px" Width="100px" BorderStyle="Outset" onclick="BtnSave_Click"  /></td>
	<td align="center">
        <asp:Button ID="BtnReset" runat="server" ToolTip="Clear Form" 
             Text="Clear" 
        Height="35px" Width="100px" BorderStyle="Outset" onclick="BtnReset_Click"  /></td>    
	</tr>		
</table>
    <table style="width: 100%">
        <tr>
            <td style="width: 20%">
            </td>
            <td style="width: 60%">
            <fieldset style="vertical-align: top; border: solid 1px #8BB381;">
                <legend style="color: maroon; font-weight: 700;">Project Setup<b> </b></legend>
            
                <table style="width: 100%">
                    <tr>
                        <td style="width: 40%; text-align: center;">
                            <b>Land Owner Name</b></td>
                        <td style="width: 5%; text-align: center;">
                            :</td>
                        <td style="width: 55%">
                            <asp:DropDownList ID="ddlClient" runat="server" Width="100%">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 40%; text-align: center;">
                            <b>Project Name</b></td>
                        <td style="width: 5%" class="style1">
                            :</td>
                        <td style="width: 55%">
                            <asp:TextBox ID="txtProjectName" runat="server" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 40%; text-align: center;">
                            <b>Site/Address</b></td>
                        <td style="width: 5%" class="style1">
                            :</td>
                        <td style="width: 55%">
                            <asp:TextBox ID="txtAddress" runat="server" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                   
                    <tr>
                        <td style="width: 40%; text-align: center;">
                            <asp:HiddenField ID="hidenId" runat="server" />
                          
                        </td>
                        <td style="width: 5%" class="style1">
                            &nbsp;</td>
                        <td style="width: 55%">
                            &nbsp;</td>
                    </tr>
                   
                </table>
                 </fieldset>
              <fieldset style="vertical-align: top; border: solid 1px #8BB381;">
                  <legend style="color: maroon; font-weight: 700;">Total Information<b 
                          style="text-align: center"> </b></legend>
            
                <table style="width: 100%">
                    <tr>
                        <td style="width: 100%; text-align: center; font-size: large;"> 
                          
                            <asp:Label ID="lblTotalHistory" runat="server"></asp:Label>
                          
                        </td>
                        <td style="width: 100%; text-align: center;"> 
                          
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 100%" colspan="2">   
                            <asp:GridView ID="dgProjectHistory" runat="server" AutoGenerateColumns="False" 
                                CssClass="mGrid" Width="100%" 
                                
                                onselectedindexchanged="dgProjectHistory_SelectedIndexChanged" 
                                onrowdatabound="dgProjectHistory_RowDataBound">
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" />
                                    <asp:BoundField DataField="Id" HeaderText="Id" />                                   
                                    <asp:BoundField DataField="ClientID" HeaderText="Client Name" />
                                    <asp:BoundField DataField="ProjectName" HeaderText="Project Name" />
                                    <asp:BoundField DataField="Address" HeaderText="Site/Address" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>

                </table>
                 </fieldset>    
            </td>
            <td style="width: 20%"></td>
            
        </tr>
    </table>
</asp:Content>
