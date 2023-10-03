using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PoolManager : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject[] pool;

    Controls controls;

    private void Awake()
    {
        controls = new Controls();
        controls.Player.Click.performed += (ctx) =>
        {
            Vector2 mousePosition = controls.Player.MousePosition.ReadValue<Vector2>();
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            InstantiateNewGameObject(worldPosition);
        };
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    public void InstantiateNewGameObject(Vector2 position)
    {
        int index = FreeSpace();
        if (index >= 0)
        {
            GameObject go = Instantiate(prefab, position, Quaternion.identity);
            pool[index] = go;
        }
        else
        {
            pool[0].transform.position = position;
            GameObject go = pool[0];
            for(int i = 0; i < pool.Length; i++)
            {
                
                if(i == 0)
                {
                    GameObject provisional = pool[pool.Length - 1];
                    pool[pool.Length - 1] = go;
                    go = provisional;
                }
                else
                {
                    pool[i - 1] = go;
                    go = pool[i];
                }
            }
        }
    }

    public int FreeSpace()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (pool[i] == null)
            {
                return i;
            }
        }
        return -1;
    }
}
