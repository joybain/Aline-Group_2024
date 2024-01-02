<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="frmDistributionRecived.aspx.cs" Inherits="frmDistributionRecived" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <script language="javascript" type="text/javascript">

        function OpenWindow(Url) {
            var testwindow = window.open(Url, '', 'width=600px,height=400px,top=100,left=300,scrollbars=1');
        }
        function myFunction() {
            var input, filter, table, tr, td, td2, i, txtValue, txtValue2;
            input = document.getElementById("MainContent_TabContainer1_TabPanel3_txtSearchBydateCode");
            filter = input.value.toUpperCase();
            table = document.getElementById("MainContent_TabContainer1_TabPanel3_dgHistory");
            tr = table.getElementsByTagName("tr");
            for (i = 0; i < tr.length; i++) {
                td = tr[i].getElementsByTagName("td")[3];
                td2 = tr[i].getElementsByTagName("td")[2];
                if (td2 || td) {
                    txtValue = td2.textContent || td2.innerText;
                    txtValue2 = td.textContent || td.innerText;
                    if (txtValue.toUpperCase().indexOf(filter) > -1 || txtValue2.toUpperCase().indexOf(filter) > -1) {
                        tr[i].style.display = "";
                    } else {
                        tr[i].style.display = "none";
                    }
                }
            }
        }
        $(document).ready(function () {
            setTimeout(function () {
                if ($("#MainContent_TabContainer1_TabPanel1_tab").hasClass('ajax__tab_active')) {
                    $("#MainContent_TabContainer1_TabPanel1_txtItemSearch").focus();
                }


            }, 100);

        });
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
     <table  id="pageFooterWrapper">
  <tr>  
        <td align="center">
        <asp:Button ID="BtnDelete" runat="server" BorderStyle="Outset" 
                                            BorderWidth="1px" Height="35px"  
                                            onclientclick="javascript:return window.confirm('are u really want to delete these data')" 
                                            Text="Delete" ToolTip="Delete Record" 
                                            Width="110px" />
        </td>       
        <td align="center" >
            &nbsp;</td>       
        <td align="center" >
                                        <asp:Button runat="server" 
                OnClientClick="javascript:return window.confirm(&#39;are u really want to Recived these data&#39;)" 
                Text="Recived" BorderWidth="1px" BorderStyle="Outset" Height="35px" 
                ToolTip="Save or Update Record" Width="110px" ID="btnRecive" onclick="btnRecive_Click"></asp:Button>

        </td>
        <td align="center" >
                                        <asp:Button runat="server" 
                OnClientClick="javascript:return window.confirm(&#39;are u really want to Return these data&#39;)" 
                Text="Return" BorderWidth="1px" BorderStyle="Outset" Height="35px" 
                ToolTip="Return" Width="110px" ID="btnReturn"></asp:Button>

        </td>
        <td align="center" >
                                        <asp:Button runat="server" Text="Clear" 
                BorderWidth="1px" BorderStyle="Outset" Height="35px" ToolTip="Clear Form" 
                Width="110px" ID="BtnReset" onclick="BtnReset_Click"></asp:Button>

        </td>
        <td align="center" >
        <asp:Button ID="btnPrint" runat="server" ToolTip="Print PO" Text="Print" 
        Height="30px" Width="110px" BorderStyle="Outset"   />
        </td>            
        
   </tr>
   </table> 
    <table style="width: 100%">
        <tr>
            <td style="width: 5%">
            </td>
            <td style="width: 90%">
                <asp:Panel ID="Panel1" runat="server">
                    <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" 
                        Width="100%" >
                        <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Recived and Return">
                        <HeaderTemplate>
                           Recived and Return
                        </HeaderTemplate>
                        <ContentTemplate>
                           <fieldset style="vertical-align: top;  border: solid 1px #8BB381;" id="Fieldset1" runat="server">
                                           <legend style="color: maroon;"><b>
                                               <asp:Label ID="Label5" runat="server" Text="Search Recived Product" 
                                                   ></asp:Label>
                                               </b></legend>
                           <table style="width: 100%">
                           
                    <tr>
                       <td style="width:20%"> Project Name</td>
                         <td style="width:25%"> 
                             <asp:DropDownList ID="ddlProjectName" runat="server" AutoPostBack="True" 
                                 Font-Size="8pt" Height="26px" 
                                 SkinID="ddlPlain" 
                                 Width="100%" onselectedindexchanged="ddlProjectName_SelectedIndexChanged">
                             </asp:DropDownList>
                        </td>
                        <td style="width:10%"> </td>
                          <td style="width:20%; text-align: left;"> Site Name</td>
                           <td style="width:25%"> 
                               <asp:TextBox ID="txtSiteName" runat="server" Enabled="False" Width="92%"></asp:TextBox>
                        </td>
                    </tr>
                            
                               <tr>
                                   <td style="width:20%; height: 21px;">
                                   </td>
                                   <td style="width:25%; height: 21px;">
                                       <asp:HiddenField ID="hidMstId" runat="server" />
                                   </td>
                                   <td style="width:10%; height: 21px;">
                                   </td>
                                   <td style="width:20%; text-align: left; height: 21px;">
                                       Recived Date</td>
                                   <td style="width:25%; height: 21px;">
                                       <asp:TextBox ID="txtRecivedDate" runat="server" Width="92%"></asp:TextBox>
                                       <ajaxToolkit:CalendarExtender ID="Calendarextender1" runat="server" 
                                           Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtRecivedDate">
                                       </ajaxToolkit:CalendarExtender>
                                   </td>
                               </tr>
                               <tr>
                                   <td>
                                       <asp:Button ID="btnSerch" runat="server" BorderStyle="Outset" BorderWidth="1px" 
                                           Height="35px" OnClick="btnSerch_Click" TabIndex="904" Text="Serch" 
                                           ToolTip="Serch" Width="110px" />
                                   </td>
                                   <td>
                                       <asp:Button ID="btnExcel0" runat="server" BorderStyle="Outset" 
                                           BorderWidth="1px" Height="35px" OnClick="btnExcel0_Click" TabIndex="904" 
                                           Text="Refresh" ToolTip="Refresh" Width="110px" />
                                   </td>
                                   <td colspan="3">
                                       &nbsp;</td>
                               </tr>
                               <tr>
                                   <td style="width:20%">
                                       &nbsp;</td>
                                   <td style="width:25%">
                                       &nbsp;</td>
                                   <td style="width:10%">
                                       &nbsp;</td>
                                   <td style="width:20%; text-align: left;">
                                       &nbsp;</td>
                                   <td style="width:25%">
                                       &nbsp;</td>
                               </tr>
                               </fieldset>
                               <tr>
                                   <td colspan="5">
                                       <fieldset style="vertical-align: top;  border: solid 1px #8BB381;" id="HistoryFildSet" runat="server">
                                           <legend style="color: maroon;"><b>
                                               <asp:Label ID="Label1" runat="server" Text="History"></asp:Label>
                                               </b></legend>
                                           <asp:GridView ID="dgHistory" runat="server" AllowPaging="True" 
                                               AutoGenerateColumns="False" CssClass="mGrid" 
                                               onrowdatabound="dgHistory_RowDataBound" 
                                               OnSelectedIndexChanged="dgHistory_SelectedIndexChanged" 
                                               style="text-align:center;" Width="100%">
                                               <Columns>
                                                   <asp:CommandField ShowSelectButton="True">
                                                   <ItemStyle Font-Bold="True" HorizontalAlign="Center" Width="5%" />
                                                   </asp:CommandField>
                                                   <asp:BoundField DataField="ID" HeaderText="ID">
                                                   <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                   </asp:BoundField>
                                                   <asp:BoundField DataField="Code" HeaderText="Code">
                                                   <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                   </asp:BoundField>
                                                   <asp:BoundField DataField="fromProjectName" HeaderText="From-Project-Name">
                                                   <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                   </asp:BoundField>
                                                   <asp:BoundField DataField="TransferDate" HeaderText="Transfer Date">
                                                   <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                   </asp:BoundField>
                                                    <asp:BoundField DataField="ToProjectName" HeaderText="To-Project-Name">
                                                   <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                   </asp:BoundField>
                                                   <asp:BoundField DataField="Remark" HeaderText="Remark">
                                                   <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                   </asp:BoundField>
                                                   <asp:BoundField DataField="Status" HeaderText="Status">
                                                   <ItemStyle HorizontalAlign="Left" Width="12%" />
                                                   </asp:BoundField>
                                               </Columns>
                                           </asp:GridView>
                                       </fieldset>
                                   </td>
                               </tr>
                               <tr>
                                   <td style="width:20%">
                                       &nbsp;</td>
                                   <td style="width:25%">
                                       &nbsp;</td>
                                   <td style="width:10%">
                                       &nbsp;</td>
                                   <td style="width:20%; text-align: left;">
                                       &nbsp;</td>
                                   <td style="width:25%">
                                       &nbsp;</td>
                               </tr>
                               <tr>
                                   <td colspan="5">
                                   </td>
                               </tr>
                               <tr>
                                   <td colspan="5">
                                       &nbsp;</td>
                               </tr>
                               <tr>
                                   <td colspan="5">
                                   </td>
                               </tr>
                               </tr>
                            
                </table>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                     <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Recived Data">
                        <HeaderTemplate>
                          Recived Data
                        </HeaderTemplate>
                        <ContentTemplate>
                           
                           <table style="width: 100%">
                                <tr>
                                    <td style="width:5%">
                                        &nbsp;</td>
                                    <td style="width:90%">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 40%; text-align: right;">
                                                    Material Requistion No</td>
                                                <td style="width: 5%; text-align: center;">
                                                    :</td>
                                                <td style="width: 55%">
                                                    <asp:TextBox ID="txtMatrialNo" runat="server" Width="193px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 40%; text-align: right;">
                                                    Order Number</td>
                                                <td style="width: 5%; text-align: center;">
                                                    :</td>
                                                <td style="width: 55%">
                                                    <asp:TextBox ID="txtOrderNumber" runat="server" Width="193px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 40%; text-align: right;">
                                                    Total Requistion</td>
                                                <td style="width: 5%; text-align: center;">
                                                    :</td>
                                                <td style="width: 55%">
                                                    <asp:TextBox ID="txtTotalTotalRequ" runat="server" Width="193px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 40%; text-align: right;">
                                                    Total Due Requistion
                                                </td>
                                                <td style="width: 5%; text-align: center;">
                                                    &nbsp;</td>
                                                <td style="width: 55%">
                                                    <asp:TextBox ID="txtTotalDueRequ" runat="server" Width="193px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width:5%">
                                        <tr>
                                            <td style="width:5%">
                                                &nbsp;</td>
                                            <td style="width:90%">
                                                <fieldset style="vertical-align: top; border: solid 1px #8BB381;">
                                                    <legend style="color: maroon;"><b>
                                                        <asp:Label ID="lbldtlRecive" runat="server" Text="Detiels Recived Item"></asp:Label>
                                                        </b></legend>
                                                    <asp:GridView ID="dgDistributionRecived" runat="server" 
                                                        AutoGenerateColumns="False" BorderColor="LightGray" CssClass="mGrid" 
                                                        Font-Size="9pt" OnRowDataBound="dgDistributionRecived_RowDataBound" 
                                                        Width="100%">
                                                        <AlternatingRowStyle CssClass="alt" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" 
                                                                        Checked='<%# Eval("check").ToString().Equals("1") %>' 
                                                                        oncheckedchanged="chkSelect_CheckedChanged" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblid" runat="server" Font-Size="8pt" Text='<%#Eval("ID")%>' 
                                                                        Width="10%"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Code">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtCode" runat="server" AutoPostBack="true" Enabled="true" 
                                                                        Font-Size="8pt" MaxLength="15" onFocus="this.select()" SkinId="tbPlain" 
                                                                        Text='<%#Eval("item_code")%>' Width="90%"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <FooterStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemStyle HorizontalAlign="Center" Width="11%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Items Name">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtItemName" runat="server" AutoPostBack="True" 
                                                                        placeHolder="Search Items Code/Style No./Name" Text='<%#Eval("item_desc")%>' 
                                                                        Width="96%"></asp:TextBox>
                                                                    <%--  <asp:Label ID="lblItemsID" runat="server" style="display:none;" 
                                                                                Text='<%#Eval("ItemId")%>'></asp:Label>--%>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemStyle HorizontalAlign="Center" Width="25%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Due Qty">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtDueQty" runat="server" AutoPostBack="False" 
                                                                        CssClass="txtVisibleAlign" Enabled="false" Font-Size="8pt" 
                                                                        onFocus="this.select()" onkeypress="return isNumber(event)" placeHolder="0.00" 
                                                                        SkinId="tbPlain" style="text-align:right;" Text='<%#Eval("Due_qnty")%>' 
                                                                        Width="90%">
           </asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemStyle HorizontalAlign="Right" Width="8%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Transefer Qty">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtStockQuantity" runat="server" AutoPostBack="False" 
                                                                        CssClass="txtVisibleAlign" Enabled="false" Font-Size="8pt" 
                                                                        onFocus="this.select()" onkeypress="return isNumber(event)" placeHolder="0.00" 
                                                                        SkinId="tbPlain" style="text-align:right;" Text='<%#Eval("present_qnty")%>' 
                                                                        Width="90%">
           </asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemStyle HorizontalAlign="Right" Width="8%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Unit">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlMeasure" runat="server" AutoPostBack="true" 
                                                                        DataSource="<%#PopulateMeasure()%>" DataTextField="Name" DataValueField="ID" 
                                                                        Enabled="False" Font-Size="8pt" Height="26px" 
                                                                        SelectedValue='<%#Eval("msr_unit_code")%>' SkinId="ddlPlain" Width="96%">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                                <FooterStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemStyle HorizontalAlign="Center" Width="6%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remarks">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtRemarks" runat="server" AutoPostBack="False" 
                                                                        CssClass="txtVisibleAlign" Enabled="True" Font-Size="8pt" 
                                                                        onFocus="this.select()" SkinId="tbPlain" style="text-align:right;" 
                                                                        Text='<%#Eval("Remarks")%>' Width="90%">
           </asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                <ItemStyle HorizontalAlign="Right" Width="8%" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle Font-Bold="True" Font-Size="9pt" ForeColor="White" />
                                                        <PagerStyle CssClass="pgr" ForeColor="White" HorizontalAlign="Center" />
                                                        <RowStyle BackColor="White" />
                                                    </asp:GridView>
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </td>
                                </tr>
                </table>
                            <table style="width: 100%">
                                <tr>
                                    <td align="center" style="vertical-align: middle; width: 25%">
                                    </td>
                                    <td align="center" style="vertical-align: middle; width: 25%">
                                        <asp:Label ID="lblQuantity" runat="server"></asp:Label>
                                    </td>
                                    <td align="center" style="vertical-align: middle; width: 25%">
                                        &nbsp;</td>
                                    <td align="center" style="vertical-align: middle; width: 25%">
                                        &nbsp;</td>
                                </tr>
                            </table>
                            <td style="width: 5%;">
                                &nbsp;</td>
                            </tr>
                            </table>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                     <ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Project Stock">
                        <HeaderTemplate>
                          Project Stock
                        </HeaderTemplate>
                        <ContentTemplate>                           
                           <table style="width: 100%">                               
                  
                               <tr>
                                   
                                   <td style="width:5%">
                                       &nbsp;</td>
                                   <td style="width:90%">
                                      
                                     <table style="width:100%">
                                         <tr>
                                             <td style="width:100%">
                                             <fieldset style="vertical-align: top; border: solid 1px #8BB381;">
                                            <legend style="color: maroon;"><b>
                                                <asp:Label ID="Label4" runat="server" Text="Serch Project Wish Item"></asp:Label></b> </legend>
                                                 <table style="width:100%">
                                                     
                                                      <tr>
                                                         <td style="width:20%; height: 17px; font-weight: 700; color: #FF0000;">Serch Project Name</td>
                                                        <td style="width:35%; height: 17px;">
                                                            <asp:DropDownList ID="ddlSerchProjectName" runat="server" AutoPostBack="True" 
                                                                Font-Size="8pt" Height="30px" 
                                                                SkinID="ddlPlain" 
                                                                Width="100%">
                                                            </asp:DropDownList>
                                                          </td>
                                                          <td style="width:5%; height: 17px;"></td>
                                                         <td style="width:20%; height: 17px;">
                                                             <asp:Button ID="btnSerchProjectStock" runat="server" BorderStyle="Outset" 
                                                                 BorderWidth="1px" Height="35px" 
                                                                 
                                                                 Text="Serch" ToolTip="Return" Width="110px" 
                                                                 onclick="btnSerchProjectStock_Click" />
                                                          </td>
                                                         <td style="width:20%; height: 17px;">
                                                             <asp:Button ID="btnProjectStockclear" runat="server" BorderStyle="Outset" 
                                                                 BorderWidth="1px" Height="35px" Text="Clear" ToolTip="Clear Form" 
                                                                 Width="110px" onclick="btnProjectStockclear_Click" />
                                                          </td>
                                                     </tr>                                                    
                                                  
                                                 </table>
                                                   </fieldset>
                                             </td>
                                           
                                         </tr>
                                         <tr>
                                             <td style="width:96%">
                                        <fieldset style="vertical-align: top; border: solid 1px #8BB381;">
                                            <legend style="color: maroon;"><b>
                                                <asp:Label ID="Label2" runat="server" Text="Project Stock"></asp:Label></b> </legend>
                                             <asp:GridView ID="gdiProjectStockHistory" runat="server" AllowPaging="True" 
                                               AutoGenerateColumns="False" CssClass="mGrid"                                                                                              
                                               style="text-align:center;" Width="100%" 
                                                onrowdatabound="gdiProjectStockHistory_RowDataBound" 
                                                onpageindexchanging="gdiProjectStockHistory_PageIndexChanging" PageSize="30" 
                                               >
                                               <Columns>
                                                   
                                                   <asp:BoundField DataField="ID" HeaderText="ID">
                                                   <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                   </asp:BoundField>
                                                   <asp:BoundField DataField="Code" HeaderText="Code">
                                                   <ItemStyle HorizontalAlign="Center" Width="12%" />
                                                   </asp:BoundField>
                                                   <asp:BoundField DataField="items_Desc" HeaderText="Items Name">
                                                   <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                   </asp:BoundField>
                                                    <asp:BoundField DataField="Qnty" HeaderText="Quantity">
                                                   <ItemStyle HorizontalAlign="Center" Width="12%" />
                                                   </asp:BoundField>
                                                     <asp:BoundField DataField="UOM" HeaderText="Unit Measurement">
                                                   <ItemStyle HorizontalAlign="Center" Width="12%" />
                                                   </asp:BoundField>
                                                 
                                                   <asp:BoundField DataField="ProjectName" HeaderText="Project Name">
                                                   <ItemStyle HorizontalAlign="Left" Width="12%" />
                                                   </asp:BoundField>
                                                   
                                               </Columns>
                                           </asp:GridView>
                                        </fieldset>
                                        </td>
                                        </tr>
                                        
                                        </table>
                                   </td>
                               </tr>
                </table>
                            <table style="width: 100%">
                                <tr>
                                    <td align="center" style="vertical-align: middle; width: 25%">
                                    </td>
                                    <td align="center" style="vertical-align: middle; width: 25%">
                                        <asp:Label ID="Label3" runat="server"></asp:Label>
                                    </td>
                                    <td align="center" style="vertical-align: middle; width: 25%">
                                        &nbsp;</td>
                                    <td align="center" style="vertical-align: middle; width: 25%">
                                        &nbsp;</td>
                                </tr>
                            </table>                          
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    </ajaxToolkit:TabContainer>

                </asp:Panel>
            </td>
            <td style="width: 5%">
            </td>
        </tr>
    </table>
</asp:Content>
