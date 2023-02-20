using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;
public enum SoundType
{
    //Add more sound names here
    BlobLoop,
    OrganClick,
    OrganPlace,
    OrganRemoval,
    ButtonClick,
    BlobStart,
    Success,
    RobotVoice,
    Music,

}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource musicSource, effectsSource, loopSource;
    [SerializeField] private List<AudioSource> loopSources = new List<AudioSource>();
    [SerializeField] private AudioMixer MixerMaster;

    [System.Serializable]
    public class Sound
    {
        [SerializeField] string name;
        [SerializeField] public SoundType soundName;
        public List<AudioClip> clips = new List<AudioClip>();
        [HideInInspector] public List<AudioClip> tempClips = new List<AudioClip>();

        public void Reset()
        {
            foreach (AudioClip clip in tempClips)
            {
                clips.Add(clip);
            }
        }

        public void CopyElements()
        {
            foreach (AudioClip clip in clips)
            {
                tempClips.Add(clip);
            }
        }

        public void Remove(AudioClip clip)
        {
            foreach (AudioClip audioClip in clips)
            {
                if (audioClip == clip)
                {
                    clips.Remove(audioClip);
                }
            }
        }
    }

    [SerializeField] List<Sound> soundlist = new List<Sound>();
    Dictionary<SoundType, List<AudioClip>> soundDictionary = new Dictionary<SoundType, List<AudioClip>>();


    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (Sound sound in soundlist)
        {
            sound.CopyElements();
            soundDictionary.Add(sound.soundName, sound.clips);
        }

        PlayMusic();
    }

    public void PlaySound(SoundType sound)
    {
        int soundIndex = PickSound(sound);
        effectsSource.PlayOneShot(soundDictionary[sound][soundIndex]);
        RemoveSound(sound, soundIndex);
    }

    public void PlaySoundWithPitch(SoundType sound, float lowestPitch, float highestPitch)
    {
        int soundIndex = PickSound(sound);
        effectsSource.pitch = Random.Range(lowestPitch, highestPitch);
        effectsSource.PlayOneShot(soundDictionary[sound][soundIndex]);
        effectsSource.pitch = 1;
        RemoveSound(sound, soundIndex);
    }

    int PickSound(SoundType sound)
    {
        if (soundDictionary[sound].Count == 0)
        {
            foreach (Sound SFX in soundlist)
            {
                if (SFX.soundName == sound)
                {
                    SFX.Reset();
                }
            }
        }

        int rnd = Random.Range(0, soundDictionary[sound].Count);
        return rnd;
    }

    void RemoveSound(SoundType sound, int soundIndex)
    {
        soundDictionary[sound].Remove(soundDictionary[sound][soundIndex]);
    }
    //This method is not very flexible, the way it is setup only 1 looping sound can play at once, not ideal. Needs to be updated
    public void StartLoopSound(SoundType loopSound)
    {
        loopSource.Play();
    }

    public IEnumerator StopLoopSound(float fadeOutTime)
    {
        float elapsedTime = 0;

        float currentVolume;
        MixerMaster.GetFloat("LoopVol", out currentVolume);
        while (fadeOutTime > elapsedTime)
        {
            MixerMaster.SetFloat("LoopVol", Mathf.Lerp(currentVolume, Mathf.Log10(0.0001f) * 20, (elapsedTime / fadeOutTime)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        MixerMaster.SetFloat("LoopVol", Mathf.Log10(0.0001f) * 20);

        loopSource.Stop();
        MixerMaster.SetFloat("LoopVol", currentVolume);

        yield return null;
    }

    public void PlayMusic()
    {
        musicSource.Play();
    }
}
