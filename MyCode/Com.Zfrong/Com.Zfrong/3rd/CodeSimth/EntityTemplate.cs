//---------------------------------------------------------------------------------------
// Copyright Notice
// This file contains proprietary information of The Austin Conner Group.
// Copying or reproduction without prior written approval is prohibited.
// Copyright (C) 2004 The Austin Conner Group. All rights reserved.
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met: 
// 
// * Redistributions of source code must retain the above copyright notice, this list 
//   of conditions and the following disclaimer. 
//
// * Redistributions in binary form must reproduce the above copyright notice, this list 
//   of conditions and the following disclaimer in the documentation and/or other materials 
//   provided with the distribution. 
//
// * Neither the name of The Austin Conner Group nor the names of its contributors may be 
//   used to endorse or promote products derived from this software without specific prior 
//   written permission. 
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE 
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE 
// OR OTHER DEALINGS IN THE SOFTWARE.
//---------------------------------------------------------------------------------------
// History
//    11-05-2004  Net Source Pro - J.R. Hull  - Original Version
//---------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.ComponentModel;
using CodeSmith.Engine;
using System.Collections.Generic;
namespace Com.Zfrong.CommonLib.CodeSimth.Templates
{
	/// <summary>
	/// Summary description for BaseTemplate.
	/// </summary>
	public class EntityTemplate : CodeTemplate
	{
		#region Private Properties
        private string rootNamespace = "Com.Zfrong";
		private string developerName = "‘¯∑±»Ÿ";
		private string companyName = "ZFRONG.COM";

        private string outputDirectory =Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        private string entityAssemblyDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
       
        #endregion

		#region Public Properties
		/// <summary>
		/// The namespace that the generated classes will be a member of.
		/// </summary>
		[Optional,
		Category("1. Common"), 
		Description("Required - The root namespace that the generated classes will be a member of.")]
		public string RootNamespace
		{
			get
			{
				return this.rootNamespace;
			}
			set
			{
				this.rootNamespace = value;
			}
		}

		/// <summary>
		/// The Developer's Name that the generated code
		/// </summary>
		[Optional,
		Category("1. Common"), 
		Description("Optional - The Developer's Name that the generated code.")]
		public string DeveloperName
		{
			get
			{
				return this.developerName;
			}
			set
			{
				this.developerName = value;
			}
		}

		/// <summary>
		/// The Developement Company's Name.
		/// </summary>
		[Optional,
		Category("1. Common"), 
		Description("Optional - The Developement Company's Name.")]
		public string CompanyName
		{
			get
			{
				return this.companyName;
			}
			set
			{
				this.companyName = value;
			}
		}

        /// <summary>
        /// The Base Directory for Code Output.
        /// </summary>
        [Optional,
        Editor(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor)),
        Category("3.Output"),
        Description("Required - The Base Directory for Code Output.")]
        public string OutputDirectory
        {
            get
            {
                return outputDirectory;
            }
            set
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(value);
                }
                outputDirectory = value;
            }
        }
        [Optional,
        Editor(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor)),
        Category("2.Input"),
        Description("Required - The Base Directory for Code Output.")]
        public string EntityAssemblyDirectory
        {
            get
            {
                return entityAssemblyDirectory;
            }
            set
            {
                entityAssemblyDirectory = value;
            }
        }
		#endregion 

		#region Constructor(s)
		/// <summary>
		/// Default Constructor for BaseTemplate.
		/// </summary>
		public EntityTemplate()
		{
		}
		#endregion

		#region Public Methods
        public virtual string Build()
        {
          // string d = this.RemoveInvalidPathChars(string.Format("{0}{1}", this.OutputDirectory,""));
           //string s = this.RemoveInvalidFileChars(string.Format("{0}{1}.cs", this.TypeSchema.Name, ""));
           //this.RenderToFile(this.OutputDirectory,DateTime.Now.ToString("HHmmss fff"), this);
           return "OK";
        }
        public virtual IList<TypeSchema> GetSchemas(string dir)
        {
            IList<TypeSchema> Schemas=new List<TypeSchema>();
            string[] files = Directory.GetFiles(dir, "*.dll");
            IList<TypeSchema> objs;
            foreach (string file in files)
            {
                Show("º”‘ÿAssembly:" + file);
                objs = TypeSchema.Create(file);
                foreach (TypeSchema obj in objs)
                {
                    Show("\tº”‘ÿType:" + obj.FullName);
                    Schemas.Add(obj);//
                }

            }
            return Schemas;//
        }
        public virtual void Show(string str)
        {
            Response.WriteLine(str);//
        }
        public virtual void RenderToFile(string dir,string file, CodeTemplate template)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string fileName = Path.Combine(dir, "\\" + file);
            template.RenderToFile(fileName, true);
        }
        #endregion

        #region Private Methods
        protected string RemoveInvalidPathChars(string str)
         {
             Char[] cs = Path.GetInvalidPathChars();
            foreach(Char c in cs)
              str=str.Replace(c,'_');
             return str;
         }
        protected string RemoveInvalidFileChars(string str)
        {
            Char[] cs = Path.GetInvalidFileNameChars();
            foreach (Char c in cs)
                str = str.Replace(c, '_');
            return str;
        }
        #endregion

    }

}

