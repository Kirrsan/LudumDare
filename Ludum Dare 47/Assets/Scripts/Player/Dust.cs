using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour {

    [SerializeField] private ParticleSystem _dustCloud;
    [SerializeField] private GameObject[] _wallParticle;
    private bool _gameStartDelay;
    private bool _isGrounded = false;
    private GameObject _ground;
    private GameObject _groundParent;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(WaitAndStart());
        StartCoroutine(WaitAndTakeAStep());
    }

    private void Update() {
        if (_ground) {
            if (!_groundParent.activeSelf) {
                _isGrounded = false;
                StopWallEffects();
            }
        }
    }

    private IEnumerator WaitAndStart() {
        yield return new WaitForSeconds(0.5f);
        _gameStartDelay = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Sol")) {
            if(transform.parent.rotation.z != 0)
            {
                if(transform.parent.rotation.z < 0)
                {
                    DoWallEffects(0);
                }
                else
                {
                    DoWallEffects(1);
                }
            }
            else
            {
                _isGrounded = true;
                _ground = other.gameObject;
                _groundParent = _ground.transform.parent.gameObject;
                if (_gameStartDelay)
                {
                    AudioManager.instance.Play("Landing");
                    _dustCloud.Play();
                    StartCoroutine(WaitBeforeStep());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        StopWallEffects();
        if (other.CompareTag("Sol")) {
            _isGrounded = false;
            AudioManager.instance.Stop("Step");
            StopWallEffects();
        }
    }

    private IEnumerator WaitBeforeStep()
    {
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(WaitAndTakeAStep());
    }

    private IEnumerator WaitAndTakeAStep()
    {
        AudioManager.instance.Play("Step");
        yield return new WaitForSeconds(0.25f);
        if (_isGrounded)
        {
            StartCoroutine(WaitAndTakeAStep());
        }
    }
    private void DoWallEffects(int index)
    {
        AudioManager.instance.Play("StickToWall");
        StartCoroutine(WaitAndGrind());
        _wallParticle[index].SetActive(true);
    }

    private void StopWallEffects()
    {
        AudioManager.instance.Stop("Grind");
        foreach(GameObject particle in _wallParticle)
        {
            particle.SetActive(false);
        }
    }

    private IEnumerator WaitAndGrind()
    {
        yield return new WaitForSeconds(AudioManager.instance.GetClipLength("StickToWall"));
        AudioManager.instance.Play("Grind");
    }

}
