using UnityEngine;

[CreateAssetMenu(fileName = "CameraData_SO", menuName = "LightLines/CameraData_SO", order = 0)]
public class CameraData_SO : ScriptableObject {
    [Tooltip("注視点"), SerializeField] public Transform _focus = default;
    [Tooltip("距離"), SerializeField, Range(1f, 20f)] float _distance = 5f;
    [Tooltip("最小距離"), SerializeField, Range(1f, 20f)] float _minDistance = 2f;
    [Tooltip("最大距離"), SerializeField, Range(1f, 20f)] float _maxDistance = 8f;
    [Tooltip("注視点半径"), SerializeField, Min(0f)] float _focusRadius = 1f;
    [Tooltip("注視点を追跡する >0は追跡する"), SerializeField, Range(0f, 1f)] float _focusCentering = 0.5f;
    [Tooltip("回転速度/秒"), SerializeField, Range(1f, 360f)] float _rotationSpeed = 90f;
    [Tooltip("垂直回転最小角度"), SerializeField, Range(-89f, 89f)] float _minVerticalAngle = -30f;
    [Tooltip("垂直回転最大角度"), SerializeField, Range(-89f, 89f)] float _maxVerticalAngle = 60f;
    [Tooltip("自動追尾遅延時間"), SerializeField, Min(0f)] float _alignDelay = 5f;
    [Tooltip("自動追尾速度"), SerializeField, Range(0f, 90f)] float _alignSmoothRange = 45f;
    [Tooltip("障害物Layer"), SerializeField] LayerMask _obstructionMask = -1;
}