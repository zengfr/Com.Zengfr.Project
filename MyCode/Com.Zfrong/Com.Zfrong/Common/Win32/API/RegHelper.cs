using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Management;
using System.Runtime.InteropServices;
using Com.Zfrong.Common.Security;
namespace Com.Zfrong.Common.Win32.API
{
   public class RegHelper
    {
        private static string GetCPUID()
        {
            string cpuInfo = "";//cpu序列号
            try
            {
                ManagementClass cimobject = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = cimobject.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                    return cpuInfo;
                }
            }
            catch { }
            return "";
        }
        private static string GetHDID()
        {
            //获取硬盘ID
            string HDid = "";
            try
            {
                ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc1 = cimobject1.GetInstances();
                foreach (ManagementObject mo in moc1)
                {
                    HDid = (string)mo.Properties["Model"].Value;
                    return HDid;
                }
                cimobject1 = null; moc1 = null;
            }
            catch { }
            return HDid.Trim();
        }
        private static string GetMACID()
        {
            //获取网卡硬件地址
            string s = "";
            try
            {
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc2 = mc.GetInstances();
                foreach (ManagementObject mo in moc2)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        s = mo["MacAddress"].ToString();
                        mo.Dispose();
                        return s;
                    }
                }
                mc = null; moc2 = null;
            }
            catch { }
            return s;
        }
        //获取主板序列号
        public static string GetBIOSID()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select   SerialNumber   From   Win32_BIOS ");
            string biosNumber = "";
            foreach (ManagementObject mgt in searcher.Get())
            {
                if (mgt["SerialNumber"] != null)
                    biosNumber = mgt["SerialNumber"].ToString();
            }
            searcher = null;
            return biosNumber.Trim();
        }
        public static string SNKey
        {
            get
            {
                return "8" + GetCPUID() + GetHDID() + GetMACID() + GetBIOSID() + "8";//
            }
        }
        public  string GetSNKey()
        {
            return ds.DesEncrypt(SNKey);
        }
        static DesSecurity ds = new DesSecurity();
        static RegistryKey hkml = Registry.LocalMachine;
        static RegistryKey software = hkml.OpenSubKey("S" + "O" + "F" + "T" + "W" + "A" + "R" + "E", true);
        static string RegSubKey = "G" + "n" + "o" + "r" + "f" + "z";
       
        static string GetRegistData(string name)
        {
            string registData = "";
            RegistryKey aimdir = software.OpenSubKey(RegSubKey, false);
            if (aimdir != null)
            {
                object v = aimdir.GetValue(name);
                if (v != null)
                    registData = v.ToString();
                aimdir.Close(); aimdir = null;
            }
            return registData;
        }
        
        static void SetRegistData(string name, string tovalue)
        {
            RegistryKey aimdir = software.CreateSubKey(RegSubKey);
            aimdir.SetValue(name, tovalue);
            aimdir.Close(); aimdir = null;
        }

        string name = "123";
        public RegHelper(string name)
        {
            this.name = name;
        }
        public string GetRegistData()
        {
            return GetRegistData(name);
        }
        public void SetRegistData(string tovalue)
        {
            SetRegistData(name, tovalue);
        }
        bool ISOK = false;
        int OKCount = 1;
        public  bool IsOK()
        {
            if (!ISOK && OKCount > 1) return ISOK;//
            if (ISOK && OKCount > 16) return ISOK;//
            DateTime t = Com.Zfrong.Common.Win32.API.Time.GetTimeForSys();
            SymmetricMethod sm = new SymmetricMethod();
            DesSecurity ds = new DesSecurity();
            string a = ds.DesEncrypt(sm.Encrypto(SNKey));
            string b = sm.Decrypto(ds.DesDecrypt(GetRegistData()));
            ds = null;
            sm = null;
            if (b != null && b.IndexOf(Split) > 5)
            {
                string c = b.Substring(b.IndexOf(Split) + 1);
                string d = b.Substring(0, b.IndexOf(Split));
                try
                {
                    DateTime.TryParse(d, out t);
                    //t = DateTime.ParseExact(d, "yyyy-MM-dd HH:mm:ss",
                    //    new System.Globalization.CultureInfo("zh-CN"),
                    //    System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                }
                catch { return 2 == 3; }
                ISOK = (c == SNKey) && (t > Com.Zfrong.Common.Win32.API.Time.GetTimeForSys());
                OKCount++;
                return ISOK;
            }
            return 1 == 0;//
        }
        public  string GetOK(string str, int d)
        {
            DesSecurity ds = new DesSecurity();
            SymmetricMethod sm = new SymmetricMethod();
            string dsk = ds.DesDecrypt(str);
            dsk = DateTime.Now.AddDays(d).ToString() + Split + dsk;
            return ds.DesEncrypt(sm.Encrypto(dsk));
        }
        public static string Split = "|";
    }
}
