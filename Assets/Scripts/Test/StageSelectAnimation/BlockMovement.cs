using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private float _duration;

    public void SetStartAndEndPosition(Vector3 start, Vector3 end, float duration){
        _startPosition = start;
        _endPosition = end;
        _duration = duration;
        StartCoroutine(MoveToPoint());
    }

    IEnumerator MoveToPoint(){
        transform.position = _startPosition;
        //単位時間の移動量
        Vector3 offset = (_endPosition - _startPosition) / _duration;

        while((transform.position - _endPosition).magnitude > offset.magnitude * Time.deltaTime * 1.5f){
            transform.position += offset * Time.deltaTime;
            yield return null;
        }

        transform.position = _endPosition;
        yield return null;
    }
}
