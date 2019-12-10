﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static void AddAudio(Transform target, string clip, string folder = "", bool spatialBlend = false, bool one = true)
    {
        if (!(!Main.isAudioCurrentFrame || !one))
            return;
        AudioSource audio = target.gameObject.AddComponent<AudioSource>();
        AudioClip c = null;
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sound/" + folder + clip);
        if (clips.Length != 0)
            c = clips[Random.Range(0, clips.Length)];
        else
        {
            clips = Resources.LoadAll<AudioClip>("Sound/Default/" + clip);
            if (clips.Length != 0)
                c = clips[Random.Range(0, clips.Length)];
        }
        audio.clip = c;
        if (audio.clip != null)
        {
            audio.Play();
            if (spatialBlend)
            {
                audio.spatialBlend = 1;
                audio.rolloffMode = AudioRolloffMode.Linear;
            }
            Destroy(audio, audio.clip.length);
            if (one)
                Main.isAudioCurrentFrame = true;
        }
        else Debug.Log("Нет звука - " + "Sound/" + folder + clip);
    }

    public static void AddAudio(Vector3 target, string clip, string folder = "", bool spantialBlend = false, bool one = true)
    {
        if (!(!Main.isAudioCurrentFrame || !one))
            return;
        GameObject gameObject = Instantiate(new GameObject(clip));
        gameObject.transform.position = target;
        AddAudio(gameObject.transform, clip, folder, spantialBlend, one);
    }

    public static void AddAudio(Character parent, string clip, bool spatialBlend = false, bool one = true)
    {
        AddAudio(parent.transform.position, clip, parent.curatorAudio + "/", spatialBlend, one);
    }


}