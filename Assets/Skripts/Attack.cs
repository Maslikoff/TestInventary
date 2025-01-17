using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float radius = 5f;

    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _gunPoint;
    [SerializeField] private float _attackCooldown = 1.5f;

    private bool _canAttack = true;

    private void Update()
    {
        TryAttackEnemies();
    }

    private void TryAttackEnemies()
    {
        Collider2D[] enemies = FindEnemiesInRadius();

        if (enemies.Length > 0 && _canAttack)
            StartCoroutine(AttackCoroutine());
    }

    private Collider2D[] FindEnemiesInRadius() => Physics2D.OverlapCircleAll(transform.position, radius, _enemyLayer);

    private IEnumerator AttackCoroutine()
    {
        _canAttack = false;
        OnAttackButtonPressed();
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }

    public void OnAttackButtonPressed()
    {
        Inventory inventory = GameObject.FindObjectOfType<Inventory>();
        inventory.BulletAttakMenager();

        if (inventory.CountBullet > 0)
            ShootProjectile();
    }

    private void ShootProjectile()
    {
        GameObject projectile = Instantiate(_bulletPrefab, _gunPoint.position, _bulletPrefab.transform.rotation);
        FlipProjectile(projectile);
    }

    private void FlipProjectile(GameObject projectile)
    {
        Vector3 origScale = projectile.transform.localScale;
        projectile.transform.localScale = new Vector3(
            origScale.x * (transform.localScale.x > 0 ? 1 : -1),
            origScale.y,
            origScale.z
        );
    }
}
