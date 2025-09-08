using UnityEngine;

public class BaseAlarm : MonoBehaviour
{
    [SerializeField] AudioClip alarmSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ship"))
        {
            Debug.Log("Alarm Triggered!");
            AudioSystem.Instance.PlayFXSound(alarmSound);
        }
    }
}
