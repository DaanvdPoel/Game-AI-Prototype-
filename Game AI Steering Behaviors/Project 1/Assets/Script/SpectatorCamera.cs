using UnityEngine;

public class SpectatorCamera : MonoBehaviour
{
    [Header("Spectator Settings")]
    [SerializeField] private bool cameraMovementActive = true;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float movementSpeed = 5f;

    [Header("Private")]
    float mouseX;
    float mouseY;
    private void Update()
    {
        if (cameraMovementActive == true)
        {
            CameraMovement();
            MouseLook();
        }
    }

    private void CameraMovement()
    {
        if (Input.GetKey(KeyCode.W))
            transform.position = transform.position + (transform.forward * Time.deltaTime * movementSpeed);
        if (Input.GetKey(KeyCode.S))
            transform.position = transform.position + (-transform.forward * Time.deltaTime * movementSpeed);
        if (Input.GetKey(KeyCode.A))
            transform.position = transform.position + (-transform.right * Time.deltaTime * movementSpeed);
        if (Input.GetKey(KeyCode.D))
            transform.position = transform.position + (transform.right * Time.deltaTime * movementSpeed);
        if (Input.GetKey(KeyCode.R))
            transform.position = transform.position + (transform.up * Time.deltaTime * movementSpeed);
        if (Input.GetKey(KeyCode.F))
            transform.position = transform.position + (-transform.up * Time.deltaTime * movementSpeed);


        if (Input.GetKeyDown(KeyCode.LeftShift))
            movementSpeed = movementSpeed * 2;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            movementSpeed = movementSpeed / 2;
    }

    private void MouseLook()
    {
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

             mouseX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
             mouseY += Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            transform.rotation = Quaternion.Euler(-mouseY, mouseX, 0f);
        }

        if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    } 
}
