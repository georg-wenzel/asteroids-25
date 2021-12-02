using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script which plays a predetermined sound via its audio source, then destroys itself
/// </summary>
public class LocalAudioScript : MonoBehaviour
{
    #region fields
    /// <summary>
    /// The audio source
    /// </summary>
    private AudioSource source;
    #endregion
    
    #region properties
    /// <summary>
    /// The clip to play
    /// </summary>
    public AudioClip Clip;
    #endregion

    #region methods
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = Clip;
        source.Play();
        Destroy(this.gameObject, source.clip.length);
    }
    #endregion
}
