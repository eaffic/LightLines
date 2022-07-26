using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretItem : MonoBehaviour
{
    [Tooltip("‰ñ“]‘¬“x"), SerializeField] float _rotateSpeed;

    GameObject _attachBox;

    // Start is called before the first frame update
    void Start()
    {
        StageManager._Instance.RegisterSecretItem(this.gameObject);    
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3( 0f, 0f, _rotateSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StageManager._Instance.RemoveGetSecretItem(this.gameObject);
            Destroy(this.gameObject);
        }

        if(other.tag == "Box")
        {
            if(_attachBox == null)
            {
                _attachBox = other.gameObject;
                _attachBox.GetComponent<Box>().IsSecretItemInBox = true;
            }
            else if(_attachBox != other.gameObject)
            {
                _attachBox.GetComponent<Box>().IsSecretItemInBox = false;
                _attachBox = other.gameObject;
                _attachBox.GetComponent<Box>().IsSecretItemInBox = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Box")
        {
            _attachBox.GetComponent<Box>().IsSecretItemInBox = false;
            _attachBox = null;
        }
    }
}
