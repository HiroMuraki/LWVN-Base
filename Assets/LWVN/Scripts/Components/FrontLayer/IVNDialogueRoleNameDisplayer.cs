# nullable enable
using LWVNFramework.Infos;
using LWVNFramework.ResourcesProvider;

namespace LWVNFramework.Components
{
    /// <summary>
    /// 角色名显示器接口
    /// </summary>
    public abstract class IVNDialogueRoleNameDisplayer : LwvnElement, IStatusResetable
    {
        public abstract void ResetStatus();
        /// <summary>
        /// 显示角色名
        /// </summary>
        /// <param name="info"></param>
        /// <param name="roleNameRes"></param>
        public abstract void Display(VNDialogueInfo info, RoleNameColorRes roleNameRes);
    }
}
