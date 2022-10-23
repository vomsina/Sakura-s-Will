using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private string playerTag;
    [SerializeField] [Range(0.5f, 7.5f)] private float movingSpeed = 1.5f;
    [SerializeField] float leftLimit;
    [SerializeField] float rightLimit;
    [SerializeField] float bottomLimit;
    [SerializeField] float upperLimit;
    float o_leftlimit;
    float o_rightlimit;


    public GameObject hero;

    private void Awake()
    {
         o_leftlimit = leftLimit;
        o_rightlimit = rightLimit;
        if (this.playerTransform == null)
        {
            if (this.playerTag == "")
            {
                this.playerTag = "Player";
            }

            this.playerTransform = GameObject.FindGameObjectWithTag(this.playerTag).transform;
        }

        this.transform.position = new Vector3()
        {
            x = this.playerTransform.position.x,
            y = this.playerTransform.position.y+2f,
            z = this.playerTransform.position.z - 10,

        };
  
    }

    private void FixedUpdate()
    {
        if (this.playerTransform)
        {
            Vector3 target = new Vector3()
            {
                x = this.playerTransform.position.x,
                y = this.playerTransform.position.y+1.5f,
                z = this.playerTransform.position.z - 10,
            };
           
            Vector3 pos = Vector3.Lerp(this.transform.position, target, this.movingSpeed * Time.deltaTime);

            this.transform.position = pos;
            ;
            if (hero.transform.position.x > 27.8)
            {
                leftLimit = 34.99f;
                rightLimit = 40.6f;
            }
            if( hero.transform.position.x <27.8)
            {
                leftLimit = o_leftlimit;
                rightLimit = o_rightlimit;
            }
            this.transform.position = new Vector3
                (
                Mathf.Clamp(this.transform.position.x, leftLimit, rightLimit),
                Mathf.Clamp(this.transform.position.y, bottomLimit, upperLimit),
                this.transform.position.z);
        }
    }

}
