using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource MusicSource;
    public bool muted;
    public float volume;

    public static Music instance;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            Mute();
    }

    void Start()
    {
        MusicSource.Play();
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Mute()
    {
        muted = !muted;
        if (muted)
            MusicSource.volume = 0f;
        else MusicSource.volume = volume;
    }

    public void ChangeVolume(float sliderValue)
    {
        volume = sliderValue * 0.1f;
        MusicSource.volume = volume;
    }
}