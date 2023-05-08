using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject projectilePrefab, hand, objInhand, bombPrefab, camera, cannon, grenadeLuncher;


    
    public int throwForce;
    public LayerMask interactionMask;
    public float maxDist = 20;

    public float attrForce;

    public float grabDist;

    public float slowScale;

    public float originalFixedDeltaTime;

    public float health = 100;
    public TextMeshProUGUI healtText;

    public GameObject gunOutput;

    public static PlayerController player;

    public AudioSource shootingSfx;

    public GameObject youLooseCanvas;

    private void Awake()
    {
        originalFixedDeltaTime = Time.fixedDeltaTime;
        player = this;
        StartCoroutine(regenerate());
        youLooseCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    IEnumerator regenerate()
    {
        while (true)
        {
            if (health < 100)
                health++;
            yield return new WaitForSeconds(2);
        }
    }

    void loose()
    {
        Time.timeScale = 0.00001f;
        youLooseCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        healtText.text = "Health: " + health + "/" + 100;

        if (health <= 0)
            loose();

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = slowScale;
                Time.fixedDeltaTime = originalFixedDeltaTime * slowScale;
            }
            else
            {
                Time.timeScale = 1;
                Time.fixedDeltaTime = originalFixedDeltaTime;
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray ray = new Ray(camera.transform.position, camera.transform.forward);
            Debug.DrawLine(ray.origin, ray.GetPoint(maxDist));
            RaycastHit obj;
            if(Physics.Raycast(ray, out obj, maxDist, interactionMask))
            {
                if (Vector3.Distance(hand.transform.position, obj.transform.position) < grabDist)
                {
                    objInhand = obj.transform.gameObject;
                    obj.transform.position = hand.transform.position;
                    obj.transform.parent = hand.transform;
                    obj.transform.GetComponent<Rigidbody>().isKinematic = true;
                }
                else
                {
                    Vector3 dir = (hand.transform.position - obj.transform.position).normalized;
                    obj.rigidbody.AddForce(dir * attrForce, ForceMode.Impulse);
                }

            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(objInhand != null)
            {
                objInhand.transform.parent = null;
                objInhand.GetComponent<Rigidbody>().isKinematic = false;
                objInhand.GetComponent<Rigidbody>().AddForce(camera.transform.forward * throwForce, ForceMode.Impulse);
                objInhand = null;
            }
            else
            {
                GameObject bomb = Instantiate(projectilePrefab, cannon.transform.position, cannon.transform.rotation);
                Destroy(bomb, 1f);
                Rigidbody bombRB = bomb.GetComponent<Rigidbody>();
                shootingSfx.Play();
                bombRB.AddForce(camera.transform.forward * throwForce, ForceMode.Impulse);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(objInhand != null)
            {
                objInhand.transform.parent = null;
                objInhand.GetComponent<Rigidbody>().isKinematic = false;
                objInhand.GetComponent<Rigidbody>().AddForce(camera.transform.forward * throwForce, ForceMode.Impulse);
                objInhand = null;
            }
            else
            {
                GameObject bomb = Instantiate(bombPrefab, grenadeLuncher.transform.position, new Quaternion(0, 0, 0, 0));
                Rigidbody bombRB = bomb.GetComponent<Rigidbody>();
                bombRB.AddForce(camera.transform.forward * throwForce, ForceMode.Impulse);
            }
        }
        
    }

    public void playAgain()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

}
