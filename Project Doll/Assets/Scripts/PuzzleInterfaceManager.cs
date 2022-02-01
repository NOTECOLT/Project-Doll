using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInterfaceManager : MonoBehaviour {
    // Manager used to cycle through which puzzle interface is active
    
    
    // SINGLETON PATTERN
    private static PuzzleInterfaceManager _instance;
    public static PuzzleInterfaceManager Instance { get {return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    // PRIVATE VARIABLES
    private List<GameObject> _interfaceList = new List<GameObject>();
    private GameObject _activatedInterface = null;
    private float _timer = 0f;

    private Vector2 _activePos = new Vector2(0, 0);
    private Vector2 _inactivePos = new Vector2(5000, 5000);

    // PUBLIC VARIABLES
    public bool hasActiveInterface = false;

    void Start() {
        foreach (Transform child in transform) {
            if (child.gameObject.tag == "PuzzleInterface") {
                _interfaceList.Add(child.gameObject);
                child.gameObject.SetActive(true);
                child.gameObject.GetComponent<RectTransform>().localPosition = _inactivePos;
            }
        }
    }

    void Update() {
        _timer += Time.deltaTime;
        if (hasActiveInterface && _timer >= 0.25f) {
            if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.E)) {
                _activatedInterface.GetComponent<RectTransform>().localPosition = _inactivePos;
                _activatedInterface = null;
                hasActiveInterface = false;
            }
        }
    }

    // Activates a single interface while leaves the rest deactivated
    public void ActivateInterface(string interfaceName) {
        foreach (GameObject interfaceChild in _interfaceList) {
            if (interfaceChild.name == interfaceName) {
                interfaceChild.GetComponent<RectTransform>().localPosition = _activePos;
                _activatedInterface = interfaceChild;
                hasActiveInterface = true;
            } else
                interfaceChild.GetComponent<RectTransform>().localPosition = _inactivePos;
        }

        _timer = 0f;
    }
}