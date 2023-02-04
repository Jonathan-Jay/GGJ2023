using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BnuuyGenerator : MonoBehaviour
{
	public MainBnuuy bnuuy;
	public FollowerBnuuy prefab;
	public int startCount;
	public Vector2 rande = new Vector2(-0.5f, 0.5f);

	private void Start() {
		Generate(startCount);
	}

	public void Generate(int count) {
		for (int i = 0; i < count; ++i) {
			Instantiate(prefab, transform.position +
				new Vector3(Random.Range(rande.x, rande.y), Random.Range(rande.x, rande.y), 0),
				Quaternion.identity).bnuuy = bnuuy;
		}
	}
}
