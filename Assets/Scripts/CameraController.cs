using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject[] _camera;

    private void Start()
    {
        StartCoroutine(CameraOn());
    }

    private IEnumerator CameraOn()
    {
        int currentCameraIndex = 0;

        while (true)
        {
            _camera[currentCameraIndex].SetActive(true);

            yield return new WaitForSeconds(5f);
            _camera[currentCameraIndex].SetActive(false);
            currentCameraIndex++;

            if (currentCameraIndex >= _camera.Length) currentCameraIndex = 0;
        }
    }
}