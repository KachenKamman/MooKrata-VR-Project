using UnityEngine;
using UnityEngine.AI; // จำเป็นสำหรับการเดิน

[RequireComponent(typeof(NavMeshAgent))] // บังคับว่าต้องมี NavMeshAgent
public class Enemy_Ghost : MonoBehaviour
{
    [Header("Target & Settings")]
    public Transform player;            // ลากตัวผู้เล่นมาใส่ตรงนี้
    public LayerMask playerLayer;       // Layer ของผู้เล่น (เพื่อให้ Raycast ไม่ติดกำแพง)
    
    [Header("Stats")]
    public float sightRange = 10f;      // ระยะมองเห็น
    public float attackRange = 2f;      // ระยะโจมตี
    public float damageAmount = 20f;    // ความแรง (ให้ตรงกับ float ใน Health ของคุณ)
    public float timeBetweenAttacks = 2f;

    [Header("Patrol")]
    public Transform[] patrolPoints;    // จุดเดินลาดตระเวน

    // สถานะภายใน
    private NavMeshAgent agent;
    private bool alreadyAttacked;
    private int currentPatrolIndex;
    private bool playerInSightRange;
    private bool playerInAttackRange;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null) return;

        // เช็คระยะห่าง
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        playerInSightRange = distanceToPlayer <= sightRange;
        playerInAttackRange = distanceToPlayer <= attackRange;

        // --- State Machine --- //

        // 1. ถ้าไม่เห็น และ ตีไม่ถึง -> เดินลาดตระเวน
        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }
        // 2. ถ้าเห็น แต่ยังตีไม่ถึง -> วิ่งไล่
        else if (playerInSightRange && !playerInAttackRange)
        {
            Chasing();
        }
        // 3. ถ้าเห็น และ อยู่ในระยะตี -> โจมตี
        else if (playerInSightRange && playerInAttackRange)
        {
            Attacking();
        }
    }

    private void Patroling()
    {
        if (patrolPoints.Length == 0) return;

        agent.speed = 2.0f; // เดินช้าๆ
        
        // ถ้าเดินถึงจุดหมายแล้ว ไปจุดถัดไป
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
        // ป้องกันยืนเอ๋อ กรณีไม่มีเป้าหมาย
        if (agent.destination == null || agent.remainingDistance < 0.5f)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    private void Chasing()
    {
        agent.speed = 4.5f; // วิ่งเร็ว
        agent.SetDestination(player.position);
    }

    private void Attacking()
    {
        agent.SetDestination(transform.position); // หยุดเดิน
        transform.LookAt(player); // หันหน้าหาผู้เล่น

        if (!alreadyAttacked)
        {
            // --- เชื่อมต่อกับ Script Health ของคุณ ---
            Health playerHealth = player.GetComponent<Health>();
            
            // เช็คว่าเจอ Script Health ไหม และผู้เล่นตายหรือยัง
            if (playerHealth != null && !playerHealth.IsDead())
            {
                Debug.Log("ผีตีผู้เล่น!");
                playerHealth.TakeDamage(damageAmount); // เรียกฟังก์ชันของคุณ
            }

            // --- จบการโจมตี ---

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    // วาดวงกลมใน Scene เพื่อให้เห็นระยะ (Debug)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}