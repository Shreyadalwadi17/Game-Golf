using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    public Sound[] clips;
    public static AudioManager inst;

    public void Awake()
    {
        inst = this;

    }

    public void SoundPlay(SoundName name)
    {
        foreach (var item in clips)
        {
            if (item.name == name)
            {
                audioSource.PlayOneShot(item.clip);
                break;
            }
        }

    }

    public void SoundMute(bool val)
    {
        audioSource.mute = val;

    }
    [System.Serializable]
    public class Sound
    {
        public SoundName name;
        public AudioClip clip;
    }
    public enum SoundName
    {
        Sound1,
     
     
    }

}




