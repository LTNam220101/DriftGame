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
    public float MaxSpeed = 25;
    public float SteerAngle = 8;
    private float Traction = 1;

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
            // float angle = Vector3.SignedAngle(transform.forward * 3, Player.transform.position - transform.position, Vector3.up);
            // if(angle > 15.0f)
            // {
            //     if (MaxSpeed > 0)
            //     {
            //     transform.Rotate(Vector3.up * MoveForce.magnitude * SteerAngle * Time.deltaTime);
            //     transform.eulerAngles = new Vector3(
            //                         transform.eulerAngles.x,
            //                         transform.eulerAngles.y,
            //                         10.0f
            //                     );
            //     }
            //     colliders.FRWheel.steerAngle = SteerAngle;
            //     colliders.FLWheel.steerAngle = SteerAngle;
            // }else if (angle < -15.0f)
            // {
            //     if (MaxSpeed > 0)
            //     {
            //     transform.Rotate(-Vector3.up * MoveForce.magnitude * SteerAngle * Time.deltaTime);
            //     transform.eulerAngles = new Vector3(
            //                         transform.eulerAngles.x,
            //                         transform.eulerAngles.y,
            //                         -10.0f
            //                     );
            //     }
            //     colliders.FRWheel.steerAngle = -SteerAngle;
            //     colliders.FLWheel.steerAngle = -SteerAngle;
            // }else {
            //     transform.eulerAngles = new Vector3(
            //                         transform.eulerAngles.x,
            //                         transform.eulerAngles.y,
            //                         0f
            //                     );
            // }

            // Shoot a ray at the position to see if we hit something
            // Ray ray = new Ray(transform.position + Vector3.up * 0.2f + transform.right * Mathf.Sin(Time.time * 20) * detectAngle, transform.TransformDirection(Vector3.forward) * detectDistance);

            // Cast two raycasts to either side of the AI car so that we can detect obstacles
            Ray rayRight = new Ray(transform.position + Vector3.up * 0.2f + transform.right * detectAngle * 0.5f + transform.right * detectAngle * 0.0f * Mathf.Sin(Time.time * 50), transform.forward * detectDistance);
            Ray rayLeft = new Ray(transform.position + Vector3.up * 0.2f + transform.right * -detectAngle * 0.5f - transform.right * detectAngle * 0.0f * Mathf.Sin(Time.time * 50), transform.forward * detectDistance);
            Debug.DrawRay(transform.position + Vector3.up * 0.2f + transform.right * detectAngle * 0.5f + transform.right * detectAngle * 0.0f * Mathf.Sin(Time.time * 50), transform.forward * detectDistance, Color.red);
            Debug.DrawRay(transform.position + Vector3.up * 0.2f + transform.right * -detectAngle * 0.5f - transform.right * detectAngle * 0.0f * Mathf.Sin(Time.time * 50), transform.forward * detectDistance, Color.blue);
            RaycastHit hit;

            // If we detect an obstacle on our right side, swerve to the left
            if ( avoidObstacles == true && Physics.Raycast(rayRight, out hit, detectDistance) && (hit.transform.GetComponent<TreeController>() || (hit.transform.GetComponent<CopController>())) )
            {
                // Change the emission color of the obstacle to indicate that the car detected it
                // if (hit.transform.GetComponent<MeshRenderer>() ) hit.transform.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.red);
                // Rotate left to avoid obstacle
                if (MaxSpeed > 0)
                {
                transform.Rotate(-Vector3.up * MoveForce.magnitude * SteerAngle * Time.deltaTime);
                transform.eulerAngles = new Vector3(
                                    transform.eulerAngles.x,
                                    transform.eulerAngles.y,
                                    -10.0f
                                );
                }
                colliders.FRWheel.steerAngle = SteerAngle;
                colliders.FLWheel.steerAngle = SteerAngle;
                //obstacleDetected = 0.1f;
            }
            else if ( avoidObstacles == true && Physics.Raycast(rayLeft, out hit, detectDistance) && (hit.transform.GetComponent<TreeController>() || (hit.transform.GetComponent<CopController>()))) // Otherwise, if we detect an obstacle on our left side, swerve to the right
            {
                // Change the emission color of the obstacle to indicate that the car detected it
                //if (hit.transform.GetComponent<MeshRenderer>()) hit.transform.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.red);
                // Rotate right to avoid obstacle
                if (MaxSpeed > 0)
                {
                transform.Rotate(Vector3.up * MoveForce.magnitude * SteerAngle * Time.deltaTime);
                transform.eulerAngles = new Vector3(
                                    transform.eulerAngles.x,
                                    transform.eulerAngles.y,
                                    10.0f
                                );
                }
                colliders.FRWheel.steerAngle = -SteerAngle;
                colliders.FLWheel.steerAngle = -SteerAngle;
                //obstacleDetected = 0.1f;
            }
            else// if (obstacleDetected <= 0) // Otherwise, if no obstacle is detected, keep chasing the player normally
            {
            float angle = Vector3.SignedAngle(transform.forward * 3, Player.transform.position - transform.position, Vector3.up);
            if(angle > 15.0f)
            {
                if (MaxSpeed > 0)
                {
                transform.Rotate(Vector3.up * MoveForce.magnitude * SteerAngle * Time.deltaTime);
                transform.eulerAngles = new Vector3(
                                    transform.eulerAngles.x,
                                    transform.eulerAngles.y,
                                    10.0f
                                );
                }
                colliders.FRWheel.steerAngle = SteerAngle;
                colliders.FLWheel.steerAngle = SteerAngle;
            }else if (angle < -15.0f)
            {
                if (MaxSpeed > 0)
                {
                transform.Rotate(-Vector3.up * MoveForce.magnitude * SteerAngle * Time.deltaTime);
                transform.eulerAngles = new Vector3(
                                    transform.eulerAngles.x,
                                    transform.eulerAngles.y,
                                    -10.0f
                                );
                }
                colliders.FRWheel.steerAngle = -SteerAngle;
                colliders.FLWheel.steerAngle = -SteerAngle;
            }else {
                transform.eulerAngles = new Vector3(
                                    transform.eulerAngles.x,
                                    transform.eulerAngles.y,
                                    0f
                                );
            }
            }
//                 // Rotate the car until it reaches the desired chase angle from either side of the player
//                 if (Vector3.Angle(transform.forward, Player.transform.position - transform.position) > 30)
//                 {
// Debug.Log(ChaseAngle(transform.forward, Player.transform.position - transform.position, Vector3.up));
//                     // transform.Rotate(-Vector3.up * MoveForce.magnitude * SteerAngle * Time.deltaTime);
//                     transform.Rotate(transform.eulerAngles.x,
//                                     transform.eulerAngles.y,ChaseAngle(transform.forward, Player.transform.position - transform.position, Vector3.up));
//                 }
//                 else // Otherwise, stop rotating
//                 {
//                     // Rotate(0);
//                 }
            // }

            MoveForce = Vector3.ClampMagnitude(MoveForce, MaxSpeed);

            MoveForce = Vector3.Lerp(MoveForce.normalized, transform.forward, Traction * Time.deltaTime) * MoveForce.magnitude;
        
            // if (angle != 0f && MaxSpeed > 0)
            // {
            //     trails.RLWheel.emitting = true;
            //     trails.RRWheel.emitting = true;

            //     particles.RLWheel.Play();
            //     particles.RRWheel.Play();
            // }
            // else
            // {
            //     trails.RLWheel.emitting = false;
            //     trails.RRWheel.emitting = false;

            //     particles.RLWheel.Stop();
            //     particles.RRWheel.Stop();
            // }
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

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Cop" || other.gameObject.tag == "Tree" || other.gameObject.tag == "Player")
        {
            Explode(); 
        }
    }
    void OnTriggerStay(Collider collisionInfo)
    {
        if(collisionInfo.gameObject.tag == "Ice"){	
            Traction = 0.01f;
        }
        // if (collisionInfo.gameObject.tag == "Terrain")
        // {
        //     MaxSpeed = 25;
        // }
    }
    void OnTriggerExit(Collider collisionInfo)
    {
        if(collisionInfo.gameObject.tag == "Ice"){
            Traction = 1f;
            // MaxSpeed = 25;
        }
        // if(collisionInfo.gameObject.tag == "Terrain")
        // {
        //     MaxSpeed = 0;
        // }
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