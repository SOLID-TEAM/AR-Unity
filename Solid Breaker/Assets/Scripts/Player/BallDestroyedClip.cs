using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDestroyedClip : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource m_audio;
    // Start is called before the first frame update
    void Start()
    {
        m_audio = GetComponent<AudioSource>();
        if (m_audio)
            m_audio.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_audio)
        {
            if (!m_audio.isPlaying)
                Destroy(this.gameObject);
        }
    }
}
