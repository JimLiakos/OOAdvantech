using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace OOAdvantech.Droid
{
    /// <MetaDataID>{1e235460-29d8-4888-9357-9a4857b828e7}</MetaDataID>
    public class RingtoneService : IRingtoneService
    {
        Ringtone RingtonePlayer;

        public void Play()
        {
            if (RingtonePlayer == null)
            {
                Android.Net.Uri uri = RingtoneManager.GetDefaultUri(RingtoneType.Ringtone);
                RingtonePlayer = RingtoneManager.GetRingtone(Platform.CurrentActivity.ApplicationContext, uri);
            }
            RingtonePlayer.Play();

        }

        public void Stop()
        {
            if (RingtonePlayer != null)
                RingtonePlayer.Stop();

        }
    }
}