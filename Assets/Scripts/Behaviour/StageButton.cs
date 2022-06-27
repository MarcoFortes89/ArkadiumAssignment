using Assets.Scripts.Models;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Behaviour
{
    public class StageButton : MonoBehaviour
    {
        public StageCreator Stage;

        private void Start()
        {
            transform.Find("Name").GetComponent<TextMeshProUGUI>().text = Stage.name;
            transform.Find("TilesValue").GetComponent<TextMeshProUGUI>().text = Stage.TileVariety.ToString();
            transform.Find("SizeValue").GetComponent<TextMeshProUGUI>().text = Stage.CubeSize.ToString();
            transform.Find("DurationValue").GetComponent<TextMeshProUGUI>().text = Stage.Duration.ToString()+" minutes";
        }

        public void SelectStage()
        {
            Core.Stage=Stage;
            SceneManager.LoadScene("Stage1");
        }
    }
}