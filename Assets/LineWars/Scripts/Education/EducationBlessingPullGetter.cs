﻿using System;
using System.Linq;
using AYellowpaper.SerializedCollections;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Education
{
    public class EducationBlessingPullGetter : BlessingPullGetter
    {
        [SerializeField] private int totalCount;
        [SerializeField] private SerializedDictionary<BaseBlessing, int> blessingToCount;

        private EducationBlessingPull educationBlessingPull;

        private void Awake()
        {
            educationBlessingPull = new EducationBlessingPull(
                blessingToCount
                    .ToDictionary(
                        pair => pair.Key.BlessingId,
                        pair => pair.Value),
                totalCount
            );
        }

        public override bool CanGet()
        {
            return true;
        }

        public override IBlessingsPull Get()
        {
            return educationBlessingPull;
        }
    }
}