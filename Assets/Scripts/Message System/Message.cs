using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    [Range(0, 20)]
    public float yRangeFromTarget = 2f;
    public GameObject target;
    public float timeToDie = 5f;
    public string displayText = "...";

    private SpriteRenderer spriteTarget;
    private Text text;
    public RectTransform positionUI;

    private void Start()
    {
        text = GetComponent<Text>();
        spriteTarget = target.GetComponent<SpriteRenderer>();
        positionUI = GetComponent<RectTransform>();

        text.text = displayText;

        SetAutoDestruction();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(spriteTarget != null)
            positionUI.position = new Vector3(
                target.transform.position.x,
                target.transform.position.y + yRangeFromTarget + spriteTarget.bounds.extents.y,
                positionUI.position.z);
    }

    void DestroyMessage()
    {
        //NotificationManager.instance.notifications.Find(
        DestroyImmediate(this.gameObject);
    }

    void SetAutoDestruction(float newTimetoDie = 0f)
    {
        if(newTimetoDie > 0f)
            timeToDie = newTimetoDie;

        StopCoroutine("FadeOut");
        StartCoroutine("FadeOut");
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(timeToDie - 1f);

        text.CrossFadeAlpha(0f, 1f, false);

        StartCoroutine(DestroyMsg());
    }

    IEnumerator DestroyMsg()
    {
        yield return new WaitForSeconds(1);

        DestroyMessage();
    }
}
