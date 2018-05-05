<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Ext.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
 <script type="text/javascript" src="<%= Request.ApplicationPath%>MyJS/User/usercommon.js"></script>
 <script type="text/javascript" src="<%= Request.ApplicationPath%>MyJS/User/view/userlist.js"></script>           
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
        <script type="text/javascript">
        Ext.onReady(function () {
            Ext.create('Pro.View.UserList', {renderTo: Ext.getBody()});
        });
    </script>
</asp:Content>
