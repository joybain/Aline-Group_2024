 
 <%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="EmployeeDesignationEntryUI.aspx.cs" Inherits="EmployeeDesignationEntryUI" Title="Untitled Page" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<table style="width:100%; height:700px;">
        <tr>
            <td  colspan="2" style="height:40px; text-align:center;">
                <h2>
                    <strong>Designation&nbsp; Information</strong></h2>
            </td>
        </tr>
        <tr>
            <td align="right" style="height:40px;" height="40" class="style1">
                <strong>Designation Id :</strong></td>
            <td height="20">
                <asp:TextBox ID="DesignationIdTextBox" runat="server" Enabled="False" 
                    Height="30px" Width="220px" CssClass="TextBox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" style="height:40px;" class="style1">
                <strong>Designation Name :</strong></td>
            <td>
                <asp:TextBox ID="DesignationNameTextBox" runat="server" Height="30px" 
                    Width="220px" CssClass="TextBox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="center" style="height:40px;" class="style1" colspan="2">
                <asp:Label ID="ConfiramationLabel" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height:40px;" colspan="2" align="center">
               <asp:Button ID="DeleteButton" runat="server" Text="Delete" Width="100px" 
                    CssClass="buttonclass" onclick="DeleteButton_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button 
                    ID="UpdateButton" runat="server" Text="Update" Width="100px" 
                    CssClass="buttonclass" onclick="UpdateButton_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                <asp:Button ID="DesignationSaveButton" runat="server" Text="Save" Width="100px" 
                    onclick="DesignationSaveButton_Click" CssClass="buttonclass" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="CloseButton" runat="server" Text="Refresh" Width="100px" 
                    onclick="CloseButton_Click" CssClass="buttonclass" />
            </td>
        </tr>
        <tr>
            <td style="height:400px;" colspan="2" valign="top" align="center">
                <asp:GridView ID="DesignationGridview" runat="server" 
                    AutoGenerateColumns="False" Width="80%" AllowPaging="True" PageSize="23" 
                    onselectedindexchanged="DesignationGridview_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="desig_id" HeaderText="Designtion Id" 
                            SortExpression="desig_id" />
                        <asp:BoundField DataField="desig_name" HeaderText="Designation Name" 
                            SortExpression="desig_name" />
                        <asp:CommandField HeaderText="Edit" NewText="Edit" SelectText="Edit" 
                            ShowSelectButton="True" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
