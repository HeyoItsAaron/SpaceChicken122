using UnityEngine.AI;
using UnityEngine;
using System.Collections;
 
public class Wander : ChickenStats
{

    public float wanderRadius;
    public float wanderTimer;

    private Transform target;
    private NavMeshAgent agent;
    private float timer;
    Animator anim;
    float total = 10f;

    // Use this for initialization
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        

    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isWalking", true);
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        total -= Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
        if(total <= 0)
        {
            Destroy(gameObject);
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    public void TakeDamage(int damage)
    {
        currHealth -= damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Light Bullet"))
        {
            TakeDamage(15);
        }
        if (collision.collider.gameObject.CompareTag("Medium Bullet"))
        {
            TakeDamage(25);
        }
        if (collision.collider.gameObject.CompareTag("Heavy Bullet"))
        {
            TakeDamage(35);
        }
    }
}