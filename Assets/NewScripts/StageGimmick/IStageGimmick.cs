public interface IStageGimmick{
    int Number{ get; set; }
    bool IsOpen { get; set; }
    void Notify(); //更新
}