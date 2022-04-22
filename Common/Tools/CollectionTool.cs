using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Tools
{
    /// <summary>
    /// 컬렉션 도구.
    /// </summary>
    public static class CollectionTool
    {
        /// <summary>
        /// 리스트를 객체까지 복사합니다.
        /// </summary>
        /// <typeparam name="T">객체 타입.</typeparam>
        /// <param name="listToClone">소스 리스트.</param>
        /// <returns>복사된 리스트.</returns>
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        /// <summary>
        /// 맵을 객체까지 복사합니다.
        /// </summary>
        /// <typeparam name="TKey">맵의 키 타입.</typeparam>
        /// <typeparam name="TValue">캡의 값 타입.</typeparam>
        /// <param name="original">소스 맵.</param>
        /// <returns>복사된 맵.</returns>
        public static IDictionary<TKey, TValue> CloneDictionary<TKey, TValue>(IDictionary<TKey, TValue> original) where TValue : ICloneable
        {
            IDictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(original.Count);
            foreach (KeyValuePair<TKey, TValue> entry in original)
            {
                ret.Add(entry.Key, (TValue)entry.Value.Clone());
            }

            return ret;
        }
    }
}
