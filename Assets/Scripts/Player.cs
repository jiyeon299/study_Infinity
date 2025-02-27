using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro.EditorUtilities;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Vector3 startPosition;
    private Vector3 oldPosition;
    private bool isTurn = false;

    private int moveCnt = 0;
    private int turnCnt = 0;
    private int spawnCnt = 0;

    private bool isDie = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Init();
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetMouseButtonDown(1))
        {
            CharTurn();
        }
       else if(Input.GetMouseButtonDown(0))
        {
            CharMove();
        }
    }

    private void Init()
    {
        anim.SetBool("Die", false);
        startPosition = transform.position;
        oldPosition = transform.localPosition;
        moveCnt = 0;
        spawnCnt = 0;
        turnCnt = 0;
        isTurn = false;
        spriteRenderer.flipX = isTurn;
        isDie = false;
    }

    private void CharTurn()
    {
        isTurn = isTurn == true ? false : true;

        spriteRenderer.flipX = isTurn;
    }
    private void CharMove()
    {
        moveCnt++;
        MoveDirection();

        if(isFailTurn()) //잘못된 방향으로 가면 사망
        {
            CharDie();
            return;
        }

        if(moveCnt > 5)
        {
            // 계단 스폰
            RespawnStair();
        }

    }

    private void MoveDirection()
    {
        if (isTurn) //Left
        {
            oldPosition += new Vector3(-0.75f, 0.5f, 0);
        }
        else
        {
            oldPosition += new Vector3(0.75f, 0.5f, 0);
        }
        transform.position = oldPosition;
        anim.SetTrigger("Move");
    }

    private bool isFailTurn()
    {
        bool resurt = false;

        if (GameManager.Instance.isTurn[turnCnt] != isTurn)
        {
            resurt = true;
        }

        turnCnt++;

        if(turnCnt > GameManager.Instance.Stairs.Length - 1) // 0~19 Length
        {
            turnCnt = 0;
        }

        return resurt;
    }

    private void RespawnStair()
    {
        GameManager.Instance.SpawnStair(spawnCnt);
        spawnCnt++;

        if(spawnCnt > GameManager.Instance.Stairs.Length - 1)
        {
            spawnCnt = 0;
        }
    }

    private void CharDie()
    {
        anim.SetBool("Die", true);
        isDie = true;
    }
}
