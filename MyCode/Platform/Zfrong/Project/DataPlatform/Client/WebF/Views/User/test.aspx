<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Ext.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
 <script type="text/javascript" src="<%= Request.ApplicationPath%>MyJS/User/base/user.js"></script>
 <script type="text/javascript" src="<%= Request.ApplicationPath%>MyJS/User/view/usertest.js"></script>           
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
        <script type="text/javascript">
            Ext.onReady(function () {
                Ext.create('Pro.View.UserTest', { renderTo: Ext.getBody() });
            });
    </script>
</asp:Content>
