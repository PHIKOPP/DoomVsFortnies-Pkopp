using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AgentControlls : MonoBehaviour
{
    private Camera mainCam;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                agent.SetDestination(hit.point);
            }
        }
    }
}
