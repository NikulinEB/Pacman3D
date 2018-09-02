using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PacmanController : MonoBehaviour {

    public event EventHandler PointIsEaten;
    public event EventHandler EnergizerIsEaten;
    public event EventHandler EnemyHit;
    public event EventHandler ScaredEnemyHit;

    public int CurrentX { get { return currentX; } }
    public int CurrentZ { get { return currentZ; } }
    
    private readonly int startX = 1; //координаты unity
    private readonly int startZ = -8;
    private readonly int startXpath = 14; //координаты в массиве path
    private readonly int startZpath = 7;
    private readonly int startDirectionX = -1;
    private readonly int startDirectionZ = 0;
    private int currentX; 
    private int currentZ;
    private int currentDirectionX;
    private int currentDirectionZ;
    private int nextDirectionX;
    private int nextDirectionZ;
    private readonly float startSpeed = 0.5f;
    private readonly float speedDelta = 0.02f;
    private readonly float maxSpeed = 0.25f;
    private float currentSpeed;
    private float time = 0;

    private Rigidbody pacmanRigidbody;

    void Start () {
        pacmanRigidbody = gameObject.GetComponent<Rigidbody>();
        ToStartPosition();
        ToStartSpeed();
    }
	
	void Update () {
        Move();
    }

    private void Move()
    {
        if (GameController.Instance.GameState == GameState.Playing)
        {
            time += Time.deltaTime;

            if (time >= currentSpeed && GameController.Instance.path[currentZ + currentDirectionZ, currentX + currentDirectionX] == 1)
            {
                pacmanRigidbody.MovePosition(new Vector3(pacmanRigidbody.position.x + currentDirectionX, 0, pacmanRigidbody.position.z + currentDirectionZ));
                currentX += currentDirectionX;
                currentZ += currentDirectionZ;
                time = 0;
                //телепортация через туннель в середине карты
                if (currentZ == 16)
                {
                    if (currentX == 0)
                    {
                        currentX = 26;
                        pacmanRigidbody.MovePosition(new Vector3(13, 0, 1));
                    }
                    else if (currentX == 27)
                    {
                        currentX = 1;
                        pacmanRigidbody.MovePosition(new Vector3(-12, 0, 1));
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                nextDirectionX = 0;
                nextDirectionZ = 1;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                nextDirectionX = -1;
                nextDirectionZ = 0;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                nextDirectionX = 0;
                nextDirectionZ = -1;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                nextDirectionX = 1;
                nextDirectionZ = 0;
            }
            //смена направления по возможности
            if (GameController.Instance.path[currentZ + nextDirectionZ, currentX + nextDirectionX] == 1)
            {
                currentDirectionX = nextDirectionX;
                currentDirectionZ = nextDirectionZ;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Point":
                other.gameObject.SetActive(false);
                if (PointIsEaten != null)
                    PointIsEaten(this, new EventArgs());
                break;
            case "Energizer":
                other.gameObject.SetActive(false);
                if (EnergizerIsEaten != null)
                    EnergizerIsEaten(this, new EventArgs());
                break;
            case "Enemy":
                var enemy = other.GetComponent<EnemyController>();
                if (enemy.Scared)
                {
                    enemy.ToStartPosition();
                    if (ScaredEnemyHit != null)
                    {
                        ScaredEnemyHit(this, new EventArgs());
                    }
                }
                else
                {
                    if (EnemyHit != null)
                    {
                        EnemyHit(this, new EventArgs());
                    }
                }
                break;
            default:
                break;
        }
    }

    public void ToStartPosition()
    {
        pacmanRigidbody.MovePosition(new Vector3(startX, 0, startZ));
        currentX = startXpath;
        currentZ = startZpath;
        currentDirectionX = startDirectionX;
        currentDirectionZ = startDirectionZ;
        nextDirectionX = startDirectionX;
        nextDirectionZ = startDirectionZ;
    }

    public IEnumerator Boost()
    {
        currentSpeed -= 0.2f;
        yield return new WaitForSeconds(7);
        currentSpeed += 0.2f;
    }

    public void ToStartSpeed()
    {
        currentSpeed = startSpeed;
    }

    public void SpeedUp()
    {
        //уменьшаем, чтобы ожидание перед перемещением было меньше
        currentSpeed -= speedDelta;
        if (currentSpeed < maxSpeed)
        {
            currentSpeed = maxSpeed;
        }
    }

}
