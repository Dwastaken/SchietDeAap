using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float MouseSensitivity = 100f;
    public Transform PlayerBody;

    float xRotation = 0f;


    private Vector3 originalPos;
    private float timer = 0f;


    public float bobSpeed = 14f;
    public float bobAmount = 0.05f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        originalPos = transform.localPosition;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);


        PlayerBody.Rotate(Vector3.up * mouseX);


        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            timer += Time.deltaTime * bobSpeed;


            float bobOffsetY = Mathf.Sin(timer) * bobAmount;


            transform.localPosition = originalPos + new Vector3(0, bobOffsetY, 0);
        }
        else
        {

            timer = 0;
            transform.localPosition = originalPos;
        }
    }
}
