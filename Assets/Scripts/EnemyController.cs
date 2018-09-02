using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public PacmanController pacman;
    public Material normalRed;
    public Material scaredBlue;

    public bool Scared { get; set; }

    private readonly int startX = 1; //координаты unity
    private readonly int startZ = 4;
    private readonly int startXpath = 14; //координаты в массиве path
    private readonly int startZpath = 19;
    private readonly int startDirectionX = -1;
    private readonly int startDirectionZ = 0;
    private int currentX;
    private int currentZ;
    private int currentDirectionX;
    private int currentDirectionZ;
    private int nextDirectionX;
    private int nextDirectionZ;
    private readonly float startSpeed = 0.6f;
    private readonly float speedDelta = 0.03f;
    private readonly float maxSpeed = 0.2f;
    public float currentSpeed;
    private float time = 0;
    private float distance;

    private Rigidbody enemyRigidbody;

    private void Awake()
    {
        enemyRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void Start () {
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
                enemyRigidbody.MovePosition(new Vector3(enemyRigidbody.position.x + currentDirectionX, 0, enemyRigidbody.position.z + currentDirectionZ));
                currentX += currentDirectionX;
                currentZ += currentDirectionZ;
                time = 0;
                //телепортация через туннель в середине карты
                if (currentZ == 16)
                {
                    if (currentX == 0)
                    {
                        currentX = 26;
                        enemyRigidbody.MovePosition(new Vector3(13, 0, 1));
                    }
                    else if (currentX == 27)
                    {
                        currentX = 1;
                        enemyRigidbody.MovePosition(new Vector3(-12, 0, 1));
                    }
                }
            }
            distance = 1000;
            if (currentDirectionZ != -1 && GameController.Instance.path[currentZ + 1, currentX + 0] == 1)
            {
                distance = Vector3.Distance(new Vector3(pacman.CurrentZ, 0, pacman.CurrentX), new Vector3(currentZ + 1, 0, currentX + 0));
                nextDirectionX = 0;
                nextDirectionZ = 1;
            }
            if (currentDirectionX != 1 && GameController.Instance.path[currentZ + 0, currentX - 1] == 1)
            {
                if (Vector3.Distance(new Vector3(pacman.CurrentZ, 0, pacman.CurrentX), new Vector3(currentZ + 0, 0, currentX - 1)) < distance)
                {
                    distance = Vector3.Distance(new Vector3(pacman.CurrentZ, 0, pacman.CurrentX), new Vector3(currentZ + 0, 0, currentX - 1));
                    nextDirectionX = -1;
                    nextDirectionZ = 0;
                }
            }
            if (currentDirectionZ != 1 && GameController.Instance.path[currentZ - 1, currentX + 0] == 1)
            {
                if (Vector3.Distance(new Vector3(pacman.CurrentZ, 0, pacman.CurrentX), new Vector3(currentZ - 1, 0, currentX + 0)) < distance)
                {
                    distance = Vector3.Distance(new Vector3(pacman.CurrentZ, 0, pacman.CurrentX), new Vector3(currentZ - 1, 0, currentX + 0));
                    nextDirectionX = 0;
                    nextDirectionZ = -1;
                }
            }
            if (currentDirectionX != -1 && GameController.Instance.path[currentZ + 0, currentX + 1] == 1)
            {
                if (Vector3.Distance(new Vector3(pacman.CurrentZ, 0, pacman.CurrentX), new Vector3(currentZ + 0, 0, currentX + 1)) < distance)
                {
                    distance = Vector3.Distance(new Vector3(pacman.CurrentZ, 0, pacman.CurrentX), new Vector3(currentZ + 0, 0, currentX + 1));
                    nextDirectionX = 1;
                    nextDirectionZ = 0;
                }
            }

            //смена направления по возможности
            if (GameController.Instance.path[currentZ + nextDirectionZ, currentX + nextDirectionX] == 1)
            {
                currentDirectionX = nextDirectionX;
                currentDirectionZ = nextDirectionZ;
            }
        }
    }


    public void ToStartPosition()
    {
        enemyRigidbody.MovePosition(new Vector3(startX, 0, startZ));
        currentX = startXpath;
        currentZ = startZpath;
        currentDirectionX = startDirectionX;
        currentDirectionZ = startDirectionZ;
        nextDirectionX = startDirectionX;
        nextDirectionZ = startDirectionZ;
        Unscare();
    }

    public void Scare()
    {
        Scared = true;
        gameObject.GetComponent<Renderer>().material = scaredBlue;
    }

    public void Unscare()
    {
        Scared = false;
        gameObject.GetComponent<Renderer>().material = normalRed;

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
