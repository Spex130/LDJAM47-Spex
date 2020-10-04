using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFXTrigger : MonoBehaviour
{

    public PlayerScript Player;

    [Header("Particle Systems")]
    public ParticleSystem SmokeSystem;
    public ParticleSystem SlashSystem;
    public ParticleSystem ShineSystem;
    public ParticleSystem ShotSystem;

    public void spawnSmokeParticle()
    {
        SmokeSystem.Play();
    }

    public void spawnSlashParticle()
    {
        SlashSystem.Play();
    }

    public void spawnShineParticle()
    {
        ShineSystem.Play();
    }

    public void spawnShotParticle()
    {
        ShotSystem.Play();
    }

    public void markPlayerSpawned()
    {
        Player.MarkPlayerSpawned();
    }
    
}
