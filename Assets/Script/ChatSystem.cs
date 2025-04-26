using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatSystem : NetworkBehaviour
{

    public TextMeshProUGUI textMessage;
    public TMP_InputField inputFieldMessage;
    public GameObject buttonSend;
    //chạy ngay sau khi nhân vật được spawn trong mạng
    public override void Spawned()
    {
        textMessage = GameObject.Find("Text Message").GetComponent<TextMeshProUGUI>();
        inputFieldMessage = GameObject.Find("InputField (Message)").GetComponent<TMP_InputField>();
        buttonSend = GameObject.Find("Button Send");
        buttonSend.GetComponent<Button>().onClick.AddListener(SendMessageChat);
    }

    public void SendMessageChat()
    {
        var message = inputFieldMessage.text;
        if (string.IsNullOrEmpty(message)) return;
        var id = Runner.LocalPlayer.PlayerId;
        var text = $"Player {id}: {message}";
        RpcChat(text);
        inputFieldMessage.text = "";
    }    

    //Sources: gửi từ đầu
    //Targets: đối tượng nhận
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcChat(string message)
    {
        textMessage.text += message + "\n";
    }    
}
