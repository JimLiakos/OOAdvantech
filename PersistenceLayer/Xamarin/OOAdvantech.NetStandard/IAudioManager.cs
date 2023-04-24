using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OOAdvantech.AudioManager
{
    /// <MetaDataID>{979f4bf2-2513-4a26-a91f-afea80299b2b}</MetaDataID>
    public interface IAudioManager
    {
        #region Computed Properties

        float BackgroundMusicVolume { get; set; }

        bool MusicOn { get; set; }

        bool EffectsOn { get; set; }

        float EffectsVolume { get; set; }

        string SoundPath { get; set; }

        #endregion


        #region Public Methods

        void ActivateAudioSession();

        void DeactivateAudioSession();

        void ReactivateAudioSession();

        Task<bool> PlayBackgroundMusic(string filename);

        void StopBackgroundMusic();

        void SuspendBackgroundMusic();

        Task<bool> RestartBackgroundMusic();

        Task<bool> PlaySound(string filename);

        #endregion
    }

    /// <MetaDataID>{d64a47ab-b5aa-4c6e-b439-7d8d46b73d30}</MetaDataID>
    public static class Audio
    {
        public static IAudioManager Manager { get; } = DependencyService.Get<IAudioManager>();


    }
}
