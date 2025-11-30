using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MaterialHitsoundV2 : MonoBehaviourPunCallbacks
{
    [System.Serializable]
    public struct MaterialSound
    {
        public List<Material> materials;
        public AudioClip[] sounds;
    }

    [Header("MATERIAL HITSOUNDS BY RG_vr")]
    public List<MaterialSound> materialSounds;
    public bool useTrigger = false;

    [Header("Audio Source (fallback only)")]
    public AudioSource audioSource;

    [Header("Cooldown Settings")]
    public float cooldownTime = 0.5f;

    [Header("Debugging")]
    public bool cooldownBool = false;

    private PhotonView photonView;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogWarning("MaterialHitsoundV2: No AudioSource assigned or found on the GameObject.");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (useTrigger)
        {
            HandleCollisionOrTrigger(other.GetComponent<Renderer>(), other.ClosestPoint(transform.position));
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!useTrigger && collision.relativeVelocity.magnitude > 0.1f)
        {
            HandleCollisionOrTrigger(collision.collider.GetComponent<Renderer>(), collision.contacts[0].point);
        }
    }

    private void HandleCollisionOrTrigger(Renderer renderer, Vector3 hitPoint)
    {
        if (cooldownBool)
            return;

        if (renderer != null && renderer.sharedMaterial != null)
        {
            Material touchedMaterial = renderer.sharedMaterial;

            for (int i = 0; i < materialSounds.Count; i++)
            {
                if (materialSounds[i].materials.Contains(touchedMaterial))
                {
                    AudioClip[] clips = materialSounds[i].sounds;
                    if (clips != null && clips.Length > 0)
                    {
                        int index = Random.Range(0, clips.Length);

                        if (PhotonNetwork.InRoom)
                        {
                            photonView.RPC("PlaySoundAtPositionRPC", RpcTarget.All, i, index, hitPoint);
                        }
                        else
                        {
                            PlaySoundAtPosition(i, index, hitPoint);
                        }

                        StartCoroutine(Cooldown());
                        break;
                    }
                }
            }
        }
    }

    [PunRPC]
    private void PlaySoundAtPositionRPC(int materialSoundIndex, int clipIndex, Vector3 position)
    {
        PlaySoundAtPosition(materialSoundIndex, clipIndex, position);
    }

    private void PlaySoundAtPosition(int materialSoundIndex, int clipIndex, Vector3 position)
    {
        if (materialSoundIndex < 0 || materialSoundIndex >= materialSounds.Count)
            return;

        AudioClip[] clips = materialSounds[materialSoundIndex].sounds;
        if (clips == null || clipIndex < 0 || clipIndex >= clips.Length)
            return;

        AudioClip clip = clips[clipIndex];

        GameObject temp = new GameObject("TempSpatialSound");
        temp.transform.position = position;

        AudioSource tempSource = temp.AddComponent<AudioSource>();
        tempSource.spatialBlend = 1.0f;
        tempSource.minDistance = 1f;
        tempSource.maxDistance = 12.5f;
        tempSource.rolloffMode = AudioRolloffMode.Linear;

        tempSource.PlayOneShot(clip);
        Destroy(temp, 2f);
    }

    private IEnumerator Cooldown()
    {
        cooldownBool = true;
        yield return new WaitForSeconds(cooldownTime);
        cooldownBool = false;
    }
}