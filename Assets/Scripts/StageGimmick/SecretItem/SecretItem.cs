using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ内の収集アイテム
/// </summary>
public class SecretItem : MonoBehaviour
{
    [Tooltip("回転速度"), SerializeField] private float _rotateSpeed;

    //箱の中にいる場合、その箱を記録する
    private GameObject _attachBox;

    private void Start()
    {
        //ステージ内のアイテム総数を計算する
        StageDataManager.Instance.TotalScretItemCountInStage++;
    }

    void Update()
    {
        //ゆっくり回転する
        transform.Rotate(new Vector3(0f, 0f, _rotateSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーに触ったら消える
        if (other.tag == "Player")
        {
            AudioManager.Instance.Play("Item", "GetItem", false);
            EventCenter.GetSecretItemNotify();
            Destroy(this.gameObject);

            //TODO 消えるのエフェクト、動画
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
}
