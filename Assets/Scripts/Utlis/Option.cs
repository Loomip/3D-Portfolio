using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Toggle muteToggle;
    [SerializeField] private TextMeshProUGUI bgmText;
    [SerializeField] private TextMeshProUGUI sfxText;
    [SerializeField] private TextMeshProUGUI muteText;

    private void Start()
    {
        //텍스트 설정
        bgmText.text = DataManager.instance.GetWordData("BGM");
        sfxText.text = DataManager.instance.GetWordData("SFX");
        muteText.text = DataManager.instance.GetWordData("Mute");

        // 초기 볼륨 설정
        bgmVolumeSlider.value = SoundManager.instance.GetBgmVolume();
        sfxVolumeSlider.value = SoundManager.instance.GetSfxVolume();
        muteToggle.isOn = SoundManager.instance.IsMuted();

        // 볼륨 조절 이벤트 등록
        bgmVolumeSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
        muteToggle.onValueChanged.AddListener(OnMuteChanged);
    }

    private void OnBgmVolumeChanged(float value)
    {
        SoundManager.instance.SetBgmVolume(value);
    }

    private void OnSfxVolumeChanged(float value)
    {
        SoundManager.instance.SetSfxVolume(value);
    }

    private void OnMuteChanged(bool isMuted)
    {
        SoundManager.instance.SetMute(isMuted);
    }
}
