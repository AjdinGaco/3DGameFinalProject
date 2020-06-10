using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public GameObject enemyHead;
    public GameObject enemyBullet;
    public GameObject bulletspawnplace;
    public GameObject targeter;
    public string tagToTarget = "Player";
    public float detectionRange = 20f;
    public float dmgBullet = 10f;
    public float speedBullet = 100f;
    public float cooldownShoot = 1f;
    public float aimSpeed = 10f;
    private bool canShoot = true;
    private bool TagWithinRange;

    private GameObject tagTarget;


    public AudioSource shootingEffect;

    private void FixedUpdate()

    {
        CheckForNearbyTag();
    }
    void CheckForNearbyTag()
    {
        GameObject[] plist = GameObject.FindGameObjectsWithTag(tagToTarget);
        TagWithinRange = false;
        foreach (GameObject item in plist)
        {
            if (Vector3.Distance(this.transform.position, item.transform.position) < detectionRange)
            {
                TagWithinRange = true;
                tagTarget = item;
            }
        }
    }
    void AimAtTag()
    {
        if (tagTarget != null)
        {
            targeter.transform.LookAt(tagTarget.transform.position);
            float step = aimSpeed * Time.deltaTime;

            enemyHead.transform.rotation = Quaternion.RotateTowards(enemyHead.transform.rotation, targeter.transform.rotation, step);
        }
    }
    bool CanHitTag()
    {
        RaycastHit hit;
        if (Physics.Raycast(enemyHead.transform.position, enemyHead.transform.forward, out hit, detectionRange * 1.2f))
        {
            if (hit.transform.tag == tagToTarget)
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
    // Update is called once per frame
    void Shoot()
    {
        GameObject Shot = (GameObject)Instantiate(enemyBullet, bulletspawnplace.transform.position, bulletspawnplace.transform.rotation);
        //Here i will adjust the dmg the shot does based on the charge
        Damage shotdamage = Shot.GetComponent<Damage>();
        BulletLogic shotlogic = Shot.GetComponent<BulletLogic>();
        shotdamage.damageAmount = dmgBullet;
        shotlogic.m_Speed = speedBullet;
        canShoot = false;
        cooldowntimer = cooldownShoot;

        shootingEffect.GetComponent<AudioSource>().Play();
    }
    private float cooldowntimer;
    void ShootCooldown()
    {
        cooldowntimer -= Time.deltaTime;
        if (cooldowntimer < 0)
        {
            canShoot = true;
        }
    }
    void Update()
    {
        if (TagWithinRange)
        {
            AimAtTag();
        }
        if (CanHitTag() && canShoot)
        {
            Shoot();
        }
        if (!canShoot)
        {
            ShootCooldown();
        }

    }
}
