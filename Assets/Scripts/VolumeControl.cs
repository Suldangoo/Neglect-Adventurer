using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// 볼륨 제어
public class VolumeControl : MonoBehaviour
{
    [SerializeField]
    AudioMixer audioMixer;

    [SerializeField]
    float min_dB = -40;
    [SerializeField]
    float max_dB = 0;

    [SerializeField]
    float currentVolume_dB_bgm;
    [SerializeField]
    float currentVolume_dB_sfx;

    [Range(0, 1)]
    [SerializeField]
    float bgmVolume;
    public float BgmVolume
    {
        get
        {
            return bgmVolume;
        }
        set
        {
            bgmVolume = value;
            UpdateMixer();
        }
    }

    [Range(0, 1)]
    [SerializeField]
    float sfxVolume;
    public float SfxVolume
    {
        get
        {
            return sfxVolume;
        }
        set
        {
            sfxVolume = value;            
            UpdateMixer();
        }
    }

    private void Start()
    {
        BgmVolume = 0.5f;
        SfxVolume = 0.5f;   
    }

    private void OnValidate()
    {
        UpdateMixer();
    }

    // 실제 볼륨 제어
    void UpdateMixer()
    {
        currentVolume_dB_bgm = Get_dB(bgmVolume);
        audioMixer.SetFloat("BGM", currentVolume_dB_bgm);

        currentVolume_dB_sfx = Get_dB(sfxVolume);
        audioMixer.SetFloat("SFX", currentVolume_dB_sfx);
    }

    float Get_dB(float volume)
    {
        float dB;
        if (volume == 0) dB = - 80; // 완전 음소거
        else dB = Mathf.Lerp(min_dB, max_dB, volume);

        return dB;
    }
}
