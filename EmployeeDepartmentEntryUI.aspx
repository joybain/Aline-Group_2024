<%--<%@ Page Title="Employee Department" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Theme="Theme" CodeBehind="EmployeeDepartmentEntryUI.aspx.cs" Inherits="ACCWebApplication.EmployeeDepartmentEntryUI" %>--%>
<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="EmployeeDepartmentEntryUI.aspx.cs" Inherits="EmployeeDepartmentEntryUI" Title="Untitled Page" %>
 
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width:100%; height:700px;">
        <tr>
            <td  colspan="2" style="height:40px; text-align:center;">
                <h2>
                    <strong>Section Information</strong></h2>
            </td>
        </tr>
        <tr>
            <td align="right" style="height:40px;" height="40" class="style1">
                Section Id :</td>
            <td height="20">
                <asp:TextBox ID="DepartmentIdTextBox" runat="server" Enabled="False" 
                    Height="30px" Width="220px" SkinID="tbPlain" CssClass="TextBox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" style="height:40px;" class="style1">
                Section Name :</td>
            <td>
                <asp:TextBox ID="DepartmentNameTextBox" runat="server" Height="30px" 
                    Width="220px" SkinID="tbPlain" CssClass="TextBox"></asp:TextBox>
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
                <asp:Button ID="DeptSaveButton" runat="server" Text="Save" Width="100px" 
                    onclick="DeptSaveButton_Click" CssClass="buttonclass" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="CloseButton" runat="server" Text="Refresh" Width="100px" 
                    onclick="CloseButton_Click" CssClass="buttonclass" />
            </td>
        </tr>
        <tr>
            <td style="height:400px;" colspan="2" valign="top" align="center">
                <asp:GridView ID="DepartmentGridview" runat="server" 
                    AutoGenerateColumns="False" Width="80%" AllowPaging="True" PageSize="23" 
                    onselectedindexchanged="DepartmentGridview_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="dept_id" HeaderText="Department Id" 
                            SortExpression="dept_id" />
                        <asp:BoundField DataField="dept_name" HeaderText="Department Name" 
                            SortExpression="dept_name" />
                        <asp:CommandField HeaderText="Edit" NewText="Edit" SelectText="Edit" 
                            ShowSelectButton="True" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
