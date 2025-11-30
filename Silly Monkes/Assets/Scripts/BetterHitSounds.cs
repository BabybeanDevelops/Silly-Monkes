using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR;

public class BetterHitSounds : MonoBehaviourPun
{
    public AudioSource audioSource;
    public bool LeftController;
    public List<TagSound> tagSounds = new List<TagSound>();

    [System.Serializable]
    public class TagSound
    {
        public string tag;
        public AudioClip[] sounds;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("PlayHitSound", RpcTarget.All, other.gameObject.tag, LeftController);
        }
        else
        {
            PlayLocalHitSound(other.gameObject.tag);
        }
    }

    [PunRPC]
    void PlayHitSound(string tag, bool leftController)
    {
        TagSound sound = tagSounds.Find(x => x.tag == tag);
        if (sound != null)
        {
            PlayRandomSound(sound.sounds, audioSource);
        }
    }

    void PlayLocalHitSound(string tag)
    {
        TagSound sound = tagSounds.Find(x => x.tag == tag);
        if (sound != null)
        {
            AudioClip clip = sound.sounds[Random.Range(0, sound.sounds.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    void PlayRandomSound(AudioClip[] audioClips, AudioSource audioSource)
    {
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.Play();
    }

    }