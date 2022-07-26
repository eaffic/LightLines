using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageIcon : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 10f;

    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}
