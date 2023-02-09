using System.Collections.Generic;
using UnityEngine;
using LWVNFramework.Infos;
using UnityEngine.UIElements;

namespace LWVNFramework.Controllers
{
    /// <summary>
    /// VNCommandCente的扩展方法，用于提高易用性
    /// </summary>
    public static class VNCommandCenterExtensions
    {
        public static void ShowDialog(this VNCommandCenter self, List<VNCharacterInfo> characterInfos, VNDialogueInfo dialogInfo)
        {
            VNSceneInfo info = VNSceneInfo.CreateDefault();
            info.CharacterInfos = characterInfos;
            info.DialogInfo = dialogInfo;
            self.SceneController.LoadVNSceneInfo(info);
        }
        public static void ShowDialog(this VNCommandCenter self, string dialogText)
        {
            self.ShowDialog("", dialogText, null, self.SceneController.CenterLayerController.DefaultCharacterTag);
        }
        public static void ShowDialog(this VNCommandCenter self, string roleName, string dialogText)
        {
            self.ShowDialog(roleName, dialogText, null, self.SceneController.CenterLayerController.DefaultCharacterTag);
        }
        public static void ShowDialog(this VNCommandCenter self, string roleName, string dialogText, string roleImage)
        {
            self.ShowDialog(roleName, dialogText, roleImage, self.SceneController.CenterLayerController.DefaultCharacterTag);
        }
        public static void ShowDialog(this VNCommandCenter self, string roleName, string dialogText, string roleImage, string characterId)
        {
            VNSceneInfo info = VNSceneInfo.CreateDefault();
            info.DialogInfo = new VNDialogueInfo()
            {
                RoleName = roleName,
                DialogueText = dialogText
            };
            if (!string.IsNullOrWhiteSpace(roleImage))
            {
                var t = new string[4];
                var r = roleImage.Split('-');
                for (int i = 0; i < r.Length; i++)
                {
                    t[i] = r[i];
                }
                info.CharacterInfos.Add(new VNCharacterInfo()
                {
                    Status = Status.Shown,
                    Id = t[0],
                    Clothing = t[1],
                    Expression = t[2],
                    Decoration = t[3],
                    CharacterTag = characterId
                });
            }
            else
            {
                info.CharacterInfos.Add(new VNCharacterInfo()
                {
                    Status = Status.Hidden,
                    HiddenAnimation = CharacterHiddenAnimation.Fade
                });
            }
            self.SceneController.LoadVNSceneInfo(info);
        }
        public static void HideDialog(this VNCommandCenter self)
        {
            VNSceneInfo info = VNSceneInfo.CreateDefault();
            info.DialogInfo = new VNDialogueInfo() { Status = Status.Hidden };
            info.CharacterInfos.Add(new VNCharacterInfo()
            {
                Status = Status.Hidden,
                CharacterTag = null,
                HiddenAnimation = CharacterHiddenAnimation.Fade
            });
            self.SceneController.LoadVNSceneInfo(info);
        }
        public static void SkipDialogTextAnimation(this VNCommandCenter self)
        {
            self.SceneController.FrontLayerController.SkipCurrentTranscation();
        }
    }
}