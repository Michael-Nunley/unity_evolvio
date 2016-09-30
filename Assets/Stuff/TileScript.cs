using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour 
{
	public bool IsWater = true;
	public float WaterHeight = 0.2F;
	public float SnowHeight = 0.0F;

	public float BiomeSand = 0.0F;
	public float Yummyness = 0.0F;
	public float Fertileness = 0.0F;
	public float maximum = 0.0F;

	// Use this for initialization
	void Start () 
	{
		if(IsWater)
		{
			Renderer rend = GetComponent<Renderer>();
			rend.material.shader = Shader.Find("Unlit/Color");
			rend.material.SetColor("_Color", Color.blue);
		}
		else
		{
			if(BiomeSand>=0.6)
			{
				if (Yummyness >= SnowHeight)
				{
					Renderer rend = GetComponent<Renderer>();
					rend.material.shader = Shader.Find("Unlit/Color");
					rend.material.SetColor("_Color", new Color(Fertileness, 1.0F, 0.0F));
				}
				else if (Yummyness <= WaterHeight)
				{
					IsWater = true;
					Renderer rend = GetComponent<Renderer>();
					rend.material.shader = Shader.Find("Unlit/Color");
					rend.material.SetColor("_Color", new Color(0.0F, 0.0F, Fertileness + 0.7F));
				}
				else
				{
					Renderer rend = GetComponent<Renderer>();
					rend.material.shader = Shader.Find("Unlit/Color");
					rend.material.SetColor("_Color", new Color(Fertileness, Fertileness, 0.0F));
				}
			}
			else
			{
				if (Yummyness >= SnowHeight)
				{
					Renderer rend = GetComponent<Renderer>();
					rend.material.shader = Shader.Find("Unlit/Color");
					rend.material.SetColor("_Color", new Color(Fertileness, 1.0F, Fertileness));
				}
				else if (Yummyness <= WaterHeight)
				{
					IsWater = true;
					Renderer rend = GetComponent<Renderer>();
					rend.material.shader = Shader.Find("Unlit/Color");
					rend.material.SetColor("_Color", new Color(0.0F, 0.0F, Fertileness + 0.7F));
				}
				else
				{
					Renderer rend = GetComponent<Renderer>();
					rend.material.shader = Shader.Find("Unlit/Color");
					rend.material.SetColor("_Color", new Color(Fertileness / 2, Yummyness, 0.0F));
				}
			}
		}
	}
}
