using UnityEngine;
using System.Collections.Generic;

public class BlobBrain : MonoBehaviour
{
	NeuralNet.NeuralNetwork network;
	
#if UNITY_EDITOR
	private void OnGUI()
	{
		GUI.Label(new Rect(0, 50, 200, 50), new GUIContent("Time taken to render previous frame: " + Time.deltaTime));
		GUI.Label(new Rect(0, 100, 200, 50), new GUIContent("Frames per second: " + 1 / Time.deltaTime));
	}
#endif

	public bool dumpNeuron = false;

	[TextArea(3, 30)]
	public string netDump;

	public float ThinkTimer = 0.2F;

	public List<double> Brain_Memories = new List<double>();
	public GameObject Director, Base, Body, Head, Looking;

	double Energy = 100.0F, Temperature = 90.0F, Age = 0.0F;

	//brain shit
	float			DesireToMakeBaby =	0.0F;
	public float	 _R =				0.5F,	_G =			0.5F,	_B =		0.5F;
	float			CurrentVel =		0.0F,	CurrentRotVel = 0.0F,
					Look_R =			0.5F,	Look_G =		0.5F,	Look_B =	0.5F, 
					Look_Dist =			1.0F,	Look_Angle =	0.0F;

	void Start()
	{
		Brain_Memories.Add(1.0D); //constant
		Brain_Memories.Add(0.0D); //Mem1
		Brain_Memories.Add(0.0D); //Mem2
		Brain_Memories.Add(0.0D); //Mem3
		Brain_Memories.Add(0.0D); //Mem4
		Brain_Memories.Add(0.0D); //Mem5

		List<int> layerList = new List<int>();
		var _with1 = layerList;
		_with1.Add(19);
		_with1.Add(17);
		_with1.Add(14);
		network = new NeuralNet.NeuralNetwork(21.5, layerList);
		InvokeRepeating("Think", 0.2F, ThinkTimer);
	}

	void Think()
	{
		List<double> inputs = new List<double>();
		inputs.Add(Brain_Memories[0]);
		inputs.Add(Brain_Memories[1]);
		inputs.Add(Brain_Memories[2]);
		inputs.Add(Brain_Memories[3]);
		inputs.Add(Brain_Memories[4]);
		inputs.Add(Brain_Memories[5]);
		inputs.Add(_R); inputs.Add(_G); inputs.Add(_B);
		inputs.Add(Look_R); inputs.Add(Look_G); inputs.Add(Look_B);
		inputs.Add(Look_Dist); inputs.Add(Look_Angle);
		inputs.Add(Temperature);
		inputs.Add(CurrentVel); inputs.Add(CurrentRotVel);
		inputs.Add(Age); inputs.Add(Energy);

		List<double> ots = new List<double>();
		ots = network.Execute(inputs);
		
		Brain_Memories[1] = ots[1];
		Brain_Memories[2] = ots[2];
		Brain_Memories[3] = ots[3];
		Brain_Memories[4] = ots[4];
		Brain_Memories[5] = ots[5];
		UpdateColor((float)ots[6], (float)ots[7], (float)ots[8]);
		Look_Dist = (float)ots[9]; Look_Angle = (float)ots[10];
		MoveForward((float)ots[11]); RotateRight((float)ots[12]);
		Attack(ots[13]);
	}
	bool _in = true;
	void Update()
	{
		if (dumpNeuron)
		{
			if (_in)
			{
				netDump = network.ToString();
				_in = false;
			}
		}
		else
		{
			_in = true;
		}
		/*
		if (Input.GetKeyDown(KeyCode.A))
		{
			RotateRight(-45.00000F);
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			RotateRight(+45.00000F);
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			MoveForward(-0.20000F);
		}
		else if (Input.GetKeyDown(KeyCode.W))
		{
			MoveForward(+0.20000F);
		}
		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		else if (Input.GetKeyDown(KeyCode.F))
		{
			_R = Random.Range(0.00F, 1.00F);
			_G = Random.Range(0.00F, 1.00F);
			_B = Random.Range(0.00F, 1.00F);
			UpdateColor(_R, _G, _B);
		}
		*/
	}

	void Attack(double need)
	{ 
		if (need > 1)
		{
			//beat the shit out of who ever is in front of me
		}
		else{ } //Nah
	}

	void Eat(double need)
	{
		if (need > 1)
		{
			//nom the shit out of the ground
		}
		else { } //Nah
	}

	void Breed(double need)
	{
		if (need > 1.0D)
		{
			//Asexual Sexy
		}
		else if (need < -0.5)
		{ } //Nah
		else 
		{ 
			//Group sexy
		}
	}

	void MoveForward(float speed)
	{
		Base.transform.localPosition += Base.transform.forward * speed;
	}

	void RotateRight(float speed)
	{
		Base.transform.Rotate(0.0000F, speed, 0.0000F);
	}

	void UpdateColor(float r, float g, float b)
	{
		Renderer rend = Head.GetComponent<Renderer>();
		rend.material.shader = Shader.Find("Unlit/Color");
		rend.material.SetColor("_Color", new Color(r,g,b));
	}
}