using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using static UnityEngine.Rendering.DebugUI;

public class FieldOfView : MonoBehaviour {

	public float viewRadius;
	[Range(0,360)]
	public float viewAngle;

	public LayerMask targetMask;
	public LayerMask obstacleMask;

	public List<Transform> visibleTargets = new List<Transform>();

	public float meshResolution;
	public int edgeResolveIterations;
	public float edgeDstThreshold;
	public float targetVisibleDelay = 0.2f;

    public MeshFilter viewMeshFilter;
	Mesh viewMesh;
	public CutsceneController controller;
	public AudioSource audioSource;
	public AudioClip clip;


    public Renderer quadRenderer; // Reference to the renderer of the screen where the shader will be applied
    public Material detectionMaterial; // Material containing the shader effect
    public float maxBrightnessDistance = 10.0f; // Maximum distance for maximum brightness
    public float detectionThreshold = 0.8f; // Threshold for displaying "I found you"
    public GameObject detectionText; // Reference to the UI text element
    private bool cutsceneTriggered = false; // Flag to track if cutscene has been triggered

    public float cutsceneTriggerDistance = 2.0f; // Distance threshold to trigger the cutscene

    public float _BrightnessDetectedValue, _BrightnessUndetectedValue = 0;


    float brightnessValue = 0.0f;

    void Start() {
        viewMesh = new Mesh
        {
            name = "View Mesh"
        };
        viewMeshFilter.mesh = viewMesh;

        StartCoroutine("FindTargetsWithDelay", targetVisibleDelay);
        
    }


	IEnumerator FindTargetsWithDelay(float delay) {
		while (true) {
			yield return new WaitForSeconds (delay);
			FindVisibleTargets ();
		}
	}


    void Update()
    {
        UpdateBrightnessBasedOnDistance();
    }
    void LateUpdate() {
        DrawFieldOfView();
	}

    void UpdateBrightnessBasedOnDistance()
    {

        if (visibleTargets.Count > 0)
        {
            float maxBrightnessDistance = 10.0f; // Change this value to suit your needs

            // Consider the first target in the list
            Transform nearestTarget = visibleTargets[0];
            float distance = Vector3.Distance(transform.position, nearestTarget.position);

            // Calculate brightness linearly proportional to the distance of the first target
            float brightnessFactor = Mathf.Clamp01(1 - distance / maxBrightnessDistance);
            brightnessValue = Mathf.Lerp(0.0f, 1.0f, brightnessFactor);
        }

        AdjustShaderBrightness(brightnessValue);

        DisplayDetectionText(brightnessValue >= detectionThreshold);
    }



    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        float minDistance = Mathf.Infinity;
        Transform nearestTarget = null;

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    if (dstToTarget < minDistance)
                    {
                        minDistance = dstToTarget;
                        nearestTarget = target;
                    }
                }
            }
        }

        if (nearestTarget != null)
        {
            visibleTargets.Add(nearestTarget);

            // Perform actions or calculations related to the nearest target...
            // For example:
            float delay = Mathf.Clamp(minDistance * 0.1f, 0.0f, 2.0f);

            if (minDistance <= cutsceneTriggerDistance && !cutsceneTriggered)
            {

                StartCoroutine(StartCutsceneWithDelay(delay));
            }
            else if (minDistance > cutsceneTriggerDistance && cutsceneTriggered)
            {
                cutsceneTriggered = false;
            }
        }
        else
        {
            AdjustShaderBrightness(0.0f);
            DisplayDetectionText(false);
        }
    }

    void AdjustShaderBrightness(float brightnessValue)
    {
        // Ensure detectionMaterial and screenRenderer are assigned in the Inspector
        if (quadRenderer!=null)
        {
            // Map the brightnessValue between _BrightnessUndetectedValue and _BrightnessDetectedValue
            float mappedBrightness = Mathf.Lerp(_BrightnessUndetectedValue, _BrightnessDetectedValue, brightnessValue);

            // Clamp the mapped brightness between 0 and 1
            //float clampedBrightness = Mathf.Clamp01(mappedBrightness);

            // Set the shader property for brightness
            quadRenderer.material.SetFloat("_Brightness", mappedBrightness *10);
        }
    }

    void DisplayDetectionText(bool show)
    {
        // Display or hide the detection text based on the boolean parameter
        if (detectionText != null)
        {
            detectionText.SetActive(show) ;
            if (show)
            {
				Debug.Log("I Found You");
					//detectionText.text = "I found you";
            }
        }
    }
    IEnumerator StartCutsceneWithDelay(float delay)
    {
		yield return new WaitForSecondsRealtime(delay);

        // Start the cutscene after the calculated delay
        if (!cutsceneTriggered)
        {
            Debug.Log("Cutscene trigger");
            cutsceneTriggered = true;
            controller.gameObject.SetActive(true);
            // Play the cutscene
            controller.GetComponent<CutsceneController>().StartCutscene();
        }
    }

 






    void DrawFieldOfView() {
		int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
		float stepAngleSize = viewAngle / stepCount;
		List<Vector3> viewPoints = new List<Vector3> ();
		ViewCastInfo oldViewCast = new ViewCastInfo ();
		for (int i = 0; i <= stepCount; i++) {
			float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
			ViewCastInfo newViewCast = ViewCast (angle);

			if (i > 0) {
				bool edgeDstThresholdExceeded = Mathf.Abs (oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
				if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded)) {
					EdgeInfo edge = FindEdge (oldViewCast, newViewCast);
					if (edge.pointA != Vector3.zero) {
						viewPoints.Add (edge.pointA);
					}
					if (edge.pointB != Vector3.zero) {
						viewPoints.Add (edge.pointB);
					}
				}

			}


			viewPoints.Add (newViewCast.point);
			oldViewCast = newViewCast;
		}

		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount-2) * 3];

		vertices [0] = Vector3.zero;
		for (int i = 0; i < vertexCount - 1; i++) {
			vertices [i + 1] = transform.InverseTransformPoint(viewPoints [i]);

			if (i < vertexCount - 2) {
				triangles [i * 3] = 0;
				triangles [i * 3 + 1] = i + 1;
				triangles [i * 3 + 2] = i + 2;
			}
		}

		viewMesh.Clear ();

		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals ();
	}


	EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast) {
		float minAngle = minViewCast.angle;
		float maxAngle = maxViewCast.angle;
		Vector3 minPoint = Vector3.zero;
		Vector3 maxPoint = Vector3.zero;

		for (int i = 0; i < edgeResolveIterations; i++) {
			float angle = (minAngle + maxAngle) / 2;
			ViewCastInfo newViewCast = ViewCast (angle);

			bool edgeDstThresholdExceeded = Mathf.Abs (minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
			if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded) {
				minAngle = angle;
				minPoint = newViewCast.point;
			} else {
				maxAngle = angle;
				maxPoint = newViewCast.point;
			}
		}

		return new EdgeInfo (minPoint, maxPoint);
	}


	ViewCastInfo ViewCast(float globalAngle) {
		Vector3 dir = DirFromAngle (globalAngle, true);
		RaycastHit hit;

		if (Physics.Raycast (transform.position, dir, out hit, viewRadius, obstacleMask)) {
			return new ViewCastInfo (true, hit.point, hit.distance, globalAngle);
		} else {
			return new ViewCastInfo (false, transform.position + dir * viewRadius, viewRadius, globalAngle);
		}
	}

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
		if (!angleIsGlobal) {
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

	public struct ViewCastInfo {
		public bool hit;
		public Vector3 point;
		public float dst;
		public float angle;

		public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle) {
			hit = _hit;
			point = _point;
			dst = _dst;
			angle = _angle;
		}
	}

	public struct EdgeInfo {
		public Vector3 pointA;
		public Vector3 pointB;

		public EdgeInfo(Vector3 _pointA, Vector3 _pointB) {
			pointA = _pointA;
			pointB = _pointB;
		}
	}

}