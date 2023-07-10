﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(VivaWalletPos.Android.Pos))]
namespace VivaWalletPos.Android
{


    public class Pos : IPos
    {

        static VivaWalletAppPos VivaWalletAppPos= new VivaWalletAppPos();
        public void Confing(POSType terminalType, string ipAddress = "", int port = 0)
        {
            if (terminalType==POSType.AppPOS)
                VivaWalletAppPos=new VivaWalletAppPos();
            else
            {

            }
           
            
        }

        public Task<PaymentData> Sale(decimal total, decimal tips)
        {
            if(VivaWalletAppPos!=null)
                return VivaWalletAppPos.Sale(total, tips);

            throw new NotImplementedException();
        }
    }
}