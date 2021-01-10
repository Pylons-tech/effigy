using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobberCharacterController : MonoBehaviour
{
    public enum CompassDirection
    {
        North,
        South,
        East,
        West
    }

    public Animator Animator { get; private set; }
    public CompassDirection Direction;

    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Animator.SetBool("Forward", Input.GetKey(KeyCode.W));
        Animator.SetBool("Back", Input.GetKey(KeyCode.S));
        Animator.SetBool("StrafeL", Input.GetKey(KeyCode.A));
        Animator.SetBool("StrafeR", Input.GetKey(KeyCode.D));
        Animator.SetBool("RotateCW", Input.GetKey(KeyCode.E));
        Animator.SetBool("RotateCCW", Input.GetKey(KeyCode.Q));
    }
}
