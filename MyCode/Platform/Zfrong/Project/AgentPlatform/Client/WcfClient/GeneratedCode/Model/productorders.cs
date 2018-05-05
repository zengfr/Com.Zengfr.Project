using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using Zfrong.Framework.Core.Model.Base;
namespace GeneratedCode.Model{
	public class productorders:ModelBase
	{
		public int id {get;set;}
		public int? productid {get;set;}
		public int? quantity {get;set;}
		public int? price {get;set;}
		public int? totalprice {get;set;}
		public DateTime? creattime {get;set;}
		public int? accountid {get;set;}
		public bool enabled {get;set;}
		public bool deleted {get;set;}
	}
	}
