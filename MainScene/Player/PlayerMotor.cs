using UnityEngine;

public class PlayerMotor : MonoBehaviour {

    public Camera camPlayer;
    public AudioClip[] audioWalkSounds;

    private Vector3 moveVelocity;
    private Vector3 rotation;
    private float rotationCamera;
    private Vector3 thrusterForce;

    private float currentRotationCamera;
    private float cameraRotationXLimit = 85;

    private Rigidbody rigi;
    public AudioSource audioSource;

    private float timeBefore;

    void Start()
    {
        rigi = GetComponent<Rigidbody>(); 
    }


    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }
     
    // get the move velocity
    public void Move(Vector3 _moveVelocity)
    {
        moveVelocity = _moveVelocity;
    }

    // get the rotation, to rotate player around Y axis
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation; 
    } 

    // get the amount of degrees to rotate camera around X axis
    public void RotateCamera(float _rotationCamera)
    {
        rotationCamera = _rotationCamera;
    }

    // get the thruster force to jump up
    public void ThrustJump(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }

    // move performance
    private void PerformMovement()
    {
        if (moveVelocity != Vector3.zero)
        {
            rigi.MovePosition(rigi.position + moveVelocity * Time.fixedDeltaTime);

            // make sound  
            if (Time.time > timeBefore + 2 / Vector3.Magnitude(moveVelocity) && transform.position.y < 1.5f)
            {
                audioSource.clip = audioWalkSounds[Random.Range(0, audioWalkSounds.Length)];
                audioSource.Play();
                timeBefore = Time.time;
            } 
             
        }

        if(thrusterForce != Vector3.zero)
        {
            rigi.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }

    // rotate performance
    private void PerformRotation()
    {
        rigi.MoveRotation(rigi.rotation * Quaternion.Euler(rotation));

        if(camPlayer != null)
        {
            currentRotationCamera -= rotationCamera;
            currentRotationCamera = Mathf.Clamp(currentRotationCamera, -cameraRotationXLimit, cameraRotationXLimit);

            camPlayer.transform.localEulerAngles = new Vector3(currentRotationCamera, 0, 0);
        }
    }



}
