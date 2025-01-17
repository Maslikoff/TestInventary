using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private Vector2 _speed = new Vector3(3f, 0);
    [SerializeField] private int _attackDamage = 10;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _rb.velocity = new Vector2(_speed.x * transform.localScale.x, _speed.y);
    }

    private void Update()
    {
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            HealthBar healthBar = collision.gameObject.GetComponent<HealthBar>();
            healthBar.TakeDamage(_attackDamage);

            Destroy(gameObject);
        }
    }
}
