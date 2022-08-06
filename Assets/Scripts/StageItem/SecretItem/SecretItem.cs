using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretItem : MonoBehaviour
{
    [Tooltip("回転速度"), SerializeField] private float _rotateSpeed;

    private GameObject _attachBox;

    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, _rotateSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EventCenter.GetSecretItemNotify();
            Destroy(this.gameObject);
        }

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
        if (other.tag == "Box")
        {
            _attachBox.GetComponent<Box>().IsSecretItemInBox = false;
            _attachBox = null;
        }
    }
}
