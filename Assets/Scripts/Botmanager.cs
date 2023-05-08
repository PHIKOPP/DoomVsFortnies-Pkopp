using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Botmanager : MonoBehaviour
{

    public static List<Bot> bots = new List<Bot>();
    public GameObject youWinCanvas;
    public float botsCount;
    public TextMeshProUGUI botsCountText;
    public GameObject botObject, botSpawnPoint;

    // Start is called before the first frame update

    private void Awake()
    {
        bots.Clear();
    }

    void Start()
    {
        youWinCanvas.SetActive(false);

        for (int i = 0; i < Random.Range(10, 20); i++)
            Instantiate(botObject, botSpawnPoint.transform.position, botSpawnPoint.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        botsCount = bots.Count;
        botsCountText.text = "Remaining enemies: " + botsCount;
        if (botsCount <= 0)
        {
            youWinCanvas.SetActive(true);
            Time.timeScale = 0.00001f;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    

}
