using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoParticle : MonoBehaviour
{
	public ParticleSystem particles;
	public void EmitParticles() {
		particles.Play();
	}
}
