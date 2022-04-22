using System;
using System.Text.RegularExpressions;

namespace Common.Tools
{
    /// <summary>
    /// 문자열 도구 집합.
    /// </summary>
    public class StringTool
    {
        #region Fields

        private static readonly Regex NUMERIC_REGEX = new Regex("[^0-9.-]+");

        #endregion

        #region Public methods

        /// <summary>
        /// 수를 표현하는 문자열인지 확인.
        /// </summary>
        /// <param name="text">입력 문자열.</param>
        /// <returns>true:수 문자열, false:수 문자열 아님.</returns>
        public static bool IsNumericText(string text)
        {
            return NUMERIC_REGEX.IsMatch(text);
        }

        /// <summary>
        /// 전달된 문자열에서 단어를 포함하는지 확인. 단 대소문자 무시.
        /// 전달된 문자열이 null이거나 빈 경우 항상 false.
        /// 전달된 단어가 null이거나 빈 경우 항상 true.
        /// </summary>
        /// <param name="paragraph">포함하는지 확인 할 대상 문자열.</param>
        /// <param name="word">포함 단어.</param>
        /// <returns>true:포함, false:미포함.</returns>
        public static bool IsContainsIgnoreCase(string paragraph, string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return true;
            }

            if (string.IsNullOrEmpty(paragraph))
            {
                return false;
            }

            return paragraph.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        #endregion
    }
}
