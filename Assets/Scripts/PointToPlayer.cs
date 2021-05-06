using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToPlayer : MonoBehaviour
{
    bool detected;
    GameObject target;
    public Transform enemy;
    public GameObject bullet;
    public Transform shootPoint;
    public float shootSpeed = 50f;
    public float timeToShoot = 1.5f;
    public Animator enemyAnimator;

    float originalTime;

    // Start is called before the first frame update
    void Start()
    {
        originalTime = timeToShoot;
    }

    void FixedUpdate()
    {
        if(detected)
        {
            timeToShoot -= Time.deltaTime;

            if(timeToShoot < 0)
            {
                ShootPlayer();
                timeToShoot = originalTime;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (detected)
        {
            Vector3 targetPosition = new Vector3(target.transform.position.x, enemy.transform.position.y, target.transform.position.z);
            enemy.LookAt(targetPosition);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            detected = true;
            target = other.gameObject;
        }
    }

    private void ShootPlayer()
    {
        GameObject currentBullet = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * shootSpeed * Time.deltaTime, ForceMode.VelocityChange);
        enemyAnimator.Play("shoot");
    }
}
