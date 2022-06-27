using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Management
{
    internal class Home : MonoBehaviour
    {
        public void LoadStage()
        {
            SceneManager.LoadScene("StageSelector");
        }
    }
}
