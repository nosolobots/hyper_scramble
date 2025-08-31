using UnityEngine;

public class CursorMode : MonoBehaviour
{
    void Start()
    {
        HideCursor(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HideCursor(false);
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            HideCursor(true);
        }
    }
    
    void HideCursor(bool hide)
    {
        Cursor.visible = !hide;
        Cursor.lockState = hide ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
