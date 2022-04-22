using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Tools
{
    public class ReflectionTool
    {
        /// <summary>
        /// 읽기전용 프로퍼티 객체들 가져오기 메소드.
        /// </summary>
        /// <typeparam name="T">가져올 타입.</typeparam>
        /// <param name="ignoreMethodNames">가져오지 않을 메소드 이름 컬렉션.</param>
        /// <returns>읽기전용 프로퍼티 객체들.</returns>
        public static IEnumerable<T> GetReadOnlyProperties<T>(IEnumerable<string> ignoreMethodNames = null)
        {
            bool contains(string x)
            {
                return ignoreMethodNames != null && ignoreMethodNames.Any(y => y.Equals(x, StringComparison.OrdinalIgnoreCase));
            }

            return typeof(T).GetProperties()
                .Where(x => x.CanRead && x.CanWrite == false && contains(x.GetMethod.Name) == false)
                .Select(x => (T)x.GetValue(null, null));
        }

        /// <summary>
        /// 읽기전용 프로퍼티 객체들 가져오기 메소드.
        /// </summary>
        /// <typeparam name="T">가져올 타입.</typeparam>
        /// <param name="ignoreMethodName">가져오지 않을 메소드 이름.</param>
        /// <returns>읽기전용 프로퍼티 객체들.</returns>
        public static IEnumerable<T> GetReadOnlyProperties<T>(string ignoreMethodName)
        {
            return GetReadOnlyProperties<T>(new List<string>() { ignoreMethodName });
        }

        /// <summary>
        /// 도메인 전체 어셈블리에서 타입이름으로 타입 가져오는 메소드.
        /// </summary>
        /// <param name="typeName">타입 이름.</param>
        /// <returns>해당 타입.</returns>
        public static Type GetTypeFromDomain(string typeName)
        {
            return string.IsNullOrEmpty(typeName)
                ? null
                : AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).FirstOrDefault(x => x.Name.Equals(typeName));
        }
    }
}
