using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string m_Name;
    public AudioClip m_Clip;

    public bool m_Loop;
    public bool m_OnAwake;

    [Range(0f, 1f)]
    public float m_Volume;
    [Range(.1f, 3f)]
    public float m_Pitch;

    [HideInInspector] // Wont show the variable
    public AudioSource m_Source;


}
