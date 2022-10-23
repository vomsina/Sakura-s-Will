using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Entity
{
    public int coins = 0;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;
    private Transform tr;
    private float dirX = 0f;
    [SerializeField] private float movespeed = 4f;
    [SerializeField] private float jumpForce = 5f;
    int isJump = 0;
    private bool _isGround;
    private bool pressE = false;
    public bool isAttack = false;
    public bool isRecharge = true;

    public Transform attackPos;
    public float attackRabge;
    public LayerMask enemy;
    public GameObject Sword;
    MovementState state = 0;

    public GameObject Shop;
    public GameObject Settings;
    public GameObject healthpoints;
    public GameObject GameOver;

    public AudioSource music;
    public AudioSource effects_coin;
    public AudioSource menu_buttons_effects;
    public AudioSource menu_buttons_click;
    public Slider slider_music;
    public Slider slider_effects;

    public GameObject f5_;
    public GameObject f1_;

    public Text coin_text;

    public static bool load = false;

    public bool isPause;
    private enum MovementState { idle, run, jump, fall, sprint, attack }


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        isRecharge = true;
        OnLoad();
    }


    public void OnLoad()
    {
        BinaryReader sr = new BinaryReader(File.Open("music.dat", FileMode.Open));
        music.volume = (float)sr.ReadDouble();
        sr.Close();
        slider_music.value = music.volume;
        Debug.Log(music.volume);
        BinaryReader sr_ = new BinaryReader(File.Open("effects.dat", FileMode.Open));
        effects_coin.volume = (float)sr_.ReadDouble();
        sr_.Close();
        menu_buttons_click.volume = effects_coin.volume;
        menu_buttons_effects.volume = effects_coin.volume;
        slider_effects.value = effects_coin.volume;
        if (load)
        {
            BinaryReader hero_load = new BinaryReader(File.Open("hero_load.dat", FileMode.Open));
            Vector3 load_pos = new Vector3()
            {
                x = (float)hero_load.ReadDouble(),
                y = (float)hero_load.ReadDouble(),
                z = (float)hero_load.ReadDouble(),
            };
            transform.position = load_pos;
            coins = (int)hero_load.ReadDouble();
            health = (int)hero_load.ReadDouble();
            hero_load.Close();
        }
        else
        {
            health = 100;
        }

    }

    private void Attack()
    {
        if (isRecharge && !isPause)
        {
            OnAttack();
            Sword.SetActive(true);
            state = MovementState.attack;
            isAttack = true;
            isRecharge = false;

            StartCoroutine(AttackAnimation());

            StartCoroutine(AttackCoolDown());

        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPos.position, attackRabge);
    }

    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.4f);
        isAttack = false;
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isRecharge = true;
    }

    private void OnAttack()
    {

        if (isRecharge && !isPause)
        {
            isRecharge = false;
            isAttack = true;
            Sword.SetActive(true);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRabge, enemy);
            for (int i = 0; i < colliders.Length; i++)
            {

                colliders[i].GetComponent<Entity>().GetDamage();
            }


        }
    }
    public override void GetDamage()
    {
        if (!isPause)
        {

            health -= 5;
            if (health <= 0)
            {
                GameOver.SetActive(true);
            }
        }
    }

    private void Healthpoints_update()
    {
        if (health > 20)
        {
            GameObject Child1 = healthpoints.gameObject.transform.Find("healthpoint 1").gameObject;
            Child1.SetActive(true);
            GameObject Child0 = healthpoints.gameObject.transform.Find("healthpoint 0").gameObject;
            Child0.SetActive(false);
            if (health > 20)
            {
                GameObject Child2 = healthpoints.gameObject.transform.Find("healthpoint 2").gameObject;
                Child2.SetActive(true);
                if (health > 40)
                {
                    GameObject Child3 = healthpoints.gameObject.transform.Find("healthpoint 3").gameObject;
                    Child3.SetActive(true);
                    if (health > 60)
                    {
                        GameObject Child4 = healthpoints.gameObject.transform.Find("healthpoint 4").gameObject;
                        Child4.SetActive(true);
                        if (health > 75)
                        {
                            GameObject Child5 = healthpoints.gameObject.transform.Find("healthpoint 5").gameObject;
                            Child5.SetActive(true);
                            if (health > 85)
                            {
                                GameObject Child6 = healthpoints.gameObject.transform.Find("healthpoint 6").gameObject;
                                Child6.SetActive(true);
                            }
                            else
                            {
                                GameObject Child6 = healthpoints.gameObject.transform.Find("healthpoint 6").gameObject;
                                Child6.SetActive(false);
                            }
                        }
                        else
                        {
                            GameObject Child5 = healthpoints.gameObject.transform.Find("healthpoint 5").gameObject;
                            Child5.SetActive(false);
                        }
                    }
                    else
                    {
                        GameObject Child4 = healthpoints.gameObject.transform.Find("healthpoint 4").gameObject;
                        Child4.SetActive(false);
                    }
                }
                else
                {
                    GameObject Child3 = healthpoints.gameObject.transform.Find("healthpoint 3").gameObject;
                    Child3.SetActive(false);
                }
            }
            else
            {
                GameObject Child2 = healthpoints.gameObject.transform.Find("healthpoint 2").gameObject;
                Child2.SetActive(false);
            }
        }
        else
        {
            GameObject Child2 = healthpoints.gameObject.transform.Find("healthpoint 2").gameObject;
            Child2.SetActive(false);
            GameObject Child1 = healthpoints.gameObject.transform.Find("healthpoint 1").gameObject;
            Child1.SetActive(false);
            if (health > 0)
            {
                GameObject Child0 = healthpoints.gameObject.transform.Find("healthpoint 0").gameObject;
                Child0.SetActive(true);
            }
            else
            {

                GameObject Child0 = healthpoints.gameObject.transform.Find("healthpoint 0").gameObject;
                Child0.SetActive(false);
            }
        }
    }

    private void Update()
    {

        if (isAttack) movespeed = 0;
        Healthpoints_update();
        if (!isPause) dirX = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump") && (_isGround || (isJump != 1)) && !isPause)
        {

            if (!_isGround) rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.85f);
            else rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJump++;
            Debug.Log(isJump);
            state = MovementState.jump;

        }
        if (_isGround) isJump = 0;
        KeyDown_check();
        UpdateAnumationUpdaty();
        coin_text.text = Convert.ToString(coins);
    }

    private void KeyDown_check()
    {
        if (pressE && Input.GetKeyDown(KeyCode.E) && !isPause)
        {
            Shop.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject healthbar = GameObject.Find("healthbar");
            GameObject coinhbar = GameObject.Find("coin_bar");
            healthbar.SetActive(false);
            coinhbar.SetActive(false);
            Settings.SetActive(true);
            isPause = true;
            menu_buttons_click.PlayOneShot(menu_buttons_click.clip);
        }


        if (Settings.active == false)
        {
            isPause = false;
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Save_hero();
            f5_.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.F1))
        {
            f1_.SetActive(true);

        }
    }

    private void Save_hero()
    {
        BinaryWriter sw = new BinaryWriter(File.Open("hero_load.dat", FileMode.OpenOrCreate));
        sw.Write((double)transform.position.x);
        sw.Write((double)transform.position.y);
        sw.Write((double)transform.position.z);
        sw.Write((double)coins);
        sw.Write((double)health);
        sw.Close();
    }

    private void UpdateAnumationUpdaty()
    {

        if (!isAttack) { state = MovementState.idle; Sword.SetActive(false); }
        if (dirX > 0f && !isAttack)
        {
            Sword.SetActive(false);
            if (Input.GetButton("run"))
            {
                movespeed = 0.8f;
                state = MovementState.run;
            }
            else { state = MovementState.sprint; movespeed = 2.5f; }

            Quaternion target1 = new Quaternion()
            {
                x = 0,
                y = 0,
                z = 0,
            };
            this.transform.rotation = target1;



        }

        else if (dirX < 0f && !isAttack)
        {
            Sword.SetActive(false);
            if (Input.GetButton("run"))
            {
                movespeed = 0.8f;
                state = MovementState.run;
            }
            else { state = MovementState.sprint; movespeed = 2.5f; }
            Quaternion target = new Quaternion()
            {
                x = 0,
                y = 200,
                z = 0,
            };
            this.transform.rotation = target;
        }
        else if (!isAttack && _isGround)
        {
            Sword.SetActive(false);
            state = MovementState.idle;
        }
        if (rb.velocity.y > 1f && !isAttack)
        {
            Sword.SetActive(false);
            state = MovementState.jump;
        }
        else if (rb.velocity.y < -1f && !isAttack)
        {
            Sword.SetActive(false);
            state = MovementState.fall;
        }
        if (Input.GetMouseButton(1)) { Attack(); }
        rb.velocity = new Vector2(dirX * movespeed, rb.velocity.y);
        animator.SetInteger("State", (int)state);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) _isGround = true;
        if (collision.gameObject.CompareTag("Coin"))
        {
            coins++;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Grib"))
        {
            collision.gameObject.transform.Find("Press E").gameObject.SetActive(true);
            pressE = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground"))
        {
            if (collision.gameObject.CompareTag("Grib"))
                collision.gameObject.transform.Find("Press E").gameObject.SetActive(false);
            else collision.gameObject.SetActive(false);
        }
        else _isGround = false;
        Shop.SetActive(false);
    }

    public void heal()
    {
        if (coins >= 5 && health < 100)
        {
            coins -= 5;
            health += 30;
            Shop.SetActive(false);
        }
    }

    public void exit_shop()
    {
        Shop.SetActive(false);
    }
}
