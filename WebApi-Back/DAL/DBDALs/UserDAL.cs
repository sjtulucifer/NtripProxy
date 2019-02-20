using NtripProxy.DAL.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Security.Cryptography;

namespace NtripProxy.DAL.DBDALs
{
    /// <summary>
    /// 用户数据库操作类
    /// </summary>
    public class UserDAL
    {
        /// <summary>
        /// 添加单个用户
        /// </summary>
        /// <param name="user">需要添加的用户信息</param>
        /// <returns>用户是否添加成功</returns>
        public bool AddUser(USER user)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                ctx.USERs.Add(user);
                //用户密码进行加密操作
                user.User_Password = MD5Encrypt(user.User_Password);
                result = ctx.SaveChanges() == 1;
            }
            return result;
        }

        /// <summary>
        /// 通过ID号逻辑删除用户
        /// </summary>
        /// <param name="id">用户ID号</param>
        /// <returns>是否删除成功</returns>
        public bool SoftDeleteUserByID(Guid id)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                var user = ctx.USERs.FirstOrDefault<USER>(u => u.ID == id);
                user.isDelete = true;
                result = ctx.SaveChanges() >= 1;
            }
            return result;
        }

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool ResetUserPassword(USER user)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                if (user.ID != null)
                {
                    user.User_Password = MD5Encrypt(user.User_Password);
                    ctx.USERs.Attach(user);
                    ctx.Entry(user).State = EntityState.Unchanged;
                    //更改密码信息
                    ctx.Entry(user).Property("User_Password").IsModified = true;
                    result = ctx.SaveChanges() >= 1;
                }
            }
            return result;
        }

        /// <summary>
        /// 更新单个用户基础资料，不更改密码信息
        /// </summary>
        /// <param name="user">需要更新的用户</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateUser(USER user)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                if (user.ID != null)
                {
                    //User_Password 必须有值,不然无法跳过框架检查，所以设置为password值
                    user.User_Password = "password";
                    ctx.USERs.Attach(user);
                    ctx.Entry(user).State = EntityState.Modified;
                    //不更改密码信息
                    ctx.Entry(user).Property("User_Password").IsModified = false;
                    result = ctx.SaveChanges() >= 1;
                }
            }
            return result;
        }

        /// <summary>
        /// 更新用户相关公司
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="company">相关公司</param>
        /// <returns>更新是否成功</returns>
        public bool UpdateUserCompany(USER user, COMPANY company)
        {
            bool result = false;
            using (var ctx = new NtripProxyDB())
            {
                if (user.ID != null && company.ID != null)
                {
                    //User_Password 必须有值,不然无法跳过框架检查，所以设置为password值
                    user.User_Password = "password";
                    user.User_Campany = company.ID;
                    ctx.USERs.Attach(user);
                    ctx.Entry(user).Property("User_Campany").IsModified = true;
                    ctx.Entry(user).Property("User_Password").IsModified = false;
                    result = ctx.SaveChanges() >= 1;
                }
            }
            return result;
        }

        /// <summary>
        /// 更新用户相关角色
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <param name="roles">角色列表</param>
        /// <returns>更新角色数量</returns>
        public int UpdateUserRoles(USER user, List<ROLE> roles)
        {
            int result = 0;
            using (var ctx = new NtripProxyDB())
            {
                var updateItem = ctx.USERs.Include("ROLEs").FirstOrDefault<USER>(u => u.ID == user.ID);
                updateItem.ROLEs.Clear();
                if (roles != null && roles.Count > 0)
                {
                    foreach (var role in roles)
                    {
                        updateItem.ROLEs.Add(ctx.ROLEs.Find(role.ID));
                    }
                    result = ctx.SaveChanges();
                }
                else
                {
                    result = ctx.SaveChanges();
                }
            }
            return result;
        }

        /// <summary>
        /// 查找所有用户基本信息
        /// </summary>
        /// <returns>所有用户列表</returns>
        public List<USER> FindAllUsers()
        {
            List<USER> result = new List<USER>();
            using (var ctx = new NtripProxyDB())
            {
                result = ctx.USERs.Where<USER>(u => u.isDelete == false).OrderByDescending(u=>u.createTime).ToList<USER>();
            }
            return result;
        }

        /// <summary>
        /// 通过用户ID查找用户全部信息
        /// </summary>
        /// <param name="id">用户ID号</param>
        /// <returns>查找到的用户信息</returns>
        public USER FindUserByID(Guid id)
        {
            USER user = new USER();
            using (var ctx = new NtripProxyDB())
            {
                user = ctx.USERs.Include("COMPANY").Include("ACCOUNTs").Include("ROLEs").FirstOrDefault(u => u.ID == id && u.isDelete == false);
            }
            return user;
        }

        /// <summary>
        /// 通过用户登陆名查找登陆用户所需信息
        /// </summary>
        /// <param name="name">用户登陆名</param>
        /// <returns>查找到的用户信息</returns>
        public USER FindLoginUserByLoginName(string name)
        {
            USER user = new USER();
            using (var ctx = new NtripProxyDB())
            {
                user = ctx.USERs.Include("COMPANY").Include("ACCOUNTs").Include("ROLEs").FirstOrDefault(u => u.User_Name == name && u.isDelete == false);
                List<ROLE> roles = user.ROLEs.ToList<ROLE>();
                user.ROLEs.Clear();
                foreach (var role in roles)
                {
                    ROLE fullRole = ctx.ROLEs.Include("PERMISSIONs").FirstOrDefault(r => r.ID == role.ID && r.isDelete == false);
                    List<PERMISSION> permissions = fullRole.PERMISSIONs.ToList<PERMISSION>();
                    role.PERMISSIONs.Clear();
                    foreach (var permission in permissions)
                    {
                        PERMISSION fullPermission = ctx.PERMISSIONs.Include("MENUs").FirstOrDefault(p => p.ID == permission.ID && p.isDelete == false);
                        role.PERMISSIONs.Add(fullPermission);
                    }
                    user.ROLEs.Add(fullRole);
                }
            }
            return user;
        }

        /// <summary>
        /// 通过用户登陆名查找用户全部信息
        /// </summary>
        /// <param name="loginName">用户登陆名</param>
        /// <returns>查找到的用户信息</returns>
        public USER FindUserByLoginName(string loginName)
        {
            USER user = new USER();
            using (var ctx = new NtripProxyDB())
            {
                user = ctx.USERs.Include("COMPANY").Include("ACCOUNTs").Include("ROLEs").FirstOrDefault(u => u.User_Login == loginName && u.isDelete == false);
                //去除逻辑删除的角色
                user.ROLEs = user.ROLEs.Where(r => r.isDelete == false).ToList<ROLE>();
                foreach (var role in user.ROLEs)
                {
                    ctx.Entry(role).Collection("PERMISSIONs");
                    //去除逻辑删除的权限
                    role.PERMISSIONs = role.PERMISSIONs.Where(p => p.isDelete == false).ToList<PERMISSION>();
                    foreach (var permission in role.PERMISSIONs)
                    {
                        ctx.Entry(permission).Collection("MENUs");
                        //去除逻辑删除的目录
                        permission.MENUs = permission.MENUs.Where(m => m.isDelete == false).ToList<MENU>();
                    }
                }
            }
            return user;
        }

        /// <summary>
        /// 检查用户是否可登陆
        /// </summary>
        /// <param name="user">检查的用户</param>
        /// <returns>是否可登陆</returns>
        public bool LoginCheck(USER user)
        {
            bool allowLogin = false;
            USER temp = this.FindUserByLoginName(user.User_Login);
            if (temp.User_Password == this.MD5Encrypt(user.User_Password))
            {
                allowLogin = true;
            }
            return allowLogin;
        }

        /// <summary>
        /// 用MD5加密字符串
        /// </summary>
        /// <param name="password">待加密的字符串</param>
        /// <returns></returns>
        private string MD5Encrypt(string password)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            hashedDataBytes = md5Hasher.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(password));
            StringBuilder tmp = new StringBuilder();
            foreach (byte i in hashedDataBytes)
            {
                tmp.Append(i.ToString("x2"));
            }
            return tmp.ToString();
        }
    }
}
