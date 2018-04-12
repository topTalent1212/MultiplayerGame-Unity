﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{

    public Transform chatMessageContainer;
    public GameObject messagePrefab;
    public GameObject privateMessagePrefab;
    public GameObject alertMessagePrefab;

    public InputField input;
    public AudioSource notification;

    public Client client;
    public ButtonManager buttonManager;

    List<GameObject> allMessages;

    public enum Type
    {
        NORMAL,
        PRIVATE,
        ALERT
    }

    void Start()
    {
        input = GameObject.Find("MessageInput").GetComponent<InputField>();
        allMessages = new List<GameObject>();

        client = FindObjectOfType<Client>();
        buttonManager = FindObjectOfType<ButtonManager>();

        if(!GameManager.Instance.editor)
            notification = GetComponent<AudioSource>();
        else
        {
            Transform chat = GameObject.Find("Chat").transform;
            foreach(Transform t in chat)
            {
                t.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.T))
            input.ActivateInputField();
        if(Input.GetKey("return"))
            SendChatMessage();
    }

    public void ChatMessage(string message, Type type)
    {
        GameObject msg;
        if(type == Type.NORMAL)
            msg = Instantiate(messagePrefab) as GameObject;
        else if(type == Type.PRIVATE)
            msg = Instantiate(privateMessagePrefab) as GameObject;
        else
            msg = Instantiate(alertMessagePrefab) as GameObject;

        msg.transform.SetParent(chatMessageContainer);
        msg.GetComponentInChildren<Text>().text = message;

        allMessages.Add(msg);

        notification.Play();
    }

    public void SendChatMessage()
    {
        if(input.text == "")
            return;

        if(input.text[0] == '/')
        {
            int index = 1;
            string command = ExtractCommand(ref index, input.text);
            index++;
            switch(command)
            {
                case "msg":
                    string receiver = ExtractCommand(ref index, input.text);
                    string message = ExtractMessage(++index, input.text);
                    client.Send("CMSP|" + receiver + "|" + message);
                    break;
                case "god":
                    buttonManager.CheatMode();
                    ChatMessage("GODMOD command", ChatManager.Type.ALERT);
                    break;

                case "clear":
                    string commandClear = ExtractCommand(ref index, input.text);
                    if(commandClear == "unit" || commandClear == "units")
                        FindObjectOfType<HexGrid>().ClearUnits();
                    else if(commandClear == "" || commandClear == "msg" || commandClear == "message" || commandClear == "messages")
                    {
                        int indexMessages = allMessages.Count - 1;
                        while(indexMessages >= 0)
                        {
                            Destroy(allMessages[indexMessages]);
                            --indexMessages;
                        }
                        allMessages.Clear();
                    }
                    else
                        ChatMessage("ERROR: Unknown command \"/clear " + commandClear + "\"", Type.ALERT);
                    break;

                case "unit":
                    string commandUnit = ExtractCommand(ref index, input.text);


                    ChatMessage("ERROR: Unknown command \"unit " + commandUnit + "\"", Type.ALERT);
                    break;

                case "kick":
                    string kicked = ExtractCommand(ref index, input.text);
                    if(kicked == "")
                        ChatMessage("You didn't specified the player to kick", Type.ALERT);
                    string kickMessage = ExtractMessage(++index, input.text);
                    client.Send("CKIK|" + kicked + "|" + kickMessage);
                    break;

                case "help":
                    string helpCommand = ExtractCommand(ref index, input.text);
                    if(helpCommand == "")
                        ChatMessage("The available commands are : msg, clear, unit, kick", Type.ALERT);
                    else
                    {
                        switch(helpCommand)
                        {
                            case "msg":
                                ChatMessage("Use to talk with another player in private : /msg playerName message", Type.ALERT);
                                break;

                            case "god":
                                ChatMessage("Use to cheat", Type.ALERT);
                                break;

                            case "clear":
                                ChatMessage("Use to clear something (messages by default): /clear [msg.message.messages.unit.units]", Type.ALERT);
                                break;

                            case "unit":
                                ChatMessage("Use to spawn unit : /unit ????", Type.ALERT);
                                break;

                            case "kick":
                                ChatMessage("Use to kick a player : /kick playerName [reason]", Type.ALERT);
                                break;

                            case "help":
                                ChatMessage("You need help...", Type.ALERT);
                                break;

                            default:
                                ChatMessage("ERROR: Unknown command \"" + helpCommand + "\"", Type.ALERT);
                                break;
                        }
                    }
                    break;

                case "code":
                    switch(ExtractCommand(ref index, input.text))
                    {
                        case "coinage":
                            client.player.Money += 1000;
                            break;
                    }
                    break;

                default:
                    ChatMessage("ERROR: Unknown command \"" + command + "\"", Type.ALERT);
                    break;
            }
        }
        else
            client.Send("CMSG|0|" + input.text);
        input.text = "";
    }

    private string ExtractCommand(ref int index, string data)
    {
        string command = "";
        while(index < data.Length && data[index] != ' ')
        {
            command += data[index];
            index++;
        }
        return command;
    }

    private string ExtractMessage(int index, string data)
    {
        string message = "";
        while (index < data.Length)
        {
            message += data[index];
            index++;
        }
        return message;
    }
}