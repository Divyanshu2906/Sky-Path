using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction thrustdown;
    [SerializeField] InputAction rotation;
    Rigidbody rb;   // made a rigidbody variable for adding physics to the rocket
    [SerializeField] float thruststrength = 10f;
    [SerializeField] float rotationStrength = 100f;
    [SerializeField] float thrustd = 10f;
    [SerializeField] AudioClip mainengine;

    AudioSource audioSource; // made a variable of audio source 


    void Start()
    {
        rb = GetComponent<Rigidbody>(); // assigned rigidbody to rb 
        audioSource = GetComponent<AudioSource>(); // this statements gives component to audio source 
    }

    void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
        thrustdown.Enable();
    }

    void FixedUpdate()   // when we do anything with the physics it is considered as good practice to write it under fixed update
    {
        Processthrust();
        ProcessRotation();
        processthrustdescend();
        processthrustdescend();
    }

    private void Processthrust() // made a diffrent method for thrust so the code looks clean will do the same for every method 
    {
        if (thrust.IsPressed())
        {
            rb.AddRelativeForce(Vector3.up * thruststrength * Time.fixedDeltaTime);  // using relative force so it manages both local and global
    
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainengine); // now audio only plays when spacebar is pressed
            }
        }
        else
        {
            audioSource.Stop(); // when spacebar is not pressed the audio stops 
        }
    }

    private void processthrustdescend()
    {
        if (thrustdown.IsPressed())
        {
            rb.AddRelativeForce(-Vector3.up * thrustd * Time.fixedDeltaTime);
        }
    }

    void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();

        if (Mathf.Abs(rotationInput) > 0.1f)
        {
            ApplyRotation(rotationInput);
        }
    }

    void ApplyRotation(float rotationInput)
    {
        // Stop physics from messing with manual rotation
        rb.freezeRotation = true;

        // Rotate around local Z-axis
        transform.Rotate(Vector3.forward * -rotationInput * rotationStrength * Time.fixedDeltaTime, Space.Self);

        // Re-enable physics rotation
        rb.freezeRotation = false;
    }
}