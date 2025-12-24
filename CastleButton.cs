using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CastleButton : MonoBehaviour
{
    public string castleId;        // Kale1, Kale2...
    public string levelSceneName;  // SampleScene
    public GameObject lockIcon;    // Kilit objesi (Kale1’de boþ olabilir)
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();

        int unlocked = PlayerPrefs.GetInt(castleId, castleId == "Kale1" ? 1 : 0);
        bool isUnlocked = unlocked == 1;

        button.interactable = isUnlocked;

        // Kale1’de kilit yoksa patlamasýn
        if (lockIcon != null)
            lockIcon.SetActive(!isUnlocked);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickCastle);
    }

    void OnClickCastle()
    {
        PlayerPrefs.SetString("SelectedCastle", castleId);
        PlayerPrefs.Save();
        SceneManager.LoadScene(levelSceneName);
    }
}
