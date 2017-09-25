using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Catlike Coding Unity C# Tutorials
//Contructing a Fractal
//Instantiate game object
//Use coroutines
//Add randomness

public class Fractal : MonoBehaviour {

	public Mesh mesh; // a Mesh is a construct used by the graphics hardware to draw complex stuff.
	public Material material; //Materials are used to define the visual properties of objects.
	public int maxDepth;
	private int depth;
	public float childScale;
	private Material[,] materials;
	public float maxRotationSpeed;
	private float rotationSpeed;
	public float maxTwist;

	private void InitializeMaterials () {
		materials = new Material[maxDepth + 1, 2];
		for (int i = 0; i <= maxDepth; i++) {
			float t = i / (maxDepth - 1f);
			t *= t;
			materials [i, 0] = new Material(material);
			materials [i, 0].color = Color.Lerp (Color.red, Color.cyan, t);
			materials[i, 1] = new Material(material);
			materials[i, 1].color = Color.Lerp(Color.black, Color.white, t);
		}
		materials [maxDepth, 1].color = Color.cyan;
		materials [maxDepth, 0].color = Color.red;
	}

	public Mesh [] meshes;

	private void Start () {
		rotationSpeed = Random.Range (-maxRotationSpeed, maxRotationSpeed);
		transform.Rotate (Random.Range (-maxTwist, maxTwist), 0f, 0f);
		if (materials == null) {
			InitializeMaterials ();
		}
		gameObject.AddComponent<MeshFilter> ().mesh = meshes [Random.Range (0, meshes.Length)]; //The AddComponent method creates a new component of a certain type, attached it to the game object, and returns a reference to it.
		gameObject.AddComponent<MeshRenderer> ().material = materials[depth, Random.Range(0, 2)];
		if (depth < maxDepth) {
			StartCoroutine (CreateChildren ());
			/*
			new GameObject ("Fractal Child").
				AddComponent<Fractal>().
			Initialize(this, Vector3.up);
			new GameObject ("Fractal Child").AddComponent<Fractal> ().
			Initialize (this, Vector3.right); */
		}
	}
	public float spawnProbability;

	private IEnumerator CreateChildren () {
		for (int i = 0; i < childDirections.Length; i++) {
			if (Random.value < spawnProbability) {
				yield return new WaitForSeconds (Random.Range (0.1f, 0.5f));
				new GameObject ("Fractal Child").AddComponent<Fractal> ().Initialize (this, i);
			}
		}
		/*
		yield return new WaitForSeconds (0.5f);
		new GameObject ("Fractal Child").
		AddComponent<Fractal> ().Initialize (this, Vector3.up, Quaternion.identity);
		yield return new WaitForSeconds (0.5f);
		new GameObject ("Fractal Child").
		AddComponent<Fractal> ().Initialize (this, Vector3.right, Quaternion.Euler(0f, 0f, -90f));
		yield return new WaitForSeconds (0.5f);
		new GameObject ("Fractal Child").
		AddComponent<Fractal> ().Initialize (this, Vector3.left, Quaternion.Euler(0f, 0f, -90f));
		*/
	}

	private static Vector3[] childDirections = {
		Vector3.up,
		Vector3.right,
		Vector3.left,
		Vector3.forward,
		Vector3.back,
		Vector3.down
	};

	private static Quaternion[] childOrientation = {
		Quaternion.identity,
		Quaternion.Euler (0f, 0f, -90f),
		Quaternion.Euler (0f, 0f, 90f),
		Quaternion.Euler (90f, 0f, 0f),
		Quaternion.Euler (-90f, 0f, 0f),
		Quaternion.Euler (0f, -90f, 0f)
	};

	private void Initialize (Fractal parent, int childIndex) {
		meshes = parent.meshes;
		materials = parent.materials;
		maxDepth = parent.maxDepth;
		depth = parent.depth + 1;
		childScale = parent.childScale;
		transform.parent = parent.transform;
		transform.localScale = Vector3.one * childScale;
		transform.localPosition = childDirections [childIndex] * (0.5f + 0.5f * childScale);
		transform.localRotation = childOrientation [childIndex];
		spawnProbability = parent.spawnProbability;
		maxRotationSpeed = parent.maxRotationSpeed;
		maxTwist = parent.maxTwist;

	}
	
	// Update is called once per frame
	public void Update () {
		transform.Rotate (0f, rotationSpeed * Time.deltaTime, 0f);
	}
}
