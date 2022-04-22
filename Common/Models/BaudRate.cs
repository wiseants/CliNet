using Common.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class BaudRate
    {
        #region Enums

        /// <summary>
        /// BaudRate 57600
        /// </summary>
        public static BaudRate B57600 => new BaudRate()
        {
            Name = "57600",
            Value = 57600
        };

        /// <summary>
        /// BaudRate 115200
        /// </summary>
        public static BaudRate B115200 => new BaudRate()
        {
            Name = "115200",
            Value = 115200
        };

        /// <summary>
        /// BaudRate 230400
        /// </summary>
        public static BaudRate B230400 => new BaudRate()
        {
            Name = "230400",
            Value = 230400
        };

        /// <summary>
        /// 전체.
        /// </summary>
        public static List<BaudRate> All => ReflectionTool.GetReadOnlyProperties<BaudRate>(MethodBase.GetCurrentMethod().Name).ToList();

        #endregion

        #region Constructors

        private BaudRate()
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// 언어 이름.
        /// </summary>
        public string Name
        {
            get; private set;
        }

        /// <summary>
        /// 정수형.
        /// </summary>
        public int Value
        {
            get; private set;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 키워드로 상태 변경.
        /// </summary>
        /// <param name="keyword">변경 할 키워드.</param>
        /// <returns></returns>
        public static BaudRate Convert(string name)
        {
            return string.IsNullOrEmpty(name) == true
                ? B115200
                : All.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #region Override methods

        public static bool operator ==(BaudRate left, BaudRate right)
        {
            object leftObject = left;
            object rightObject = right;

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

        public static bool operator !=(BaudRate left, BaudRate right)
        {
            return left == right == false;
        }

        public override bool Equals(object obj)
        {
            return (obj is BaudRate rate) && Name.Equals(rate.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
