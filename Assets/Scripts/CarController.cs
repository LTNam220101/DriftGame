using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    public GameObject ExplodeEffect;
    public GameObject AmokEffect;
    private Rigidbody playerRB;
    public WheelColliders colliders;
    public WheelMeshes wheelMeshes;
    public WheelParticles wheelParticles;
    public WheelTrails wheelTrails;
    public float motorPower = 200;
    public float steeringAngle = 8;

    public float MaxSpeed = 30;
    public float MinSpeed = 30;
    public float speed = 40;
    public float Traction = 1;

    public bool canControl = true;

    public float steeringInput;
    private float currentAxisValue;

    private Vector3 MoveForce;

    public MyButton leftButton;
    public MyButton rightButton;

    public TimerController timer;

    public InputAction playerControls;

    private void OnEnable(){
        playerControls.Enable();
    }
    private void OnDisable(){
        playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody>();
        Time.timeScale = 1f;
        timer.BeginTimer();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        ApplyMotor();
        ApplySteering();
        CheckParticles();
        ApplyWheelPositions();
        CheckDistanceToTerrain();
        //MaxSpeed += MaxSpeed*0.2f*Time.deltaTime/20;
    }

    void CheckInput()
    {
        if(steeringInput >= -0.1f && steeringInput <= 0.1f) steeringInput = 0.0f;
        if(canControl){
            currentAxisValue = playerControls.ReadValue<float>();
            if (rightButton.isPressed)
            {
                currentAxisValue = 1;
            }
            if (leftButton.isPressed)
            {
                currentAxisValue =-1;
            }
        }
        if(steeringInput < currentAxisValue){
            steeringInput = Mathf.Lerp(steeringInput, currentAxisValue, 0.125f);
        }
        else {
            steeringInput = Mathf.Lerp(steeringInput, currentAxisValue, 0.125f);
        }
        speed = MaxSpeed - (MaxSpeed - MinSpeed) * Mathf.Abs(steeringInput);
    }
    void ApplyMotor() {

        colliders.RRWheel.motorTorque = motorPower;
        colliders.RLWheel.motorTorque = motorPower;
        MoveForce += transform.forward * speed * Time.deltaTime;
        transform.position += MoveForce * Time.deltaTime;
        MoveForce = Vector3.ClampMagnitude(MoveForce, speed);
        MoveForce = Vector3.Lerp(MoveForce.normalized, transform.forward, Traction * Time.deltaTime) * MoveForce.magnitude;
    }
    void ApplySteering()
    {
        colliders.FRWheel.steerAngle = steeringInput * 60;
        colliders.FLWheel.steerAngle = steeringInput * 60;
        if (speed > 0)
        {
        float a = steeringInput > 0 ? 1 : -1;
        transform.Rotate(Vector3.up * steeringInput * steeringInput * a * MoveForce.magnitude * steeringAngle * Time.deltaTime);
        }
        transform.eulerAngles = new Vector3(
                                    transform.eulerAngles.x,
                                    transform.eulerAngles.y,
                                    10.0f * steeringInput
                                );
    }

    void ApplyWheelPositions()
    {
        UpdateWheel(colliders.FRWheel, wheelMeshes.FRWheel);
        UpdateWheel(colliders.FLWheel, wheelMeshes.FLWheel);
        UpdateWheel(colliders.RRWheel, wheelMeshes.RRWheel);
        UpdateWheel(colliders.RLWheel, wheelMeshes.RLWheel);
    }
    void CheckParticles() {
        if(steeringInput != 0f && speed > 0)
        {
            wheelParticles.RRWheel.Play();
            wheelTrails.RRWheel.emitting = true;
            wheelParticles.RLWheel.Play();
            wheelTrails.RLWheel.emitting = true;
        }
        else
        {
            wheelParticles.RRWheel.Stop();
            wheelTrails.RRWheel.emitting = false;
            wheelParticles.RLWheel.Stop();
            wheelTrails.RLWheel.emitting = false;
        }

    }
    void UpdateWheel(WheelCollider coll, MeshRenderer wheelMesh)
    {
        Quaternion quat;
        Vector3 position;
        coll.GetWorldPose(out position, out quat);
        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = quat;
    }

    void OnCollisionEnter(Collision other)
    {
        if(gameObject.tag == "Player"){
            if(other.gameObject.tag == "Tree" || other.gameObject.tag == "Cop")
            {
                ContactPoint contact = other.contacts[0];
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 position = contact.point;
                Explode(position, rotation);
            }
        }
    }

    private void Explode(Vector3 position, Quaternion rotation)
    {
        speed = 0;
        timer.EndTimer();
        GameObject explodeEffect = Instantiate(ExplodeEffect, position, rotation);
        bool disableSound = PlayerPrefs.GetInt("disableSound") == 1 ? true : false;
        explodeEffect.GetComponent<AudioSource>().mute = disableSound;
        Destructable dest = gameObject.GetComponent<Destructable>();
        if(dest != null)
        {
            dest.DestroyObject(false, -1, position);
            Destroy(explodeEffect, 2);
        }
    }

    private void CheckDistanceToTerrain()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit) && hit.collider.CompareTag("Terrain"))
        {
            if(hit.distance > 10f){
                Explode(transform.position, transform.rotation);
            }
        }
    }
}
[System.Serializable]
public class WheelColliders
{
    public WheelCollider FRWheel;
    public WheelCollider FLWheel;
    public WheelCollider RRWheel;
    public WheelCollider RLWheel;
}
[System.Serializable]
public class WheelMeshes
{
    public MeshRenderer FRWheel;
    public MeshRenderer FLWheel;
    public MeshRenderer RRWheel;
    public MeshRenderer RLWheel;
}
[System.Serializable]
public class WheelParticles{
    public ParticleSystem RRWheel;
    public ParticleSystem RLWheel;
}
[System.Serializable]
public class WheelTrails{
    public TrailRenderer RRWheel;
    public TrailRenderer RLWheel;
}