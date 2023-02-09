# nullable enable
using LWVNFramework.Infos;
using System.Collections.Generic;

namespace LWVNFramework.Controllers
{
    /// <summary>
    /// 中景层控制器接口
    /// </summary>
    public abstract class IVNCenterLayerController : LwvnElement, IVNLayerController
    {
        public abstract bool Fastforward { get; set; }
        /// <summary>
        /// 当前过渡（动画）是否完成
        /// </summary>
        public abstract bool IsTransitionCompleted { get; }
        /// <summary>
        /// 默认的人物Tag，当人物信息未指定应用的人物Tag时使用该Tag
        /// </summary>
        public abstract string DefaultCharacterTag { get; }
        /// <summary>
        /// 当前人物信息
        /// </summary>
        public abstract IEnumerable<VNCharacterInfo> VNCharacterInfos { get; }
        /// <summary>
        /// 当前启用的物品信息
        /// </summary>
        public abstract IEnumerable<VNInGameItemInfo> CreatedItemsInfo { get; }

        public abstract void ResetLayer();
        /// <summary>
        /// 载入角色信息
        /// </summary>
        /// <param name="infos"></param>
        public abstract void LoadCharacterInfos(IEnumerable<VNCharacterInfo> infos);
        /// <summary>
        /// 载入游戏内物品信息
        /// </summary>
        /// <param name="infos"></param>
        public abstract void LoadInGameItemInfos(IEnumerable<VNInGameItemInfo> infos);
        /// <summary>
        /// 跳过当前过渡（动画）
        /// </summary>
        public abstract void SkipCurrentTransition();
    }
}
