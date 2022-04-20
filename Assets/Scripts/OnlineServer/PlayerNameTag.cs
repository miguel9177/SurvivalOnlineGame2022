using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerNameTag : MonoBehaviourPun
{
    //this will store the textbox with the player name tag
    [SerializeField] private TextMeshProUGUI nameText;

    private void Start()
    {
        //if this is my photon view leave
        if (photonView.IsMine)
        {
            //since this is my photon view, we want the text to be invisible, since we dont want to see our own name on top of our characters head
            nameText.text = "";
            return;
        }
        //since this isnt my photon view, we call the function SetName, and that function will give the correct name to the textbox with the player name tag
        SetName();
    }

    private void SetName()
    {
        //edit the textbox with its user nick name
        nameText.text = photonView.Owner.NickName;
    }
}
