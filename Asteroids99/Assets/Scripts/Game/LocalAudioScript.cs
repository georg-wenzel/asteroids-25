using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script which plays a predetermined sound via its audio source, then destroys itself
/// </summary>
public class LocalAudioScript : MonoBehaviour
{    
    #region properties
    /// <summary>
    /// The clip to play
    /// </summary>
    public AudioClip Clip;
    /// <summary>
    /// Whether or not to play the asteroid death particle system
    /// </summary>
    public bool ParticleSystem = false;
    /// <summary>
    /// The amount of generated particles over time
    /// </summary>
    public int Particles = 100;
    #endregion

    #region methods
    void Start()
    {
        var source = GetComponent<AudioSource>();
        source.clip = Clip;
        source.Play();
        float duration = source.clip.length;
        if(ParticleSystem)
        {
            var particles = GetComponent<ParticleSystem>();
            var emission = particles.emission;
            emission.rateOverTime = Particles;
            var particles_main = particles.main;
            particles_main.maxParticles = Particles;
            particles.Play();
            float particleDuration = particles.main.startLifetime.constant + particles.main.duration;
            if (duration < particleDuration) duration = particleDuration;
        }
        Destroy(this.gameObject, duration);
    }
    #endregion
}
