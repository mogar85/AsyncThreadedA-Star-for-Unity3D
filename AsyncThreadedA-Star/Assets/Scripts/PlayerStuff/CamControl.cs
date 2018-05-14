using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    public float speed = 20;

    // Update is called once per frame
    void FixedUpdate()
    {
        // pivot  left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = Vector3.Lerp(transform.position, transform.position - transform.right * speed * Time.deltaTime, .1f);
        }

        // pivot  right
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + transform.right * speed * Time.deltaTime, .1f);
        }

        // pivot  up
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward * speed * Time.deltaTime, .1f);
        }

        // pivot  down
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = Vector3.Lerp(transform.position, transform.position - transform.forward * speed * Time.deltaTime, .1f);
        }
    }
}
