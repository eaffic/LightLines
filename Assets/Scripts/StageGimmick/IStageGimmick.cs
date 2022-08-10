public interface IStageGimmick{
    int Number{ get; set; }
    bool IsOpen { get; }
    void Notify(int num, bool state); //更新
}