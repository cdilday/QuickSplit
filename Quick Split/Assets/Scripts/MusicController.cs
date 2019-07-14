using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is attatched to the Music controller and handles the current song and volume that is being played
/// </summary>
public class MusicController : MonoBehaviour
{ 
    public float musicVolume;
    public float SFXVolume;

    public AudioClip WizMusic;
    public AudioClip QuickMusic;
    public AudioClip WitMusic;
    public AudioClip HolyMusic;
    public AudioClip MenuMusic;
    public AudioClip GameOverMusic;

    // 0 is the slow tick, 1 is the fast tick
    public AudioClip[] WizTicks;
    public AudioClip[] QuickTicks;
    public AudioClip[] WitTicks;
    public AudioClip[] HolyTicks;

    /* Song titles -> modes:
	 * Cog Capers = Quick
	 * Resistance March = Holy
	 * Satellite = Wit
	 * The Flying Machine = Wiz
	 * */

    public AudioSource MusicSource;
    public AudioSource SlowTickSource;
    public AudioSource FastTickSource;
    private bool isSlowTicking = false;
    private bool isFastTicking = false;

    public bool IsFastTicking
    {
        get
        {
            return isFastTicking;
        }
    }

    private void Awake()
    {
        //get rid of redundant music controllers, there should always be one but only one
        GameObject[] mcs = GameObject.FindGameObjectsWithTag("Music Controller");
        if (mcs.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(transform.gameObject);

        MusicSource = gameObject.GetComponent<AudioSource>();
        MusicSource.loop = true;

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            MusicSource.clip = MenuMusic;
        }
        else
        {
            //we're in the game scene, need to look up game type
            switch (Game_Mode_Helper.ActiveRuleSet.Mode)
            {
                case GameMode.Wit:
                    MusicSource.clip = WitMusic;
                    SlowTickSource.clip = WitTicks[0];
                    FastTickSource.clip = WitTicks[1];
                    break;
                case GameMode.Quick:
                    MusicSource.clip = QuickMusic;
                    SlowTickSource.clip = QuickTicks[0];
                    FastTickSource.clip = QuickTicks[1];
                    break;
                case GameMode.Wiz:
                    MusicSource.clip = WizMusic;
                    SlowTickSource.clip = WizTicks[0];
                    FastTickSource.clip = WizTicks[1];
                    break;
                case GameMode.Holy:
                    MusicSource.clip = HolyMusic;
                    SlowTickSource.clip = HolyTicks[0];
                    FastTickSource.clip = HolyTicks[1];
                    break;
                case GameMode.Custom:
                    MusicSource.clip = QuickMusic;
                    SlowTickSource.clip = QuickTicks[0];
                    FastTickSource.clip = QuickTicks[1];
                    break;
                default:
                    break;
            }
        }

        MusicSource.volume = PlayerPrefs.GetFloat(Constants.MusicVolumeLookup, 1);
        musicVolume = MusicSource.volume;
        SlowTickSource.volume = musicVolume;
        FastTickSource.volume = musicVolume;
        SFXVolume = PlayerPrefs.GetFloat(Constants.SfxVolumeLookup, 1);

    }

    private void Update()
    {
        if (MusicSource.clip == GameOverMusic)
        {
            if (SlowTickSource.isPlaying)
            {
                SlowTickSource.Stop();
            }

            if (FastTickSource.isPlaying)
            {
                FastTickSource.Stop();
            }
        }
    }

    public void StopMusic()
    {
        MusicSource.Stop();
    }

    public void PauseMusic()
    {
        MusicSource.Pause();
        if (isSlowTicking)
        {
            SlowTickSource.Pause();
        }
        if (isFastTicking)
        {
            FastTickSource.Pause();
        }
    }

    /// <summary>
    /// Unpauses the music
    /// </summary>
    public void ResumeMusic()
    {
        MusicSource.UnPause();
        if (isSlowTicking)
        {
            SlowTickSource.UnPause();
        }
        if (isFastTicking)
        {
            FastTickSource.UnPause();
        }
    }

    /// <summary>
    /// Plays music for the given string
    /// </summary>
    public void PlayMusic(string gameType)
    {
        switch (gameType)
        {
            case "Wit":
                MusicSource.clip = WitMusic;
                SlowTickSource.clip = WitTicks[0];
                FastTickSource.clip = WitTicks[1];
                break;
            case "Quick":
                MusicSource.clip = QuickMusic;
                SlowTickSource.clip = QuickTicks[0];
                FastTickSource.clip = QuickTicks[1];
                break;
            case "Wiz":
                MusicSource.clip = WizMusic;
                SlowTickSource.clip = WizTicks[0];
                FastTickSource.clip = WizTicks[1];
                break;
            case "Holy":
                MusicSource.clip = HolyMusic;
                SlowTickSource.clip = HolyTicks[0];
                FastTickSource.clip = HolyTicks[1];
                break;
            case "Menu":
                MusicSource.clip = MenuMusic;
                SlowTickSource.Stop();
                FastTickSource.Stop();
                break;
            case "Gameover":
                MusicSource.clip = GameOverMusic;
                SlowTickSource.Stop();
                FastTickSource.Stop();
                break;
            default:
                break;
        }

        MusicSource.Play();

    }

    /// <summary>
    /// Plays the music for the given Game Mode
    /// </summary>
    public void PlayMusic(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.Wit:
                MusicSource.clip = WitMusic;
                SlowTickSource.clip = WitTicks[0];
                FastTickSource.clip = WitTicks[1];
                break;
            case GameMode.Quick:
                MusicSource.clip = QuickMusic;
                SlowTickSource.clip = QuickTicks[0];
                FastTickSource.clip = QuickTicks[1];
                break;
            case GameMode.Wiz:
                MusicSource.clip = WizMusic;
                SlowTickSource.clip = WizTicks[0];
                FastTickSource.clip = WizTicks[1];
                break;
            case GameMode.Holy:
                MusicSource.clip = HolyMusic;
                SlowTickSource.clip = HolyTicks[0];
                FastTickSource.clip = HolyTicks[1];
                break;
            case GameMode.Custom:
                MusicSource.clip = QuickMusic;
                SlowTickSource.clip = QuickTicks[0];
                FastTickSource.clip = QuickTicks[1];
                break;
            default:
                break;
        }

        MusicSource.Play();

    }

    public void ChangeMusicVolume(float value)
    {
        musicVolume = value;
        MusicSource.volume = musicVolume;
        SlowTickSource.volume = musicVolume;
        FastTickSource.volume = musicVolume;
    }

    /// <summary>
    /// Begins the slow tick music (plays when a piece it one space away from ending the game
    /// </summary>
    public void StartSlowTick()
    {
        SlowTickSource.Play();
        SlowTickSource.timeSamples = MusicSource.timeSamples;
        isSlowTicking = true;
    }

    /// <summary>
    /// Ends the slow tick music
    /// </summary>
    public void StopSlowTick()
    {
        SlowTickSource.Stop();
        isSlowTicking = false;
    }

    /// <summary>
    /// Starts the fast tick music (plays when the side bars are close to entering
    /// </summary>
    public void StartFastTick()
    {
        FastTickSource.Play();
        FastTickSource.timeSamples = MusicSource.timeSamples;
        isFastTicking = true;
    }

    /// <summary>
    /// Stops the fast ticking music
    /// </summary>
    public void StopFastTick()
    {
        FastTickSource.Stop();
        isFastTicking = false;
    }

}