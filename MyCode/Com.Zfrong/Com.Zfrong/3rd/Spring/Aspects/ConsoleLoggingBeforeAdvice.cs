#region License

/*
 * Copyright ?2002-2006 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

#region Imports

using System;
using System.Reflection;

using Spring.Aop;

#endregion

namespace Com.Zfrong.Common.Spring.Aspects
{
    /// <summary>
    /// Simple implementation of the <see cref="Spring.Aop.IMethodBeforeAdvice"/> interface 
    /// for a logging aspect using <see cref="System.Console"/>.
    /// </summary>
    /// <author>Rick Evans</author>
    /// <version>$Id: ConsoleLoggingBeforeAdvice.cs,v 1.1 2006/11/26 18:57:59 bbaia Exp $</version>
	public class ConsoleLoggingBeforeAdvice : IMethodBeforeAdvice
	{
		public void Before(MethodInfo method, object[] args, object target)
		{
            Console.Out.WriteLine("Before".PadRight(80, '-'));
			Console.Out.WriteLine("Target:" + target);
            Console.Out.WriteLine("Method:" + method.Name);
            Console.Out.WriteLine("Args:");
			if(args != null)
			{
				foreach (object arg in args)
				{
					Console.Out.WriteLine("\t:" + arg);
				}
			}
		}
	}
}