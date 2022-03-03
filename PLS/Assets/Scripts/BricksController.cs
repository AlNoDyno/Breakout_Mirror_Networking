using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BricksController : MonoBehaviour
{
    [SerializeField]
    private GameObject brickCanvas;

    [SerializeField]
    private GameObject BrickPrefab;

    [SerializeField]
    private ScoreController scoreController;

    private void OnEnable()
    {
        CreateBricks(50);
    }

    private void CreateBricks(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject BrickGO = Instantiate(BrickPrefab);
            BrickGO.transform.parent = brickCanvas.transform;

            Brick brick = BrickGO.GetComponent<Brick>();

            Color col = new Color(1f, 1f, 1f);

            if (i < 10)
            {
                col = new Color(Random.Range(0.6f, 1f), 0f, 0f);
            }
            else if (i < 20)
            {
                col = new Color(1f, Random.Range(0.4f, 0.6f), 0f);
            }
            else if (i < 30)
            {
                col = new Color(1f, 1f, Random.Range(0f, 0.5f));
            }
            else if (i < 40)
            {
                col = new Color(0f, Random.Range(0.4f, 1f), 0f);
            }
            else if (i < 50)
            {
                col = new Color(0f, 0f, Random.Range(0.5f, 1f));
            }

            brick.img.color = col;
            brick.scoreController = scoreController;
        }
    }
}
