using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Tools
{
    /// <summary>
    /// 사용자 애트리뷰트 툴.
    /// </summary>
    public class AttrubuteTool
    {
        /// <summary>
        /// 애트리뷰트 리스트를 가져옵니다.
        /// </summary>
        /// <typeparam name="T">필터링 해서 가져올 애트리뷰트 타입.</typeparam>
        /// <param name="targetType">애트리뷰트가 사용된 타입.</param>
        /// <returns>전달된 타입의 필터링 된 전체 애트리뷰트 리스트.</returns>
        public static List<T> GetClassAttributes<T>(Type targetType) where T : new()
        {
            return Attribute.GetCustomAttributes(targetType).Where(x => x is T).Cast<T>().ToList();
        }

        /// <summary>
        /// 애트리뷰트를 가져옵니다.
        /// 2개 이상을 허용하는 애트리뷰트인 경우는 사용 할 수 없습니다.
        /// </summary>
        /// <typeparam name="T">필터링 해서 가져올 애트리뷰트 타입.</typeparam>
        /// <param name="targetType">애트리뷰트가 사용된 타입.</param>
        /// <returns>전달된 타입의 필터링 된 단일 애트리뷰트.</returns>
        public static T GetClassAttribute<T>(Type targetType) where T : new()
        {
            List<T> attributeList = Attribute.GetCustomAttributes(targetType).Where(x => x is T).Cast<T>().ToList();

            return attributeList.Count > 0 ? attributeList.Single() : default;
        }

        public static IEnumerable<T> GetPropertyAttribute<T>(PropertyInfo propertyInfo) where T : new()
        {
            return Attribute.GetCustomAttributes(propertyInfo).Where(x => x is T).Cast<T>();
        }
    }
}
