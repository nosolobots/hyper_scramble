using TMPro;
using UnityEngine;

public class CallSignInput : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] letterSlots;
    [SerializeField] AudioClip paradeSong;
    [SerializeField] AudioClip indexChangeFX;
    [SerializeField] AudioClip charChangeFX;

    Controls _controls;
    Vector2 _navigation;
    char[] _initials = { '_', '_', '_', '_', '_' };
    int _currentIndex = 0;
    const string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#$%&*_";

    void Awake()
    {
        _controls = new Controls();
        _controls.Player.FireCannon.performed += _ => ConfirmInitials();
        _controls.Player.LaunchBomb.performed += _ => ConfirmInitials();

        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }
    void OnEnable()
    {
        _controls.Enable();
    }

    void Start()
    {
        UpdateLetterSlots();

        if (paradeSong != null)
        {
            AudioSystem.Instance.PlayMusic(paradeSong);
        }
    }

    void UpdateLetterSlots()
    {
        for (int i = 0; i < letterSlots.Length; i++)
        {
            letterSlots[i].text = _initials[i].ToString();
            letterSlots[i].color = (i == _currentIndex) ? Color.cyan : Color.white;
        }
    }

    void Update()
    {
        ReadInput();
    }

    void ReadInput()
    {
        _navigation = _controls.Player.Move.ReadValue<Vector2>();

        if (_navigation.y > 0.5f)
        {
            ChangeLetter(1);
        }
        else if (_navigation.y < -0.5f)
        {
            ChangeLetter(-1);
        }

        if (_navigation.x > 0.5f)
        {
            ChangeIndex(1);
        }
        else if (_navigation.x < -0.5f)
        {
            ChangeIndex(-1);
        }

        // Añade un pequeño retraso para evitar múltiples cambios rápidos
        if (_navigation.magnitude > 0.5f)
        {
            _controls.Player.Move.Disable();
            Invoke(nameof(EnableMovement), 0.2f);
        }
    }
    
    void EnableMovement()
    {
        _controls.Player.Move.Enable();
    }

    void ChangeLetter(int direction)
    {
        int currentLetterIndex = _alphabet.IndexOf(_initials[_currentIndex]);
        currentLetterIndex = (currentLetterIndex + direction + _alphabet.Length) % _alphabet.Length;
        _initials[_currentIndex] = _alphabet[currentLetterIndex];

        UpdateLetterSlots();

        if (charChangeFX != null)
        {
            AudioSystem.Instance.PlayFXSound(charChangeFX);
        }
    }

    void ChangeIndex(int direction)
    {
        _currentIndex = (_currentIndex + direction + letterSlots.Length) % letterSlots.Length;

        UpdateLetterSlots();

        if (indexChangeFX != null)
        {
            AudioSystem.Instance.PlayFXSound(indexChangeFX);
        }
    }

    void ConfirmInitials()
    {
        string callSign = new string(_initials);
        Debug.Log($"Call Sign Confirmed: {callSign}");
        // Implement further logic for confirmed call sign
    }
}
