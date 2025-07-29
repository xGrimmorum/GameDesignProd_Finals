using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] bool inCombat = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy" && !inCombat)
        {
            inCombat = true;
            gameObject.GetComponent<CameraManager>().changeToCombat(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && inCombat)
        {
            inCombat = false;
            gameObject.GetComponent<CameraManager>().changeToCombat(false);
        }
    }

}
