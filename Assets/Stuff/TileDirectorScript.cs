using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

public class TileDirectorScript:MonoBehaviour 
{
	public struct pointI { public int x, y; }
	public struct pointF { public float x, y; }

	public struct Weather
	{
		public float sunShine;
		public float rainChance; //part of Biome 10
		public pointI[][] stormCover;
		/*
		 * [0][-] first noise map
		 * [1][-] second noise map
		 * 
		 * 
		*/
	}

	public struct S_Tile
	{
		public bool isWater;
		public float[] Biomes;
		/* =================================== 
		 * 00 = Land	[Green]					(Basic shape of scape)
		 * 01 = Sand	[Yellow]				
		 * 02 = Tiaga	[Desaturated Green]		
		 * 03 = Snow
		 * 04 = 
		 * 05 = 
		 * 06 = 
		 * 07 = 
		 * 08 = 
		 * 09 = 
		 * 10 = 
		 * ===================================
		*/
		public float foodvalue;
		public pointI size;
		public pointI location;
		public float fertileness;
		public float temperature;
		public Color color;
		public float sunShine;
	}	

	public GameObject TileActor;
	public float WaterHeight = 0.3F;
	public float SnowHeight = 0.8F;

	public bool RandomSeed = false;
	public long Seed = -3894L;
	public float max = 0.0F; public float min = 0.0F;
	public int pixWidth = 256;
    public int pixHeight = 256;
    public float xOrg = 0.0F;
    public float yOrg = 0.0F;
    public float scale = 6.0F;

	public Texture2D texture;
	public Sprite spurt;

	S_Tile[] Tiles;

	void Start() 
	{
		texture = new Texture2D( pixWidth, pixHeight,TextureFormat.RGB24,false );
		spurt = Sprite.Create( texture, new Rect( 0, 0, pixWidth, pixHeight ), new Vector2( 0.5F, 0.5F ), scale );
		GetComponent<SpriteRenderer>().sprite = spurt;

		if (RandomSeed)
		{
			Seed = (long)UnityEngine.Random.Range(-9223372036854775808L, 9223372036854775807L);
			//Seed = DateTime.Now.Ticks;
		}

		float xBCoord = 0.0F; float yBCoord = 0.0F;
		float xCoord = 0.0F; float yCoord = 0.0F;

		OpenSNoise.OpenSimplexNoise BiomeMaker = new OpenSNoise.OpenSimplexNoise(Seed+1L); float BiomeSandRange = 0.0F;
		OpenSNoise.OpenSimplexNoise noisy = new OpenSNoise.OpenSimplexNoise(Seed); float sample = 0.0F;

		float oldsample = 0.0F;
		int count = 0;
		int x = 0, y =0;
		for (y = 0; y < pixWidth; y++)
		{
			
			for (x = 0; x < pixHeight; x++)
			{
				xBCoord = x / pixWidth * scale;
				yBCoord = y / pixHeight * scale;

				BiomeSandRange = (float)BiomeMaker.Evaluate( xBCoord, yBCoord );
				/*
				xCoord = xOrg + x / pixWidth * scale/10;
				yCoord = yOrg + y / pixHeight * scale/10;
				*/
				xCoord = xOrg + x / pixWidth;
				yCoord = yOrg + y / pixHeight;

				sample = (float)noisy.Evaluate( xCoord, yCoord );

				oldsample = Mathf.PerlinNoise( xCoord, yCoord );
				count = x + y;

				//Tiles[count].fertileness = sample;

				/*
				Tiles[count].location.x = x;
				Tiles[count].location.y = y;
				Tiles[count].fertileness = sample;
				Tiles[count].foodvalue = sample;
				Tiles[count].Biomes[0] = sample;
				Tiles[count].Biomes[1] = BiomeSandRange;
				*/

				//UnityEngine.Random.Range(0.0F,1.0F)
				texture.SetPixel( x, y, new Color( 0.0F, oldsample, System.Math.Abs(oldsample % 2) -1) );
				//texture.SetPixel(x, y, new Color(0.0F, 0.4F, 0.0F));

				if (oldsample > max) { max = oldsample; }
				if (oldsample < min) { max = oldsample; }

				/*
				if (BiomeSandRange >= 0.6)
				{
					if (sample >= SnowHeight)
					{
						texture.SetPixel(x, y, new Color(Tiles[count].fertileness, 1.0F, 0.0F));
					}
					else if (sample <= WaterHeight)
					{
						Tiles[count].isWater = true;
						texture.SetPixel(x, y, new Color(0.0F, 0.0F, Tiles[count].fertileness + 0.7F));
					}
					else
					{
						texture.SetPixel(x, y, new Color(Tiles[count].fertileness, Tiles[count].fertileness, 0.0F));
					}
				}
				else
				{
					if (sample >= SnowHeight)
					{
						texture.SetPixel(x, y, new Color(Tiles[count].fertileness, 1.0F, Tiles[count].fertileness));
					}
					else if (sample <= WaterHeight)
					{
						Tiles[count].isWater = true;
						texture.SetPixel(x, y, new Color(0.0F, 0.0F, Tiles[count].fertileness + 0.7F));
					}
					else
					{
						texture.SetPixel(x, y, new Color(Tiles[count].fertileness / 2, Tiles[x + y].foodvalue, 0.0F));
					}
				}
				*/

				//Tiles[count]



				//texture.SetPixel(x, y, new Color(1.0f, 1.0f, 1.0f));




				/* Make real tiles
				GameObject Placed = (GameObject)Instantiate(TileActor, new Vector3((x-(pixHeight/2)), 0, (y - (pixWidth/2))), Quaternion.Euler(90, 0, 0));

				TileScript script = Placed.GetComponent<TileScript>();
				script.Yummyness = sample;
				script.Fertileness = sample;
				script.WaterHeight = WaterHeight;
				script.SnowHeight = SnowHeight;
				script.BiomeSand = BiomeSandRange;
				if (sample > max)
				{
					max = sample;
				}
				script.maximum = max;


				*/
			}
		}
		// Apply all SetPixel calls
		texture.Apply();

		// connect texture to material of GameObject this script is attached to
		/*
		spurt = Sprite.Create( texture, new Rect( 0.0F, 0.0F, pixWidth, pixHeight ), new Vector2( 0.5F, 0.5F ), 1.0F );
		
		GetComponent<SpriteRenderer>().sprite = spurt;
		*/
		//GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
	}
}
