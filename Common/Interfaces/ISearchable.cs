namespace Common.Interfaces
{
    /// <summary>
    /// 키워드로 검색이 되었는지 확인하는 인터페이스.
    /// </summary>
    public interface ISearchable
    {
        /// <summary>
        /// 오브젝트가 검색이 찾아졌는지 확인.
        /// </summary>
        /// <param name="keyword">검색 키워드.</param>
        /// <returns>true:검색에 찾아짐, false:검색에 찾아지지 않음.</returns>
        bool IsSearched(string keyword);
    }
}
