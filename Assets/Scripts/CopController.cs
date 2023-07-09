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
    public float MaxSpeed = 20;
    public float SteerAngle = 8;
    private float Traction = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        MaxSpeed += MaxSpeed*0.2f*Time.deltaTime/20;
        if(Player != null)
        {
            MoveForce += transform.forward * MoveSpeed * Time.deltaTime;
            transform.position += MoveForce * Time.deltaTime;
            float angle = Vector3.SignedAngle(transform.forward * 3, Player.transform.position - transform.position, Vector3.up);
            if(angle > 5.0f)
            {
                transform.Rotate(Vector3.up * MoveForce.magnitude * SteerAngle * Time.deltaTime);
                colliders.FRWheel.steerAngle = SteerAngle;
                colliders.FLWheel.steerAngle = SteerAngle;
            }else if (angle < -5.0f)
            {
                transform.Rotate(-Vector3.up * MoveForce.magnitude * SteerAngle * Time.deltaTime);
                colliders.FRWheel.steerAngle = -SteerAngle;
                colliders.FLWheel.steerAngle = -SteerAngle;
            }

            MoveForce = Vector3.ClampMagnitude(MoveForce, MaxSpeed);

            MoveForce = Vector3.Lerp(MoveForce.normalized, transform.forward, Traction * Time.deltaTime) * MoveForce.magnitude;
        
            if (angle != 0f)
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
    void OnCollisionStay(Collision collisionInfo)
    {
        if(collisionInfo.gameObject.tag == "Ice"){	
            Traction = 0.01f;
		}
    }
    void OnCollisionExit(Collision collisionInfo)
    {
        if(collisionInfo.gameObject.tag == "Ice"){
            Traction = 1f;
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