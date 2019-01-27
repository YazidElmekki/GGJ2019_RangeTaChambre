using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMixPostProcess : MonoBehaviour
{
	[SerializeField]
	private Material mixMaterial;

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (mixMaterial != null)
		{
			Graphics.Blit(source, destination, mixMaterial);
		}
	}
}
