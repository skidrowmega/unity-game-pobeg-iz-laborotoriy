using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    [SerializeField] public VoxelCut cuttingboard;

    [SerializeField] Text OutputText;

    float StartTime;

    bool started = false;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (started&&!cuttingboard.Submitted)
        {
            int timeleft = ((int)(cuttingboard.Timer + StartTime - Time.time));
            OutputText.text = timeleft.ToString();
            if (timeleft <= 0)
            {
                StopTimer();
                OutputText.text = "0";
                CutoutManager.instance.SubmitButtonPress();
                Destroy(this);
            }
        }
        
    }

    public void StartTimer()
    {
        StartTime= Time.time;
        started = true;
    }

    public void StopTimer()
    {
        started = false;
    }
    public void Reset()
    {
        OutputText.text = cuttingboard.Timer.ToString();
    }
}
