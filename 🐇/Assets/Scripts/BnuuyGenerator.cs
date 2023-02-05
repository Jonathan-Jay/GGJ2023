using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BnuuyGenerator : MonoBehaviour
{
	public MainBnuuy bnuuy;
	public FollowerBnuuy prefab;
	public CameraController cam;
	public int startCount;
	public Vector2 rande = new Vector2(-0.5f, 0.5f);

	private void Start() {
		cam = Camera.main.GetComponent<CameraController>();

		Generate(startCount);
	}

	public void Generate(int count) {
		FollowerBnuuy bun;
		for (int i = 0; i < count; ++i) {
			bun = Instantiate(prefab, transform.position +
				new Vector3(Random.Range(rande.x, rande.y), Random.Range(rande.x, rande.y), 0),
				Quaternion.identity);
			bun.bnuuy = bnuuy;
			cam.following.Add(bun.transform);
		}
	}
}
