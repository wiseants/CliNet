using System.Collections;

namespace Common
{
    /// <summary>
    /// 아이템소스 변경 이벤트 델리게이트.
    /// </summary>
    /// <param name="oldValue">이전 값.</param>
    /// <param name="newValue">새로운 값.</param>
    public delegate void ItemsSourceChangedHandler(IEnumerable oldValue, IEnumerable newValue);

    /// <summary>
    /// 파라미터를 참조로 하는 액션 델리게이트.
    /// </summary>
    /// <param name="sender">발송자.</param>
    /// <param name="param">파라미터 오브젝트.</param>
    public delegate bool RefActionEventHandler(object sender, ref string message, ref object param);

    /// <summary>
    /// 버퍼로 데이터를 받은 액션 델리게이트.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="buffer"></param>
    public delegate void ReceivedHandler(object sender, byte[] buffer);
}
