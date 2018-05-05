/**
 * MemcachedBench.cs
 *
 * Copyright (c) 2005
 * Tim Gebhardt <tim@gebhardtcomputing.com>
 * 
 * Based off of code written by
 * Greg Whalin <greg@meetup.com>
 * for his Java Memcached client:
 * http://www.whalin.com/memcached/
 * 
 *
 * See the memcached website:
 * http://www.danga.com/memcached/
 *
 * This module is Copyright (c) 2005 Tim Gebhardt.
 * All rights reserved.
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later
 * version.
 *
 * This library is distributed in the hope that it will be
 * useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
 * PURPOSE.  See the GNU Lesser General Public License for more
 * details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307  USA
 *
 * @author Tim Gebhardt<tim@gebhardtcomputing.com> 
 * @version 1.0
 */
namespace Com.Zfrong.Common.Memcache
{
	using System;
	using System.Collections;
	using Memcached.ClientLibrary;

	public class MemcachedBench 
	{
		/// <summary>
		/// Arguments: 
		///		arg[0] = the number of runs to do
		///		arg[1] = the run at which to start benchmarking
		/// </summary>
		/// <param name="args"></param>
        static MemcachedClient mc;
        static SockIOPool pool;
		public static void Init() 
		{
			string[] serverlist = { "202.85.216.242:11211" };

			pool = SockIOPool.GetInstance();
			pool.SetServers(serverlist);

			pool.InitConnections = 2;
			pool.MinConnections = 2;
			pool.MaxConnections = 100;

			pool.SocketConnectTimeout = 1000;
            pool.SocketTimeout = 3000;

			pool.MaintenanceSleep = 30;
			pool.Failover = true;

			pool.Nagle = false;
			pool.Initialize();
//
//			// get client instance
			 mc = new MemcachedClient();
            if (mc != null)
            {
                Stats();
                mc.EnableCompression = false;
                Com.Zfrong.Common.Win32.Console.ConsoleProgram.ShowSuccess("Memcached Successed");//
               //mc.FlushAll(); pool.Shutdown();
            }
        }
        public static void Set(string key,object obj)
        {
            if (mc != null)
            {
                mc.Set(key, obj);
                Console.WriteLine(string.Format("Memcached->Set:{0} {1}", key, obj));//
            }
            else
            {
                Com.Zfrong.Common.Win32.Console.ConsoleProgram.ShowError("Server Error Code 1001");
                Console.ReadLine();
            }
        }
        public static void Stats()
        {
            if (mc == null) return; 
            Hashtable stats = mc.Stats();  
             foreach(string key1 in stats.Keys)  
            {    Console.WriteLine("--");//+key1);  
                 Hashtable values = (Hashtable)stats[key1];  
                foreach(string key2 in values.Keys)  
                {
                    Com.Zfrong.Common.Win32.Console.ConsoleProgram.ShowInfo("  " + key2 + ":" + values[key2]);  
                }                           
             }  
        }
        public static bool KeyExists(string key)
        {
            if(mc!=null)
              return mc.KeyExists(key);//
            else
            {
                Com.Zfrong.Common.Win32.Console.ConsoleProgram.ShowError("Server Error Code 1002");
                Console.ReadLine();
            }
            return true;
        }
        public static void Shutdown()
        {
            if(pool!=null)
			  pool.Shutdown();
		}
	}
}