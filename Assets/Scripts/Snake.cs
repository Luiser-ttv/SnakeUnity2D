using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class Snake : MonoBehaviour {
    //La direccion predeterminada
    Vector2 dir = Vector2.right;

    //Mantiene el seguimiento de la cola
    List<Transform> tail = new List<Transform>();
    
    //Para comprobar si ha comido
    bool ate = false;

    //Prefab de la cola
    public GameObject tailPrefab;

    private bool deadSnake = false;

    public TMP_Text LText;
    public TMP_Text RText;


    void Start () {
        //Movimiento cada 100ms
        InvokeRepeating("Move", 0.1f, 0.1f);    
    }
    
    
    void Update() {
        //Comprobar la direccion nueva de movimiento
        if (Input.GetKey(KeyCode.RightArrow))
            dir = Vector2.right;
        else if (Input.GetKey(KeyCode.DownArrow))
            dir = -Vector2.up;    
        else if (Input.GetKey(KeyCode.LeftArrow))
            dir = -Vector2.right; 
        else if (Input.GetKey(KeyCode.UpArrow))
            dir = Vector2.up;


        if (deadSnake)
        {
            if (GameObject.FindGameObjectWithTag("Tail") != null)
            {
                Destroy(GameObject.FindGameObjectWithTag("Tail"));
                tail.Clear();
            }
            if (GameObject.FindGameObjectWithTag("Food") != null)
            {
                Destroy(GameObject.FindGameObjectWithTag("Food"));
            }
        }

        if (Input.GetKey("space") && deadSnake)
        {
            deadSnake = false;
            GameObject.FindGameObjectWithTag("Player").transform.position = new Vector2(0, 0);
            GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().enabled = true;
            LText.text = "";
            RText.text = "";
        }
    }
    
    void Move() {
        //Guarda la position actual
        Vector2 v = transform.position;

        //Mueve la cabeza de snake
        transform.Translate(dir);

        //Si come añade un bloque más a la cola
        if (ate) {
            //Genera un nuevo bloque
            GameObject g =(GameObject)Instantiate(tailPrefab, v, Quaternion.identity);

            //Mantiene en seguimiento la cola
            tail.Insert(0, g.transform);

            //Resetea el booleano
            ate = false;
        }
        //Comprobamos si tiene cola
        else if (tail.Count > 0) {
            //Mueve la cola a la misma posición que la cabeza
            tail.Last().position = v;

            //Añade al principio y borra al final
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count-1);
        }
    }
    
    void OnTriggerEnter2D(Collider2D coll) {
        //Comprueba si entra en contacto con la comida
        if (coll.name.StartsWith("FoodPrefab")) {
            
            ate = true;
            
            Destroy(coll.gameObject);
        }
        
        else {
            deadSnake = true;
            LText.text = "You Lose";
            RText.text = "Press Space to restart";
            GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}