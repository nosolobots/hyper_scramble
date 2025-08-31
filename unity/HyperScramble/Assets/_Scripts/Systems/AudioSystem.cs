
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sistema de audio que maneja la reproducción de música y efectos de sonido.
/// Utiliza un pool de AudioSources para optimizar la reproducción de efectos de sonido.
/// </summary>
/// <remarks>
/// Este sistema es un singleton persistente, por lo que sólo habrá una instancia en la escena,
/// y ésta persistirá a través de las cargas de escenas sucesivas.
/// </remarks>

public class AudioSystem : PersistentSingleton<AudioSystem>
{
    [Header("Audio Settings")]
    [SerializeField] bool soundEnabled = true;
    [SerializeField][Range(0f, 1f)] float musicVolume = 0.5f;
    [SerializeField][Range(0f, 1f)] float fxVolume = 1f;
    [SerializeField] bool spatializeFX = false;

    [Header("Audio Pool Settings")]
    [SerializeField] int initialPoolSize = 10;
    [SerializeField] bool autoExpandPool = true;

    AudioSource _musicSource; // Fuente de audio para la música
    List<AudioSource> _fxSourcePool; // Pool de fuentes de audio para efectos de sonido

    /// <summary>
    /// Ajusta el volumen de la música.
    /// </summary>
    public float MusicVolume
    {
        get => musicVolume;
        private set
        {
            musicVolume = Mathf.Clamp01(value);
            _musicSource.volume = musicVolume;
        }
    }

    /// <summary>
    /// Ajusta el volumen de los efectos de sonido.
    /// </summary>
    public float FxVolume
    {
        get => fxVolume;
        private set => fxVolume = Mathf.Clamp01(value);
    }

    protected override void Awake()
    {
        base.Awake();

        InitiateAudioSources();
    }

    /// <summary>
    /// Inicializa las fuentes de audio para la música y los efectos de sonido.
    /// Crea un pool de AudioSources para los efectos de sonido.
    /// </summary>
    /// <remarks>
    /// Esta función se llama una vez al inicio del juego para configurar el sistema de audio.
    /// </remarks>
    private void InitiateAudioSources()
    {
        // Inicializa la fuente de música
        _musicSource = CreateAudioSource("MusicSource");

        // Inicializar el pool de fuentes de sonido FX
        _fxSourcePool = new List<AudioSource>();
        for (int i = 0; i < initialPoolSize; i++)
        {
            _fxSourcePool.Add(CreateAudioSource());
        }
    }

    /// <summary>
    /// Crea un nuevo AudioSource para reproducir efectos de sonido.
    /// Este método se utiliza para crear tanto la fuente de música como las fuentes de efectos de sonido.
    /// </summary>
    /// <param name="name">Nombre del GameObject que contendrá el AudioSource.</param>
    /// <returns>Un nuevo AudioSource configurado.</returns>
    /// <remarks>
    /// El AudioSource creado se configura con spatial blend según la configuración del sistema.
    /// </remarks>
    private AudioSource CreateAudioSource(string name = "FXSource")
    {
        // Crea un nuevo GameObject (hijo del AudioSystem) para la fuente de sonido
        GameObject fxObject = new GameObject(name);
        fxObject.transform.SetParent(transform);

        // Añade un AudioSource para la reproducción de efectos de sonido
        AudioSource fxSource = fxObject.AddComponent<AudioSource>();
        fxSource.spatialBlend = spatializeFX ? 1f : 0f;
        fxSource.playOnAwake = false;

        return fxSource;
    }

    /// <summary>
    /// Reproduce un clip de música.
    /// </summary>
    /// <param name="clip">El clip de audio a reproducir.</param>
    /// <param name="loop">Indica si la música debe reproducirse en bucle.</param>
    /// <remarks>
    /// Si el sonido está desactivado, no se reproducirá la música.
    /// </remarks>
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (!soundEnabled) return;

        _musicSource.clip = clip;
        _musicSource.loop = loop;
        _musicSource.spatialBlend = 0f; // No usar spatial blend para la música        
        _musicSource.volume = musicVolume;

        _musicSource.Play();
    }

    /// <summary>
    /// Pausa la música si está reproduciéndose.
    /// </summary>
    public void PauseMusic()
    {
        if (!soundEnabled) return;

        if (_musicSource.isPlaying)
        {
            _musicSource.Pause();
        }
    }

    /// <summary>
    /// Detiene la música si está reproduciéndose.
    /// </summary>
    public void StopMusic()
    {
        if (!soundEnabled) return;

        if (_musicSource.isPlaying)
        {
            _musicSource.Stop();
        }
    }

    /// <summary>
    /// Reanuda la música si está pausada.
    /// </summary>
    public void ResumeMusic()
    {
        if (!soundEnabled) return;

        if (!_musicSource.isPlaying)
        {
            _musicSource.UnPause();
        }
    }

    /// <summary>
    /// Reproduce un efecto de sonido.
    /// </summary>
    /// <param name="clip">El clip de audio a reproducir.</param>
    public void PlayFXSound(AudioClip clip)
    {
        PlayFXSound(clip, Vector3.zero, fxVolume);
    }

    /// <summary>
    /// Reproduce un efecto de sonido en una posición específica.
    /// </summary>
    /// <param name="clip">El clip de audio a reproducir.</param>
    /// <param name="position">La posición en el mundo donde se reproducirá el efecto de sonido.</param>
    public void PlayFXSound(AudioClip clip, Vector3 position)
    {
        PlayFXSound(clip, position, fxVolume);
    }

    /// <summary>
    /// Reproduce un efecto de sonido en una posición específica con un volumen específico.
    /// Para la reproducción debe obtener una fuente de sonido del pool.
    /// Si no hay fuentes disponibles y el pool puede expandirse, se crea una nueva fuente.
    /// Si spatializeFX está activado, la fuente de sonido se posiciona en el mundo; 
    /// de lo contrario, se posiciona en el centro de la cámara.
    /// </summary>
    /// <param name="clip">El clip de audio a reproducir.</param>
    /// <param name="position">La posición en el mundo donde se reproducirá el efecto de sonido.</param>
    /// <param name="volume">El volumen del efecto de sonido.</param>
    public void PlayFXSound(AudioClip clip, Vector3 position, float volume)
    {
        if (!soundEnabled || clip == null) return;

        // Busca una fuente de sonido FX disponible en el pool
        AudioSource _fxSource = _fxSourcePool.Find(source => !source.isPlaying);

        // Si no hay fuentes disponibles y el pool puede expandirse, crea una nueva fuente
        if (_fxSource == null && autoExpandPool)
        {
            _fxSource = CreateAudioSource();
            _fxSourcePool.Add(_fxSource);
        }
        else if (_fxSource == null)
        {
            // Si finalmente no se configue una fuente disponible, no se reproduce el sonido
            Debug.LogWarning("Ningún AudioSource disponible en el pool para reproducir el sonido.");
            return;
        }

        // Configura la fuente de sonido y reproduce el clip
        _fxSource.clip = clip;
        _fxSource.volume = volume;
        _fxSource.loop = false;

        // Si se está usando spatial blend, posiciona la fuente de sonido
        if (spatializeFX)
        {
            _fxSource.transform.position = position;
        }

        _fxSource.Play();
    }

    /// <summary>
    /// Alterna el estado del sonido (activado/desactivado).
    /// Si el sonido está desactivado, pausa la música y detiene todos los efectos de sonido en reproducción.
    /// Si el sonido está activado, reanuda la música si estaba pausada.
    /// </summary>
    /// <remarks>
    /// Este método se utiliza para activar o desactivar el sonido globalmente en el juego.
    /// </remarks>
    public void ToggleSound()
    {
        if (soundEnabled)
        {
            // Pausa la música si el sonido está activado
            if (_musicSource.isPlaying && _musicSource.clip != null)
            {
                _musicSource.Pause();
            }

            // Detiene todos los efectos de sonido en reproducción
            foreach (var source in _fxSourcePool)
            {
                if (source != null && source.isPlaying)
                {
                    source.Stop();
                }
            }
        }
        else
        {
            // Reanuda la música si el sonido está activado
            if (!_musicSource.isPlaying && _musicSource.clip != null)
            {
                _musicSource.UnPause();
            }
        }

        soundEnabled = !soundEnabled;
    }
}
