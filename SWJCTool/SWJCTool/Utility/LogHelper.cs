using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SWJCTool.Utility
{
    public static class LogHelper
    {
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");  // 这里的 loginfo 和 log4net.config 里的名字要一样
        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");// 这里的 logerror 和 log4net.config 里的名字要一样
        
        public static bool IsWriteInfoLog { get; set; }
        //public static string LogStr { get; set; }


        //[UnmanagedFunctionPointer(CallingConvention.StdCall)]
        //public delegate void RecvLogEventHandler(string LogStr);//接收事件委托
        ////添加委托
        //public static RecvLogEventHandler OnRecvLogEvent;

        //public static event RecvLogEventHandler RecvLog
        //{
        //    add { OnRecvLogEvent += new RecvLogEventHandler(value); }
        //    remove { OnRecvLogEvent -= new RecvLogEventHandler(value); }
        //}

        //添加委托
        [UnmanagedFunctionPointer(CallingConvention.StdCall)] 
        public delegate void RecvLogEventHandler(string LogStr);//接收事件委托
        public static RecvLogEventHandler OnRecvScriptLogEvent;

        public static event RecvLogEventHandler RecvScriptLog
        {
            add { OnRecvScriptLogEvent += new RecvLogEventHandler(value); }
            remove { OnRecvScriptLogEvent -= new RecvLogEventHandler(value); }
        }

        public static void WriteLog(string info)
        {
            //OnRecvLogEvent(DateTime.Now + ":" + info);
            if (loginfo.IsInfoEnabled && IsWriteInfoLog)
            {
                loginfo.Info(info);
            }
        }

        public static void WriteLog(string info, Exception ex)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(info, ex);
            }
        }

        public static void WriteScriptLog(string info)
        {
            OnRecvScriptLogEvent(DateTime.Now + ":" + info);
        }
    }
}
