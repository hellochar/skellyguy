using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MakeFoodCutscene : MonoBehaviour
{
    public GameObject dialogPrefab;
    public GameObject explosionPrefab;

    void Start()
    {
        StartCoroutine(cutscene());
    }

    IEnumerator cutscene()
    {
        GameObject player = GameObject.Find("Player");
        GameObject skelly = GameObject.Find("SkellyGuy");
        GameObject skellyHead = GameObject.Find("skelly-head");
        GameObject skellyPieces = GameObject.Find("skelly-pieces");
        GameObject banana = GameObject.Find("Banana");
        GameObject flambe = GameObject.Find("flambe");
        player.GetComponent<ClickToMove>().enabled = false;

        Vector3 fireplace = new Vector3(-13.123f, 1.36f, 9.04f);
        // skellyHead.SetActive(false);
        // skellyPieces.SetActive(false);

        // put head on kitchen table
        skellyHead.transform.parent = null;
        StartCoroutine(MoveTo(skellyHead, new Vector3(-9.02f, 1.36f, 9.04f), 0.5f));
        showDialog(skellyHead, "", false);

        yield return new WaitForSeconds(1);

        // move player to banana
        NavMeshAgent playerAgent = player.GetComponent<NavMeshAgent>();
        playerAgent.SetDestination(banana.transform.position);
        yield return new WaitForSeconds(1.5f);

        // put banana in fireplace
        StartCoroutine(MoveTo(banana, fireplace, 1.2f));
        yield return new WaitForSeconds(2f);

        // explosion
        GameObject explosion = Instantiate(explosionPrefab, fireplace, Quaternion.identity);
        explosion.transform.localScale.Set(0.1f, 0.1f, 0.1f);
        yield return new WaitForSeconds(0.2f);
        banana.SetActive(false);

        yield return new WaitForSeconds(1.2f);

        StartCoroutine(ScaleTo(flambe, new Vector3(50, 50, 50), 0.5f));
        yield return new WaitForSeconds(1f);

        Vector3 flambePosition = new Vector3(-10.43f, 1.53f, 9.48f);
        StartCoroutine(MoveTo(flambe, flambePosition, 1f));
        yield return new WaitForSeconds(2);

        StartCoroutine(RotateTo(skellyHead, Quaternion.Euler(0, -90, 0), 0.5f));
        yield return new WaitForSeconds(1);

        StartCoroutine(MoveTo(skellyHead, flambePosition, 0.2f));
        yield return new WaitForSeconds(0.5f);

        // eat flambe
        StartCoroutine(ScaleTo(flambe, new Vector3(0, 0, 0), 0.2f));
        yield return new WaitForSeconds(1f);

        skellyHead.transform.Find("Particle System").gameObject.SetActive(false);

        // you win!
        showDialog(skellyHead, "You win!!!", false);
        yield return new WaitForSeconds(1f);

        showDialog(player, "I win!!!", false);

        player.GetComponent<ClickToMove>().enabled = true;
    }

    IEnumerator RotateTo(GameObject obj, Quaternion target, float time)
    {
        float now = Time.time;
        Quaternion start = obj.transform.rotation;
        while (Time.time - now < time)
        {
            float t = (Time.time - now) / time;
            obj.transform.rotation = Quaternion.Lerp(start, target, t);
            yield return null;
        }
    }

    IEnumerator MoveTo(GameObject obj, Vector3 target, float time)
    {
        float now = Time.time;
        Vector3 start = obj.transform.position;
        while (Time.time - now < time)
        {
            float t = (Time.time - now) / time;
            obj.transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }
    }
    IEnumerator ScaleTo(GameObject obj, Vector3 target, float time)
    {
        float now = Time.time;
        Vector3 start = obj.transform.localScale;
        while (Time.time - now < time)
        {
            float t = (Time.time - now) / time;
            obj.transform.localScale = Vector3.Lerp(start, target, t);
            yield return null;
        }
    }

    void showDialog(GameObject obj, string text, bool autoHide)
    {
        TextMesh textMesh = obj.GetComponentInChildren<TextMesh>();
        if (textMesh == null)
        {
            GameObject dialogObject = Instantiate(dialogPrefab, obj.transform);
            textMesh = dialogObject.GetComponent<TextMesh>();
        }
        textMesh.text = text;
        textMesh.transform.position = new Vector3(textMesh.transform.position.x, 2, textMesh.transform.position.z);
        if (autoHide)
        {
            StartCoroutine(hideTextAfter(textMesh, 0.25f * text.Length));
        }
    }

    IEnumerator hideTextAfter(TextMesh text, float time)
    {
        yield return new WaitForSeconds(time);
        text.text = "";
    }
}
