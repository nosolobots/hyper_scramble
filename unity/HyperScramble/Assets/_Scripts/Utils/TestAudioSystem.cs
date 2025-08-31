using UnityEngine;

public class TestAudioSystem : MonoBehaviour
{
    [SerializeField] private AudioClip fxTestClip;
    [SerializeField] private AudioClip musicTestClip;
    [SerializeField] private Vector3 position = Vector3.zero;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioSystem.Instance.PlayMusic(musicTestClip);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            AudioSystem.Instance.PauseMusic();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            AudioSystem.Instance.StopMusic();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            AudioSystem.Instance.ResumeMusic();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            AudioSystem.Instance.PlayFXSound(fxTestClip, position);
        }
    }
}
