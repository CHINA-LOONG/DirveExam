using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoadingWindow : UIDialog
{
    public ProgressBar progressBar;

    private AsyncOperation async;
    public void InitWith(AsyncOperation async)
    {
        this.async = async;
    }

    private void Update()
    {
        progressBar.Value = async.progress;
    }
}
