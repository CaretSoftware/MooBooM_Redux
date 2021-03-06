using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gyroscope : MonoBehaviour {

    private Camera cam;
    [SerializeField] private Vector3 mouseStartPos = Vector3.zero;
    [SerializeField] private Vector3 gravityForce = Vector3.down * 20f;
    [SerializeField] private float gravityFactor = 9f;
    [SerializeField] private float touchHeight = 0f;
    [SerializeField] private float fadeOutTime = 2f;
    [SerializeField] [Range(0f, 20f)] private float gyroscopeSensitivity = 10f;

    // Gyro
    private UnityEngine.Gyroscope gyro;
    private readonly float smallAngle = 5f;
    private readonly float calibrationSpeed = 2f;
    private readonly float lateralRatio = 200f;
    private readonly Quaternion xAxisOnly = new Quaternion(0, 1, 0, 0);
    private Quaternion gyroOffset;
    private Quaternion prevAngle;
    private Quaternion currAngle;
    private bool isGyroEnabled;
    private bool calibrating;
    private bool isGyroSettingsOn = true;
    private bool calibrated;
    private Vector2 calibrationOffset;
    private Vector3 phoneGravity;
    private Vector3 previousGravity = Vector3.down * 20;
    private float fadeOutControlls = 0;
    private float gravity = 20f;

    // Canvas
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private Image image1 = null;
    [SerializeField] private Image image2 = null;
    [SerializeField] private Image centerPosImage = null;
    [SerializeField] private GameObject images = null;
    [SerializeField] private Image touchControlsRadius = null;
    [SerializeField] private Image touchControlsCenter = null;
    [SerializeField] private Color clear = Color.white;
    private Color UiDefaultColor;
    private Vector2 centerPosCanvas;
    private Vector2 scale;

    private GameController gameController; //isGameWon

    private void Awake() {
        cam = Camera.main;
    }

    private void Start() {
        UiDefaultColor = image2.color;
        centerPosCanvas = centerPosImage.rectTransform.position;

        isGyroEnabled = EnableGyro();
        gameController = FindObjectOfType<GameController>();
    }

    private bool EnableGyro() {
        if (SystemInfo.supportsGyroscope) {
            gyro = Input.gyro;
            gyro.enabled = true;
            return true;
        }

        return false;
    }

    public bool IsCalibrated()
    {
        if (!SystemInfo.supportsGyroscope)
        {
            images.SetActive(false);
            calibrated = true;
            calibrating = false;
        }
        if (!calibrated && !calibrating)
        {
            calibrating = true;
            StartCoroutine(Calibrate());

            return false;
        }
        return calibrated;
    }

    private IEnumerator Calibrate() {

        images.SetActive(true);
        bool centered;
        float t = fadeOutTime;
        float inv;

        while (!calibrated) {
            SetDeviceRotation();

            centered = calibrationOffset.magnitude < smallAngle;

            if (centered) {
                t -= Time.deltaTime;
            } else {
                t = fadeOutTime;
            }

            image1.color = Color.Lerp(clear, UiDefaultColor, t * 2f);
            image2.color = Color.Lerp(clear, UiDefaultColor, t * 2f);
            inv = Mathf.InverseLerp(0f, .1f, image1.color.a);
            centerPosImage.color = Color.Lerp(clear, UiDefaultColor, inv);

            calibrated =
                    centered
                    && t < -.5f;

            yield return null;
        }
        images.SetActive(false);
        image1.color = UiDefaultColor;
        image2.color = UiDefaultColor;
        calibrating = false;
    }

    private void Update() {
        if (gameController != null && gameController.isGameOver()) {
            LevelOutGravity();
            TouchControlsVisuals(false);
            return;
		}
        if (Input.GetMouseButtonDown(0)) {
            mouseStartPos = ScreenPointToWorldPosition(touchHeight);
            TouchControlsVisuals(true, Input.mousePosition);

        } else if (Input.GetMouseButtonUp(0)) {
            TouchControlsVisuals(false);
        }
        if (Input.GetMouseButton(0)) {
            TouchControllGravity();
            TouchControlsVisuals(Input.mousePosition);

        } else if (isGyroEnabled && isGyroSettingsOn) {
            OrientToGravity();
        }
        previousGravity = Physics.gravity;
    }

    private void LevelOutGravity() {
        Vector3 gravity = Vector3.Lerp(
                    previousGravity,
                    Vector2.down * 20f,
                    fadeOutControlls);

        fadeOutControlls += Time.deltaTime;

        Physics.gravity = gravity;
    }

    private void TouchControlsVisuals(bool enabled, Vector3? mousePos = null) {
        touchControlsCenter.enabled = enabled;
        touchControlsRadius.enabled = enabled;
        if (mousePos != null) {
            touchControlsRadius.rectTransform.position = (Vector3)mousePos;
            touchControlsCenter.rectTransform.position = (Vector3)mousePos;
        }
    }

    private void TouchControlsVisuals(Vector3 mousePos) {
        touchControlsCenter.rectTransform.position = mousePos;
    }

    private void TouchControllGravity() {
        gravityForce =
                    (ScreenPointToWorldPosition(touchHeight) - mouseStartPos)
                    * gravityFactor
                    * gravityFactor;
        gravityForce += Vector3.down * gravity;
        Physics.gravity = gravityForce;
    }

    private Vector3 ScreenPointToWorldPosition(float y) {

        Ray mousePos = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.down, new Vector3(0, y, 0));
        ground.Raycast(mousePos, out float distance);
        return mousePos.GetPoint(distance);
    }

    private void OrientToGravity() {

        phoneGravity = ReadGyroscope();
        phoneGravity.x *= gyroscopeSensitivity;
        phoneGravity.z = phoneGravity.y * gyroscopeSensitivity;
        phoneGravity.y = -gravity;
        Physics.gravity = phoneGravity;
    }

    private Vector3 ReadGyroscope() {

        Vector3 gravity = Input.gyro.gravity;
        return gyroOffset * gravity;
    }

    private void SetDeviceRotation() {

        if (!calibrating) {
            calibrating = true;
        }
        currAngle = (Input.gyro.attitude * xAxisOnly).normalized;
        prevAngle = Quaternion.Slerp(prevAngle, currAngle, Time.deltaTime * calibrationSpeed);

        float xAngle =
                Mathf.Clamp((prevAngle * xAxisOnly).normalized.eulerAngles.x,
                0f,
                Mathf.Infinity);

        gyroOffset =
                Quaternion.Euler(xAngle, 0f, 0f).normalized;

        float angle = WrapAngle(prevAngle.eulerAngles.x)
                - WrapAngle(currAngle.eulerAngles.x);

        Vector2 pos =
                new Vector2(
                        Input.gyro.gravity.x * lateralRatio,
                        angle);

        SetCalibrationCanvasPosition(pos);
    }

    private static float WrapAngle(float angle) {
        angle %= 360f;
        return angle > 180f ? angle - 360f : angle;
    }

    public void SetCalibrationCanvasPosition(Vector2 offsetPos) {
        image1.rectTransform.position = centerPosCanvas + offsetPos;
        image2.rectTransform.position = centerPosCanvas + offsetPos;
        float x = Mathf.InverseLerp(400f, 0f, Mathf.Abs(offsetPos.x));
        float y = Mathf.InverseLerp(400f, 0f, Mathf.Abs(offsetPos.y));

        scale.x = Mathf.LerpUnclamped(0f, 1f, x);
        scale.y = Mathf.LerpUnclamped(0f, 1f, y);
        image1.rectTransform.localScale = scale;
        image2.rectTransform.localScale = scale;
        calibrationOffset = offsetPos;
    }

    public Vector3 GetGyroV3() {
        return phoneGravity;
    }
}