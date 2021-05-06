using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    public NavMeshAgent agent;
    public LayerMask whatIsGround, whatIsPlayer;
    public Animator enemyAnimator;
    public GameObject bullet;
    public Transform shootPoint;
    public float shootSpeed = 50f;
    public float timeToShoot = 1.5f;
    public float shootDistance = 5f;
    bool walkPointSet;
    public float walkPointRange;
    public float sightRange, attackRange;
    public bool playerInSight, playerInRange;
    Vector3 walkPoint;
    Transform player;
    float originalTime;


    void Start() {
        originalTime = timeToShoot;
        player = GameObject.Find("Player").transform;
        Debug.Log(player);
    }

    private void Update() {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(!playerInSight && !playerInRange)Patrolling();
        if(playerInSight && !playerInRange)ChasePlayer();
        if(playerInSight && playerInRange)AttackPlayer();

    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Patrolling()
    {
        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
            Run();
        } else {
            SearchWalkPoint();
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if(distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

    private void SearchWalkPoint(){
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        Run();
    }

    private void AttackPlayer()
    {
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(targetPosition);
        timeToShoot -= Time.deltaTime;
        if(timeToShoot < 0)
        {
            ShootPlayer();
            timeToShoot = originalTime;
        }

        Vector3 distanceToWalkPoint = transform.position - targetPosition;
        if (distanceToWalkPoint.magnitude > shootDistance)
        {
            agent.SetDestination(player.position);
            Run();
        } else {
            agent.SetDestination(transform.position);
        }
    }

    private void ShootPlayer()
    {
        GameObject currentBullet = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * shootSpeed * Time.deltaTime, ForceMode.VelocityChange);
        enemyAnimator.CrossFade("shoot", 0.5f);
    }

    private void Run()
    {
        if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("run"))
        enemyAnimator.Play("run");
    }
}