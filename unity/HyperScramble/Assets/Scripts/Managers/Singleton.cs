using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    static T _instance;
    public static T Instance => _instance;

    protected virtual void Awake()
    {
        if (_instance != null && this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = (T)this;
        }

        // Ensure the singleton persists across scene loads
        if (!gameObject.transform.parent)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
