<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Ext.Master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<style type="text/css">
    #loading-mask{
        background-color:white;
        height:100%;
        position:absolute;
        left:0;
        top:0;
        width:100%;
        z-index:20000;
    }
    #loading{
        height:auto;
        position:absolute;
        left:45%;
        top:40%;
        padding:2px;
        z-index:20001;
    }
    #loading a {
        color:#225588;
    }
    #loading .loading-indicator{
        background:white;
        color:#444;
        font:bold 13px Helvetica, Arial, sans-serif;
        height:auto;
        margin:0;
        padding:10px;
    }
    #loading-msg {
        font-size: 10px;
        font-weight: normal;
    }
}
</style>
<title>主界面</title>
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
<div id="loading-mask" style=""></div>
<div id="loading">
        <div class="loading-indicator">
        <img src="http://127.0.0.1/img/loading.gif" width="32" height="32" 
        style="margin-right:8px;float:left;vertical-align:top;"/><span>通用综合管理系统平台</span>
        <br /><span id="loading-msg">Loading styles and images...</span></div>
    </div>
<div id="west" class="x-hide-display"></div>
<div id="center2" class="x-hide-display"></div>
<div id="center1" class="x-hide-display"></div>
<div id="props-panel" class="x-hide-display"></div>
<div id="south" class="x-hide-display"></div>
<script type="text/javascript">document.getElementById('loading-msg').innerHTML = 'Loading Core API...';</script>
<script type="text/javascript">document.getElementById('loading-msg').innerHTML = 'Loading UI Components...';</script>
<script type="text/javascript" src="<%= Request.ApplicationPath%>myjs/Home/TreeList.js"></script>
<script type="text/javascript" src="<%= Request.ApplicationPath%>myjs/Home/Main.js"></script>
<script type="text/javascript">
     Ext.onReady(function () {
        Ext.QuickTips.init();
        var firebugWarning = function () { };
        var hideMask = function () {
            Ext.get('loading').remove();
            Ext.fly('loading-mask').animate({
                opacity: 0,
                remove: true,easing: 'bounceOut',
                callback: firebugWarning
            });
        };
        new App.Main();
        Ext.defer(hideMask, 250);
    });
</script>
<script type="text/javascript"> document.getElementById('loading-msg').innerHTML = 'Initializing...';</script>
</asp:Content>