namespace Common.Interfaces
{
    /// <summary>
    /// 객체 업데이트 인터페이스.
    /// </summary>
    public interface IUpdateable
    {
        /// <summary>
        /// 업데이트.
        /// </summary>
        /// <param name="newObject"></param>
        void Update(object newObject);
    }
}
