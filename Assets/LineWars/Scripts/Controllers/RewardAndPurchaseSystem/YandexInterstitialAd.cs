﻿using System;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

namespace LineWars.Controllers
{
    public class YandexInterstitialAd
    {
        private InterstitialAdLoader interstitialAdLoader;
        private Interstitial interstitial;
        private string adUnitId = "demo-interstitial-yandex";

        public void Initialize(string adUnitId)
        {
            this.adUnitId = adUnitId;
            SetupLoader();
            RequestInterstitial();
        }

        private void SetupLoader()
        {
            interstitialAdLoader = new InterstitialAdLoader();
            interstitialAdLoader.OnAdLoaded += HandleInterstitialLoaded;
            interstitialAdLoader.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
        }

        private void RequestInterstitial()
        {
             // замените на "R-M-XXXXXX-Y"
            AdRequestConfiguration adRequestConfiguration = new AdRequestConfiguration.Builder(adUnitId).Build();
            interstitialAdLoader.LoadAd(adRequestConfiguration);
        }

        public void ShowInterstitial()
        {
            if (interstitial != null)
            {
                interstitial.Show();
            }
        }

        public void HandleInterstitialLoaded(object sender, InterstitialAdLoadedEventArgs args)
        {
            // The ad was loaded successfully. Now you can handle it.
            interstitial = args.Interstitial;

            // Add events handlers for ad actions
            interstitial.OnAdClicked += HandleAdClicked;
            interstitial.OnAdShown += HandleInterstitialShown;
            interstitial.OnAdFailedToShow += HandleInterstitialFailedToShow;
            interstitial.OnAdImpression += HandleImpression;
            interstitial.OnAdDismissed += HandleInterstitialDismissed;
        }

        public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            // Ad {args.AdUnitId} failed for to load with {args.Message}
            // Attempting to load a new ad from the OnAdFailedToLoad event is strongly discouraged.
        }

        public void HandleInterstitialDismissed(object sender, EventArgs args)
        {
            // Called when ad is dismissed.

            // Clear resources after Ad dismissed.
            DestroyInterstitial();

            // Now you can preload the next interstitial ad.
            RequestInterstitial();
        }

        public void HandleInterstitialFailedToShow(object sender, EventArgs args)
        {
            // Called when an InterstitialAd failed to show.

            // Clear resources after Ad dismissed.
            DestroyInterstitial();

            // Now you can preload the next interstitial ad.
            RequestInterstitial();
        }

        public void HandleAdClicked(object sender, EventArgs args)
        {
            // Called when a click is recorded for an ad.
        }

        public void HandleInterstitialShown(object sender, EventArgs args)
        {
            // Called when ad is shown.
        }

        public void HandleImpression(object sender, ImpressionData impressionData)
        {
            // Called when an impression is recorded for an ad.
        }

        public void DestroyInterstitial()
        {
            if (interstitial != null)
            {
                interstitial.Destroy();
                interstitial = null;
            }
        }
    }
}

