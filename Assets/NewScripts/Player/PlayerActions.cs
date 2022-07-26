using UnityEngine;

/// <summary>
/// 移動以外のアクション制御
/// </summary>
public class PlayerActions : MonoBehaviour {
    private PlayerFSM _fsm;
    private GameObject _targetBox;

    [Tooltip("目の高さ"), SerializeField] 
    private float _eyeHeight = 0.7f;
    [Tooltip("前方レイの長さ(箱判定用)"), SerializeField] 
    private float _forwardRayLength = 0.5f;
    
    private bool _inBoxPushArea;    //箱のプッシュ可能範囲との接触
    

    private void Awake() {
        TryGetComponent(out _fsm);
    }

    public bool BoxCheck(){
        //目先の確認レイ
        Ray ray = new Ray(transform.position + new Vector3(0f, _eyeHeight, 0f), transform.forward);
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hitInfo, _forwardRayLength, LayerMask.GetMask("Box")))
        {
            //前フレームと別の箱をハイライトしたとき、リセットする
            if (_targetBox && hitInfo.collider.gameObject != _targetBox)
            {
                _targetBox.GetComponent<Box>().IsPlayerPushTarget = false;
                _targetBox = null;
                _targetBox = hitInfo.collider.gameObject;
                _targetBox.GetComponent<Box>().IsPlayerPushTarget = true;
            }
            else
            {
                //前フレームはハイライトの箱は存在していないとき
                _targetBox = hitInfo.collider.gameObject;
                _targetBox.GetComponent<Box>().IsPlayerPushTarget = true;
            }
            return true;
        }

        if (_targetBox)
        {
            _targetBox.GetComponent<Box>().IsPlayerPushTarget = false;
            _targetBox = null;
        }
        return false;

    }
}