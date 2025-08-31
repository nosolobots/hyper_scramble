using UnityEngine;

/// <summary>
/// Una instancia estática es similar a un singleton pero, en lugar de destruir 
/// las nuevas instancias, sobreescribe la instancia existente. 
/// Esto es útil cuando deseamos resetear el estado de una instancia.
/// </summary>
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance;
    public static T Instance => _instance;

    protected virtual void Awake() => _instance = this as T;

    protected virtual void OnApplicationQuit()
    {
        _instance = null;
        Destroy(gameObject);
    }
}

/// <summary>
/// Transforma StaticInstance en un singleton que destruye las nuevas instancias,
/// asegurando que solo una instancia persista en la escena.
/// </summary>
public abstract class BaseSingleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            base.Awake();   
        }
    }
}

/// <summary>
/// Un singleton que persiste a través de las cargas de escena.
/// </summary>
public abstract class PersistentSingleton<T> : BaseSingleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();

        // Asegura que el singleton persista a través de las cargas de escena
        if (!gameObject.transform.parent)
        {
            DontDestroyOnLoad(gameObject);
        }
    }   
} 

