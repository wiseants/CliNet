using Common.Attributes;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Tools
{
    /// <summary>
    /// 컨테이너 도구 모음.
    /// </summary>
    public class ContainerTool
    {
        #region Fields

        private static readonly string ASSEMBLY_GROUP_NAME = "Accesswe";

        #endregion

        #region Public methods

        /// <summary>
        /// 전달된 인터페이스 타입의 모든 클래스를 컨테이너로 등록.
        /// ContainerName 애트리뷰트로 컨테이너 이름을 정할 수 있으며 없는 경우 클래스 이름으로 등록됩니다.
        /// </summary>
        /// <param name="container">등록 할 컨테이너.</param>
        /// <param name="type">타겟 인터페이스 타입.</param>
        public static void RegisterAll(IUnityContainer container, Type type)
        {
            AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.IndexOf(ASSEMBLY_GROUP_NAME, StringComparison.CurrentCultureIgnoreCase) >= 0)
                .SelectMany(x => x.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsAbstract == false).ToList()
                .ForEach(x =>
                {
                    ContainerNameAttribute attr = AttrubuteTool.GetClassAttribute<ContainerNameAttribute>(x);
                    string name = attr == null || string.IsNullOrEmpty(attr.Name) ? x.Name : attr.Name;

                    if (container.IsRegistered(type, name))
                    {
                        throw new Exception(string.Format(
                            "Duplicate registration for container. Type:{0}, Name:{1}",
                            type.ToString(),
                            name));
                    }

                    _ = container.RegisterType(type, x, name);
                });
        }

        /// <summary>
        /// 전달된 추상 클래스 타입의 모든 구현 클래스 타입을 반환.
        /// </summary>
        /// <param name="type">추상 클래스 타입.</param>
        /// <returns>구현 클래스 타입 컬렉션.</returns>
        public static IEnumerable<Type> GetAllImplementations(Type type)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.IndexOf(ASSEMBLY_GROUP_NAME, StringComparison.CurrentCultureIgnoreCase) >= 0)
                .SelectMany(x => x.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsAbstract == false);
        }

        #endregion
    }
}
