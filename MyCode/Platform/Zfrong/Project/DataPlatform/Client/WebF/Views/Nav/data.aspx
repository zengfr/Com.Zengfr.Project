<%@ Page Language="C#" MasterPageFile="~/Views/Shared/BootStrap.Master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <div class="row-fluid">
        <div class="span3">
          <div class="well sidebar-nav">
            <ul class="nav nav-list">
               
               <li class="nav-header">数据中心</li>
			   <li><a href="#" onclick="openurl('/DataItem/list')"><i class="icon-cog"></i>今日数据</a></li>
			   <li><a href="#"><i class="icon-cog"></i>昨日数据</a></li>
			   <li><a href="#"><i class="icon-cog"></i>数据检索</a></li>
            <li><a href="#"><i class="icon-cog"></i>调性设置</a></li>
			   <li class="nav-header">数据过滤</li>
			  <li><a href="#" onclick="openurl('/DataFilter/list')"><i class="icon-cog"></i>标题过滤</a></li>
			   <li><a href="#"><i class="icon-cog"></i>内容过滤</a></li>
				 <li><a href="#"><i class="icon-cog"></i>URL过滤</a></li>
              </ul>
          </div><!--/.well -->
        </div><!--/span3-->
        
       <div class="span9" style="border:1px">
	   
		<iframe id="maincc" src="" frameborder="0" scrolling="auto" style="width:100%;height:640px"></iframe>
		 </div><!--/span9-->
      </div><!--/row-->

</asp:Content>