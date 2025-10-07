using UnityEngine;
//using static Unity.Cinemachine.InputAxisControllerBase<T>;

public class PlayerMovement : MonoBehaviour
{

    // �������  
    public Transform Head, Body, Hands;       
  //  public Animator animator;
    public CharacterController controller;
    

    // �������� ������ � ������������
    public float walkSpeed = 5f;
    public float mouseSensitivity = 5f;

    private float pitch = 0f;      // ������������ ���� ������
    private float yaw = 0f;        // �������������� ���� ����

    private float weaponStepForSwaySpeed = 6f;
    private float weaponReturnToAimpointSpeed = 0.03f;


    // ������
    private float gravity = 9.81f;
    private float fallSpeed = 1f;
    private float timer = 0f;

    private void Start()
    {
        //animator.applyRootMotion = false;

        if (Hands != null)
        {
            Debug.Log("ok");
        }
        if (Hands == null)
        {
            Debug.Log("not ok");
        }
    }

    void Update()
    {
        // �������� �����
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -60f, 60f);


        Body.localRotation = Quaternion.Euler(0, yaw, 0);
        Head.localRotation = Quaternion.Euler(pitch, 0, 0);              // �������� ������

        // �������� WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(horizontal, 0, vertical);
        Vector3 move = Body.TransformDirection(input) * walkSpeed;
        float speed = move.magnitude;

        // ���� -2 < Y < 2, -6,5 < X < -5,5 ��������
        Vector3 handsSwayY = new Vector3(0, 0, 0);
        Vector3 currentRotation = Hands.transform.localEulerAngles;
        if (speed > 0.1f)
        {
            timer += Time.deltaTime;

            // �����-������

            currentRotation.y = Mathf.Sin(timer * weaponStepForSwaySpeed) * 2f;

            handsSwayY = Hands.localPosition;
            handsSwayY.y = -2.6f + Mathf.Sin(timer * weaponStepForSwaySpeed) * 0.05f;

            Hands.localPosition = handsSwayY;

            Hands.transform.localEulerAngles = currentRotation;

        }
        else
        {

            float angleY = Hands.localEulerAngles.y > 180f ? Hands.localEulerAngles.y - 360f : Hands.localEulerAngles.y;


            if (Hands.localEulerAngles.y != 0f)
            {
                if (angleY < 0.1f && angleY > -0.1f)
                {
                    currentRotation.y = 0f;
                    Hands.transform.localEulerAngles = currentRotation;
                }
                if (angleY > 0.1f)
                {
                    currentRotation.y -= weaponReturnToAimpointSpeed;
                    Hands.transform.localEulerAngles = currentRotation;
                }

                if (angleY < -0.1f)
                {
                    
                    currentRotation.y += weaponReturnToAimpointSpeed;
                    Hands.transform.localEulerAngles = currentRotation;
                }

                
            }

            handsSwayY.y = Hands.localPosition.y;

            if (Hands.localPosition.y != -2.6f)
            {
                if (Hands.localPosition.y > -2.61f && Hands.localPosition.y < -2.59f) // - 2,7  -2,6  -2,5
                {
                    Debug.Log("1");
                    Debug.Log(Hands.localPosition.y);
                    handsSwayY = Hands.localPosition;
                    handsSwayY.y = -2.6f;
                    Hands.localPosition = handsSwayY;
                }
                if (Hands.localPosition.y > -2.6f)
                {
                    Debug.Log("2");
                    Debug.Log(Hands.localPosition.y);
                    handsSwayY = new Vector3(0, 0.01f, 0);
                    Hands.localPosition -= handsSwayY;
                }
                if (Hands.localPosition.y < -2.6f)
                {
                    Debug.Log("3");
                    handsSwayY = new Vector3(0, 0.01f, 0);
                    Hands.localPosition += handsSwayY;
                }
            }
            
                
            
            
        timer = 0f;
        }
            
        
       





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

      //  animator.SetFloat("Speed", vertical);
    }
}