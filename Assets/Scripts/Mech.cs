using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mech : MonoBehaviour {

    public NavMeshAgent agent;
    public LayerMask whatIsGround, whatIsPlayer;
    public Animator enemyAnimator;
    public GameObject bullet;
    public Transform shootPoint;
    public Transform shootPoint2;
    public float shootSpeed = 50f;
    public float timeToShoot = 1.5f;
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
    }

    private void Update() {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if(!playerInSight && !playerInRange) Patrolling();
        if(playerInSight && !playerInRange) ChasePlayer();
        if(playerInSight && playerInRange) AttackPlayer();

        Vector3 distanceToWalkPoint = transform.position - player.position;
        if (distanceToWalkPoint.magnitude > 10f)
        {
            enemyAnimator.SetBool("isWalking", true);
        } else {
            enemyAnimator.SetBool("isWalking", false);
        }
        
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
    }

    private void AttackPlayer()
    {
        Transform bodyTransfrom = GameObject.FindGameObjectsWithTag("MechBody")[0].transform;
        Vector3 underPlayer = new Vector3(player.position.x, player.position.y - 4f, player.position.z);
        var targetRotation = Quaternion.LookRotation(underPlayer - bodyTransfrom.position);
        bodyTransfrom.rotation = Quaternion.Slerp(bodyTransfrom.rotation, targetRotation, 5f * Time.deltaTime);
        Vector3 eulerRotation = bodyTransfrom.rotation.eulerAngles;
        bodyTransfrom.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 270);

        timeToShoot -= Time.deltaTime;
        if(timeToShoot < 0)
        {
            ShootPlayer(bodyTransfrom);
            timeToShoot = originalTime;
        }

        Vector3 distanceToWalkPoint = transform.position - player.position;
        if (distanceToWalkPoint.magnitude > 10f)
        {
            agent.SetDestination(player.position);
            enemyAnimator.SetBool("isWalking", true);
        } else {
            agent.SetDestination(transform.position);
        }
    }

    private void ShootPlayer(Transform bodyTransfrom)
    {
        GameObject currentBullet = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
        GameObject currentBullet2 = Instantiate(bullet, shootPoint2.position, shootPoint2.rotation);
        Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
        Rigidbody rb2 = currentBullet2.GetComponent<Rigidbody>();
        rb.AddForce(bodyTransfrom.forward * shootSpeed * Time.deltaTime, ForceMode.VelocityChange);
        rb2.AddForce(bodyTransfrom.forward * shootSpeed * Time.deltaTime, ForceMode.VelocityChange);
        enemyAnimator.CrossFade("shoot", 0.3f);
    }
}