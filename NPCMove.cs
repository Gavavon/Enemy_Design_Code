using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// this class holds all code that makes the nav mesh agent move
/// </summary>
public class NPCMove : MonoBehaviour
{
    private string[] states = new string[3] { "PATROL1", "PATROL2", "IDLE1" };
    private string currentState;
    public Transform[] patrol1;
    public Transform[] patrol2;
    public Transform[] idle1;
    private NavMeshAgent agent;
    int destPoint = 0;
    
    public TextMeshProUGUI tStateInfo;
    public TextMeshProUGUI tSpeedInfo;
    public TextMeshProUGUI tXInfo;
    public TextMeshProUGUI tYInfo;
    public TextMeshProUGUI tZInfo;

    /// <summary>
    /// this decideds what state the npc will start in
    /// additionally it will make sure a nav mesh agent is created and assigned
    /// </summary>
    void Start()
    {
        int temp = Random.Range(0, 2);
        currentState = states[temp];

        agent = this.GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
        }
    }
    /// <summary>
    /// this update method is used to display information
    /// and run certain methods depending on what state the npc is in
    /// </summary>
    void Update()
    {
        if (currentState == states[0])
        {
            navigate(patrol1);
        }
        if (currentState == states[1])
        {
            navigate(patrol2);
        }
        if (currentState == states[2])
        {
            idleNavigate(idle1);
        }
        displayInfo();
    }
    /// <summary>
    /// this IEnumerator is used to allow the agent to idle when at the outcrop
    /// additionally it is used to call the state change method after every patrol
    /// </summary>
    /// <returns></returns>
    IEnumerator enemyStateChange()
    {
        if (currentState.Equals(states[2]))
        {
            yield return new WaitForSeconds(9);
        }
        stateChanger();
        agent.isStopped = true;
        Debug.Log(currentState);
        agent.isStopped = false;

    }
    /// <summary>
    /// this is the markov chain and state changing method
    /// it will take the current state of the npc and decide what the next state is
    /// the probability of going from one state to the next is listed in the method via comments
    /// </summary>
    void stateChanger()
    {
        /* currently patrolling area 1
         * .4 chance continue
         * .3 chance patrol area 2
         * .3 chance idle
         * 
         * currently patrolling area 2
         * .4 chance continue
         * .5 chance patrol area 1
         * .1 chance idle
         * 
         * currently idle
         * .6 chance patrol area 1
         * .4 chance patrol area 2
         */

        double temp = Random.Range(1, 11);
        double chance = temp / 10;
        destPoint = 0;
        string nextState = "";
        if (currentState.Equals(states[0]))
        {
            if (chance <= .4)
            {
                nextState = states[0];
            }
            else
            {
                if (chance <= .7)
                {
                    nextState = states[1];
                }
                else
                {
                    nextState = states[2];
                }
            }
        }
        else
        {
            if (currentState.Equals(states[1]))
            {
                if (chance <= .4)
                {
                    nextState = states[0];
                }
                else
                {
                    if (chance <= .9)
                    {
                        nextState = states[1];
                    }
                    else
                    {
                        nextState = states[2];
                    }
                }
            }
            else
            {
                if (currentState.Equals(states[2]))
                {
                    if (chance <= .6)
                    {
                        nextState = states[0];
                    }
                    else
                    {
                        nextState = states[1];
                    }
                }
            }
        }

        if (nextState.Equals(""))
        {
            Debug.LogError("It broke");
            Debug.LogError(chance);
            Debug.LogError(currentState);
            Debug.LogError(nextState);
        }
        currentState = nextState;
    }
    /// <summary>
    /// this navigate method will take a list of points and patrol them
    /// at the end of one route it will change the state of the npc
    /// the param points is the list of desitinations
    /// WARNNING POINTS MUST PROVIDE MORE THAN 1 DESTINATION
    /// </summary>
    /// <param name="points"></param>
    void navigate(Transform[] points)
    {
        if (points.Length <= 1)
        {
            Debug.LogError("WARNNING POINTS MUST PROVIDE MORE THAN 1 DESTINATION");
            return;
        }
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {

            if (destPoint < points.Length)
            {
                agent.destination = points[destPoint].position;

            }
            destPoint = (destPoint + 1);
            if (destPoint == points.Length + 1)
            {
                destPoint = 0;
                agent.destination = points[destPoint].position;
                agent.isStopped = true;
                StartCoroutine(enemyStateChange());
            }

        }

    }
    /// <summary>
    /// this is for the idle navigation which brings you to a point
    /// after arriving at the point it will change the state of the npc
    /// the param points is the list of 1 destination
    /// WARNNING POINTS MUST PROVIDE 1 DESTINATION NO MORE OR LESS
    /// </summary>
    /// <param name="points"></param>
    void idleNavigate(Transform[] points)
    {
        if (points.Length == 0 || points.Length > 1)
        {
            Debug.LogError("WARNNING POINTS MUST PROVIDE 1 DESTINATION NO MORE OR LESS");
            return;
        }
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (destPoint < points.Length)
            {
                agent.destination = points[destPoint].position;

            }
            destPoint = (destPoint + 1);
            if (destPoint == points.Length + 1)
            {
                destPoint = 3;
                agent.isStopped = true;
                StartCoroutine(enemyStateChange());
            }


        }
    }
    /// <summary>
    /// this displays the information of the enemy object
    /// this is used for debugging
    /// </summary>
    void displayInfo()
    {
        tStateInfo.text = "State: " + currentState;
        tSpeedInfo.text = "Speed: " + GetComponent<NavMeshAgent>().speed;
        tXInfo.text = "X: " + transform.position.x;
        tYInfo.text = "Y: " + transform.position.y;
        tZInfo.text = "Z: " + transform.position.z;
    }

}
