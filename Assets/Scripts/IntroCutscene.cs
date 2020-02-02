using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutscene : MonoBehaviour
{
    public GameObject dialogPrefab;
    public GameObject explosionPrefab;
    // Start is called before the first frame update
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
        //player.GetComponent<ClickToMove>().enabled = true;
        skellyHead.SetActive(false);
        skellyPieces.SetActive(false);

        yield return new WaitForSeconds(3);
        showDialog(player, "Boss is home!!!", true);
        yield return new WaitForSeconds(6);
        Animator skellyAnimator = skelly.GetComponent<Animator>();
        skellyAnimator.Play("SkellyWalkIn");
        StartCoroutine(MoveTo(skelly, new Vector3(0, 0, -1.58f), 2));
        yield return new WaitForSeconds(2);
        skellyAnimator.Play("SkellyIdle");
        showDialog(player, "Welcome home!!!", true);
        yield return new WaitForSeconds(4.5f);
        showDialog(skelly, "I feel...", true);
        yield return new WaitForSeconds(3.5f);
        GameObject e = Instantiate(explosionPrefab, skelly.transform.position, Quaternion.identity);
        e.transform.position += new Vector3(0, 1, 0);
        yield return new WaitForSeconds(0.2f);
        showDialog(player, "Boss!!!", true);
        skelly.SetActive(false);
        skellyHead.SetActive(true);
        skellyPieces.SetActive(true);
        yield return new WaitForSeconds(1.8f);
        showDialog(skellyHead, "Overwhelmed!!!", true);
        yield return new WaitForSeconds(4);
        showDialog(skellyHead, "I could use some food...", true);
        yield return new WaitForSeconds(5);
        showDialog(skellyHead, "Carry me to the kitchen!", false);
        yield return new WaitForSeconds(4);
        player.GetComponent<ClickToMove>().enabled = true;
    }

    IEnumerator MoveTo(GameObject obj, Vector3 target, float time)
    {
        float now = Time.time;
        Vector3 start = obj.transform.position;
        while(Time.time - now < time)
        {
            float t = (Time.time - now) / time;
            obj.transform.position = Vector3.Lerp(start, target, t);
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
