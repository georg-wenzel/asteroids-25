using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the default behaviour for an asteroid
/// </summary>
public class BaseAsteroidBehaviour : MonoBehaviour, IAsteroidDeathObservable
{
    #region fields
    /// The corresponding asteroid properties of the GameObject
    /// </summary>
    private AsteroidProperties properties;
    /// <summary>
    /// The rigid body of the object
    /// </summary>
    private Rigidbody2D rigidbody2d;
    /// <summary>
    /// The desired magnitude of velocity
    /// </summary>
    private float velocityMagnitude;
    /// <summary>
    /// internal health of the asteroid
    /// </summary>
    private int health;
    /// <summary>
    /// The bounds of the playing field
    /// </summary>
    private GameBoundaries bounds;
    //all observers of this asteroid's death
    private HashSet<IAsteroidDeathObserver> observers;
    /// <summary>
    /// The particle system when an asteroid gets hit
    /// </summary>
    private ParticleSystem hitParticles;
    #endregion

    #region properties
    /// <summary>
    /// The sound that plays when the missile hits
    /// </summary>
    public AudioClip Hit;
    /// <summary>
    /// The sound that plays when an asteroid is destroyed
    /// </summary>
    public AudioClip Explosion;
    /// <summary>
    /// The Game Object to play a local audio clip.
    /// </summary>
    public GameObject LocalAudioPrefab;
    /// <summary>
    /// A vector which stores the direction of impact for observers to read before death (this is 0/0 until the asteroid is hit by a missile which destroys it)
    /// </summary>
    public Vector2 ImpactDirection { get; private set; }
    #endregion

    #region methods
    public void Awake()
    {
        observers = new HashSet<IAsteroidDeathObserver>();
    }

    public void Start()
    {
        //get components of this game object
        rigidbody2d = GetComponent<Rigidbody2D>();
        properties = GetComponent<AsteroidProperties>();
        hitParticles = GetComponent<ParticleSystem>();

        Vector2 direction;
        //if no direction is given in the properties
        if (properties.InitialMovementDirection.magnitude > 0)
            //use this vector
            direction = properties.InitialMovementDirection.normalized;
        else
            //define a random movement vector this asteroid travels in
            direction = new Vector2(Random.Range(0.0f, 10.0f), Random.Range(0.0f, 10.0f)).normalized;

        //add the initial force and store the desired magnitude of velocity
        rigidbody2d.AddForce(direction * properties.Speed, ForceMode2D.Impulse);
        velocityMagnitude = rigidbody2d.velocity.magnitude;

        //Set a default value for the impact direction
        ImpactDirection = new Vector2(0, 0);

        //add a small random spin to the asteroid
        rigidbody2d.AddTorque(Random.Range(0.0f,5.0f));

        //get initial health from properties
        this.health = properties.InitialHealth;

        //inject game bounds
        bounds = GameObject.Find("GameView").GetComponent<GameBoundaries>();

        StartCoroutine(VelocityMagnitudeCoroutine());
    }

    /// <summary>
    /// Coroutine which updates the velocity magnitude to stay constant after a collision
    /// </summary>
    IEnumerator VelocityMagnitudeCoroutine()
    {
        for (; ; )
        { 
            //if the magnitude of velocity changes from the desired (after a collision)
            var magnitudeDiff = Mathf.Abs(velocityMagnitude - rigidbody2d.velocity.magnitude);
            if (magnitudeDiff > 0.001f)
            {
                //make sure the velocity stays constant
                rigidbody2d.velocity = rigidbody2d.velocity.normalized * velocityMagnitude;
            }

            yield return new WaitForSeconds(.25f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //On collision with a missile
        if (collision.collider.gameObject.tag.Equals("Missile"))
        {
            //Spawn an object for sound
            GameObject go = GameObject.Instantiate(LocalAudioPrefab);
            go.transform.position = this.transform.position;

            var missileDirection = collision.collider.gameObject.GetComponent<MissileLogic>().MovementDirection;

            health--;
            if (health == 0)
            {
                var script = go.GetComponent<LocalAudioScript>();
                script.Clip = Explosion;
                script.ParticleSystem = true;
                //If the asteroid was large
                if(this.transform.localScale.x > 1)
                {
                    //Spawn large particle system
                    script.Particles = 1000;
                }    
                else
                {
                    //Spawn small particle system
                    script.Particles = 100;
                }
                //Set the impact vector
                this.ImpactDirection = missileDirection;
                Destroy(this.gameObject);
            }
            else
            {
                go.GetComponent<LocalAudioScript>().Clip = Hit;
                var collisionAngle = Vector2.SignedAngle(new Vector3(1,0), new Vector2(-missileDirection.x, -missileDirection.y));
                var shape = hitParticles.shape;
                //30 is half the angle of the cone in which particles can spawn in the particle system
                shape.rotation = new Vector3(0, 0, collisionAngle - transform.rotation.eulerAngles.z - 30);
                hitParticles.Play();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Not sure why this sometimes happens, but this should not break anything ever, as all asteroids travel inwards first.
        if (collision == null || bounds == null) return;

        //General sanity check - if we hit the outer borders (asteroid escaped)
        if (collision.Equals(bounds.OuterBounds))
        {
            //Define a random target point inside the center 80% of the game field.
            Vector2 targetPoint = new Vector2(
                Random.Range(bounds.LeftBorder * 0.8f, bounds.RightBorder * 0.8f),
                Random.Range(bounds.BottomBorder * 0.8f, bounds.TopBorder * 0.8f));

            //Change velocity towards this point
            rigidbody2d.velocity = (targetPoint - rigidbody2d.position).normalized * velocityMagnitude;
        }
    }

    private void OnDestroy()
    {
        notifyAll();
    }

    /// <summary>
    /// Register a death observer
    /// </summary>
    /// <param name="observer">The observer to add</param>
    public void register(IAsteroidDeathObserver observer)
    {
        observers.Add(observer);
    }

    /// <summary>
    /// Unregister a death observer
    /// </summary>
    /// <param name="observer">The observer to remove</param>
    public void unregister(IAsteroidDeathObserver observer)
    {
        observers.Remove(observer);
    }

    /// <summary>
    /// Notify all observers about this asteroid's death
    /// </summary>
    public void notifyAll()
    {
        foreach(IAsteroidDeathObserver o in observers)
        {
            if(o != null && this.gameObject != null)
                o.NotifyDeath(this.gameObject);
        }
    }
    #endregion
}
