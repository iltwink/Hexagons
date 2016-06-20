using UnityEngine;
using System.Collections;

public class Hex : MonoBehaviour {

    public Vector2 HexPosition;
    public HexModel HexModel { get; set; }

    public void InitializeModel()
    {
        var hex = new GameObject();
        hex.AddComponent<HexModel>();
        HexModel = hex.GetComponent<HexModel>();
        hex.transform.parent = transform;
        hex.transform.localPosition = new Vector3(0, 0, 0);
        hex.GetComponent<Renderer>().material.shader = Shader.Find("Diffuse");
        hex.GetComponent<Renderer>().material.mainTexture = Resources.Load("textures/hex") as Texture2D;
    }
}
