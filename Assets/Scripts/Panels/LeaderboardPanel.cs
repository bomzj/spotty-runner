using Assets.Scripts.Classes.Core;
using Assets.Scripts.Classes.Utils;
using Assets.Scripts.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

namespace Assets.Scripts.Panels
{
    //TODO: refactor menu panels as separate states/GameScreens if possible
    public class LeaderboardPanel : MonoBehaviour // GameState/Screen
    {
        private GameObject scoreRowTemplate;
        private List<GameObject> rows = new List<GameObject>();
        
        private IEnumerable<IScore> loadedScores;
        private IEnumerable<IUserProfile> loadedUsers;

        void Start()
        {
            // Get first row already existed as template
            scoreRowTemplate = transform.Find("Scores/Score Row Template").gameObject;
            scoreRowTemplate.SetActive(false);

            ShowOrHideLoadingMessage(true);

            if (Social.localUser.authenticated)
            {
                Social.LoadScores(GameConsts.GeneralLeaderboardID, ScoresLoadedHandler);
                UpdateLoadingMessage("Loading scores...");
            }
            else
            {
                Social.localUser.Authenticate(UserAuthenticatedHandler);
                UpdateLoadingMessage("Authenticating user...");
            }
        }

        void OnEnable()
        {
            print("leaderboard enabled");
        }

        void OnDisable()
        {
            print("leaderboard disabled");
        }

        void UpdateLoadingMessage(string message)
        {
            var loadingMessage = this.transform.Find("Loading ...").GetComponent<UILabel>();
            loadingMessage.text = message;
        }

        void ShowOrHideLoadingMessage(bool show)
        {
            var loadingTransform = this.transform.Find("Loading ...");
            loadingTransform.gameObject.SetActive(show);
            
        }

        private void UserAuthenticatedHandler(bool authenticated)
        {
            if (authenticated)
            {
                Debug.Log("Authenticated, loading scores");
                UpdateLoadingMessage("Loading scores...");
                Social.LoadScores(GameConsts.GeneralLeaderboardID, ScoresLoadedHandler);
            }
            else
            {
                Debug.Log("Failed to authenticate");
                UpdateLoadingMessage("Failed to authenticate");
            }
        }

        private void ScoresLoadedHandler(IScore[] scores)
        {
            if (scores.Length > 0)
            {
                Debug.Log("Scores loaded");
                UpdateLoadingMessage("Loading users...");
                loadedScores = scores;
                var userIDs = scores.Select(s => s.userID).ToArray();
                Social.LoadUsers(userIDs, UsersLoadedHandler);
            }
            else
            {
                Debug.Log("Failed to load scores");
                UpdateLoadingMessage("Failed to load scores");
            }
        }

        private void UsersLoadedHandler(IUserProfile[] users)
        {
            if (users.Length > 0)
            {
                loadedUsers = users;
                ShowOrHideLoadingMessage(false);
                // Add scores to panel view
                DisplayScores();
            }
            else
            {
                Debug.Log("Failed to load users");
                UpdateLoadingMessage("Failed to load users");
            }
        }

        private void DisplayScores()
        {
            var combinedScores = loadedScores.Join(loadedUsers, 
                s => s.userID, 
                u => u.id, 
                (s, u) => new { Score = s, User = u});

            foreach (var item in combinedScores)
            {
                AddScoreRow(item.Score, item.User);
            }

            ArrangeRowPositions();
        }

        private void AddScoreRow(IScore score, IUserProfile user)
        {
            // Clone row from template
            var row = Instantiate(scoreRowTemplate) as GameObject;
            row.transform.parent = transform.Find("Scores");
            var position = scoreRowTemplate.transform.localPosition;
            var scale = scoreRowTemplate.transform.localScale;
            row.transform.localPosition = position;
            row.transform.localScale = scale;
            row.SetActive(true);

            var rankAndUserNameLabel = row.transform.Find("1 - Rank and UserName").GetComponent<UILabel>();
            rankAndUserNameLabel.text = string.Format("{0}. {1}", score.rank, user.userName);
            
            var scoreLabel = row.transform.Find("2 - Score").GetComponent<UILabel>();
            scoreLabel.text = score.value.ToString();

            rows.Add(row);
        }

        private void AddSeparatorRow()
        {
            // Clone row from template
            var row = Instantiate(scoreRowTemplate) as GameObject;
            row.transform.parent = transform.Find("Scores");
            row.SetActive(true);

            var rankAndUserNameLabel = row.transform.Find("1 - Rank and UserName").GetComponent<UILabel>();
            rankAndUserNameLabel.text = string.Format("...");

            rows.Add(row);
        }

        private void ArrangeRowPositions()
        {
            for (int i=0; i < rows.Count; i++)
            {
                var row = rows[i];
                row.transform.localPosition = row.transform.localPosition + new Vector3(0, -30 * i, 0);
            }
        }

        private void ClearAllScoreRows()
        {
            foreach (var item in rows)
            {
                Destroy(item);
            }
            rows.Clear();
        }

        
    }
}
