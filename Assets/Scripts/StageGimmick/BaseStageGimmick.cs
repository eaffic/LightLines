using UnityEngine;

public abstract class BaseStageGimmick : MonoBehaviour, IStageGimmick
{
    [SerializeField] protected int _number;
    public int Number
    {
        get => _number;
        set => _number = value;
    }

    [SerializeField] protected bool _isOpen;
    public bool IsOpen
    {
        get => _isOpen;
    }

    public virtual void Notify(int num, bool state) {}
}