using UnityEngine;
using UnityEngine.InputSystem; 


public class PlayerController : MonoBehaviour
{

    private CharacterController _controller;  
    private Animator _animator;               
    private InputAction _moveAction;          
    private InputAction _jumpAction;       
    private Vector2 _moveInputValue;          
    private float _movementSpeed = 12f;       
    private float _gravity = -9.81f;        
    private float _jumpForce = 5f;          
    private Vector3 _playerGravity;           
    private float _turnSmoothVelocity;   
    private Transform _cameraTransform;   

    [SerializeField] private float _smoothTime = 0.05f; 
    [SerializeField] private Transform _sensorPosition; 
    [SerializeField] private float _sensorRadius = 0.5f; 
    [SerializeField] private LayerMask _groundLayer;    
 
    void Awake()
    {
        Debug.Log("Void Awake Inicializada");
        _controller = GetComponent<CharacterController>(); 
        _animator = GetComponentInChildren<Animator>();   
        _moveAction = InputSystem.actions["Move"];
        _jumpAction = InputSystem.actions["Jump"];
        _cameraTransform = Camera.main.transform; 
    }
    
    void Update()
    {
        Debug.Log("Void Update Inicializada");
        _moveInputValue = _moveAction.ReadValue<Vector2>();

        Movement();

        if (_jumpAction.WasPressedThisFrame() && IsGrounded())
        {
            Debug.Log("Void Jump Inicializada En el IF");
            Jump();
        }

        Gravity(); 
        
        _animator.SetBool("IsGrounded", IsGrounded());
    }

    
    void Movement()
    {
        Debug.Log("Void Movement Inicializada.");
        Vector3 direction = new Vector3(_moveInputValue.x, 0, _moveInputValue.y);
       
        if (direction != Vector3.zero)
        {
           
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;

           
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _smoothTime);

            
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

          
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

           
          _controller.Move(moveDirection * _movementSpeed * Time.deltaTime);
        }
    
        _animator.SetFloat("Horizontal", _moveInputValue.x, 0.2f, Time.deltaTime);
        _animator.SetFloat("Vertical", _moveInputValue.y, 0.2f, Time.deltaTime);
    }
   
    void Gravity()
    {
        Debug.Log("Void Gravity Inicializada");
        if (!IsGrounded())
        {
            _playerGravity.y += _gravity * Time.deltaTime;
        }

        
        if (IsGrounded() && _playerGravity.y < 0)
        {
            _playerGravity.y = _gravity;
        }

       
        _controller.Move(_playerGravity * Time.deltaTime);
    }
    
    void Jump()
    {
         Debug.Log("Void Jump Inicializada");
        _playerGravity.y = Mathf.Sqrt(_jumpForce * -2 * _gravity);
    }
    
    bool IsGrounded()
    {
         Debug.Log("Void IsGrounded Inicializada");
        return Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);
    }
}
