<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Ext.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div style="display:block;">
<style>
    .breadcrumb
    {list-style: none;box-shadow: inset 0 1px 0 white;border-radius: 3px;background-repeat: repeat-x;
border: 1px solid #DDD;background-color: #F4f4f4;padding: 7px 14px;
margin: 0 0 8px;
        
        }
.breadcrumb li {
display: inline-block;
text-shadow: 0 1px 0 white;
}
.breadcrumb a {
color: #08C;text-decoration: none;}
</style>
<div>
    <ul class="breadcrumb">
    <li>设置中心</li>->
    <li>统计</li>->
    <li>列表</li>
    </ul>
</div>
	<div id="maincontainer" style="width:auto;display:block;"></div> 
</div>
<script type="text/javascript" src="/myjs/Statistics/StatisticsCommon.js"></script>
<script type="text/javascript" src="/myjs/Statistics/view/StatisticsList.js"></script>

<script type="text/javascript">
        Ext.onReady(function () {
            Ext.create('Pro.View.StatisticsList', { renderTo: Ext.getBody() });
        });
    </script>   
</asp:Content>
