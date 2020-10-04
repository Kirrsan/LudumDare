using UnityEngine;
using UnityEngine.UI;

public class DisplayMetersLeft : MonoBehaviour
{
    [SerializeField]private Transform _player;
    [SerializeField]private Transform _levelEnd;

    [SerializeField]private Text _metersLeft;
    // Update is called once per frame
    private void Update()
    {
        _metersLeft.text = CalculateDistance().ToString() + " m";
    }

    private int CalculateDistance()
    {
        return (int)(_levelEnd.position - _player.position).magnitude;
    }
}
