<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AccessDenied.aspx.cs" Inherits="AccessDenied" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<h2>Security restrictions applied</h2>
<h3>
It seems that the security restrictions for Your role do not allow accesss to the requested resourse.
Click back to return to the page You came from (or You were redirected from).

Alternatively pick one of allowed menu items.
</h3>
<img src="<% Response.Write(Utils.GetApplicationPath(Request)); %>/images/navigate-back.png" class="roundedButtonLong" onclick="window.history.back();" title="Go back" alt="Go back" /> 
</asp:Content>

