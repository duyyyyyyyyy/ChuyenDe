using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public int damage = 20;

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu đối tượng bị trúng có tag Enemy
        if (other.CompareTag("Enemy"))
        {
            // Gọi TakeDamage từ EnemyAI
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // Huỷ đạn sau khi trúng
            Runner.Despawn(Object);
        }
    }
}
