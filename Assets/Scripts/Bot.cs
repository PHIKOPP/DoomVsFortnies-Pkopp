using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    public float health = 100;
    NavMeshAgent agent;
    public GameObject testDestination;
    public float throwForce;

    public GameObject projectilePrefab, cannon;
    Vector3 nextTarget;
    bool shooting;
    public AudioSource shootingSfx;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        pickRandomDestination();
        Botmanager.bots.Add(this);
        //agent.SetDestination(testDestination.transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            die();
        if (Vector3.Distance(transform.position,agent.destination)< .1f)        
            pickRandomDestination();
        if (nextTarget == PlayerController.player.transform.position  && GetComponent<NavMeshAgent>().enabled)
            agent.SetDestination(nextTarget);
        if (GetComponent<BotEyes>().seesPlayer)
        {
            if(!shooting)
                StartCoroutine(shoot());
            StartCoroutine(chasePlayer());
        }
        
    }

    void pickRandomDestination()
    {
        float maxWalkDistance = 50f;
        Vector3 direction = Random.insideUnitSphere * maxWalkDistance;

        direction += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(direction, out hit, Random.Range(0f, maxWalkDistance), 1);

        Vector3 destination = hit.position;
        nextTarget = destination;
        agent.SetDestination(destination);
    }

    public void gotHit()
    {
        Animator anim = GetComponent<Animator>();
        anim.Play("Base Layer.Hit");

        // Move to player
        agent.SetDestination(PlayerController.player.transform.position);
        transform.LookAt(PlayerController.player.transform);
        if(!shooting)
            StartCoroutine(shoot());
        StartCoroutine(chasePlayer());
    }

    void die()
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<BoxCollider>().center = new Vector3(0,1.71f,0.154f);
        GetComponent<BoxCollider>().size = new Vector3(1,0.5f,3.75f);
        GetComponent<NavMeshAgent>().enabled = false;
        Animator anim = GetComponent<Animator>();
        anim.Play("Base Layer.Dying");
        Botmanager.bots.Remove(this);
    }

    IEnumerator shoot()
    {
        while (true)
        {
            shooting = true;
            if (health <= 0 || !GetComponent<BotEyes>().seesPlayer)
            {
                shooting = false;
                break;
            }
            GameObject bomb = Instantiate(projectilePrefab, cannon.transform.position, cannon.transform.rotation);
            Destroy(bomb, 1f);
            bomb.GetComponent<Bulletshot>().shotByBot = true;
            Rigidbody bombRB = bomb.GetComponent<Rigidbody>();
            shootingSfx.Play();
            bombRB.AddForce(transform.forward * throwForce, ForceMode.Impulse);
            yield return new WaitForSeconds(.25f);
        }
    }

    float chasingWhileSeeing;
    IEnumerator chasePlayer()
    {
        while (true)
        {
            if (health <= 0)
                break;
            nextTarget = PlayerController.player.transform.position;
            yield return new WaitForSeconds(.1f);
        }
    }
}
