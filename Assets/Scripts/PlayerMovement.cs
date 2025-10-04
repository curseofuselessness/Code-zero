using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform Head, Body;         // ������ Head
    public Animator animator;

    public float walkSpeed = 5f;
    public float mouseSensitivity = 3f;

    private float pitch = 0f;      // ������������ ���� ������
    private float yaw = 0f;        // �������������� ���� ����

    void Update()
    {
        // �������� �����
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -60f, 60f);

        

        Body.localRotation = Quaternion.Euler(0, yaw, 0);                 // �������� ����
        Head.localRotation = Quaternion.Euler(pitch, 0, 0);              // �������� ������

        // �������� WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical);
        movement = Body.TransformDirection(movement);               // ��������� �����������

        Body.position += movement * walkSpeed * Time.deltaTime;

        animator.SetFloat("Speed", vertical);
    }
}