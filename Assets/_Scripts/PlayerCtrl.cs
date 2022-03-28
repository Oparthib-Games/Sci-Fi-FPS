using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    CharacterController CharCtrl;

    [SerializeField]    private float speed = 3.5f;
    [SerializeField]    private float mouseSensitivity = 1.2f;
    [SerializeField]    private float gravity = 9.81f;

    public GameObject muzzleFlash;
    public GameObject hitMarker;
    public GameObject crackedWoodenCrate;

    AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        CharCtrl = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Movement();

        LookX();
        LookY();

        Shoot();
    }

    void Shoot()
    {
        if(Input.GetMouseButton(0))
        {
            //Ray rayOrigin = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            Ray rayOrigin = Camera.main.ViewportPointToRay(new Vector3(1/2f, 1/2f, 0f));

            RaycastHit hitInfo;

            Debug.DrawRay(rayOrigin.origin, rayOrigin.direction * 500000, Color.green);

            muzzleFlash.SetActive(true);

            if (!audioSource.isPlaying) audioSource.Play();

            if (Physics.Raycast(rayOrigin, out hitInfo, Mathf.Infinity))
            {
                print("Raycast Hit =>>> " + hitInfo.transform.name);
                Debug.DrawRay(rayOrigin.origin, rayOrigin.direction * 500000, Color.red);
                Instantiate(hitMarker, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

                DestroyCrate(hitInfo.transform.gameObject);
            }
        }
        else
        {
            audioSource.Stop();
            muzzleFlash.SetActive(false);
        }
    }

    void DestroyCrate(GameObject hit_obj)
    {
        if(hit_obj.name == "Wooden_Crate")
        {
            Instantiate(crackedWoodenCrate, hit_obj.transform.position, hit_obj.transform.rotation);
            Destroy(hit_obj);
        }
    }

    void Movement()
    {
        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed * Time.deltaTime;

        moveDir.y -= gravity;

        moveDir = this.transform.TransformDirection(moveDir); //Moving Local to World space

        CharCtrl.Move(moveDir);
    }


    void LookX()
    {
        float mouseX = Input.GetAxis("Mouse X");

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + (mouseX * mouseSensitivity), transform.localEulerAngles.z);
    }

    void LookY()
    {
        float mouseY = Input.GetAxis("Mouse Y");

        Camera.main.transform.localEulerAngles = new Vector3(Camera.main.transform.localEulerAngles.x - (mouseY * mouseSensitivity), Camera.main.transform.localEulerAngles.y, Camera.main.transform.localEulerAngles.z);
    }
}
