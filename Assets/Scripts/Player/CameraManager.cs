using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject combatCamera;

    private void Start()
    {
        combatCamera.SetActive(false);
    }

    public void changeToCombat(bool change)
    {
        combatCamera.SetActive(change);
    }
}
