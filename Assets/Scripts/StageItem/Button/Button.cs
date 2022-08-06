using UnityEngine;

public class Button : BaseStageGimmick {
    [SerializeField] private bool _stateLock;   //起動状態を維持する
    [SerializeField] private LayerMask _searchLayer = default;

    Light _light;
    MeshRenderer _meshRenderer;

    void Awake()
    {
        _light = GetComponentInChildren<Light>();
        TryGetComponent(out _meshRenderer);
    }

    private void Update() {
        _isOpen = TopCheck();
    }

    /// <summary>
    /// ボタンの上の物体チェック
    /// </summary>
    /// <returns></returns>
    bool TopCheck()
    {
        var colliders = Physics.OverlapBox(transform.position + transform.up * 0.5f, new Vector3(0.4f, 0.4f, 0.4f), Quaternion.identity, _searchLayer);
        if (colliders.Length > 0)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other) {
        _isOpen = true;
        EventCenter.ButtonNotify(Number, IsOpen);
    }

    private void OnTriggerExit(Collider other) {
        _isOpen = false;
        EventCenter.ButtonNotify(Number, IsOpen);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + transform.up * 0.5f, new Vector3(0.4f, 0.4f, 0.4f));
    }
}