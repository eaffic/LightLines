/// <summary>
/// 連動が必要のギミックのインターフェース
/// </summary>
public interface IStageGimmick
{
    int ID { get; set; } //ギミックのId

    bool IsOpen { get; } // ギミックの現在状態

    /// <summary>
    /// EventCenterに登録、他のギミックからの呼び出しを対応する
    /// </summary>
    /// <param name="num"></param>
    /// <param name="state"></param>
    void OnNotify(int num, bool state);
}