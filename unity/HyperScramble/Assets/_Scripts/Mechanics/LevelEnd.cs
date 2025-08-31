using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ship"))
        {
            GameManager.Instance.CompleteLevel();
        }
    }
}
