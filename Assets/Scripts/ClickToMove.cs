using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour {

    public LayerMask groundMask;
    public LayerMask itemMask;
    NavMeshAgent agent;
    public GameObject spawnee;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            bool hitItem = Physics.Raycast(ray, out hitInfo, 1000, itemMask);
            if (hitItem)
            {
                // pick up item
                Debug.Log(hitInfo.transform.gameObject.name);
            }
            else
            {
                bool hitGround = Physics.Raycast(ray, out hitInfo, 1000, groundMask);
                if (hitGround)
                {
                    agent.SetDestination(hitInfo.point);
                    GameObject gameObject1 = Instantiate(spawnee, hitInfo.point, Quaternion.identity);
                }
            }
        }
        maybeUpdateCamera();
    }

    void maybeUpdateCamera()
    {
        GameObject currentRoom = getCurrentRoom();
        CameraView view = currentRoom.GetComponent<CameraView>();
        Debug.Log(view.view.transform.name);
        if (view != null)
        {
            Transform transform = Camera.main.transform;
            transform.position = Vector3.Lerp(transform.position, view.view.position, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, view.view.rotation, 0.2f);
        }
    }

    // gets the "default" object of whatever room you're in
    GameObject getCurrentRoom()
    {
        Ray ray = new Ray(transform.position, new Vector3(0, -1, 0));
        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo, 10, groundMask);
        // Debug.Log(hitInfo.transform.gameObject.transform.parent);
        return hitInfo.transform.gameObject;
    }
}
