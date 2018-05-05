using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using Zfrong.Framework.Core.Model.Base;
namespace GeneratedCode.Model{
	public class products:ModelBase
	{
		public int id {get;set;}
		public string name {get;set;}
		public int? stock {get;set;}
		public int? price {get;set;}
		public bool? enabled {get;set;}
		public int? categoryid {get;set;}
	}
	}
