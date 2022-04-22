using Common.Tools;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace Common.Models
{
    /// <summary>
    /// 메시지 레벨 열거형.
    /// </summary>
    public class MessageLevel
    {
        #region Enums

        /// <summary>
        /// 알림 메시지.
        /// </summary>
        public static MessageLevel Info
        {
            get
            {
                return new MessageLevel()
                {
                    NLogLevel = LogLevel.Info,
                    Name = "알림",
                    Color = Brushes.Black
                };
            }
        }

        /// <summary>
        /// 한국어.
        /// </summary>
        public static MessageLevel Error
        {
            get
            {
                return new MessageLevel()
                {
                    NLogLevel = LogLevel.Error,
                    Name = "에러",
                    Color = Brushes.Red
                };
            }
        }

        /// <summary>
        /// 전체 메시지 레벨.
        /// </summary>
        public static List<MessageLevel> All
        {
            get
            {
                return ReflectionTool.GetReadOnlyProperties<MessageLevel>(MethodBase.GetCurrentMethod().Name).ToList();
            }
        }

        #endregion

        #region Constructors

        private MessageLevel()
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// 언어 키워드.
        /// </summary>
        public LogLevel NLogLevel
        {
            get; private set;
        }

        /// <summary>
        /// 언어 이름.
        /// </summary>
        public string Name
        {
            get; private set;
        }

        public Brush Color
        {
            get; private set;
        }

        #endregion

        #region Override methods

        public static bool operator ==(MessageLevel left, MessageLevel right)
        {
            object leftObject = left as object;
            object rightObject = right as object;

            if (leftObject == null && rightObject == null)
            {
                return true;
            }
            else if (leftObject == null || rightObject == null)
            {
                return false;
            }

            return leftObject.Equals(rightObject);
        }

        public static bool operator !=(MessageLevel left, MessageLevel right)
        {
            return (left == right) == false;
        }

        public override bool Equals(object obj)
        {
            if (obj is MessageLevel == false)
            {
                return false;
            }

            return NLogLevel.Equals(((MessageLevel)obj).NLogLevel);
        }

        public override int GetHashCode()
        {
            return NLogLevel.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
