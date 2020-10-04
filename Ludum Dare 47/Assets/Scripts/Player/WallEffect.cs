using System.Collections;
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
            DoWallEffects();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            StopWallEffects();
        }
    }

    private void DoWallEffects()
    {
        AudioManager.instance.Play("StickToWall");
        StartCoroutine(WaitAndGrind());
        _particles.SetActive(true);
    }

    private void StopWallEffects()
    {
        AudioManager.instance.Stop("Grind");
        _particles.SetActive(false);
    }

    private IEnumerator WaitAndGrind()
    {
        yield return new WaitForSeconds(AudioManager.instance.GetClipLength("StickToWall"));
        AudioManager.instance.Play("Grind");
    }

}
