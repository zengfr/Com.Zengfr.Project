using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ScrcpyNet.Ex
{
    public class StepRecorder
    {
        public static StepRecorder Instance = new StepRecorder();


        public IList<ScriptItem> Scripts { get; set; } = new List<ScriptItem>();
        private bool isPlay = false;
        public void Clear()
        {
            Scripts.Clear();
        }

        public void Record(IControlMessage msg) {
            if(!isPlay)
            Scripts.Add(buildItem(msg));
        }

        internal void Play(Scrcpy scrcpy)
        {
            isPlay = true;
            foreach (ScriptItem item in Scripts) {
                IControlMessage msg;
                switch (item.name)
                {
                    case "":
                        msg = null;
                        break;
                    default:
                        msg = JsonConvert.DeserializeObject(item.json, Type.GetType(item.name)) as IControlMessage;
                        break;
                }
                if (msg != null)
                {
                    Thread.Sleep(1000);
                    scrcpy.SendControlCommand(msg);
                }
            }
            isPlay = false;
        }

        private ScriptItem buildItem(IControlMessage msg)
        {
            ScriptItem item = new ScriptItem();
            item.name = msg.GetType().FullName;
            item.json = JsonConvert.SerializeObject(msg);
            return item;
        }
    }

    public class ScriptItem
    {
        public string name { get; set; } = "";
        public string json { get; set; } = "";
    }
}
