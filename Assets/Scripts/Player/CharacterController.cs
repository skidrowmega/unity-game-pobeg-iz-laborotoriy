using UnityEditor;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float Speed;
    public Transform Cursor;
    Vector3 CursorVector;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Speed;
        }
        CursorVector =  Cursor.position- transform.position;
        transform.rotation = Quaternion.Euler(0, Mathf.Rad2Deg * Mathf.Atan2(-CursorVector.z, CursorVector.x), 0);
    }
}
