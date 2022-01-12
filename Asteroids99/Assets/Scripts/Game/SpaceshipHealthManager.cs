using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is responsible for collisions with Asteroids, as well as managing the ship's HP
/// and propagating it to interested Observers via an Observable Interface
/// </summary>
public class SpaceshipHealthManager : MonoBehaviour, IHPObservable
{
    #region fields
    /// <summary>
    /// The true HP of the ship
    /// </summary>
    private int hp;
    /// <summary>
    /// A list of HP observers
    /// </summary>
    private HashSet<IHPObserver> observers;
    /// <summary>
    /// The time in seconds the spaceship has active iframes
    /// </summary>
    private bool iframes;
    #endregion

    #region properties
    /// <summary>
    /// Audio to play when the spaceship gets hit
    /// </summary>
    public AudioClip SpaceshipHitSound;
    /// <summary>
    /// The Game Object to play a local audio clip.
    /// </summary>
    public GameObject LocalAudioPrefab;
    #endregion

    #region methods
    void Awake()
    {
        observers = new HashSet<IHPObserver>();
    }

    void Start()
    {
        this.hp = 3;
        this.iframes = false;
    }
 
    /// <summary>
    /// Add a HP observer
    /// </summary>
    /// <param name="observer">The observer to add</param>
    public void register(IHPObserver observer)
    {
        observers.Add(observer);
    }

    /// <summary>
    /// Remove a HP observer
    /// </summary>
    /// <param name="observer">The observer to remove</param>
    public void unregister(IHPObserver observer)
    {
        observers.Remove(observer);
    }

    /// <summary>
    /// Notify all HP observers of the current HP of the ship
    /// </summary>
    public void notifyAll()
    {
        foreach(IHPObserver o in observers)
        {
            if (o != null && this.gameObject != null)
                o.UpdateHP(this.hp);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Asteroid") && !iframes)
        {
            //start invulnerability
            StartCoroutine(InvulnerabilityFrames());

            //play hit audio
            GameObject go = GameObject.Instantiate(LocalAudioPrefab);
            go.transform.position = this.transform.position;
            go.GetComponent<LocalAudioScript>().Clip = SpaceshipHitSound;

            //register a hit if HP is > 0 and notify the observers 
            if (this.hp > 0)
                this.hp -= 1;
            notifyAll();
        }
    }

    IEnumerator InvulnerabilityFrames()
    {
        StartInvulnerability();
        yield return new WaitForSeconds(1f);
        EndInvulnerability();
        
    }

    private void StartInvulnerability()
    {
        this.iframes = true;
        this.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.5f);
    }

    private void EndInvulnerability()
    {
        this.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 0.5f);
        this.iframes = false;
    }
    #endregion
}
