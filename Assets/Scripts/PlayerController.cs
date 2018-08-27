using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region GlobalVariables

    [SerializeField]
    private float _MoveSpeed = 5f;

    [SerializeField]
    private float _SprintSpeedMult = 5f;

    [SerializeField]
    private float _JumpSpeed = 12f;

    [SerializeField]
    private Transform _CameraPivot;

    private Rigidbody _RB;
    private Animator _Anim;
    private Transform _CameraTransform;

    private float _XInput = 0f;
    private float _ZInput = 0f;
    private float _SpeedMult = 5f;

    private bool _JumpPressed = false;


    #endregion GlobalVariables


    #region UnityFunctions

    private void Awake()
    {
        _RB = GetComponent<Rigidbody>();
        _Anim = GetComponentInChildren<Animator>();
        _CameraTransform = _CameraPivot.GetComponentInChildren<Camera>().transform;
    }

    private void Update()
    {

		Cursor.lockState = CursorLockMode.Locked;

        ReadMoveInputs();
        CameraRoations();
        CameraZoom();
        SetAnimValues();
    }


    private void FixedUpdate()
    {
        ApplyMovmentPhysics();
    }


    #endregion Unityfunctions

    #region ClassFunctions

    private void ReadMoveInputs()
    {
        _XInput = Input.GetAxis("Horizontal");
        _ZInput = Input.GetAxis("Vertical");
        _SpeedMult = Input.GetKey(KeyCode.LeftShift) ? _SprintSpeedMult : 1f;
        _JumpPressed |= Input.GetKeyDown(KeyCode.Space);
    }

    private void ApplyMovmentPhysics()
    {
        Vector3 newVel = new Vector3(_XInput, 0, _ZInput) * _MoveSpeed * _SpeedMult;
        newVel.y = _JumpPressed ? _JumpSpeed : _RB.velocity.y;

        _RB.velocity = transform.TransformVector(newVel);

		if(_JumpPressed)
		{
			_Anim.SetTrigger("JumpTrigger");
		}

        _JumpPressed = false;
    }

    private void CameraRoations()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(0f, mouseX, 0f);
        _CameraPivot.Rotate(-mouseY, 0f, 0f);
    }

    private void CameraZoom()
    {
        Vector3 newZoom = _CameraTransform.localPosition;
        newZoom.z += Input.mouseScrollDelta.y;
        newZoom.z = Mathf.Clamp(newZoom.z, -8, 0);
        _CameraTransform.localPosition = newZoom;
    }

    private void SetAnimValues()
    {
		_Anim.SetFloat("HorizontalMovement", _XInput);
		_Anim.SetFloat("VerticalMovement", _ZInput);
		_Anim.SetFloat("SpeedMult", _SpeedMult);
    }


    #endregion ClassFunctions



}
