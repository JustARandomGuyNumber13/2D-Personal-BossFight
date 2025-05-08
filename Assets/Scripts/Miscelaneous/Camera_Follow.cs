using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    [SerializeField] private Transform p;
    [SerializeField] private float xBound;

    private Camera cam;
    private float camSize;
    private Transform t;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        t = transform;
    }
    private void Start()
    {
        camSize = cam.orthographicSize * cam.aspect;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero - Vector3.right * xBound, Vector3.zero + Vector3.right * xBound);
    }

    private void LateUpdate()
    {
        if (IsInBoundary())
            t.position = new Vector3(p.position.x, t.position.y, t.position.z);
    }
    private bool IsInBoundary()
    {
        return camSize + Mathf.Abs(p.position.x) < xBound;
    }
}
