<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmProjectCostAnalyses.aspx.cs" Inherits="frmProjectCostAnalyses" %>

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
        Height="30px" Width="110px" BorderStyle="Outset" onclick="btnSave_Click"  />
        </td>
        <td align="center" >
        <asp:Button ID="btnNew" runat="server" ToolTip="New"   Text="New" 
        Height="30px" Width="110px" BorderStyle="Outset" onclick="btnNew_Click"   /> 
        </td>
        <td align="center" >
        <asp:Button ID="btnClear" runat="server"  ToolTip="Clear"  Text="Clear" 
        Height="30px" Width="110px" BorderStyle="Outset" onclick="btnClear_Click"  />
        </td>
        <td align="center" >
        <asp:Button ID="btnPrint" runat="server" ToolTip="Print PO" Text="Print" 
        Height="30px" Width="110px" BorderStyle="Outset" onclick="btnPrint_Click"   />
        </td> 
        </table>
 <table style="width:100%">
     <tr>
         <td style="width:5%"></td>
          <td style="width:90%">
              <table style="width:100%">
                  
                  <tr>
                     
                      <td style="width:20%; height: 26px;">Project Name</td>
                      <td style="width:5%; text-align: center; height: 26px;">:</td>
                      <td style="width:20%; height: 26px;">
                          <asp:TextBox ID="txtProjectName" runat="server" Width="100%"></asp:TextBox>
                      </td>
                       <td style="width:10%; height: 26px;"></td>
                         <td style="width:20%; height: 26px;">Date</td>
                      <td style="width:5%; text-align: center; height: 26px;">:</td>
                      <td style="width:20%; height: 26px;">
                           <asp:TextBox ID="txtDate" runat="server" CssClass="tbc" Font-Size="8pt" 
                                                                PlaceHolder="dd/MM/yyyy" SkinId="tbPlain" style="text-align:center;" 
                                                                Width="93%"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="Calendarextender1" runat="server" 
                                                                Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDate" />

                      </td>
                  </tr>
                  <tr>
                      <td style="width:20%">&nbsp;</td>
                      <td style="width:5%">&nbsp;</td>
                      <td style="width:20%">
                          <asp:HiddenField ID="HiId" runat="server" />
                      </td>
                       <td style="width:10%">&nbsp;</td>
                         <td style="width:20%">&nbsp;</td>
                      <td style="width:5%">&nbsp;</td>
                      <td style="width:20%">&nbsp;</td>
                  </tr>
                  <tr>
                      <td colspan="7">
                          <table style="width: 100%">
                              <tr>
                                  <td style="width:100%">
                                       <asp:UpdatePanel ID="MRIesms_UP" runat="server"  UpdateMode="Conditional">
                <ContentTemplate>
                
                                                <div ID="ItemsDetails" runat="server">
                                                   
                                                            <asp:GridView ID="dgProjectCost" runat="server" AutoGenerateColumns="False" 
                                                                BorderColor="LightGray" CssClass="mGrid" Font-Size="9pt" 
                                                               
                                                                Width="100%" onrowdatabound="dgProjectCost_RowDataBound"  
                                                               >
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
                                                                    <asp:TemplateField HeaderText="Code">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtCode" runat="server" AutoPostBack="true" Enabled="true" 
                                                                                Font-Size="8pt" MaxLength="15" onFocus="this.select()" 
                                                                                SkinId="tbPlain" 
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
                                                                                Width="96%" ontextchanged="txtItemName_TextChanged"></asp:TextBox>
                                                                            <ajaxToolkit:AutoCompleteExtender ID="autoComplete1" runat="server" 
                                                                                CompletionInterval="20" CompletionSetCount="12" EnableCaching="true" 
                                                                                MinimumPrefixLength="1" ServiceMethod="GetItemListSerch" 
                                                                                ServicePath="AutoComplete.asmx" TargetControlID="txtItemName" />
                                                                          <%--  <asp:Label ID="lblItemsID" runat="server" style="display:none;" 
                                                                                Text='<%#Eval("ItemId")%>'></asp:Label>--%>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        <ItemStyle HorizontalAlign="Center" Width="25%" />
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
                                                                    
                                                                    <asp:TemplateField HeaderText="Rate Of Qty">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtRate" runat="server" AutoPostBack="true" 
                                                                                CssClass="6xtVisibleAlign" Enabled="true" Font-Size="8pt" 
                                                                                onFocus="this.select()" onkeypress="return isNumber(event)" placeHolder="0.00" 
                                                                                SkinId="tbPlain" style="text-align:right;" Text='<%#Eval("ItemRate")%>' 
                                                                                Width="90%" ontextchanged="txtRate_TextChanged"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:TemplateField>
                                                                      <asp:TemplateField HeaderText="Item Quentity">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtqnty" runat="server" AutoPostBack="true" 
                                                                                CssClass="6xtVisibleAlign" Enabled="true" Font-Size="8pt" 
                                                                                onFocus="this.select()" onkeypress="return isNumber(event)" placeHolder="0.00" 
                                                                                SkinId="tbPlain" style="text-align:right;" Text='<%#Eval("Qnty")%>' 
                                                                                Width="90%" ontextchanged="txtqnty_TextChanged"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:TemplateField>
                                                                      <asp:TemplateField HeaderText="Total Amount">
                                  <ItemTemplate>
                                    <asp:Label ID="lblTotal" runat="server" 
                                     Font-Size="8pt" Width="95%" Text='<%#Eval("Total")%>'></asp:Label>                                            
                                    </ItemTemplate></asp:TemplateField>
                                                                      <asp:TemplateField HeaderText="Remarks">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtRemarks" runat="server" AutoPostBack="False" 
                                                                                CssClass="txtVisibleAlign" Enabled="True" Font-Size="8pt" 
                                                                                onFocus="this.select()" 
                                                                                SkinId="tbPlain" style="text-align:right;" Text='<%#Eval("Remarksany")%>' 
                                                                                Width="90%">
           </asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" Width="8%" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    </asp:TemplateField>
                                                                    
                                                                                                  <asp:TemplateField HeaderText="ID">
                                  <ItemTemplate>
                                    <asp:Label ID="lblid" runat="server" 
                                     Font-Size="8pt" Width="95%" Text='<%#Eval("ID")%>'></asp:Label>                                            
                                    </ItemTemplate></asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle Font-Bold="True" Font-Size="9pt" ForeColor="White" />
                                                                <PagerStyle CssClass="pgr" ForeColor="White" HorizontalAlign="Center" />
                                                                <RowStyle BackColor="White" />
                                                            </asp:GridView>
                                                       
                                                </div>
                                                </ContentTemplate>
                </asp:UpdatePanel>
                                  </td>
                              </tr>
                          </table>
                           
                      </td>
                  </tr>
                  <tr>
                      <td style="width:20%; height: 17px;">&nbsp;</td>
                      <td style="width:5%; height: 17px;">&nbsp;</td>
                      <td style="width:20%; height: 17px;">&nbsp;</td>
                       <td style="width:10%; height: 17px;">&nbsp;</td>
                         <td style="width:20%; height: 17px; text-align: right;">
                             <asp:Label ID="lblCost" runat="server" Text="Totel Cost :" Visible="False"></asp:Label>
                         &nbsp;</td>
                      <td style="width:5%; height: 17px; text-align: center;">&nbsp;</td>
                      <td style="width:20%; height: 17px;">
                      <%--   <asp:TextBox ID="txtCostPrice" runat="server" CssClass="tbc" Font-Size="8pt" onfocus="this.select();" SkinID="tbPlain" TabIndex="10"
             style="text-align:right;" Width="92%" Enabled="False" onkeypress="return isNumber(event)"></asp:TextBox>--%>
             
                          <asp:TextBox ID="txtCostPrice" runat="server" Visible="False"></asp:TextBox>
                      </td>
                  </tr>
                    </table>
                  <tr>
                      <td style="height: 17px;" colspan="7">
                          <asp:GridView ID="dgProposeCostHistory" runat="server" 
                         AutoGenerateColumns="False" CssClass="mGrid" Width="100%" 
                              onselectedindexchanged="dgProposeCostHistory_SelectedIndexChanged" 
                              onrowdatabound="dgProposeCostHistory_RowDataBound" >
                                              
                                                
                         <Columns>
                             <asp:CommandField ShowSelectButton="True" />
                             <asp:BoundField DataField="ID" HeaderText="Id" />
                             <asp:BoundField DataField="ProjectName" HeaderText="Project Name" />
                             <asp:BoundField DataField="Date" HeaderText="Date" >
                             <FooterStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                             <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                             </asp:BoundField>
                             <asp:BoundField DataField="CostPrice" HeaderText="Totel Cost" >                         
                             <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                             </asp:BoundField>
                         </Columns>
                     </asp:GridView>
                      </td>
                  </tr>
                  <td style="width:5%"></td>
              </table>
          
           
</asp:Content>

