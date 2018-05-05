<%@ Page Language="C#" MasterPageFile="~/Views/Shared/BootStrap.Master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <div class="row-fluid">
        <div class="span3">
          <div class="well sidebar-nav">
            <ul class="nav nav-list">
               
               <li class="nav-header">数据分析</li>
			   <li><a href="#" onclick="openurl('/Statistics/list')"><i class="icon-cog"></i>网站分布</a></li>
			   <li><a href="#"><i class="icon-cog"></i>语义调性</a></li>
              <li><a href="#"><i class="icon-cog"></i>媒体分布</a></li>
			  <li><a href="#"><i class="icon-cog"></i>网站分布</a></li>
			  <li><a href="#"><i class="icon-cog"></i>时段分布</a></li>
              </ul>
          </div><!--/.well -->
        </div><!--/span3-->
        
       <div class="span9" style="border:1px">
	   
		<iframe id="maincc" src="" frameborder="0" scrolling="auto" style="width:100%;height:640px"></iframe>
		 </div><!--/span9-->
      </div><!--/row-->

</asp:Content>