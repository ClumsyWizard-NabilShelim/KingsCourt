using ClumsyWizard.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : CWSceneManagement
{
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    protected override void OnLoadTriggered()
    {
        animator.SetBool("Fade", true);
    }
    protected override void OnFinishLoadingScene()
    {
        animator.SetBool("Fade", false);
    }

    //UI
    protected override void LoadingProgress(float progress)
    {
    }

    //Cleanup
    protected override void CleanUpStaticData()
    {
    }
}
