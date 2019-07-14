using UnityEngine;

/// <summary>
/// Handles ensuring the the game board is fully displayed on any screen via resizing 
/// </summary>
public class GameScreenCameraHandler : MonoBehaviour
{
    public Vector3 checkerPoint;
    private Camera mainCamera;

    // Use this for initialization
    private void Awake()
    {
        mainCamera = GetComponent<Camera>();

        //first zoom the camera so everything important is on-screen
        while (mainCamera.WorldToViewportPoint(checkerPoint).x < 0)
        {
            mainCamera.orthographicSize = mainCamera.orthographicSize + 0.1f;
        }

        //now that we've zoomed it, reposition it properly so the UI elements line up
        while (mainCamera.WorldToViewportPoint(checkerPoint).y < 0.99f)
        {
            transform.Translate(new Vector3(0, -0.05f, 0));
        }

        //make sure resizing didn't somehow move it above the screen
        while (mainCamera.WorldToViewportPoint(checkerPoint).y > 1f)
        {
            transform.Translate(new Vector3(0, 0.05f, 0));
        }
    }
}
