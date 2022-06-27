using UnityEngine;
using UnityEngine.Animations;

public class CameraManager : MonoBehaviour
{
    Animator animator;
    int CameraPos = 1;
    ConstraintSource constraintSource;
    float offset = 0.5f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        int gridSize = Core.Stage.CubeSize;
        constraintSource.sourceTransform = Core.Match.transform;
        constraintSource.weight = 1;
        float scale = Mathf.Pow(1.1f, gridSize);
        transform.parent.transform.localScale=new Vector3(scale, scale, scale);
        GetComponent<LookAtConstraint>().AddSource(constraintSource);
    }

    bool updateCamera;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (CameraPos == 4)
                CameraPos = 1;
            else
                CameraPos++;
            updateCamera = true;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (CameraPos == 1)
                CameraPos = 4;
            else
                CameraPos--;
            updateCamera = true;
        }
        if (updateCamera)
        {
            animator.SetInteger("Pos", CameraPos);
            updateCamera = false;
        }
    }
}
