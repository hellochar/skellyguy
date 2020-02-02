using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float timeToFade = 0.2f;
    float timeMade = 0;
    // Start is called before the first frame update
    void Start()
    {
        timeMade = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float percentLeft = Mathf.Max(0, 1 - (Time.time - timeMade) / timeToFade);
        transform.localScale = new Vector3(percentLeft, percentLeft, percentLeft);
        if (percentLeft <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
