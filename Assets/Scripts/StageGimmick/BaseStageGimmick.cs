using UnityEngine;

/// <summary>
/// 他のギミックと連動するのベースクラス
/// </summary>
public abstract class BaseStageGimmick : MonoBehaviour, IStageGimmick
{
    [SerializeField] protected int _number;
    public int ID
    {
        get => _number;
        set => _number = value;
    }

    [SerializeField] protected bool _isOpen;
    public bool IsOpen
    {
        get => _isOpen;
    }

    public virtual void OnNotify(int num, bool state) { }
}