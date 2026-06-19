using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeRewinder : MonoBehaviour
{
    public float recordTimer = 3f; 

    private bool isRecording = false;
    private bool isRewinding = false;
    
    private float timer = 0f;
    private List<PointInTime> pointsInTime;

    private Rigidbody rb;
    
    public Image stateImage;
    public Sprite recordSprite;
    public Sprite rewindSprite;

    void Start()
    {
        pointsInTime = new List<PointInTime>();
        
        rb = GetComponent<Rigidbody>();
        
        if (stateImage != null)
        {
            stateImage.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isRecording && !isRewinding)
        {
            StartRecording();
        }
    }

    void FixedUpdate()
    {
        if (isRecording)
        {
            Record();
        }
        else if (isRewinding)
        {
            Rewind();
        }
    }

    private void StartRecording()
    {
        pointsInTime.Clear();
        timer = 0f;
        
        isRecording = true;
        
        if (stateImage != null)
        {
            stateImage.sprite = recordSprite;
            stateImage.gameObject.SetActive(true);
        }
    }

    private void Record()
    {
        pointsInTime.Add(new PointInTime(transform.position, transform.rotation));
        timer += Time.fixedDeltaTime;

        if (timer >= recordTimer)
        {
            StartRewind();
        }
    }

    private void StartRewind()
    {
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        
        isRecording = false;
        isRewinding = true;
        
        if (stateImage != null)
        {
            stateImage.sprite = rewindSprite;
            stateImage.gameObject.SetActive(true);
        }
    }

    private void Rewind()
    {
        if (pointsInTime.Count > 0)
        {
            int lastIndex = pointsInTime.Count - 1;
            PointInTime point = pointsInTime[lastIndex];

            transform.position = point.position;
            transform.rotation = point.rotation;

            pointsInTime.RemoveAt(lastIndex);
        }
        else
        {
            StopRewind();
        }
    }

    private void StopRewind()
    {
        isRewinding = false;
        
        if (rb != null)
        {
            rb.isKinematic = false;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        
        if (stateImage != null)
        {
            stateImage.gameObject.SetActive(false);
        }
    }
}

public struct PointInTime
{
    public Vector3 position;
    public Quaternion rotation;

    public PointInTime(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}