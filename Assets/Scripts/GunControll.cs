using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public class GunControll : MonoBehaviour
{
    //This gun has both the functions of an grapling and and chargeable gun :D
    public GameObject playerCam;
    public GameObject playerBody;
    public GameObject playerGun; // For turning it and stuff
    public float maxGrapleDis = 10f;
    public string tagForGrapplable = "AbleToGrapTo";

    public Transform gunPoint;
    private SpringJoint grapjoint;

    public LineRenderer grapRope;
    private Vector3 grappleEndpoint;

    //Gun Setttings
    private float charge = 0f;
    public float chargeSpeed = 1f;
    public float chargeMax = 10f;
    public float chargeStartScale = 1f;
    public float chargeToScaleRation = 1f;
    public float chargeToDamage = 1f;
    public float chargedShotSpeed = 10;
    public float chargeToKnockBackValue = 100f;
    public GameObject projectile;

    public GameObject graplingHitPrefab;
    public AudioSource chargingSoundEffect;
    public AudioSource shootingSoundEffect;

    public bool canGrapple;


    private bool fireGrapple = false, chargeShot = false;
    public float GetChargePercentage()
    {
        return (charge / chargeMax);
    }
    void Update()
    {
        canGrapple = CheckIfCanGrapple();
        MouseInput();
        GunControl();
        DrawRope();
    }
    void GunControl()
    {
        if (fireGrapple)
            GraplingFire();
        else
        {
            ResetLookGun();
            GraplingStop();
        }
        if (chargeShot)
            ChargeShot();
        else if (charge > 0) // That means there is a shot ready to fire
        {
            ShootChargedShot();
        }


    }

    private void ChargeShot()
    {
        if (charge == 0)
        {
            chargingSoundEffect.GetComponent<AudioSource>().Play();
        }
        if (charge < chargeMax)
        {
            charge += chargeSpeed * Time.deltaTime;
        }
        else
        {
            chargingSoundEffect.GetComponent<AudioSource>().Stop();
        }
            
        
    }
    private void ShootChargedShot()
    {
        GameObject Shot = (GameObject)Instantiate(projectile, gunPoint.position, gunPoint.rotation);
        //Here i will adjust the dmg the shot does based on the charge

        Damage shotdamage = Shot.GetComponent<Damage>();
        BulletLogic shotlogic = Shot.GetComponent<BulletLogic>();
        shotdamage.damageAmount = charge * chargeToDamage;
        Shot.transform.localScale = Shot.transform.localScale * (chargeStartScale + charge) * chargeToScaleRation;
        shotlogic.m_Speed = chargedShotSpeed;

        playerBody.GetComponent<PlayerController>().AddForce(-playerCam.transform.forward * charge * chargeToKnockBackValue); // KnockBaco

        chargingSoundEffect.GetComponent<AudioSource>().Stop();
        shootingSoundEffect.GetComponent<AudioSource>().Play();
        charge = 0f;

    }

    public bool CheckIfCanGrapple() // This is mostly meant for the Hud
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, maxGrapleDis))
        {
            if (hit.transform.gameObject.tag == tagForGrapplable && !playerBody.GetComponent<SpringJoint>())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private RaycastHit lastHit;
    void GraplingFire()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, maxGrapleDis))
        {
            if (hit.transform.gameObject.tag == tagForGrapplable && !playerBody.GetComponent<SpringJoint>())
            {
                lastHit = hit;
                grappleEndpoint = hit.point;
                grapjoint = playerBody.gameObject.AddComponent<SpringJoint>();
                grapjoint.autoConfigureConnectedAnchor = false;
                grapjoint.connectedAnchor = hit.point;
                float distance = Vector3.Distance(playerBody.transform.position, hit.point);
                grapjoint.maxDistance = distance * 0.8f;
                grapjoint.minDistance = 0;

                grapjoint.spring = 1f;
                grapjoint.damper = 5f;
                grapjoint.massScale = 4.5f;

                //Enable the rope thing to work
                grapRope.positionCount = 2;

                // Hit Effect
                Instantiate(graplingHitPrefab, gunPoint.position, gunPoint.rotation);

            }
            else if (playerBody.GetComponent<SpringJoint>())
            {
                //Lenght Adjust so it feels a bit better
                float distance = Vector3.Distance(playerBody.transform.position, lastHit.point);
                if (distance < grapjoint.maxDistance)
                {
                    grapjoint.maxDistance = distance * 0.9f;
                }
                grapjoint.minDistance = 0;
            }
        }
    }
    void GraplingStop()
    {
        if (playerBody.GetComponent<SpringJoint>())
        {
            grapRope.positionCount = 0;
            Destroy(grapjoint);
        }


    }
    void ResetLookGun()
    {
        playerGun.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private Vector3 grappledPos;

    void DrawRope()
    {
        if (grapjoint && grapRope.positionCount > 0)
        {
            grappledPos = Vector3.Lerp(grappledPos, grappleEndpoint, Time.deltaTime * 8f);
            grapRope.SetPosition(0, gunPoint.position);
            grapRope.SetPosition(1, grappledPos);
        }
    }
    void MouseInput()
    {
        if (Input.GetMouseButton(0))
            fireGrapple = true;
        else
            fireGrapple = false;
        if (Input.GetMouseButton(1))
            chargeShot = true;
        else
            chargeShot = false;
    }
}
