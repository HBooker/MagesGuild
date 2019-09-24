using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalQueue : MonoBehaviour
{
    public GameObject[] path;
    public Vector2 destinationOffset = Vector2.zero;
    public float speed = 1.0f;
    public GameObject QueuedObject;

    private List<GameObject> queueMembers;
    private Vector3 startPos;
    private Vector3 endPos;

    private bool portalOn = true;
    private float epsilon = float.Epsilon;
    private Vector3 epsilonVector;

    // Start is called before the first frame update
    void Start()
    {
        queueMembers = new List<GameObject>();
        startPos = path[0].transform.position;
        endPos = path[1].transform.position;
        epsilonVector = new Vector2(epsilon, epsilon);
    }

    // Update is called once per frame
    void Update()
    {
        // DEBUG - Spawn new person in this queue
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Enqueue(QueuedObject);
        }

        AdvanceQueueMembers();
    }

    private void AdvanceQueueMembers()
    {
        // Iterate through all queue members
        for (int i = 0; i < queueMembers.Count; ++i)
        {
            GameObject queueMember = queueMembers[i];

            if (queueMember.transform.position != endPos)
            {
                Vector2 moveFrom = queueMember.transform.position;
                Vector2 moveTo;

                if (i == 0)
                {
                    // Move the first queue member toward the portal
                    if (!portalOn)
                    {
                        moveTo = endPos + new Vector3(path[1].GetComponent<BoxCollider2D>().bounds.size.x * 2.0f, 0.0f);
                    }
                    else
                    {
                        moveTo = endPos;
                    }
                }
                else
                {
                    // Move all other queue members toward the member in front of them
                    GameObject precedingMember = queueMembers[i - 1];
                    moveTo = precedingMember.transform.position + new Vector3(precedingMember.GetComponent<BoxCollider2D>().bounds.size.x, 0.0f);
                }

                queueMember.transform.position = Vector2.MoveTowards(moveFrom, moveTo, speed);
            }
        }
    }

    public void Enqueue(GameObject queuedObjectTemplate)
    {
        GameObject newQueueMember = Object.Instantiate(queuedObjectTemplate);
        newQueueMember.transform.position = startPos;
        queueMembers.Add(newQueueMember);
    }

    public void PortalHit(GameObject hitBy)
    {
        Debug.Log("portal hit");
        if (portalOn)
        {
            queueMembers.Remove(hitBy);
            Object.Destroy(hitBy);
            GameObject.Find("Scoreboard").GetComponent<Scoreboard>().IncrementScore();
        }
    }

    public void DisablePortal()
    {
        portalOn = false;
    }

    public void EnablePortal()
    {
        portalOn = true;
    }
}
