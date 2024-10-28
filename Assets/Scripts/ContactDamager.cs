using UnityEngine;

public class ContactDamager : MonoBehaviour
{
    public float damage;

    void OnTriggerEnter(Collider other)
    {

        Life life = other.GetComponent<Life>();

        // if this is Player's bullet
        if (gameObject.CompareTag("PlayerBullet"))
        {
            if (!other.CompareTag("Player"))
            {
                Destroy(gameObject);

                if (life != null)
                    life.amount -= damage;
            }
        }

        // if this is Enemy's bullet
        if (gameObject.CompareTag("EnemyBullet"))
        {
            if (!other.CompareTag("Enemy"))
            {
                Destroy(gameObject);

                if (life != null)
                    life.amount -= damage;
            }
        }

    }
}