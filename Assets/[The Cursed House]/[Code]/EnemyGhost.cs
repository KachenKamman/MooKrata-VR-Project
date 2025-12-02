using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement; // [NEW] 1. ต้องเพิ่มบรรทัดนี้เพื่อใช้คำสั่งรีสตาร์ทฉาก

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy_Ghost : MonoBehaviour
{
    [Header("Target & Settings")]
    public Transform player;
    public LayerMask playerLayer;
    
    [Header("Stats")]
    public float sightRange = 10f;
    public float attackRange = 2f;
    public float damageAmount = 999f;
    public float timeBetweenAttacks = 2f;
    public float MoveS = 0.1f;

    [Header("Patrol")]
    public Transform[] patrolPoints;

    private NavMeshAgent agent;
    private bool alreadyAttacked;
    private bool playerInSightRange;
    private bool playerInAttackRange;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null) return;

        Vector3 currentPositionNoY = transform.position;
        currentPositionNoY.y = 0;

        Vector3 playerPositionNoY = player.position;
        playerPositionNoY.y = 0;    

        float distanceToPlayer = Vector3.Distance(currentPositionNoY, playerPositionNoY);
        playerInSightRange = distanceToPlayer <= sightRange;
        playerInAttackRange = distanceToPlayer <= attackRange;

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            Chasing();
        }
        else if (playerInSightRange && playerInAttackRange)
        {
            Attacking();
        }
    }

    private int currentPatrolIndex; // ย้ายมาประกาศตรงนี้เพื่อให้เก็บค่าได้ถูกต้อง
    private void Patroling()
    {
        if (patrolPoints.Length == 0) return;

        agent.speed = MoveS;
        
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
        if (agent.destination == null || agent.remainingDistance < 0.5f) // กันบั๊ก
        {
             agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    private void Chasing()
    {
        agent.speed = MoveS;
        agent.SetDestination(player.position);
    }

    private void Attacking()
    {
        Debug.Log("attacking");
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            Health playerHealth = player.GetComponent<Health>();
            
            if (playerHealth != null && !playerHealth.IsDead())
            {
                Debug.Log("ผีตีผู้เล่นตาย!");
                playerHealth.TakeDamage(9999); 

                // [NEW] 2. สั่งรีสตาร์ท Scene ปัจจุบันทันที
                // โค้ดนี้จะดึงชื่อ Scene ปัจจุบันมาแล้วสั่ง Load ใหม่
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}