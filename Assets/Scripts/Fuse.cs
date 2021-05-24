using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuse : MonoBehaviour {

	[SerializeField] private GameObject FusePrefab;
	[SerializeField] private GameObject fuseBurn;
	[SerializeField] private bool fuseRoot;
	private List<Fuse> fuseSegments = new List<Fuse>();
	private int numSegments;
	private Bomb bomb;
	private float fuseLength;
	private float fuseRemaining;
	private int currentSegment;
	private Vector3 scale;
	private GameObject fuseBurnGO;
	private Vector3 fuseBurnPos;
	private bool fuseLit;
	private GameController gameController;

	private void Start() {
		gameController = FindObjectOfType<GameController>();
		scale = transform.localScale;
		bomb = GetComponentInParent<Bomb>();
		fuseLength = bomb.GetTimeBeforeExploding();
		if (fuseRoot) {
			InstantiateFuseSegments();
		}
	}

	private float GetRemainingFuse() {
		return fuseLength - bomb.GetTimeUntilExploding();
	}

	private void Update() {
		if (fuseRoot) {
			BurnFuse();
		}
	}

	private void InstantiateFuseSegments() {

		fuseLength = bomb.GetTimeBeforeExploding();
		numSegments = Mathf.RoundToInt(fuseLength);

		fuseSegments.Add(this);

		for (int i = 0; i < numSegments; i++) {
			fuseSegments.Add(
					Instantiate(
							FusePrefab,
							fuseSegments[fuseSegments.Count -1].transform).GetComponent<Fuse>());
		}

		fuseBurnPos = fuseSegments[fuseSegments.Count - 1].transform.position;
	}

	private void BurnFuse() {
		if (gameController.isGameReady()) {
			LightAndMoveFuse();
		}
		fuseRemaining = GetRemainingFuse();
		currentSegment = Mathf.Clamp(Mathf.FloorToInt(fuseRemaining), 0, int.MaxValue);
		
		fuseSegments[currentSegment].SetFuseLength(fuseRemaining - currentSegment);
		if (currentSegment < fuseSegments.Count -1) {
			fuseSegments[currentSegment +1].SetFuseLength(0);
		}
	}

	private void LightAndMoveFuse() {
		if (!fuseLit) {
			fuseLit = true;
			fuseBurnGO = Instantiate(fuseBurn, fuseBurnPos, Quaternion.identity, this.transform.parent);
		}

		fuseBurnGO.transform.position = fuseBurnPos;
		fuseBurnPos = fuseSegments[currentSegment].transform.position
				+ fuseSegments[currentSegment].transform.up
				* .1f;
	}

	public void SetFuseLength(float yScale) {
		scale.y = Mathf.Clamp01(yScale);
		this.transform.localScale
			= scale;
	}

}
