using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public abstract class UserInfoProvider: ScriptableObject, IProvider<UserInfo>
    {
        public abstract void Save(UserInfo value, int id);
        public abstract UserInfo Load(int id);
        public abstract IEnumerable<UserInfo> LoadAll();
    }
}