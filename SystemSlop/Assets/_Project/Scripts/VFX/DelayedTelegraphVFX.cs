using UnityEngine;

public class DelayedTelegraphVFX : MonoBehaviour
{

    [SerializeField] private ParticleSystem implosionVFX;
    [SerializeField] private ParticleSystem beamVFX;
    public void Init(float duration)
    {
        SetParticleDuration(implosionVFX, duration);
        SetParticleDuration(beamVFX, duration);
    }

    private void SetParticleDuration(ParticleSystem particleSystem, float duration)
    {
        if (particleSystem == null) return;

        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        var main = particleSystem.main;
        main.duration = duration;

        particleSystem.Play();
    }
}
