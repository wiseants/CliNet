using Common.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Models
{
    /// <summary>
    /// 언어 열거형.
    /// </summary>
    public class Language
    {
        #region Enums

        /// <summary>
        /// 영어.
        /// </summary>
        public static Language English
        {
            get
            {
                return new Language()
                {
                    Keyword = "EN".ToUpper(),
                    Name = "English",
                    TokenValue = "en-US"
                };
            }
        }

        /// <summary>
        /// 한국어.
        /// </summary>
        public static Language Korean
        {
            get
            {
                return new Language()
                {
                    Keyword = "KO".ToUpper(),
                    Name = "Korean",
                    TokenValue = "ko-KR"
                };
            }
        }

        /// <summary>
        /// 전체 언어.
        /// </summary>
        public static List<Language> All
        {
            get
            {
                return ReflectionTool.GetReadOnlyProperties<Language>(MethodBase.GetCurrentMethod().Name).ToList();
            }
        }

        #endregion

        #region Constructors

        private Language()
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// 언어 키워드.
        /// </summary>
        public string Keyword
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

        public string TokenValue
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
        public static Language Convert(string keyword)
        {
            if (string.IsNullOrEmpty(keyword) == true)
            {
                return Language.Korean;
            }

            return All.FirstOrDefault(x => x.Keyword.Equals(keyword, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #region Override methods

        public static bool operator == (Language left, Language right)
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

        public static bool operator != (Language left, Language right)
        {
            return (left == right) == false;
        }

        public override bool Equals (object obj)
        {
            if (obj is Language == false)
            {
                return false;
            }

            return Keyword.Equals(((Language)obj).Keyword, StringComparison.CurrentCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Keyword.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
