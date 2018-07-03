using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

public class UnityGuiConsole : MonoBehaviour {
    public static UnityGuiConsole Instance { get; private set; }

	private static readonly int MAX_LOG = 250;
	private static readonly int WND_ID = 0x1435;
	private static readonly float EDGE_X = 16, EDGE_Y = 8;

	public bool Visible = false;

	private readonly string[] logTypeNames_;
	private readonly Queue<string>[] logList_;
	private readonly Vector2[] scrollPos_;

	private UnityGuiConsole() {
		this.logTypeNames_ = Enum.GetNames(typeof(LogType));
		this.logList_ = new Queue<string>[this.logTypeNames_.Length];
		this.scrollPos_ = new Vector2[this.logTypeNames_.Length];
		for (int i = 0; i < logList_.Length; ++i) {
			this.logList_[i] = new Queue<string>(MAX_LOG);
			this.scrollPos_[i] = new Vector2(0, 1);
		}
	}

	void Start(){
	    Instance = this;        
		Application.RegisterLogCallback(LogCallback);
	}

    private float CoolDown_ = 0;
    void Update(){
        if ( ( Input.touches.Length >= 3 || ( Input.GetMouseButton( 0 ) && Input.GetMouseButton( 1 ) ) )
            && Time.time - CoolDown_ > 2.0f ) {
            Visible = !Visible;
            CoolDown_ = Time.time;
        }
    }

	private int logTypeChoose_ = (int)LogType.Log;
	private Rect rcWindow_;

	void OnGUI() {
		if (!Visible) { return; }
		EventType et = Event.current.type;
		if (et == EventType.Repaint || et == EventType.Layout) {
			this.rcWindow_ = new Rect(EDGE_X, EDGE_Y, Screen.width - EDGE_X * 2, Screen.height - EDGE_Y * 2);
			GUI.Window(WND_ID, rcWindow_, WindowFunc, string.Empty);
		}
	}

	void WindowFunc(int id) {
		try {
			GUILayout.BeginVertical();
			try {
				logTypeChoose_ = GUILayout.Toolbar(logTypeChoose_, this.logTypeNames_);
				var queue = this.logList_[logTypeChoose_];
				if (queue.Count > 0) {
					scrollPos_[logTypeChoose_] = GUILayout.BeginScrollView(scrollPos_[logTypeChoose_]);
					try {
						foreach (var s in queue) {
							GUILayout.Label(s);
						}
					} finally {
						GUILayout.EndScrollView();
					}
				}
			} finally {
				GUILayout.EndVertical();
			}
		} catch (Exception ex) {
			UnityEngine.Debug.LogException(ex);
		}
	}

	static void Enqueue(Queue<string> queue, string text, string stackTrace) {
		while (queue.Count >= MAX_LOG) {
			queue.Dequeue();
		}
		queue.Enqueue(text);
		if (!string.IsNullOrEmpty(stackTrace)) {
			queue.Enqueue(stackTrace);
		}
	}

	void LogCallback(string condition, string stackTrace, LogType type) {
		int index = (int)type;
		var queue = this.logList_[index];
		switch (type) {
		case LogType.Exception:
		case LogType.Error:
		case LogType.Warning:
			Enqueue(queue, condition, stackTrace);
			break;
		default:
			Enqueue(queue, condition, null);
			break;
		}
		this.scrollPos_[index] = new Vector2(0, 100000f);
	}


}