using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MainMenuScreen { MAIN, CONTROL }

public class MainMenuScreenType
{
    public MainMenuScreen mainMenuScreen;
    public GameObject menuObject;
}

public class MainMenu : MonoBehaviour
{
    List<MainMenuScreenType> menuList;
    [SerializeField] List<GameObject> menuObjectsList;

    void OnDisable()
    {
        Cursor.visible = false;
    }

    void OnEnable()
    {
        Cursor.visible = true;
    }

    public void GoToScreen(MainMenuScreenType menuToGo)
    {
        foreach (MainMenuScreenType menu in menuList)
            menu.menuObject.SetActive(menu == menuToGo);
    }

    public void GoToScreen(GameObject menuToGo)
    {
        foreach (GameObject menu in menuObjectsList)
            menu.SetActive(menu == menuToGo);
    }

}
