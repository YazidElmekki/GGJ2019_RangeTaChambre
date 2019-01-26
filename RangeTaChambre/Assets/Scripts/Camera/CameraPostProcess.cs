using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPostProcess : MonoBehaviour
{
	[SerializeField]
	private Material pencilEffectMaterial;

	[SerializeField]
	private float rotationAngleOffset = 5.0f;

	private float currentRotationAngle = 0.0f;

	[SerializeField]
	private bool updateAngle = true;

	[SerializeField]
	private float updateRate = 1.0f;

	private void Start()
	{
		StartCoroutine(UpdateAngleRoutine());
	}

	private IEnumerator UpdateAngleRoutine()
	{
		while(true)
		{
			yield return new WaitForSeconds(updateRate);
			OffsetAngle();
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (pencilEffectMaterial != null)
		{
			Graphics.Blit(source, destination, pencilEffectMaterial);
		}
	}

	private void OffsetAngle()
	{
		if (updateAngle == true)
		{
			currentRotationAngle += rotationAngleOffset;
			pencilEffectMaterial.SetFloat("_RotationAngle", currentRotationAngle);
		}
	}
}