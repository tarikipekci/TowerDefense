using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace Managers
{
    [Serializable]
    public struct WaveInfo
    {
        public GameObject enemyPrefab; 
        public int count;
        public SplineContainer splineContainer;
    }

    [Serializable]
    public struct Wave
    {
        public List<WaveInfo> waveInfo; 
    }

    public class WaveManager : Singleton<WaveManager>
    {
        public List<Wave> waves;
    }
}