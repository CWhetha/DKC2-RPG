using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource BGM;
    public BattleSystem battleSystem;
    public GameManager1 control;

    private void Awake()
    {
        if (control)
        {
            control.StartMusic += ChangeBGM;
        }
        if (battleSystem)
        {
            battleSystem.StartMusic += ChangeBGM;
        }
    }

    public void ChangeBGM(AudioClip music, bool loop)
    {
        BGM.Stop();
        BGM.loop = loop;
        BGM.clip = music;
        BGM.Play();
    }
}
