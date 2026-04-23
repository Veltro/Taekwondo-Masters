using UnityEngine;

public class ScrollMap : MonoBehaviour
{
    Material _material; // Material do mapa para aplicar a rolagem
    private Vector2 _offset; // Deslocamento da textura para criar o efeito de rolagem
    public float xVelocity, yVelocity; // Velocidade de rolagem do mapa

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
    }

    private void Start()
    {
        //_offset = new Vector2(xVelocity, yVelocity); 
    }

    // Update is called once per frame
    void Update()
    {
        _offset = new Vector2(xVelocity, yVelocity); 
        _material.mainTextureOffset += _offset * Time.deltaTime; // Atualiza o deslocamento da textura para criar o efeito de rolagem
    }
}
