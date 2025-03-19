using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class HueShifter : MonoBehaviour
{
    private PostProcessVolume _ppVolume;
    private ColorGrading _colorGrading;
    private float _hueShiftValue = 0f; 
    private float _rate = 0f;

    private void Start()
    {
        _ppVolume = GetComponent<PostProcessVolume>();

        if (_ppVolume != null && _ppVolume.profile != null)
        {
            _ppVolume.profile.TryGetSettings(out _colorGrading);
        }
    }

    private void Update()
    {
        if (_colorGrading != null)
        {
            _rate += 0.01f * Time.deltaTime;
            if (_rate > 1f) _rate = 1f;

            _hueShiftValue += 0.1f * _rate;
            if (_hueShiftValue > 100f) _hueShiftValue = -100f;

            _colorGrading.hueShift.value = _hueShiftValue;
        }
    }
}