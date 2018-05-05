function include_abc(path)
{     
      var sobj = document.createElement('script');
      sobj.type = "text/javascript";
      sobj.src = path;
	  sobj.charset="gb2312";
      var headobj = document.getElementsByTagName('head')[0];
      headobj.appendChild(sobj);
}
//根据已经包含的第一个js文件路径，包含新的js文件
function include(path)
{
      var scripts = document.getElementsByTagName("script");
      if(!scripts) return;
      var jsPath = scripts[0].src;      
      jsPath=jsPath.substring(0,jsPath.lastIndexOf('/')+1);
      var sobj = document.createElement('script');
      sobj.type = "text/javascript";
      sobj.src = jsPath+path;
	  //sobj.charset="gb2312";
      var headobj = document.getElementsByTagName('head')[0];
      headobj.appendChild(sobj);
}
function AddScriptTo(id,path)
{
	var sobj = document.createElement('script');
      sobj.type = "text/javascript";
      sobj.src = path;
	  //sobj.charset="gb2312";
      var obj = document.getElementsById(id);
      obj.appendChild(sobj);
}
function AddBoxTo(id,str)
{
	var obj1 = document.createElement('div');
    obj1.className = "t3 bcb bgb";

	var obj2 = document.createElement('div');
    obj2.className = "b3 bcb mb6";
	obj2.innerHTML=str;//
	
	  var obj = document.getElementById(id);
      obj.appendChild(obj1);
	  obj.appendChild(obj2);
	
}