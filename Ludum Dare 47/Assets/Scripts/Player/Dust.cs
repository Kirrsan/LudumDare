using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{

    
    [SerializeField] private ParticleSystem dustCloud;
    private bool _gameStartDelay;
    private bool _isGrounded = false;
    private GameObject _ground;
    private GameObject _groundParent;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitAndStart());
        StartCoroutine(WaitAndTakeAStep());
    }

    private void Update()
    {
        if (_ground)
        {
            if (!_groundParent.activeSelf)
            {
                _isGrounded = false;
            }
        }
    }

    private IEnumerator WaitAndStart()
    {
        yield return new WaitForSeconds(0.3f);
        _gameStartDelay = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sol"))
        {
            print("hello");
            _isGrounded = true;
            _ground = other.gameObject;
            _groundParent = _ground.transform.parent.gameObject;
            if (_gameStartDelay) {
                AudioManager.instance.Play("Landing");
                dustCloud.Play();
                StartCoroutine(WaitBeforeStep());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Sol"))
        {
            _isGrounded = false;
            AudioManager.instance.Stop("Step");
        }
    }

    private IEnumerator WaitBeforeStep()
    {
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(WaitAndTakeAStep());
    }

    private IEnumerator WaitAndTakeAStep()
    {
        AudioManager.instance.Play("Step");
        yield return new WaitForSeconds(AudioManager.instance.GetClipLength("Step"));
        if (_isGrounded)
        {
            StartCoroutine(WaitAndTakeAStep());
        }
    }
}
