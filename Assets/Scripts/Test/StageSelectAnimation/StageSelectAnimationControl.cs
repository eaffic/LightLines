using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEnumList;

public class StageSelectAnimationControl : UnitySingleton<StageSelectAnimationControl>
{
    [SerializeField] private GameObject _groundBlock;
    [SerializeField] private GameObject _groundStair;

    [SerializeField] private GameObject[] _stageMagicCircle;
    [SerializeField] private Transform _player;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start() {
        if (GameManager.CurrentScene != SceneType.StageSelect) { return; }

        SetPlayerPosition();
        StartToAreaAnimation();
    }

    private void SetPlayerPosition(){
        if(GameManager.PreviewScene >= SceneType.Stage1_1){
            _player.transform.position = _stageMagicCircle[GameManager.PreviewScene - SceneType.Stage1_1].transform.position;
            _player.transform.localEulerAngles = _stageMagicCircle[GameManager.PreviewScene - SceneType.Stage1_1].transform.localEulerAngles;
        }
    }

    private void StartToAreaAnimation()
    {
        SceneType scene = DataManager.Instance.GetLastClearStage();
        switch (scene)
        {
            case SceneType.Stage1_8:
                StartCoroutine(ToArea9Animation(false));
                break;
            case SceneType.Stage1_7:
                StartCoroutine(ToArea8Animation(false));
                break;
            case SceneType.Stage1_6:
                StartCoroutine(ToArea7Animation(false));
                break;
            case SceneType.Stage1_5:
                StartCoroutine(ToArea6Animation(false));
                break;
            case SceneType.Stage1_4:
                StartCoroutine(ToArea5Animation(false));
                break;
            case SceneType.Stage1_3:
                StartCoroutine(ToArea4Animation(false));
                break;
            case SceneType.Stage1_2:
                StartCoroutine(ToArea3Animation(false));
                break;
            case SceneType.Stage1_1:
                StartCoroutine(ToArea2Animation(false));
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Area2までの通路生成
    /// </summary>
    IEnumerator ToArea2Animation(bool isDone)
    {
        Vector3 basicPoint = new Vector3(61, -25f, -24f); //最終位置の基本値
        Vector3 endOffset = new Vector3(0, 0, 1); //ループ中の変化量
        Vector3 startOffset = new Vector3(0, -10f, 0); //生成位置の移動量
        float duration = 2f; //生成したブロックの移動時間
        float loops = 7; //ループ回数
        for (int i = 0; i < loops; ++i)
        {
            CreateBlock(basicPoint, startOffset * Mathf.Pow(-1, i), endOffset * i, duration, isDone);
            if(isDone == false) yield return new WaitForSeconds(0.1f);
        }
        EventCenter.ButtonNotify(0, true);
        _stageMagicCircle[1].SetActive(true);
        yield return null;
    }

    /// <summary>
    /// Area3までの通路生成
    /// </summary>
    IEnumerator ToArea3Animation(bool isDone)
    {
        StartCoroutine(ToArea2Animation(true));

        Vector3 basicPoint = new Vector3(61, -25f, -11); //最終位置の基本値
        Vector3 endOffset = new Vector3(0, 0, 1); //ループ中の変化量
        Vector3 startOffset = new Vector3(0, -10f, 0); //生成位置の移動量
        float duration = 2f; //生成したブロックの移動時間
        float loops = 6; //ループ回数
        for (int i = 0; i < loops; ++i)
        {
            CreateBlock(basicPoint, startOffset * Mathf.Pow(-1, i), endOffset * i, duration, isDone);
            if(isDone == false) yield return new WaitForSeconds(0.1f);
        }

        basicPoint = new Vector3(60, -24, -4);
        endOffset = new Vector3(-1, 1, 0);
        loops = 5;
        for (int i = 0; i < loops; ++i)
        {
            var item = CreateStair(basicPoint, startOffset, endOffset * i, duration, isDone);
            item.transform.localEulerAngles = new Vector3(0, -90, 0);
            if (isDone == false) yield return new WaitForSeconds(0.1f);
        }

        EventCenter.ButtonNotify(1, true);
        _stageMagicCircle[2].SetActive(true);
        yield return null;
    }

    /// <summary>
    /// Area4までの通路生成
    /// </summary>
    IEnumerator ToArea4Animation(bool isDone){
        StartCoroutine(ToArea3Animation(true));

        Vector3 basicPoint = new Vector3(49, -20f, -4); //最終位置の基本値
        Vector3 endOffset = new Vector3(-1, 0, 0); //ループ中の変化量
        Vector3 startOffset = new Vector3(0, -10f, 0); //生成位置の移動量
        float duration = 2f; //生成したブロックの移動時間
        float loops = 11; //ループ回数
        for (int i = 0; i < loops; ++i)
        {
            var item = CreateBlock(basicPoint, startOffset * Mathf.Pow(-1, i), endOffset * i, duration, isDone);
            item.transform.localEulerAngles = new Vector3(0, 90, 0);
            if(isDone == false) yield return new WaitForSeconds(0.1f);
        }

        basicPoint = new Vector3(36, -20, -6);
        endOffset = new Vector3(0, 0, -1);
        loops = 8;
        for (int i = 0; i < loops; ++i)
        {
            var item = CreateBlock(basicPoint, startOffset * Mathf.Pow(-1, i), endOffset * i, duration, isDone);
            if (isDone == false) yield return new WaitForSeconds(0.1f);
        }

        EventCenter.ButtonNotify(2, true);
        _stageMagicCircle[3].SetActive(true);
        yield return null;
    }

    /// <summary>
    /// Area5までの通路生成
    /// </summary>
    IEnumerator ToArea5Animation(bool isDone){
        StartCoroutine(ToArea4Animation(true));

        Vector3 basicPoint = new Vector3(34, -19f, -16); //最終位置の基本値
        Vector3 endOffset = new Vector3(-1, 0, 0); //ループ中の変化量
        Vector3 startOffset = new Vector3(0, -10f, 0); //生成位置の移動量
        float duration = 2f; //生成したブロックの移動時間
        float loops = 5; //ループ回数
        for (int i = 0; i < loops; ++i){
            var stair = CreateStair(basicPoint, startOffset * Mathf.Pow(-1, i), new Vector3(-3, 1, 0) * i, duration, isDone);
            stair.transform.localEulerAngles = new Vector3(0, -90, 0);
            if(isDone == false) yield return new WaitForSeconds(0.1f);

            var block = CreateBlock(basicPoint, startOffset * Mathf.Pow(-1, i), endOffset + new Vector3(-3, 1, 0) * i, duration, isDone);
            block.transform.localEulerAngles = new Vector3(0, 90, 0);
            if(isDone == false) yield return new WaitForSeconds(0.1f);

            block = CreateBlock(basicPoint, startOffset * Mathf.Pow(-1, i), endOffset * 2 + new Vector3(-3, 1, 0) * i, duration, isDone);
            block.transform.localEulerAngles = new Vector3(0, 90, 0);
            if (isDone == false) yield return new WaitForSeconds(0.1f);
        }

        basicPoint = new Vector3(17, -15, -18);
        endOffset = new Vector3(0, 0, -1);
        loops = 7;
        for (int i = 0; i < loops; ++i){
            CreateBlock(basicPoint, startOffset * Mathf.Pow(-1, i), endOffset * i, duration, isDone);
            if(isDone == false) yield return new WaitForSeconds(0.1f);
        }

        EventCenter.ButtonNotify(3, true);
        _stageMagicCircle[4].SetActive(true);
        yield return null;
    }

    /// <summary>
    /// Area6までの通路生成
    /// </summary>
    IEnumerator ToArea6Animation(bool isDone){
        StartCoroutine(ToArea5Animation(true));

        Vector3 basicPoint = new Vector3(17, -15f, -16); //最終位置の基本値
        Vector3 endOffset = new Vector3(-1, 0, 0); //ループ中の変化量
        Vector3 startOffset = new Vector3(0, -10f, 0); //生成位置の移動量
        float duration = 2f; //生成したブロックの移動時間
        float loops = 10; //ループ回数
        for (int i = 0; i < loops; ++i){
            var block = CreateBlock(basicPoint, startOffset * Mathf.Pow(-1, i), endOffset * i, duration, isDone);
            block.transform.localEulerAngles = new Vector3(0, 90, 0);
            if(isDone == false) yield return new WaitForSeconds(0.1f);
        }

        EventCenter.ButtonNotify(4, true);
        _stageMagicCircle[5].SetActive(true);
        yield return null;
    }

    /// <summary>
    /// Area7までの通路生成
    /// </summary>
    IEnumerator ToArea7Animation(bool isDone){
        StartCoroutine(ToArea6Animation(true));

        Vector3 basicPoint = new Vector3(1, -5f, -13); //最終位置の基本値
        Vector3 endOffset = new Vector3(0, 0, 1); //ループ中の変化量
        Vector3 startOffset = new Vector3(0, -10f, 0); //生成位置の移動量
        float duration = 2f; //生成したブロックの移動時間
        float loops = 7; //ループ回数
        for (int i = 0; i < loops; ++i){
            CreateBlock(basicPoint, startOffset * Mathf.Pow(-1, i), endOffset * i, duration, isDone);
            if(isDone == false) yield return new WaitForSeconds(0.1f);
        }

        EventCenter.ButtonNotify(5, true);
        _stageMagicCircle[6].SetActive(true);
        yield return null;
    }

    /// <summary>
    /// Area8までの通路生成
    /// </summary>
    IEnumerator ToArea8Animation(bool isDone){
        StartCoroutine(ToArea7Animation(true));

        EventCenter.ButtonNotify(6, true);
        _stageMagicCircle[7].SetActive(true);
        yield return null;
    }

    /// <summary>
    /// Area9までの通路生成
    /// </summary>
    IEnumerator ToArea9Animation(bool isDone){
        StartCoroutine(ToArea8Animation(true));
        yield return null;
    }

    /// <summary>
    /// 地面ブロック生成
    /// </summary>
    /// <param name="basicPoint">最終位置の基本値</param>
    /// <param name="endOffset">ループ中の変化量</param>
    /// <param name="startOffset">生成位置の移動量</param>
    /// <param name="duration">生成したブロックの移動時間</param>
    /// <param name="done">移動なし生成</param>
    private GameObject CreateBlock(Vector3 basicPoint, Vector3 startOffset, Vector3 endOffset, float duration, bool done)
    {
        var item = GameObject.Instantiate(_groundBlock);

        if (done)
        {
            item.transform.position = basicPoint + endOffset;
        }
        else
        {
            item.AddComponent<BlockMovement>().SetStartAndEndPosition(basicPoint + startOffset + endOffset, basicPoint + endOffset, duration);
        }

        item.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        item.layer = LayerMask.NameToLayer("Ground");
        item.transform.parent = this.transform;
        return item;
    }

    /// <summary>
    /// 階段ブロック生成
    /// </summary>
    /// <param name="basicPoint"></param>
    /// <param name="endOffset"></param>
    /// <param name="startOffset"></param>
    /// <param name="duration"></param>
    /// <param name="done"></param>
    private GameObject CreateStair(Vector3 basicPoint, Vector3 startOffset, Vector3 endOffset, float duration, bool done){
        var item = GameObject.Instantiate(_groundStair);

        if (done)
        {
            item.transform.position = basicPoint + endOffset;
        }
        else
        {
            item.AddComponent<BlockMovement>().SetStartAndEndPosition(basicPoint + startOffset + endOffset, basicPoint + endOffset, duration);
        }

        item.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        item.layer = LayerMask.NameToLayer("Ground");
        item.transform.parent = this.transform;
        return item;
    }
}