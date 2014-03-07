<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="fc" TagName="FilterControl" Src="~/Controls/FilterWindow.ascx" %>
<%@ Register TagPrefix="lsd" TagName="LoanSelectionDialog" Src="~/Controls/LoanSelectionDialog.ascx" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" Runat="Server">    
    <script type="text/javascript" src="Scripts/jquery-1.8.0.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery.helper.js"></script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" Runat="Server">

    <asp:Panel ID="mainPagePanel" runat="server" Visible="False">              
            <div class="module" style="width:30%; height:500px; float:left;">  
			    <div class="moduleHeader" style="position:relative;">DATORNICI (<asp:Label ID="countDebitLabel" runat="server"></asp:Label>)</div>  
			    <div class="module_content" style="height: 440px; overflow-y: scroll; margin:10px 5px 0 10px !important;">
				         <asp:GridView ID="creditListGridView" runat="server" 
                            EnableModelValidation="True" 
                            AutoGenerateColumns="False" 
                            CssClass="mGrid"
                            PagerStyle-CssClass="pgr"
                            SelectedRowStyle-BackColor="#CCCCFF"
                            AlternatingRowStyle-CssClass="alt"
                            AllowPaging="false" 
                            onselectedindexchanged="creditListGridView_SelectedIndexChanged" onrowdatabound="creditListGridView_RowDataBound"  >
                            <AlternatingRowStyle CssClass="alt" />
            
                            <Columns>            
                                <asp:BoundField DataField="loanID" HeaderText="loanID" HtmlEncode="False" HeaderStyle-CssClass="HiddenColumn" ItemStyle-CssClass="HiddenColumn" >  </asp:BoundField>
                                
                                <asp:BoundField DataField="contractCode" HeaderText="Nr.Contract" HtmlEncode="False" />

                                <asp:BoundField DataField="clentName" HeaderText="Client" HtmlEncode="False" />
               
                                <asp:BoundField DataField="daysDelay" HeaderText="Zile intirziere"   HtmlEncode="False" ItemStyle-ForeColor="Red" ItemStyle-HorizontalAlign="Right" />                
                            </Columns>
                        </asp:GridView>  
			    </div>
            </div>  

        
            <div style="width:60%; height:500px; float:left;">

                <div class="module" style=" width:250px; height:100px; float:left; ">
                    <div class="moduleHeader">Clienti</div>
                    <div class="module_content">
                        <div class="leftColumn" style="padding-top:10px; text-align:center;"  >
                            <asp:HyperLink ID="newClientHyperLink" runat="server">Nou</asp:HyperLink> 
                        </div>
                        <div class="rightColumn" style="padding-top:10px; text-align:center;" >
                            <fc:FilterControl ID="clientFilter" runat="server" Height="250px" Width="400px"  AllowMultiSelection="false" SelectButtonText="Client List" SelectButtonWidth="120px" TitleWindow="Lista Clientilor inregistrati" OnEntrySelected="ClientSelecetd_Event" />
                        </div>
                    </div>
                </div>
                                
                <div class="module" style=" width:250px; height:100px; float:left; ">
                    <div class="moduleHeader">Credite</div>
                    <div class="module_content">
                        <div class="leftColumn" style="padding-top:10px; text-align:center;" >
                            <asp:HyperLink ID="newTrainingHyperLink" runat="server" NavigateUrl="#">Nou</asp:HyperLink> 
                        </div>
                        <div class="rightColumn" style="padding-top:10px; text-align:center;" >
                            <%--<fc:FilterControl ID="trainingFilterWindow" runat="server"  Height="250px" Width="400px"  TitleWindow="Training Selection" AllowMultiSelection="true" SelectButtonText="Select Training" SelectButtonWidth="100px" OnEntrySelected="trainingSelecetd_Event"/>                            --%>                          
                            <asp:Button ID="loanSelectionButton" runat="server" Text="Credite inregistrate" Width="120px" OnClick="loanSelectionButton_Click" />
                            <lsd:LoanSelectionDialog ID="loanSelectionDialog" runat="server" OnOnClientSelected="loanSelectionDialog_OnClientSelected" />
                        </div>
                    </div>
                </div>

                <div class="module" style=" width:250px; height:100px; float:left; ">
                    <div class="moduleHeader">Ramburasri</div>
                    <div class="module_content">   
                        <div class="rightColumn" style="padding-top:10px; text-align:center;" >
                            <fc:FilterControl ID="paysFilterControl" runat="server" Height="250px" Width="400px"  AllowMultiSelection="false" SelectButtonText="Client List" SelectButtonWidth="120px" TitleWindow="Lista Clientilor inregistrati" OnEntrySelected="paysSelected_Event" />
                        </div>                        
                    </div>
                </div>

                <div class="module" style=" width:250px; height:150px; float:left; ">
                    <div class="moduleHeader">Contabilitate</div>
                    <div class="module_content">                      
                    </div>
                </div>

                <div class="module" style=" width:520px; height:150px; float:left;  ">
                    <div class="moduleHeader">Rapoarte</div>
                    <div class="module_content" style=" height:100px; overflow-y: scroll; margin: 5px 5px 0 5px !important;"> 

                         <asp:HyperLink ID="RPT_PAR_HyperLink" runat="server">PAR - raport</asp:HyperLink>     
                         <br />  
                         <asp:HyperLink ID="RPTListOfPaymentsInPeriodHyperLink" runat="server">Lista platilor dintr-o perioada</asp:HyperLink>     
                         <br />            
                         <asp:HyperLink ID="RPTClientPersonalReportHyperLink" runat="server">Raportul datelor personale</asp:HyperLink>      
                         <br />            
                         <asp:HyperLink ID="RPTListaCreditelorAcordateHyperLink" runat="server">Lista creditelor acordate</asp:HyperLink>    
                         <br />            
                         <asp:HyperLink ID="RPTConsulEvalHyperLink" runat="server">Consultari / Evaluari</asp:HyperLink>           
                          <br />  
                         <asp:HyperLink ID="RPTLoansPartOfCreditLinesHyperLink" runat="server">Lista tuturor creditorilor ce fac parte dintr-o linie de credit</asp:HyperLink>                                                                                                         
                    </div>
                </div>

                <div class="module" style=" width:180px; height:200px; float:left; ">
                    <div class="moduleHeader">Curs Valutar BNM</div>
                    <div class="module_content">    
                        <div style="text-align:center;color: #000; font-size: 13px;"><asp:Label ID="changeRateDateLabel" runat="server"></asp:Label></div>
                        <table style="padding: 14px 0px; color: #000; font-size: 12px;" cellpadding="0" cellspacing="0" border=" 0" align="center">
	                        <tbody>
		                        <tr>
			                        <td valign="top">
				                        <img src="App_Images/eur.png" />
			                        </td>
			                        <td style="padding-left:3px; padding-right:3px;">EUR</td>
			                        <td align="right"><asp:TextBox ID="cursEUROTextBox" runat="server" MaxLength="7" Width="50px" Style="background-color:transparent; border:none;"></asp:TextBox></td>
                                    <td style="padding-left:3px;">MDL</td>
		                        </tr>
		                        <tr>
			                        <td valign="top">
				                        <img src="App_Images/usd.png" />
			                        </td>
			                        <td style="padding-left:3px; padding-right:3px;">USD</td>
			                        <td align="right"><asp:TextBox ID="cursUSDTextBox" runat="server" MaxLength="7" Width="50px" Style="background-color:transparent; border:none;"></asp:TextBox></td>
                                    <td style="padding-left:3px;">MDL</td>
		                        </tr>
	                        </tbody>
                        </table>     
                        <div style=" margin:auto; width: 50px;"><asp:Button ID="currencyRateUpdateButton" runat="server" Text="Salvare" OnClick="currencyRateUpdateButton_Click"  visible="false"/> </div>             
                    </div>                   
                </div>

            </div>    

    </asp:Panel>

    <asp:Panel ID="emptyPanel" runat="server" Visible="False">
        <h1>Bine ati venit la DINAR-CAPITAL</h1>
    </asp:Panel>

    <asp:Panel ID="loginPanel" runat="server" Visible="False" >
        <div class="login_box"  >
            <div class="form"> 
                <fieldset class="login">
			        <legend>Login</legend>
			        <p class="notice">Login to complete your purchase.</p>
			        <p>
				        <label>Username: </label>
				        <asp:TextBox ID="userNameTextBox" runat="server"></asp:TextBox>
			        </p>
			        <p>
				        <label>Password: </label>
				        <asp:TextBox ID="passwordTextBox" runat="server" TextMode="Password"></asp:TextBox>
			        </p>
                    <p>
			            <asp:Button ID="login_Ok_Button" runat="server"  CssClass="loginBox_Button" Text="Ok" onclick="login_Ok_Button_Click" />
                    </p>
		        </fieldset>
            </div>
        </div>
    </asp:Panel>

</asp:Content>

