using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.SerializedObjects;
using Photon.MmoDemo.Common;
using PubSub;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace MGFClient
{
    [Serializable]
    public class GameData : MonoBehaviour
    {
        public static GameData Instance = null;

        [SerializeField] public Character selectedCharacter;

        [SerializeField] public List<Character> characters;

        [SerializeField] public Region region;

        [SerializeField] public PlayerChannel channel;

        public List<Player> players;
        public List<NpcCharacter> npcCharacters;
        public List<Entity> entities;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                //Destroy object duplicate
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            players = new List<Player>();
            npcCharacters = new List<NpcCharacter>();
            entities = new List<Entity>();
        }


        void FixedUpdate()
        {
            foreach (var npcCharacter in npcCharacters)
            {
                SpawnNpc(npcCharacter);
            }
        }

        private void SpawnNpc(NpcCharacter npcCharacter)
        {
            if (entities.FirstOrDefault(x => x.startPosition.Equals(npcCharacter.StartPosition)) != null)
                return;

            Vector3 npcPosition =
                new Vector3(npcCharacter.Position.X, npcCharacter.Position.Y, npcCharacter.Position.Z);
            npcPosition.y = Terrain.activeTerrain.SampleHeight(npcPosition);

            var characterPrefab = Resources.Load(npcCharacter.NpcTemplate.Prefab) as GameObject;
            var obj = Instantiate(characterPrefab, npcPosition, Quaternion.identity);
            if (obj != null)
            {
                var entity = obj.AddComponent<Entity>();
                
                entity.Position = npcPosition;
                entity.startPosition = npcCharacter.StartPosition;
                entity.EntityName = npcCharacter.NpcTemplate.Name;
                
                entities.Add(entity);
            }
        }
    }
}