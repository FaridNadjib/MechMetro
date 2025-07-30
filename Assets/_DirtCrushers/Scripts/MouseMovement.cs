using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public float range;

    private float distaceToCollectable;
    public float CollectableRange; // Range in which "mouse" can see the collectable
    public GameObject Collectable;

    public Transform centrePoint; //Set either AI gameobject or target area

    public GameObject Player;
    public float EnemyDistanceRun = 2f; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        float squaredDistance = (transform.position - Player.transform.position).sqrMagnitude;
        float enemyDistanceRunSquared = EnemyDistanceRun * EnemyDistanceRun;

        if (squaredDistance < enemyDistanceRunSquared)
        {
            // Run away from player
            Vector3 dirToPlayer = transform.position - Player.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;
            agent.SetDestination(newPos);
            Debug.Log("Running away");
            return;
        }

        if (Collectable != null)
        {
            distaceToCollectable = Vector3.Distance(transform.position, Collectable.transform.position);

            if (distaceToCollectable < CollectableRange)
            {
                // go to the collectable to pick it up
                Debug.Log("Me want screws");
                agent.SetDestination(Collectable.transform.position);
                return;
            }
        }

        if (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance) //done with path
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
            {
                Debug.Log("Patroling");
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                agent.SetDestination(point);
                Debug.Log("roaming");
            }
        }

    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {

            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }


}
