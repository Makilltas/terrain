using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform feet;
    public LayerMask groundMask;

    public float speed = 15f;
    public float gravity = -9.8f;
    public float jumpHeiht = 3;

    private CharacterController controller;
    private bool isGrounded;
    private float y;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }


    void Update()
    {
        isGrounded = Physics.CheckSphere(feet.position, 0.4f, groundMask);

        if (isGrounded) y = 0;

        var input = new Vector3();
        input.x = Input.GetAxis("Horizontal");
        input.z = Input.GetAxis("Vertical");

        var move = (transform.right * input.x + transform.forward * input.z) * speed * Time.deltaTime;

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            y = Mathf.Sqrt(jumpHeiht * -2f * gravity) * Time.deltaTime;
        }

        y += gravity * Time.deltaTime * Time.deltaTime;
        move.y = y;

        controller.Move(move);
    }

    private void OnDrawGizmos()
    {
        if (feet != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(feet.position, 0.4f);
        }
    }
}
