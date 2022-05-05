using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    
    //this struct will store which player has wich hp bar 
    struct PlayersAndHpBar
    {
        //this will store the players and functionalities script
        public PlayerStatsAndFunctionalities playersAndFunctionalities;
        //this will store the hp bar of this player
        public Slider hpBar;
        //this will store the numebr of the player
        public int indexOfPlayer;
    }

    [System.Serializable]
    //this will store the player icon and its hpbar
    struct PlayerIconAndHpBar
    {
        public Image playerImage;
        public Slider hpBar;
    }

    
    //this will have all players information, and its hp bar
    List<PlayersAndHpBar> allPlayersAndTheirHpSliders = new List<PlayersAndHpBar>();

    //this will store all hp bars
    [SerializeField]
    PlayerIconAndHpBar[] allIconcAndHpBars;

    //this will store the number of players spawned so that it can check wich index the current player is
    int currentPlayersSpawned;

    [SerializeField]
    //this will store the text for the bullets
    public TextMeshProUGUI txtBulletsOfWeapon;

    [SerializeField]
    //this will store the text informing the player is reloading
    public TextMeshProUGUI txtReloadInformation;

    [SerializeField]
    //this will store the text informing the player hp
    public TextMeshProUGUI txtPlayerHp;

    [HideInInspector]
    //this will store the player gameobject, so that i can access every script from here, its filled by the player spawner script
    public GameObject player;

    [SerializeField]
    //this will hold the revive button, and will be used by the revive controller
    public Button reviveButton;

    [SerializeField]
    MobileJoystick movementJoystick;

    //this will store the character input, so that i can send him the movement
    CharacterInput characterInput;

    //this will be true if we are on android
    bool android=false;

    //this will be true if we have runed the late start function
    bool haveIRunedTheLateStartFunction=false;

    private void Start()
    {
        //call the courotine that is going to make this script start after everyone else 
        StartCoroutine(LateStart());
        //make this text invisible since we only want to tell the user that we are reloading, when we are reloading
        txtReloadInformation.gameObject.SetActive(false);

        //this will check if we are on android if we are, tell the code that
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("Im ON ANDROID");
            android = true;
        }
           
    }

    
    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);
        //this will assign the textbox to the gun
        player.GetComponent<CharacterWeapon>().pistol.GetComponent<GunInformation>().txtBulletsOfWeapon = txtBulletsOfWeapon;
        player.GetComponent<CharacterWeapon>().pistol.GetComponent<GunInformation>().txtReloadInformation = txtReloadInformation;

        player.GetComponent<CharacterWeapon>().knife.GetComponent<GunInformation>().txtBulletsOfWeapon = txtBulletsOfWeapon;
        player.GetComponent<CharacterWeapon>().knife.GetComponent<GunInformation>().txtReloadInformation = txtReloadInformation;

        player.GetComponent<CharacterWeapon>().rifle.GetComponent<GunInformation>().txtBulletsOfWeapon = txtBulletsOfWeapon;
        player.GetComponent<CharacterWeapon>().rifle.GetComponent<GunInformation>().txtReloadInformation = txtReloadInformation;

        //this will assign the textbox to the player
        player.GetComponent<PlayerStatsAndFunctionalities>().txtPlayerHp = txtPlayerHp;
        player.GetComponent<PlayerStatsAndFunctionalities>().UpdateMoneyText();

        //this will get the character input script
        characterInput = player.GetComponent<CharacterInput>();
            
        haveIRunedTheLateStartFunction = true;
    }

    private void Update()
    {
        //if we are on android we send the joystick movement normalized value
        if(android && haveIRunedTheLateStartFunction)
        {
            characterInput.receiveMovementFromUiManager(movementJoystick.GetNotmalizedMovement());
            txtPlayerHp.text = "Android";
        }
            
    }

    //this will add a player to the Ui Manager players list, with this i will control their hp bar
    public int PlayerSpawned(PlayerStatsAndFunctionalities playerStatsAndFunctionalities)
    {
        //this will create a new playersAndHpBar item, and this will create a new item for the all players and their hp sliders list
        PlayersAndHpBar newPlayer;
        //this will store the player stats and functionalities script, of the spawned player
        newPlayer.playersAndFunctionalities = playerStatsAndFunctionalities;
        ///this will store its hpbar
        newPlayer.hpBar = allIconcAndHpBars[currentPlayersSpawned].hpBar;

        //this will store the index of the player
        newPlayer.indexOfPlayer = currentPlayersSpawned;
        //this will add an item to the players and their hp sliders
        allPlayersAndTheirHpSliders.Add(newPlayer);

        //this will change the image of the player ui to the correct player
        allIconcAndHpBars[currentPlayersSpawned].playerImage.sprite = playerStatsAndFunctionalities.playerStats.iconOfSkin;

        //this will tell this script that we added a new player
        currentPlayersSpawned++;
        
        //this will return the index of the player spawned back to them
        return currentPlayersSpawned - 1;
    }
    
    //this will be called by the player stats and functionalities and will update the hp slider of the player that loss hp
    public void UpdateHpOfPlayer(int indexOfPlayer, int hpLeft)
    {
        //this will update the hp of the player
        allPlayersAndTheirHpSliders[indexOfPlayer].playersAndFunctionalities.playerStats.hp = hpLeft;
        //this will update the playerHp bar slider
        allPlayersAndTheirHpSliders[indexOfPlayer].hpBar.value = (float)allPlayersAndTheirHpSliders[indexOfPlayer].playersAndFunctionalities.playerStats.hp /
            (float)allPlayersAndTheirHpSliders[indexOfPlayer].playersAndFunctionalities.playerStats.maxHp;

       
    }

}
