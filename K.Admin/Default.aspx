<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" Runat="Server">    
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" Runat="Server">

    <asp:Panel ID="emptyPanel" runat="server" Visible="False">
        <h1>Bine ati venit la DINAR-CAPITAL</h1>
    </asp:Panel>

    <asp:Panel ID="loginPanel" runat="server" Visible="true" >
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

