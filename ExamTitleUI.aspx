<%--<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExamTitleUI.aspx.cs" Inherits="ACCWebApplication.ExamTitleUI" %>--%>
 <%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ExamTitleUI.aspx.cs" Inherits="ExamTitleUI" Title="Untitled Page" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<table style="width:100%; height:700px;">
        <tr>
            <td  colspan="2" style="height:40px; text-align:center;">
                <h2>
                    Exam Title<strong> Information</strong></h2>
            </td>
        </tr>
        <tr>
            <td align="right" style="height:40px; width:50%;"  class="style1">
                Exam Title Id :</td>
            <td height="20">
                <asp:TextBox ID="ExamTitleIdTextBox" runat="server" Enabled="False" 
                    Height="30px" Width="220px" SkinID="tbPlain" CssClass="TextBox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" style="height:40px;" class="style1">
                Exam Title Name :</td>
            <td>
                <asp:TextBox ID="ExamTitleNameTextBox" runat="server" Height="30px" 
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
                <asp:Button ID="SaveButton" runat="server" Text="Save" Width="100px" 
                    onclick="DeptSaveButton_Click" CssClass="buttonclass" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="RefreshButton" runat="server" Text="Refresh" Width="100px" 
                    onclick="CloseButton_Click" CssClass="buttonclass" />
            </td>
        </tr>
        <tr>
            <td style="height:400px;" colspan="2" valign="top" align="center">
                <asp:GridView ID="ExamTitleGridView" runat="server" 
                    AutoGenerateColumns="False" Width="80%" AllowPaging="True" PageSize="23" 
                    onselectedindexchanged="ExamTitleGridView_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                        <asp:CommandField HeaderText="Edit" SelectText="Edit" ShowSelectButton="True" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
