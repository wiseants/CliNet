using Newtonsoft.Json;
using System;

namespace Common.Models
{
    /// <summary>
    /// COM 포트 정보 모델.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class ComInfo
    {
        #region Constructors

        public ComInfo()
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// 이름.
        /// </summary>
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// 설명.
        /// </summary>
        public string Description
        {
            get; set;
        }

        #endregion

        #region Override methods

        public override string ToString()
        {
            return string.IsNullOrEmpty(Description) ? Name : string.Format("{0} - {1}", Name, Description);
        }

        public static bool operator ==(ComInfo left, ComInfo right)
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

        public static bool operator !=(ComInfo left, ComInfo right)
        {
            return left == right == false;
        }

        public override bool Equals(object obj)
        {
            return (obj is ComInfo rate) && Name.Equals(rate.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        #endregion
    }
}
