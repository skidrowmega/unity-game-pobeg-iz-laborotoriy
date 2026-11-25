using System;
using UnityEngine;

public class FrogDialog : Trigger
{
    public void InitiateDialog(string[] lines)
    {
        DialogueBox.StartDialogue(lines, OnDialogueEnd);
    }
    public void InitiateDialog(string[] lines, Action callback)
    {
        AddCallback(callback);
        InitiateDialog(lines);
    }
    void OnDialogueEnd()
    {
        OnSuccess();
    }

}
