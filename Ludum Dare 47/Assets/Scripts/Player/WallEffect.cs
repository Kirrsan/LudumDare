using UnityEngine;

public class WallEffect : MonoBehaviour
{

    [SerializeField] private GameObject _particles;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            AudioManager.instance.Play("Grind");
            _particles.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            AudioManager.instance.Stop("Grind");
            _particles.SetActive(false);
        }
    }

}
