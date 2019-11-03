using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Animator Anim;
    public float speed = 0.001F;
    public float rotationSpeed = 100.0F;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        if(translation > 0)
        {
            Anim.SetBool("isWalking", true);
            Anim.SetBool("isWalkBack", false);
            Anim.SetBool("isTurnLeft", false);
            Anim.SetBool("isTurnRight", false);
        }
        else if (translation < 0)
        {
            Anim.SetBool("isWalking", false);
            Anim.SetBool("isWalkBack", true);
            Anim.SetBool("isTurnLeft", false);
            Anim.SetBool("isTurnRight", false);
        }
        else if (rotation > 0)
        {
            Anim.SetBool("isWalking", false);
            Anim.SetBool("isWalkBack", false);
            Anim.SetBool("isTurnLeft", true);
            Anim.SetBool("isTurnRight", false);
        }
        else if (rotation < 0)
        {
            Anim.SetBool("isWalking", false);
            Anim.SetBool("isWalkBack", false);
            Anim.SetBool("isTurnLeft", false);
            Anim.SetBool("isTurnRight", true);
        }
        else
        {
            Anim.SetBool("isWalking", false);
            Anim.SetBool("isWalkBack", false);
            Anim.SetBool("isTurnLeft", false);
            Anim.SetBool("isTurnRight", false);
        }
    }
}
