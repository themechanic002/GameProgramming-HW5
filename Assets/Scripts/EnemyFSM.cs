using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    public enum EnemyState { MoveToBase, ShootBase, MoveToPlayer, ShootPlayer }

    public EnemyState currentState;

    float lastShootTime;
    NavMeshAgent navMeshAgent;
    Transform baseTransform;

    public GameObject bulletPrefab;
    public float fireRate;
    public Sight sight;
    public float baseShootDistance;
    public float playerShootDistance;
    public ParticleSystem shootEffect;

    void Awake()
    {
        baseTransform = GameObject.Find("BaseDamagePoint").transform;
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.MoveToBase:
                MoveToBase();
                break;
            case EnemyState.ShootBase:
                ShootBase();
                break;
            case EnemyState.MoveToPlayer:
                MoveToPlayer();
                break;
            case EnemyState.ShootPlayer:
                ShootPlayer();
                break;
        }
    }


    void MoveToBase()
    {
        navMeshAgent.isStopped = false;

        navMeshAgent.SetDestination(baseTransform.position);

        if (sight.detectedObject != null)
            currentState = EnemyState.MoveToPlayer;

        float distanceToBase = Vector3.Distance(transform.position, baseTransform.position);
        if (distanceToBase <= baseShootDistance)
            currentState = EnemyState.ShootBase;
    }


    void MoveToPlayer()
    {
        navMeshAgent.isStopped = false;

        if (sight.detectedObject == null)
        {
            currentState = EnemyState.MoveToBase;
            return;
        }

        navMeshAgent.SetDestination(sight.detectedObject.transform.position);

        float distanceToPlayer = Vector3.Distance(transform.position, sight.detectedObject.transform.position);
        if (distanceToPlayer <= playerShootDistance)
            currentState = EnemyState.ShootPlayer;

    }

    void ShootPlayer()
    {
        navMeshAgent.isStopped = true;

        if (sight.detectedObject == null)
        {
            currentState = EnemyState.MoveToBase;
            return;
        }

        LookTo(sight.detectedObject.transform.position);
        Shoot();

        float distanceToPlayer = Vector3.Distance(transform.position, sight.detectedObject.transform.position);
        if (distanceToPlayer > playerShootDistance * 1.1f)
            currentState = EnemyState.MoveToPlayer;
    }

    void ShootBase()
    {
        navMeshAgent.isStopped = true;
        LookTo(baseTransform.position);
        Shoot();
    }

    void LookTo(Vector3 targetPosition)
    {
        Vector3 directionToPosition = Vector3.Normalize(targetPosition - transform.parent.position);
        directionToPosition.y = 0;
        transform.parent.forward = directionToPosition;
    }

    void Shoot()
    {
        var timeSinceLastShoot = Time.time - lastShootTime;
        if (timeSinceLastShoot < fireRate)
            return;

        lastShootTime = Time.time;
        Instantiate(bulletPrefab, transform.position, transform.rotation);
        shootEffect.Play();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, playerShootDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, baseShootDistance);
    }
}