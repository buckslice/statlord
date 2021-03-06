﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class StatUI : MonoBehaviour {

    public bool visible = false;
    public Font font;
    public Sprite buttonSprite;
    public Sprite buttonDownSprite;
    public Sprite buttonHightlightSprite;
    public Sprite buttonDisabled;

    public Button undoButton;
    public Button doneButton;

    private bool lastVisible = false;

    private PlayerStats stats;

    private GameObject mainPanel;
    private GameObject topPanel;
    private List<RectTransform> statPanels = new List<RectTransform>();
    private List<Button> buttons = new List<Button>();

    private Text pointsLeftText;
    private int pointsLeft = 5;

    private Stack<string> actions = new Stack<string>();
    private Dictionary<string, Text> valueTable = new Dictionary<string, Text>();

    // Use this for initialization
    void Start() {
        stats = GameObject.Find("Player").GetComponent<PlayerStats>();

        mainPanel = new GameObject("main stat panel");
        RectTransform mpt = mainPanel.AddComponent<RectTransform>();
        mpt.SetParent(transform, false);
        mpt.offsetMin = Vector2.zero;
        mpt.offsetMax = Vector2.zero;
        mpt.anchorMin = new Vector2(0.0f, 0.0f);
        mpt.anchorMax = new Vector2(1.0f, 0.8f);

        topPanel = new GameObject("top panel");
        RectTransform tpt = topPanel.AddComponent<RectTransform>();
        tpt.SetParent(transform, false);
        tpt.offsetMin = Vector2.zero;
        tpt.offsetMax = Vector2.zero;
        tpt.anchorMin = new Vector2(0.0f, 0.8f);
        tpt.anchorMax = new Vector2(1.0f, 1.0f);
        topPanel.AddComponent<Image>().color = new Color(0, 0, 0, 0.5f);

        GameObject titlePanel = new GameObject("title panel");
        RectTransform rt = titlePanel.AddComponent<RectTransform>();
        rt.SetParent(tpt, false);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.anchorMin = new Vector2(0.05f, 0.0f);
        rt.anchorMax = new Vector2(0.95f, 1.0f);
        Text t1 = titlePanel.AddComponent<Text>();
        t1.text = "UPGRADE STATS!";
        t1.font = font;
        t1.resizeTextForBestFit = true;
        t1.resizeTextMinSize = 10;
        t1.resizeTextMaxSize = 200;
        t1.color = Color.yellow;
        t1.alignment = TextAnchor.MiddleCenter;

        GameObject sidePanel = new GameObject("side panel");
        rt = sidePanel.AddComponent<RectTransform>();
        rt.SetParent(mpt, false);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.anchorMin = new Vector2(0.8f, 0.2f);
        rt.anchorMax = new Vector2(1.0f, 0.8f);
        sidePanel.AddComponent<Image>().color = new Color(0, 0, 0, 0.5f);

        GameObject pointsPanel = new GameObject("point title panel");
        rt = pointsPanel.AddComponent<RectTransform>();
        rt.SetParent(mpt, false);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.anchorMin = new Vector2(0.8f, 0.65f);
        rt.anchorMax = new Vector2(1.0f, 0.8f);
        t1 = pointsPanel.AddComponent<Text>();
        t1.text = "points";
        t1.font = font;
        t1.resizeTextForBestFit = true;
        t1.resizeTextMinSize = 10;
        t1.resizeTextMaxSize = 200;
        t1.color = Color.magenta;
        t1.alignment = TextAnchor.MiddleCenter;

        pointsPanel = new GameObject("point value panel");
        rt = pointsPanel.AddComponent<RectTransform>();
        rt.SetParent(mpt, false);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.anchorMin = new Vector2(0.8f, 0.2f);
        rt.anchorMax = new Vector2(1.0f, 0.8f);
        pointsLeftText = pointsPanel.AddComponent<Text>();
        pointsLeftText.font = font;
        pointsLeftText.resizeTextForBestFit = true;
        pointsLeftText.resizeTextMinSize = 10;
        pointsLeftText.resizeTextMaxSize = 300;
        pointsLeftText.color = Color.magenta;
        pointsLeftText.alignment = TextAnchor.MiddleCenter;

        GameObject undoButtonGO = new GameObject("undo button");
        RectTransform ubt = undoButtonGO.AddComponent<RectTransform>();
        ubt.SetParent(mpt, false);
        ubt.offsetMin = Vector2.zero;
        ubt.offsetMax = Vector2.zero;
        ubt.anchorMin = new Vector2(0.8f, 0.8f);
        ubt.anchorMax = new Vector2(1.0f, 1.0f);

        undoButton = buildButton(undoButtonGO, "UNDO", Color.red);
        UnityAction ba = new UnityAction(() => undoLast());
        undoButton.onClick.AddListener(ba);
        undoButton.interactable = false;

        GameObject doneButtonGO = new GameObject("done button");
        RectTransform dbt = doneButtonGO.AddComponent<RectTransform>();
        dbt.SetParent(mpt, false);
        dbt.offsetMin = Vector2.zero;
        dbt.offsetMax = Vector2.zero;
        dbt.anchorMin = new Vector2(0.8f, 0.0f);
        dbt.anchorMax = new Vector2(1.0f, 0.2f);

        doneButton = buildButton(doneButtonGO, "DONE", Color.green);
        ba = new UnityAction(() => visible = false);
        doneButton.onClick.AddListener(ba);
        doneButton.interactable = false;

    }

    // Update is called once per frame
    void Update() {
        if (visible && !lastVisible) {
            mainPanel.SetActive(true);
            topPanel.SetActive(true);
            //buildUI();
        }

        if (!visible) {
            // clear old panels
            for (int i = 0; i < statPanels.Count; ++i) {
                Destroy(statPanels[i].gameObject);
            }
            statPanels.Clear();
            mainPanel.SetActive(false);
            topPanel.SetActive(false);
        } else {
            pointsLeftText.text = pointsLeft.ToString();
        }

        lastVisible = visible;
    }

    public void buildUI(int level) {
        // more linear than one for each level
        // should make the early game easier and late game harder
        pointsLeft = 3 + (level / 2);

        visible = true;
        actions.Clear();
        valueTable.Clear();
        buttons.Clear();

        int len = Mathf.Min(stats.numStats(), level + 3);

        for (int i = 0; i < len; ++i) {
            Stat stat = stats.get(i);
            if (stat == null) {
                //Debug.Log("OUT OF STATS!");
                return;
            }

            // later have max rows variable then make new column and balance
            // if rows > 10 make two columns of 5 maybe
            GameObject statPanel = new GameObject(stat.name + " panel");
            RectTransform spt = statPanel.AddComponent<RectTransform>();
            spt.SetParent(mainPanel.transform, false);
            spt.offsetMin = Vector2.zero;
            spt.offsetMax = Vector2.zero;
            //statPanel.AddComponent<Image>().color = Random.ColorHSV(0.6f, 0.7f, 1, 1, .1f, .2f, .5f, .5f);
            Image img = statPanel.AddComponent<Image>();

            img.color = new Color(0.0f, (i % 2 == 0 ? 0.0f : 0.2f), 0.4f, 0.75f);
            float fi = i;
            spt.anchorMin = new Vector2(0.0f, 1.0f - (fi + 1) / len);
            spt.anchorMax = new Vector2(0.8f, 1.0f - fi / len);

            // add name of stat
            GameObject namePanel = new GameObject("name");
            RectTransform npt = namePanel.AddComponent<RectTransform>();
            npt.SetParent(spt, false);
            npt.anchorMin = Vector2.zero;
            npt.anchorMax = new Vector2(0.5f, 1.0f);
            npt.offsetMin = Vector2.zero;
            npt.offsetMax = Vector2.zero;

            Text text = namePanel.AddComponent<Text>();
            text.text = stat.name;//.PadLeft(10);
            text.font = font;
            text.alignment = TextAnchor.MiddleCenter;
            //text.fontSize = 40;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 10;
            text.resizeTextMaxSize = 80;
            //text.horizontalOverflow = HorizontalWrapMode.Overflow;

            // add current stat value
            GameObject valuePanel = new GameObject("value");
            RectTransform vpt = valuePanel.AddComponent<RectTransform>();
            vpt.SetParent(spt, false);
            vpt.anchorMin = new Vector2(0.55f, 0.0f);
            vpt.anchorMax = new Vector2(0.75f, 1.0f);
            vpt.offsetMin = Vector2.zero;
            vpt.offsetMax = Vector2.zero;

            text = valuePanel.AddComponent<Text>();
            text.color = Color.yellow;
            text.text = stat.value.ToString("F2");
            text.font = font;
            text.alignment = TextAnchor.MiddleCenter;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 10;
            text.resizeTextMaxSize = 100;
            valueTable[stat.name] = text;

            // show increment and also make it a button???
            GameObject incPanel = new GameObject("increment");
            RectTransform ipt = incPanel.AddComponent<RectTransform>();
            ipt.SetParent(spt, false);
            ipt.anchorMin = new Vector2(0.8f, 0.0f);
            ipt.anchorMax = new Vector2(1.0f, 1.0f);
            ipt.offsetMin = Vector2.zero;
            ipt.offsetMax = Vector2.zero;

            //Button button = incPanel.AddComponent<Button>();
            string buttonText;
            if (stat.increment > 0.0f) {
                buttonText = "+" + stat.increment.ToString("F2");
            } else {
                buttonText = stat.increment.ToString("F2");
            }
            Button button = buildButton(incPanel, buttonText, new Color(0.0f, 0.0f, 1.0f));
            UnityAction ba = new UnityAction(() => addToList(stat.name));
            button.onClick.AddListener(ba);
            buttons.Add(button);
            statPanels.Add(spt);

        }

        checkButtonsInteractible();
    }

    private Button buildButton(GameObject parent, string text, Color color) {
        Button button = parent.AddComponent<Button>();
        Image buttonImage = parent.AddComponent<Image>();
        buttonImage.sprite = buttonSprite;
        button.targetGraphic = buttonImage;
        //button.transition = Selectable.Transition.ColorTint;
        //ColorBlock cb = button.colors;
        //cb.normalColor = new Color(0.66f, 0.66f, 0.66f);
        //cb.highlightedColor = Color.white;
        //cb.pressedColor = new Color(0.33f, 0.33f, 0.33f);
        //cb.fadeDuration = 0.0f;
        //button.colors = cb;

        button.transition = Selectable.Transition.SpriteSwap;
        SpriteState ss = new SpriteState();
        ss.highlightedSprite = buttonHightlightSprite;
        ss.pressedSprite = buttonDownSprite;
        ss.disabledSprite = buttonDisabled;
        button.spriteState = ss;
        // if no more points left
        //button.interactable = false;

        Navigation nm = new Navigation();
        nm.mode = Navigation.Mode.None;
        button.navigation = nm;

        // add text to button
        GameObject buttonTextObj = new GameObject("text");
        Text buttonText = buttonTextObj.AddComponent<Text>();
        RectTransform trt = buttonText.rectTransform;
        trt.SetParent(button.transform, false);
        trt.anchorMin = Vector2.zero;
        trt.anchorMax = Vector2.one;
        trt.anchorMin = new Vector2(0.1f, 0.0f);
        trt.anchorMax = new Vector2(0.9f, 1.0f);
        trt.offsetMin = Vector2.zero;
        trt.offsetMax = Vector2.zero;
        buttonText.text = text;
        buttonText.font = font;
        buttonText.color = color;
        buttonText.alignment = TextAnchor.MiddleCenter;
        //text.fontSize = 40;
        buttonText.resizeTextForBestFit = true;
        buttonText.resizeTextMinSize = 10;
        buttonText.resizeTextMaxSize = 100;

        return button;
    }

    private void checkButtonsInteractible() {
        doneButton.interactable = pointsLeft == 0;
        undoButton.interactable = actions.Count != 0;


        for (int i = 0; i < buttons.Count; ++i) {
            if (pointsLeft == 0) {
                buttons[i].interactable = false;
            } else {
                Stat s = stats.get(i);
                if (s == null) {
                    Debug.Log("wtfack stats");
                    return;
                }

                if ((s.value >= s.cap && s.increment > 0.0f) ||
                    (s.value < s.cap && s.increment < 0.0f)) {
                    buttons[i].interactable = false;
                    valueTable[s.name].color = Color.red;
                } else {
                    buttons[i].interactable = true;
                    valueTable[s.name].color = Color.yellow;
                }
            }
        }
    }

    public void addToList(string name) {
        if (pointsLeft > 0) {
            //Debug.Log(name + " " + Time.deltaTime);
            Stat s = stats.get(name);
            s.value += s.increment;
            valueTable[s.name].text = s.value.ToString("F2");
            actions.Push(name);
            pointsLeft--;
        }
        checkButtonsInteractible();

    }

    // add undo that call undo
    private void undoLast() {
        if (actions.Count > 0) {
            string last = actions.Pop();
            Stat s = stats.get(last);
            s.value -= s.increment;
            valueTable[s.name].text = s.value.ToString("F2");
            pointsLeft++;
        }
        checkButtonsInteractible();
    }

}
