using UnityEngine;
using Util.Audio;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private float _fadeInTime;
        [SerializeField] private float _fadeOutTime;
        [SerializeField] private float _backgroundVolume = 1f;
        [SerializeField] private float _effectVolume = 1f;
        [SerializeField] private float _musicVolume = 1f;

        [Header("Audio Sources")]
        [SerializeField] private AudioSource backgroundAudio;
        [SerializeField] private AudioSource mainMenuBackgroundAudio;
        [SerializeField] private AudioSource removeRockSound;
        [SerializeField] private AudioSource goldFoundSound;
        [SerializeField] private AudioSource itemFoundSound;
        [SerializeField] private AudioSource arrowAttackSound;
        [SerializeField] private AudioSource inspectionSound;
        [SerializeField] private AudioSource errorSound;
        [SerializeField] private AudioSource healSound;
        [SerializeField] private AudioSource swingSound;
        [SerializeField] private AudioSource playerHitSound;
        [SerializeField] private AudioSource enemyHitSound;
        [SerializeField] private AudioSource nextLineDialogueSound;
        [SerializeField] private AudioSource openShopSound;
        [SerializeField] private AudioSource purchaseSound;
        [SerializeField] private AudioSource toggleTorchSound;
        [SerializeField] private AudioSource descendSound;
        [SerializeField] private AudioSource switchItemSound;

        private static SoundManager _instance;
        private static SoundManager Instance => _instance ??= FindObjectOfType<SoundManager>();

        void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
        }

        public static void SetBackgroundVolume(float volume) => Instance._SetBackgroundVolume(volume);
        private void _SetBackgroundVolume(float volume)
        {
            _backgroundVolume = volume;

            backgroundAudio.volume = volume;
        }

        public static void SetEffectVolume(float volume) => Instance._SetEffectVolume(volume);
        private void _SetEffectVolume(float volume)
        {
            _effectVolume = volume;

            removeRockSound.volume = volume;
            goldFoundSound.volume = volume;
            itemFoundSound.volume = volume;
            arrowAttackSound.volume = volume;
            inspectionSound.volume = volume;
            nextLineDialogueSound.volume = volume;
            openShopSound.volume = volume;
            purchaseSound.volume = volume;
            toggleTorchSound.volume = volume;
            descendSound.volume = volume;
            switchItemSound.volume = volume;
            if(errorSound) errorSound.volume = volume;
            if(healSound) healSound.volume = volume;
            if(swingSound) swingSound.volume = volume;
            if(playerHitSound) playerHitSound.volume = volume;
            if(enemyHitSound) enemyHitSound.volume = volume;
        }

        public static void SetMusicVolume(float volume) => Instance._SetMusicVolume(volume);
        private void _SetMusicVolume(float volume)
        {
            _musicVolume = volume;

            // TODO: If music is implemented, adjust the volume here
            mainMenuBackgroundAudio.volume = volume;
        }

        public static void StartBackgroundAudio() => Instance._StartBackgroundAudio();
        private void _StartBackgroundAudio()
        {
            mainMenuBackgroundAudio.Stop();
            backgroundAudio.Play();
        }
        public static void StopBackgroundAudio() => Instance._StopBackgroundAudio();
        private void _StopBackgroundAudio() => backgroundAudio.Stop();
        public static void PauseBackgroundAudio() => Instance._PauseBackgroundAudio();
        private void _PauseBackgroundAudio() => StartCoroutine(AudioHelper.FadeOut(backgroundAudio, _fadeOutTime, backgroundAudio.Pause));
        public static void ResumeBackgroundAudio() => Instance._ResumeBackgroundAudio();
        private void _ResumeBackgroundAudio()
        {
            if (backgroundAudio.isPlaying) return;
            StartCoroutine(AudioHelper.FadeIn(backgroundAudio, Instance._fadeInTime, _backgroundVolume, backgroundAudio.UnPause));
        }

        public static void StartMainMenuBackgroundAudio() => Instance._StartMainMenuBackgroundAudio();
        private void _StartMainMenuBackgroundAudio()
        {
            _StopBackgroundAudio();
            mainMenuBackgroundAudio.Play();
        }
        public static void StopMainMenuBackgroundAudio() => Instance._StopMainMenuBackgroundAudio();
        private void _StopMainMenuBackgroundAudio() => mainMenuBackgroundAudio.Stop();

        public static void RemoveRock() => Instance._RemoveRock();
        private void _RemoveRock() => removeRockSound.Play();

        public static void GoldFound() => Instance._GoldFound();
        private void _GoldFound() => goldFoundSound.Play();

        public static void ItemFound() => Instance._ItemFound();
        private void _ItemFound() => itemFoundSound.Play();

        public static void Inspection() => Instance._Inspection();
        private void _Inspection() => inspectionSound.Play();

        public static void ArrowAttack() => Instance._ArrowAttack();
        private void _ArrowAttack() => arrowAttackSound.Play();

        public static void NextLineDialogue() => Instance._NextLineDialogue();
        private void _NextLineDialogue() => nextLineDialogueSound.Play();

        public static void OpenShop() => Instance._OpenShop();
        private void _OpenShop() => openShopSound.Play();

        public static void Purchase() => Instance._Purchase();
        private void _Purchase() => purchaseSound.Play();

        public static void ToggleTorch() => Instance._ToggleTorch();
        private void _ToggleTorch() => toggleTorchSound.Play();

        public static void Descend() => Instance._Descend();
        private void _Descend() => descendSound.Play();

        public static void SwitchItem() => Instance._SwitchItem();
        private void _SwitchItem() => switchItemSound.Play();

        public static void Error() => Instance._Error();
        public void _Error()
        {
            //TODO Add error sound effect
            if (errorSound) errorSound.Play();
        }

        public static void Heal() => Instance._Heal();
        private void _Heal()
        {
            //TODO add heal sound effect
            if (healSound) healSound.Play();
        }
        
        public static void Swing() => Instance._Swing();
        private void _Swing()
        {
            if (swingSound) swingSound.Play();
        }
        
        public static void PlayerHit() => Instance._PlayerHit();
        private void _PlayerHit()
        {
            // TODO add player hit swing sound effect
            if (playerHitSound) playerHitSound.Play();
        }
        
        public static void EnemyHit() => Instance._EnemyHit();
        private void _EnemyHit()
        {
            // TODO add enemy hit sound effect
            if (enemyHitSound) enemyHitSound.Play();
        }
    }
}
