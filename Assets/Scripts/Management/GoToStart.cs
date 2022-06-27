using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Management
{
    public class GoToStart : MonoBehaviour
    {
        public void LoadStage()
        {
            SceneManager.LoadScene("Home");
        }
    }
}
