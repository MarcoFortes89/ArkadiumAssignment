using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class Tile : MonoBehaviour
    {
        public EventHandler OnSelected,OnDestroyed;
        public int Index;
        public int TileType;
        public bool match;
        Animator animator;
        //front, right, back, left
        public Tile[] Neighborhood;

        public bool IsValid
        {
            get {
                int horizontal_1 = (Neighborhood[0] != null ? 1 : 0) + (Neighborhood[2] != null ? 1 : 0);
                int horizontal_2 = (Neighborhood[1] != null ? 1 : 0) + (Neighborhood[3] != null ? 1 : 0);
                return horizontal_1<2 && horizontal_2<2;
            }
        }

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set 
            {
                isSelected = value;
                if (isSelected)
                    transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                else
                    transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }


        private void OnMouseDown()
        {
            OnSelected?.Invoke(this, EventArgs.Empty);
        }

        public void Match(int Multiplier)
        {
            OnDestroyed.Invoke(this, EventArgs.Empty);
            GetComponent<Collider>().enabled = false;
            AnimationManager.AssignMatchAnimation(this, Multiplier);
            animator = GetComponent<Animator>();
            animator.SetTrigger("Match");
            match = true;
        }

        private void Update()
        {
            if (animator == null)
                return;
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (match && info.IsName("Match") && info.normalizedTime >= 1)
            {
                Destroy(gameObject);
            }
        }

        public void EventNeighborDestroyed(object sender, EventArgs e)
        {
            int indexOfDestroyed=Array.IndexOf(Neighborhood,sender);
            Neighborhood[indexOfDestroyed] = null;
        }
    }
}
