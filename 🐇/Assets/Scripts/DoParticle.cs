using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoParticle : MonoBehaviour
{
	public ParticleSystem particles;
	public AudioQueue sounds;
	public void EmitParticles() {
		particles.Play();
		sounds.Play();
	}
}
