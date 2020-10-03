using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, Fadeable
{

    private PlayerInputActions controls;
    private Vector2 movement;
    private bool selectTriggered;
    private AudioSource menuAudio;
    public Animator playQuitBoxAnimator;
    public FadeOutManager fadeOutManager;
    public AudioClip _changeSelection;
    public AudioClip _select;

    void Awake() {
        controls = new PlayerInputActions();
        
        controls.Player.Move.started += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => movement = ctx.ReadValue<Vector2>();

        controls.Player.Interact.started += ctx => selectTriggered = true;

        menuAudio = GetComponent<AudioSource>();
    }

    void Update() {
        Vector2 moveInput = new Vector2(movement.x, movement.y).normalized;

        if (Mathf.Abs(moveInput.y) > 0.8f) {
            bool selection = moveInput.y > 0.8f;

            Debug.Log(selection);
            if (playQuitBoxAnimator.GetBool("IsPlay") != selection) {
                // Play sound for toggle
                menuAudio.clip = _changeSelection;
                menuAudio.Play();
                playQuitBoxAnimator.SetBool("IsPlay", selection);
            }
        }

        if (selectTriggered) {
            bool isPlay = playQuitBoxAnimator.GetBool("IsPlay");

            

            if (isPlay) {
                OnDisable();

                menuAudio.clip = _select;
                menuAudio.Play();    

                fadeOutManager.FadeOut(this);
            } else {
                Debug.Log("Quitting...");
                
                Application.Quit();
            }
        }

        selectTriggered = false;
    }

    public void RunPostFade() {
        StartCoroutine(LoadPlay());
    }

    private IEnumerator LoadPlay() {
        GameObject menuMusic = GameObject.Find("MenuMusic");
        if (menuMusic) {
            AudioSource music = menuMusic.GetComponent<AudioSource>();
            music.loop = false;
            while (music.isPlaying) {
                yield return null;
            }
        }

        SceneManager.LoadScene("CG_PrototypePuzzles");
    }

    private void OnEnable() {
        controls.Player.Move.Enable();
        controls.Player.Interact.Enable();
    }

    private void OnDisable() {
        controls.Player.Move.Disable();
        controls.Player.Interact.Disable();
    }
}
