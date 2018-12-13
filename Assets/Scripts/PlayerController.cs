using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Animator animator;
    /*public Collider floorPlane;//in this demonstration this is set manually, the Retail Ability system has methods for dealing with this automatically via data structures for environments
    public Collider attackPlane;
    public Enemy[] enemies;//this array is filled during START by searching for prefabs that have the enemy script attached to them
    public Transform hitReport;//this is for a text mesh object that tells us what damage we did...we need to know where this instance is so we can instantiate off of it
    public Transform particleHit;//this is for a particle emmiter that shows us a hit...we need to know where this instance is so we can instantiate off of it
                                 //the retail ability system will have this "Inside" an abilities class structual data

    public List<Ability> abilities = new List<Ability>();//not really actual abilities as yet, HERE they are only attacks...the retail Ability System package will have actual ones 
    int ahc = 1; //ability hit counter, combo attacks have more than one hit, this variable keeps track of how many hits we have used in update

    public bool hitCheck;//<<< IMPORTANT mecanim tells us to perform a hit check at a specific point in attack animations by settting this to TRUE

    public int WeaponState = 0;//unarmed, 1H, 2H, bow, dual, pistol, rifle, spear and ss(sword and shield)
    public bool wasAttacking;// we need this so we can take lock the direction we are facing during attacks, mecanim sometimes moves past the target which would flip the character around wildly

    public Renderer movementTarget;
    Transform destFloor;

    float rotateSpeed = 20.0f; //used to smooth out turning

    public Vector3 attackPos;
    public Vector3 lookAtPos;
    //float				gravity = -0.3f;//unused in this demonstration
    float fallspeed = 0.0f;
    */
    const int           HERO_Up = 0;
    const int           HERO_Right = 1;
    const int           HERO_Down = 2;
    const int           HERO_Left = 3;

    int                 state = 0;
    float               m_speed = 5.0f;

    public bool rightButtonDown = false;//we use this to "skip out" of consecutive right mouse down input...


    // Use this for initialization
    void Start () {
        animator = GetComponentInChildren<Animator>();//need this...
        //enemies = Transform.FindObjectsOfType(typeof(Enemy)) as Enemy[];//find all the instances of the enemy script which are attached to the targets
    }
	
	// Update is called once per frame
	void Update () {
        /*if (hitCheck)//hitCheck is a boolean variable, it gets set by mecanim attack states, if mecanim set it...then we need to do hit checks right now
        {

            if (ahc < 1) ahc = 1;//we may have a "double pulse" coming from Mecanim so...
            AbilityCollision abilColl = new AbilityCollision();
            abilColl = abilities[WeaponState].collChecks[abilities[WeaponState].collChecks.Count - ahc];
            if (abilColl.type == 0)
            {
                //ANGLE RANGE which can be used for any angle/range including radial attacks
                for (int i = 0; i < enemies.Length; i++)//loop throught the enemies
                {
                    CheckForHit(enemies[i], abilColl);//ahc= ability hit counter, which is used for indexing a a one tow three punch combo for example...
                }
                ahc -= 1;// some abilities have multiple checks, so when we use an ability, we set ahc to the number of hits in the ability (combo punches for example)
            }
            else if (abilColl.type == 1)
            {
                //Missiles
                //are a special type, my enemies are its enemies, my damage is its damage, it needs to know who I am, and which of my abilities used it this time
                Transform tm = (Transform)Instantiate(abilColl.missile, abilColl.missile.position, abilColl.missile.rotation);
                tm.gameObject.SetActive(true);
                Missile missile = tm.GetComponent<Missile>();
                missile.speed = abilColl.speed;
                missile.damage = abilColl.damage;
                missile.enemies = enemies;
                missile.abc = this;
            }
            if (abilColl.type == 2)
            {
                //Beams/Bullets
                Transform tm = (Transform)Instantiate(abilColl.missile, abilColl.missile.position, abilColl.missile.rotation);
                tm.gameObject.SetActive(true);
                RayShot rayshot = tm.GetComponent<RayShot>();
                rayshot.damage = abilColl.damage;
                rayshot.enemies = enemies;
                Vector3 tempPos = attackPos;
                tempPos.y = abilColl.missile.position.y;
                Vector3 tempdelta = Vector3.Normalize(tempPos - abilColl.missile.position); //this is the actual vector from the source point to the attack point normalized
                rayshot.endPos = tempdelta * abilColl.range;
                rayshot.abc = this;
            }

            hitCheck = false;// we are done checking, reset the hitCheck bool
        }*/

        /*float KeyVertical = Input.GetAxis("Vertical");
        float KeyHorizontal = Input.GetAxis("Horizontal");

        if (KeyVertical == -1)
        {
            setHeroState(HERO_Left);
            //wasAttacking = false;
        }
        else if (KeyVertical == 1)
        {
            setHeroState(HERO_Right);
            //wasAttacking = false;
        }
        else if (KeyHorizontal == 1)
        {
            setHeroState(HERO_Down);
            //wasAttacking = false;
        }
        else if (KeyHorizontal == -1)
        {
            setHeroState(HERO_Up);
            //wasAttacking = false;
        }

        if (KeyVertical == 0 && KeyHorizontal == 0)
        {

        }*/
        if (Input.GetKey(KeyCode.W))
        {
            gameObject.GetComponent<Transform>().Translate(Vector3.forward * 0.1f, Space.Self);
        }
        if (Input.GetKey(KeyCode.S))
        {
            gameObject.GetComponent<Transform>().Translate(Vector3.back * 0.1f, Space.Self);
        }
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.GetComponent<Transform>().Translate(Vector3.left * 0.1f, Space.Self);
        }
        if (Input.GetKey(KeyCode.D))
        {
            gameObject.GetComponent<Transform>().Translate(Vector3.right * 0.1f, Space.Self);
        }
        //跳跃
        if (Input.GetKey(KeyCode.Space))
        {
            gameObject.GetComponent<Transform>().Translate(Vector3.up * Time.deltaTime * m_speed);
        }
        //转向
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            gameObject.GetComponent<Transform>().Rotate(0f, -2f, 0f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            gameObject.GetComponent<Transform>().Rotate(0f, 2f, 0f);
        }
    }

    /*void CheckForHit(Enemy en, AbilityCollision ac)
    {
        //AngleRanged
        if (ac.type == 0)
        {
            float angle = ac.angle / 2;
            Vector3 tDelta = en.gameObject.transform.position - transform.position;
            float tAngle = Vector3.Angle(transform.forward, tDelta);
            if (tAngle < 0) tAngle *= -1;
            if (tAngle < angle)
            {
                if (Vector3.Distance(transform.position, en.gameObject.transform.position) < ac.range)
                {
                    //we have a hit
                    //AngleRanged
                    Transform tm = (Transform)Instantiate(hitReport, (en.gameObject.transform.position + new Vector3(0.0f, 1.6f, 0.0f)), Quaternion.identity);
                    tm.gameObject.SetActive(true);
                    Hit tmHit = tm.GetComponent<Hit>();
                    tmHit.text = ac.damage.ToString();
                    Transform ph = (Transform)Instantiate(particleHit, (en.gameObject.transform.position + new Vector3(0.0f, 1.5f, 0.0f)), Quaternion.identity);
                    ph.transform.LookAt(Camera.main.transform.position);
                    ph.transform.position += (ph.transform.forward * 2.0f);
                    ph.gameObject.SetActive(true);
                }
            }
        }
        return;
    }*/

    /*public void setHeroState(int newState)
    {
        int rotateValue = (newState - state) * 90;
        Vector3 transformValue = new Vector3();

        switch (newState)
        {
            case HERO_Up:
                transformValue = Vector3.forward * Time.deltaTime;
                break;
            case HERO_Down:
                transformValue = (-Vector3.forward) * Time.deltaTime;
                break;
            case HERO_Left:
                transformValue = Vector3.left * Time.deltaTime;
                break;
            case HERO_Right:
                transformValue = (-Vector3.left) * Time.deltaTime;
                break;
        }

        transform.Rotate(Vector3.up * 0.1f, rotateValue);
        transform.Translate(transformValue * m_speed, Space.World);

        state = newState;
    }*/
 }
