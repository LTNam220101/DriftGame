using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopController : MonoBehaviour
{
    // Variables
    private Vector3 MoveForce;

    public WheelColliders colliders;
    public WheelMeshes wheelMeshes;
    public TrailsRenderer trails;
    public ParticlesSystem particles;
    public GameObject Player;
    public GameObject ExplodeEffect;

    public TimerController timer;
    // Settings
    public float MoveSpeed = 40;
    public float MaxSpeed = 30;
    public float SteerAngle = 8;
    private float Traction = 1;
    
    public float steeringInput;
    private float currentAxisValue;

    [Tooltip("The width of the obstacle detection area for this AI car")]
    public float detectAngle = 5;

    [Tooltip("The forward distance of the obstacle detection area for this AI car")]
    public float detectDistance = 20;

    [Tooltip("Make AI cars try to avoid obstacles. Obstacle are objects that have the ECCObstacle component attached to them")]
    public bool avoidObstacles = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Player != null)
        {
            MoveForce += transform.forward * MoveSpeed * Time.deltaTime;
            transform.position += MoveForce * Time.deltaTime;

            // Cast two raycasts to either side of the AI car so that we can detect obstacles
            Ray rayRight = new Ray(transform.position + Vector3.up * 0.2f + transform.right * detectAngle * 0.5f + transform.right * detectAngle * 0.0f * Mathf.Sin(Time.time * 50), transform.forward * detectDistance);
            Ray rayLeft = new Ray(transform.position + Vector3.up * 0.2f + transform.right * -detectAngle * 0.5f - transform.right * detectAngle * 0.0f * Mathf.Sin(Time.time * 50), transform.forward * detectDistance);
            Debug.DrawRay(transform.position + Vector3.up * 0.2f + transform.right * detectAngle * 0.5f + transform.right * detectAngle * 0.0f * Mathf.Sin(Time.time * 50), transform.forward * detectDistance, Color.red);
            Debug.DrawRay(transform.position + Vector3.up * 0.2f + transform.right * -detectAngle * 0.5f - transform.right * detectAngle * 0.0f * Mathf.Sin(Time.time * 50), transform.forward * detectDistance, Color.blue);
            RaycastHit rightHit;
            RaycastHit leftHit;
            bool isHitRight = Physics.Raycast(rayRight, out rightHit, detectDistance);
            bool isHitLeft = Physics.Raycast(rayLeft, out leftHit, detectDistance);
            
            if ( avoidObstacles == true && isHitRight && isHitLeft && (rightHit.transform.tag == "Tree" || rightHit.transform.tag == "Cop")) {
                CheckInput(-1);
            }
            else if ( avoidObstacles == true && isHitLeft && (leftHit.transform.tag == "Tree" || leftHit.transform.tag == "Cop")) {
                CheckInput(1);
            }
            else {
                float angle = Vector3.SignedAngle(transform.forward * 3, Player.transform.position - transform.position, Vector3.up);
                if(angle > 15.0f) {
                    CheckInput(1);
                }
                else if (angle < -15.0f) {
                    CheckInput(-1);
                }else {
                    CheckInput(0);
                }
            }

            if (MaxSpeed > 0) {
                transform.Rotate(Vector3.up * steeringInput * MoveForce.magnitude * SteerAngle * Time.deltaTime);
                transform.eulerAngles = new Vector3(
                                    transform.eulerAngles.x,
                                    transform.eulerAngles.y,
                                    10.0f * steeringInput
                                );
            } else {
                transform.eulerAngles = new Vector3(
                                    transform.eulerAngles.x,
                                    transform.eulerAngles.y,
                                    0f
                                );
            }

            colliders.FRWheel.steerAngle = steeringInput * SteerAngle;
            colliders.FLWheel.steerAngle = steeringInput * SteerAngle;
            MoveForce = Vector3.ClampMagnitude(MoveForce, MaxSpeed);

            MoveForce = Vector3.Lerp(MoveForce.normalized, transform.forward, Traction * Time.deltaTime) * MoveForce.magnitude;
        
            CheckParticles();
            ApplyWheelPositions();
        }
        else
        {
            SteerAngle = 0;
            MoveForce += transform.forward * 1 * Time.deltaTime;
            transform.position += MoveForce * Time.deltaTime;
            trails.RLWheel.emitting = false;
            trails.RRWheel.emitting = false;
            particles.RLWheel.Stop();
            particles.RRWheel.Stop();
        }
    }

    void CheckParticles() {
        if (steeringInput != 0f && MaxSpeed > 0)
        {
            trails.RLWheel.emitting = true;
            trails.RRWheel.emitting = true;

            particles.RLWheel.Play();
            particles.RRWheel.Play();
        }
        else
        {
            trails.RLWheel.emitting = false;
            trails.RRWheel.emitting = false;

            particles.RLWheel.Stop();
            particles.RRWheel.Stop();
        }
    }
    void CheckInput(float currentAxisValue)
    {
        steeringInput = Mathf.Lerp(steeringInput, currentAxisValue, 0.25f);
        if(steeringInput >= -0.1f && steeringInput <= 0.1f) steeringInput = 0.0f;
        MaxSpeed = 28 - 8 * Mathf.Abs(steeringInput);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Cop" || other.gameObject.tag == "Tree" || other.gameObject.tag == "Player")
        {
            Explode(); 
        }
    }
    void OnTriggerEnter(Collider collisionInfo)
    {
        if(collisionInfo.gameObject.tag == "Ice"){	
            Traction = 0.01f;
        }
        if (collisionInfo.gameObject.tag == "Terrain")
        {
            Traction = 1f;
        }
    }
    void OnTriggerExit(Collider collisionInfo)
    {
        // if(collisionInfo.gameObject.tag == "Ice"){
            // Traction = 1f;
            // MaxSpeed = 30;
        // }
        if(collisionInfo.gameObject.tag == "Terrain")
        {
            MaxSpeed = 0;
        }
    }

    void Explode()
    {
        MaxSpeed = 0;
        MoveSpeed = 0;
        SteerAngle = 0;
        GameObject explodeEffect = Instantiate(ExplodeEffect, transform.position, transform.rotation);
        Destructable dest = gameObject.GetComponent<Destructable>();
        if (dest != null)
        {
            dest.DestroyObject();
            Destroy(explodeEffect, 2);
        }
    }

    void ApplyWheelPositions()
    {
        UpdateWheel(colliders.FRWheel, wheelMeshes.FRWheel);
        UpdateWheel(colliders.FLWheel, wheelMeshes.FLWheel);
        UpdateWheel(colliders.RRWheel, wheelMeshes.RRWheel);
        UpdateWheel(colliders.RLWheel, wheelMeshes.RLWheel);
    }
    void UpdateWheel(WheelCollider coll, MeshRenderer wheelMesh)
    {
        Quaternion quat;
        Vector3 position;
        coll.GetWorldPose(out position, out quat);
        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = quat;
    }
    public float ChaseAngle(Vector3 forward, Vector3 targetDirection, Vector3 up)
    {
        // Calculate the approach angle
        float approachAngle = Vector3.Dot(Vector3.Cross(up, forward), targetDirection);
        
        // If the angle is higher than 0, we approach from the left ( so we must rotate right )
        if (approachAngle > 0f)
        {
            return 1f;
        }
        else if (approachAngle < 0f) //Otherwise, if the angle is lower than 0, we approach from the right ( so we must rotate left )
        {
            return -1f;
        }
        else // Otherwise, we are within the angle range so we don't need to rotate
        {
            return 0f;
        }
    }
}

[System.Serializable]
public class TrailsRenderer
{
    public TrailRenderer RRWheel;
    public TrailRenderer RLWheel;
}

[System.Serializable]
public class ParticlesSystem
{
    public ParticleSystem RRWheel;
    public ParticleSystem RLWheel;
}