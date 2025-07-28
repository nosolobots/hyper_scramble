using UnityEditor.Search;
using UnityEngine;

using UnityEngine.Audio;

public enum FXSoundType
{
    None,
    FireLaser,
    DropBomb,
    ShipExplosion,
    FuelExplosion,
    MysteryExplosion,
    RocketExplosion,
    EnemyExplosion,
    FuelWarning,
    NoFuel
}

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Settings")]
    [SerializeField][Range(0f, 1f)] float fxVolume = 1f;

    [Header("Sound Clips")]
    [SerializeField] AudioClip musicClip;
    [SerializeField] AudioClip fireLaserClip;
    [SerializeField] AudioClip dropBombClip;
    [SerializeField] AudioClip shipExplosionClip;
    [SerializeField] AudioClip fuelExplosionClip;
    [SerializeField] AudioClip mysteryExplosionClip;
    [SerializeField] AudioClip rocketExplosionClip;
    [SerializeField] AudioClip enemyExplosionClip;
    [SerializeField] AudioClip fuelWarningClip;
    [SerializeField] AudioClip noFuelClip;

    public void PlaySound(FXSoundType soundType)
    {
        PlaySound(soundType, Camera.main.transform.position);
    }

    public void PlaySound(FXSoundType soundType, Vector3 position)
    {
        AudioClip clip = null;

        switch (soundType)
        {
            case FXSoundType.FireLaser:
                clip = fireLaserClip;
                break;
            case FXSoundType.DropBomb:
                clip = dropBombClip;
                break;
            case FXSoundType.ShipExplosion:
                clip = shipExplosionClip;
                break;
            case FXSoundType.FuelExplosion:
                clip = fuelExplosionClip;
                break;
            case FXSoundType.MysteryExplosion:
                clip = mysteryExplosionClip;
                break;
            case FXSoundType.RocketExplosion:
                clip = rocketExplosionClip;
                break;
            case FXSoundType.EnemyExplosion:
                clip = enemyExplosionClip;
                break;
            case FXSoundType.FuelWarning:
                clip = fuelWarningClip;
                break;
            case FXSoundType.NoFuel:
                clip = noFuelClip;
                break;
        }

        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position, fxVolume);
        }
    }
}
