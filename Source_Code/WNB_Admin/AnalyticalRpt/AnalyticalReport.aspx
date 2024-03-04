<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="AnalyticalReport.aspx.vb" Inherits="WNB_Admin.AnalyticalReport" 
    title="Analytical Reports" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


    <%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    
 <script language="javascript" type="text/javascript">
     function openPage(objUser) {
         var strURL;
         var strFileName = document.getElementById('ctl00_ContentPlaceHolder1_HdnReportFileName').value;

         var ddlExportFormat = document.getElementById('ctl00_ContentPlaceHolder1_ddlExportFormat');
         if (ddlExportFormat.options[ddlExportFormat.selectedIndex].value == "4") {
             strURL = '../Temp/' + objUser + '_' + strFileName.replace('.rpt', '.xls');
         }
         else {
             strURL = '../Temp/' + objUser + '_' + strFileName.replace('.rpt', '.csv');
         }


         mywindow = window.open(strURL, '', 'resizable=1');
        
         return false;
     }
     
     function EnableDisableDepCountryDDL(elementRef) {
         var radioButtonListArray = elementRef.getElementsByTagName('input');
         var checkedValue = '';

         for (var i = 0; i < radioButtonListArray.length; i++) {
             var radioButtonRef = radioButtonListArray[i];

             if (radioButtonRef.checked == true) {
                 // To get the Value property, use this code:
                 checkedValue = radioButtonRef.value;
             }
         }

         if (checkedValue == 'Airport') {
             document.getElementById('ctl00_ContentPlaceHolder1_trDepPortType').style.display='inline'; 
             document.getElementById('ctl00_ContentPlaceHolder1_trDepCountry').style.display='none'; 
             document.getElementById('ctl00_ContentPlaceHolder1_ddlDepCountry').selectedIndex=0;
         }
         else {
             document.getElementById('ctl00_ContentPlaceHolder1_trDepPortType').style.display='none'; 
              document.getElementById('ctl00_ContentPlaceHolder1_trDepCountry').style.display='inline';
               document.getElementById('ctl00_ContentPlaceHolder1_LstDepAirport').selectedIndex=0;
         }         
     }


     function EnableDisableArrCountryDDL(elementRef) {
         var radioButtonListArray = elementRef.getElementsByTagName('input');
         var checkedValue = '';

         for (var i = 0; i < radioButtonListArray.length; i++) {
             var radioButtonRef = radioButtonListArray[i];

             if (radioButtonRef.checked == true) {
                 // To get the Value property, use this code:
                 checkedValue = radioButtonRef.value;
             }
         }

         if (checkedValue == 'Airport') {
             document.getElementById('ctl00_ContentPlaceHolder1_trArrPortType').style.display='inline'; 
             document.getElementById('ctl00_ContentPlaceHolder1_trArrCountry').style.display='none'; 
             document.getElementById('ctl00_ContentPlaceHolder1_ddlArrCountry').selectedIndex=0;
         }
         else {
             document.getElementById('ctl00_ContentPlaceHolder1_trArrPortType').style.display='none'; 
              document.getElementById('ctl00_ContentPlaceHolder1_trArrCountry').style.display='inline';
               document.getElementById('ctl00_ContentPlaceHolder1_LstARRAirport').selectedIndex=0;              
         }         
     }

     
     function EnableDisableBaseDDL(elementRef) {
         var radioButtonListArray = elementRef.getElementsByTagName('input');
         var checkedValue = '';

         for (var i = 0; i < radioButtonListArray.length; i++) {
             var radioButtonRef = radioButtonListArray[i];

             if (radioButtonRef.checked == true) {
                 // To get the Value property, use this code:
                 checkedValue = radioButtonRef.value;
             }
         }

         if (checkedValue == 'UTC') {
             document.getElementById('ctl00_ContentPlaceHolder1_ddlBase').disabled=true; 
             document.getElementById('ctl00_ContentPlaceHolder1_ddlBase').selectedIndex=0;
         }
         else {
             document.getElementById('ctl00_ContentPlaceHolder1_ddlBase').disabled=false;  
         }         
     }

     function getRadioButtonListSelection(elementRef) {
         var radioButtonListArray = elementRef.getElementsByTagName('input');
         var checkedValue = '';

         for (var i = 0; i < radioButtonListArray.length; i++) {
             var radioButtonRef = radioButtonListArray[i];

             if (radioButtonRef.checked == true) {
                 // To get the Value property, use this code:
                 checkedValue = radioButtonRef.value;
             }
         }

         if (checkedValue == 'Trainee') {
             document.getElementById('lblRPTOption').innerText = 'Only Unassigned.'; 
             document.getElementById('ctl00_ContentPlaceHolder1_lstSpecCrewCat').disabled=false; 
         }
         else {
             document.getElementById('lblRPTOption').innerText = 'Only Unassigned.'; 
             document.getElementById('ctl00_ContentPlaceHolder1_lstSpecCrewCat').disabled=true; 
             document.getElementById('ctl00_ContentPlaceHolder1_lstSpecCrewCat').selectedIndex=0;
         } 
         
         
         
                 
     }
     
     function getSelectedRadioButton(elementRef) {
     
         var radioButtonListArray = elementRef.getElementsByTagName('input');
         var checkedValue = '';
         
         
         for (var i = 0; i < radioButtonListArray.length; i++) {
             var radioButtonRef = radioButtonListArray[i];

             if (radioButtonRef.checked == true) {
                 // To get the Value property, use this code:
                 checkedValue = radioButtonRef.value;
             }
         }

       
         return checkedValue;      
     }      
           
      function IsListBoxItemSelected(objSelect) 
       {
           
             var optsLength = objSelect.options.length;
             var IsSelected=false;
             
              for(var i=0;i<optsLength;i++){
                if(objSelect.options[i].selected)
                    IsSelected=true;
              }
              
              return IsSelected;
        }
     
      function ValidateControls(strCmd)
        {
            var bReturn = 'true';
                      
            return bReturn;        
        }


        function checkDate(sender, args) {

            if (document.getElementById('ctl00_ContentPlaceHolder1_HdnReportId').value == '27') {

                var strDate = document.getElementById('ctl00_ContentPlaceHolder1_HdnSixMonthBackDate').value;
                var strDay = strDate.substr(0, 2);
                var strMonth = strDate.substr(3, 2);
                var strYear = strDate.substr(6, 4);
                var currentDate = new Date();

                var dt = new Date(parseInt(strYear), parseInt(strMonth) - 1, parseInt(strDay), 0, 0, 0, 0);
                if (sender._selectedDate < dt) {
                   sender._textbox.set_Value(currentDate.format(sender._format));
                    alert('As On Date within 6 months back from Today Date.');
                }
            }
        }
        
        var isShift=false;
        
        function keyUP(keyCode)
        {
             if(keyCode==16)
                isShift = false;
        }

       
        function isNumeric(keyCode)
        {
               if(keyCode==16)
                    isShift = true;       
                    
              return ((keyCode >= 48 && keyCode <= 57 || keyCode == 8 || keyCode == 9 || keyCode == 46 || keyCode == 190 ||(keyCode >= 96 && keyCode <= 105) ||(keyCode >= 37 && keyCode <= 40)) && isShift == false);

        }

 </script>
 
    <style type="text/css">
        .ReportTitle
        {
            font-size:18px;
            font-family:Times New Roman;
            font-weight:bold;
                        
        }
        .LabelHeader
        {
            font-size:15px;
            font-family:Times New Roman;
            font-weight:bold;
            padding-left:5px;
                        
        }
        
        .Label
        {
	        font-size:12px;
	        font-weight:bold;
	        text-decoration:none;
	        color:#000000;	
	        height:30px;
	        text-align:left;
        }

        .Button
        {
            font-size:15px;
            font-family:Times New Roman;
            font-weight:bold;                       
            border:solid 1px Black;
            height:22px;
            
        }
        .style3
        {
            height: 25px;
            border: solid 1px Black;
        }
        .gdItem
        {
            background-color:White;           
        }
        .gdHeader
        {                  
            background-color:#E4E4EC;              
        }
        
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="36000" ></asp:ScriptManager>

 <div style="width: 100%">

    <table cellpadding="0" cellspacing="0" style="width:100%;">
        <tr>
            <td class="tr" align="center">
                <asp:Label ID="lblReportTitle" runat="server" Text="IPad DB Version History" CssClass="ReportTitle" ></asp:Label>                
            </td>
        </tr>
    </table>
    <br /> 
 
    <table cellpadding="0" cellspacing="0" style="width:100%;border:solid 1px Black;"> 
        <tr>
            <td width="30%" align="left" valign="top" id="tdParam" runat="server">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" > 
                    <ContentTemplate> 
                        <table cellpadding="0" cellspacing="0" border="0" width="100%" > 
                        <tr style="height:5px;"><td></td></tr>
                        <tr>
                            <td width="10px"></td>
                            <td>
                                <fieldset>
                                    <legend><span id="Span2" style="font-weight:bold;font-size:small;color:#000000;">Selection Parameters</span></legend>                                     
                                    <table cellpadding="0" cellspacing="9" style="width:100%;text-align:center;" border="0"> 
                                                                    
                                        <tr id="trPort" runat="server">                                       
                                            <td class="Label"><asp:Label ID="lblIpadUDId" runat="server" Text="IPAD UDID" ></asp:Label><br />
                                                <asp:DropDownList ID="ddlIpadUDId" runat="server" Width="100%" Height="22px" DataTextField="IPAD_UDID" DataValueField="IPAD_UDID"></asp:DropDownList>
                                                <input id="HdnReportId" type="hidden" runat="server" />
                                                <input id="HdnReportFileName" type="hidden" runat="server" />
                                                <input id="HdnReportColNames" type="hidden" runat="server" />
                                                <input id="HdnReportColCaptions" type="hidden" runat="server" />
                                                <input id="HdnFilterCriteria" type="hidden" runat="server" />
                                            </td>           
                                        </tr> 
                                         <tr >
                                            <td class="Label" >
                                                <input id="chkRptOption" runat="server" type="checkbox"/><label id="lblRPTOption">Exclude ipad having latest database.</label>                                                 
                                            </td>
                                        </tr>
                                        <tr >
                                           <td class="Label">
                                           <fieldset>
                                                <legend><span id="Span4" style="font-weight:bold;font-size:small;color:#000000;">DB Version No</span></legend>                                     
                                                <br />
                                                <asp:DropDownList ID="ddlDBVersionNo" runat="server" Width="150px" Height="22px" DataTextField="DBVersionNo" DataValueField="DBVersionNo"></asp:DropDownList>
                                                 <br />
                                                 <asp:RadioButtonList ID="radDBVerExclude" runat="server" RepeatDirection="Vertical"  CssClass="Label" Width="220px" Height="10px" TextAlign="Right" CellPadding="0" CellSpacing="0" >
                                                    <asp:ListItem Text="Including DB Version No" Value="Include" Selected="true"></asp:ListItem>
                                                    <asp:ListItem Text="Excluding DB Version No" Value="Exclude"></asp:ListItem>
                                                 </asp:RadioButtonList>                                                
                                            </fieldset>  
                                           </td>
                                        </tr>
                                        <tr id="trGenerateRpt" runat="server"> 
                                             <td class="Label" style="height:22px;">
                                              <asp:Button ID="BtnGenReport" runat="server" Text="Generate Report" CssClass="Button" Width="130px" />
                                             </td>
                                        </tr>                                                                                                
                                    </table>
                                </fieldset>                        
                            </td>
                            <td width="10px"></td>
                        </tr>
                        <tr style="height:5px;"><td></td></tr>
                        <tr>
                            <td width="10px"></td>
                            <td>
                                <fieldset>
                                    <legend><span id="Span3" style="font-weight:bold;font-size:small;color:#000000;">Export Option</span></legend>                                     
                                    <table cellpadding="0" cellspacing="4" style="width:100%;text-align:center;" border="0">  
                                        <asp:Panel ID="pnlExport" runat="server">                               
                                            <tr id="trExportFormat" runat="server">                                        
                                               <td class="Label"><asp:DropDownList ID="ddlExportFormat" runat="server" Width="80px" Height="22px">
                                                        <asp:ListItem Text="Excel" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="CSV" Value="-1"></asp:ListItem>
                                                        <asp:ListItem Text = "PDF" Value = "5"></asp:ListItem>
                                                    </asp:DropDownList> &nbsp;<asp:Button ID="btnExport" runat="server" Text="Export" CssClass="Button" Width="60px" />
                                                </td>                                                
                                            </tr> 
                                        </asp:Panel>                                  
                                    </table>
                                </fieldset>                        
                            </td>
                            <td width="10px"></td>
                        </tr>                    
                    </table>
                    </ContentTemplate>
                    <Triggers>
                    <asp:PostBackTrigger ControlID="btnExport"  />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td  valign="top">
                <table cellpadding="0" cellspacing="0" border="0" width="100%" > 
                    <tr style="height:5px;">
                        <td>
                            <div style="position:absolute; margin-top:100px; left:700px;">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
                                    AssociatedUpdatePanelID="UpdatePanel1"
                                    DisplayAfter="50" DynamicLayout="true" >                                                                                                    
                                    <ProgressTemplate>
                                        <img border="0" src="../Images/loading.gif" />
                                    </ProgressTemplate>                                                                                                    
                                </asp:UpdateProgress>
                                <asp:UpdateProgress ID="UpdateProgress2" runat="server" 
                                    AssociatedUpdatePanelID="UpdatePanel2"
                                    DisplayAfter="50" DynamicLayout="true" >                                                                                                    
                                    <ProgressTemplate>
                                        <img border="0" src="../Images/loading.gif" />
                                    </ProgressTemplate>                                                                                                    
                                </asp:UpdateProgress> 
                            </div>                                    
                        </td>
                    </tr>
                    <tr>                       
                        <td valign="top">
                            <fieldset>
                                <legend><span id="Span1" style="font-weight:bold;font-size:small;color:#000000;">Report Area</span></legend>                                     
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" > 
                                    <ContentTemplate>    
                                    <table cellpadding="0" cellspacing="10" style="width:100%;" border="0">                                   
                                    <tr id="trCryRpt" runat="server"> 
                                        <td>                                                     
                                            <CR:CrystalReportViewer ID="CRV_CPReport" runat="server" AutoDataBind="true" 
                                                DisplayGroupTree="False" EnableViewState ="true"
                                                EnableDatabaseLogonPrompt="False" EnableDrillDown="False" 
                                                EnableParameterPrompt="False"  HasCrystalLogo="False" 
                                                HasDrillUpButton="False" HasGotoPageButton="False" 
                                                HasRefreshButton="false" HasSearchButton="False" 
                                                HasToggleGroupTreeButton="False" HasViewList="False"  
                                                HasZoomFactorList="False" HasExportButton="False" HasPrintButton="False" HyperlinkTarget="_blank" />                                                 
                                        </td> 
                                    </tr>
                                                                      
                                    <tr id="trGridRpt" runat="server" > 
                                        <td style="width:100%;"> 
                                             <table cellpadding="0" cellspacing="0" style="width:100%;" border="0">
                                                <tr>
                                                    <td style="width:100%;" align="center" >
                                                        <asp:DataGrid ID="dgRptHeader" runat="server" AutoGenerateColumns="false" ShowHeader="true" BorderStyle="None" GridLines="None" ItemStyle-Width="100%"  Width="100%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                            <Columns>
                                                                <asp:TemplateColumn>
                                                                    <HeaderTemplate>
                                                                        <table width="100%">
                                                                            <tr style="height:30px;">
                                                                                <td align="center" ><asp:Label ID="lblRptHeader" runat="server" Font-Bold="true" Font-Underline="true" Font-Size="Medium" Text="Crew Establishment Report" style="text-align:center; font-family:Verdana;"></asp:Label></td>
                                                                            </tr>
                                                                            <tr style="height:15px;">
                                                                                <td align="left" ><asp:Label ID="lblFilterCriteria" runat="server" Text="" ></asp:Label></td>
                                                                            </tr>
                                                                        </table>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate >   
                                                                        <table width="100%">
                                                                            <tr>                                                       
                                                                                <td align="left" >                           
                                                                                    <asp:DataGrid ID="dgRptDetail" HeaderStyle-BackColor="#E4E4EC" HeaderStyle-Font-Bold="true" runat="server"  
                                                                                    AutoGenerateColumns="true" ShowHeader="true"  GridLines="Both" CellPadding="0" CellSpacing="0"  ItemStyle-CssClass="gdItem"
                                                                                    ItemStyle-HorizontalAlign="left"  HeaderStyle-HorizontalAlign="Left" ItemStyle-Height="18px" HeaderStyle-Height="20px" ></asp:DataGrid>
                                                                                </td>
                                                                            </tr>
                                                                            
                                                                        </table> 
                                                                     </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                            </Columns>
                                                        </asp:DataGrid>   
                                                    </td> 
                                                </tr>                
                                            </table>  
                                        </td> 
                                    </tr>                                    
                                </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </fieldset>
                        </td>
                         <td width="10px"></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="height:10px;"><td colspan="2"></td></tr>
    </table>    
</div>
 
</asp:Content>
