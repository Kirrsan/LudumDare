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
    }

    private void Update() {
        if (_ground) {
            if (!_ground.activeSelf) {
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
            _ground = other.gameObject;
            if (transform.parent.rotation.z != 0)
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
                print(_ground.name);
                _ground = other.gameObject;
                if (_gameStartDelay)
                {
                    AudioManager.instance.Play("Landing");
                    _dustCloud.Play();
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
    private void DoWallEffects(int index)
    {
        AudioManager.instance.Play("StickToWall");
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

}
