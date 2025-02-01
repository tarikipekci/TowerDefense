using System;
using System.Collections.Generic;
using UnityEngine;
using WaypointSystem;

namespace Managers
{
    [Serializable]
    public struct WaveInfo
    {
        public GameObject enemyPrefab; 
        public int count;
        public Waypoint waypoint;
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