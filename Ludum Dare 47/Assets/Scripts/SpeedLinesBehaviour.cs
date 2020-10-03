using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLinesBehaviour : MonoBehaviour
{

    private float _speed;

    private PlayerMovement _playerMovement;

    [SerializeField] private float[] _speedPallier;

    [SerializeField] private float[] _rateOvertime;
    [SerializeField] private float[] _startSpeed;

    private bool[] _pallierReached;

    private ParticleSystem _system;

    private int _currentPallier;

    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();

        _system = GetComponent<ParticleSystem>();

        _pallierReached = new bool[_speedPallier.Length];
        for (int i = 0; i < _pallierReached.Length; i++)
        {
            _pallierReached[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _speed = _playerMovement.speedMultiplier;
        if (_currentPallier < _speedPallier.Length)
        {
            if (_speed >= _speedPallier[_currentPallier] && !_pallierReached[_currentPallier])
            {
                _pallierReached[_currentPallier] = true;
                _system.startSpeed = _startSpeed[_currentPallier];
                _system.emissionRate = _rateOvertime[_currentPallier];
                _currentPallier++;
            }
        }
        if (_currentPallier - 1 > 0)
        {
            if (_speed < _speedPallier[_currentPallier - 1] && _pallierReached[_currentPallier - 1])
            {
                _currentPallier--;
                _pallierReached[_currentPallier] = false;
                _system.startSpeed = _startSpeed[_currentPallier - 1];
                _system.emissionRate = _rateOvertime[_currentPallier - 1];
            }
        }
    }
}
