using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    public GameObject ExplodeEffect;
    private Rigidbody playerRB;
    public WheelColliders colliders;
    public WheelMeshes wheelMeshes;
    public WheelParticles wheelParticles;
    public WheelTrails wheelTrails;
    public float motorPower = 200;
    private float speed = 40;
    public float steeringAngle = 8;

    public float MaxSpeed = 30;
    public float Traction = 1;

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
        //MaxSpeed += MaxSpeed*0.2f*Time.deltaTime/20;
    }

    void CheckInput()
    {
        currentAxisValue = playerControls.ReadValue<float>();
        // steeringInput = Input.GetAxis("Horizontal");
        if(steeringInput >= -0.1f && steeringInput <= 0.1f) steeringInput = 0.0f;
        if (rightButton.isPressed)
        {
            currentAxisValue = 1;
        }
        if (leftButton.isPressed)
        {
            currentAxisValue =-1;
        }
        steeringInput = Mathf.Lerp(steeringInput, currentAxisValue, 0.25f);
        MaxSpeed = 28 -8 * Mathf.Abs(steeringInput);
    }
    void ApplyMotor() {

        colliders.RRWheel.motorTorque = motorPower;
        colliders.RLWheel.motorTorque = motorPower;
        MoveForce += transform.forward * speed * Time.deltaTime;
        transform.position += MoveForce * Time.deltaTime;
        MoveForce = Vector3.ClampMagnitude(MoveForce, MaxSpeed);
        MoveForce = Vector3.Lerp(MoveForce.normalized, transform.forward, Traction * Time.deltaTime) * MoveForce.magnitude;
    }
    void ApplySteering()
    {
        colliders.FRWheel.steerAngle = steeringInput * 25;
        colliders.FLWheel.steerAngle = steeringInput * 25;
        if (MaxSpeed > 0)
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
        if(steeringInput != 0f && MaxSpeed > 0)
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
        if(other.gameObject.tag == "Tree" || other.gameObject.tag == "Cop")
        {
	        timer.EndTimer();
            Explode();
        }
    }
    void OnTriggerEnter(Collider collisionInfo)
    {
        if(collisionInfo.gameObject.tag == "Ice"){	
            Traction = 0.01f;
            MaxSpeed = 30;
        }
        if (collisionInfo.gameObject.tag == "Terrain")
        {
            MaxSpeed = 30;
        }
    }
    void OnTriggerExit(Collider collisionInfo)
    {
        if(collisionInfo.gameObject.tag == "Ice"){
            Traction = 1f;
            MaxSpeed = 30;
        }
        if(collisionInfo.gameObject.tag == "Terrain")
        {
            MaxSpeed = 0;
        }
    }
    void Explode()
    {
        MaxSpeed = 0;
        speed = 0;
        GameObject explodeEffect = Instantiate(ExplodeEffect, transform.position, transform.rotation);
        Destructable dest = gameObject.GetComponent<Destructable>();
        if(dest != null)
        {
            dest.DestroyObject();
            Destroy(explodeEffect, 2);
        }

        var rbs = GetComponentsInChildren<Rigidbody>();
        // Debug.Log(rbs);
        foreach (var rb in rbs)
        {
            rb.AddExplosionForce(100000, transform.position + transform.forward * 10, 10);
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