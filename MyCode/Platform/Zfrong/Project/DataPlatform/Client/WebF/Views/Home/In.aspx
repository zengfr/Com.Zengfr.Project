<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Ext.Master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <script type="text/javascript" src="<%= Request.ApplicationPath%>MyJS/login.js"></script>
     <title>主界面</title>
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
     <script type="text/javascript">
         Ext.onReady(function () {
         });
    </script>
</asp:Content>
