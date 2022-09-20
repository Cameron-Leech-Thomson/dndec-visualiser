using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ErrorMessage : MonoBehaviour
{
    private TextMeshProUGUI text = null;
    private float duration = 1f;
    private float offset = 150f;
    private string message = null;
    private bool childFound = false;
    private bool shouldStart = false;
    private Vector3 displayPosition;
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        transform.localPosition = new Vector3(0f, -600f, 0f);
        findText(transform);
        displayPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + offset, transform.localPosition.z);
    }

    void Update() {
        if (shouldStart){
            if (childFound){
                shouldStart = false;
                text.text = message;
                StartCoroutine(displayMessage());
            }
        }
    }

    private void findText(Transform root){
        foreach(Transform child in root){
            if (child.GetComponentInChildren<TextMeshProUGUI>() != null){
                text = child.GetComponentInChildren<TextMeshProUGUI>();
                childFound = true;
                return;
            } else{
                findText(child);
            }
        }
    }

    public void createErrorMessage(string message){
        this.message = message;
        shouldStart = true;
    }

    private IEnumerator displayMessage(){
        Vector3 startPosition = transform.localPosition;
        yield return StartCoroutine(lerpPosition(startPosition, displayPosition, duration));
        yield return new WaitForSeconds(5f);
        yield return StartCoroutine(lerpPosition(displayPosition, startPosition, duration));
        Destroy(gameObject);
    }

    private IEnumerator lerpPosition(Vector3 start, Vector3 finish, float d){
        float time = 0;
        while (time < duration){
            transform.localPosition = Vector3.Lerp(start, finish, time / d);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = displayPosition;
    }
}
