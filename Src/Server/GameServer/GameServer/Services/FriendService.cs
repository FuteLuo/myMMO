using Common;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using GameServer.Entities;
using GameServer.Managers;

namespace GameServer.Services
{
    class FriendService : Singleton<FriendService>
    {

        public FriendService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendRemoveRequest>(this.OnFriendRemove);
        }


        public void Init()
        {

        }

        /// <summary>
        /// 收到好友请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnFriendAddRequest(NetConnection<NetSession> sender, FriendAddRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendAddRequest: FromId:{0} :FromName{1} :ToId{2} ToName:{3}", message.FromId, message.FromName, message.ToId, message.ToName);
            
            if(message.ToId == 0)
            {
                foreach (var cha in CharacterManager.Instance.Characters)
                {
                    if(cha.Value.Data.Name == message.ToName)
                    {
                        message.ToId = cha.Key;
                        break;
                    }
                }
            }
            NetConnection<NetSession> friend = null;
            if(message.ToId > 0)
            {
                if(character.FriendManager.GetFriendInfo(message.ToId) != null)
                {
                    sender.Session.Response.friendAddRes = new FriendAddResponse();
                    sender.Session.Response.friendAddRes.Result = Result.Failed;
                    sender.Session.Response.friendAddRes.Errormsg = "已经是好友了";
                    sender.SendResponse();
                    return;
                }
                friend = SessionManager.Instance.GetSession(message.ToId);
            }
            if(friend == null)
            {
                sender.Session.Response.friendAddRes = new FriendAddResponse();
                sender.Session.Response.friendAddRes.Result = Result.Failed;
                sender.Session.Response.friendAddRes.Errormsg = "好友不存在或者不在线";
                sender.SendResponse();
                return;
            }

            Log.InfoFormat("ForwardRequest: : FromId: {0} FromName: {1} ToID: {2} ToName: {3}", message.FromId, message.FromName, message.ToId, message.ToName);
            friend.Session.Response.friendAddReq = message;
            friend.SendResponse();
        }

        /// <summary>
        /// 收到加好友响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnFriendAddResponse(NetConnection<NetSession> sender, FriendAddResponse message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendAddResponse: :character:{0} Result:{1} FromId:{2} ToID:{3}", character.Id, message.Result, message.Request.FromId, message.Request.ToId);
            sender.Session.Response.friendAddRes = message;
            if(message.Result == Result.Success)
            {   //接受了好友请求
                var requester = SessionManager.Instance.GetSession(message.Request.FromId);
                if(requester == null)
                {
                    sender.Session.Response.friendAddRes.Result = Result.Failed;
                    sender.Session.Response.friendAddRes.Errormsg = "请求者已下线";
                }
                else
                {
                    //互相加好友
                    character.FriendManager.AddFriend(requester.Session.Character);
                    requester.Session.Character.FriendManager.AddFriend(character);
                    DBService.Instance.Save();
                    requester.Session.Response.friendAddRes = message;
                    requester.Session.Response.friendAddRes.Result = Result.Success;
                    requester.Session.Response.friendAddRes.Errormsg = "添加好友成功";
                    requester.SendResponse();
                }
            }
            sender.SendResponse();
        }

        private void OnFriendRemove(NetConnection<NetSession> sender, FriendRemoveRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendRemove: :character:{0} FriendRelationID:{1}", character.Id, message.Id);
            sender.Session.Response.friendRemove = new FriendRemoveResponse();
            sender.Session.Response.friendRemove.Id = message.Id;

            if (character.FriendManager.RemoveFriendById(message.Id))
            {
                sender.Session.Response.friendRemove.Result = Result.Success;

                var friend = SessionManager.Instance.GetSession(message.friendId);
                if(friend != null)
                {
                    friend.Session.Character.FriendManager.RemoveFriendByFriendId(character.Id);
                }
                else
                {
                    this.RemoveFriend(message.friendId, character.Id);
                }
            }
            else
            {
                sender.Session.Response.friendRemove.Result = Result.Failed;
            }

            DBService.Instance.Save();

            sender.SendResponse();
        }

        void RemoveFriend(int charId, int friendId)
        {
            var removeItem = DBService.Instance.Entities.CharacterFriends.FirstOrDefault(v => v.TCharacterID == charId && v.FriendID == friendId);
            if(removeItem != null)
            {
                DBService.Instance.Entities.CharacterFriends.Remove(removeItem);
            }
        }


    }
}
