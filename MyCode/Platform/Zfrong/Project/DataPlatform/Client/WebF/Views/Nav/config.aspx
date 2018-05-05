<%@ Page Language="C#" MasterPageFile="~/Views/Shared/BootStrap.Master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div class="row-fluid">
        <div class="span3">
          <div class="well sidebar-nav">
            <ul class="nav nav-list">
              <li class="nav-header">我方设置</li>
              <li><a href="#" onclick="openurl('/CompanyGroup/list?Adversary=false')"><i class="icon-cog"></i>集团</a></li>
              <li><a href="#" onclick="openurl('/Brand/list?Adversary=false')"><i class="icon-pencil"></i>品牌</a></li>
              <li><a href="#" onclick="openurl('/Product/list?Adversary=false')"><i class="icon-wrench"></i>产品</a></li>
              <li><a href="#" onclick="openurl('/SearchTerm/list?Adversary=false')"><i class="icon-list"></i>关键字</a></li>
              <li class="nav-header">对手设置</li>
              <li><a href="#" onclick="openurl('/CompanyGroup/list?Adversary=true')"><i class="icon-cog"></i>集团</a></li>
              <li><a href="#" onclick="openurl('/Brand/list?Adversary=true')"><i class="icon-pencil"></i>品牌</a></li>
              <li><a href="#" onclick="openurl('/Product/list?Adversary=true')"><i class="icon-wrench"></i>产品</a></li>
              <li><a href="#" onclick="openurl('/SearchTerm/list?Adversary=true')"><i class="icon-list"></i>关键字</a></li>
              
			  
            </ul>
          </div><!--/.well -->
        </div><!--/span3-->
        
       <div class="span9" style="border:1px">
	   
		<iframe id="maincc" src="" frameborder="0" scrolling="auto" style="width:100%;height:640px"></iframe>
		 </div><!--/span9-->
      </div><!--/row-->

</asp:Content>