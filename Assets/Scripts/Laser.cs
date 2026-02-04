using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : InteractableObject
{
    [SerializeField]
    LineRenderer line;

    [SerializeField]
    LineRenderer reflectLine;

    [SerializeField]
    LayerMask mask;

    [SerializeField]
    LayerMask reflectMask;

    [SerializeField]
    GameObject particles;

    [SerializeField]
    int points = 5;

    [SerializeField]
    float amplitude = 1;

    [SerializeField]
    float waveSpeed = 1;

    [SerializeField]
    AudioSource laserAudio;

    bool hittingSaw = false;
    float sawElapsed = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, line.transform.position);

        RaycastHit2D hit = Physics2D.Raycast(line.transform.position, line.transform.up * -1, 100, mask);

        var endPoint = line.transform.position + line.transform.up * -100;

        if (hit)
        {
            endPoint = hit.point;
            var player = hit.transform.gameObject.GetComponent<PlayerLogic>();
            if (player != null)
            {
                Debug.Log("HIT PLAYER");
                player.DeathByLaser();
            }

            var saw = hit.transform.gameObject.GetComponent<SawBlade>();
            if(saw != null)
            {
                Debug.Log("Hit Saw");
                // spawn laser reflection
                hittingSaw = true;
                
            }

            else
            {
                hittingSaw = false;
                
            }

            laserAudio.transform.position = endPoint;
        }

        else
        {
            laserAudio.transform.position = transform.position + Vector3.down * 2;
        }

        //line.SetPosition(1, endPoint);

        Draw(endPoint);
        particles.transform.position = endPoint;

        if(hittingSaw)
        {
            sawElapsed += Time.deltaTime;
            if(sawElapsed > 0.1f)
            {
                var randomDir = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

                RaycastHit2D reflectHit = Physics2D.Raycast(endPoint, randomDir, 100, reflectMask);

                //var endPoint = line.transform.position + line.transform.up * -100;

                Vector3 reflectEnd = endPoint + (randomDir * 100);

                if (reflectHit)
                {
                    Debug.Log("reflection hit " + reflectHit.transform.name);
                    var player = reflectHit.transform.gameObject.GetComponent<PlayerLogic>();
                    if (player != null)
                    {
                        Debug.Log("HIT PLAYER");
                        player.DeathByLaser();
                    }

                    reflectEnd = reflectHit.point;
                }

                reflectLine.SetPosition(0, endPoint);
                reflectLine.SetPosition(1, reflectEnd);
                sawElapsed = 0;
            }
        }

        else
        {
            reflectLine.SetPosition(0, endPoint);
            reflectLine.SetPosition(1, endPoint);
        }
        
    }

    void Draw(Vector3 endPoint)
    {
        float start = transform.position.y;
        float finish = endPoint.y;

        //line.SetPosition(0, transform.position);

        line.positionCount = points + 1;
        for (int currentPoint = 1; currentPoint < points; currentPoint++)
        {
            float progress = (float)currentPoint / (points - 1);
            float y = Mathf.Lerp(start, finish, progress);
            float x = amplitude * Mathf.Sin(y + (Time.timeSinceLevelLoad * waveSpeed));
            line.SetPosition(currentPoint, new Vector3(transform.position.x + x, y, 0));
        }

        //line.SetPosition(0, transform.position);
        line.SetPosition(line.positionCount - 1, endPoint);
    }
}
