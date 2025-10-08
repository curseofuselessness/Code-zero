using UnityEngine;

public class FireShotgun : MonoBehaviour
{
    public AudioSource shotgunShoot;
    public Transform muzzle, hands; // дуло
    public LayerMask hitLayers; // слои, по которым можно стрелять
    public GameObject hitSpherePrefab, muzzleflashPrefab;

   

    public float range = 100f;
    public float spread = 0.001f;

    public int amountOfBulletsPerShot = 12;
    
    public bool isRecoilWorking = false;
    

    void Update()
    {

        // ОТДАЧА
        Vector3 currentRotation = hands.transform.localEulerAngles;

        currentRotation.x = Utils.NormalizeAngle(hands.transform.localEulerAngles.x);

        if (Input.GetMouseButtonDown(0)) // левая кнопка мыши
        {
            Shoot();
            isRecoilWorking = true;
        }

        if (isRecoilWorking)
        {
            if (currentRotation.x > -15f)   // Верхний порог отдачи
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
            if(currentRotation.x < -6f) // Исходная позиция
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

            // Случайное отклонение от основного направления
            Vector3 direction = muzzle.forward;
            direction.x += Random.Range(-spread, spread) * 0.05f;
            direction.y += Random.Range(-spread, spread) * 0.05f;
            direction = direction.normalized; // ВАЖНО: нормализуем!

            Ray ray = new Ray(muzzle.position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, range, hitLayers))
            {
                Debug.Log("Попадание в: " + hit.collider.name);
                Debug.DrawLine(muzzle.position, hit.point, Color.red, 15f);

                // Пример: если цель имеет Rigidbody — оттолкнуть
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(-hit.normal * 10f, ForceMode.Impulse);
                }

                // Пример: если цель имеет скрипт Damageable
                // hit.collider.GetComponent<Damageable>()?.TakeDamage(10);
                // Создаём сферу на месте попадания
                 Instantiate(hitSpherePrefab, hit.point, Quaternion.identity);





            }
            else
            {
                Debug.Log("Промах");
                Debug.DrawRay(muzzle.position, muzzle.forward * range, Color.gray, 15f);
            }

        }

        
    }
}
