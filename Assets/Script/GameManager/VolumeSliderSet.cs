using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSliderSet : MonoBehaviour
{
    Slider _slider;
    [SerializeField] string _volumeParameter;
    [SerializeField] AudioMixer audioMixer;
    //[SerializeField] private float _multiplier = 30;

    // Start is called before the first frame update
    void Awake()
    {
        TryGetComponent(out _slider);

        if (_slider == null)
        {
            enabled = false;
            return;
        }

        _slider.onValueChanged.AddListener(HandleSliderValueChanged);

    }

    private void OnDisable()
    {
        //PlayerPrefs.SetFloat(_volumeParameter, _slider.value);
    }

    private void HandleSliderValueChanged(float value)
    {
        audioMixer.SetFloat(_volumeParameter, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(_volumeParameter, value);
    }

    void Start()
    {
        _slider.value = PlayerPrefs.GetFloat(_volumeParameter,1);

    }
}
