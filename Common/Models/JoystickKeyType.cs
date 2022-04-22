using Common.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Models
{
    /// <summary>
    /// 조이스틱 버튼 타입.
    /// </summary>
    public class JoystickKeyType
    {
        #region Enums

        /// <summary>
        /// 푸시 버튼 타입.
        /// </summary>
        public static JoystickKeyType Button => new JoystickKeyType()
        {
            Id = 0,
            Name = "버튼",
            Description = "누루는 버튼 형태 타입입니다."
        };

        /// <summary>
        /// 조이스틱 축 타입.
        /// </summary>
        public static JoystickKeyType Axis => new JoystickKeyType()
        {
            Id = 1,
            Name = "축",
            Description = "네 방향으로 동작하는 축 형태 타입입니다."
        };

        /// <summary>
        /// 전체.
        /// </summary>
        public static List<JoystickKeyType> All => ReflectionTool.GetReadOnlyProperties<JoystickKeyType>(MethodBase.GetCurrentMethod().Name).ToList();

        #endregion

        #region Constructors

        private JoystickKeyType()
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// 식별자.
        /// </summary>
        public int Id
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

        /// <summary>
        /// 정수형.
        /// </summary>
        public string Description
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
        public static JoystickKeyType Convert(int id)
        {
            return All.FirstOrDefault(x => x.Id.Equals(id));
        }

        #endregion

        #region Override methods

        public static bool operator ==(JoystickKeyType left, JoystickKeyType right)
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

        public static bool operator !=(JoystickKeyType left, JoystickKeyType right)
        {
            return left == right == false;
        }

        public override bool Equals(object obj)
        {
            return (obj is JoystickKeyType rate) && Id.Equals(rate.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
