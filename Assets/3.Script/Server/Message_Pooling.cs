using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message_Pooling : MonoBehaviour
{
    [SerializeField] private Text[] Message_Box;

    public Action<string> Message;

    private string current_me = string.Empty;
    private string past_me;


    void Start()
    {
        Message_Box = transform.GetComponentsInChildren<Text>();
        Message = AddingMessage;
        past_me = current_me;
    }

    public void AddingMessage(string me)
    {
        current_me = me;
    }


    void Update()
    {
        if(past_me.Equals(current_me)) return;
        ReadText(current_me);
        past_me = current_me;
        
    }
    public void ReadText(string me)
    {
        bool isinput = false;
        for(int i = 0; i < Message_Box.Length; i++)
        {
            if(Message_Box[i].text.Equals(""))
            {
                Message_Box[i].text = me;
                isinput = true;
                break;
            }
        }
        if(!isinput)
        {
            for(int i = 1; i < Message_Box.Length; i++)
            {
                // 메세지 미는 작업
                 Message_Box[i-1].text = Message_Box[i].text;
            }
            Message_Box[Message_Box.Length -1].text = me;
        }
   
    }

}
