using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class TeamService : Singleton<TeamService>
    {
        public TeamService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<TeamLeaveRequest>(this.OnTeamLeave);

        }



        public void Init()
        {
            TeamManager.Instance.Init();
        }

        private void OnTeamInviteRequest(NetConnection<NetSession> sender, TeamInviteRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnTeamInviteRequest: : FromId:{0} :FromName{1} :ToId{2} ToName:{3}", message.FromId, message.FromName, message.ToId, message.ToName);
            //TODO: 执行一些前置的数据校验

            //开始逻辑
            NetConnection<NetSession> target = SessionManager.Instance.GetSession(message.ToId);
            if(target == null)
            {
                sender.Session.Response.teamInviteRes = new TeamInviteResponse();
                sender.Session.Response.teamInviteRes.Result = Result.Failed;
                sender.Session.Response.teamInviteRes.Errormsg = "好友不在线";
                sender.SendResponse();
                return;
            }

            if (target.Session.Character.Team != null)
            {
                sender.Session.Response.teamInviteRes = new TeamInviteResponse();
                sender.Session.Response.teamInviteRes.Result = Result.Failed;
                sender.Session.Response.teamInviteRes.Errormsg = "对方已经有队伍了";
                sender.SendResponse();
                return;
            }

            Log.InfoFormat("ForwardTeamInviteRequest: : FromId: {0} FromName: {1} ToID: {2} ToName: {3}", message.FromId, message.FromName, message.ToId, message.ToName);
            target.Session.Response.teamInviteReq = message;
            target.SendResponse();
        }


        private void OnTeamInviteResponse(NetConnection<NetSession> sender, TeamInviteResponse message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnTeamInviteResponse: :character:{0} Result:{1} FromId:{2} ToID:{3}", character.Id, message.Result, message.Request.FromId, message.Request.ToId);
            sender.Session.Response.teamInviteRes = message;
            if (message.Result == Result.Success)
            {   //接受了组队请求
                var requester = SessionManager.Instance.GetSession(message.Request.FromId);
                if (requester == null)
                {
                    sender.Session.Response.teamInviteRes.Result = Result.Failed;
                    sender.Session.Response.teamInviteRes.Errormsg = "请求者已下线";
                }
                else
                {
                    //互相加好友
                    TeamManager.Instance.AddTeamMember(requester.Session.Character, character);
                    requester.Session.Response.teamInviteRes = message;
                    requester.SendResponse();
                }
            }
            sender.SendResponse();
        }

        private void OnTeamLeave(NetConnection<NetSession> sender, TeamLeaveRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnTeamLeave: :character:{0} TeamID:{1} : {2}", character.Id, message.TeamId, message.characterId);
            sender.Session.Response.teamLeave = new TeamLeaveResponse();
            sender.Session.Response.teamLeave.Result = Result.Success;
            sender.Session.Response.teamLeave.characterId = message.characterId;
            sender.SendResponse();
        }
    }
}
