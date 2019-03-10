using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;

    public float playerY;
    public float playerX;

    private Rigidbody2D rb2d;
    public float speed;
    public float jumpForce;
    private bool facingRight = true;
    Animator anim;

    public Text scoreText;
    public Text livesText;
    public Text loseText;
    public Text winText;

    private int score;
    private int lives;
    public int state;
    private int flag;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        score = 0;
        lives = 3;
        flag = 0;
        loseText.text = "";
        winText.text = "";
        
        setText();

        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        //lets you quit game
        if (Input.GetKey("escape"))
            Application.Quit();
        //starts run animation when <- or -> are pressed
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))&& ((state != 2)))
        {
            anim.SetInteger("State", 1);
            state = 1;
        }
        if((Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) && ((state != 2)))
        {
            anim.SetInteger("State", 0);
            state = 0;
        }
        //starts jump animation when up arrow pressed
        if(Input.GetKey(KeyCode.UpArrow))
        {
            anim.SetInteger("State", 2);
            state = 2;
        }
        /*
        if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            anim.SetInteger("State", 3);
            state = 3;
        }*/

        if(score == 8 && flag == 0)
        {
            winText.text = "You Win!";
            flag = 1;
        }

        if (flag == 1)
        {
            musicSource.Stop();
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            musicSource.loop = true;
            flag = 3;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        Vector2 movement = new Vector2(moveHorizontal, 0);

        rb2d.AddForce(movement * speed);

        //flip code
        if (facingRight == false && moveHorizontal > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveHorizontal < 0)
        {
            Flip();
        }


    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            if(Input.GetKey(KeyCode.UpArrow))
            {
                rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
    }

    //resets jump anim to idle upon colliding with ground
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            anim.SetInteger("State", 0);
            state = 0;
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {     //when coin encountered, 
        if (other.gameObject.CompareTag("coin"))
        {
            other.gameObject.SetActive(false); //deactivate coin
            score = score + 1; //increment score
            setText();
        }
            //when enemy encountered
        else if (other.gameObject.CompareTag("slime"))
        {
            other.gameObject.SetActive(false); //deactive enemy
            lives = lives - 1;  //decrement lives
            setText();
        }
        //kill player if lives reach 0
        if (lives == 0)
        {
            Destroy(this);
            Destroy(gameObject);
            loseText.text = "YOU LOSE";
        }

        //transfers to level 2 if 4 coins obtained, reset lives
        if (score == 4) 
        {
            transform.position = new Vector2(playerX, playerY);
            lives = 3;
            setText();
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
    }

    void setText()
    {
        scoreText.text = "Score: " + score.ToString();
        livesText.text = "Lives: " + lives.ToString();
    }

}
