using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;

//TestSkin命令空间，别忘了改成你自己的。
namespace Com.Zfrong.Common.DotnetSkin
{
    public class DotnetSkinClass
    {
        public static DotNetSkin.SkinUI se = null;
        const string DefaultName = "Default默认";
        const string SkinFile = "skin.xml";
        //const string ResourceNamespace = "WindowsApplication1.dotnetskin.";
        ///   <summary>
        ///   增加换肤菜单
        ///   </summary>
        ///   <param   name= "toolMenu "> </param>
        public static void AddSkinMenu(ToolStripMenuItem toolMenu)
        {
            DataSet skin = new DataSet();
            try
            {
                skin.ReadXml(SkinFile, XmlReadMode.Auto);
            }
            catch
            {

            }
            if (skin == null || skin.Tables.Count < 1)
            {
                SaveSkin(DefaultName);
            }
           // toolMenu.DropDownItems.Add(new ToolStripMenuItem(DefaultName));
            //toolMenu.DropDownItems[toolMenu.DropDownItems.Count - 1].Click += new EventHandler(frm_Main_Click);
            if (skin.Tables[0].Rows[0][0].ToString() == DefaultName)
            {
                ((ToolStripMenuItem)toolMenu.DropDownItems[toolMenu.DropDownItems.Count - 1]).Checked = true;
            } 
            foreach (DotnetSkinType st in (DotnetSkinType[])System.Enum.GetValues(typeof(DotnetSkinType)))
            {
                toolMenu.DropDownItems.Add(new ToolStripMenuItem(st.ToString()));
                toolMenu.DropDownItems[toolMenu.DropDownItems.Count - 1].Click += new EventHandler(frm_Main_Click);

                if (st.ToString() == skin.Tables[0].Rows[0][0].ToString())
                {
                    ((ToolStripMenuItem)toolMenu.DropDownItems[toolMenu.DropDownItems.Count - 1]).Checked = true;
                    frm_Main_Click(toolMenu.DropDownItems[toolMenu.DropDownItems.Count - 1], null);
                }
            }
            skin = null;
        }
        static void frm_Main_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < ((ToolStripMenuItem)((ToolStripMenuItem)sender).OwnerItem).DropDownItems.Count; i++)
            {
                if (((ToolStripMenuItem)sender).Text == ((ToolStripMenuItem)((ToolStripMenuItem)sender).OwnerItem).DropDownItems[i].Text)
                {
                    ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;
                    SaveSkin(((ToolStripMenuItem)((ToolStripMenuItem)sender).OwnerItem).DropDownItems[i].Text);
                }
                else
                {
                    ((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)sender).OwnerItem).DropDownItems[i]).CheckState = CheckState.Unchecked;
                }
            }
            if (((ToolStripMenuItem)sender).Text == DefaultName)
            {
                RemoveSkin();
                //SaveSkin(DefaultName);
                return;
            }
            foreach (DotnetSkinType st in (DotnetSkinType[])System.Enum.GetValues(typeof(DotnetSkinType)))
            {
                if (st.ToString() == ((ToolStripMenuItem)sender).Text)
                {
                    ChangeSkin(st);
                    return;
                }
            }
        }
        ///   <summary>
        ///   改变皮肤
        ///   </summary>
        ///   <param   name= "st "> </param>
        public static void ChangeSkin(DotnetSkinType st)
        {
            System.Reflection.Assembly thisDll = System.Reflection.Assembly.GetExecutingAssembly();
            if (se != null)
            {
                se.SkinFile = null;
                System.Resources.ResourceManager resources = new System.Resources.ResourceManager("WindowsApplication1.DotNetSkinRes", thisDll);
                object obj=resources.GetObject(st.ToString());
                if (obj != null)
                {
                     se.SkinSteam = new System.IO.MemoryStream(obj as byte[]);
                     Console.WriteLine("加载[皮肤-->完成");
                     se.Active = true;
                }
                obj = null; resources = null;
            }
            thisDll = null;
        }
        ///   <summary>
        ///   移除皮肤
        ///   </summary>
        public static void RemoveSkin()
        {
            if (se == null)
            {
                return;
            }
            else
            {
                se.Active = false;
            }
        }
        static void  SaveSkin(string skinName)
        {
            DataSet skin = new DataSet();
            skin.Tables.Add("skin");
            skin.Tables["skin"].Columns.Add("style");
            System.Data.DataRow dr = skin.Tables["skin"].NewRow();
            dr[0] = skinName;
            skin.Tables[0].Rows.Add(dr);
            skin.WriteXml(SkinFile, XmlWriteMode.IgnoreSchema); dr = null; skin = null;
        }
    }
    ///   <summary>
    ///   换肤类型
    ///   </summary>
    public enum DotnetSkinType
    {
        MacOS_TGRB2,
        MacOS_BLUE,
       XP_BLUE,
    XP_HOMESTEAD,
    XP_METALLIC,
    Office2007,
    Royale_INDIGOT,
    VISTAXPB2,
        Wmpx_XMP2
    }
} 
