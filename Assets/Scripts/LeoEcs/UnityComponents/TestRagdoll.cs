using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRagdoll : MonoBehaviour
{
    public bool isActive = false;
    private bool activated = false;
    private Rigidbody[] bodies;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        bodies = GetComponentsInChildren<Rigidbody>();
        isActive = animator.enabled;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive == activated) return;

        foreach (var item in bodies)
        {
            item.isKinematic = isActive;
        }
        animator.enabled = isActive;
        activated = isActive;
    }

    private IEnumerator RagdollOptimization()
    {
        yield return null;
    }
}
