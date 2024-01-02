<%--<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmployeeRegistrationUI.aspx.cs" Theme="Theme" Inherits="ACCWebApplication.EmployeeRegistrationUI" %>--%>

<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="EmployeeRegistrationUI.aspx.cs"
    Inherits="EmployeeRegistrationUI" Title="Employee Personal Information" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript">
    function SetImage() {
        document.getElementById('<%=lbImgUpload.ClientID %>').click();
    }
    function SetImageStdCur() {
        document.getElementById('<%=lbImgUploadStdCur.ClientID %>').click();
    }
     
</script>

<table  id="pageFooterWrapper">
 <tr>

 <td style="vertical-align:middle;" align="center">
     <asp:Button  ID="BtnDelete" 
            runat="server"  ToolTip="Delete Record"   
            OnClick="DeleteButton_Click" 
            onclientclick="javascript:return window.confirm('are u really want to delete these data')"             
            Text="Delete"
             Width="100px" Height="30px" BorderStyle="Outset" BorderWidth="1px" /></td>

             <td style="vertical-align:middle;" align="center">
        <asp:Button ID="BtnFind" runat="server" ToolTip="Find Form" Text="Find" 
             Width="100px" Height="30px" BorderStyle="Outset" BorderWidth="1px" 
            onclick="BtnFind_Click" /></td>

	<td style="vertical-align:middle; height:100%;" align="center">
        <asp:Button ID="BtnSave" runat="server" ToolTip="Save or Update Record" 
            OnClick="SaveButton_Click" Text="Save"             
             Width="100px" Height="30px" BorderStyle="Outset" BorderWidth="1px"/></td>
	<td style="vertical-align:middle;" align="center">
	<asp:Button  ID="BtnUpdate" runat="server"  ToolTip="Find"  
            OnClick="UpdateButton_Click"
            Text="Update"
             Width="100px" Height="30px" BorderStyle="Outset" BorderWidth="1px" />
	</td>

	<td style="vertical-align:middle;" align="center">
        <asp:Button ID="BtnReset" runat="server" ToolTip="Clear Form" 
            OnClick="RefreshButton_Click"   
            Text="Clear" 
             Width="100px" Height="30px" BorderStyle="Outset" BorderWidth="1px" /></td>
    <td style="vertical-align:middle;" align="center">
        <asp:Button ID="btnPrint" runat="server" ToolTip="Print Employee" 
            OnClick="btnPrint_Click"   
            Text="Print" 
             Width="100px" Height="30px" BorderStyle="Outset" BorderWidth="1px" /></td>    
    <td style="vertical-align:middle;" align="center">
        &nbsp;</td>        
	</tr>		

	</table>
</div>	
    <table style="width: 100%;">
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <fieldset style="width: 99%; height: auto; padding: 0px 0px 0px 0px; margin: 0px 0px 0px 0px;">
                    <legend><b>Employee Basic Information</b> </legend>
                    <table style="width: 100%;">
                        <tr>
                            <td align="right">
                                &nbsp;
                                <asp:Label ID="Label1" runat="server" Text="Employee Id :"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="EmployeeIdTextBox" runat="server" Width="190px"
                                    Height="20px" SkinID="tbGray" CssClass="TextBox"></asp:TextBox>
                            </td>
                            <td align="right">
                                <asp:Label runat="server" Text="N. ID :" ID="Label24"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" CssClass="TextBox" Height="20px" SkinID="tbPlain" Width="190px"
                                    ID="NIDTextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                &nbsp;
                                <asp:Label ID="Label12" runat="server" Text="First Name :"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="FirstNameTextBox" runat="server" Width="190px" Height="20px" SkinID="tbGray"
                                    CssClass="TextBox"></asp:TextBox>
                            </td>
                            <td align="right">
                                <asp:Label ID="Label29" runat="server" Text="Middle Name :" Visible="False"></asp:Label>
                                <asp:Label runat="server" Text="Mobile No :" ID="Label43"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" CssClass="TextBox" Height="20px" SkinID="tbPlain" Width="190px"
                                    ID="MiddleNameTextBox" Visible="False"></asp:TextBox>
                                <asp:TextBox runat="server" CssClass="TextBox" Height="20px" SkinID="tbGray" 
                                    Width="190px" ID="MailMobileNoTextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label30" runat="server" Text="Last Name :"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="LastNameTextBox" runat="server" Width="190px" Height="20px" SkinID="tbGray"
                                    CssClass="TextBox"></asp:TextBox>
                            </td>
                            <td align="right">
                                <asp:Label ID="Label31" runat="server" Text="Category :"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="TeacherCheckBox" runat="server" AutoPostBack="True" OnCheckedChanged="TeacherCheckBox_CheckedChanged"
                                    Text="Teacher" />
                                &nbsp;&nbsp;
                                <asp:CheckBox ID="StaffCheckBox" runat="server" AutoPostBack="True" OnCheckedChanged="StaffCheckBox_CheckedChanged"
                                    Text="Staff" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label7" runat="server" Text="Designation :"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DesignationDrropDownList" runat="server"
                                    Width="198px" CssClass="DDL" Height="30px">
                                </asp:DropDownList>
                            </td>
                            <td align="right">
                                <asp:Label ID="Label6" runat="server" Text="Section :"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DepartmentDrropDownList" runat="server"
                                    Width="198px" CssClass="DDL" Height="30px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label10" runat="server" Text="Joining Date :"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="JoiningDateTextBox" runat="server" Width="190px" Height="20px" SkinID="tbGray"
                                    CssClass="TextBox"></asp:TextBox>
                                    <ajaxtoolkit:calendarextender runat="server" ID="Calendarextender3" TargetControlID="JoiningDateTextBox" Format="dd/MM/yyyy"/>
                                    </td>
                            <td align="right">
                                <asp:Label ID="Label8" runat="server" Text="Sex :"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="GenderDrropDownList" runat="server" Width="198px" CssClass="DDL"
                                    Height="30px">
                                    <asp:ListItem>Select Sex</asp:ListItem>
                                    <asp:ListItem>Male</asp:ListItem>
                                    <asp:ListItem>Female</asp:ListItem>
                                    <asp:ListItem>Others</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label3" runat="server" Text="Date Of Birth :"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="DateOfBirthTextBox" runat="server" Width="190px" Height="20px" SkinID="tbGray"
                                    CssClass="TextBox"></asp:TextBox>
                                    <ajaxtoolkit:calendarextender runat="server" ID="Calendarextender1" TargetControlID="DateOfBirthTextBox" Format="dd/MM/yyyy"/>
                            </td>
                            <td align="right">
                                <asp:Label ID="Label5" runat="server" Text="Religion :"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ReligionDrropDownList" runat="server" Width="198px" CssClass="DDL"
                                    Height="30px">
                                    <asp:ListItem>Select Religion</asp:ListItem>
                                    <asp:ListItem>Islam</asp:ListItem>
                                    <asp:ListItem>Christism</asp:ListItem>
                                    <asp:ListItem>Hinduism</asp:ListItem>
                                    <asp:ListItem>Budhism</asp:ListItem>
                                    <asp:ListItem>Othsers</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label4" runat="server" Text="Blood Group :"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="BloodGroupDrropDownList" runat="server" Width="198px" CssClass="DDL"
                                    Height="30px">
                                    <asp:ListItem>Select Blood Group</asp:ListItem>
                                    <asp:ListItem>A+</asp:ListItem>
                                    <asp:ListItem>B+</asp:ListItem>
                                    <asp:ListItem>A-</asp:ListItem>
                                    <asp:ListItem>B-</asp:ListItem>
                                    <asp:ListItem>AB+</asp:ListItem>
                                    <asp:ListItem>AB-</asp:ListItem>
                                    <asp:ListItem>O+</asp:ListItem>
                                    <asp:ListItem>0-</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td align="right">
                                <asp:Label ID="Label32" runat="server" Text="Marital Status :"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="MaritalStatusDrropDownBox" runat="server" Width="198px" CssClass="DDL"
                                    Height="30px">
                                    <asp:ListItem>Select Marital Staus</asp:ListItem>
                                    <asp:ListItem>Marid</asp:ListItem>
                                    <asp:ListItem>Unmarid</asp:ListItem>
                                    <asp:ListItem>Single</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label33" runat="server" Text="Service Period (in month) :"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="ServicePreiordTextBox" runat="server" Width="190px" Height="20px"
                                    SkinID="tbGray" CssClass="TextBox"></asp:TextBox>
                            </td>
                            <td align="right">
                                <asp:Label ID="Label13" runat="server" Text="Job Status :"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="JobStatusDrropDownList" runat="server" Width="198px" CssClass="DDL"
                                    Height="30px">
                                    <asp:ListItem>Select Job Status</asp:ListItem>
                                    <asp:ListItem>Active</asp:ListItem>
                                    <asp:ListItem>InActive</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label runat="server" Text="TIN Number :" ID="Label23"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" CssClass="TextBox" Height="20px" SkinID="tbPlain" Width="190px"
                                    ID="TINTextBox"></asp:TextBox>
                            </td>
                            <td align="right">
                                <asp:Label runat="server" Text="Passposrt No :" ID="Label25"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" CssClass="TextBox" Height="20px" SkinID="tbPlain" Width="190px"
                                    ID="PassportTextBox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="4" height="40" valign="middle">
                                <fieldset style="height: 40px;">
                                    <table style="height: 38px; width: 100%;">
                                        <tr>
                                            <td style="width: 200px;" align="right">
                                                Have You Any Certification of :
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="BEdChechkBox" runat="server" Text="B.Ed" />
                                                &nbsp;&nbsp;
                                                <asp:CheckBox ID="MEdCheckBox" runat="server" Text="M.Ed" />
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:CheckBox ID="NTRCACheckBox" runat="server" Text="NTRCA" />
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:CheckBox ID="NACheckBox" runat="server" Text="Not Applicable " />
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td style="width: 20%;" valign="top">
                <fieldset style="width: 99; height: auto; padding: 0px 0px 0px 0px; margin: 0px 0px 0px 0px;">
                    <legend><b>Image</b></legend>
                    <table style="width: 100%; height: 100%;">
                          
            <tr>
            <td align="center">
            <asp:FileUpload ID="imgUpload" runat="server" Visible="true" Size="20%" Height="20px" Font-Size="8pt" onchange="javascript:SetImage();"/>
            <asp:Button ID="lbImgUpload" runat="server" Text="Upload" Font-Size="8pt"  style="display:none;"
                    Width="50px" Height="20px" onclick="lbImgUpload_Click"></asp:Button>
            </td>
            </tr>
            <tr>
            <td style=" height: 162px; vertical-align:top;" align="center">
                <asp:Image ID="Image1" runat="server" Height="165px" Width="145px" BorderStyle="Solid" BackColor="#EFF3FB" BorderWidth="1px"  />  
            </td>	
            </tr>
           
                        <tr>
                            <td align="center">
                                <asp:Image ID="Image2" runat="server" Width="40px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--<asp:FileUpload ID="FileUpload2" runat="server" />--%>

                                 <asp:FileUpload ID="imgUploadStdCur" runat="server" Visible="true" Size="20%" Height="20px" Font-Size="8pt" onchange="javascript:SetImageStdCur();"/>
            <asp:Button ID="lbImgUploadStdCur" runat="server" Text="Upload" Font-Size="8pt" style="display:none;"
                    Width="50px" Height="20px" onclick="lbImgUploadStdCur_Click"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="ImagePreviewButton" runat="server" Text="Upload Img" OnClick="ImagePreviewButton_Click"
                                    Width="160px" Visible="False" />
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
        <tr>
            <td colspan="5">
                <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" 
                    Width="100%">
                    <asp:TabPanel runat="server" HeaderText="Education Information" ID="TabPanel1">
                        <HeaderTemplate>
                            Education Information</HeaderTemplate>
                        <ContentTemplate>
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:Panel ID="EducationPanel" runat="server" Height="170px" ScrollBars="Vertical">
                                            <asp:GridView ID="EducationGridView" runat="server" CssClass="mGrid" AutoGenerateColumns="False" style="text-align:center"
                                                AllowPaging="True" Width="100%" BackColor="White" BorderWidth="1px" BorderStyle="Solid"
                                                CellPadding="2" BorderColor="LightGray" Font-Size="8pt" AllowSorting="True" PageSize="15"
                                                ForeColor="#333333" OnRowDataBound="EducationGridView_RowDataBound" OnRowDeleting="EducationGridView_RowDeleting"
                                                OnSelectedIndexChanged="EducationGridView_SelectedIndexChanged">
                                                <AlternatingRowStyle CssClass="alt" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Exam Title">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ExamTitleComboBox" OnSelectedIndexChanged="ExamTitleComboBox_SelectedIndexChanged"
                                                                runat="server"  AutoPostBack="true" Style="width: 98%; height: 30px;" 
                                                                Width="95%">
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Institute">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="InstituteTextBox" SkinID="tbPlain" runat="server" 
                                                                Text='<%# Eval("Institute") %>' Style="width:90%"></asp:TextBox></ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="GPA">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="GPATextBox" SkinID="tbPlain" runat="server" 
                                                                Text='<%# Eval("GPA") %>' Style="width:90%"></asp:TextBox></ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Grade">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="GradeTextBox" SkinID="tbPlain" runat="server" 
                                                                Text='<%# Eval("Grade") %>' Style="width:90%"></asp:TextBox></ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Group">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="GroupTextBox" SkinID="tbPlain" runat="server" 
                                                                Text='<%# Eval("Group") %>' Style="width:90%"></asp:TextBox></ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Board/University">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="BoardUniversityTextBox" SkinID="tbPlain" 
                                                                Text='<%# Eval("Board/University") %>' runat="server" Style="width:90%"></asp:TextBox></ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Passing Year">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="PassingYearTextBox" SkinID="tbPlain" 
                                                                Text='<%# Eval("Passing Year") %>' runat="server" Style="width:90%"></asp:TextBox></ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" />
                                                </Columns>
                                                <PagerStyle CssClass="pgr" />
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="TabPanel8" runat="server" HeaderText="Extra Curriculumns/Dgree/Qualifications/Training">
                        <ContentTemplate>
                            <table width="100%" style="height: 200px;">
                                <tr>
                                    <td style="width: 20px; text-align: center;">
                                        01
                                    </td>
                                    <td>
                                        <asp:TextBox ID="ExtraCurriculamTextBox1" runat="server" Height="60px" TextMode="MultiLine"
                                            Width="500px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20px; text-align: center;">
                                        02
                                    </td>
                                    <td>
                                        <asp:TextBox ID="ExtraCurriculamTextBox2" runat="server" Height="60px" TextMode="MultiLine"
                                            Width="500px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20px; text-align: center;">
                                        03
                                    </td>
                                    <td>
                                        <asp:TextBox ID="ExtraCurriculamTextBox3" runat="server" Height="60px" TextMode="MultiLine"
                                            Width="500px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="TabPanel4" runat="server" HeaderText="Job Experience">
                        <ContentTemplate>
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <asp:GridView ID="ExperienceGridview" runat="server" AutoGenerateColumns="False" style="text-align:center"
                                            Width="100%" OnRowDataBound="ExperienceGridview_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="Serial" HeaderText="Serial" SortExpression="Serial" />
                                                <asp:TemplateField HeaderText="Organization Name">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="OrganizzationNameTextBox" Text='<%# Eval("OrganizationName") %>'
                                                            CssClass="TextBox" SkinID="tbPlain" Style="width: 90%;" runat="server"></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Position">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="PositionTextBox" CssClass="TextBox" SkinID="tbPlain" Style="width: 90%;"
                                                            runat="server" Text='<%# Eval("Position") %>'></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Duration">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="DurationTextBox" CssClass="TextBox" SkinID="tbPlain" Style="width: 90%;"
                                                            runat="server" Text='<%# Eval("Duration") %>'></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reason For Leave">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="ReasonForLEaveTextBox" CssClass="TextBox" SkinID="tbPlain" Style="width: 90%;"
                                                            runat="server" Text='<%# Eval("ReasonForLeave") %>'></asp:TextBox></ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="TabPanel5" runat="server" HeaderText="Contact Information">
                        <ContentTemplate>
                            <table style="width: 100%; height: 220px;">
                                <tr>
                                    <td>
                                        <fieldset style="height: 90px;">
                                            <legend>Permanent Address :</legend>
                                            <table style="width: 100%; height: 90px;">
                                                <tr>
                                                    <td align="right" colspan="2">
                                                        <asp:Label ID="Label34" runat="server" Text="House No / Road No / Area :"></asp:Label>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="PerHouseNoTextBox" runat="server" CssClass="TextBox" Height="20px"
                                                            SkinID="tbGray" Width="290px"></asp:TextBox>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="Label35" runat="server" Text="Thana / Upazila :"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PerThanaTextBox" runat="server" CssClass="TextBox" Height="20px"
                                                            SkinID="tbGray" Width="190px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="Label36" runat="server" Text="District :"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="PerDistrictTextBox" runat="server" CssClass="TextBox" Height="20px"
                                                            SkinID="tbGray" Width="190px"></asp:TextBox>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="Label37" runat="server" Text="Zip/Post Code :"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PerZipCodeTextBox" runat="server" CssClass="TextBox" Height="20px"
                                                            SkinID="tbGray" Width="190px"></asp:TextBox>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="Label38" runat="server" Text="Contact No :"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="perContactNoTextBox" runat="server" CssClass="TextBox" Height="20px"
                                                            SkinID="tbGray" Width="190px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top">
                                        <fieldset style="height: 130px;">
                                            <legend>Present/Mailing Address</legend>
                                            <table style="width: 100%; height: 130px;">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="Label39" runat="server" Text="House No / Road No / Area :"></asp:Label>
                                                    </td>
                                                    <td align="left" colspan="2">
                                                        <asp:TextBox ID="MailHouseNoTextBox" runat="server" CssClass="TextBox" Height="20px"
                                                            SkinID="tbGray" Width="290px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label40" runat="server" Text="Thana / Upazila :"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="MailThanaTextBox" runat="server" CssClass="TextBox" Height="20px"
                                                            SkinID="tbGray" Width="190px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label41" runat="server" Text="District :"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="MailDistrictTextBox" runat="server" CssClass="TextBox" Height="20px"
                                                            SkinID="tbGray" Width="190px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label42" runat="server" Text="Zip/Post Code :"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="MailZipCodeTextBox" runat="server" CssClass="TextBox" Height="20px"
                                                            SkinID="tbGray" Width="190px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        &nbsp;</td>
                                                    <td align="left">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label44" runat="server" Text="Email (If Any) :"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="MailEmailTextBox" runat="server" CssClass="TextBox" Height="20px"
                                                            SkinID="tbGray" Width="190px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td align="left">
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td align="left">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="TabPanel6" runat="server" HeaderText="Others">
                        <HeaderTemplate>
                            Family Info</HeaderTemplate>
                        <ContentTemplate>
                            <table style="width: 100%; height: 250px;">
                                <tr>
                                    <td style="height: 70px; text-align: left;">
                                        <fieldset style="height: 70px;">
                                            <legend>Dependants Information</legend>
                                            <table style="width: 100%; height: 70px;">
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="Label2" runat="server" Text="Father's Name :"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="FathersNameTextBox" runat="server" CssClass="TextBox" Height="20px"
                                                            SkinID="tbGray" Width="190px"></asp:TextBox>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="Label11" runat="server" Text="Mother's Name :"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="MothersNameTextBox" runat="server" CssClass="TextBox" Height="20px"
                                                            SkinID="tbGray" Width="190px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" style="height: 23px">
                                                        <asp:Label ID="Label45" runat="server" Text="Spouse Name :"></asp:Label>
                                                    </td>
                                                    <td align="left" style="height: 23px">
                                                        <asp:TextBox ID="SpouseNameTextBox" runat="server" CssClass="TextBox" Height="20px"
                                                            SkinID="tbGray" Width="190px"></asp:TextBox>
                                                    </td>
                                                    <td align="right" style="height: 23px">
                                                        <asp:Label ID="Label46" runat="server" Text="Occupation :"></asp:Label>
                                                    </td>
                                                    <td align="left" style="height: 23px">
                                                        <asp:TextBox ID="OccupationTextBox" runat="server" CssClass="TextBox" Height="20px"
                                                            SkinID="tbGray" Width="190px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" style="height: 27px">
                                                    </td>
                                                    <td align="left" style="height: 27px">
                                                    </td>
                                                    <td align="right" style="height: 27px">
                                                    </td>
                                                    <td align="left" style="height: 27px">
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 180px; text-align: left;">
                                        <fieldset style="height: 180px;">
                                            <legend>Children Information</legend>
                                            <table style="width: 100%; height: 180px;">
                                                <tr>
                                                    <td style="width: 20px;" align="center" valign="middle">
                                                        01
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:TextBox ID="ChildInfo1TextBox" runat="server" CssClass="TextBox" Height="45px"
                                                            SkinID="tbGray" TextMode="MultiLine" Width="500px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" valign="middle">
                                                        02
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:TextBox ID="ChildInfo2TextBox" runat="server" CssClass="TextBox" Height="45px"
                                                            SkinID="tbGray" TextMode="MultiLine" Width="500px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" valign="middle">
                                                        03
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:TextBox ID="ChildInfo3TextBox" runat="server" CssClass="TextBox" Height="45px"
                                                            SkinID="tbGray" TextMode="MultiLine" Width="500px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel runat="server" HeaderText="Account Information" ID="TabPanel2">
                        <ContentTemplate>
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        &nbsp;&nbsp;<asp:Label ID="Label20" runat="server" Text="Bank Name :"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="BankNameDrropDownList" runat="server" AutoPostBack="True" Width="198px"
                                            CssClass="DDL" Height="30px">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right">
                                        &nbsp;&nbsp;<asp:Label ID="Label22" runat="server" Text="Branch Name :"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="BranchNameTextBox" runat="server" Width="198px" Height="20px" SkinID="tbPlain"
                                            CssClass="TextBox"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        &nbsp;&nbsp;<asp:Label ID="Label21" runat="server" Text="Account No :"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="AccountNoTextBox" runat="server" Width="190px" Height="20px" SkinID="tbPlain"
                                            CssClass="TextBox"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        &nbsp;Accont Type :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="AccountTypetextBox" runat="server" Height="20px" Width="198px" SkinID="tbPlain"
                                            CssClass="TextBox"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="Refrence">
                        <ContentTemplate>
                            <asp:GridView ID="RefrenceGridview" runat="server" AutoGenerateColumns="False" 
                                Height="120px" style="text-align:center"
                                OnRowDataBound="RefrenceGridview_RowDataBound" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="Serial" HeaderText="Serial" SortExpression="Serial" />
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:TextBox ID="RfNameTextBox" runat="server" Width="90%" Text='<%# Eval("Name") %>'></asp:TextBox></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Designation">
                                        <ItemTemplate>
                                            <asp:TextBox ID="RfDescriptionTextBox" runat="server" Width="90%" Text='<%# Eval("Designation") %>'></asp:TextBox></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Organization">
                                        <ItemTemplate>
                                            <asp:TextBox ID="RfOrganizationTextBox" runat="server" Width="90%" Text='<%# Eval("Organization") %>'></asp:TextBox></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Contact No">
                                        <ItemTemplate>
                                            <asp:TextBox ID="RfContactNoTextBox" runat="server" Width="90%" Text='<%# Eval("Contact") %>'></asp:TextBox></ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="TabPanel7" runat="server" HeaderText="History">
                        <ContentTemplate>
                        <table style="width:100%;">
                            <tr>
                                 <td style="height: 2px">
                                 </td>                                 
                            </tr>
                       
                            
                            <tr>
                                 <td>
                                    <asp:GridView ID="TeacherInformationDetails" runat="server" CssClass="mGrid" AutoGenerateColumns="False"
                                AllowPaging="True" Width="100%" BackColor="White" BorderWidth="1px" BorderStyle="Solid"
                                CellPadding="2" BorderColor="LightGray" Font-Size="8pt" 
                                AllowSorting="True" PageSize="50"
                                ForeColor="#333333" 
                                OnSelectedIndexChanged="TeacherInformationDetails_SelectedIndexChanged" 
                                onpageindexchanging="TeacherInformationDetails_PageIndexChanging" 
                                onrowdatabound="TeacherInformationDetails_RowDataBound">
                                <Columns>
                                    <asp:CommandField HeaderText="Select" ShowSelectButton="True" />
                                    <asp:BoundField DataField="emp_employee_id" HeaderText="Id" SortExpression="emp_employee_id" />
                                    <asp:BoundField DataField="emp_name" HeaderText="Name" SortExpression="emp_name" />
                                    <asp:BoundField DataField="emp_fathers_name" HeaderText="Father Name" SortExpression="emp_fathers_name" />
                                    <asp:BoundField DataField="emp_mail_mobile" HeaderText="Phone Number" 
                                        SortExpression="emp_mail_mobile" />
                                    <asp:BoundField DataField="emp_dateof_birth" HeaderText="Date Of Birth" SortExpression="emp_dateof_birth" />
                                    <asp:BoundField DataField="emp_joining_date" HeaderText="Joinig Date" SortExpression="emp_joining_date" />
                                    <asp:BoundField DataField="emp_blood" HeaderText="Blood" SortExpression="emp_blood" />
                                    <asp:BoundField DataField="desig_name" HeaderText="Designation" SortExpression="desig_name" />
                                    <asp:BoundField DataField="emp_job_status" HeaderText="Job Status" SortExpression="emp_job_status" />
                                </Columns>
                            </asp:GridView>
                                 </td>                                 
                            </tr>
                       
                            
                         </table>
                        </ContentTemplate>
                    </asp:TabPanel>
                </asp:TabContainer>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" colspan="5">
                &nbsp;&nbsp;
                &nbsp;&nbsp;
                &nbsp;&nbsp;
                </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
