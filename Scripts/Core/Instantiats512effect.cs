using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiats512effect : MonoBehaviour {

    public float _maxScale;

    public GameObject _sampleCubePrefab;
    GameObject[] _sampleCube = new GameObject[512];

	// Use this for initialization
	void Start () {

        for (int i=0;i<512;i++) {
            GameObject _insraceSampleCube = (GameObject)Instantiate(_sampleCubePrefab);
            //在父物件中心產生
            _insraceSampleCube.transform.position = this.transform.position;
            //生產出的物件依附在此物件下
            _insraceSampleCube.transform.parent = this.transform;

            _insraceSampleCube.name = "SampleCube" + i;

            this.transform.eulerAngles = new Vector3(0, 0, -0.703125f * i);
            _insraceSampleCube.transform.position = Vector3.down * 5;
            //放進array
            _sampleCube[i] = _insraceSampleCube;
        }
	}
	

	void Update () {
        for (int i=0;i<512;i++) {
            if (_sampleCube != null) {
                _sampleCube[i].transform.localScale = new Vector3(10,AudioPeer._samples[i]*_maxScale+2, 10);
            }
        }
	}
}
