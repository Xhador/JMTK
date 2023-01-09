using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed;
    Rigidbody rgBody;

    Vector3 moveInput;
    Vector3 moveVelocity;

    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        rgBody = GetComponent<Rigidbody>();
        mainCamera = FindObjectOfType<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.position = new Vector3(transform.position.x, mainCamera.transform.position.y, transform.position.z);

        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput * moveSpeed;

        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    private void FixedUpdate()
    {
        rgBody.velocity = moveVelocity;
    }
}
