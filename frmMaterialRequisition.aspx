<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmMaterialRequisition.aspx.cs" Inherits="frmMaterialRequisition" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
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
    function LoadModalDiv() {

        var bcgDiv = document.getElementById("divBackground");
        bcgDiv.style.display = "block";

    }
    function HideModalDiv() {

        var bcgDiv = document.getElementById("divBackground");
        bcgDiv.style.display = "none";

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
        Height="30px" Width="110px" BorderStyle="Outset" Enabled="False" 
                Visible="False" onclick="btnSave_Click"  />
        
        </td>
               <td align="center" >
        
            <asp:Button ID="lbAuth" runat="server" BorderStyle="Outset" Height="25px" 
                                                             OnClientClick="LoadModalDiv();" Text="Approve" 
                                                            ToolTip="Authorize" Width="100px" />
                                                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderLogin" runat="server" 
                                                            BackgroundCssClass="modalBackground" DropShadow="True" DynamicServicePath="" 
                                                            Enabled="True" PopupControlID="LoginPanel" TargetControlID="lbAuth">
                                                        </ajaxToolkit:ModalPopupExtender>
        
        </td>
        <td align="center" >
        <asp:Button ID="btnNew" runat="server" ToolTip="New" Text="New" 
        Height="30px" Width="110px" BorderStyle="Outset" onclick="btnNew_Click"  /> 
        </td>
        <td align="center" >
        <asp:Button ID="btnClear" runat="server"  ToolTip="Clear"  Text="Clear" 
        Height="30px" Width="110px" BorderStyle="Outset" onclick="btnClear_Click"  />
        </td>
        <td align="center" >
        <asp:Button ID="btnPrint" runat="server" ToolTip="Print PO" Text="Print" 
        Height="30px" Width="110px" BorderStyle="Outset" onclick="btnPrint_Click"  />
        </td>            
        
   </tr>
   </table>
    <table style="width: 100%">
        <tr>
            <td style="width: 3%"></td>
            <td style="width: 94%">
                <fieldset style="vertical-align: top; border: solid 1px #8BB381;">
                    <legend style="color: maroon; font-weight: 700;"><b>Material Requisition From</b></legend>

                    <table style="width: 90%">
                        <tr align="center"> 
                            
                            <td style="width: 25%; font-weight: 700; height: 70px; text-align: right;">Material Requisition No </td>
                            <td style="width: 5%; text-align: center; height: 70px;">:</td>
                            <td style="width: 25%; height: 70px;">
                                <asp:TextBox ID="txtRequisitionNo" runat="server" Width="100%" Enabled="False" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td style="width: 20%; text-align: right; font-weight: 700; height: 70px;">Date</td>
                            <td style="width: 5%; text-align: center; height: 70px;">:</td>
                            <td style="width: 20%; height: 70px;">
                                <asp:TextBox SkinID="tbPlain" ID="txtPresentDate" runat="server" 
                                    Enabled="False" Width="98%" Style="text-align: center;"
                                    CssClass="tbc" AutoPostBack="False" Font-Size="8pt"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender runat="server" ID="Calendarextender1"
                                    TargetControlID="txtPresentDate" Format="dd/MM/yyyy" />
                                &nbsp;</td>
                        </tr>

                       <tr align="center">
                            <td style="width: 25%; font-weight: 700; height: 17px; text-align: right;">Project</td>
                            <td style="width: 5%; text-align: center; height: 17px;">:</td>
                            <td style="width: 25%; height: 17px;">
                                <asp:DropDownList ID="ddlProject" runat="server" Width="100%" Enabled="False" 
                                    AutoPostBack="True" onselectedindexchanged="ddlProject_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 20%; text-align: right; font-weight: 700; height: 17px;">Address</td>
                            <td style="width: 5%; text-align: center; height: 17px;">:</td>
                            <td style="width: 20%; height: 17px;">
                                <asp:TextBox ID="txtSite" runat="server" Width="98%" Enabled="False" 
                                    ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>

                       <tr align="center">
                            <td style="width: 25%; font-weight: 700; text-align: right;">Recomments</td>
                            <td style="width: 5%; text-align: center;">:</td>
                            <td colspan="4">
                                <asp:TextBox ID="txtRecomment" runat="server" Width="98%" Height="41px" 
                                    TextMode="MultiLine" Enabled="False" ></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 25%; font-weight: 700;">&nbsp;</td>
                            <td style="width: 5%; text-align: center;">&nbsp;</td>
                            <td colspan="4">
                                <asp:HiddenField ID="hidnId" runat="server" />
                            </td>
                        </tr>

                    </table>
                </fieldset>
                 <fieldset style="vertical-align: top; border: solid 1px #8BB381;">
                 <table style="width: 100%">
                 <tr>
                 <td style="width: 100%; margin-left: 40px;">
                     <asp:GridView ID="dgRequistionHistory" runat="server" 
                         AutoGenerateColumns="False" CssClass="mGrid" Width="100%" 
                         onrowdatabound="dgRequistionHistory_RowDataBound" 
                         onselectedindexchanged="dgRequistionHistory_SelectedIndexChanged">                       
                         <Columns>
                             <asp:CommandField ShowSelectButton="True" />
                              <asp:TemplateField ShowHeader="False">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" 
                                                                    Checked='<%# Eval("check").ToString().Equals("1") %>' oncheckedchanged="chkSelect_CheckedChanged1" 
                                                                   />
                                                             
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                             <asp:BoundField DataField="Id" HeaderText="Id" />
                         
                             <asp:BoundField DataField="RequisitionNo" HeaderText="Requisition No" />
                             <asp:BoundField DataField="RequisitionDate" HeaderText="Requisition Date" />
                             <asp:BoundField DataField="ProjectName" HeaderText="Project Name" />
                             <asp:BoundField DataField="Address" HeaderText="Site/Address" />
                             <asp:BoundField DataField="ClientName" HeaderText="Client Name" />
                             <asp:BoundField DataField="Approve" HeaderText="Approve By"/> 
                             <asp:BoundField DataField="ApproveStatus" HeaderText="ApproveStatus"/>                       
                             <asp:BoundField DataField="Authorize" HeaderText="Authorize">
                                                <HeaderStyle ForeColor="Black" />
                                                <ItemStyle Font-Bold="True" Font-Size="Small" HorizontalAlign="Center" 
                                                    Width="10%" />
                                                </asp:BoundField>
                       
                         </Columns>
                     </asp:GridView>
                 </td>
                 </tr>
                 </fieldset>
               <%--  <tr>
                 <td style="width:100%">
                 
                     <rsweb:ReportViewer ID="RequisitionReport" runat="server" Height="200px" 
                         Width="100%">
                     </rsweb:ReportViewer>
                 
                 </td></tr>--%>
                 </table>
                <div style="font-size: 8pt;" align="center">
                <asp:UpdatePanel ID="MRIesms_UP" runat="server"  UpdateMode="Conditional">
                <ContentTemplate>
                <table style="width:100%">
                    <tr>
                        <td style="width:100%">
                         <asp:GridView ID="dgMaterial" runat="server" AutoGenerateColumns="False" 
                                CssClass="mGrid" onrowdatabound="dgMaterial_RowDataBound" 
                                onrowdeleting="dgMaterial_RowDeleting" >
                                <AlternatingRowStyle CssClass="alt" />
                             <Columns>
                                   <asp:TemplateField>
                                     <ItemTemplate>
                                        <asp:ImageButton ID="lbDelete" runat="server" CausesValidation="False" 
                                 CommandName="Delete" ImageAlign="Middle" ImageUrl="~/img/delete.png" 
                                 Text="Delete" />
                                     </ItemTemplate>
                                 </asp:TemplateField>
                                  <asp:TemplateField>
                                                <ItemTemplate>
                                                
                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" 
                                                        Checked='<%# Eval("check").ToString().Equals("1") %>' oncheckedchanged="chkSelect_CheckedChanged" 
                                                         />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                                            </asp:TemplateField>
                               
                                 <asp:TemplateField HeaderText="Code">
                                     <ItemTemplate>
                                         <asp:TextBox ID="txtItemCode" runat="server" autocomplete="off" 
          AutoPostBack="true" Font-Size="8pt"  
          SkinId="tbPlain" Text='<%#Eval("item_Code")%>' Width="92%" onFocus="this.select()" Enabled="False" 
                                             ReadOnly="True"></asp:TextBox>       
                                     </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Desctiption of Items">
                                     <ItemTemplate>
                                         <asp:TextBox ID="txtDesctiption" runat="server" autocomplete="off" 
          AutoPostBack="true" Font-Size="8pt"  
          SkinId="tbPlain" Text='<%#Eval("item_desc")%>' Width="92%" onFocus="this.select()" 
                                             ontextchanged="txtDesctiption_TextChanged"></asp:TextBox>
          <ajaxToolkit:AutoCompleteExtender ServicePath="AutoComplete.asmx" runat="server" 
                                     ID="autoComplete1" TargetControlID="txtDesctiption"
           ServiceMethod="GetItemListSerch" MinimumPrefixLength="1" CompletionInterval="10" EnableCaching="true" 
                                     CompletionSetCount="12"/>
                                     </ItemTemplate>
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
                                 <asp:TemplateField HeaderText="Previous">
                                     <ItemTemplate>
                                    
           <asp:Label ID="lblPreviousRequisition" runat="server" 
                                     Font-Size="8pt" Width="95%" Text='<%#Eval("Previous")%>'> </asp:Label>
                                                      
                                    </ItemTemplate>
                                     <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="This Time">
                                     <ItemTemplate>
                                      
                                        <asp:TextBox ID="txtThisTimeRequisition" runat="server" autocomplete="off" 
          AutoPostBack="true" Font-Size="8pt"  onkeypress="return isNumber(event)"
          SkinId="tbPlain" Text='<%#Eval("This_time_Requisition")%>' Width="92%" onFocus="this.select()" 
                                             ontextchanged="txtThisTimeRequisition_TextChanged"></asp:TextBox>
                                     </ItemTemplate>
                                     <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Total">
                                 <ItemTemplate>
                                     <asp:Label ID="lblTotalRequisition" runat="server" 
                                     Font-Size="8pt" Width="95%"> </asp:Label>                                             
                                    </ItemTemplate>
                                     <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Supplid Qty.">
                                  <ItemTemplate>
                                     <asp:Label ID="lblSupplidRequisition" runat="server" 
                                     Font-Size="8pt" Width="95%" Text='<%#Eval("SupplidQnt")%>'> </asp:Label>                                             
                                    </ItemTemplate>
                                     <HeaderStyle HorizontalAlign="Left" />
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Present Stock">
                                 <ItemTemplate>
                                     <asp:Label ID="lblPresentStock" runat="server" 
                                     Font-Size="8pt" Width="95%" Text='<%#Eval("PresentStock")%>'> </asp:Label>                                             
                                    </ItemTemplate>
                                     <HeaderStyle HorizontalAlign="Left" />
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="P.S.Requirement">
                                 <ItemTemplate>
                                     <asp:Label ID="lblPSRequirement" runat="server" 
                                     Font-Size="8pt" Width="95%" > </asp:Label>                                             
                                    </ItemTemplate>
                                     <HeaderStyle HorizontalAlign="Left" />
                                 </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Remarks Any">
                                  <ItemTemplate>
                                   
                                       <asp:TextBox ID="txtGrvRemarks" runat="server" 
                                     AutoPostBack="True" CssClass="tbr" Font-Size="8pt" 
           SkinId="tbPlain" Text='<%#Eval("Remarks")%>' Width="93%" onFocus="this.select()" 
                                           ontextchanged="txtGrvRemarks_TextChanged" ></asp:TextBox>  
                                                                                 
                                    </ItemTemplate></asp:TemplateField>
                                     <asp:TemplateField HeaderText="ID">
                                  <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" 
                                     Font-Size="8pt" Width="95%" Text='<%#Eval("ID")%>'></asp:Label>                                            
                                    </ItemTemplate></asp:TemplateField>
                             </Columns>
                    </asp:GridView></td>                      
                    </tr>
                      <tr>
                 <td style="width:100%">
                 
                     <rsweb:ReportViewer ID="RequisitionReport" runat="server" Height="200px" 
                         Width="100%">
                     </rsweb:ReportViewer>
                 
                 </td></tr>
                </table>
                </ContentTemplate>
                   
                </asp:UpdatePanel>
                </div>
            </td>
            <td style="width: 3%"></td>
        </tr>
    </table>
    <table>
     <tr>
                                        <td style="width: 5%">
                                            &nbsp;</td>
                                        <td align="right" style="width: 90%">
                                           <asp:Panel ID="LoginPanel" runat="server" CssClass="modalPopup" Style="display:none; padding: 15px 15px 15px 15px; background-color: White; border: 1px solid black;" Width="250px" Height="80px">

                    <table style="width: 250px;">
                        <tr>
                            <td style="width: 150px" align="left">
                                <asp:Label ID="lblUserName" runat="server" Font-Size="8pt" Height="23px" Text="Login ID" Width="100px"></asp:Label>
                            </td>
                            <td style="width: 116px">
                                <asp:TextBox SkinID="tbGray" ID="loginId" runat="server" Font-Size="8pt" Width="115px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 150px" align="left">
                                <asp:Label ID="lblPassword" runat="server" Font-Size="8pt" Height="23px" Text="Password" Width="100px"></asp:Label>
                            </td>
                            <td style="width: 116px">
                                <asp:TextBox SkinID="tbGray" ID="pwd" runat="server" Font-Size="8pt" Width="115px" TextMode="Password"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 150px">
                                <asp:Button ID="CancelBtn" OnClientClick="HideModalDiv();" runat="server" Font-Size="8pt" Text="Cancel" Width="60px"  />
                            </td>
                            <td style="width: 116px">
                                <asp:Button ID="LoginBtn" OnClientClick="HideModalDiv();" runat="server" 
                                    Font-Size="8pt" Text="Authorize" onclick="LoginBtn_Click"/></td>
                        </tr>
                    </table>
                </asp:Panel>
                                            
                                        </td>
                                        <td style="width: 5%">
                                            &nbsp;</td>
                                    </tr>
                                    </table>
                                      <div id="divBackground" style="position: fixed; z-index: 999; height: 100%; width: 100%;
        top: 0; left:0; background-color: White; filter: alpha(opacity=60); opacity: 0.6; -moz-opacity: 0.8;-webkit-opacity: 0.8;display:none">
    </div>
</asp:Content>

