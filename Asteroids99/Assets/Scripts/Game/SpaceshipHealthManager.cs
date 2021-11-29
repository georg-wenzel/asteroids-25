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
    private float iframes;
    #endregion

    #region properties
    /// <summary>
    /// Defines how much HP the ship starts out with.
    /// Does NOT update with the true HP of the ship. Use the observable for this.
    /// </summary>
    public int StartingHP = 3;
    #endregion

    #region methods
    void Awake()
    {
        observers = new HashSet<IHPObserver>();
    }

    void Start()
    {
        this.hp = StartingHP;
        this.iframes = 0;
    }
    
    void Update()
    {
        //reduce iframes if they are > 0
        if (iframes > 0.0f)
        {
            iframes -= Time.deltaTime;
            if (iframes <= 0.0f)
                EndInvulnerability();
        }
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
            o.UpdateHP(this.hp);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Asteroid") && iframes == 0)
        {
            //register a hit if HP is > 0 and notify the observers 
            if (this.hp > 0)
                this.hp -= 1;
            notifyAll();

            //start invulnerability
            StartInvulnerability();
        }
    }

    private void StartInvulnerability()
    {
        this.iframes = 1.0f;
        this.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.5f);
    }

    private void EndInvulnerability()
    {
        this.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 0.5f);
        iframes = 0;
    }
    #endregion
}
