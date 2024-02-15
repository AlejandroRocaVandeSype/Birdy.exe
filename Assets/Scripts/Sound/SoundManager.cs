using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    #region SINGLETON
    private static SoundManager m_Instance;

    public static SoundManager Instance
    {
        get
        {
            if (m_Instance == null && !m_IsApplicationQuiting)
            {
                m_Instance = FindObjectOfType<SoundManager>();
                if (m_Instance == null)
                {
                    // There was no instance found of this class. Create a new one
                    GameObject newSoundManager = new GameObject("Singleton_SoundManager");
                    m_Instance = newSoundManager.AddComponent<SoundManager>();
                }
            }

            return m_Instance;
        }
    }

    private static bool m_IsApplicationQuiting = false;
    public void OnApplicationQuit()
    {
        m_IsApplicationQuiting = true;
    }

    #endregion
    [SerializeField] private Sound[] m_Sounds;
    private const string BACKGROUND_MUSIC = "BackgroundMusic";
    private void Awake()
    {
        foreach (Sound s in m_Sounds)
        {
            s.m_Source = gameObject.AddComponent<AudioSource>();
            s.m_Source.clip = s.m_Clip;
            s.m_Source.volume = s.m_Volume;
            s.m_Source.pitch = s.m_Pitch;
            s.m_Source.loop = s.m_Loop;
            s.m_Source.playOnAwake = s.m_OnAwake;
        }
    }

    public void PlaySound(string name, bool waitSoundEnd)
    {
        Sound s = Array.Find(m_Sounds, sound => sound.m_Name == name);

        if (s != null)
        {
            if (waitSoundEnd == false)
            {
                // Dont wait the sound to end in order to play again
                s.m_Source.Play();
            }
            else
            {
                // Wait until the sound has finished playing
                if (!s.m_Source.isPlaying)
                    s.m_Source.Play();
            }
        }


    }

    public void StopSound(string name)
    {
        Sound s = Array.Find(m_Sounds, sound => sound.m_Name == name);

        if (s != null && s.m_Source.isPlaying)
            s.m_Source.Stop();

    }

    public void PlayMusic()
    {
        PlaySound(BACKGROUND_MUSIC, false);
    }

    public void StopMusic()
    {
        StopSound(BACKGROUND_MUSIC);
    }
}
