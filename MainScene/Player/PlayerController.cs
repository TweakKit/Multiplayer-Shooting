using UnityEngine;

public class PlayerController : MonoBehaviour {


    public float speedBoost = 10;
    public float lookSensitivity = 3;
    public float thrusterForce = 100;

    public LayerMask environmentMask;

    private PlayerMotor playerMotor;
    private ConfigurableJoint configurableJoint;
    private Animator animator;

    private float speedMove;
    private float thrusterFuelAmount;

    void Start()
    {
        playerMotor = GetComponent<PlayerMotor>();
        configurableJoint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();

        JointSetting(20);
    }


    void Update()
    {
        // if pauseUIMenu active, we set something
        if(PauseUIMenu.isActive)
        {
            if (Cursor.lockState != CursorLockMode.None)
                Cursor.lockState = CursorLockMode.None;

            playerMotor.Move(Vector3.zero);
            playerMotor.Rotate(Vector3.zero);
            playerMotor.RotateCamera(0);
            
            return;
        } 

        // set curson display
        if (Cursor.lockState != CursorLockMode.Locked)
            Cursor.lockState = CursorLockMode.Locked;

        // Movement
        float _xMove = Input.GetAxis("Horizontal");
        float _zMove = Input.GetAxis("Vertical");

        // set animation for player
        animator.SetFloat("ForwardVelocity", _zMove);

        // boost when press Left Shift
        if (Input.GetKey(KeyCode.LeftShift))
            speedMove = speedBoost;
        else speedMove = speedBoost / 2;
         
        Vector3 _moveHorizontal = transform.right * _xMove * speedMove;
        Vector3 _moveVertical = transform.forward * _zMove * speedMove;
        Vector3 _moveVelocity = _moveHorizontal + _moveVertical;

        // call and pass argument
        playerMotor.Move(_moveVelocity);

        // Rotation Player around Y axis
        float _yRotate = Input.GetAxisRaw("Mouse X");
        Vector3 _rotation = new Vector3(0, _yRotate, 0) * lookSensitivity;

        playerMotor.Rotate(_rotation);

        // Rotation camera around X axis
        float _XRotate = Input.GetAxisRaw("Mouse Y");
        float _rotationCamera = _XRotate * lookSensitivity;

        playerMotor.RotateCamera(_rotationCamera);

        // jump 
        Vector3 _thrusterForce = Vector3.zero;
        if(Input.GetButton("Jump"))
        {
            // increase this to set UI thrusterBar 
            thrusterFuelAmount += Time.deltaTime;

            _thrusterForce = Vector3.up * thrusterForce * speedMove;
            JointSetting(0);
        }
        else
        {
            thrusterFuelAmount -= 0.4f * Time.deltaTime;
            JointSetting(20);
        }
        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0, 1);

        playerMotor.ThrustJump(_thrusterForce);


        // setting the offset when player is spawned in a higher position, such as : table...
        // first of all : uncheck: Auto configure connected anchor in Inspector
        RaycastHit hit;
        if(Physics.Raycast(transform.position,Vector3.down,out hit,Mathf.Infinity,environmentMask))
        { 
            configurableJoint.targetPosition = new Vector3(0, -hit.point.y, 0);
        }
        else
        {
            configurableJoint.targetPosition = Vector3.zero;
        }
    }

    // set force joint 
    private void JointSetting(float _jointSpring)
    {
        configurableJoint.yDrive = new JointDrive()
        {
            positionSpring = _jointSpring,
            maximumForce = 40
        };
    }

    public float GetThrusterFuelAmount()
    {
        return thrusterFuelAmount;
    } 
}
