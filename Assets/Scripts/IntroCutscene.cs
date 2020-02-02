using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutscene : MonoBehaviour
{
    public GameObject dialogPrefab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(cutscene());
    }

    IEnumerator cutscene()
    {
        yield return new WaitForSeconds(3);
        GameObject player = GameObject.Find("Player");
        showDialog(player, "Boss is home!!!", true);
        yield return new WaitForSeconds(6);
        GameObject skelly = GameObject.Find("SkellyGuy");
        Animator skellyAnimator = skelly.GetComponent<Animator>();
        skellyAnimator.Play("SkellyWalkIn");
        yield return new WaitForSeconds(2);
        showDialog(player, "Welcome home!!!", true);
        yield return new WaitForSeconds(6);
        showDialog(skelly, "I feel...", true);
        yield return new WaitForSeconds(4);
        showDialog(skelly, "Overwhelmed...", true);
        yield return new WaitForSeconds(6);
        showDialog(skelly, "I could use some food...", false);
        yield return new WaitForSeconds(4);
        player.GetComponent<ClickToMove>().enabled = true;
    }

    void showDialog(GameObject obj, string text, bool autoHide)
    {
        TextMesh textMesh = obj.GetComponentInChildren<TextMesh>();
        if (textMesh == null)
        {
            GameObject dialogObject = Instantiate(dialogPrefab, obj.transform);
            textMesh = dialogObject.GetComponent<TextMesh>();
        }
        Debug.Log(textMesh);
        textMesh.text = text;
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
