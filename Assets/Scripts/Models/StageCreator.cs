using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [CreateAssetMenu(fileName ="new stage",menuName ="CreateStage")]
    public class StageCreator : ScriptableObject
    {
        [Range(1, 13)]
        public int TileVariety;
        [Range(1, 10)]
        public int CubeSize;
        [Range(1,10)]
        public int Duration;
    }
}
