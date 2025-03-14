using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Cube ChangeColor(Cube cube)
    {
        cube.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);

        return cube;
    }
}