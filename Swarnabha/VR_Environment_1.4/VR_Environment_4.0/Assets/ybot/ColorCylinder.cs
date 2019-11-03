using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCylinder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        var cylinderRenderer = cylinder.GetComponent<Renderer>();

        //Call SetColor using the shader property name "_Color" and setting the color to red
        cylinderRenderer.material.SetColor("_Color", Color.red);
    }
}