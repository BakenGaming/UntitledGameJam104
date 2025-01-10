using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class StaticVariables : MonoBehaviour
{
    public static StaticVariables i;
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer, whatIsEnemy, 
        collectable, whatIsUI;
    [SerializeField] private AudioMixerGroup masterMixer, sfxMixer, musicMixer;

    private void Awake() 
    {
        i = this;
    }

    public LayerMask GetGroundLayer() { return whatIsGround; }
    public LayerMask GetPlayer() { return whatIsPlayer; }
    public LayerMask GetEnemy() { return whatIsEnemy; }
    public LayerMask GetCollectable() { return collectable; }
    public LayerMask GetUI(){ return whatIsUI; }
    public AudioMixerGroup GetMasterMixer(){ return masterMixer; }
    public AudioMixerGroup GetSFXMixer(){ return sfxMixer; }
    public AudioMixerGroup GetMusicMixer(){ return musicMixer; }

}
