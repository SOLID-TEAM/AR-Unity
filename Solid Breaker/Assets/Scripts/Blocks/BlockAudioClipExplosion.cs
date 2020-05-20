using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAudioClipExplosion : MonoBehaviour
{
    private AudioSource m_audio;
    public AudioClip[] explosionClips;

    // Start is called before the first frame update
    void Start()
    {
        m_audio = GetComponent<AudioSource>();

        if (m_audio)
            m_audio.PlayOneShot(explosionClips[Random.Range(0, explosionClips.Length)]);
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_audio.isPlaying)
            Destroy(this.gameObject);
    }
}
