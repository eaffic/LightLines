using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ内の収集アイテム
/// </summary>
public class Item : MonoBehaviour
{
    public bool OnDissolve;
    [Tooltip("回転速度"), SerializeField] private float _rotateSpeed;

    //箱の中にいる場合、その箱を記録する
    private GameObject _attachBox;
    private Renderer _renderer;

    private void Start()
    {
        TryGetComponent(out _renderer);
        _renderer.material.SetFloat("_CutoffHeight", 1f);

        //ステージ内のアイテム総数を計算する
        StageDataManager.Instance.TotalScretItemCountInStage++;
    }

    void Update()
    {
        //ゆっくり回転する
        transform.Rotate(new Vector3(0f, 0f, _rotateSpeed * Time.deltaTime));
    }

#region 接触処理
    private void OnTriggerEnter(Collider other)
    {
        if (OnDissolve) { return; }

        //プレイヤーに触ったら消える
        if (other.tag == "Player")
        {
            AudioManager.Instance.Play("Item", "ItemGet", false);
            EventCenter.GetSecretItemNotify();
            Destroy(this.gameObject);
        }

        //箱の中に隠している場合、その箱を記録し、通知する
        if (other.tag == "Box")
        {
            if (_attachBox == null)
            {
                _attachBox = other.gameObject;
                _attachBox.GetComponent<Box>().IsSecretItemInBox = true;
            }
            else if (_attachBox != other.gameObject)
            {
                _attachBox.GetComponent<Box>().IsSecretItemInBox = false;
                _attachBox = other.gameObject;
                _attachBox.GetComponent<Box>().IsSecretItemInBox = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //箱から出るとき、その箱に通知する
        if (other.tag == "Box")
        {
            _attachBox.GetComponent<Box>().IsSecretItemInBox = false;
            _attachBox = null;
        }
    }
#endregion

    /// <summary>
    /// 消滅エフェクト
    /// </summary>
    public void StartDissolve(){
        if (OnDissolve) { return; }
        
        OnDissolve = true;
        StartCoroutine(Dissolve());
    }

    /// <summary>
    /// 消滅エフェクト
    /// </summary>
    /// <returns></returns>
    IEnumerator Dissolve(){
        AudioManager.Instance.Play("Item", "ItemDissolve", false);

        float dissolve = _renderer.material.GetFloat("_CutoffHeight");
        while(dissolve > -1){
            dissolve -= Time.deltaTime;
            _renderer.material.SetFloat("_CutoffHeight", dissolve);
            yield return null;
        }
        Destroy(gameObject);
        yield return null;
    }
}
