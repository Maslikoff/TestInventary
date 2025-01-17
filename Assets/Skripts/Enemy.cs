using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _attackCooldown;

    [SerializeField] protected GameObject[] _dropItemPrefabs;
    [SerializeField] private float _dropChance = 0.5f;

    private HealthBar _healthBar;
    private Transform _playerPos;
    private bool _isPlayerInRange;
    private bool _canAttack = true;

    private void Start()
    {
        _playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        _healthBar = gameObject.GetComponent<HealthBar>();
    }

    private void Update()
    {
        MoveEnemy();
        EnemyAlive();
    }

    public void EnemyAlive()
    {
        if (_healthBar.CurrentHealth <= 0)
        {
            if (_dropItemPrefabs.Length > 0)
            {
                if (Random.value <= _dropChance)
                {
                    int randomIndex = Random.Range(0, _dropItemPrefabs.Length);
                    GameObject itemToDrop = _dropItemPrefabs[randomIndex];

                    Instantiate(itemToDrop, transform.position, Quaternion.identity);
                }
            }
            Destroy(gameObject);
        }
    }

    private void MoveEnemy()
    {
        if (_isPlayerInRange)
        {
            Vector2 direction = (_playerPos.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, _playerPos.position, _moveSpeed * Time.deltaTime);

            if (direction.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else if (direction.x > 0)
                transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
            _isPlayerInRange = true; 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
            _isPlayerInRange = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _canAttack)
        {
            HealthBar healhPlayer = collision.gameObject.GetComponent<HealthBar>();
            StartCoroutine(AttackCoroutine(healhPlayer));
        }
    }

    private IEnumerator AttackCoroutine(HealthBar healhPlayer)
    {
        _canAttack = false;
        healhPlayer.TakeDamage(_damage);
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }
}
