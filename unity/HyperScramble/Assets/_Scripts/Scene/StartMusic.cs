using UnityEngine;

public class StartMusic : MonoBehaviour
{
    [SerializeField] AudioClip backgroundMusic;

    void Start()
    {
        // Start playing the background music
        AudioSystem.Instance.PlayMusic(backgroundMusic);
    }
}
