using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;
using UnityEngine;
using Models;
using Common.Data;
using Managers;

namespace Services
{
    class MapService : Singleton<MapService>, IDisposable
    {
        public MapService()
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Subscribe<MapEntitySyncResponse>(this.OnMapEntitySync);
        }


        public int CurrentMapId = 0;

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            
        }

        public void Init()
        {

        }

        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse message)
        {
            Debug.LogFormat("OnMapCharacterEnter:{0} Count:{1}", message.mapId, message.Characters.Count);
            foreach(var cha in message.Characters)
            {
                if(User.Instance.CurrentCharacter == null || User.Instance.CurrentCharacter.Id == cha.Id)
                {
                    User.Instance.CurrentCharacter = cha;
                }
                CharacterManager.Instance.AddCharacter(cha);
            }
            if(CurrentMapId != message.mapId)
            {
                this.EnterMap(message.mapId);
                this.CurrentMapId = message.mapId;
            }
            
        }

        private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse message)
        {
            Debug.LogFormat("OnMapCharacterLeave:CharID:{0}", message.characterId);
            if(message.characterId != User.Instance.CurrentCharacter.Id)
            {
                CharacterManager.Instance.RemoveCharacter(message.characterId);
            }
            else
            {
                CharacterManager.Instance.Clear();
            }
        }


        private void EnterMap(int mapId)
        {
            if(DataManager.Instance.Maps.ContainsKey(mapId))
            {
                MapDefine map = DataManager.Instance.Maps[mapId];
                User.Instance.CurrentMapData = map;
                SceneManager.Instance.LoadScene(map.Resource);
            }
            else
            {
                Debug.LogErrorFormat("EnterMap: Map {0} not existed", mapId);
            }
        }

        public void SendEntitySync(EntityEvent entityEvent, NEntity entity)
        {
            Debug.LogFormat("MapEntityUpdateRequest : ID:{0} POS:{1} DIR:{2} SPD:{3}", entity.Id, entity.Position, entity.Direction, entity.Speed);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapEntitySync = new MapEntitySyncRequest();
            message.Request.mapEntitySync.entitySync = new NEntitySync()
            {
                Id = entity.Id,
                Event = entityEvent,
                Entity = entity
            };
            NetClient.Instance.SendMessage(message);
        }

        private void OnMapEntitySync(object sender, MapEntitySyncResponse message)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("MapEntityUpdateResponse: Entitys:{0}", message.entitySyncs.Count);
            sb.AppendLine();
            foreach(var entity in message.entitySyncs)
            {
                EntityManager.Instance.OnEntitySync(entity);
                sb.AppendFormat(" [{0} evt:{1} entity:{2}", entity.Id, entity.Event, entity.Entity.String());
                sb.AppendLine();
            }
            Debug.Log(sb.ToString());
        }

        internal void SendMapTeleport(int teleporterID)
        {
            Debug.LogFormat("MapTeleportRequest :teleporterID:{0}", teleporterID);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapTeleport = new MapTeleportRequest();
            message.Request.mapTeleport.teleporterId = teleporterID;
            NetClient.Instance.SendMessage(message);
        }

    }
}
