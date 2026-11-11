using System;
using UnityEngine;
using BrewAndDream.Core.Enums;

namespace BrewAndDream.Audio
{
    /// <summary>
    /// Bộ điều khiển âm thanh theo nhịp ca và hiệu ứng âm thanh pha chế (SFX / Ambient / BGM).
    /// - Sáng: Tiếng phố thị thức giấc, xe cộ xa xa, lofi nhẹ nhàng.
    /// - Trưa: Tiếng ồn ào đông đúc, tiếng máy xay dồn dập, order liên tục.
    /// - Tối: Nhạc Acoustic mộc mạc, tiếng mưa rơi, không gian lãng mạn.
    /// </summary>
    public class CoffeeShopAudioManager : MonoBehaviour
    {
        public static CoffeeShopAudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource ambientSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("BGM từng ca")]
        [SerializeField] private AudioClip morningChillBgm;
        [SerializeField] private AudioClip noonRushBgm;
        [SerializeField] private AudioClip eveningAcousticBgm;

        [Header("SFX Tương tác pha chế")]
        [SerializeField] private AudioClip pourLiquidSfx;
        [SerializeField] private AudioClip espressoMachineSfx;
        [SerializeField] private AudioClip iceDropSfx;
        [SerializeField] private AudioClip posRegisterTingSfx;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SwitchShiftAudio(ShiftType shift)
        {
            if (bgmSource == null) return;

            AudioClip nextClip = shift switch
            {
                ShiftType.Morning => morningChillBgm,
                ShiftType.NoonRush => noonRushBgm,
                ShiftType.AfternoonEvening => eveningAcousticBgm,
                _ => morningChillBgm
            };

            if (nextClip != null && bgmSource.clip != nextClip)
            {
                bgmSource.clip = nextClip;
                bgmSource.loop = true;
                bgmSource.Play();
            }
        }

        public void PlayPourSound() => PlayOneShot(pourLiquidSfx);
        public void PlayEspressoMachineSound() => PlayOneShot(espressoMachineSfx);
        public void PlayIceDropSound() => PlayOneShot(iceDropSfx);
        public void PlayPOSTingSound() => PlayOneShot(posRegisterTingSfx);

        private void PlayOneShot(AudioClip clip)
        {
            if (sfxSource != null && clip != null)
            {
                sfxSource.PlayOneShot(clip);
            }
        }
    }
}

