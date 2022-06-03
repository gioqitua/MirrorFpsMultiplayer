using UnityEngine;

public class PlayerCameraSettings : MonoBehaviour
{
    [SerializeField] float defaultFov = 60;
    [SerializeField] float aimFov = 30;
    public float fovSmoothSpeed = 100f;
    Camera cam;
    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
    }
    public void TurnOnCamera()
    {
        this.gameObject.SetActive(true);
    }

    public void AimFOV()
    {
        cam.fieldOfView = aimFov;
    }
    public void DefaultFOV()
    {
        cam.fieldOfView = defaultFov;
    }
     
}