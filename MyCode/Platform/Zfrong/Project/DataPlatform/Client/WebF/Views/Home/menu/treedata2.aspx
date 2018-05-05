<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage"%><%=Request.QueryString.Get("callback")%>([
{"id":"21","text":"基本设置","cls":"forum-ct","iconCls":"forum-parent","expanded":true,
"children":[
{"id":"211","text":"集团管理","cls":"forum","iconCls":"icon-forum","url":"#","leaf":true},
{"id":"212","text":"品牌管理","cls":"forum","iconCls":"icon-forum","url":"#","leaf":true},
{"id":"213","text":"产品管理","cls":"forum","iconCls":"icon-forum","url":"#","leaf":true}
]},
{"id":"23","text":"语法管理","cls":"forum-ct","iconCls":"forum-parent","expanded":true,
"children":[
{"id":"231","text":"语法列表","cls":"forum","iconCls":"icon-forum","url":"#","leaf":true}
]},
{"id":"22","text":"检索设置","cls":"forum-ct","iconCls":"forum-parent","expanded":true,
"children":[
{"id":"221","text":"分析器管理","cls":"forum","iconCls":"icon-forum","url":"#","leaf":true},
{"id":"222","text":"词典管理","cls":"forum","iconCls":"icon-forum","url":"#","leaf":true},
{"id":"223","text":"过滤管理","cls":"forum","iconCls":"icon-forum","url":"#","leaf":true}
]},

{"id":"24","text":"数据管理","cls":"forum-ct","iconCls":"forum-parent","expanded":true,
"children":[
{"id":"241","text":"数据列表","cls":"forum","iconCls":"icon-forum","url":"#","leaf":true}
]}
]);
<%Response.ContentType = "text/html";Response.HeaderEncoding=Encoding.UTF8;%>