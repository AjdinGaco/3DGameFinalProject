using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleControll : MonoBehaviour
{

    // This is mostly for controlling the particles of the player, depending on varius things. This is only meant o make it looks a bit fancier :D

    private ParticleSystem playerParticles;
    private PlayerController playerControl;
    private void Start()
    {
        playerParticles = this.GetComponent<ParticleSystem>();
        playerControl = this.GetComponent<PlayerController>();
    }

    //= playerControl.GetVelocity().x + playerControl.GetVelocity().y;
    void Update()
    {
        if (!playerControl.grouded)
        {
            playerParticles.enableEmission = true;
            playerParticles.emissionRate = 5f + playerControl.GetVelocity().x + playerControl.GetVelocity().y;
        }
        else
        {
            playerParticles.enableEmission = false;
        }
    }
}
