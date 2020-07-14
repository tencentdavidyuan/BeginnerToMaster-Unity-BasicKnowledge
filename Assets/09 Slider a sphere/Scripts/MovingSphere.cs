using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere : MonoBehaviour
{
    #region enums
    public enum VelocityMode {
        None = 0,
        NormalizeInputVector,
        ConstrainingInputVector,
        RelativeMovement,
        Velocity,
        Speed,
        Accelerate,
        DesiredVelocity,
        Max,
    }

    public enum ConstrainPosition {
        JerkyPlacement = 0,
        ExactPlacement,
        EliminateSpeed,
        Bouncing,
        Bounciness,
    }
    #endregion

    #region Velocity Varibles
    /// <summary> </summary>
    public VelocityMode _handleMode = VelocityMode.None;
    /// <summary> </summary>
    public ConstrainPosition _constrainPositionMode = ConstrainPosition.JerkyPlacement;

    /// <summary> </summary>
    [Range(1, 10)]
    public float _maxSpeed = 1f;
    /// <summary> </summary>
    public Vector3 _velocity;

    /// <summary> </summary>
    [SerializeField, Range(1, 10)]
    public float _maxAccelerate = 1f;
    #endregion

    #region Area Varibles
    public Rect _allowedArea = new Rect(-10f, -10f, 20f, 20f);
    public Vector3 _newPosition = Vector3.zero;
    [Range(0, 1)]
    public float _bounciness = 0.5f;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector2 playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        switch (_handleMode) {
            case VelocityMode.None: {                    
                    _newPosition = new Vector3(playerInput.x, 0f, playerInput.y);
                }
                break;
            case VelocityMode.NormalizeInputVector: {
                    playerInput.Normalize();
                    _newPosition = new Vector3(playerInput.x, 0f, playerInput.y);
                }
                break;
            case VelocityMode.ConstrainingInputVector: {
                    playerInput = Vector2.ClampMagnitude(playerInput, 1f);
                    _newPosition = new Vector3(playerInput.x, 0f, playerInput.y);
                }
                break;
            case VelocityMode.RelativeMovement: {
                    Vector3 displacement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
                    _newPosition += displacement;
                }
                break;
            case VelocityMode.Velocity: {
                    Vector3 velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                    Vector3 displacement = velocity * Time.deltaTime * 2;
                    _newPosition += displacement;
                }
                break;
            case VelocityMode.Speed: {
                    Vector3 velocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * _maxSpeed;
                    Vector3 displacement = velocity * Time.deltaTime;
                    _newPosition += displacement;
                }
                break;
            case VelocityMode.Accelerate: {
                    Vector3 accelerate = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * _maxSpeed;
                    _velocity += accelerate * Time.deltaTime;
                    Vector3 displacement = _velocity * Time.deltaTime;
                    _newPosition += displacement;
                }
                break;
            case VelocityMode.DesiredVelocity: {
                    Vector3 desiredVelocity = new Vector3(playerInput.x, 0, playerInput.y) * _maxSpeed;
                    float maxSpeedChange = _maxAccelerate * Time.deltaTime;

                    //DesiredVelocity_Version1(desiredVelocity, maxSpeedChange);
                    //DesiredVelocity_Version2(desiredVelocity, maxSpeedChange);
                    DesiredVelocity_Version3(desiredVelocity, maxSpeedChange);

                    Vector3 displacement = _velocity * Time.deltaTime;
                    _newPosition += displacement;
                }
                break;
            case VelocityMode.Max:
            default:
                return;
        }

        switch (_constrainPositionMode) {
            case ConstrainPosition.JerkyPlacement: {
                    if (!_allowedArea.Contains(new Vector2(_newPosition.x, _newPosition.z))) {
                        _newPosition = transform.localPosition;
                    }
                    transform.localPosition = _newPosition;
                }
                break;

            case ConstrainPosition.ExactPlacement: {
                    if (!_allowedArea.Contains(new Vector2(_newPosition.x, _newPosition.z))) {
                        _newPosition.x = Mathf.Clamp(_newPosition.x, _allowedArea.min.x, _allowedArea.max.x);
                        _newPosition.y = Mathf.Clamp(_newPosition.y, _allowedArea.min.y, _allowedArea.max.y);
                    }
                    transform.localPosition = _newPosition;
                }
                break;

            case ConstrainPosition.EliminateSpeed: {
                    if (_newPosition.x < _allowedArea.xMin) {
                        _newPosition.x = _allowedArea.xMin;
                        _velocity.x = 0f;
                    }

                    if (_newPosition.x > _allowedArea.xMax) {
                        _newPosition.x = _allowedArea.xMax;
                        _velocity.x = 0f;
                    }

                    if (_newPosition.z < _allowedArea.yMin) {
                        _newPosition.z = _allowedArea.yMin;
                        _velocity.y = 0f; 
                    }

                    if (_newPosition.z > _allowedArea.yMax) {
                        _newPosition.z = _allowedArea.yMax;
                        _velocity.y = 0f;
                    }

                    transform.localPosition = _newPosition;
                }
                break;

            case ConstrainPosition.Bouncing: {
                    if (_newPosition.x < _allowedArea.xMin) {
                        _newPosition.x = _allowedArea.xMin;
                        _velocity.x *= -1;
                    }
                    if (_newPosition.x > _allowedArea.xMax) {
                        _newPosition.x = _allowedArea.xMax;
                        _velocity.x *= -1;
                    }

                    if (_newPosition.z < _allowedArea.yMin) {
                        _newPosition.z = _allowedArea.yMin;
                        _velocity.z *= -1;
                    }
                    if (_newPosition.z > _allowedArea.yMax) {
                        _newPosition.z = _allowedArea.yMax;
                        _velocity.z *= -1;
                    }

                    transform.localPosition = _newPosition;
                }
                break;

            case ConstrainPosition.Bounciness: {
                    if (_newPosition.x < _allowedArea.xMin) {
                        _newPosition.x = _allowedArea.xMin;
                        _velocity.x *= -_bounciness;
                    }
                    if (_newPosition.x > _allowedArea.xMax) {
                        _newPosition.x = _allowedArea.xMax;
                        _velocity.x *= -_bounciness;
                    }

                    if (_newPosition.z < _allowedArea.yMin) {
                        _newPosition.z = _allowedArea.yMin;
                        _velocity.z *= -_bounciness;
                    }
                    if (_newPosition.z > _allowedArea.yMax) {
                        _newPosition.z = _allowedArea.yMax;
                        _velocity.z *= -_bounciness;
                    }

                    transform.localPosition = _newPosition;
                }
                break;

            default:
                break;
        }


    }

    private void OnGUI() {
    }

    void DesiredVelocity_Version1(Vector3 desiredVelocity, float maxSpeedChange) {
        if (_velocity.x < desiredVelocity.x) {
            _velocity.x += maxSpeedChange;
        }
        if (_velocity.x > desiredVelocity.x) {
            _velocity.x -= maxSpeedChange;
        }

        if (_velocity.z < desiredVelocity.z) {
            _velocity.z += maxSpeedChange;
        }
        if (_velocity.z > desiredVelocity.z) {
            _velocity.z -= maxSpeedChange;
        }
    }

    void DesiredVelocity_Version2(Vector3 desiredVelocity, float maxSpeedChange) {
        if (_velocity.x < desiredVelocity.x) {
            _velocity.x = Mathf.Min(_velocity.x + maxSpeedChange, desiredVelocity.x);
        }
        if (_velocity.x > desiredVelocity.x) {
            _velocity.x = Mathf.Max(_velocity.x - maxSpeedChange, desiredVelocity.x);
        }

        if (_velocity.z < desiredVelocity.z) {
            _velocity.z = Mathf.Min(_velocity.z + maxSpeedChange, desiredVelocity.z);
        }
        if (_velocity.z > desiredVelocity.z) {
            _velocity.z = Mathf.Max(_velocity.z - maxSpeedChange, desiredVelocity.z);
        }
    }

    void DesiredVelocity_Version3(Vector3 desiredVelocity, float maxSpeedChange) {
        _velocity.x = Mathf.MoveTowards(_velocity.x, desiredVelocity.x, maxSpeedChange);
        _velocity.z = Mathf.MoveTowards(_velocity.z, desiredVelocity.z, maxSpeedChange);
    }
}
