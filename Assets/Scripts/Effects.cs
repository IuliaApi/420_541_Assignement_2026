using UnityEngine;

public static class Effects
{
    public static void Burst(Vector3 position, Color color)
    {
        GameObject effect = Object.Instantiate(Resources.Load<GameObject>("PickupEffect"), position, Quaternion.identity);
        ParticleSystem particles = effect.GetComponent<ParticleSystem>();
        var settings = particles.main;
        settings.startColor = color;
        particles.Play();
        Object.Destroy(effect, 2f);
    }
}
