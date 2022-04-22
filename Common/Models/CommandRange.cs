using Common.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Models
{
    /// <summary>
    /// 전역 명령 범위 열거형.
    /// </summary>
    public class CommandRange
    {
        #region Enums

        /// <summary>
        /// 선택시 동작하는 명령 타입.
        /// </summary>
        public static CommandRange MyTurnOnly => new CommandRange()
        {
            Id = 0,
            Name = "선택시 사용",
            Description = "명령 장치가 선택되어 있을 경우에만 실행됩니다."
        };

        /// <summary>
        /// 항상 동작하는 명령 타입.
        /// </summary>
        public static CommandRange Always => new CommandRange()
        {
            Id = 1,
            Name = "항상 사용",
            Description = "명령 실행시 항상 실행됩니다."
        };

        /// <summary>
        /// 전체.
        /// </summary>
        public static List<CommandRange> All => ReflectionTool.GetReadOnlyProperties<CommandRange>(MethodBase.GetCurrentMethod().Name).ToList();

        #endregion

        #region Constructors

        private CommandRange()
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
        public static CommandRange Convert(int id)
        {
            return All.FirstOrDefault(x => x.Id.Equals(id));
        }

        #endregion

        #region Override methods

        public static bool operator ==(CommandRange left, CommandRange right)
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

        public static bool operator !=(CommandRange left, CommandRange right)
        {
            return left == right == false;
        }

        public override bool Equals(object obj)
        {
            return (obj is CommandRange rate) && Id.Equals(rate.Id);
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
