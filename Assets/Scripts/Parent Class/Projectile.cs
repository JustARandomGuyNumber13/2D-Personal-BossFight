using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float damageValue;
    [SerializeField] float gravityValue;
    [SerializeField] Vector3 launchDir;

    Rigidbody2D rb;
    Transform _transform;
    GameObject shooter;
    Vector3 shootDir;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _transform = rb.transform;
    }
    private void Start()
    {
        rb.gravityScale = gravityValue;
        transform.SetParent(transform.parent.parent.parent);
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != shooter)
        {
            Health_Handler other;
            collision.TryGetComponent<Health_Handler>(out other);

            if (other != null)
                other.Public_DecreaseHealth(damageValue);

            gameObject.SetActive(false);
        }
    }

    public void P_Launch(GameObject shooter, Transform shootPoint)
    {
        _transform.position = shootPoint.position;
        _transform.localScale = shootPoint.lossyScale;
        shootDir = launchDir;
        shootDir.x *= _transform.localScale.x;

        this.shooter = shooter;
        gameObject.SetActive(true);
        rb.linearVelocity = shootDir;
    }
    public void P_SetPos(Vector3 pos)
    { _transform.position = pos; }
}
