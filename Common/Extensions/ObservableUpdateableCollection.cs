using Common.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Common.Extensions
{
    /// <summary>
    /// 아이템을 업데이트하기 위한 컬렉션.
    /// </summary>
    /// <typeparam name="T">컬렉션 타입.</typeparam>
    public class ObservableUpdateableCollection<T> : ObservableCollection<T> where T : IUpdateable
    {
        #region Constructors

        public ObservableUpdateableCollection()
            : base()
        {
        }

        public ObservableUpdateableCollection(List<T> list)
            : base(list)
        {
        }

        public ObservableUpdateableCollection(IEnumerable<T> collection)
             : base(collection)
        {
        }

        #endregion

        #region IUpdateable implementations

        /// <summary>
        /// 컬렉션 업데이트.
        /// </summary>
        /// <param name="newObject"></param>
        public void Update(object newObject)
        {
            ObservableCollection<T> castObject = newObject as ObservableCollection<T>;
            if (castObject == null)
            {
                return;
            }

            foreach (var item in castObject)
            {
                if (this.FirstOrDefault(x => x.Equals(item)) == null)
                {
                    Add(item);
                }
            }

            for (int i = 0; i < Count;)
            {
                T item = castObject.FirstOrDefault(x => x.Equals(this[i]));
                if (item != null)
                {
                    this[i].Update(item);
                    i++;
                }
                else
                {
                    RemoveAt(i);
                }
            }
        }

        #endregion
    }
}
