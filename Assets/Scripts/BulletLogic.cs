using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    public float m_Speed = 10f;   // this is the projectile's speed
    public float m_Lifespan = 3f; // this is the projectile's lifespan (in seconds)
    public float accel = 1f;
    public float StormtrooperVariable = 0;



    private Rigidbody m_Rigidbody;
    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        Vector3 posoffset = new Vector3(Random.Range(-StormtrooperVariable, StormtrooperVariable), 0, Random.Range(-StormtrooperVariable, StormtrooperVariable));
        m_Rigidbody.AddForce((m_Rigidbody.transform.forward + posoffset) * m_Speed);
        if (gameObject.name.Contains("(Clone)"))
        {
            //If this is a clone then it must DIE!
            Destroy(gameObject, m_Lifespan);
        }
        else
        {

        }
    }
    private void Update()
    {
        m_Rigidbody.AddForce(m_Rigidbody.transform.forward * accel * Time.deltaTime);
    }
}
