using UnityEngine;
//using static Unity.Cinemachine.InputAxisControllerBase<T>;

public class PlayerMovement : MonoBehaviour
{

    // �������  
    public Transform Head, Body;       
    public Animator animator;
    public CharacterController controller;


    // �������� ������ � ������������
    public float walkSpeed = 5f;
    public float mouseSensitivity = 3f;

    private float pitch = 0f;      // ������������ ���� ������
    private float yaw = 0f;        // �������������� ���� ����

    // ������
    private float gravity = 9.81f;
    private float fallSpeed = 1f;

    private void Start()
    {
        animator.applyRootMotion = false;
    }

    void Update()
    {
        // �������� �����
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -60f, 60f);



        //transform.rotation = Quaternion.Euler(0, yaw, 0);               // �������� ����
        Body.localRotation = Quaternion.Euler(0, yaw, 0);
        Head.localRotation = Quaternion.Euler(pitch, 0, 0);              // �������� ������

        // �������� WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(horizontal, 0, vertical);
        Vector3 move = Body.TransformDirection(input) * walkSpeed;

        if (controller.isGrounded)
        {
            fallSpeed = -1;
        }
        else
        {
            fallSpeed += gravity;
        }

        // ���������� ����������
        move.y -= fallSpeed * Time.deltaTime;

        // ����������� � ������ ��������
        controller.Move(move * Time.deltaTime);

        animator.SetFloat("Speed", vertical);
    }
}