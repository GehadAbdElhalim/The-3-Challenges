using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemController : MonoBehaviour
{
    public static ParticleSystemController instance;
    ParticleSystem particles;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    public void PlayParticlesAt(Transform pos)
    {
        transform.position = pos.position;
        particles.Play();
    }
}
