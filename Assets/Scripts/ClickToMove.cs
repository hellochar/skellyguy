using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour {

    public LayerMask groundMask;
    public LayerMask itemMask;
    public LayerMask dropItemMask;
    NavMeshAgent agent;
    public GameObject spawnee;
    public GameObject makeFoodCutscene;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void pickUp(GameObject obj)
    {
        // parent the item onto the bat
        obj.transform.SetParent(this.transform);
        StartCoroutine(MoveToLocal(obj, new Vector3(0, 1.5f, 0.5f), 0.5f));
    }

    IEnumerator MoveToLocal(GameObject obj, Vector3 target, float time)
    {
        float now = Time.time;
        Vector3 start = obj.transform.localPosition;
        while (Time.time - now < time)
        {
            float t = (Time.time - now) / time;
            obj.transform.localPosition = Vector3.Lerp(start, target, t);
            yield return null;
        }
    }


    void dropItemOnto(GameObject container)
    {
        GameObject heldItem = getHeldItem();
        Debug.Log(heldItem);
        Mesh mesh = container.GetComponent<MeshFilter>().mesh;
        float containerY = mesh.bounds.max.y;
        heldItem.transform.SetParent(container.transform);
        heldItem.transform.position = new Vector3(heldItem.transform.position.x, containerY, heldItem.transform.position.z);
    }

    GameObject getHeldItem()
    {
        Transform t = transform;
        foreach (Transform tr in t)
        {
            if (tr.gameObject.layer == itemMask)
            {
                return tr.gameObject;
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        maybeUpdateCamera();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            bool hitItem = Physics.Raycast(ray, out hitInfo, 1000, itemMask);
            if (hitItem)
            {
                GameObject item = hitInfo.transform.gameObject;
                if (item.name == "Banana")
                {
                    Instantiate(makeFoodCutscene);
                } else
                {
                    pickUp(item);
                }
                
                return;
            }

            //bool hitDropItem = Physics.Raycast(ray, out hitInfo, 1000, dropItemMask);
            //if (hitDropItem)
            //{
            //    dropItemOnto(hitInfo.transform.gameObject);
            //    return;
            //}
            
            bool hitGround = Physics.Raycast(ray, out hitInfo, 1000, groundMask);
            if (hitGround)
            {
                agent.SetDestination(hitInfo.point);
                Instantiate(spawnee, hitInfo.point, Quaternion.identity);
            }
        }
    }

    void maybeUpdateCamera()
    {
        GameObject currentRoom = getCurrentRoom();
        if (currentRoom != null)
        {
            CameraView view = currentRoom.GetComponent<CameraView>();
            if (view != null)
            {
                Transform transform = Camera.main.transform;
                transform.position = Vector3.Lerp(transform.position, view.view.position, 0.2f);
                transform.rotation = Quaternion.Lerp(transform.rotation, view.view.rotation, 0.2f);
            }
        }
    }

    // gets the "default" object of whatever room you're in
    GameObject getCurrentRoom()
    {
        Ray ray = new Ray(transform.position, new Vector3(0, -1, 0));
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(ray, out hitInfo, 10, groundMask);
        if (hit)
        {
            return hitInfo.transform.gameObject;
        } else
        {
            return null;
        }
    }
}
