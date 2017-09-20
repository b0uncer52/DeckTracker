using System.Collections.Generic;
using DeckTrackerCLI.Models;
using Microsoft.Data.Sqlite;

namespace DeckTrackerCLI.Managers
{
    public class TeamManager 
    {
        private DatabaseInterface _db;
        private List<Team> _teams;
        
        public TeamManager(DatabaseInterface db)
        {
            _db = db;
        }

        // Accept instance of team and the creators userId, returns new team id
        public int CreateTeam(Team team, int userId) 
        {
            team.TeamId = _db.Insert($"INSERT INTO team VALUES (null, '{team.Name}', {userId}, null");
            _db.Insert($"INSERT INTO teammember VALUES (null, {userId}, {team.TeamId}, null, 1");
            _teams.Add(team);
            return team.TeamId;
        }

        // accept a userId and teamId and delete the matching teammember instance
        public void LeaveTeam(int userId, int teamId) //need current user and teamid
        {
            _db.Delete($"DELETE FROM teammember WHERE userid == {userId} AND teamid == {teamId}");
            _teams.Remove(_teams.Find(t => t.TeamId == teamId));
        }

        //  accept a userid and team id and create a teammember in the database where accepted will default to false
        public int InviteToTeam(int invitee, int teamId)
        {
            int teamMemberId = _db.Insert($"INSERT INTO teammember VALUES (null, {invitee}, {teamId}, null, 0");
            return teamMemberId;
        }

        // accept a teammember and change the 'accepted' value to true
        public void JoinTeam(int teamMemberId)
        {
            _db.Update($"UPDATE teammember SET accepted = 1 WHERE teammemberid = {teamMemberId}");
        }

        // accept userid and return a list of teams that user has teammember items for
        public List<Team> ListUserTeams(int userId)
        {
            _teams.Clear();
            _db.Query($"SELECT * FROM team t LEFT JOIN teammember tm ON tm.teamid == t.teamid WHERE tm.userid = {userId}", (SqliteDataReader reader) =>{
                while(reader.Read())
                {
                    _teams.Add(new Team(){
                        TeamId = reader.GetInt32(0),
                        Name =  reader[1].ToString(),
                        CreatorId = reader.GetInt32(2),
                        DateCreated = reader.GetDateTime(3)
                    });
                }
            });
            return _teams;
        }
    }
}