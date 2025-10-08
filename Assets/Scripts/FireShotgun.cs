using UnityEngine;

public class FireShotgun : MonoBehaviour
{
    public AudioSource shotgunShoot;
    public Transform muzzle, hands; // ����
    public LayerMask hitLayers; // ����, �� ������� ����� ��������
    public GameObject hitSpherePrefab, muzzleflashPrefab;

   

    public float range = 100f;
    public float spread = 0.001f;

    public int amountOfBulletsPerShot = 12;
    
    public bool isRecoilWorking = false;
    

    void Update()
    {

        // ������
        Vector3 currentRotation = hands.transform.localEulerAngles;

        currentRotation.x = Utils.NormalizeAngle(hands.transform.localEulerAngles.x);

        if (Input.GetMouseButtonDown(0)) // ����� ������ ����
        {
            Shoot();
            isRecoilWorking = true;
        }

        if (isRecoilWorking)
        {
            if (currentRotation.x > -15f)   // ������� ����� ������
            {
                currentRotation.x -= 100f * Time.deltaTime;
            }
            else
            {
                currentRotation.x = -15f;

                isRecoilWorking = false;
            }
        }
        else
        {
            if(currentRotation.x < -6f) // �������� �������
            {
                currentRotation.x += 10f * Time.deltaTime;
            }
            else
            {
                currentRotation.x = -6f;
            }

        }

        hands.transform.localEulerAngles = currentRotation;

        Debug.Log(muzzle.position);
    }

    void Shoot()
    {

        shotgunShoot.Play();

        Vector3 forwardMuzzle = muzzle.forward;
       // forwardMuzzle.x 
        

        Quaternion flashRotation = Quaternion.identity;

        Instantiate(muzzleflashPrefab, muzzle.position, flashRotation);

        for (int i = 0; i < amountOfBulletsPerShot; i++)
        {

            // ��������� ���������� �� ��������� �����������
            Vector3 direction = muzzle.forward;
            direction.x += Random.Range(-spread, spread) * 0.05f;
            direction.y += Random.Range(-spread, spread) * 0.05f;
            direction = direction.normalized; // �����: �����������!

            Ray ray = new Ray(muzzle.position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, range, hitLayers))
            {
                Debug.Log("��������� �: " + hit.collider.name);
                Debug.DrawLine(muzzle.position, hit.point, Color.red, 15f);

                // ������: ���� ���� ����� Rigidbody � ����������
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(-hit.normal * 10f, ForceMode.Impulse);
                }

                // ������: ���� ���� ����� ������ Damageable
                // hit.collider.GetComponent<Damageable>()?.TakeDamage(10);
                // ������ ����� �� ����� ���������
                 Instantiate(hitSpherePrefab, hit.point, Quaternion.identity);





            }
            else
            {
                Debug.Log("������");
                Debug.DrawRay(muzzle.position, muzzle.forward * range, Color.gray, 15f);
            }

        }

        
    }
}
