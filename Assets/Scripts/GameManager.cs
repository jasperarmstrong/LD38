using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	private static GameManager gm;

	public static Vector3 mousePos;
	public static float horizontal, vertical;

	public static bool canShoot = true;
	public static bool isPaused = true;

	public static PlayerController pc;
	public static PlanetController planet;
	public static UIManager ui;
	public static ExplosionManager em;

	private static GameObject levelUI, menuUI;
	private static Text startButtonText;

	private static int _highScore;
	public static int highScore {
		get {
			return _highScore;
		}
		set {
			_highScore = value;
			PlayerPrefs.SetInt("HighScore", _highScore);
			ui.UpdateUI("HighScore", _highScore);
		}
	}

	public static void LoadHighScore() {
		highScore = PlayerPrefs.GetInt("HighScore", 0);
	}
		
	public static void SetPlayer(PlayerController _pc) {
		pc = _pc;
	}

	public static void SetPlanet(PlanetController _pc) {
		planet = _pc;
	}

	void Awake() {
		if (gm == null) {
			DontDestroyOnLoad(this);
			gm = this;
			transform.FindChild("EventSystem").gameObject.SetActive(true);
			ui = GetComponentInChildren<UIManager>();
			em = GetComponentInChildren<ExplosionManager>();
			Time.timeScale = 0;
			levelUI = GameObject.Find("LevelUI");
			menuUI = GameObject.Find("MenuUI");
			startButtonText = GameObject.Find("StartButton").GetComponentInChildren<Text>();
			if (SceneManager.GetActiveScene().name != "Level") {
				Reset();
			}
		} else if (gm != this) {
			Destroy(gameObject);
		}
	}

	public static void Pause() {
		isPaused = true;
		Time.timeScale = 0;
		menuUI.SetActive(true);
		Camera.main.GetComponent<SmoothFollow>().enabled = false;
	}

	public static void Unpause() {
		isPaused = false;
		Time.timeScale = 1;
		menuUI.SetActive(false);
		Camera.main.GetComponent<SmoothFollow>().enabled = true;
		if (startButtonText.text.IndexOf("Resume") == -1) {
			startButtonText.text = "Resume";
		}
	}

	public static void TogglePause() {
		if (isPaused) {
			Unpause();
		} else {
			Pause();
		}
	}

	public void ButtonCloseWindow() {
		ui.UpdateUI("Window", null);
	}

	public void ButtonInstructions() {
		ui.UpdateUI("Window", "Instructions");
	}

	public void ButtonCredits() {
		ui.UpdateUI("Window", "Credits");
	}

	public void ButtonStart() {
		Unpause();
	}

	public void ButtonQuit() {
		Application.Quit();
	}

	public static void Reset() {
		SceneManager.LoadScene("Level");
		if (!isPaused) {
			Camera.main.GetComponent<SmoothFollow>().enabled = true;
		}
	}

	public static void Quit() {
		Application.Quit();
	}

	void Update () {
		mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos = new Vector3(mousePos.x, mousePos.y, 0);

		horizontal = Input.GetAxisRaw("Horizontal");
		vertical = Input.GetAxisRaw("Vertical");
		if (Input.GetButtonDown("Restart") && (pc.isDead || planet.isDead)) {
			Reset();
		}
		if (Input.GetButtonDown("Pause")) {
			TogglePause();
		}
	}
}
