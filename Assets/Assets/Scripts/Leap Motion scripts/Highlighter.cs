using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Highlighter : MonoBehaviour {

	protected bool isHighlighting;
	protected Material[] originalMaterials;
	protected Material[] highlightMaterials;
	protected float originalAlpha;

	public Material highlightMaterial;

	[Range(0.5f, 10f)]
	public float highlightDuration = 5f;

	[Range(0.1f, 10f)]
	public float highlightFrequency = 1f;

	[Range(0f, 1f)]
	public float alphaOffset = 0.2f;

	public bool disableAfterDuration = true;

	public bool highlight = false;

	protected void Start() {
		if(highlightMaterial == null) {
			throw new System.ArgumentNullException("Highlight material not defined.");
		}

		// Get the mesh renderer
		MeshRenderer renderer = GetComponent<MeshRenderer>();

		// Save the object original materials
		originalMaterials = renderer.sharedMaterials;

		// Create a duplicated instance of the material
		highlightMaterial = new Material(highlightMaterial);

		// Save the initial alpha value of the material
		originalAlpha = highlightMaterial.color.a;

		// Create a list of highlight materials to replace the object materials
		highlightMaterials = new Material[originalMaterials.Length];
		for(int i = 0; i < highlightMaterials.Length; ++i) {
			highlightMaterials[i] = highlightMaterial;
		}

		isHighlighting = false;
	}

	protected void Update() {
		if(highlight && ! isHighlighting) {
			StartCoroutine(HighlightCoroutine());
		}
	}

	protected IEnumerator HighlightCoroutine() {
		// Set flag to show highlight is in progress
		isHighlighting = true;
		
		// Record starting time
		float start_time = Time.time;

		// Get the mesh renderer
		MeshRenderer renderer = GetComponent<MeshRenderer>();

		// Set the materials to highlight mode
		renderer.sharedMaterials = highlightMaterials;

		do {
			// Convert spent time to oscillating value
			float current_timing = (Time.time - start_time) * highlightFrequency;
			float current_timing_to_rad = 2f * Mathf.PI * (current_timing - (int) current_timing);

			// Get the highlight color
			Color color = highlightMaterial.color;

			// Set the highlight alpha as a oscillating value
			color.a = Mathf.Clamp01(originalAlpha + Mathf.Sin(current_timing_to_rad) * alphaOffset);

			// Save the modified color
			highlightMaterial.color = color;

			// Wait until next frame
			yield return new WaitForEndOfFrame();
		}
		while(highlight && (! disableAfterDuration || Time.time - start_time < highlightDuration)); // Until duration is passed

		// Reset the materials to their initial value
		renderer.sharedMaterials = originalMaterials;

		// Disable highlight
		isHighlighting = false;
		highlight = false;
	}
}
