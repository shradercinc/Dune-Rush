using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using MyBox;
using UnityEditor;

public class DrillPlayerMovement : MonoBehaviour
{
    PlayerInput myPlayerInput;
    float inputX;
    Rigidbody2D rb;
    [Foldout("Speed Variables", true)]
    [SerializeField] float speed, gravity, jumpStartingPush, jumpHeldTimerMax, fallSpeedMax, jumpPreloadTimerMax, coyoteTimerMax, apexTimerMax;
    [SerializeField] float aimTurnSpeed, AimTimeMax, slamSpeed, initialSlamSpeed, drillSpeedRec, drillAcceleration, drillStartSpeed, drillSpeedMax, drillTurnSpeed, rotationCorrection, DrillVerticalBoost, DrillBoostPower, drillSpeedBoostConversion, Friction, reboundEfficiency, reboundPenalty, fuelMax, fuelBoost, slamFuelConsumption;
    [SerializeField] Vector2 velocity;

    bool jumpHeld, jumpTriggered, apexReached, enteredPlatformJumpNotResetYet;
    bool onPlatform, jumping;
    public bool canSlam, slamAiming, drilling;
    public bool slamming;
    float jumpHeldTimer, jumpPreloadTimer, coyoteTimer, apexTimer, drillSpeed, Momentum, AimTimer, fuel, particleDelayTimer, platformsEntered;

    [Foldout("Drillbit Visuals", true)]
    [SerializeField] GameObject drillBit;
    [SerializeField] GameObject drillBlades;
    [SerializeField] float offSetXMax, offSetYMax, rotationMax, speedShakeRatio;

    float gold = 0;
    [Foldout("Score")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameOverController LevelGameOverCon;

    [Foldout("Particle", true)]
    [SerializeField] GameObject particle;
    [SerializeField] Color particleColor;
    [SerializeField] Color ExhaustColor = new Color(0.1f, 0.1f, 0.1f, 1);
    [SerializeField] Color ExhaustColorBloom;
    [SerializeField] float particleDelayMax, particleCount;

    [Foldout("Bar Visuals")]
    [SerializeField] Image StaminaBar, fuelBar;


    [SerializeField] BoxCollider2D footBoxCollider;
    CapsuleCollider2D capsuleCollider;



    [SerializeField] TMP_Text gameOver;

    [Foldout("Sounds", true)]
    [SerializeField] AK.Wwise.Event drillSound;
    [SerializeField] AK.Wwise.Event drillEnd;
    [SerializeField] AK.Wwise.Event treasureSound;
    [SerializeField] AK.Wwise.Event fuelSound;

    private void Start()
    {
        //get access to the PlayerInput and Rigidbody2D components
        myPlayerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        canSlam = true;
        AimTimer = AimTimeMax;
        fuel = fuelMax;
        scoreText.text = "Gold: " + gold + "$";
        //gameOver.gameObject.SetActive(false);
    }

    //all our input logic and one-time actions that are based on inputs are in update
    private void Update()
    {

        if (!slamAiming)
        {
            AimTimer = AimTimeMax;
        }
        //get values from inputs
        if (AimTimer < 0)
        {
            AimTimer = 0;
        }
        StaminaBar.fillAmount = AimTimer / AimTimeMax;
        fuelBar.fillAmount = fuel / fuelMax;

        inputX = myPlayerInput.actions["Move"].ReadValue<Vector2>().x; //analog stick/WASD horizontal input for left/right movement
        jumpHeld = myPlayerInput.actions["Jump"].ReadValue<float>() > 0.9f; //is the jump button being held down this frame?
        jumpTriggered = myPlayerInput.actions["Jump"].triggered; //was the jump button just pressed this frame?

        //if I just pressed the jump button
        if (jumpTriggered)
        {
            if (!onPlatform)
            { //if I'm in the air
                if (coyoteTimer > 0) BeginJump(); //if I just left the ground, I have a few milliseconds to still jump
                else
                {
                    if (!capsuleCollider.isTrigger && canSlam)
                    {
                        slamAiming = true;
                    }
                    jumpPreloadTimer = jumpPreloadTimerMax; //if I'm in the air and I press the jump button, preload it so I can jump if I land very soon
                }
            }
            else
            { //if I'm on the ground, being a jump
                BeginJump();
            }
        }

        if (slamAiming)
        {
            AimTimer -= Time.deltaTime;
            if (AimTimer <= 0) slamAiming = false;
            if (myPlayerInput.actions["Jump"].WasReleasedThisFrame())
            {
                print("released");
                canSlam = false;
                slamAiming = false;
                slamming = true;
                createParticles(particleCount * 2, ExhaustColor, new Vector3(drillBit.transform.localPosition.x, drillBit.transform.localPosition.y - 1, drillBit.transform.localPosition.z - 2));
                createParticles(particleCount * 2, ExhaustColor, new Vector3(drillBit.transform.localPosition.x, drillBit.transform.localPosition.y + 1, drillBit.transform.localPosition.z - 2));
                createParticles(particleCount * 2, ExhaustColor, new Vector3(drillBit.transform.localPosition.x, drillBit.transform.localPosition.y, drillBit.transform.localPosition.z - 2));
                createParticles(particleCount * 2, ExhaustColorBloom, new Vector3(drillBit.transform.localPosition.x, drillBit.transform.localPosition.y, drillBit.transform.localPosition.z - 1));
                fuel -= slamFuelConsumption;
                float boostAngle = (transform.localEulerAngles.z - 90) * Mathf.Deg2Rad;
                float boostY = Mathf.Sin(boostAngle);
                float boostX = Mathf.Cos(boostAngle);
                Momentum = boostX * initialSlamSpeed;
                velocity.y += boostY * initialSlamSpeed;
                drillSpeed = 0;


                //velocity.y = -initialSlamSpeed;
                footBoxCollider.isTrigger = true;
                capsuleCollider.isTrigger = true;
            }
        }
        else
        {
            
        }

        if (drilling)
        {

            if (fuel > 0)
            {
                transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z - inputX * drillTurnSpeed * Time.deltaTime);
            }
            else
            {
                if (drillSpeed == 0)
                {
                    LevelGameOverCon.startGameOver(gold.RoundToInt());
                    //gameOver.gameObject.SetActive(true);
                    drillEnd.Post(gameObject);
                }

            }
            shaking();
            particleDelayTimer -= Time.deltaTime;
            if (particleDelayTimer < 0 && drillSpeed > 0)
            {
                particleDelayTimer = particleDelayMax;
                createParticles(particleCount, particleColor, new Vector3(drillBit.transform.localPosition.x, drillBit.transform.localPosition.y, drillBit.transform.localPosition.z - 1));
            }
        }
        else if (slamAiming)
        {
            transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z - inputX * aimTurnSpeed * Time.deltaTime);
        }
        else
        {
            if (Mathf.Abs(transform.localEulerAngles.z) - rotationCorrection * Time.deltaTime > 0)
            {
                if (transform.localEulerAngles.z < 180)
                {
                    transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z - rotationCorrection * Time.deltaTime);
                }
                else
                {
                    transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + rotationCorrection * Time.deltaTime);
                }
                
            }
            else
            {
                transform.localEulerAngles = new Vector3(0, 0, 0);
            }

        }

        //lower these timers at the end of each frame
        jumpPreloadTimer -= Time.deltaTime;
        coyoteTimer -= Time.deltaTime;
        //drillBlades.transform.position = drillBit.transform.position - Vector3.back;
    }

    void shaking()
    {
        float offSetX = drillSpeed * speedShakeRatio * Random.Range(-offSetXMax, offSetXMax);
        float offSetY = drillSpeed * speedShakeRatio * Random.Range(-offSetYMax, offSetYMax);
        drillBit.transform.localPosition = new Vector3(offSetX, offSetY, 0);

        float angle = drillSpeed * speedShakeRatio * Random.Range(-rotationMax, rotationMax);
        drillBit.transform.localRotation = Quaternion.Euler(0, 0, angle);
        if (drillSpeed == 0)
        {
            drillBit.transform.localPosition = Vector3.zero;
            drillBit.transform.localRotation = Quaternion.identity;
        }
    }

    void createParticles(float count, Color ParticleColor, Vector3 offset)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject myParticle = Instantiate(particle, transform.position + offset + new Vector3(Random.Range(-0.8f, 0.8f), Random.Range(-0.8f, 0.8f), 0), Quaternion.identity);
            myParticle.GetComponent<SpriteRenderer>().color = ParticleColor;    
        }
    }

    //all our physics logic is in fixedupdate
    private void FixedUpdate()
    {
        //set velocity based on inputX (which is a -1 to 1 value)
        velocity.x = inputX * speed + Momentum;
        Momentum *= Friction;

        //if I'm jumping and the jump button is still held down, don't apply any gravity and increase the jumpHeldTimer
        if (jumping && jumpHeldTimer < jumpHeldTimerMax)
        {
            if (jumpHeld) jumpHeldTimer += Time.fixedDeltaTime;
            else jumpHeldTimer = jumpHeldTimerMax;
        }
        else if (slamAiming)
        {
            velocity = Vector2.zero;
        }
        else if (slamming)
        {
            velocity.y -= gravity * Time.fixedDeltaTime;
            if (velocity.y < -fallSpeedMax) velocity.y = -fallSpeedMax;
            //velocity.y -= slamSpeed * Time.fixedDeltaTime;
            //velocity.x = 0;
            if (velocity.magnitude < drillSpeedRec)
            {
                slamming = false;
                print("burnout");
                footBoxCollider.isTrigger = false;
                capsuleCollider.isTrigger = false;

            }
        }
        else if (drilling)
        {
            fuel -= Time.fixedDeltaTime;
            velocity = Vector2.zero;

            if (fuel > 0) drillSpeed += drillAcceleration;
            else drillSpeed -= drillAcceleration;

            if (drillSpeed < 0)
            {
                drillSpeed = 0;
            }
            if (drillSpeed > drillSpeedMax)
            {
                drillSpeed = drillSpeedMax;
            }
            //rb.MovePosition(rb.position - (Vector2)transform.up * drillSpeed * Time.fixedDeltaTime);
            transform.Translate(Vector3.down * drillSpeed * Time.fixedDeltaTime);
            //transform.localPosition += Vector3.down * drillSpeed * Time.fixedDeltaTime;

        }
        //if I reached the apex, lower the gravity until the apexTimer runs out...
        else if (apexReached && apexTimer > 0)
        {
            apexTimer -= Time.fixedDeltaTime;
            velocity.y -= .5f * gravity * Time.fixedDeltaTime;
            //...and enable the footBoxCollider, which allows me to land on the edge of platforms
            if (apexTimer <= 0) footBoxCollider.enabled = true;
        }
        //if I'm not being pushed up by the jump button and I'm not in the apex, apply normal gravity
        else
        {
            velocity.y -= gravity * Time.fixedDeltaTime;
            if (velocity.y < -fallSpeedMax) velocity.y = -fallSpeedMax; //make sure fall speed never exceeds fallSpeedMax

            //but also keep checking if I reached the apex, and set apex values if it's true
            if (!apexReached && jumping && velocity.y <= 0)
            {
                apexReached = true;
                apexTimer = apexTimerMax;
            }
        }
        //move to the next position while checking all spaces between the current position and the next position
        //if(velocity.y < -fallSpeedMax) velocity.y = -fallSpeedMax;
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Begins a new jump.Sets velocity.y to jumpStartingPush, sets jumping to true, and sets jumpHeldTimer to 0. Disables footBoxCollider.
    /// </summary>
    void BeginJump()
    {
        velocity.y = jumpStartingPush;
        jumping = true;
        jumpHeldTimer = 0;
        footBoxCollider.enabled = false;
    }

    /// <summary>
    /// Call this when a jump ends. Resets jumping and apexReached to false.
    /// </summary>
    void ResetJump()
    {
        jumping = false;
        apexReached = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            /*
            When you enter a platform, set this value to true so that when you are on top of a platform, jump will get reset.
            This is here for an edge case where you enter a platform by sliding up its side but then land on top of the platform without ever leaving it.
            If we just reset the jump here, then it would get reset when you touch a wall. 
            But if we only reset when you land directly on top of a platform, then it wouldn't reset the jump when you slide up a wall onto the top of a platform.
            */
            enteredPlatformJumpNotResetYet = true;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Treasure")
        {
            gold += collision.GetComponent<treasure>().value;
            createParticles(50, collision.GetComponent<SpriteRenderer>().color, Vector3.zero);
            scoreText.text = "Gold: " + gold + "$";
            Destroy(collision.gameObject);
            treasureSound.Post(gameObject);
        }

        if(collision.gameObject.tag == "Obstruction") 
        {
            print("interaction");
            var collisionPoint = collision.ClosestPoint(transform.position);
            var collisionNormal = new Vector2(transform.position.x, transform.position.y) - collisionPoint;
            Debug.DrawRay(collisionPoint, collisionNormal * 100, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 1f);

            float attackAngle = (transform.localEulerAngles.z - 90) * Mathf.Deg2Rad;
            float AAngleX = Mathf.Sin(attackAngle);
            float AAngleY = Mathf.Cos(attackAngle);


            //print("Attack = " + AAngleX + "," + AAngleY);
            var reflectVector = Vector3.Reflect(new Vector3(AAngleX, AAngleY, 0), collisionNormal);
            //print("Reflect = " + reflectVector.x + "," + reflectVector.y);
            float angle = Vector3.SignedAngle(reflectVector, new Vector3(AAngleX, AAngleY, 0), Vector3.forward);
            if (Mathf.Abs(angle) < 45) angle = 180 - angle;
            print(angle);
            transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z - angle);
            drillSpeed *= reboundEfficiency;
            fuel -= reboundPenalty;
            //transform.forward = reflectVector;


            /*
            print("angle of change = " + angle);
            if (transform.localEulerAngles.z < 180)
            {
                //print("less than 180");
                print("old " + transform.localEulerAngles.z);
                transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + angle);
                print("new " + transform.localEulerAngles.z);
            } 
            else
            {
                //print("less than 180");
                print(transform.localEulerAngles.z);
                transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z - angle);
                print(transform.localEulerAngles.z);
            }
            */



        }

        if (collision.gameObject.tag == "Fuel")
        {
            fuelSound.Post(gameObject);
            fuel += fuelBoost;
            if (fuel > fuelMax)
            {
                fuel = fuelMax;
            }

            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Platform")
        {

            print("Entered " + platformsEntered);
            if (slamming)
            {
                drillSound.Post(gameObject);
                print("collided");
                slamming = false;

                velocity = Vector2.zero;
                rb.gravityScale = 0;
                drilling = true;
                if (platformsEntered == 0)
                {
                    drillSpeed = drillStartSpeed;
                }


            }
            platformsEntered++;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            if (slamming)
            {
                print("collided");
                slamming = false;
                velocity = Vector2.zero;
                rb.gravityScale = 0;
                drilling = true;
                drillSpeed = drillStartSpeed;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    { 
        if (collision.gameObject.tag == "Platform")
        {
            platformsEntered--;
            if (platformsEntered < 0) platformsEntered = 0;
            print("Left " + platformsEntered);
            if (platformsEntered == 0)
            {
                drillSpeed = 0;
                drillEnd.Post(gameObject);
            }
            drilling = false;
            slamming = true;
            rb.gravityScale = 1;
            //capsuleCollider.isTrigger = false;
            //footBoxCollider.isTrigger = false;
            if (transform.position.y > collision.transform.position.y - (collision.transform.localScale.y / 2))
            {
                velocity.y += DrillVerticalBoost;
            }
            //converts current downwards angle into a vector to apply to the rigidbody. the x vector is added to a degrading momentum so it can be factored into the general horizontal calculation in fixed update
            float boostAngle = (transform.localEulerAngles.z - 90) * Mathf.Deg2Rad;
            float boostY = Mathf.Sin(boostAngle);
            float boostX = Mathf.Cos(boostAngle);
            Momentum = boostX * (DrillBoostPower + drillSpeedBoostConversion * drillSpeed);
            velocity.y += boostY * (DrillBoostPower + drillSpeedBoostConversion * drillSpeed);
            canSlam = true;

            //resets vibrating drillbit
            drillBit.transform.localPosition = Vector3.zero;
            drillBit.transform.localRotation = Quaternion.identity;

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform") //if the object I am touching is a platform
        {
            foreach (ContactPoint2D contact in collision.contacts) //go through each contact point where I collided with this platform
            {
                //draw a debug ray in the Unity scene view that shows the normal of the point where I collided with the platform in Unity (not visible in play mode)
                Debug.DrawRay(contact.point, contact.normal * 100, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 1f);

                //am I standing on the platform vertically?
                if (Mathf.Abs(contact.normal.y) > Mathf.Abs(contact.normal.x))
                {
                    if (contact.normal.y > 0) //am I standing on top of the platform?
                    {
                        onPlatform = true;
                        if (velocity.y < 0) velocity.y = 0; //if I landed on the platform, make sure my velocity is not negative
                        if (jumpPreloadTimer > 0) BeginJump(); //if I pressed the jump button just before landing, jump now
                        if (enteredPlatformJumpNotResetYet)
                        { //see the long comment in OnCollisionEnter2D for why this is here
                            ResetJump();
                            enteredPlatformJumpNotResetYet = false;
                            canSlam = true;
                        }
                    }
                    else //am I touching the platform from below?
                    {
                        if (velocity.y > 0) velocity.y = 0; //if I bumped my head below the platform, make sure my velocity is no longer positive
                    }
                }
                else //did I hit the platform from the side?
                {
                    //TODO: check right vs left. Maybe use this for wall jump?
                }

            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform") //if the object I just stopped touching is a platform
        {
            if (onPlatform)
            {
                onPlatform = false; //I'm no longer on a platform
                coyoteTimer = coyoteTimerMax; //start the coyote timer so I can still jump for a few milliseconds after leaving the platform
                enteredPlatformJumpNotResetYet = false; //see the long comment in OnCollisionEnter2D for how enteredPlatformJumpNotResetYet works
            }
        }
    }
}
