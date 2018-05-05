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
using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.ComponentModel;
using CodeSmith.Engine;
using System.Collections.Generic;
using System.Reflection;
namespace Com.Zfrong.CommonLib.CodeSimth.Templates
{
	/// <summary>
	/// Summary description for Builder.
	/// </summary>
	public class TemplatesBuilder :EntityTemplate
	{
       
		#region Private Properties
		protected IList<CodeTemplate> Templates = new List<CodeTemplate>();
        protected IList<TypeSchema> Schemas = new List<TypeSchema>();
        
		#endregion

		#region Constructor(s)

        public TemplatesBuilder()
		{
		}

		#endregion
		
		#region Public Methods

		/// <summary>
		/// 
		/// </summary>
        public override string Build()
        {
            Schemas.Clear();
            Schemas = GetSchemas(this.EntityAssemblyDirectory);

            Templates.Clear();//
            string[] files = Directory.GetFiles(CodeTemplateInfo.DirectoryName, "*.cst");
            foreach (string file in files)
            {
                if (file!=this.CodeTemplateInfo.FullPath)
                {
                    Show("¼ÓÔØTemplate:" + file);
                    Templates.Add(CompileTemplate(file));
                }
            }
            foreach (EntityTemplate Template in Templates)
            {
                foreach (TypeSchema schema in Schemas)
                {
                    Template.SetProperty("CompanyName", this.CompanyName);
                    Template.SetProperty("DeveloperName", this.DeveloperName);
                    Template.SetProperty("RootNamespace", this.RootNamespace);
                    Template.SetProperty("OutputDirectory", this.OutputDirectory);
                    Template.SetProperty("EntityAssemblyDirectory", this.EntityAssemblyDirectory);
                    Template.SetProperty("TypeSchema", schema);//
                    Template.SetProperty("TypeSchemas", this.Schemas);//
                    Template.SetProperty("Templates", this.Templates);//
                    Show("Éú³É:\t"+Template.Build());//
                }

            }
            return "OK";
        }

		#endregion

		#region Private Methods

      
		/// <summary>
		/// 
		/// </summary>
		/// <param name="templateName"></param>
		/// <returns></returns>
		private CodeTemplate CompileTemplate(string templateName)
		{
			CodeTemplateCompiler compiler = new CodeTemplateCompiler(templateName);
			compiler.Compile();
			
			if (compiler.Errors.Count == 0)
			{
				return compiler.CreateInstance();
			}
			else
			{
				for (int i = 0; i < compiler.Errors.Count; i++)
				{
					Show("´íÎó:"+compiler.Errors[i].ToString());
				}			
				return null;
			}
			
		}

		#endregion
	}

}
