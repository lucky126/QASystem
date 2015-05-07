using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace ChinaHCM.QA.Component.Tools
{
    public class PageLoger
    {
        #region 常量

        private const string c_EventLogName = "WebApplicaion";
        private static string c_EventSource = "SS_ISS_Net_HCM";

        #endregion

        #region 私有变量

        private static string m_FileName;

        /// <summary>
        /// 锁定系统操作的一个辅助对象
        /// </summary>
        private static object logHelper = new object();

        #endregion

        #region 属性
        /// <summary>
        /// 日志文件的默认文件名
        /// 文件全路径名
        /// </summary>
        public static string DebugFileName
        {
            get
            {
                return m_FileName;
            }
            set
            {
                m_FileName = value;
            }
        }

        #endregion

        #region 记录Log
        /// <summary>
        /// 使用默认文件名记录Log
        /// </summary>
        /// <param name="strMessage">需要记录的信息</param>
        /// <param name="EventLogEnabled">是否记录到系统日志</param>
        public static void WriteLog(string strMessage, bool EventLogEnabled)
        {
            try
            {
                lock (logHelper)
                {
                    if (EventLogEnabled)
                    {
                        try
                        {
                            // 检查事件源是否已经注册，没有，则注册，
                            // 这样就可以把错误信息记录到Windows的“应用程序日志”中了
                            if (!EventLog.SourceExists(c_EventSource))
                            {
                                EventLog.CreateEventSource(c_EventSource, c_EventLogName);
                            }
                            // write the message as an error
                            using (EventLog msg = new EventLog(c_EventLogName))
                            {
                                msg.Source = c_EventSource;
                                msg.WriteEntry(strMessage, EventLogEntryType.Error);
                            }
                        }
                        catch
                        { }
                    }

                    //如果文件名不为空，记录之
                    if (!string.IsNullOrEmpty(m_FileName))
                    {
                        FileInfo file = new FileInfo(m_FileName);
                        using (StreamWriter debugWriter = new StreamWriter(
                            file.Open(FileMode.Append, FileAccess.Write, FileShare.Read)))
                        {
                            debugWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                                "\t" + strMessage);
                        }
                    }
                }
            }
            catch
            {

            } //忽略错误
        }

        /// <summary>
        /// 使用默认文件名记录Log
        /// 此方法不记录系统日志
        /// </summary>
        /// <param name="strMessage">需要记录的信息</param>
        public static void WriteLog(string strMessage)
        {
            WriteLog(strMessage, false);
        }

        /// <summary>
        /// 记录Log
        /// </summary>
        /// <param name="strLogFileFullPath">log文件全路径</param>
        /// <param name="strMessage">需要记录的信息</param>
        /// <param name="EventLogEnabled">是否记录到系统日志</param>
        public static void WriteLog(string strLogFileFullPath, string strMessage,
            bool EventLogEnabled)
        {
            try
            {
                lock (logHelper)
                {
                    if (EventLogEnabled)
                    {
                        try
                        {
                            // 检查事件源是否已经注册，没有，则注册，
                            // 这样就可以把错误信息记录到Windows的“应用程序日志”中了
                            if (!EventLog.SourceExists(c_EventSource))
                            {
                                EventLog.CreateEventSource(c_EventSource, c_EventLogName);
                            }
                            // write the message as an error
                            using (EventLog msg = new EventLog(c_EventLogName))
                            {
                                msg.Source = c_EventSource;
                                msg.WriteEntry(strMessage, EventLogEntryType.Error);
                            }
                        }
                        catch
                        { }
                    }

                    //如果文件名不为空，记录之
                    if (!string.IsNullOrEmpty(strLogFileFullPath))
                    {
                        FileInfo file = new FileInfo(strLogFileFullPath);
                        using (StreamWriter sWriter = file.AppendText())
                        {
                            sWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                                "\t" + strMessage);

                            sWriter.Flush();

                            sWriter.Close();
                        }
                    }
                }
            }
            catch
            {
            } //忽略错误
        }

        /// <summary>
        /// 记录Log
        /// 此方法不记录系统日志
        /// </summary>
        /// <param name="strLogFileFullPath">log文件全路径</param>
        /// <param name="strMessage">需要记录的信息</param>
        public static void WriteLog(string strLogFileFullPath, string strMessage)
        {
            WriteLog(strLogFileFullPath, strMessage, false);
        }
        #endregion
    }
}
