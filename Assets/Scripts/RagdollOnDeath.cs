using NUnit.Framework;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RagdollOnDeath : MonoBehaviour
{

    List<Collider> colliders;
    List<Rigidbody> RBodies;

    private void Start()
    {
        colliders = new List<Collider>();
        RBodies = new List<Rigidbody>();
        InitializeColliders();
#if UNITY_EDITOR
        if (SceneManager.GetActiveScene().name == "RAGDOLLTEST") StartRagdoll(new Vector3(Random.Range(-1,1), Random.Range(-1, 1), Random.Range(-1, 1)).normalized*100, "spine.005");
#endif
    }

    public void StartRagdoll()
    {
        StartRagdoll(new Vector3(), "");
    }

    public void StartRagdoll(Vector3 force,string BodyPartName= "spine.005")
    {
        //Disable Animators
        //Turn on colliders
        //Make rigidbodies non-kinematic
        GetComponent<Collider>().enabled = false;
        foreach (Animator animator in GetComponentsInChildren<Animator>())
        {
            animator.enabled = false;
        }
        colliders.ForEach(x => x.enabled = true);
        RBodies.ForEach(x => x.isKinematic = false);
        if (BodyPartName=="") RBodies[Random.Range(0, RBodies.Count - 1)].AddForce(force, ForceMode.Impulse);
        else
        {
            RBodies.Find(x => x.gameObject.name == BodyPartName).AddForce(force, ForceMode.Impulse);
        }
    }


    void InitializeColliders()
    {
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            if (collider.gameObject != gameObject)
                colliders.Add(collider);
        }
        foreach (Rigidbody RBody in GetComponentsInChildren<Rigidbody>())
        {
            RBodies.Add(RBody);
        }
        colliders.ForEach(x => x.enabled = false);
        RBodies.ForEach(x => x.isKinematic = true);

    }


}
