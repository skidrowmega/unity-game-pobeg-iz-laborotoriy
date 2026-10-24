using System.Collections;
using UnityEngine;

public class aaa : MonoBehaviour
{
    public float delay;
    public Vector3 offset;
    Vector3 startpos;
    Vector3 endpos;
    public float speed = 10;
    void Start()
    {
        startpos = transform.position;
        transform.position += offset;
        endpos=transform.position;
        StartCoroutine(dfgaaa());
    }

    IEnumerator dfgaaa()
    {
        yield return new WaitForSeconds(delay);
        float time=0;
        while (time<1)
        {
            transform.position=Vector3.Lerp(endpos, startpos, time);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
