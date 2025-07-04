using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 전체적으로 PlayerPrefs을 넣어서 게임을 꺼도 설정한거 저장되게 만듦.
public class AudioManager : SingletonBehaviour<AudioManager>
{
    protected override bool IsDontDestroy() => true;
    [Header("BGM")]
    public AudioSource bgmSource_bgm;
    public Slider volumeSlider_bgm;

    [Header("SFX")]
    public List<AudioSource> sfxSources = new List<AudioSource>(); // 효과음이 여러개라 리스트로 저장 
    public Slider volumeSlider_sfx;

 

    private void OnEnable() //UI 슬라이더/토글의 이벤트 리스너 연결, 중복 연결 방지도 함
    {
        if (volumeSlider_bgm != null)
        {
            volumeSlider_bgm.onValueChanged.RemoveListener(OnBgmVolumeChange);
            volumeSlider_bgm.onValueChanged.AddListener(OnBgmVolumeChange);
        }

        if (volumeSlider_sfx != null)
        {
            volumeSlider_sfx.onValueChanged.RemoveListener(OnSfxVolumeChange);
            volumeSlider_sfx.onValueChanged.AddListener(OnSfxVolumeChange);
        }

        if (!bgmSource_bgm.isPlaying)   // 배경음이 재생 X -> 자동 재생
            bgmSource_bgm.Play();

     
    }
    void Update()
    {
        LoadVolumeSettings();
        
    }
    

    void LoadVolumeSettings()   // 저장된 볼륨 값 가져와서 슬라이더, 오디오 소스에 반영
    {
        float savedBgm = Mathf.Clamp(PlayerPrefs.GetFloat("BGM", 1.0f), 0f, 1f);
        bgmSource_bgm.volume = savedBgm;
        bgmSource_bgm.loop = true;
        if (volumeSlider_bgm != null)
            volumeSlider_bgm.value = savedBgm;

        float savedSfx = Mathf.Clamp(PlayerPrefs.GetFloat("SFX", 1.0f), 0f, 1f);
        foreach (var source in sfxSources)
        {
            if (source != null)
                source.volume = savedSfx;
        }
        if (volumeSlider_sfx != null)
            volumeSlider_sfx.value = savedSfx;
    }


    void OnBgmVolumeChange(float value) // 슬라이더 조작 시 BGM 볼륨 변경 및 저장
    {
        bgmSource_bgm.volume = value;
        PlayerPrefs.SetFloat("BGM", value);
        PlayerPrefs.Save();
    }

    void OnSfxVolumeChange(float value) // 모든 효과음의 볼퓸을 슬라이더 값에 맞춰 조절
    {
        foreach (var source in sfxSources)
        {
            source.volume = value;
        }
        PlayerPrefs.SetFloat("SFX", value);
        PlayerPrefs.Save();
    }

    public void PlaySFX(AudioClip clip) // 전달된 효과음을 비어 있는 오디오 소스를 찾아 재생, 모두 사용중이면 첫 번째 소스 사용
    {
        if (sfxSources != null && clip != null && sfxSources.Count > 0)
        {
            foreach (var source in sfxSources)
            {
                if (!source.isPlaying)
                {
                    source.PlayOneShot(clip); // 겹쳐서 재생 가능
                    return;
                }
            }
            sfxSources.Add(gameObject.AddComponent<AudioSource>());
            sfxSources[sfxSources.Count-1].PlayOneShot(clip);
        }
    }

    
    }