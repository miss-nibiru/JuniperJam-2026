using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private GameObject cursor;

    private GameObject cursorInstance;
    private bool isCursorEnabled;

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();

        mouseScreenPosition.z = -Camera.main.transform.position.z;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        mouseWorldPosition.z = 0f;
        
        if (isCursorEnabled) cursorInstance.transform.position = mouseWorldPosition;
    }

    public void SetShootingCursorEnabled(bool enabled)
    {
        if (enabled)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            cursorInstance = Instantiate(cursor);
            isCursorEnabled = true;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Destroy(cursorInstance);
            isCursorEnabled = false;
        }
    }
}
